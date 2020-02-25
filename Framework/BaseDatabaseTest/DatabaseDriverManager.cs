﻿//--------------------------------------------------
// <copyright file="DatabaseDriverManager.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Database driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Data;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Database driver
    /// </summary>
    public class DatabaseDriverManager : DriverManager
    {
        /// <summary>
        /// Cached copy of the connection driver
        /// </summary>
        private DatabaseDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDriverManager"/> class
        /// </summary>
        /// <param name="getConnection">Function for getting an database connection</param>
        /// <param name="testObject">The associated test object</param>
        public DatabaseDriverManager(Func<IDbConnection> getConnection, BaseTestObject testObject) : base(getConnection, testObject)
        {
        }

        /// <summary>
        /// Override the database driver
        /// </summary>
        /// <param name="newDriver">The new driver</param>
        [Obsolete("Change to OverrideDriver for consistency")]
        public void OverwriteDriver(DatabaseDriver newDriver)
        {
            this.OverrideDriver(newDriver);
        }

        /// <summary>
        /// Override the database driver
        /// </summary>
        /// <param name="newDriver">The new driver</param>
        public void OverrideDriver(DatabaseDriver newDriver)
        {
            this.driver = newDriver;
            this.BaseDriver = newDriver.Connection;
        }

        /// <summary>
        /// Override the email driver - respects lazy loading
        /// </summary>
        /// <param name="overrideDriver">Function for getting new database connection</param>
        public void OverrideDriver(Func<IDbConnection> overrideDriver)
        {
            this.driver = null;
            this.OverrideDriverGet(overrideDriver);
        }

        /// <summary>
        /// Get the database driver
        /// </summary>
        /// <returns>The database driver</returns>
        public DatabaseDriver GetDatabaseDriver()
        {
            if (this.driver == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting database driver");
                    this.driver = new DatabaseDriver(GetBase() as IDbConnection);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing database driver");
                    this.driver = new EventFiringDatabaseDriver(GetBase() as IDbConnection);
                    this.MapEvents(this.driver as EventFiringDatabaseDriver);
                }
            }

            return this.driver;
        }

        /// <summary>
        /// Get the database driver
        /// </summary>
        /// <returns>The database driver</returns>
        public override object Get()
        {
            return this.GetDatabaseDriver();
        }

        /// <summary>
        /// Have the driver cleanup after itself
        /// </summary>
        protected override void DriverDispose()
        {
            if (this.driver != null)
            {
                this.driver.Dispose();
            }

            this.driver = null;
        }

        /// <summary>
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionDriver">The event firing database driver that we want mapped</param>
        private void MapEvents(EventFiringDatabaseDriver eventFiringConnectionDriver)
        {
            eventFiringConnectionDriver.DatabaseEvent += this.Database_Event;
            eventFiringConnectionDriver.DatabaseActionEvent += this.Database_Action;
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
        /// Database action event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void Database_Action(object sender, string message)
        {
            this.Log.LogMessage(MessageType.ACTION, message);
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
