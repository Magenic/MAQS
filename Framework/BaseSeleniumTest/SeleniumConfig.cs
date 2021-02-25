//--------------------------------------------------
// <copyright file="SeleniumConfig.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class SeleniumConfig
    {
        /// <summary>
        /// Loads when class is loaded
        /// </summary>
        static SeleniumConfig()
        {
            CheckConfig();
        }

        /// <summary>
        /// Ensure required fields are in the config
        /// </summary>
        private static void CheckConfig()
        {
            var validator = new ConfigValidation()
            {
                RequiredFields = new List<string>()
                {
                    "BrowserTimeout"
                }
            };
            Config.Validate(ConfigSection.SeleniumMaqs, validator);
        }

        /// <summary>
        ///  Static name for the Selenium configuration section
        /// </summary>
        private const string SELENIUMSECTION = "SeleniumMaqs";

        /// <summary>
        /// Get the browser type
        /// </summary>
        /// <returns>The browser type</returns>
        public static BrowserType GetBrowserType()
        {
            return GetBrowserType(GetBrowserName());
        }

        /// <summary>
        /// Get the browser type based on the provide browser name
        /// </summary>
        /// <param name="browserName">Name of the browser</param>
        /// <returns>The browser type</returns>
        public static BrowserType GetBrowserType(string browserName)
        {
            switch (browserName.ToUpper())
            {
                case "INTERNET EXPLORER":
                case "INTERNETEXPLORER":
                case "IE":
                    return BrowserType.IE;
                case "FIREFOX":
                    return BrowserType.Firefox;
                case "CHROME":
                    return BrowserType.Chrome;
                case "HEADLESSCHROME":
                    return BrowserType.HeadlessChrome;
                case "EDGE":
                    return BrowserType.Edge;
                case "REMOTE":
                    return BrowserType.Remote;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Browser type '{0}' is not supported", browserName));
            }
        }

        /// <summary>
        /// Get the remote browser type
        /// </summary>
        /// <returns>The remote browser type</returns>
        public static RemoteBrowserType GetRemoteBrowserType()
        {
            return GetRemoteBrowserType(GetRemoteBrowserName());
        }

        /// <summary>
        /// Get the remote browser type
        /// </summary>
        /// <param name="remoteBrowser">Name of the remote browser</param>
        /// <returns>The remote browser type</returns>
        public static RemoteBrowserType GetRemoteBrowserType(string remoteBrowser)
        {
            switch (remoteBrowser.ToUpper())
            {
                case "INTERNET EXPLORER":
                case "INTERNETEXPLORER":
                case "IE":
                    return RemoteBrowserType.IE;
                case "FIREFOX":
                    return RemoteBrowserType.Firefox;
                case "CHROME":
                    return RemoteBrowserType.Chrome;
                case "SAFARI":
                    return RemoteBrowserType.Safari;
                case "EDGE":
                    return RemoteBrowserType.Edge;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Remote browser type '{0}' is not supported", remoteBrowser));
            }
        }

        /// <summary>
        /// Get the browser type name - Example: Chrome
        /// </summary>
        /// <returns>The browser type</returns>
        public static string GetBrowserName()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "Browser", "Chrome");
        }

        /// <summary>
        /// Get the initialize Selenium timeout timespan
        /// </summary>
        /// <returns>The initialize timeout</returns>
        public static TimeSpan GetCommandTimeout()
        {
            string value = Config.GetValueForSection(SELENIUMSECTION, "SeleniumCommandTimeout", "60000");
            if (!int.TryParse(value, out int timeout))
            {
                throw new ArgumentException("SeleniumCommandTimeout should be a number but the current value is: " + value);
            }

            return TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Get the hint path for the web driver
        /// </summary>
        /// <returns>The hint path for the web driver</returns>
        public static string GetDriverHintPath()
        {
            string defaultPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Config.GetValueForSection(SELENIUMSECTION, "WebDriverHintPath", defaultPath);
        }

        /// <summary>
        /// Get the remote browser type name
        /// </summary>
        /// <returns>The browser type being used on grid</returns>
        public static string GetRemoteBrowserName()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "RemoteBrowser", "Chrome");
        }

        /// <summary>
        /// Get the remote browser version
        /// </summary>
        /// <returns>The browser version to run against on grid</returns>
        public static string GetRemoteBrowserVersion()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "RemoteBrowserVersion");
        }

        /// <summary>
        /// Get the remote platform type name
        /// </summary>
        /// <returns>The platform (or OS) to run remote tests against</returns>
        public static string GetRemotePlatform()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "RemotePlatform");
        }

        /// <summary>
        /// Get the hub Uri
        /// </summary>
        /// <returns>The hub Uri</returns>
        public static Uri GetHubUri()
        {
            return new Uri(Config.GetValueForSection(SELENIUMSECTION, "HubUrl"));
        }

        /// <summary>
        /// Get the wait default wait driver
        /// </summary>
        /// <param name="driver">Web driver to associate with the wait driver</param>
        /// <returns>An WebDriverWait</returns>
        public static WebDriverWait GetWaitDriver(IWebDriver driver)
        {
            return new WebDriverWait(new SystemClock(), driver, GetTimeoutTime(), GetWaitTime());
        }

        /// <summary>
        /// Get the web site base url
        /// </summary>
        /// <returns>The web site base url</returns>
        public static string GetWebSiteBase()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "WebSiteBase");
        }

        /// <summary>
        /// Get if we should save page source on fail
        /// </summary>
        /// <returns>True if we want to save page source on fail</returns>
        public static bool GetSavePagesourceOnFail()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "SavePagesourceOnFail").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get if we should save screenshots on soft alert fails
        /// </summary>
        /// <returns>True if we want to save screenshots on soft alert fails</returns>
        public static bool GetSoftAssertScreenshot()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "SoftAssertScreenshot").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get the format we want to capture screenshots with
        /// </summary>
        /// <returns>The desired format</returns>
        public static string GetImageFormat()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "ImageFormat", "PNG");
        }

        /// <summary>
        /// Get the remote capabilities as a dictionary
        /// </summary>
        /// <returns>Dictionary of remote capabilities</returns>
        public static Dictionary<string, string> GetRemoteCapabilitiesAsStrings()
        {
            return Config.GetSection(ConfigSection.RemoteSeleniumCapsMaqs);
        }

        /// <summary>
        /// Get the remote capabilities as a dictionary
        /// </summary>
        /// <returns>Dictionary of remote capabilities</returns>
        public static Dictionary<string, object> GetRemoteCapabilitiesAsObjects()
        {
            return GetRemoteCapabilitiesAsStrings().ToDictionary(pair => pair.Key, pair => (object)pair.Value);
        }

        /// <summary>
        /// Get the wait timespan
        /// </summary>
        /// <returns>The wait time span</returns>
        public static TimeSpan GetWaitTime()
        {
            int waitTime = Convert.ToInt32(Config.GetValueForSection(SELENIUMSECTION, "BrowserWaitTime", "0"));
            return TimeSpan.FromMilliseconds(waitTime);
        }

        /// <summary>
        /// Get the timeout timespan
        /// </summary>
        /// <returns>The timeout time span</returns>
        public static TimeSpan GetTimeoutTime()
        {
            int timeoutTime = Convert.ToInt32(Config.GetValueForSection(SELENIUMSECTION, "BrowserTimeout"));
            return TimeSpan.FromMilliseconds(timeoutTime);
        }

        /// <summary>
        /// get the browser size
        /// </summary>
        /// <returns>string of desired browser size</returns>
        public static string GetBrowserSize()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "BrowserSize", "MAXIMIZE").ToUpper();
        }

        /// <summary>
        /// Get if we want to use a proxy for the web driver traffic
        /// </summary>
        /// <returns>True if we want to use the proxy</returns>
        public static bool GetUseProxy()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "UseProxy", "NO").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get the proxy address to use
        /// </summary>
        /// <returns>The proxy address</returns>
        public static string GetProxyAddress()
        {
            return Config.GetValueForSection(SELENIUMSECTION, "ProxyAddress");
        }
    }
}