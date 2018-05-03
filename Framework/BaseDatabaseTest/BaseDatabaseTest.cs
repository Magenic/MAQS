//--------------------------------------------------
// <copyright file="BaseDatabaseTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base database test class</summary>
//--------------------------------------------------

using System.Data;
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Generic base database test class
    /// </summary>
    public class BaseDatabaseTest : BaseExtendableTest<DatabaseConnectionWrapper, DatabaseTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDatabaseTest"/> class.
        /// Setup the database client for each test class
        /// </summary>
        public BaseDatabaseTest()
        {
        }

        /// <summary>
        /// Gets or sets the web service wrapper
        /// </summary>
        public DatabaseConnectionWrapper DatabaseWrapper
        {
            get => this.ObjectUnderTest;

            set
            {
                this.ObjectUnderTest = value;
            }
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        protected virtual IDbConnection GetDataBaseConnection()
        {
            return DatabaseConfig.GetOpenConnection();
        }

        /// <summary>
        /// Close the database connection
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected override void BeforeLoggingTeardown(TestResultType resultType)
        {
            if (this.IsObjectUnderTestStored())
            {
                this.DatabaseWrapper.Dispose();
            }
        }

        /// <summary>
        /// Setup the event firing database connection
        /// </summary>
        protected override void SetupEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting event logging database wrapper");
            this.DatabaseWrapper = new EventFiringDatabaseConnectionWrapper(this.GetDataBaseConnection);
            this.MapEvents((EventFiringDatabaseConnectionWrapper)this.DatabaseWrapper);
        }

        /// <summary>
        /// Setup the normal database connection - the none event firing implementation
        /// </summary>
        protected override void SetupNoneEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting database wrapper");
            this.DatabaseWrapper = new DatabaseConnectionWrapper(this.GetDataBaseConnection);
        }

        /// <summary>
        /// Create a database test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new DatabaseTestObject(this.DatabaseWrapper, this.Log, this.SoftAssert, this.PerfTimerCollection);
        }

        /// <summary>
        /// Map database events to log events
        /// </summary>
        /// <param name="eventFiringConnectionWrapper">The event firing database wrapper that we want mapped</param>
        private void MapEvents(EventFiringDatabaseConnectionWrapper eventFiringConnectionWrapper)
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