//--------------------------------------------------
// <copyright file="MongoDriverStoreTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Mongo database driver store tests</summary>
//-------------------------------------------------- 
using Magenic.MaqsFramework.BaseMongoTest;
using Magenic.MaqsFramework.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the Mongo driver store
    /// </summary>
    [TestClass]
    public class MongoDriverStoreTests : BaseMongoTest<BsonDocument>
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideMongoDriver()
        {
            MongoDBDriver<BsonDocument> tempDriver = new MongoDBDriver<BsonDocument>();
            this.MongoDBWrapper = tempDriver;
            
            Assert.AreEqual(this.TestObject.MongoDBDriver.Get(), tempDriver);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            MongoDriverStore<BsonDocument> newDriver = new MongoDriverStore<BsonDocument>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString(),  this.TestObject);
            this.TestObject.DriversStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.MongoDBWrapper, (MongoDriverStore<BsonDocument>)this.TestObject.DriversStore["test"]);
            Assert.AreNotEqual(this.TestObject.MongoDBDriver.Get(), ((MongoDriverStore<BsonDocument>)this.TestObject.DriversStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void MongoWrapperInDriverStore()
        {
            Assert.AreEqual(this.TestObject.MongoDBWrapper, this.TestObject.GetDriver<MongoDriverStore<BsonDocument>>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriver(new WebServiceDriverStore(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriver<MongoDriverStore<BsonDocument>>(), "Expected a Mongo driver store");
            Assert.IsNotNull(this.TestObject.GetDriver<WebServiceDriverStore>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the web driver
            this.MongoDBWrapper.IsCollectionEmpty();

            MongoDriverStore<BsonDocument> driverWrapper = this.TestObject.DriversStore[typeof(MongoDriverStore<BsonDocument>).FullName] as MongoDriverStore<BsonDocument>;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            MongoDriverStore<BsonDocument> driverWrapper = this.TestObject.DriversStore[typeof(MongoDriverStore<BsonDocument>).FullName] as MongoDriverStore<BsonDocument>;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
