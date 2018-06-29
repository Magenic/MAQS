//--------------------------------------------------
// <copyright file="SeleniumDriverStoreTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Selenium driver store tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the Selenium driver store
    /// </summary>
    [TestClass]
    public class SeleniumDriverStoreTests : BaseSeleniumTest
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideWebDriver()
        {
            IWebDriver tempDriver = SeleniumConfig.Browser("phantomjs");
            this.WebDriver = tempDriver;

            Assert.AreEqual(this.TestObject.WebDriver.GetLowLevelDriver(), tempDriver.GetLowLevelDriver());
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            SeleniumDriverStore newDriver = new SeleniumDriverStore(() => SeleniumConfig.Browser("phantomjs"), this.TestObject);
            this.TestObject.DriversStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.WebDriver.GetLowLevelDriver(), ((SeleniumDriverStore)this.TestObject.DriversStore["test"]).Get().GetLowLevelDriver());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void SeleniumWebDriverInDriverStore()
        {
            Assert.AreEqual(this.TestObject.WebDriver, this.TestObject.GetDriver<SeleniumDriverStore>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriver(new WebServiceDriverStore(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriver<SeleniumDriverStore>(), "Expected a Selenium driver store");
            Assert.IsNotNull(this.TestObject.GetDriver<WebServiceDriverStore>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure separate interactions go to separate drivers
        /// </summary>
        [TestMethod]
        public void SeparateInteractions()
        {
            SeleniumDriverStore newDriver = new SeleniumDriverStore(() => SeleniumConfig.Browser("phantomjs"), this.TestObject);
            newDriver.Get().Navigate().GoToUrl("http://magenicautomation.azurewebsites.net/");

            this.TestObject.DriversStore.Add("test", newDriver);

            this.TestObject.WebDriver.Navigate().GoToUrl("http://magenicautomation.azurewebsites.net/Automation");

            Assert.AreNotEqual(this.TestObject.WebDriver.Url, ((SeleniumDriverStore)this.TestObject.DriversStore["test"]).Get().Url);
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the web driver
            this.WebDriver.Manage().Window.Maximize();

            SeleniumDriverStore driverWrapper = this.TestObject.DriversStore[typeof(SeleniumDriverStore).FullName] as SeleniumDriverStore;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            SeleniumDriverStore driverWrapper = this.TestObject.DriversStore[typeof(SeleniumDriverStore).FullName] as SeleniumDriverStore;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
