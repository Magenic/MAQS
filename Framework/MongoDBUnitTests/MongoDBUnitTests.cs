//--------------------------------------------------
// <copyright file="MongoDBUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database wrapper without base database test</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseMongoDBTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MongoDBUnitTests
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// reference used for the MongoDB interaction methods
        /// </summary>
        private MongoDBMethods mongoDBMethods;

        /// <summary>
        /// place holder of the MongoDB client
        /// </summary>
        private IMongoClient mongoClient;

        /// <summary>
        /// place holder of the MongoDB database
        /// </summary>
        private IMongoDatabase mongoDatabase;

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
        /// Setup the test parameters
        /// </summary>
        [TestInitialize]
        public void SetUpMongoConnection()
        {
            var mongoURL = new MongoUrl(ConfigurationManager.AppSettings["MongoConnectionString"]);
            this.mongoClient = new MongoClient(mongoURL);
            this.mongoDatabase = this.mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDatabase"]);
            this.mongoDBMethods = new MongoDBMethods(this.mongoDatabase, ConfigurationManager.AppSettings["MongoCollection"]);
        }

        /// <summary>
        /// Check that we get back the loginID table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void VerifyReadNoWrapper()
        {
            Assert.IsTrue(this.mongoDBMethods.GetLoginID("test1").Equals("test1"));

            Assert.IsTrue(this.mongoDBMethods.QueryMongo("lid", "test1")[0].LoginID.Equals("test1"));

            Assert.IsTrue(this.mongoDBMethods.QueryMongo("isChanged", true)[0].IsChanged.Equals(true));
        }

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
        public void VerifyWriteNoWrapper()
        {
            Assert.IsTrue(this.mongoDBMethods.GetLoginID("test1").Equals("test1"));
            this.mongoDBMethods.EditLoginID("test1", "test2");
            Assert.IsTrue(this.mongoDBMethods.GetLoginID("test2").Equals("test2"));
            this.mongoDBMethods.EditLoginID("test2", "test1");
            Assert.IsTrue(this.mongoDBMethods.GetLoginID("test1").Equals("test1"));
        }*/

        /// <summary>
        /// Test mongo function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void MongoTest()
        {
            this.mongoDBMethods.MongoTest();
            Debugger.Break();
        }
    }
}
