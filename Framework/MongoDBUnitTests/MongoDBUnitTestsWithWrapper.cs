//--------------------------------------------------
// <copyright file="MongoDBUnitTestsWithWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseMongoDBTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MongoDBUnitTestsWithWrapper : Magenic.MaqsFramework.BaseMongoDBTest.BaseMongoDBTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// for use of mongoDB collection interaction methods
        /// </summary>
        private MongoDBMethods mongoDBMethods;

        /// <summary>
        /// connects to the wrapper for MongoDB
        /// </summary>
        //// MongoDBConnectionWrapper connectionWrapper;

        /// <summary>
        /// Setup before we start running selenium tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running selenium tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Setup connection before each test
        /// </summary>
        [TestInitialize]
        public void SetUpMongoConnection()
        {
            /*this.connectionWrapper = new MongoDBConnectionWrapper(ConfigurationManager.AppSettings["MongoConnectionString"],
                ConfigurationManager.AppSettings["MongoDatabase"]);
            this.mongoDBMethods = new MongoDBMethods(this.connectionWrapper.ReturnMongoDBDatabase(), 
                ConfigurationManager.AppSettings["MongoCollection"]);*/

            this.mongoDBMethods = new MongoDBMethods(
                new MongoDBConnectionWrapper(
                    ConfigurationManager.AppSettings["MongoConnectionString"],
                    ConfigurationManager.AppSettings["MongoDatabase"]).ReturnMongoDBDatabase(),
                    ConfigurationManager.AppSettings["MongoCollection"]);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        #region QueryAndGetTableData
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void VerifyTableExists()
        {
            Assert.IsTrue(this.mongoDBMethods.IsCollectionEmpty());
        }
        #endregion

        /// <summary>
        /// Check if we get the expect number of results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void VerifyCollectionHasCorrectNumberOfRecords()
        {
            Assert.IsTrue(
                this.mongoDBMethods.CountItemsInCollection("test1").Equals(1), 
                "Count should equal: " + this.mongoDBMethods.CountItemsInCollection("test1"));
        }

        /// <summary>
        /// Check if Procedures actions can update an item
        /// </summary>
        /*[TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void VerifyWriteWithWrapper()
        {
            Assert.IsTrue(
                this.mongoDBMethods.GetLoginID("test1").Equals("test1"));
                this.mongoDBMethods.EditLoginID("test1", "test2");
            Assert.IsTrue(
                this.mongoDBMethods.GetLoginID("test2").Equals("test2"));
                this.mongoDBMethods.EditLoginID("test2", "test1");
            Assert.IsTrue(
                this.mongoDBMethods.GetLoginID("test1").Equals("test1"));
        }*/

        /// <summary>
        /// Check that we get back the loginID table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void VerifyReadWithWrapper()
        {
            Assert.IsTrue(this.mongoDBMethods.GetLoginID("test1").Equals("test1"));

            Assert.IsTrue(this.mongoDBMethods.QueryMongo("lid", "test1")[0].LoginID.Equals("test1"));
            Assert.IsTrue(this.mongoDBMethods.QueryMongo("isChanged", true)[0].IsChanged.Equals(true));
            Assert.IsTrue(this.mongoDBMethods.QueryMongo("order", 1)[0].Order.Equals(1));

            var variable = this.mongoDBMethods.QueryMongo("lid", "test1")[0].Order;
            Assert.IsTrue(this.mongoDBMethods.QueryMongo("lid", "test1")[0].Order.Equals(1));
        }

        ////[TestMethod]
        ////[TestCategory(TestCategories.MongoDB)]
        ////public void VerifyReadNoWrapper2(string serachHeader, string searchCategory)
        ////{  
        ////    var query1 = mongoDBMethods.QueryMongo2("lid", "test1")[0]["isChanged"];
        ////    var query2 = query1.GetType();
        ////    Assert.IsTrue(mongoDBMethods.QueryMongo2("lid", "test1")[0]["lid"].ToString().Equals("test1"));
        ////    Assert.IsTrue(mongoDBMethods.QueryMongo2("isChanged", true)[0]["isChanged"].AsBoolean.Equals(true));

        ////    var variable = mongoDBMethods.QueryMongo2("lid", "test1")[0]["order"];
        ////    Assert.IsTrue(mongoDBMethods.QueryMongo2("lid", "test1")[0]["order"].ToInt32().Equals(1));
        ////}

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        [TestCategory(TestCategories.Utilities)]
        public void DatabaseTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.MongoDBWrapper, this.MongoDBWrapper, "Web service wrapper don't match");
        }
    }
}
