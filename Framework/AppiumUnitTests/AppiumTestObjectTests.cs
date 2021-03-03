﻿//--------------------------------------------------
// <copyright file="AppiumTestObjectTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for appium test object files</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace AppiumUnitTests
{
    /// <summary>
    /// Appium Test Object Unit Tests
    /// </summary>
    [TestClass]
    public class AppiumTestObjectTests : BaseAppiumTest
    {
        /// <summary>
        /// Test for getting Appium Test Object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void AppiumTestObjectTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
        }

        /// <summary>
        /// Test for getting instance of Wait Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetAndCloseDriverTest()
        {
            AppiumDriver<IWebElement> driver = AppiumDriverFactory.GetDefaultMobileDriver();
            try
            {
                Assert.IsNotNull(driver);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }

        /// <summary>
        /// Test for getting instance of Wait Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetAndCloseDriverTestFromManager()
        {
            this.TestObject.GetDriverManager<MobileDriverManager>().Dispose();
        }

        /// <summary>
        /// Override driver respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void OverrideDriverRespected()
        {
            var driver = AppiumDriverFactory.GetDefaultMobileDriver();
            this.TestObject.OverrideWebDriver(driver);

            Assert.AreEqual(driver, this.AppiumDriver);
        }

        /// <summary>
        /// Make sure separate lazy mobile elements can interactions with separate drivers
        /// </summary>
        [TestMethod]
        public void SeparateLazyElementInteractions()
        {
            MobileDriverManager newDriver = new MobileDriverManager(() => AppiumDriverFactory.GetDefaultMobileDriver(), this.TestObject);
            newDriver.GetMobileDriver().Navigate().GoToUrl("https://magenicautomation.azurewebsites.net/");
            this.ManagerStore.Add("test", newDriver);

            this.TestObject.AppiumDriver.Navigate().GoToUrl("https://magenicautomation.azurewebsites.net/Automation");

            LazyMobileElement topNew = new LazyMobileElement(this.TestObject, newDriver.GetMobileDriver(), By.CssSelector("*"));
            LazyMobileElement topDefault = new LazyMobileElement(this.TestObject, By.CssSelector("*"));

            Assert.AreNotEqual(topNew.Text, topDefault.Text);
        }

        /// <summary>
        /// Override driver function respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void OverrideDriverFuncRespected()
        {
            var driver = AppiumDriverFactory.GetDefaultMobileDriver();
            this.TestObject.OverrideWebDriver(() => driver);

            Assert.AreEqual(driver, this.AppiumDriver);
        }

        /// <summary>
        /// Is the driver manager get respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetRespected()
        {
            Assert.AreEqual(this.TestObject.GetDriverManager<MobileDriverManager>().Get(), this.AppiumDriver);
        }
    }
}