//--------------------------------------------------
// <copyright file="MongoDBTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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
    public class MongoDBTests : BaseMongoTest<BsonDocument>
    {

        /// <summary>
        /// Is the collection override respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void OverrideCollectionFunction()
        {
            var collection = this.MongoDBDriver.Collection;
            var newCollection = MongoFactory.GetDefaultCollection<BsonDocument>();
            this.TestObject.OverrideMongoDBDriver(() => newCollection);

            Assert.AreNotEqual(collection, this.MongoDBDriver.Collection);
            Assert.AreEqual(newCollection, this.MongoDBDriver.Collection);
            Assert.IsFalse(this.MongoDBDriver.IsCollectionEmpty());
        }

        /// <summary>
        /// Are the connection string overrides respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void OverrideConnectionStrings()
        {
            var collection = this.MongoDBDriver.Collection;
            this.TestObject.OverrideMongoDBDriver(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString());

            Assert.AreNotEqual(collection, this.MongoDBDriver.Collection);
            Assert.IsFalse(this.MongoDBDriver.IsCollectionEmpty());
        }

        /// <summary>
        /// Is the driver override respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void OverrideDriver()
        {
            var firstDriver = this.MongoDBDriver;
            var newDriver = new MongoDBDriver<BsonDocument>(MongoFactory.GetDefaultCollection<BsonDocument>());
            this.TestObject.OverrideMongoDBDriver(newDriver);

            Assert.AreNotEqual(firstDriver, this.MongoDBDriver);
            Assert.AreEqual(newDriver, this.MongoDBDriver);
            Assert.IsFalse(this.MongoDBDriver.IsCollectionEmpty());
        }

        /// <summary>
        /// Is the test over-ridable with a custom driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void OverrideWithCustomDriver()
        {
            var firstDriver = this.MongoDBDriver;
            var newDriver = new MongoDBDriver<BsonDocument>(MongoFactory.GetCollection<BsonDocument>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), null, MongoDBConfig.GetCollectionString()));
            this.TestObject.OverrideMongoDBDriver(newDriver);

            Assert.AreNotEqual(firstDriver, this.MongoDBDriver);
            Assert.AreEqual(newDriver, this.MongoDBDriver);
            Assert.IsFalse(this.MongoDBDriver.IsCollectionEmpty());
        }

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
    }
}
