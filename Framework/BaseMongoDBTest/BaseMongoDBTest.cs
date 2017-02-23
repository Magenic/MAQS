//--------------------------------------------------
// <copyright file="BaseMongoDBTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base MongoDB test class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using MongoDB.Driver;

namespace Magenic.MaqsFramework.BaseMongoDBTest
{ 
    /// <summary>
    /// Generic base MongoDB test class
    /// </summary>
    public class BaseMongoDBTest : BaseExtendableTest<MongoDBConnectionWrapper, MongoTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMongoDBTest"/> class.
        /// Setup the database client for each test class
        /// </summary>
        public BaseMongoDBTest()
        {
        }

        /// <summary>
        /// Gets or sets the web service wrapper
        /// </summary>
        public MongoDBConnectionWrapper MongoDBWrapper
        {
            get
            {
                return (MongoDBConnectionWrapper)this.ObjectUnderTest;
            }

            set
            {
                this.ObjectUnderTest = value;
            }
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        protected virtual IMongoDatabase GetMongoDBConnection()
        {
            var mongoURL = new MongoUrl(this.GetBaseConnectionString());
            IMongoClient client = new MongoClient(mongoURL);
            return client.GetDatabase(MongoDBConfig.GetDatabaseString());
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual string GetBaseConnectionString()
        {
            return MongoDBConfig.GetConnectionString();
        }

        /// <summary>
        /// Setup the event firing database connection
        /// </summary>
        protected override void SetupEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting event logging database wrapper");
            this.MongoDBWrapper = new EventFiringMongoDBConnectionWrapper(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString());
            this.MapEvents((EventFiringMongoDBConnectionWrapper)this.MongoDBWrapper);
        } 

        /// <summary>
        /// Setup the normal database connection - the none event firing implementation
        /// </summary>
        protected override void SetupNoneEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting database wrapper");
            this.MongoDBWrapper = new MongoDBConnectionWrapper(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString());
        }

        /// <summary>
        /// Create a MongoDB test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new MongoTestObject(this.MongoDBWrapper, this.Log, this.SoftAssert, this.PerfTimerCollection);
        }

        /// <summary>
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionWrapper">The event firing database wrapper that we want mapped</param>
        private void MapEvents(EventFiringMongoDBConnectionWrapper eventFiringConnectionWrapper)
        {
            if (this.LoggingEnabledSetting == LoggingEnabled.YES || this.LoggingEnabledSetting == LoggingEnabled.ONFAIL)
            {
                eventFiringConnectionWrapper.DatabaseEvent += this.Database_Event;
                eventFiringConnectionWrapper.DatabaseErrorEvent += this.Database_Error;
            }
        }

        /// <summary>
        /// Database event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void Database_Event(object sender, string message)
        {
            this.Log.LogMessage(MessageType.INFORMATION, message);
        }

        /// <summary>
        /// Database error event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void Database_Error(object sender, string message)
        {
            this.Log.LogMessage(MessageType.ERROR, message);
        }
    }
}