//--------------------------------------------------
// <copyright file="MongoDriverManagerTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Mongo database driver store tests</summary>
//-------------------------------------------------- 
using Magenic.Maqs.BaseMongoTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the Mongo driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.MongoDB)]
    [DoNotParallelize]
    [ExcludeFromCodeCoverage]
    public class MongoDriverManagerTests : BaseMongoTest<BsonDocument>
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideMongoDriver()
        {
            MongoDBDriver<BsonDocument> tempDriver = new MongoDBDriver<BsonDocument>();
            this.MongoDBDriver = tempDriver;
            
            Assert.AreEqual(this.TestObject.MongoDBManager.Get(), tempDriver);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            MongoDriverManager<BsonDocument> newDriver = new MongoDriverManager<BsonDocument>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString(),  this.TestObject);
            this.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.MongoDBDriver, (MongoDriverManager<BsonDocument>)this.ManagerStore["test"]);
            Assert.AreNotEqual(this.TestObject.MongoDBManager.Get(), ((MongoDriverManager<BsonDocument>)this.ManagerStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object driver is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void MongoDriverInDriverStore()
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

            Assert.IsNotNull(this.TestObject.GetDriverManager<MongoDriverManager<BsonDocument>>(), "Expected a Mongo driver store");
            Assert.IsNotNull(this.TestObject.GetDriverManager<WebServiceDriverManager>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we initialize the web driver
            this.MongoDBDriver.IsCollectionEmpty();

            MongoDriverManager<BsonDocument> driverDriver = this.ManagerStore[typeof(MongoDriverManager<BsonDocument>).FullName] as MongoDriverManager<BsonDocument>;
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been initialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            MongoDriverManager<BsonDocument> driverDriver = this.ManagerStore[typeof(MongoDriverManager<BsonDocument>).FullName] as MongoDriverManager<BsonDocument>;
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be initialized until it gets used");
        }
    }
}
