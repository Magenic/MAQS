//--------------------------------------------------
// <copyright file="WebServiceDriverStoreTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service driver store tests</summary>
//-------------------------------------------------- 
using Magenic.MaqsFramework.BaseDatabaseTest;
using Magenic.MaqsFramework.BaseWebServiceTest;
using Magenic.MaqsFramework.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the web driver store
    /// </summary>
    [TestClass]
    public class WebServiceDriverStoreTests : BaseWebServiceTest
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideWebServiceDriver()
        {
            WebServiceDriver tempDriver = new WebServiceDriver(WebServiceConfig.GetWebServiceUri());
            this.WebServiceWrapper = tempDriver;
            
            Assert.AreEqual(this.TestObject.WebServiceDriverStore.Get(), tempDriver);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            WebServiceDriverStore newDriver = new WebServiceDriverStore(() => new HttpClient(), this.TestObject);
            this.TestObject.DriversStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.WebServiceDriver, (WebServiceDriverStore)this.TestObject.DriversStore["test"]);
            Assert.AreNotEqual(this.TestObject.WebServiceDriverStore.Get(), ((WebServiceDriverStore)this.TestObject.DriversStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void DatabaseWrapperInDriverStore()
        {
            Assert.AreEqual(this.TestObject.WebServiceDriver, this.TestObject.GetDriver<WebServiceDriverStore>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriver(new DatabaseDriverStore(() => DatabaseConfig.GetOpenConnection(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriver<DatabaseDriverStore>(), "Expected a database driver store");
            Assert.IsNotNull(this.TestObject.GetDriver<WebServiceDriverStore>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the web driver
            this.WebServiceWrapper.ToString();

            WebServiceDriverStore driverWrapper = this.TestObject.DriversStore[typeof(WebServiceDriverStore).FullName] as WebServiceDriverStore;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            WebServiceDriverStore driverWrapper = this.TestObject.DriversStore[typeof(WebServiceDriverStore).FullName] as WebServiceDriverStore;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
