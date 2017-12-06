using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Feature.Library.Tests
{
    [TestClass]
    public class FeatureExtensionsTests
    {     
        [TestMethod]
        public void IsFeatureEnablingString_ValidTestString_ReturnsCorrectly()
        {
            //  Arrange
            Dictionary<string, bool> testEnableStrings = new Dictionary<string, bool>()
            {
                {"", false},
                {"enabled", true},
                {"enable", true},
                {"on", true},

                /* JSON objects */
                {"{true}", true },
                {"{enabled}", true },
                {"{\"true\"}", true },

                /* Everything else*/
                {"off", false},
                {"disabled", false},
                {"pretty much anything else", false},
            };

            //  For each item in the test table ... 
            foreach (var item in testEnableStrings)
            {
                //  Act
                var retval = item.Key.IsFeatureCompletelyEnabled();

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }            
        }

        [TestMethod]
        public void ToFeatureFlag_ValidRuleString_ParsesCorrectly()
        {
            //  Arrange
            Dictionary<string, FeatureFlag> testJSONRules = new Dictionary<string, FeatureFlag>()
            {
                {"{true}", new FeatureFlag{ Enabled = true } },
                {"{\"enabled\": true}", new FeatureFlag{ Enabled = true } },
                {"{\"percent_loggedin\": 5, \"variant_name\": \"testing\"}", new FeatureFlag{ PercentLoggedIn = 5, VariantName = "testing", Enabled = null } },
            };

            //  For each item in the test table...
            foreach (var item in testJSONRules)
            {
                //  Act
                var retval = item.Key.ToFeatureFlag();

                //  Assert
                Check.That(retval).HasFieldsWithSameValues(item.Value);
            }
        }

        [TestMethod]
        public void ParseFeatureFlag_NullRuleString_ParsesCorrectly()
        {
            //  Arrange
            string jsonString = null;
            FeatureFlag expectedRule = new FeatureFlag();

            //  Act
            var retval = jsonString.ToFeatureFlag();

            //  Assert
            Check.That(retval).HasFieldsWithSameValues(expectedRule);
        }

        [TestMethod]
        public void ToJSON_ValidFeatureFlag_SerializesCorrectly()
        {
            //  Arrange
            Dictionary<string, FeatureFlag> testJSONRules = new Dictionary<string, FeatureFlag>()
            {   
                /* Note the sorting and spacing in the JSON strings... */
                {"{\"enabled\":true}", new FeatureFlag{ Enabled = true } },
                {"{\"admin\":true,\"internal\":true}", new FeatureFlag{ Internal = true, Admin = true } },
                {"{\"users\":[\"testuser\"]}", new FeatureFlag{ Users = new List<string>{ "testuser"} } },
            };

            //  For each item in the test table...
            foreach (var item in testJSONRules)
            {
                //  Act
                var retval = item.Value.ToJSON();

                //  Assert
                Assert.AreEqual(item.Key, retval);
            }
        }
    }
}
