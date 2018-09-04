//--------------------------------------------------
// <copyright file="SeleniumConfigTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace UnitTests
{
    /// <summary>
    /// Test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumConfigTests
    {
        /// <summary>
        /// Browser check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetBrowser()
        {
            #region GetBrowser

            IWebDriver driver = SeleniumConfig.Browser();

            #endregion GetBrowser
            try
            {
                Assert.IsNotNull(driver);
            }
            finally
            {
                driver?.KillDriver();
            }
        }

        /// <summary>
        /// Browser name
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetBrowserName()
        {
            #region GetBrowserName

            string driverName = SeleniumConfig.GetBrowserName();

            #endregion GetBrowserName

            Assert.IsTrue(driverName.Equals("HeadlessChrome", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Verify resize browser window to Maximum lengths
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ResizeBrowserWindowMaximize()
        {
            /* 
             * Create driver at whatever size
             * Manually maximize the window
             * Override the Config BrowserSize value to MAXIMIZE
             * Verify new and old driver size values are the same
             */

            // Using PhantomJS because headless Chrome does not respect Maximize as of 8/24/2018
            var driverManualSize = SeleniumConfig.Browser("PhantomJS");

            try
            {
                driverManualSize.Manage().Window.Maximize();

                var manuallyMaximizedWidth = driverManualSize.Manage().Window.Size.Width;
                var manuallyMaximizedHidth = driverManualSize.Manage().Window.Size.Height;

                Config.AddTestSettingValues(
                   new Dictionary<string, string>
                   {
                        { "BrowserSize", "MAXIMIZE" }
                   },
                   "SeleniumMaqs",
                   true);

                var driverConfigSize = SeleniumConfig.Browser("PhantomJS");

                try
                {
                    Assert.AreEqual(manuallyMaximizedWidth, driverConfigSize.Manage().Window.Size.Width);
                    Assert.AreEqual(manuallyMaximizedHidth, driverConfigSize.Manage().Window.Size.Height);
                }
                finally
                {
                    driverConfigSize?.KillDriver();
                }
            }
            finally
            {
                driverManualSize?.KillDriver();
            }
        }

        /// <summary>
        /// Verify resize browser window to Default lengths
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [DoNotParallelize]
        public void ResizeBrowserWindowDefault()
        {
            /* 
             * Create driver at default size, 
             * Set the driver window so that it is not at a default size
             * Create a new browser at default size and verify it is created at the default size and not changed size
             */
            Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "BrowserSize", "DEFAULT" }
                    },
                   "SeleniumMaqs",
                    true);

            var driverChangeSize = SeleniumConfig.Browser();

            try
            {
                var defaultWidth = driverChangeSize.Manage().Window.Size.Width;
                var defaultHidth = driverChangeSize.Manage().Window.Size.Height;
                var nonDefaultWidth = defaultWidth + 1;
                var nonDefaultHidth = defaultHidth + 1;

                driverChangeSize.Manage().Window.Size = new System.Drawing.Size(nonDefaultWidth, nonDefaultHidth);

                var driverDefault = SeleniumConfig.Browser();

                try
                {
                    Assert.AreEqual(defaultWidth, driverDefault.Manage().Window.Size.Width);
                    Assert.AreEqual(defaultHidth, driverDefault.Manage().Window.Size.Height);
                }
                finally
                {
                    driverDefault?.KillDriver();
                }
            }
            finally
            {
                driverChangeSize?.KillDriver();
            }
        }

        /// <summary>
        /// Verify resize browser window to custom lengths 1024x768
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [DoNotParallelize]
        public void ResizeBrowserWindowCustom()
        {
            var expectedWidth = 1024;
            var expectedHeight = 768;

            Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "BrowserSize", $"{expectedWidth}x{expectedHeight}" }
                    },
                   "SeleniumMaqs",
                    true);

            var driver = SeleniumConfig.Browser();

            try
            {
                Assert.AreEqual(expectedWidth, driver.Manage().Window.Size.Width);
                Assert.AreEqual(expectedHeight, driver.Manage().Window.Size.Height);
            }
            finally
            {
                driver?.KillDriver();
            }
        }

        /// <summary>
        /// Web site base
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWebsiteBase()
        {
            #region GetWebsiteBase

            string website = SeleniumConfig.GetWebSiteBase();

            #endregion GetWebsiteBase

            Assert.IsTrue(website.Equals("http://magenicautomation.azurewebsites.net/", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Driver hint path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetDriverHintPath()
        {
            #region GetDriverHintPath

            string path = SeleniumConfig.GetDriverHintPath();

            #endregion GetDriverHintPath

            Assert.AreEqual(path, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        /// <summary>
        /// Remote browser name test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetRemoteBrowserName()
        {
            #region GetRemoteName

            string browser = SeleniumConfig.GetRemoteBrowserName();

            #endregion GetRemoteName

            Assert.AreEqual(browser, "Chrome");
        }

        /// <summary>
        /// Get command timeout test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetCommandTimeout()
        {
            #region GetCommandTimeout

            TimeSpan initTimeout = SeleniumConfig.GetCommandTimeout();

            #endregion GetCommandTimeout

            Assert.AreEqual(TimeSpan.FromSeconds(61).Ticks, initTimeout.Ticks);
        }

        /// <summary>
        /// Testing of the RemoteCapabilityMaqs section of the app.config file
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [DoNotParallelize]
        public void RemoteCapabilitySectionRespected()
        {
            IWebDriver driver = null;

            try
            {
                driver = SeleniumConfig.Browser("Remote");
                driver.Navigate().GoToUrl("https://magenic.com/");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException.Message.Contains("Sauce_Labs_Username"), "Did not see 'Sauce_Labs_Username' in error message: " + e.Message + " -- " + e.InnerException.Message);
            }
            finally
            {
                // Cleanup if we need to
                if (driver != null)
                {
                    driver.Quit();
                }
            }
        }

        /// <summary>
        /// Remote platform test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetRemotePlatform()
        {
            #region RemotePlatform

            string platform = SeleniumConfig.GetRemotePlatform();

            #endregion RemotePlatform

            Assert.AreEqual(platform, string.Empty);
        }

        /// <summary>
        /// Remote browser version
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetRemoteBrowserVersion()
        {
            #region RemoteVersion

            string version = SeleniumConfig.GetRemoteBrowserVersion();

            #endregion RemoteVersion

            Assert.AreEqual(version, string.Empty);
        }

        /// <summary>
        /// Browser with string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetBrowserWithString()
        {
            #region GetBrowserWithString

            IWebDriver driver = SeleniumConfig.Browser("HeadlessChrome");

            #endregion GetBrowserWithString            

            try
            {
                Assert.IsNotNull(driver);
            }
            finally
            {
                driver?.KillDriver();
            }
        }

        /// <summary>
        /// Verify that correct exception is returned when unrecognizable remote browser is found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(ArgumentException))]
        [DoNotParallelize]
        public void GetBrowserRemoteThrowException()
        {
            string hubUrl = Config.GetGeneralValue("HubUrl");
            string remoteBrowser = Config.GetGeneralValue("RemoteBrowser");

            try
            {
                Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "HubUrl", "http://localhost:4444/wd/hub" },
                        { "RemoteBrowser", "NoBrowser" }
                    },
                   "SeleniumMaqs",
                    true);
                IWebDriver driver = SeleniumConfig.Browser("remote");
            }
            finally
            {
                Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "HubUrl", hubUrl },
                        { "RemoteBrowser", remoteBrowser }
                    },
                   "SeleniumMaqs",
                    true);
            }
        }

        #region WaitDriver 
        /// <summary>
        /// Get wait driver test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriver()
        {
            IWebDriver driver = SeleniumConfig.Browser();

            try
            {
                WebDriverWait wait = SeleniumConfig.GetWaitDriver(driver);
                Assert.AreEqual(20, wait.Timeout.Seconds);
                Assert.AreEqual(1, wait.PollingInterval.Seconds);
            }
            finally
            {
                driver?.KillDriver();
            }
        }
        #endregion WaitDriver

        /// <summary>
        /// Verifies that SetTimeouts sets driver timeouts to equal the default values in the Config
        /// </summary>
        #region SetTimeouts
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetTimeoutsGetWaitDriver()
        {
            var driver = SeleniumConfig.Browser();
            var newDriver = SeleniumConfig.Browser();
            try
            {
                SeleniumConfig.SetTimeouts(driver);
                WebDriverWait wait = SeleniumConfig.GetWaitDriver(newDriver);
                Assert.AreEqual(wait.Timeout.TotalMilliseconds.ToString(), Config.GetValueForSection("SeleniumMaqs", "BrowserTimeout", "0"));
            }
            finally
            {
                try
                {
                    driver?.KillDriver();
                }
                finally
                {
                    newDriver?.KillDriver();
                }
            }
        }
        #endregion
    }
}