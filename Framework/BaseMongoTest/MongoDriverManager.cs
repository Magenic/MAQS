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
        /// Cached copy of the connection wrapper
        /// </summary>
        private MongoDBDriver<T> wrapper;

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
        /// Override the Mongo wrapper
        /// </summary>
        /// <param name="overrideWrapper">The new Mongo wrapper</param>
        public void OverrideWrapper(MongoDBDriver<T> overrideWrapper)
        {
            this.wrapper = overrideWrapper;
        }

        /// <summary>
        /// Get the Mongo wrapper
        /// </summary>
        /// <returns>The Mongo wrapper</returns>
        public new MongoDBDriver<T> Get()
        {
            if (this.wrapper == null)
            {
                ValueTuple<string, string, string> temp = (ValueTuple<string, string, string>)base.Get();

                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting Mongo wrapper");
                    this.wrapper = new MongoDBDriver<T>(temp.Item1, temp.Item2, temp.Item3);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing Mongo wrapper");
                    this.wrapper = new EventFiringMongoDBDriver<T>(temp.Item1, temp.Item2, temp.Item3);
                    this.MapEvents((EventFiringMongoDBDriver<T>)this.wrapper);
                }
            }

            return this.wrapper;
        }

        /// <summary>
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionWrapper">The event firing database wrapper that we want mapped</param>
        private void MapEvents(EventFiringMongoDBDriver<T> eventFiringConnectionWrapper)
        {
            eventFiringConnectionWrapper.DatabaseEvent += this.Database_Event;
            eventFiringConnectionWrapper.DatabaseErrorEvent += this.Database_Error;
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
