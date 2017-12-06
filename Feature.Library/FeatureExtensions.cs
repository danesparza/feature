using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Feature.Library
{
    public static class FeatureExtensions
    {
        /// <summary>
        /// Parse a string (possibly JSON) into a FlagResult
        /// </summary>
        /// <param name="featureFlagConfig"></param>
        /// <returns></returns>
        public static FlagRule ParseFeatureFlag(this string featureFlagConfig)
        {
            FlagRule retval = new FlagRule();
            
            //  First, see if it's just directly turning the feature on or off:
            if (IsFeatureEnablingString(featureFlagConfig ?? ""))
            {
                retval = new FlagRule()
                {
                    Enabled = true
                };
            }
            else
            {
                //  Otherwise, assume it's JSON and that we need to parse...
                try
                {
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(featureFlagConfig ?? "")))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(FlagRule));
                        retval = (FlagRule)serializer.ReadObject(ms);

                        //  Make sure we have our properties initialized
                        retval.Users = retval.Users ?? new List<string>();
                        retval.Groups = retval.Groups ?? new List<string>();
                        retval.Variants = retval.Variants ?? new List<FlagVariant>();
                        retval.VariantName = retval.VariantName ?? string.Empty;
                        retval.Url = retval.Url ?? string.Empty;
                    }                    
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }            
            
            return retval;
        }

        /// <summary>
        /// Checks to see if the passed string roughly equates to turning the feature completely on
        /// </summary>
        /// <param name="testString"></param>
        /// <returns></returns>
        public static bool IsFeatureEnablingString(this string testString)
        {
            bool retval = false;

            //  Clean up the string to check it:
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            testString = rgx.Replace(testString ?? "", string.Empty);

            switch (testString)
            {
                case "enabled":
                case "enable":
                case "on":
                case "true":
                    retval = true;
                    break;
                default:
                    break;
            }

            return retval;
        }
    }
}
