//--------------------------------------------------
// <copyright file="MongoDriverManagerTests.cs" company="Magenic">
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
    public class MongoDriverManagerTests : BaseMongoTest<BsonDocument>
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
            MongoDriverManager<BsonDocument> newDriver = new MongoDriverManager<BsonDocument>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString(),  this.TestObject);
            this.TestObject.DriverStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.MongoDBWrapper, (MongoDriverManager<BsonDocument>)this.TestObject.DriverStore["test"]);
            Assert.AreNotEqual(this.TestObject.MongoDBDriver.Get(), ((MongoDriverManager<BsonDocument>)this.TestObject.DriverStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void MongoWrapperInDriverStore()
        {
            Assert.AreEqual(this.TestObject.MongoDBWrapper, this.TestObject.GetDriver<MongoDriverManager<BsonDocument>>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriver(new WebServiceDriverManager(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriver<MongoDriverManager<BsonDocument>>(), "Expected a Mongo driver store");
            Assert.IsNotNull(this.TestObject.GetDriver<WebServiceDriverManager>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the web driver
            this.MongoDBWrapper.IsCollectionEmpty();

            MongoDriverManager<BsonDocument> driverWrapper = this.TestObject.DriverStore[typeof(MongoDriverManager<BsonDocument>).FullName] as MongoDriverManager<BsonDocument>;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            MongoDriverManager<BsonDocument> driverWrapper = this.TestObject.DriverStore[typeof(MongoDriverManager<BsonDocument>).FullName] as MongoDriverManager<BsonDocument>;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
