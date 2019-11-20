//--------------------------------------------------
// <copyright file="AppiumUtilitiesTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for utility files</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
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
            AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject);
            string filePath = Path.ChangeExtension(((FileLogger)this.Log).FilePath, ".png");
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify deprecated CaptureScreenshot works - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotTestDeprecated()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.Log);
#pragma warning restore CS0618 // Type or member is obsolete
            string filePath = Path.ChangeExtension(((FileLogger)this.Log).FilePath, ".png");
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify CaptureScreenshot works with console logger - Validating that the screenshot was not created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotWithConsoleLoggerTest()
        {
            // Create a console logger and calculate the file location
            ConsoleLogger consoleLogger = new ConsoleLogger();
            this.TestObject.Log = consoleLogger;

            // Take a screenshot
            bool success = AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject, "Delete");

            // Make sure we didn't take the screenshot
            Assert.IsFalse(success, "Screenshot taken with console logger");
        }

        /// <summary>
        /// Verify deprecated CaptureScreenshot works with console logger - Validating that the screenshot was not created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotWithConsoleLoggerTestDeprecated()
        {
            // Create a console logger and calculate the file location
            ConsoleLogger consoleLogger = new ConsoleLogger();

            // Take a screenshot
#pragma warning disable CS0618 // Type or member is obsolete
            bool success = AppiumUtilities.CaptureScreenshot(TestObject.AppiumDriver, consoleLogger, "Delete");
#pragma warning restore CS0618 // Type or member is obsolete

            // Make sure we didn't take the screenshot
            Assert.IsFalse(success, "Screenshot taken with console logger");
        }

        /// <summary>
        /// Verify that CaptureScreenshot properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotThrownException()
        {
            FileLogger tempLogger = new FileLogger
            {
                FilePath = "<>" // illegal file path
            };

            this.TestObject.Log = tempLogger;
            bool successfullyCaptured = AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject);
            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that deprecated CaptureScreenshot properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotThrownExceptionDeprecated()
        {
            FileLogger tempLogger = new FileLogger
            {
                FilePath = "<>" // illegal file path
            };

#pragma warning disable CS0618 // Type or member is obsolete
            bool successfullyCaptured = AppiumUtilities.CaptureScreenshot(TestObject.AppiumDriver, tempLogger);
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify when a screenshot is captured it is associated to the test object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void CaptureScreenshotTestObjectAssociation()
        {
            AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject);
            string filePath = Path.ChangeExtension(((FileLogger)this.Log).FilePath, ".png");
            Assert.IsTrue(this.TestObject.ContainsAssociatedFile(filePath), "Failed to find screenshot");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify SavePageSource works - Validating that the Page Source file was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceTest()
        {
            AppiumUtilities.SavePageSource(this.TestObject.AppiumDriver, this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string pageSourceFilelocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS.txt";

            Assert.IsTrue(File.Exists(pageSourceFilelocation), "Failed to find page source");
            File.Delete(pageSourceFilelocation);
        }

        /// <summary>
        /// Verify deprecated SavePageSource works - Validating that the Page Source file was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceTestDeprecated()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            AppiumUtilities.SavePageSource(this.TestObject.AppiumDriver, this.Log);
#pragma warning restore CS0618 // Type or member is obsolete
            string logLocation = ((FileLogger)this.Log).FilePath;
            string pageSourceFilelocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS.txt";

            Assert.IsTrue(File.Exists(pageSourceFilelocation), "Failed to find page source");
            File.Delete(pageSourceFilelocation);
        }

        /// <summary>
        /// Verify that SavePageSource properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceThrownException()
        {
            FileLogger tempLogger = new FileLogger();
            tempLogger.FilePath = "<>"; // illegal file path
            this.TestObject.Log = tempLogger;
            bool successfullyCaptured = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject);

            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that deprecated SavePageSource properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceThrownExceptionDeprecated()
        {
            FileLogger tempLogger = new FileLogger();
            tempLogger.FilePath = "<>"; // illegal file path
#pragma warning disable CS0618 // Type or member is obsolete
            bool successfullyCaptured = AppiumUtilities.SavePageSource(this.AppiumDriver, tempLogger);
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that SavePageSource creates Directory if it does not exist already 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceNoExistingDirectory()
        {
            string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject, "TempTestDirectory", "SavePSNoDir");
            Assert.IsTrue(File.Exists(pageSourcePath), "Fail to find Page Source");
            File.Delete(pageSourcePath);
        }

        /// <summary>
        /// Verify that deprecated SavePageSource creates Directory if it does not exist already 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavePageSourceNoExistingDirectoryDeprecated()
        {
#pragma warning disable CS0618 // Type or member is obsolete
            string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, "TempTestDirectory", "OldSavePSNoDir");
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.IsTrue(File.Exists(pageSourcePath), "Fail to find Page Source");
            File.Delete(pageSourcePath);
        }

        /// <summary>
        /// Verify when a page source is saved it is associated to the test object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Appium)]
        public void SavedPageSourceTestObjectAssociation()
        {
            string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject, "TempTestDirectory", "TestObjAssoc");
            Assert.IsTrue(this.TestObject.ContainsAssociatedFile(pageSourcePath), "Failed to find page source");
            File.Delete(pageSourcePath);
        }
    }
}