//--------------------------------------------------
// <copyright file="DatabaseDriverStoreTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Database driver store tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the database driver store
    /// </summary>
    [TestClass]
    public class DatabaseDriverStoreTests : BaseDatabaseTest
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
            DatabaseDriverStore newDriver = new DatabaseDriverStore(() => DatabaseConfig.GetOpenConnection(), this.TestObject);
            this.TestObject.DriversStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.DatabaseDriver, (DatabaseDriverStore)this.TestObject.DriversStore["test"]);
            Assert.AreNotEqual(this.TestObject.DatabaseDriver.Get(), ((DatabaseDriverStore)this.TestObject.DriversStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void DatabaseWrapperInDriverStore()
        {
            Assert.AreEqual(this.TestObject.DatabaseWrapper, this.TestObject.GetDriver<DatabaseDriverStore>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriver(new WebServiceDriverStore(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriver<DatabaseDriverStore>(), "Expected a database driver store");
            Assert.IsNotNull(this.TestObject.GetDriver<WebServiceDriverStore>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the driver
            this.DatabaseWrapper.Execute("Select * from Sys.Databases");

            DatabaseDriverStore driverWrapper = this.TestObject.DriversStore[typeof(DatabaseDriverStore).FullName] as DatabaseDriverStore;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            DatabaseDriverStore driverWrapper = this.TestObject.DriversStore[typeof(DatabaseDriverStore).FullName] as DatabaseDriverStore;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
