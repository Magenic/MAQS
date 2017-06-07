//--------------------------------------------------
// <copyright file="AppiumUtilitiesTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for utility files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace AppiumUnitTests
{
    /// <summary>
    /// Appium Utilities Unit Tests
    /// </summary>
    [TestClass]
    public class AppiumUtilitiesTests : BaseAppiumTest
    {
        /// <summary>
        /// Verify CaptureScreenshot works - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotTest()
        {
            
            AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.Log);
            string filePath = Path.ChangeExtension(((FileLogger)this.Log).FilePath, ".png");
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            File.Delete(filePath);
        }

       
    }
}