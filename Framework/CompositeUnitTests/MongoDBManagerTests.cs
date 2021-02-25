//--------------------------------------------------
// <copyright file="MongoDBManagerTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test class for the MongoDB driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseMongoTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Test basic mongo base test functionality
    /// </summary>
    [TestClass]
    public class MongoDBManagerTests : BaseMongoTest<BsonDocument>
    {
        /// <summary>
        /// Test the list all collection items helper function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoListAllCollectionItems()
        {
            List<BsonDocument> collectionItems = this.MongoDBDriver.ListAllCollectionItems();
            foreach (BsonDocument bson in collectionItems)
            {
                Assert.IsTrue(bson.Contains("lid"));
            }

            Assert.AreEqual(4, collectionItems.Count);
        }

        /// <summary>
        /// Test the count all collection items helper function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoCountItemsInCollection()
        {
            Assert.AreEqual(4, this.MongoDBDriver.CountAllItemsInCollection());
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoGetLoginID()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("lid", "test3");
            var value = this.MongoDBDriver.Collection.Find(filter).ToList()[0]["lid"].ToString();
            Assert.AreEqual("test3", value);
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoQueryAndReturnFirst()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("lid", "test3");
            BsonDocument document = this.MongoDBDriver.Collection.Find(filter).ToList().First();
            Assert.AreEqual("test3", document["lid"].ToString());
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoFindListWithKey()
        {
            var filter = Builders<BsonDocument>.Filter.Exists("lid");
            List<BsonDocument> documentList = this.MongoDBDriver.Collection.Find(filter).ToList();
            foreach (BsonDocument documents in documentList)
            {
                Assert.AreNotEqual(documents["lid"].ToString(), string.Empty);
            }

            Assert.AreEqual(4, documentList.Count);
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoLinqQuery()
        {
            IMongoQueryable<BsonDocument> query =
                from e in this.MongoDBDriver.Collection.AsQueryable<BsonDocument>()
                where e["lid"] == "test1"
                select e;
            List<BsonDocument> retList = query.ToList<BsonDocument>();
            foreach (var value in retList)
            {
                Assert.AreEqual("test1", value["lid"]);
            }
        }

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        [TestCategory(TestCategories.Utilities)]
        public void TestMongoDBTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.MongoDBDriver, this.MongoDBDriver, "Web service driver don't match");
        }

        /// <summary>
        /// Gets the connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseConnectionStringTest()
        {
            string connection = MongoDBConfig.GetConnectionString();
            Assert.AreEqual("mongodb://localhost:27017", connection);
        }

        /// <summary>
        /// Gets the database string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseStringTest()
        {
            string databaseString = MongoDBConfig.GetDatabaseString();
            Assert.AreEqual("MongoDatabaseTest", databaseString);
        }

        /// <summary>
        /// Gets the timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseQueryTimeout()
        {
            int databaseTimeout = MongoDBConfig.GetQueryTimeout();
            Assert.AreEqual(30, databaseTimeout);
        }

        /// <summary>
        /// Override with default driver
        /// </summary>
        [TestMethod]
        public void RespectDefaultDriverOverride()
        {
            var mongoDriver = new MongoDBDriver<BsonDocument>();
            this.TestObject.MongoDBManager.OverrideDriver(mongoDriver);

            Assert.AreEqual(mongoDriver.Collection, this.MongoDBDriver.Collection);
            Assert.AreEqual(mongoDriver.Collection.Database, this.MongoDBDriver.Database);
            Assert.AreEqual(mongoDriver.Collection.Database.Client, this.MongoDBDriver.Client);
        }

        /// <summary>
        /// Override driver with collection string
        /// </summary>
        [TestMethod]
        public void RespectCollectionDriverOverride()
        {
            //MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), collectionString
            var mongoDriver = new MongoDBDriver<BsonDocument>(MongoDBConfig.GetCollectionString());
            this.TestObject.MongoDBManager.OverrideDriver(mongoDriver);

            Assert.AreEqual(mongoDriver.Collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override drive with all 3 connection strings
        /// </summary>
        [TestMethod]
        public void RespectDriverConnectionsOverride()
        {
            var mongoDriver = new MongoDBDriver<BsonDocument>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString());
            this.TestObject.MongoDBManager.OverrideDriver(mongoDriver);

            Assert.AreEqual(mongoDriver.Collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override driver directly
        /// </summary>
        [TestMethod]
        public void RespectDirectDriverOverride()
        {
            var mongoDriver = new MongoDBDriver<BsonDocument>(MongoDBConfig.GetCollectionString());
            this.MongoDBDriver = mongoDriver;

            Assert.AreEqual(mongoDriver.Collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override driver with new driver
        /// </summary>
        [TestMethod]
        public void RespectNewDriverOverride()
        {
            var mongoDriver = new MongoDBDriver<BsonDocument>(MongoDBConfig.GetCollectionString());
            this.TestObject.OverrideMongoDBDriver(mongoDriver);

            Assert.AreEqual(mongoDriver.Collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override drive with collection function
        /// </summary>
        [TestMethod]
        public void RespectCollectionOverride()
        {
            var collection = MongoFactory.GetDefaultCollection<BsonDocument>();
            this.TestObject.OverrideMongoDBDriver(() => collection);

            Assert.AreEqual(collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override drive with all 3 connection strings
        /// </summary>
        [TestMethod]
        public void RespectDriverConnectionStingsOverride()
        {
            var collection = this.MongoDBDriver.Collection;
            this.TestObject.OverrideMongoDBDriver(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString());

            Assert.AreNotEqual(collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override in base with collection function
        /// </summary>
        [TestMethod]
        public void RespectCollectionOverrideInBase()
        {
            var collection = MongoFactory.GetDefaultCollection<BsonDocument>();
            this.OverrideConnectionDriver(() => collection);

            Assert.AreEqual(collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override in base with new driver
        /// </summary>
        [TestMethod]
        public void RespectDriverOverrideInBase()
        {
            var collection = MongoFactory.GetDefaultCollection<BsonDocument>();
            this.OverrideConnectionDriver(new MongoDBDriver<BsonDocument>(collection));

            Assert.AreEqual(collection, this.MongoDBDriver.Collection);
        }

        /// <summary>
        /// Override drive with strings in base
        /// </summary>
        [TestMethod]
        public void RespectConnectionStingsOverrideInBase()
        {
            var collection = this.MongoDBDriver.Collection;
            this.OverrideConnectionDriver(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString());

            Assert.AreNotEqual(collection, this.MongoDBDriver.Collection);
        }
    }
}
