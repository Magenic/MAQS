//--------------------------------------------------
// <copyright file="DatabaseSQLiteUnitTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Database base test unit tests</summary>
//--------------------------------------------------

using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test basic database base test functionality
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
    public class DatabaseSQLiteUnitTests
    {
        /// <summary>
        /// Hold the connection string for replacing at the end
        /// </summary>
        public static string ConnectionStringToReplace = DatabaseConfig.GetConnectionString();

        /// <summary>
        /// Holds the provider type to replace in the class cleanup
        /// </summary>
        public static string ProviderTypeToReplace = DatabaseConfig.GetProviderTypeString();

        /// <summary>
        /// Setup to override the connections
        /// </summary>
        /// <param name="testContext">TestContext that is unused</param>
        [TestInitialize]
        public void Setup()
        {
            ConnectionStringToReplace = DatabaseConfig.GetConnectionString();
            ProviderTypeToReplace = DatabaseConfig.GetProviderTypeString();
            // Override the configuration
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", "SQLITE" },
                { "DataBaseConnectionString", $"Data Source={ GetDByPath() }" },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs", true);
        }

        [TestCleanup]
        public void TearDown()
        {
            // Override the configuration
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", ProviderTypeToReplace },
                { "DataBaseConnectionString", ConnectionStringToReplace },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs", true);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyOrdersSqliteNoDriverDefault()
        {
            using (DatabaseDriver driver = new DatabaseDriver())
            {
                var orders = driver.Query("select * from orders").ToList();
                Assert.AreEqual(11, orders.Count);
            }
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyOrdersSqliteNoDriverString()
        {
            using (DatabaseDriver driver = new DatabaseDriver("SQLITE", $"Data Source={ GetDByPath() }"))
            {
                var orders = driver.Query("select * from orders").ToList();
                Assert.AreEqual(11, orders.Count);
            }
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        [DoNotParallelize]
        public void VerifyOrdersSqliteNoDriverFunction()
        {
            using (SqliteConnection connection = new SqliteConnection(DatabaseConfig.GetConnectionString()))
            {
                SQLitePCL.Batteries.Init();
                connection.Open();
                DatabaseDriver driver = new DatabaseDriver(connection);

                var orders = driver.Query("select * from orders").ToList();
                Assert.AreEqual(11, orders.Count);
            }
        }

        /// <summary>
        /// Gets the path of the SQLITE file
        /// </summary>
        /// <returns>string path of the file</returns>
        private static string GetDByPath()
        {
            Uri uri = null;
            // Building an absolute URL from the assembly location fails on some
            // Azure DevOps hosted build environments.
            if (Uri.TryCreate(Assembly.GetExecutingAssembly().Location, UriKind.RelativeOrAbsolute, out uri) &&
                uri.IsAbsoluteUri)
            {
                return $"{Path.GetDirectoryName(Uri.UnescapeDataString(uri.AbsolutePath))}\\MyDatabase.sqlite";
            }
            else
            {
                return "MyDatabase.sqlite";
            }
        }
    }
}
