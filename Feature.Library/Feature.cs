using System;
using System.Linq;

namespace Feature.Library
{
    public class Feature
    {
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
        public static bool CheckIsEnabledFor(FlagRule rule, string user = "", string group = "", string url = "", bool IsInternal = false, bool isAdmin = false)
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
            }            

            return retval;
        }
    }
}
