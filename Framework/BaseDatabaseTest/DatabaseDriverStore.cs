//--------------------------------------------------
// <copyright file="DatabaseDriverStore.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Database driver</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using System;
using System.Data;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Database driver
    /// </summary>
    public class DatabaseDriverStore : DriverStore
    {
        /// <summary>
        /// Cached copy of the connection wrapper
        /// </summary>
        private DatabaseDriver wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDriverStore"/> class
        /// </summary>
        /// <param name="getConnection">Function for getting an database connection</param>
        /// <param name="testObject">The associated test object</param>
        public DatabaseDriverStore(Func<IDbConnection> getConnection, BaseTestObject testObject) : base(getConnection, testObject)
        {
        }

        /// <summary>
        /// Have the driver cleanup after itself
        /// </summary>
        public override void Dispose()
        {
            if (this.wrapper != null)
            {
                this.wrapper.Dispose();
            }

            this.wrapper = null;
        }

        /// <summary>
        /// Get the database wrapper
        /// </summary>
        /// <returns>The database wrapper</returns>
        public new DatabaseDriver Get()
        {
            if (this.wrapper == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting database wrapper");
                    this.wrapper = new DatabaseDriver(base.Get() as IDbConnection);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing database wrapper");
                    this.wrapper = new EventFiringDatabaseDriver(base.Get() as IDbConnection);
                    this.MapEvents(this.wrapper as EventFiringDatabaseDriver);
                }
            }

            return this.wrapper;
        }

        /// <summary>
        /// Override the database wrapper
        /// </summary>
        /// <param name="newWrapper">The new wrapper</param>
        public void OverwriteWrapper(DatabaseDriver newWrapper)
        {
            this.wrapper = newWrapper;
        }

        /// <summary>
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionWrapper">The event firing database wrapper that we want mapped</param>
        private void MapEvents(EventFiringDatabaseDriver eventFiringConnectionWrapper)
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
