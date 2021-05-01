//-----------------------------------------------------
// <copyright file="WebDriverFactoryUnitTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test the WebDriverFactory</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the WebDriverFactory class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebDriverFactoryUnitTests
    {
        /// <summary>
        /// Verify SetProxySettings sets the proxy in the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetProxySettings()
        {
            ChromeOptions options = new ChromeOptions();
            options.SetProxySettings(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "ProxyAddress"));

            Assert.IsNotNull(options.Proxy);
            Assert.AreEqual("http://localhost:8002", options.Proxy.HttpProxy);
            Assert.AreEqual("http://localhost:8002", options.Proxy.SslProxy);
        }

        /// <summary>
        /// Validating we can get the default IE options
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetDefaultIEOptions()
        {
            var options = WebDriverFactory.GetDefaultIEOptions();
            Assert.IsNotNull(options, "Was unable to retrieve options for IE Options");
        }

        /// <summary>
        /// Validating we can get the default Edge options
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetDefaultEdgeOptions()
        {
            var options = WebDriverFactory.GetDefaultEdgeOptions();
            Assert.IsNotNull(options, "Was unable to retrieve options for Edge Options");
        }

        /// <summary>
        /// Validating we can get the default Chrome options
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetDefaultChromeOptions()
        {
            var options = WebDriverFactory.GetDefaultChromeOptions();
            Assert.IsNotNull(options, "Was unable to retrieve options for Chrome Options");
        }

        /// <summary>
        /// Validating we can get the default remote options
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetDefaultRemoteOptions()
        {
            var options = WebDriverFactory.GetRemoteOptions(RemoteBrowserType.Chrome);
            Assert.IsNotNull(options, "Was unable to retrieve remote options");
        }

        /// <summary>
        /// Validating we can get the default remote options
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception))]
        public void CreateInvalidDriver()
        {
            WebDriverFactory.CreateDriver(null);
        }

        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetAllRemoteOptions()
        {
            RemoteBrowserType[] remoteBrowserTypes = (RemoteBrowserType[])Enum.GetValues(typeof(RemoteBrowserType));
            var remoteCapabilities = new Dictionary<string, object>();

            for (int i = 0; i < remoteBrowserTypes.Length; i++)
            {
                var options = WebDriverFactory.GetRemoteOptions(remoteBrowserTypes[i], "1.2", "2.2", remoteCapabilities);
                Assert.IsNotNull(options, $"Was unable to retrieve remote options for: {remoteBrowserTypes[i]}");
                Assert.IsTrue(options.ToString().ToLower().Contains(remoteBrowserTypes[i].ToString().ToLower()));
            }           
        }

        //[TestCategory(TestCategories.Selenium)]
        public void GetBrowserWithDefaultConfiguration()
        {
            //BrowserType[] browserTypes = (BrowserType[])Enum.GetValues(typeof(BrowserType));
            BrowserType[] browserTypes = {BrowserType.Chrome, BrowserType.Firefox, BrowserType.HeadlessChrome, BrowserType.IE, BrowserType.Remote };

            for (int i = 0; i < browserTypes.Length; i++)
            {
                var browser = WebDriverFactory.GetBrowserWithDefaultConfiguration(browserTypes[i]);
                Assert.IsNotNull(browser);
            }       
        }
    }
}
