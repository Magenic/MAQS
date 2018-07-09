//--------------------------------------------------
// <copyright file="BaseDatabaseTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base database test class</summary>
//--------------------------------------------------

using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using System.Data;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Generic base database test class
    /// </summary>
    public class BaseDatabaseTest : BaseExtendableTest<DatabaseTestObject>
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
        public DatabaseDriver DatabaseDriver
        {
            get
            {
                return this.TestObject.DatabaseDriver;
            }

            set
            {
                this.TestObject.OverrideDatabaseDriver(value);
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
        /// Create a database test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new DatabaseTestObject(() => this.GetDataBaseConnection(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
        }
    }
}