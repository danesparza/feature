using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using System.Collections.Generic;

namespace Feature.Library.Tests
{
    [TestClass]
    public class FeatureManagerTests
    {     
        [TestMethod]
        public void IsEnabledString_ValidTestString_ReturnsCorrectly()
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
                var retval = FeatureManager.IsEnablingString(item.Key);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }            
        }

        [TestMethod]
        public void ParseFeatureFlag_ValidRuleString_ParsesCorrectly()
        {
            //  Arrange
            Dictionary<string, FlagRule> testJSONRules = new Dictionary<string, FlagRule>()
            {
                {"{true}", new FlagRule{ Enabled = true } },
                {"{\"enabled\": true}", new FlagRule{ Enabled = true } },
                {"{\"percent_loggedin\": 5, \"variant_name\": \"testing\"}", new FlagRule{ PercentLoggedIn = 5, VariantName = "testing", Enabled = null } },
            };

            //  For each item in the test table...
            foreach (var item in testJSONRules)
            {
                //  Act
                var retval = FeatureManager.ParseFeatureFlag(item.Key);

                //  Assert
                Check.That(retval).HasFieldsWithSameValues(item.Value);
            }
        }

        [TestMethod]
        public void ParseFeatureFlag_NullRuleString_ParsesCorrectly()
        {
            //  Arrange
            string jsonString = null;
            FlagRule expectedRule = new FlagRule();

            //  Act
            var retval = FeatureManager.ParseFeatureFlag(jsonString);

            //  Assert
            Check.That(retval).HasFieldsWithSameValues(expectedRule);
        }

        
    }
}
