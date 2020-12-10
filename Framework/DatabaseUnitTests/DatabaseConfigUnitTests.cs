//--------------------------------------------------
// <copyright file="DatabaseConfigUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using Dapper;
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DatabaseUnitTests
{
    /// <summary>
    /// Test class for database configurations
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DatabaseConfigUnitTests
    {
        /// <summary>
        /// Verify the connection string is correct
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseConnectionStringTest()
        {
            string connection = DatabaseConfig.GetConnectionString();
            Assert.AreEqual("Data Source=localhost;Initial Catalog=MagenicAutomation;Persist Security Info=True;User ID=sa;Password=magenicMAQS2;Connection Timeout=30", connection);
        }

        /// <summary>
        /// Gets the connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseProviderTypeStringTest()
        {
            string provider = DatabaseConfig.GetProviderTypeString();
            Assert.AreEqual("SQLSERVER", provider);
        }

        /// <summary>
        /// Test the if custom providers classes can be passed to create database connection client, using configuration values for connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetConnectionProviderDefaultConnectionString()
        {
            var connection = DatabaseConfig.GetOpenConnection(new TestProvider());
            Assert.IsNotNull(connection);
            Assert.AreEqual(DatabaseConfig.GetConnectionString(), connection.ConnectionString);
        }

        /// <summary>
        /// Test the if custom providers classes can be passed to create database connection client, using passed connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetConnectionProviderNonDefaultConnectionString()
        {
            var connection = DatabaseConfig.GetOpenConnection(new TestProvider(), "Data Source=testdb;");
            Assert.IsNotNull(connection);
            Assert.AreEqual("Data Source=testdb;", connection.ConnectionString);
        }

        /// <summary>
        /// Check that we get back the state table
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void VerifyStateTableExistsCustomProvider()
        {
            var connection = DatabaseConfig.GetOpenConnection(new TestProvider());
            var table = connection.Query("SELECT * FROM information_schema.tables");

            Assert.IsTrue(table.Any(n => n.TABLE_NAME.Equals("States")));
        }
    }
}
