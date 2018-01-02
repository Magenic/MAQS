//--------------------------------------------------
// <copyright file="DatabaseConfigUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        /// Gets the connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseConnectionStringTest()
        {
            #region GetConnection
            string connection = DatabaseConfig.GetConnectionString();
            #endregion
            Assert.AreEqual(connection, "Data Source=qasqlserver.database.windows.net;Initial Catalog=MagenicAutomation;Persist Security Info=True;User ID=MagenicQA;Password=1magenicMARQ;Connection Timeout=30");
        }

        /// <summary>
        /// Gets the timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseQueryTimeout()
        {
            #region GetQueryTimeout
            int databaseTimeout = DatabaseConfig.GetQueryTimeout();
            #endregion
            Assert.AreEqual(databaseTimeout, 30);
        }
    }
}
