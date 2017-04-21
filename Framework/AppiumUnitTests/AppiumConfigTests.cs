//--------------------------------------------------
// <copyright file="AppiumConfigTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;

namespace AppiumUnitTests
{
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
        public void GetMobileDeviceOSTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileDeviceOS(), "Android");
        }

        /// <summary>
        /// Test for getting Mobile Device UDID
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileDeviceUDIDTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileDeviceUDID(), "0123456789abcdef0123456789abcdef01234567");
        }

        /// <summary>
        /// Test for getting Bundle ID
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetBundleIdTest()
        {
            Assert.AreEqual(AppiumConfig.GetBundleId(), "org.tasks");
        }

        /// <summary>
        /// Test for getting mobile OS version
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetOSVersionTest()
        {
            Assert.AreEqual(AppiumConfig.GetOSVersion(), "7.1.1");
        }

        /// <summary>
        /// Test for getting device name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetDeviceNameTest()
        {
            Assert.AreEqual(AppiumConfig.GetDeviceName(), "Nexus 6P");
        }

        /// <summary>
        /// Test for getting mobile browser value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void UsingMobileBrowserTest()
        {
            Assert.AreEqual(AppiumConfig.UsingMobileBrowser(), false);
        }

        /// <summary>
        /// Test for creating Mobile Device driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void MobileDeviceTest()
        {
            #region MobileDevice
             AppiumDriver<AppiumWebElement> driver = AppiumConfig.MobileDevice();
            #endregion

            Assert.IsNotNull(driver);
            driver.CloseApp();
            driver.Quit();
            driver.Dispose();
        }

        /// <summary>
        /// Test for getting Mobile Hub Url
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetMobileHubUrlTest()
        {
            Assert.AreEqual(AppiumConfig.GetMobileHubUrl(), "http://qat-win81-pc:4444/wd/hub");
        }

        /// <summary>
        /// Test for getting instance of Wait Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetWaitDriverTest()
        {
            AppiumDriver<AppiumWebElement> driver = AppiumConfig.MobileDevice();
            WebDriverWait wait = AppiumConfig.GetWaitDriver(driver);

            Assert.IsNotNull(wait);
            driver.CloseApp();
            driver.Quit();
            driver.Dispose();
        }

        /// <summary>
        /// Test for getting AVD name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetAvdNameTest()
        {
            Assert.AreEqual(AppiumConfig.GetAvdName(), "Nexus-6P");
        }
    }
}