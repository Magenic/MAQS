//--------------------------------------------------
// <copyright file="MongoDriverManager.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Mongo database driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
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
        public MongoDriverManager(string connectionString, string databaseString, string collectionString, BaseTestObject testObject) : base(() => null, testObject)
        {
            this.GetDriver = () => (connectionString, databaseString, collectionString);
        }

        /// <summary>
        /// Dispose of the driver
        /// </summary>
        public override void Dispose()
        {
            this.BaseDriver = null;
        }

        /// <summary>
        /// Override the Mongo driver
        /// </summary>
        /// <param name="overrideDriver">The new Mongo driver</param>
        public void OverrideDriver(MongoDBDriver<T> overrideDriver)
        {
            this.driver = overrideDriver;
        }

        /// <summary>
        /// Get the Mongo driver
        /// </summary>
        /// <returns>The Mongo driver</returns>
        public MongoDBDriver<T> GetMongoDriver()
        {
            if (this.driver == null)
            {
                ValueTuple<string, string, string> temp = (ValueTuple<string, string, string>)GetBase();

                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting Mongo driver");
                    this.driver = new MongoDBDriver<T>(temp.Item1, temp.Item2, temp.Item3);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing Mongo driver");
                    this.driver = new EventFiringMongoDBDriver<T>(temp.Item1, temp.Item2, temp.Item3);
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
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionDriver">The event firing database driver that we want mapped</param>
        private void MapEvents(EventFiringMongoDBDriver<T> eventFiringConnectionDriver)
        {
            eventFiringConnectionDriver.DatabaseEvent += this.Database_Event;
            eventFiringConnectionDriver.DatabaseErrorEvent += this.Database_Error;
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
