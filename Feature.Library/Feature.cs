using System;
using System.Linq;

namespace Feature.Library
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

            //  If we have null or empty ... 
            //  (I'm not sure why this would ever happen)
            //  return bucket 0
            if (string.IsNullOrWhiteSpace(item))
            {
                retval = 0;
            }
            else
            {
                //  Otherwise ... calculate the bucket it should be in
                unchecked
                {
                    // A hash code can be negative, and thus its remainder can be negative also.
                    // Do the math in unsigned ints to be sure we stay positive.
                    retval = (int)((uint)item.GetHashCode() % (uint)numberOfBuckets);
                }
            }

            return retval;
        }
        
        /// <summary>
        /// Given a flag ruleset and some parameters, check
        /// to see if the feature flag is enabled
        /// </summary>
        /// <param name="rule">The feature flag ruleset</param>
        /// <param name="user">The user to check</param>
        /// <param name="group">The group to check</param>
        /// <param name="url">The url part to check</param>
        /// <param name="IsInternal">'true' if the request is internal</param>
        /// <param name="isAdmin">'true' if the request is for an admin</param>
        /// <returns></returns>
        public static bool IsEnabledFor(FlagRule rule, string user = "", string group = "", string url = "", bool IsInternal = false, bool isAdmin = false)
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

                //  See if the passed url is a match:
                if (rule.Url.Trim() == url.Trim())
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

                //  Need to implment something like
                //  https://github.com/reddit/reddit/blob/40625dcc070155588d33754ef5b15712c254864b/r2/r2/config/feature/state.py#L130-L208
                //  To choose a variant

            }

            return retval;
        }
    }
}
