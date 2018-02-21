using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FeatureFlags.Library
{
    public class Feature
    {
        /// <summary>
        /// The number of buckets to use when calculating percentage
        /// logged in and percentage variants
        /// </summary>
        private const int NUMBER_OF_BUCKETS = 1000;

        /// <summary>
        /// Gets a 'bucket' for an item (using GetHashCode) and number of buckets.  
        /// Idea taken shamelessly from 
        /// https://ericlippert.com/2011/02/28/guidelines-and-rules-for-gethashcode/
        /// </summary>
        /// <param name="item">The item to get a bucket for</param>
        /// <param name="numberOfBuckets">The number of buckets to use when 'bucketing'</param>
        /// <returns></returns>
        public static int GetBucket(string item, int numberOfBuckets = 1000)
        {
            int retval = 0;

            try
            {
                //  As long as we don't have null or empty ... 
                //  (I'm not sure why this would ever happen)
                if (!string.IsNullOrWhiteSpace(item))
                {
                    unchecked
                    {
                        // A hash code can be negative, and thus its remainder can be negative also.
                        // Do the math in unsigned ints to be sure we stay positive.
                        retval = (int)((uint)item.GetHashCode() % (uint)numberOfBuckets);
                    }
                }
            }
            catch (Exception)
            { /* Don't do anything */}

            return retval;
        }
        
        /// <summary>
        /// Given a feature flag and some parameters, check
        /// to see if the feature flag is enabled
        /// </summary>
        /// <param name="rule">The feature flag ruleset</param>
        /// <param name="user">The user to check</param>
        /// <param name="group">The group to check</param>
        /// <param name="url">The url part to check</param>
        /// <param name="IsInternal">'true' if the request is internal</param>
        /// <param name="isAdmin">'true' if the request is for an admin</param>
        /// <returns></returns>
        public static bool IsEnabled(FeatureFlag rule, string user = "", string group = "", string url = "", bool IsInternal = false, bool isAdmin = false)
        {
            //  By default, the rule is disabled...
            bool retval = false;

            //  First see if the feature is hard enabled/disabled
            if (rule.Enabled.HasValue)
            {
                //  If the rule indicates it's hard enabled/disabled, 
                //  set the retval and get out
                retval = rule.Enabled.Value;
            }
            else
            {
                //  If it's not hard disabled ...
                //  - see if we're in the list of users or groups that are enabled
                //  - see if the url contains a querystring 'feature' with the matching url flag
                //  - see if the rule is for internal users (if we're internal)
                //  - see if the rule is for admin users (if we're an admin)
                //  - see if we should be percentage enabled based on our user name

                //  See if the user is in the list:
                if (rule.Users.Contains(user, StringComparer.OrdinalIgnoreCase))
                {
                    retval = true;
                }

                //  See if this group is in the list:
                if (rule.Groups.Contains(group, StringComparer.OrdinalIgnoreCase))
                {
                    retval = true;
                }

                //  See if the rule url isn't blank and the passed url is a match:
                if ((!string.IsNullOrWhiteSpace(rule.Url)) && rule.Url.Trim() == url.Trim())
                {
                    retval = true;
                }

                //  See if we're internal and the flag is for internal users:
                if (IsInternal && rule.Internal)
                {
                    retval = true;
                }

                //  See if we're an admin and the flag is for admin users:
                if (isAdmin && rule.Admin)
                {
                    retval = true;
                }
                
                //  If it's percentage enabled, see if we're in a picked
                //  bucket based on our user name and the percentage that
                //  should be enabled
                if (rule.PercentLoggedIn > 0)
                {
                    //  Get the bucket for the user
                    int userBucket = GetBucket(user, NUMBER_OF_BUCKETS);

                    //  Calculate the scaled percent for the bucket
                    int scaledPercent = (int)((userBucket / (float)NUMBER_OF_BUCKETS) * 100);

                    //  Compare the bucket's percent vs 
                    //  the percent that should get the feature
                    if (scaledPercent < rule.PercentLoggedIn)
                        retval = true;
                }

            }

            return retval;
        }

        /// <summary>
        /// Given a feature flag and a user, see what variant should be enabled 
        /// (or "None" if no variant is enabled)
        /// </summary>
        /// <param name="rule">The feature flag ruleset</param>
        /// <param name="user">The user to check</param>
        /// <returns>The variant name, or None if bucket doesn't fall into
        /// any of the variants</returns>
        public static string GetVariantFor(FeatureFlag rule, string user = "")
        {            
            //  Our default return value
            string retval = "None";

            //  If we have variants specified, determine 
            //  which variant (if any) should be selected
            if (rule.Variants.Any())
            {
                //  Get the bucket for the user
                int userBucket = GetBucket(user, NUMBER_OF_BUCKETS);

                //  We want to always include two control groups, but allow overriding of
                //  their percentages.
                Dictionary<string, double> all_variants = new Dictionary<string, double>()
                    {
                        { "control_1", 10},
                        { "control_2", 10},
                    };

                //  Add the rule variants to the base variants (and allow them
                //  to overwrite the original base variants)
                rule.Variants.ForEach(x => all_variants[x.Name] = x.Percentage);

                int numVariants = all_variants.Count;
                var variantNames = all_variants.Keys.ToList();

                //  If the variants took up the entire set of buckets, which
                //  bucket would we be in?
                string candidate = variantNames[userBucket % numVariants];

                /* Log a warning if this variant is capped, to help us prevent user (us)
                 * error.  It's not the most correct to only check the one, but it's
                 * easy and quick, and anything with that high a percentage should be
                 * selected quite often.
                */
                var variant_fraction = all_variants[candidate] / 100.0;
                var variant_cap = 1.0 / numVariants;
                if (variant_fraction > variant_cap)
                {
                    //  Log a warning
                    Trace.TraceWarning("Variant {0} exceeds allowable percentage ({1:0.0%} > {2:0.0%})", candidate, variant_fraction, variant_cap);                    
                }

                /* Variant percentages are expressed as numeric percentages rather than
                 * a fraction of 1 (that is, 1.5 means 1.5%, not 150%); thus, at 100
                 * buckets, buckets and percents map 1:1 with each other.  Since we may
                 * have more than 100 buckets (causing each bucket to represent less
                 * than 1% each), we need to scale up how far "right" we move for each
                 * variant percent. 
                */
                var bucket_multiplier = NUMBER_OF_BUCKETS / 100;
                if (userBucket < (all_variants[candidate] * numVariants * bucket_multiplier))
                {
                    retval = candidate;
                }
                else
                {
                    retval = "None";
                }
            }

            return retval;
        }
    }
}
