using System;
using System.Text.RegularExpressions;

namespace Feature.Library
{
    public class FeatureManager
    {
        /// <summary>
        /// Parse a string (possibly JSON) into a FlagResult
        /// </summary>
        /// <param name="featureFlagConfig"></param>
        /// <returns></returns>
        public static FlagRule ParseFeatureFlag(string featureFlagConfig)
        {
            FlagRule retval = new FlagRule();

            //  First, see if it's just directly turning the feature on or off:
            if (IsEnabledString(featureFlagConfig))
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

                }
                catch (Exception)
                { /* Don't really need to do anything here... */ }
            }
            
            return retval;
        }

        /// <summary>
        /// Checks to see if the passed string roughly equates to turning the feature completely on
        /// </summary>
        /// <param name="testString"></param>
        /// <returns></returns>
        public static bool IsEnabledString(string testString)
        {
            bool retval = false;

            //  Clean up the string to check it:
            Regex rgx = new Regex("[^a-zA-Z0-9]");
            testString = rgx.Replace(testString, string.Empty);

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
