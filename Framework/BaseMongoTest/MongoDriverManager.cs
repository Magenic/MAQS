//--------------------------------------------------
// <copyright file="MongoDriverManager.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Mongo database driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using MongoDB.Driver;
using System;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Mongo database driver
    /// </summary>
    /// <typeparam name="T">The Mongo collection type</typeparam>
    public class MongoDriverManager<T> : DriverManager
    {
        /// <summary>
        /// Cached copy of the connection driver
        /// </summary>
        private MongoDBDriver<T> driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDriverManager{T}" /> class
        /// </summary>
        /// <param name="connectionString">Client connection string</param>
        /// <param name="databaseString">Database connection string</param>
        /// <param name="collectionString">Mongo collection string</param>
        /// <param name="testObject">Test object this driver is getting added to</param>
        public MongoDriverManager(string connectionString, string databaseString, string collectionString, ITestObject testObject) : base(() => MongoFactory.GetCollection<T>(connectionString, databaseString, collectionString), testObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDriverManager{T}" /> class
        /// </summary>
        /// <param name="getCollection">Function for getting a Mongo collection connection</param>
        /// <param name="testObject">Test object this driver is getting added to</param>
        public MongoDriverManager(Func<IMongoCollection<T>> getCollection, ITestObject testObject) : base(getCollection, testObject)
        {
        }

        /// <summary>
        /// Override the Mongo driver
        /// </summary>
        /// <param name="overrideDriver">The new Mongo driver</param>
        public void OverrideDriver(MongoDBDriver<T> overrideDriver)
        {
            this.driver = overrideDriver;
            this.BaseDriver = overrideDriver.Collection;
        }

        /// <summary>
        /// Override the Mongo driver - respects lazy loading
        /// </summary>
        /// <param name="connectionString">Connection string of mongo DB</param>
        /// <param name="databaseString">Database string to use</param>
        /// <param name="collectionString">Collection string to use</param>
        public void OverrideDriver(string connectionString, string databaseString, string collectionString)
        {
            this.driver = null;
            this.OverrideDriverGet(() => MongoFactory.GetCollection<T>(connectionString, databaseString, collectionString));
        }

        /// <summary>
        /// Override the Mongo driver - respects lazy loading
        /// </summary>
        /// <param name="overrideCollectionConnection">The new collection connection</param>
        public void OverrideDriver(Func<IMongoCollection<T>> overrideCollectionConnection)
        {
            this.driver = null;
            this.OverrideDriverGet(overrideCollectionConnection);
        }


        /// <summary>
        /// Get the Mongo driver
        /// </summary>
        /// <returns>The Mongo driver</returns>
        public MongoDBDriver<T> GetMongoDriver()
        {
            if (this.driver == null)
            {
                IMongoCollection<T> temp = (IMongoCollection<T>)GetBase();

                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting Mongo driver");
                    this.driver = new MongoDBDriver<T>(temp);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing Mongo driver");
                    this.driver = new EventFiringMongoDBDriver<T>(temp);
                    this.MapEvents((EventFiringMongoDBDriver<T>)this.driver);
                }
            }

            return this.driver;
        }

        /// <summary>
        /// Get the Mongo driver
        /// </summary>
        /// <returns>The Mongo driver</returns>
        public override object Get()
        {
            return this.GetMongoDriver();
        }

        /// <summary>
        /// Dispose of the driver
        /// </summary>
        protected override void DriverDispose()
        {
            this.BaseDriver = null;
        }

        /// <summary>
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionDriver">The event firing database driver that we want mapped</param>
        private void MapEvents(EventFiringMongoDBDriver<T> eventFiringConnectionDriver)
        {
            eventFiringConnectionDriver.DatabaseEvent += this.MongoDatabase_Event;
            eventFiringConnectionDriver.DatabaseActionEvent += this.MongoDatabase_Action;
            eventFiringConnectionDriver.DatabaseErrorEvent += this.MongoDatabase_Error;
        }

        /// <summary>
        /// Database event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void MongoDatabase_Event(object sender, string message)
        {
            this.Log.LogMessage(MessageType.INFORMATION, message);
        }

        /// <summary>
        /// Database action event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void MongoDatabase_Action(object sender, string message)
        {
            this.Log.LogMessage(MessageType.ACTION, message);
        }

        /// <summary>
        /// Database error event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void MongoDatabase_Error(object sender, string message)
        {
            this.Log.LogMessage(MessageType.ERROR, message);
        }
    }
}
