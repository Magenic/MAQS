//--------------------------------------------------
// <copyright file="MongoDBManagerTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test class for the MongoDB driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseMongoTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Http;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Test basic mongo base test functionality
    /// </summary>
    [TestClass]
    public class MongoDBManagerTests : BaseMongoTest<BsonDocument>
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void CanOverrideDatabaseDriver()
        {
            MongoDBDriver<BsonDocument> tempDriver = new MongoDBDriver<BsonDocument>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetConnectionString());
            this.MongoDBDriver = tempDriver;
            Assert.AreEqual(this.TestObject.MongoDBManager.Get(), tempDriver);
        }

        /// <summary>
        /// Make sure the test object driver is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void MongoDBDriverInDriverStore()
        {
            Assert.AreEqual(this.TestObject.MongoDBDriver, this.TestObject.GetDriverManager<MongoDriverManager<BsonDocument>>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriverManager(new WebServiceDriverManager(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriverManager<MongoDriverManager<BsonDocument>>(), "Expected a database driver store");
            Assert.IsNotNull(this.TestObject.GetDriverManager<WebServiceDriverManager>(), "Expected a web service driver store");
        }


        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void MongoCanUseMultiple()
        {
            MongoDriverManager<BsonDocument> mongoDriverManager = new MongoDriverManager<BsonDocument>(() => MongoDBDriver.Client, this.TestObject);
            ManagerStore.Add("Test", mongoDriverManager);

            Assert.AreNotEqual(this.TestObject.MongoDBManager, (MongoDriverManager<BsonDocument>)this.ManagerStore["Test"]);
            Assert.AreNotEqual(this.TestObject.MongoDBManager.ToBsonDocument(), (MongoDriverManager<BsonDocument>)this.ManagerStore["Test"]);
        }
    }
}
