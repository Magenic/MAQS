//--------------------------------------------------
// <copyright file="MongoDBTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test class for the MongoDB wrapper</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseMongoDBTest;
using Magenic.MaqsFramework.Utilities.Helper;
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
    public class MongoDBTests : BaseMongoDBTest<BsonDocument>
    {
        /// <summary>
        /// Test the list all collection items helper function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoListAllCollectionItems()
        {
            List<BsonDocument> collectionItems = this.ObjectUnderTest.ListAllCollectionItems();
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
            Assert.AreEqual(4, this.ObjectUnderTest.CountAllItemsInCollection());
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoGetLoginID()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("lid", "test3");

            var value = this.ObjectUnderTest.Collection.Find(filter).ToList()[0]["lid"].ToString();
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
            BsonDocument document = this.ObjectUnderTest.Collection.Find(filter).ToList().First();
            Assert.AreEqual(document["lid"].ToString(), "test3");
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoFindListWithKey()
        {
            var filter = Builders<BsonDocument>.Filter.Exists("lid");
            List<BsonDocument> documentList = this.ObjectUnderTest.Collection.Find(filter).ToList();
            foreach (BsonDocument documents in documentList)
            {
                Assert.AreNotEqual(documents["lid"].ToString(), string.Empty);
            }

            Assert.AreEqual(documentList.Count, 4);
        }

        /// <summary>
        /// Test the collection works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestMongoLinqQuery()
        {
            IMongoQueryable<BsonDocument> query =
                from e in this.ObjectUnderTest.Collection.AsQueryable<BsonDocument>()
                where e["lid"] == "test1"
                select e;
            List<BsonDocument> retList = query.ToList<BsonDocument>();
            foreach (var value in retList)
            {
                Assert.AreEqual(value["lid"], "test1");
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
            Assert.AreEqual(this.TestObject.MongoDBCollectionWrapper, this.MongoDBWrapper, "Web service wrapper don't match");
        }

        /// <summary>
        /// Test the event firing collection wrapper
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void MongoDBTestSetupEventFiringTestObject()
        {
            this.SetupEventFiringTester();
            Assert.AreEqual(4, this.ObjectUnderTest.CountAllItemsInCollection());
        }
    }
}
