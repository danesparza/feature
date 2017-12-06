using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Collections.Generic;

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
        public void ParseFeatureFlag_ValidRuleString_ParsesCorrectly()
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
    }
}
