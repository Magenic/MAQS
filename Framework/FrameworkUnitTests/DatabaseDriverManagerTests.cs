//--------------------------------------------------
// <copyright file="DatabaseDriverManagerTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Database driver store tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Test the database driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Database)]
    [ExcludeFromCodeCoverage]
    public class DatabaseDriverManagerTests : BaseDatabaseTest
    {
        /// <summary>
        /// Make sure we get the proper factory failure
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OracleFailure()
        {
            this.TestObject.OverrideDatabaseDriver(ConnectionFactory.GetOpenConnection("Oracle", string.Empty));
            Assert.Fail("Get open connection should have thrown exception.");
        }

        /// <summary>
        /// Make sure we get the proper factory failure
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OverideFactoryFailure()
        {
            this.TestObject.OverrideDatabaseDriver(ConnectionFactory.GetOpenConnection(string.Empty, string.Empty));
            Assert.Fail("Get open connection should have thrown exception.");
        }

        /// <summary>
        /// Make sure we can override the driver using the connection factory
        /// </summary>
        [TestMethod]
        public void OverrideConnectionWithFactory()
        {
            var connection = ConnectionFactory.GetOpenConnection();
            this.TestObject.OverrideDatabaseDriver(connection);
            Assert.AreEqual(connection, this.DatabaseDriver.Connection);
        }

        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideDatabaseDriver()
        {
            DatabaseDriver tempDriver = new DatabaseDriver(DatabaseConfig.GetOpenConnection());
            this.DatabaseDriver = tempDriver;

            Assert.AreEqual(this.TestObject.DatabaseManager.Get(), tempDriver);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            DatabaseDriverManager newDriver = new DatabaseDriverManager(() => DatabaseConfig.GetOpenConnection(), this.TestObject);
            this.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.DatabaseManager, this.ManagerStore.GetManager<DatabaseDriverManager>("test"));
            Assert.AreNotEqual(this.TestObject.DatabaseManager.Get(), (this.ManagerStore.GetManager<DatabaseDriverManager>("test")).Get());
        }

        /// <summary>
        /// Make sure the test object driver is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void DatabaseDriverInDriverStore()
        {
            Assert.AreEqual(this.TestObject.DatabaseDriver, this.TestObject.GetDriverManager<DatabaseDriverManager>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriverManager(new WebServiceDriverManager(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriverManager<DatabaseDriverManager>(), "Expected a database driver store");
            Assert.IsNotNull(this.TestObject.GetDriverManager<WebServiceDriverManager>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            DatabaseDriverManager driverDriver = this.ManagerStore.GetManager<DatabaseDriverManager>();

            // Do something so we initialize the driver
            Assert.IsNotNull(this.DatabaseDriver.Connection, "Connection should not be null");
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been initialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            DatabaseDriverManager driverDriver = this.ManagerStore.GetManager<DatabaseDriverManager>();
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be initialized until it gets used");
        }
    }
}
