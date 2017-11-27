using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Feature.Library.Tests
{
    [TestClass]
    public class FeatureManagerTests
    {
        /// <summary>
        /// Test table of test strings and expected values
        /// </summary>
        Dictionary<string, bool> testStrings = new Dictionary<string, bool>()
        {
            {"", false},
            {"enabled", true},            
            {"enable", true},
            {"on", true},
            {"off", false},
            {"disabled", false},
            {"pretty much anything else", false},
        };

        [TestMethod]
        public void IsEnabledString_ValidTestString_ReturnsCorrectly()
        {
            //  Arrange
            foreach (var item in testStrings)
            {
                //  Act
                var retval = FeatureManager.IsEnabledString(item.Key);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }            
        }
    }
}
