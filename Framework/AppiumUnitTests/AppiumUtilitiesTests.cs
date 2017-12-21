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

        /// <summary>
        /// Verify SavePageSource works - Validating that the Page Source file was created
        /// </summary>
        #region SavePageSource
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceTest()
        {
            AppiumUtilities.SavePageSource(this.TestObject.AppiumDriver, this.Log);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string pageSourceFilelocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS.txt";

            Assert.IsTrue(File.Exists(pageSourceFilelocation), "Failed to find page source");
            File.Delete(pageSourceFilelocation);
        }
        #endregion

        /// <summary>
        /// Verify that SavePageSource properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceThrownException()
        {
            FileLogger tempLogger = new FileLogger();
            tempLogger.FilePath = "<>"; // illegal file path
            bool successfullyCaptured = AppiumUtilities.SavePageSource(this.AppiumDriver, tempLogger);

            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that SavePageSource creates Directory if it does not exist already 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SavePageSourceNoExistingDirectory()
        {
            string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, "TempTestDirectory", "TempTestFilePath");
            Assert.IsTrue(File.Exists(pageSourcePath), "Fail to find Page Source");
            File.Delete(pageSourcePath);
        }
    }
}