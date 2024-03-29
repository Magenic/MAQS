﻿//--------------------------------------------------
// <copyright file="AppiumUtilitiesTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for utility files</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace AppiumUnitTests
{
    /// <summary>
    /// Appium Utilities Unit Tests
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Appium)]
    public class AppiumUtilitiesTests : BaseAppiumTest
    {
        /// <summary>
        /// Verify we can create a Appium test object with a Appium driver
        /// </summary>
        [TestMethod]
        public void CreateAppiumTestObjectWithDriver()
        {
            AppiumTestObject newObject = new AppiumTestObject(this.AppiumDriver, this.TestObject.Log, "test");
            Assert.AreEqual(this.AppiumDriver, newObject.AppiumDriver);
            Assert.AreEqual(this.Log, newObject.Log);
        }

        /// <summary>
        /// Verify mobile options work when null capablites
        /// </summary>
        [TestMethod]
        public void SetMobileOptionsWithNullDictionary()
        {
            var options = AppiumDriverFactory.GetDefaultMobileOptions();
            var beforeOptions = options.ToDictionary();

            // Add null
            options.SetMobileOptions(null);

            // Makes sure we didn't add or remove options
            Assert.AreEqual(beforeOptions.Count, options.ToDictionary().Count);
        }

        /// <summary>
        /// Verify a bad create fails in the right way
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(WebDriverException))]
        public void BadCreate()
        {
            AppiumDriverFactory.CreateDriver(() => throw new AccessViolationException("Should fail"));
        }

        /// <summary>
        /// Verify CaptureScreenshot works - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        public void CaptureScreenshotTest()
        {
            AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject);
            string filePath = Path.ChangeExtension(((IFileLogger)this.Log).FilePath, ".Png");
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify CaptureScreenshot works with console logger - Validating that the screenshot was not created
        /// </summary>
        [TestMethod]
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
        /// Verify that CaptureScreenshot properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        public void CaptureScreenshotThrownException()
        {
            FileLogger tempLogger = new FileLogger
            {
                FilePath = "\\<>/" // illegal file path
            };

            this.TestObject.Log = tempLogger;
            bool successfullyCaptured = AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject);
            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify when a screenshot is captured it is associated to the test object
        /// </summary>
        [TestMethod]
        public void CaptureScreenshotTestObjectAssociation()
        {
            AppiumUtilities.CaptureScreenshot(this.TestObject.AppiumDriver, this.TestObject);
            string filePath = Path.ChangeExtension(((IFileLogger)this.Log).FilePath, ".Png");
            Assert.IsTrue(this.TestObject.ContainsAssociatedFile(filePath), "Failed to find screenshot");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify SavePageSource works - Validating that the Page Source file was created
        /// </summary>
        [TestMethod]
        public void SavePageSourceTest()
        {
            AppiumUtilities.SavePageSource(this.TestObject.AppiumDriver, this.TestObject);
            string logLocation = ((IFileLogger)this.Log).FilePath;
            string pageSourceFilelocation = $"{logLocation.Substring(0, logLocation.LastIndexOf('.'))}_PS.txt";

            Assert.IsTrue(File.Exists(pageSourceFilelocation), "Failed to find page source");
            File.Delete(pageSourceFilelocation);
        }

        /// <summary>
        /// Verify that SavePageSource properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        public void SavePageSourceThrownException()
        {
            FileLogger tempLogger = new FileLogger
            {
                FilePath = "<>" // illegal file path
            };
            this.TestObject.Log = tempLogger;
            bool successfullyCaptured = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject);

            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that SavePageSource creates Directory if it does not exist already
        /// </summary>
        [TestMethod]
        public void SavePageSourceNoExistingDirectory()
        {
            string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject, "TempTestDirectory", "SavePSNoDir");
            Assert.IsTrue(File.Exists(pageSourcePath), "Fail to find Page Source");
            File.Delete(pageSourcePath);
        }

        /// <summary>
        /// Verify when a page source is saved it is associated to the test object
        /// </summary>
        [TestMethod]
        public void SavedPageSourceTestObjectAssociation()
        {
            string pageSourcePath = AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject, "TempTestDirectory", "TestObjAssoc");
            Assert.IsTrue(this.TestObject.ContainsAssociatedFile(pageSourcePath), "Failed to find page source");
            File.Delete(pageSourcePath);
        }

        /// <summary>
        /// Test lazy element
        /// </summary>
        [TestMethod]
        public void AppiumLazyTest()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver);
            this.AppiumDriver.Navigate().GoToUrl(Config.GetValueForSection(ConfigSection.AppiumMaqs, "WebSiteBase"));
            LazyMobileElement lazy = new LazyMobileElement(this.TestObject, By.XPath("//button[@class=\"navbar-toggle\"]"), "Nav toggle");

            Assert.IsTrue(lazy.Enabled, "Expect enabled");
            Assert.IsTrue(lazy.Displayed, "Expect displayed");
            Assert.IsTrue(lazy.ExistsNow, "Expect exists now");
            lazy.Click();

            LazyMobileElement missing = new LazyMobileElement(this.TestObject, By.XPath("//button[@class=\"Missing\"]"), "Missing");
            this.AppiumDriver.SetWaitDriver(new WebDriverWait(this.AppiumDriver, TimeSpan.FromSeconds(10)));

            Assert.IsFalse(missing.Exists, "Expect element not to exist");
        }

        /// <summary>
        /// Test lazy parent element and finds
        /// </summary>
        [TestMethod]
        public void AppiumLazyParentTest()
        {
            this.AppiumDriver.Navigate().GoToUrl(Config.GetValueForSection(ConfigSection.AppiumMaqs, "WebSiteBase"));
            LazyMobileElement parent = new LazyMobileElement(this.TestObject, By.XPath("//*[@class=\"jumbotron\"]"), "Parent");
            LazyMobileElement child = new LazyMobileElement(this.TestObject, By.XPath("//H2"), "Child");
            LazyMobileElement missingChild = new LazyMobileElement(this.TestObject, By.XPath("//Missing"), "Missing");

            this.SoftAssert.Assert(() => Assert.AreEqual(child.Text, parent.FindElement(child.By, "Child").Text));
            this.SoftAssert.Assert(() => Assert.AreEqual(1, parent.FindElements(child.By, "Child").Count), "Name1");
            this.SoftAssert.Assert(() => Assert.IsTrue(child.Exists), "Expect exists now", "Name2");

            // Override the timeout
            this.AppiumDriver.SetWaitDriver(new WebDriverWait(this.AppiumDriver, TimeSpan.FromSeconds(10)));

            this.SoftAssert.Assert(() => Assert.IsFalse(missingChild.Exists), "Expect element not to exist");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Test lazy element wait overrides
        /// </summary>
        [TestMethod]
        public void AppiumLazyWaitOverride()
        {
            TimeSpan overrideTimeSpan = TimeSpan.FromSeconds(10);
            LazyMobileElement parent = new LazyMobileElement(this.TestObject, By.XPath("//*[@class=\"jumbotron\"]"), "Parent");
            LazyMobileElement child = new LazyMobileElement(this.TestObject, By.XPath("//H2"), "Child");

            this.AppiumDriver.SetWaitDriver(new WebDriverWait(this.AppiumDriver, overrideTimeSpan));

            this.SoftAssert.Assert(() => Assert.AreEqual(overrideTimeSpan, parent.WaitDriver().Timeout), "Name1", "Parent wait override was not respected");
            this.SoftAssert.Assert(() => Assert.AreEqual(overrideTimeSpan, child.WaitDriver().Timeout), "Name2", "Child wait override was not respected");
            this.SoftAssert.FailTestIfAssertFailed();
        }
    }
}
