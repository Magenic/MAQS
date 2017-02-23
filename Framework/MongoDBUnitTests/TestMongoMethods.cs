using Magenic.MaqsFramework.BaseMongoDBTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magenic.MaqsFramework.Utilities.Helper;
using BaseMongoDBTest;
using MongoDB.Bson;
using System.Configuration;
using System.Diagnostics;

namespace MongoDBUnitTests
{
    [TestClass]
    public class TestMongoMethods : Magenic.MaqsFramework.BaseMongoDBTest.BaseMongoDBTest
    {
        MongoDBCollectionWrapper<BsonDocument> collection;
        [TestInitialize]
        public void SetupCollectionWrapper() {
            this.collection = new MongoDBCollectionWrapper<BsonDocument>(new MongoDBConnectionWrapper(
                    ConfigurationManager.AppSettings["MongoConnectionString"],
                    ConfigurationManager.AppSettings["MongoDatabase"]).ReturnMongoDBDatabase(),
                    ConfigurationManager.AppSettings["MongoCollection"]
                    );
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void ListAllCollectionItems() {
            var collectionItems = this.collection.ListAllCollectionItems();
            Debugger.Break();
        }

        [TestMethod]
        [TestCategory(TestCategories.MongoDB)]
        public void CollectionWrapperTest() {

        }
    }
}
