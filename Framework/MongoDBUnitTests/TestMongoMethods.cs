using Magenic.MaqsFramework.BaseMongoDBTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magenic.MaqsFramework.Utilities.Helper;
using MongoDB.Bson;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using MongoDB.Driver;
using System.Linq;

namespace MongoDBUnitTests
{
    [TestClass]
    public class TestMongoMethods : BaseMongoDBTest<BsonDocument>
    {
        [TestInitialize]
        public void SetupCollectionWrapper() {
            this.ObjectUnderTest = new MongoDBCollectionWrapper<BsonDocument>();
        }


        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void ListAllCollectionItems() {
            List<BsonDocument> collectionItems = this.ObjectUnderTest.ListAllCollectionItems();
            foreach (BsonDocument bson in collectionItems) {
                Assert.IsTrue(bson.Contains("lid"));
            }
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestCountItemsInCollection() {
            Assert.AreEqual(4, this.ObjectUnderTest.CountAllItemsInCollection());
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void GetLoginID() {
            var filter = Builders<BsonDocument>.Filter.Eq("lid", "test3");
            var value = this.ObjectUnderTest.collection.Find(filter).ToList()[0]["lid"].ToString();
            Assert.AreEqual(value, "test3");
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestQueryAndReturnFirst() {
            BsonDocument document = this.ObjectUnderTest.QueryAndReturnFirst("lid", "test3");
            Assert.AreEqual(document["lid"].ToString(), "test3");
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestFindListWithKey() {
            List<BsonDocument> documentList = this.ObjectUnderTest.FindListWithKey("lid");
            foreach (BsonDocument documents in documentList) {
                Assert.AreNotEqual(documents["lid"].ToString(), "");
            }
            Assert.AreEqual(documentList.Count, 4);
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void TestQueryAndReturnList() {
            List<BsonDocument> documentList = this.ObjectUnderTest.QueryAndReturnList("lid", "test3");
            foreach (BsonDocument documents in documentList)
            {
                Assert.AreEqual(documents["lid"].ToString(), "test3");
            }
            Assert.AreEqual(documentList.Count, 1);
        }
    }
}
