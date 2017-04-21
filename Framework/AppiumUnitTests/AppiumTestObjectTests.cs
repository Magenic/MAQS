//--------------------------------------------------
// <copyright file="AppiumTestObjectTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            this.TestObject.AppiumDriver.CloseApp();
            this.TestObject.AppiumDriver.Quit();
            this.TestObject.AppiumDriver.Dispose();
        }
    }
}