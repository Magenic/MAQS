//--------------------------------------------------
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
            AppiumDriver driver = AppiumDriverFactory.GetDefaultMobileDriver();
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
            this.TestObject.GetDriverManager<AppiumDriverManager>().Dispose();
        }

        /// <summary>
        /// Override driver respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void OverrideDriverRespected()
        {
            var driver = AppiumDriverFactory.GetDefaultMobileDriver();
            this.TestObject.OverrideAppiumDriver(driver);
        }

        /// <summary>
        /// Make sure separate lazy mobile elements can interactions with separate drivers
        /// </summary>
        [TestMethod]
        public void SeparateLazyElementInteractions()
        {
            AppiumDriverManager newDriver = new AppiumDriverManager(() => AppiumDriverFactory.GetDefaultMobileDriver(), this.TestObject);
            newDriver.GetAppiumDriver().Navigate().GoToUrl("https://magenicautomation.azurewebsites.net/");
            this.ManagerStore.Add("test", newDriver);

            this.TestObject.AppiumDriver.Navigate().GoToUrl("https://magenicautomation.azurewebsites.net/Automation");

            LazyMobileElement topNew = new LazyMobileElement(this.TestObject, newDriver.GetAppiumDriver(), By.CssSelector("*"));
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
            this.TestObject.OverrideAppiumDriver(() => driver);

            Assert.AreEqual(driver, this.AppiumDriver);
        }

        /// <summary>
        /// Is the driver manager get respected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void GetRespected()
        {
            Assert.AreEqual(this.TestObject.GetDriverManager<AppiumDriverManager>().Get(), this.AppiumDriver);
        }
    }
}