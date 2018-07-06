//--------------------------------------------------
// <copyright file="DatabaseSQLiteUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------

using System;
using System.Collections.Generic;
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
    public class DatabaseSQLiteUnitTests
    {
        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void VerifyOrdersSqliteNoWrapperDefault()
        {
            // Override the configuration
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", "SQLITE" },
                { "DataBaseConnectionString", $"Data Source={ this.GetDByPath() }" },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs");

            DatabaseDriver wrapper = new DatabaseDriver();

            var orders = wrapper.Query("select * from orders").ToList();
            Assert.AreEqual(11, orders.Count);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void VerifyOrdersSqliteNoWrapperString()
        {
            // Override the configuration
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", "SQLITE" },
                { "DataBaseConnectionString", $"Data Source={ this.GetDByPath() }" },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs");

            DatabaseDriver wrapper = new DatabaseDriver(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());

            var orders = wrapper.Query("select * from orders").ToList();
            Assert.AreEqual(11, orders.Count);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void VerifyOrdersSqliteNoWrapperFunction()
        {
            // Override the configuration
            var overrides =
                new Dictionary<string, string>()
                    {
                        { "DataBaseConnectionString", $"Data Source={this.GetDByPath()}" },
                    };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs");

            using (SqliteConnection connection = new SqliteConnection(DatabaseConfig.GetConnectionString()))
            {
                SQLitePCL.Batteries.Init();
                connection.Open();
                DatabaseDriver wrapper = new DatabaseDriver(connection);

                var orders = wrapper.Query("select * from orders").ToList();
                Assert.AreEqual(11, orders.Count);
            }
        }

        /// <summary>
        /// Gets the path of the SQLITE file
        /// </summary>
        /// <returns>string path of the file</returns>
        private string GetDByPath()
        {
            UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().Location);
            return $"{Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path))}\\MyDatabase.sqlite";
        }
    }
}
