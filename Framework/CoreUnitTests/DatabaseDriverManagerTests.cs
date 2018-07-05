//--------------------------------------------------
// <copyright file="DatabaseDriverManagerTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Database driver store tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the database driver store
    /// </summary>
    [TestClass]
    public class DatabaseDriverManagerTests : BaseDatabaseTest
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideDatabaseDriver()
        {
            DatabaseDriver tempDriver = new DatabaseDriver(DatabaseConfig.GetOpenConnection());
            this.DatabaseWrapper = tempDriver;
            
            Assert.AreEqual(this.TestObject.DatabaseDriver.Get(), tempDriver);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            DatabaseDriverManager newDriver = new DatabaseDriverManager(() => DatabaseConfig.GetOpenConnection(), this.TestObject);
            this.TestObject.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.DatabaseDriver, (DatabaseDriverManager)this.TestObject.ManagerStore["test"]);
            Assert.AreNotEqual(this.TestObject.DatabaseDriver.Get(), ((DatabaseDriverManager)this.TestObject.ManagerStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void DatabaseWrapperInDriverStore()
        {
            Assert.AreEqual(this.TestObject.DatabaseWrapper, this.TestObject.GetDriverManager<DatabaseDriverManager>().Get());
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
            // Do something so we intialize the driver
            this.DatabaseWrapper.Execute("Select * from Sys.Databases");

            DatabaseDriverManager driverWrapper = this.TestObject.ManagerStore[typeof(DatabaseDriverManager).FullName] as DatabaseDriverManager;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            DatabaseDriverManager driverWrapper = this.TestObject.ManagerStore[typeof(DatabaseDriverManager).FullName] as DatabaseDriverManager;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
