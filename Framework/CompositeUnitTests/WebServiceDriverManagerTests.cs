﻿//--------------------------------------------------
// <copyright file="WebServiceDriverManagerTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Web service driver store tests</summary>
//-------------------------------------------------- 
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Http;

namespace CompositeUnitTests
{
    /// <summary>
    /// Test the web driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.WebService)]
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
            this.ManagerStore.Add("test", newDriver);
            this.ManagerStore.GetDriver<WebServiceDriver, WebServiceDriverManager>();

            Assert.AreNotEqual(this.TestObject.WebServiceDriver, this.ManagerStore.GetManager<WebServiceDriverManager>("test"));
            Assert.AreNotEqual(this.TestObject.WebServiceManager.Get(), this.ManagerStore.GetDriver<WebServiceDriver>("test"));
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
            // Do something so we initialize the web driver
            this.WebServiceDriver.HttpClient.Timeout = TimeSpan.FromSeconds(1);

            WebServiceDriverManager driverDriver = this.ManagerStore.GetManager<WebServiceDriverManager>();
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been initialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            WebServiceDriverManager driverDriver = this.ManagerStore.GetManager<WebServiceDriverManager>();
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be initialized until it gets used");
        }

        /// <summary>
        /// Override with new client
        /// </summary>
        [TestMethod]
        public void RespectClientOverride()
        {
            var httpClient = new HttpClient();
            this.TestObject.AddDriverManager(new WebServiceDriverManager(httpClient, this.TestObject), true);

            Assert.AreEqual(httpClient, this.WebServiceDriver.HttpClient);
        }

        /// <summary>
        /// Override with new client function
        /// </summary>
        [TestMethod]
        public void RespectClientFunctionOverride()
        {
            var httpClient = new HttpClient();
            this.TestObject.AddDriverManager(new WebServiceDriverManager(() => httpClient, this.TestObject), true);

            Assert.AreEqual(httpClient, this.WebServiceDriver.HttpClient);
        }

        /// <summary>
        /// Override with new driver
        /// </summary>
        [TestMethod]
        public void RespectDriverOverride()
        {
            var httpClient = new HttpClient();
            this.TestObject.AddDriverManager(new WebServiceDriverManager(new WebServiceDriver(httpClient), this.TestObject), true);

            Assert.AreEqual(httpClient, this.WebServiceDriver.HttpClient);
        }

        /// <summary>
        /// Override test object with new driver
        /// </summary>
        [TestMethod]
        public void RespectTestObjectClientOverride()
        {
            var httpClient = new HttpClient();
            this.TestObject.OverrideWebServiceDriver(new WebServiceDriver(httpClient));

            Assert.AreEqual(httpClient, this.WebServiceDriver.HttpClient);
        }

        /// <summary>
        /// Override test object with new client function
        /// </summary>
        [TestMethod]
        public void RespectTestObjectClientFunctionOverride()
        {
            var httpClient = new HttpClient();
            this.TestObject.OverrideWebServiceDriver(() => httpClient);

            Assert.AreEqual(httpClient, this.WebServiceDriver.HttpClient);
        }

        /// <summary>
        /// Override test object with new client
        /// </summary>
        [TestMethod]
        public void RespectTestObjectDriverOverride()
        {
            var httpClient = new HttpClient();
            this.TestObject.OverrideWebServiceDriver(httpClient);

            Assert.AreEqual(httpClient, this.WebServiceDriver.HttpClient);
        }
    }
}
