using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using NFluent;

namespace Feature.Library.Tests
{
    [TestClass]
    public class FeatureManagerTests
    {
        /// <summary>
        /// Test table of test strings and expected values
        /// </summary>
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

        Dictionary<string, FlagRule> testJSONRules = new Dictionary<string, FlagRule>()
        {
            {@"{true}", new FlagRule{ Enabled = true } },
        };

        [TestMethod]
        public void IsEnabledString_ValidTestString_ReturnsCorrectly()
        {
            //  Arrange
            foreach (var item in testEnableStrings)
            {
                //  Act
                var retval = FeatureManager.IsEnabledString(item.Key);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }            
        }

        [TestMethod]
        public void ParseFeatureFlag_ValidRule_ParsesCorrectly()
        {
            //  Arrange
            foreach (var item in testJSONRules)
            {
                //  Act
                var retval = FeatureManager.ParseFeatureFlag(item.Key);

                //  Assert
                Check.That(retval).HasFieldsWithSameValues(item.Value);
            }
        }
    }
}
