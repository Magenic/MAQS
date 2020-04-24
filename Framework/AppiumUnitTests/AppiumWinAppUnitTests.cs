//--------------------------------------------------
// <copyright file="AppiumWinAppUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for windows application driver related functions</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace AppiumUnitTests
{
    /// <summary>
    /// Windows application driver related Appium tests
    /// </summary>
    [TestClass]
    public class AppiumWinAppUnitTests : BaseAppiumTest
    {
        /// <summary>
        /// Tests the creation of the Appium Windows application driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        [Ignore] // Can currently only run WinAppDriver locally with our current setup
        public void AppiumWinAppDriverTest()
        {
            LazyMobileElement lazy = new LazyMobileElement(this.TestObject, By.XPath("//Button[@AutomationId=\"num7Button\"]"), "Seven");
           
            lazy.Click();
            Assert.IsTrue(lazy.Enabled, "Expect enabled");
            Assert.IsTrue(lazy.Displayed, "Expect displayed");
            Assert.IsTrue(lazy.ExistsNow, "Expect exists now");

            this.AppiumDriver.FindElementByName("Plus").Click();
            this.AppiumDriver.FindElement(By.Name("Three")).Click();
            this.AppiumDriver.FindElementByAccessibilityId("equalButton").Click();

            Assert.AreEqual("Display is 10", this.AppiumDriver.FindElementByAccessibilityId("CalculatorResults").GetAttribute("Name"));
        }

        /// <summary>
        /// Sets capabilities for testing the Windows application driver creation
        /// </summary>
        /// <returns>Windows application driver instance of the Appium Driver</returns>
        protected override AppiumDriver<IWebElement> GetMobileDevice()
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            options.AddAdditionalCapability(MobileCapabilityType.Udid, "0C0E26E7-966B-4C89-A765-32C5C997A456");
            return new WindowsDriver<IWebElement>(new Uri("http://127.0.0.1:4723"), options);
        }
    }
}
