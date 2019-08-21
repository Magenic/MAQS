//--------------------------------------------------
// <copyright file="AppiumIosUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for ios related functions</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS;

namespace AppiumUnitTests
{
    /// <summary>
    /// iOS related Appium tests
    /// </summary>
    [TestClass]
    public class AppiumIosUnitTests : BaseAppiumTest
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

            options.AddAdditionalCapability("deviceName", "iPhone 8 Simulator");
            options.AddAdditionalCapability("deviceOrientation", "portrait");
            options.AddAdditionalCapability("platformVersion", "12.2");
            options.AddAdditionalCapability("platformName", "iOS");
            options.AddAdditionalCapability("browserName", "Safari");
            options.AddAdditionalCapability("username", "Partner_Magenic");
            options.AddAdditionalCapability("accessKey", Config.GetSection(ConfigSection.AppiumCapsMaqs)["accessKey"]);

            return new IOSDriver<IWebElement>(AppiumConfig.GetMobileHubUrl(), options, AppiumConfig.GetCommandTimeout());
        }
    }
}
