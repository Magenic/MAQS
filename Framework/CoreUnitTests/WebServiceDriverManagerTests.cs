//--------------------------------------------------
// <copyright file="WebServiceDriverManagerTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service driver store tests</summary>
//-------------------------------------------------- 
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the web driver store
    /// </summary>
    [TestClass]
    public class WebServiceDriverManagerTests : BaseWebServiceTest
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideWebServiceDriver()
        {
            WebServiceDriver tempDriver = new WebServiceDriver(WebServiceConfig.GetWebServiceUri());
            this.WebServiceDriver = tempDriver;
            
            Assert.AreEqual(this.TestObject.WebServiceManager.Get(), tempDriver);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            WebServiceDriverManager newDriver = new WebServiceDriverManager(() => new HttpClient(), this.TestObject);
            this.TestObject.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.WebServiceDriver, (WebServiceDriverManager)this.TestObject.ManagerStore["test"]);
            Assert.AreNotEqual(this.TestObject.WebServiceManager.Get(), ((WebServiceDriverManager)this.TestObject.ManagerStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object driver is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void DatabaseDriverInDriverStore()
        {
            Assert.AreEqual(this.TestObject.WebServiceDriver, this.TestObject.GetDriverManager<WebServiceDriverManager>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriverManager(new DatabaseDriverManager(() => DatabaseConfig.GetOpenConnection(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriverManager<DatabaseDriverManager>(), "Expected a database driver store");
            Assert.IsNotNull(this.TestObject.GetDriverManager<WebServiceDriverManager>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the web driver
            this.WebServiceDriver.ToString();

            WebServiceDriverManager driverDriver = this.TestObject.ManagerStore[typeof(WebServiceDriverManager).FullName] as WebServiceDriverManager;
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            WebServiceDriverManager driverDriver = this.TestObject.ManagerStore[typeof(WebServiceDriverManager).FullName] as WebServiceDriverManager;
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
