//--------------------------------------------------
// <copyright file="DatabaseConfigUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

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
            Assert.AreEqual(connection, "Data Source=qasqlserver.database.windows.net;Initial Catalog=MagenicAutomation;Persist Security Info=True;User ID=MagenicQA;Password=1magenicMARQ;Connection Timeout=30");
        }

        /// <summary>
        /// Gets the connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseProviderTypeStringTest()
        {
            string provider = DatabaseConfig.GetProviderTypeString();
            Assert.AreEqual(provider, "SQLSERVER");
        }

        /// <summary>
        /// Gets the timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseQueryTimeout()
        {
            int databaseTimeout = DatabaseConfig.GetQueryTimeout();
            Assert.AreEqual(databaseTimeout, 30);
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
    }
}
