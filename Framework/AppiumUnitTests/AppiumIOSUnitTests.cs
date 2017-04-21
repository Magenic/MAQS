//--------------------------------------------------
// <copyright file="AppiumIOSUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace AppiumUnitTests
{
    /// <summary>
    /// iOS related Appium tests
    /// </summary>
    [TestClass]
    public class AppiumIOSUnitTests : BaseAppiumTest
    {
        /// <summary>
        /// Tests the creation of the Appium iOS Driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void AppiumIOSDriverTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
            this.TestObject.AppiumDriver.CloseApp();
            this.TestObject.AppiumDriver.Quit();
            this.TestObject.AppiumDriver.Dispose();
        }

        /// <summary>
        /// Sets capabilities for testing the iOS Driver creation
        /// </summary>
        /// <returns>iOS instance of the Appium Driver</returns>
        protected override AppiumDriver<AppiumWebElement> GetMobileDevice()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(CapabilityType.Platform, "MAC");
            capabilities.SetCapability(CapabilityType.Version, "10.3");
            capabilities.SetCapability("deviceName", "iPhone 7");
            capabilities.SetCapability("bundleId", "com.teamtreehouse.Diary");
            capabilities.SetCapability("automationName", "XCUITest");
            capabilities.SetCapability(MobileCapabilityType.Udid, "0C0E26E7-966B-4C89-A765-32C5C997A456");
            return new IOSDriver<AppiumWebElement>(AppiumConfig.GetMobileHubUrl(), capabilities);
        }
    }
}
