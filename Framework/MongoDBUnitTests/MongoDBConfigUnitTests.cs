//--------------------------------------------------
// <copyright file="MongoDBConfigUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseMongoDBTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Test class for database configurations
    /// </summary>
    [TestClass]
    public class MongoDBConfigUnitTests
    {
        /// <summary>
        /// Gets the connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseConnectionStringTest()
        {
            #region GetConnection
            string connection = MongoDBConfig.GetConnectionString();
            #endregion
            Assert.AreEqual(connection, "mongodb://10.155.45.36:27017");
        }

        /// <summary>
        /// Gets the timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseQueryTimeout()
        {
            #region GetQueryTimeout
            int databaseTimeout = MongoDBConfig.GetQueryTimeout();
            #endregion
            Assert.AreEqual(databaseTimeout, 30);
        }
    }
}
