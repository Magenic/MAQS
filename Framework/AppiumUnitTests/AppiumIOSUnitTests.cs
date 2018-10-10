//--------------------------------------------------
// <copyright file="AppiumIOSUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for ios related functions</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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
        }

        /// <summary>
        /// Sets capabilities for testing the iOS Driver creation
        /// </summary>
        /// <returns>iOS instance of the Appium Driver</returns>
        protected override AppiumDriver<IWebElement> GetMobileDevice()
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability(CapabilityType.Platform, "MAC");
            options.AddAdditionalCapability(CapabilityType.Version, "10.3");
            options.AddAdditionalCapability("deviceName", "iPhone 7");
            options.AddAdditionalCapability("bundleId", "com.teamtreehouse.Diary");
            options.AddAdditionalCapability("automationName", "XCUITest");
            options.AddAdditionalCapability(MobileCapabilityType.Udid, "0C0E26E7-966B-4C89-A765-32C5C997A456");
            return new IOSDriver<IWebElement>(AppiumConfig.GetMobileHubUrl(), options);
        }
    }
}
