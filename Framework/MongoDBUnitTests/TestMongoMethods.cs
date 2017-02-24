using Magenic.MaqsFramework.BaseMongoDBTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magenic.MaqsFramework.Utilities.Helper;
using MongoDB.Bson;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;

namespace MongoDBUnitTests
{
    [TestClass]
    public class TestMongoMethods : BaseMongoDBTest<BsonDocument>
    {
        MongoDBCollectionWrapper<BsonDocument> collection;
        [TestInitialize]
        public void SetupCollectionWrapper() {
            this.collection = new MongoDBCollectionWrapper<BsonDocument>();
        }


        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void ListAllCollectionItems() {
            List<BsonDocument> collectionItems = this.collection.ListAllCollectionItems();
            foreach (BsonDocument bson in collectionItems) {
                Assert.IsTrue(bson.Contains("lid"));
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestCountItemsInCollection() {
            Assert.AreEqual(4, this.collection.CountAllItemsInCollection());
        }
    }
}
