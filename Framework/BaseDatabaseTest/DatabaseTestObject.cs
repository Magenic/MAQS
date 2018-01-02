//--------------------------------------------------
// <copyright file="DatabaseTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds database context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;

namespace Magenic.MaqsFramework.BaseDatabaseTest
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
        /// <param name="perfTimerCollection">The test's performance timer collection</param>
        public DatabaseTestObject(DatabaseConnectionWrapper databaseConnection, Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection) : base(logger, softAssert, perfTimerCollection)
        {
            this.DatabaseWrapper = databaseConnection;
        }

        /// <summary>
        /// Gets the database connection wrapper
        /// </summary>
        public DatabaseConnectionWrapper DatabaseWrapper { get; private set; }
    }
}
