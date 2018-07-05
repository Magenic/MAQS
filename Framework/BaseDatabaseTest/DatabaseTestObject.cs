//--------------------------------------------------
// <copyright file="DatabaseTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds database context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Data;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Database test context data
    /// </summary>
    public class DatabaseTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseTestObject" /> class
        /// </summary>
        /// <param name="databaseConnection">The test's database connection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public DatabaseTestObject(Func<IDbConnection> databaseConnection, Logger logger, SoftAssert softAssert, string fullyQualifiedTestName) : base(logger, softAssert, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(DatabaseDriverManager).FullName, new DatabaseDriverManager(databaseConnection, this));
        }

        /// <summary>
        /// Gets the database driver
        /// </summary>
        public DatabaseDriverManager DatabaseDriver
        {
            get
            {
                return this.ManagerStore[typeof(DatabaseDriverManager).FullName] as DatabaseDriverManager;
            }
        }

        /// <summary>
        /// Gets the database wrapper
        /// </summary>
        public DatabaseDriver DatabaseWrapper
        {
            get
            {
                return this.DatabaseDriver.Get();
            }
        }

        /// <summary>
        /// Override the function for for getting a database connection
        /// </summary>
        /// <param name="databaseConnection">Function for creating a database connection</param>
        public void OverrideDatabaseConnection(Func<IDbConnection> databaseConnection)
        {
            this.OverrideDriverManager(typeof(DatabaseDriverManager).FullName, new DatabaseDriverManager(databaseConnection, this));
        }

        /// <summary>
        /// Override the database connection
        /// </summary>
        /// <param name="databaseConnection">New database connection</param>
        public void OverrideDatabaseWrapper(IDbConnection databaseConnection)
        {
            this.OverrideDriverManager(typeof(DatabaseDriverManager).FullName, new DatabaseDriverManager(() => databaseConnection, this));
        }

        /// <summary>
        /// Override the database connection wrapper
        /// </summary>
        /// <param name="wrapper">New database connection wrapper</param>
        public void OverrideDatabaseWrapper(DatabaseDriver wrapper)
        {
            this.DatabaseDriver.OverwriteWrapper(wrapper);
        }
    }
}