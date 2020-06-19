//--------------------------------------------------
// <copyright file="DatabaseSQLiteUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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
    [NonParallelizable]
    public class DatabaseSQLiteUnitTests
    {
        public static string ConnectionStringToReplace = DatabaseConfig.GetConnectionString();
        public static string ProviderTypeToReplace = DatabaseConfig.GetProviderTypeString();
        [OneTimeSetUp]
        public void Setup()
        {
            System.Console.WriteLine(DatabaseConfig.GetConnectionString());
            System.Console.WriteLine(DatabaseConfig.GetProviderTypeString());
            // Override the configuration
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", "SQLITE" },
                { "DataBaseConnectionString", $"Data Source={ this.GetDByPath() }" },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs", true);
            System.Console.WriteLine(DatabaseConfig.GetConnectionString());
            System.Console.WriteLine(DatabaseConfig.GetProviderTypeString());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            System.Console.WriteLine(DatabaseConfig.GetConnectionString());
            System.Console.WriteLine(DatabaseConfig.GetProviderTypeString());
            // Override the configuration
            var overrides = new Dictionary<string, string>()
            {
                { "DataBaseProviderType", ProviderTypeToReplace },
                { "DataBaseConnectionString", ConnectionStringToReplace },
            };

            Config.AddTestSettingValues(overrides, "DatabaseMaqs", true);
            System.Console.WriteLine(DatabaseConfig.GetConnectionString());
            System.Console.WriteLine(DatabaseConfig.GetProviderTypeString());
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void VerifyOrdersSqliteNoDriverDefault()
        {
            DatabaseDriver driver = new DatabaseDriver();

            var orders = driver.Query("select * from orders").ToList();
            Assert.AreEqual(11, orders.Count);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
        public void VerifyOrdersSqliteNoDriverString()
        {
            DatabaseDriver driver = new DatabaseDriver("SQLITE", $"Data Source={ this.GetDByPath() }");

            var orders = driver.Query("select * from orders").ToList();
            Assert.AreEqual(11, orders.Count);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [Test]
        [Category(TestCategories.Database)]
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
        private string GetDByPath()
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
