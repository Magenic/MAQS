//--------------------------------------------------
// <copyright file="SeleniumConfigTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test class for config files</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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

            Assert.IsNotNull(driver);
            driver.Close();
            driver.Dispose();
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

            Assert.IsTrue(driverName.Equals("PhantomJS", StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// resize browser window to specified lengths
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ResizeBrowserWindow()
        {
            IWebDriver driver = SeleniumConfig.Browser();

            if (Config.GetValue("BrowserSize") == "MAXIMIZE")
            {
                Assert.AreEqual(1056, driver.Manage().Window.Size.Height);
                Assert.AreEqual(1936, driver.Manage().Window.Size.Width);
            }
            else if (Config.GetValue("BrowserSize") == "DEFAULT")
            {
                Assert.AreEqual(1020, driver.Manage().Window.Size.Height);
                Assert.AreEqual(945, driver.Manage().Window.Size.Width);
            }
            else
            {
                Assert.AreNotEqual(1056, driver.Manage().Window.Size.Height);
                Assert.AreNotEqual(1020, driver.Manage().Window.Size.Height);
                Assert.AreNotEqual(1936, driver.Manage().Window.Size.Width);
                Assert.AreNotEqual(945, driver.Manage().Window.Size.Width);
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
                Assert.IsTrue(e.InnerException.Message.Contains("Sauce_Labs_Username"), "Did not see 'Sauce_Labs_Username' in error message: " + e.Message);
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

            IWebDriver driver = SeleniumConfig.Browser("phantomjs");

            #endregion GetBrowserWithString

            Assert.IsNotNull(driver);
            driver.Close();
            driver.Dispose();
        }

        /// <summary>
        /// Verify that correct exception is returned when unrecognizable remote browser is found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception))]
        public void GetBrowserRemoteThrowException()
        {
            try
            {
                Config.AddTestSettingValues(
                    new Dictionary<string, string>
                    {
                        { "HubUrl", "http://localhost:4444/wd/hub" },
                        { "RemoteBrowser", "NoBrowser" }
                    },
                    true);
                IWebDriver driver = SeleniumConfig.Browser("remote");
            }
            finally
            {
                // Set the value back to default value, since value can't be removed from the dictionary
                // Only one other test checks the value of the RemoteBrowser (GetRemoteBrowserName), so hopefully there won't be any parallel conflicts
                Config.AddTestSettingValues(new Dictionary<string, string> { { "RemoteBrowser", "Chrome" } }, true);
            }
        }

        /// <summary>
        /// Get wait driver test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriver()
        {
            #region WaitDriver

            WebDriverWait wait = SeleniumConfig.GetWaitDriver(SeleniumConfig.Browser());

            #endregion WaitDriver

            Assert.AreEqual(wait.Timeout.Seconds, 10);
            Assert.AreEqual(wait.PollingInterval.Seconds, 1);
        }

        /// <summary>
        /// Verifies that SetTimeouts sets driver timeouts to equal the default values in the Config
        /// </summary>
        #region SetTimeouts
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetTimeoutsGetWaitDriver()
        {
            SeleniumConfig.SetTimeouts(SeleniumConfig.Browser());
            WebDriverWait wait = SeleniumConfig.GetWaitDriver(SeleniumConfig.Browser());
            Assert.AreEqual(wait.Timeout.TotalMilliseconds.ToString(), Config.GetValue("Timeout", "0"));
        }
        #endregion
    }
}