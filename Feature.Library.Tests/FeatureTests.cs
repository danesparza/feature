using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Feature.Library.Tests
{
    [TestClass]
    public class FeatureTests
    {        
        [TestMethod]
        public void IsEnabledFor_ValidFlagRulesAndInfo_Correct()
        {
            //  Arrange
            string testUser = "iserra"; /* Note that iserra will return a percentage of 13% (given 1000 buckets) */
            string testGroup = "Browncoats";
            string testUrl = "lassiter";
            bool testAdmin = true;
            bool testInternal = true;

            Dictionary<FeatureFlag, bool> testRules = new Dictionary<FeatureFlag, bool>()
            {
                {new FeatureFlag{ Enabled = true }, true },
                {new FeatureFlag{ Enabled = false }, false},
                {new FeatureFlag{ Admin = true}, true},
                {new FeatureFlag{ Internal = true}, true},
                {new FeatureFlag{ Users = new List<string>{ "iserra", "MReynolds"} }, true},
                {new FeatureFlag{ Users = new List<string>{"sometestguy", "someothertestguy"} }, false},
                {new FeatureFlag{ Groups = new List<string>{"federation", "someothergroup"} }, false},
                {new FeatureFlag{ Groups = new List<string>{"travelswithjayne", "browncoats" } }, true},
                {new FeatureFlag{ PercentLoggedIn = 15 }, true}, /* iserra is in a bucket that is included */
                {new FeatureFlag{ PercentLoggedIn = 5 }, false}, /* iserra is not in a bucket that is included */
                {new FeatureFlag{ Users = new List<string>{ "iserra", "MReynolds"}, Groups = new List<string>{"federation", "someothergroup"} }, true}, /* iserra is still a user that is included */
                {new FeatureFlag{ Enabled = true, Users = new List<string>{ "sometestguy", "someothertestguy"}, Groups = new List<string>{"federation", "someothergroup"} }, true}, /* if we enable it, it's enabled for everybody */
                {new FeatureFlag{ Enabled = false, Users = new List<string>{ "iserra"}, Groups = new List<string>{"Browncoats"} }, false}, /* if we disable it, it's disabled for everybody */
                {new FeatureFlag{ Url = "lassiter", Users = new List<string>{ "someothertestguy"}, Groups = new List<string>{"Federation"} }, true}, /* If we're at the magic url, it's enabled */
                {new FeatureFlag{ Url = "jaynestown", Users = new List<string>{ "someothertestguy"}, Groups = new List<string>{"Federation"} }, false}, /* If we're NOT at the magic url, it's not necessary enabled */
            };

            //  For each item in the test table...
            foreach (var item in testRules)
            {
                //  Act
                var retval = Feature.IsEnabled(item.Key, testUser, testGroup, testUrl, testInternal, testAdmin);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }
        }

        [TestMethod]
        public void IsEnabled_TestUser_ReturnsAsExpected()
        {
            //  Arrange
            string testUser = "testuser"; 
            string testGroup = "testgroup";
            string testUrl = "";
            bool testAdmin = false;
            bool testInternal = false;

            Dictionary<FeatureFlag, bool> testRules = new Dictionary<FeatureFlag, bool>()
            {
                {new FeatureFlag{ Users = new List<string>{ "someotheruser", "anotheruser"}, Groups = new List<string>{ "someothergroup"}, Admin = true, Internal = true, Url = "" }, false},                 
            };

            //  For each item in the test table...
            foreach (var item in testRules)
            {
                //  Act
                var retval = Feature.IsEnabled(item.Key, testUser, testGroup, testUrl, testInternal, testAdmin);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }
        }

        [TestMethod]
        public void GetBucket_ValidItem_ReturnsBucketNumber()
        {
            //  Arrange
            Dictionary<string, int> bucketTests = new Dictionary<string, int>()
            {
                {"mreynolds", 787},
                {"zwashburne", 987},
                {"wwashburne", 746},
                {"iserra", 136}, 
                {"jcobb", 33},
                {"kfrye", 912},
                {"stam", 146},
                {"rtam", 147},
                {"dbook", 466},
                {"", 0},
            };

            //  For each item in the test table...
            foreach (var item in bucketTests)
            {
                //  Act
                var retval = Feature.GetBucket(item.Key);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }
        }

        [TestMethod]
        public void GetBucket_NullItem_ReturnsBucketNumber()
        {
            //  Arrange
            string itemName = null;
            int expectedBucket = 0;

            //  Act
            var retval = Feature.GetBucket(itemName);

            //  Assert
            Assert.AreEqual(expectedBucket, retval);
        }

        [TestMethod]
        public void GetVariantFor_ValidFlagRulesAndInfo_Correct()
        {
            //  Arrange
            FeatureFlag testFlag = new FeatureFlag()
            {
                Variants = new List<FlagVariant>{
                    new FlagVariant{ Name = "One", Percentage = 15 },
                    new FlagVariant{ Name = "Two", Percentage = 15 },
                    new FlagVariant{ Name = "Three", Percentage = 15 },
                }
            };

            //  Our test table of users and expected variants
            Dictionary<string, string> testUsers = new Dictionary<string, string>()
            {
                {"mreynolds", "None"},
                {"zwashburne", "None"},
                {"wwashburne", "None"},
                {"iserra", "control_2"},
                {"jcobb", "Two"},
                {"kfrye", "None"},
                {"stam", "control_2"},
                {"rtam", "One"},
                {"dbook", "control_2"},
                {"", "control_1"},
            };

            //  For each item in the test table...
            foreach (var item in testUsers)
            {
                //  Act
                var retval = Feature.GetVariantFor(testFlag, item.Key);

                //  Assert
                Assert.AreEqual(item.Value, retval);
            }
        }
    }
}
