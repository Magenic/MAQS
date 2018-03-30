//--------------------------------------------------
// <copyright file="AppiumConfigTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AppiumUnitTests
{
    using OpenQA.Selenium;

    /// <summary>
    /// Appium Config Unit Tests
    /// </summary>
    [TestClass]
    public class AppiumConfigTests
    {
        /// <summary>
        /// Test for getting Mobile Device OS
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetPlatformNameTest()
        {
            Assert.AreEqual(AppiumConfig.GetPlatformName(), "Android");
        }
        
        /// <summary>
        /// Test for getting mobile OS version
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetPlatformVersionTest()
        {
            Assert.AreEqual(AppiumConfig.GetPlatformVersion(), "6.0");
        }

        /// <summary>
        /// Test for getting device name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetDeviceNameTest()
        {
            Assert.AreEqual(AppiumConfig.GetDeviceName(), "emulator-5554");
        }

        /// <summary>
        /// Get command timeout test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetCommandTimeout()
        {
            #region GetCommandTimeout

            TimeSpan initTimeout = AppiumConfig.GetCommandTimeout();

            #endregion GetCommandTimeout

            Assert.AreEqual(TimeSpan.FromSeconds(122).Ticks, initTimeout.Ticks);
        }

        /// <summary>
        /// Test for creating Mobile Device driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void MobileDeviceTest()
        {
                #region MobileDevice
                AppiumDriver<IWebElement> driver = AppiumConfig.MobileDevice();
            #endregion

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
        /// Test for getting Mobile Hub Url
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileHubUrlTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileHubUrl(), "http://ondemand.saucelabs.com:80/wd/hub");
        }

        /// <summary>
        /// Test for getting instance of Wait Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetWaitDriverTest()
        {
            AppiumDriver<IWebElement> driver = AppiumConfig.MobileDevice();
            WebDriverWait wait = AppiumConfig.GetWaitDriver(driver);
            try
            {
                Assert.IsNotNull(wait);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();
            }
        }
    }
}