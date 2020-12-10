//--------------------------------------------------
// <copyright file="SeleniumConfigTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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

namespace SeleniumUnitTests
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
            IWebDriver driver = WebDriverFactory.GetDefaultBrowser();

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
            string driverName = SeleniumConfig.GetBrowserName();

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

            // Using FireFox because headless Chrome does not respect Maximize as of 8/24/2018
            var driverManualSize = WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.Firefox);

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

                var driverConfigSize = WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.Firefox);

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

            var driverChangeSize = WebDriverFactory.GetDefaultBrowser();

            try
            {
                var defaultWidth = driverChangeSize.Manage().Window.Size.Width;
                var defaultHidth = driverChangeSize.Manage().Window.Size.Height;
                var nonDefaultWidth = defaultWidth + 1;
                var nonDefaultHidth = defaultHidth + 1;

                driverChangeSize.Manage().Window.Size = new System.Drawing.Size(nonDefaultWidth, nonDefaultHidth);

                var driverDefault = WebDriverFactory.GetDefaultBrowser();

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

            var driver = WebDriverFactory.GetDefaultBrowser();

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
            string website = SeleniumConfig.GetWebSiteBase();

            Assert.IsTrue(website.Equals("https://magenicautomation.azurewebsites.net/", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Driver hint path
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetDriverHintPath()
        {
            string path = SeleniumConfig.GetDriverHintPath();

            Assert.AreEqual(path, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        /// <summary>
        /// Remote browser name test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetRemoteBrowserName()
        {
            string browser = SeleniumConfig.GetRemoteBrowserName();

            Assert.AreEqual("Chrome", browser);
        }

        /// <summary>
        /// Get command timeout test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetCommandTimeout()
        {
            TimeSpan initTimeout = SeleniumConfig.GetCommandTimeout();

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
            var checkedAssertion = false;
            IWebDriver driver = null;
            var username = Config.GetValueForSection(ConfigSection.RemoteSeleniumCapsMaqs, "username");
            try
            {
                Config.AddTestSettingValues(new Dictionary<string, string> { ["username"] = "Sauce_Labs_Username" }, ConfigSection.RemoteSeleniumCapsMaqs, true);
                driver = WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.Remote);
                driver.Navigate().GoToUrl("https://magenic.com/");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.InnerException.Message.Contains("Sauce_Labs_Username"), "Did not see 'Sauce_Labs_Username' in error message: " + e.Message + " -- " + e.InnerException.Message);
                checkedAssertion = true;
            }
            finally
            {
                // Cleanup if we need to
                if (driver != null)
                {
                    driver.Quit();
                }
                Config.AddTestSettingValues(new Dictionary<string, string> { ["username"] = username }, ConfigSection.RemoteSeleniumCapsMaqs, true);
            }
            Assert.IsTrue(checkedAssertion, "Did not receive the error message on inner exception");
        }

        /// <summary>
        /// Remote platform test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetRemotePlatform()
        {
            string platform = SeleniumConfig.GetRemotePlatform();

            Assert.AreEqual(platform, string.Empty);
        }

        /// <summary>
        /// Remote browser version
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetRemoteBrowserVersion()
        {
            string version = SeleniumConfig.GetRemoteBrowserVersion();

            Assert.AreEqual(version, string.Empty);
        }

        /// <summary>
        /// Browser with string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetBrowserWithString()
        {
            IWebDriver driver = WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.HeadlessChrome);

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

                WebDriverFactory.GetBrowserWithDefaultConfiguration(BrowserType.Remote);
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

        /// <summary>
        /// Get wait driver test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriver()
        {
            IWebDriver driver = WebDriverFactory.GetDefaultBrowser();

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

        /// <summary>
        /// Verifies that SetTimeouts sets driver timeouts to equal the default values in the Config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetTimeoutsGetWaitDriver()
        {
            var driver = WebDriverFactory.GetDefaultBrowser();
            var newDriver = WebDriverFactory.GetDefaultBrowser();
            try
            {
                SeleniumUtilities.SetTimeouts(driver);
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

        /// <summary>
        /// Get expected browser types
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void BrowserTypeMatch()
        {
            Assert.AreEqual(BrowserType.IE, SeleniumConfig.GetBrowserType("INTERNET EXPLORER"));
            Assert.AreEqual(BrowserType.IE, SeleniumConfig.GetBrowserType("INTERNETExPLOREr"));
            Assert.AreEqual(BrowserType.IE, SeleniumConfig.GetBrowserType("ie"));
            Assert.AreEqual(BrowserType.Firefox, SeleniumConfig.GetBrowserType("Firefox"));
            Assert.AreEqual(BrowserType.Chrome, SeleniumConfig.GetBrowserType("chrome"));
            Assert.AreEqual(BrowserType.HeadlessChrome, SeleniumConfig.GetBrowserType("HEADLESSCHROME"));
            Assert.AreEqual(BrowserType.Edge, SeleniumConfig.GetBrowserType("eDGE"));
            Assert.AreEqual(BrowserType.Remote, SeleniumConfig.GetBrowserType("remote"));
        }

        /// <summary>
        /// Expected browser types error check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void BrowserTypeError()
        {
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetBrowserType("PHANTOMJS"));
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetBrowserType("PHANTOM JS"));
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetBrowserType("PHANTOM"));
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetBrowserType("OTHER"));
        }

        /// <summary>
        /// Get expected remote browser types
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void RemoteBrowserTypeMatch()
        {
            Assert.AreEqual(RemoteBrowserType.IE, SeleniumConfig.GetRemoteBrowserType("INTERNET EXPLORER"));
            Assert.AreEqual(RemoteBrowserType.IE, SeleniumConfig.GetRemoteBrowserType("INTERNETExPLOREr"));
            Assert.AreEqual(RemoteBrowserType.IE, SeleniumConfig.GetRemoteBrowserType("ie"));
            Assert.AreEqual(RemoteBrowserType.Firefox, SeleniumConfig.GetRemoteBrowserType("Firefox"));
            Assert.AreEqual(RemoteBrowserType.Chrome, SeleniumConfig.GetRemoteBrowserType("chrome"));
            Assert.AreEqual(RemoteBrowserType.Safari, SeleniumConfig.GetRemoteBrowserType("SAFARI"));
            Assert.AreEqual(RemoteBrowserType.Edge, SeleniumConfig.GetRemoteBrowserType("edge"));
        }

        /// <summary>
        /// Expected remote browser types error check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void RemoteBrowserTypeError()
        {
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetRemoteBrowserType("remote"));
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetRemoteBrowserType("HEADLESSCHROME"));
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetRemoteBrowserType("PHANTOM"));
            Assert.ThrowsException<ArgumentException>(() => SeleniumConfig.GetRemoteBrowserType("OTHER"));
        }

        /// <summary>
        /// Get expected UseProxy configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetUseProxy()
        {
            Assert.IsFalse(SeleniumConfig.GetUseProxy());
        }

        /// <summary>
        /// Get expected proxy address configuration
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetProxyAddress()
        {
            Assert.AreEqual("http://localhost:8002", SeleniumConfig.GetProxyAddress());
        }
    }
}