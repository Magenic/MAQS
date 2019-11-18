//--------------------------------------------------
// <copyright file="MongoDBConfigUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseMongoTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Test class for database configurations
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MongoDBConfigUnitTests
    {
        /// <summary>
        /// Gets the connection string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseConnectionStringTest()
        {
            string connection = MongoDBConfig.GetConnectionString();
            Assert.AreEqual("mongodb://localhost:27017", connection);
        }

        /// <summary>
        /// Gets the database string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseStringTest()
        {
            string databaseString = MongoDBConfig.GetDatabaseString();
            Assert.AreEqual("MongoDatabaseTest", databaseString);
        }

        /// <summary>
        /// Gets the timeout value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Database)]
        public void GetDatabaseQueryTimeout()
        {
            int databaseTimeout = MongoDBConfig.GetQueryTimeout();
            Assert.AreEqual(30, databaseTimeout);
        }
    }
}
