//--------------------------------------------------
// <copyright file="SeleniumDriverManagerTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Selenium driver store tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.WebServiceTester;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the Selenium driver store
    /// </summary>
    [TestClass]
    public class SeleniumDriverManagerTests : BaseSeleniumTest
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
            SeleniumDriverManager newDriver = new SeleniumDriverManager(() => SeleniumConfig.Browser("phantomjs"), this.TestObject);
            this.TestObject.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.WebDriver.GetLowLevelDriver(), ((SeleniumDriverManager)this.TestObject.ManagerStore["test"]).Get().GetLowLevelDriver());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void SeleniumWebDriverInDriverStore()
        {
            Assert.AreEqual(this.TestObject.WebDriver, this.TestObject.GetDriverManager<SeleniumDriverManager>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriverManager(new WebServiceDriverManager(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriverManager<SeleniumDriverManager>(), "Expected a Selenium driver store");
            Assert.IsNotNull(this.TestObject.GetDriverManager<WebServiceDriverManager>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure separate interactions go to separate drivers
        /// </summary>
        [TestMethod]
        public void SeparateInteractions()
        {
            SeleniumDriverManager newDriver = new SeleniumDriverManager(() => SeleniumConfig.Browser("phantomjs"), this.TestObject);
            newDriver.Get().Navigate().GoToUrl("http://magenicautomation.azurewebsites.net/");

            this.TestObject.ManagerStore.Add("test", newDriver);

            this.TestObject.WebDriver.Navigate().GoToUrl("http://magenicautomation.azurewebsites.net/Automation");

            Assert.AreNotEqual(this.TestObject.WebDriver.Url, ((SeleniumDriverManager)this.TestObject.ManagerStore["test"]).Get().Url);
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we intialize the web driver
            this.WebDriver.Manage().Window.Maximize();

            SeleniumDriverManager driverWrapper = this.TestObject.ManagerStore[typeof(SeleniumDriverManager).FullName] as SeleniumDriverManager;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            SeleniumDriverManager driverWrapper = this.TestObject.ManagerStore[typeof(SeleniumDriverManager).FullName] as SeleniumDriverManager;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }
    }
}
