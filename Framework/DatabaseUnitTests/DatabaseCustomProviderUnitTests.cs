//--------------------------------------------------
// <copyright file="DatabaseSQLiteUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.Data.Sqlite;
using NUnit.Framework;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DatabaseCustomProviderUnitTests : BaseDatabaseTest
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void CustomIProviderTest()
        {
            var orders = this.DatabaseDriver.Query("select * from orders").ToList();
            Assert.AreEqual(11, orders.Count);
        }

        /// <summary>
        /// Get the database connection
        /// </summary>
        /// <returns>The database connection</returns>
        protected override IDbConnection GetDataBaseConnection()
        {
            // Override the configuration 
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", "DatabaseUnitTests.TestProvider" },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs");

            return DatabaseConfig.GetOpenConnection();
        }
    }
}
