//--------------------------------------------------
// <copyright file="AppiumUtilitiesTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for utility files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppiumUnitTests
{
    /// <summary>
    /// Appium Utilities Unit Tests
    /// </summary>
    [TestClass]
    public class AppiumUtilitiesTests : BaseAppiumTest
    {
        /// <summary>
        /// Test for capturing a screenshot
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotTest()
        {
            Assert.IsTrue(AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject.Log));
        }
    }
}