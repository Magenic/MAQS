//--------------------------------------------------
// <copyright file="WebDriverFactory.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Web driver factory</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Static web driver factory
    /// </summary>
    public static class WebDriverFactory
    {
        /// <summary>
        /// Get the default web driver based on the test run configuration
        /// </summary>
        /// <returns>A web driver</returns>
        public static IWebDriver GetDefaultBrowser()
        {
            return GetBrowserWithDefaultConfiguration(SeleniumConfig.GetBrowserType());
        }

        /// <summary>
        /// Get the default web driver (for the specified browser type) based on the test run configuration
        /// </summary>
        /// <param name="browser">The type of browser</param>
        /// <returns>A web driver</returns>
        public static IWebDriver GetBrowserWithDefaultConfiguration(BrowserType browser)
        {
            IWebDriver webDriver = null;
            TimeSpan timeout = SeleniumConfig.GetCommandTimeout();
            string size = SeleniumConfig.GetBrowserSize();
            try
            {
                switch (browser)
                {
                    case BrowserType.IE:
                        webDriver = GetIEDriver(timeout, GetDefaultIEOptions(), size);
                        break;
                    case BrowserType.Firefox:
                        webDriver = GetFirefoxDriver(timeout, GetDefaultFirefoxOptions(), size);
                        break;

                    case BrowserType.Chrome:
                        webDriver = GetChromeDriver(timeout, GetDefaultChromeOptions(), size);
                        break;

                    case BrowserType.HeadlessChrome:
                        webDriver = GetHeadlessChromeDriver(timeout, GetDefaultHeadlessChromeOptions(size));
                        break;

                    case BrowserType.Edge:
                        webDriver = GetEdgeDriver(timeout, GetDefaultEdgeOptions(), size);
                        break;

                    case BrowserType.Remote:
                        webDriver = new RemoteWebDriver(SeleniumConfig.GetHubUri(), GetDefaultRemoteOptions().ToCapabilities(), SeleniumConfig.GetCommandTimeout());
                        break;

                    default:
                        throw new ArgumentException(StringProcessor.SafeFormatter("Browser type '{0}' is not supported", browser));
                }

                return webDriver;
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ArgumentException))
                {
                    throw e;
                }
                else
                {
                    try
                    {
                        // Try to cleanup
                        webDriver?.KillDriver();
                    }
                    catch (Exception quitExecption)
                    {
                        throw new Exception("Web driver setup and teardown failed. Your web driver may be out of date", quitExecption);
                    }
                }

                // Log that something went wrong
                throw new Exception("Your web driver may be out of date or unsupported.", e);
            }
        }

        /// <summary>
        /// Get the default Chrome options
        /// </summary>
        /// <returns>The default Chrome options</returns>
        public static ChromeOptions GetDefaultChromeOptions()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("test-type");
            chromeOptions.AddArguments("--disable-web-security");
            chromeOptions.AddArguments("--allow-running-insecure-content");
            chromeOptions.AddArguments("--disable-extensions");

            chromeOptions.SetProxySettings();
            return chromeOptions;
        }

        /// <summary>
        /// Get the default headless Chrome options
        /// </summary>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        /// <returns>The default headless Chrome options</returns>
        public static ChromeOptions GetDefaultHeadlessChromeOptions(string size = "MAXIMIZE")
        {
            ChromeOptions headlessChromeOptions = new ChromeOptions();
            headlessChromeOptions.AddArgument("test-type");
            headlessChromeOptions.AddArguments("--disable-web-security");
            headlessChromeOptions.AddArguments("--allow-running-insecure-content");
            headlessChromeOptions.AddArguments("--disable-extensions");
            headlessChromeOptions.AddArgument("--no-sandbox");
            headlessChromeOptions.AddArguments("--headless");
            headlessChromeOptions.AddArguments(GetHeadlessWindowSizeString(size));

            headlessChromeOptions.SetProxySettings();
            return headlessChromeOptions;
        }

        /// <summary>
        /// Get the default IE options
        /// </summary>
        /// <returns>The default IE options</returns>
        public static InternetExplorerOptions GetDefaultIEOptions()
        {
            var options = new InternetExplorerOptions
            {
                IgnoreZoomLevel = true
            };

            options.SetProxySettings();
            return options;
        }

        /// <summary>
        /// Get the default Firefox options
        /// </summary>
        /// <returns>The default Firefox options</returns>
        public static FirefoxOptions GetDefaultFirefoxOptions()
        {
            FirefoxOptions firefoxOptions = new FirefoxOptions
            {
                Profile = new FirefoxProfile()
            };

            firefoxOptions.SetProxySettings();
            return firefoxOptions;
        }

        /// <summary>
        /// Get the default Edge options
        /// </summary>
        /// <returns>The default Edge options</returns>
        public static EdgeOptions GetDefaultEdgeOptions()
        {
            EdgeOptions edgeOptions = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };

            edgeOptions.SetProxySettings();
            return edgeOptions;
        }

        /// <summary>
        /// Initialize a new Chrome driver
        /// </summary>
        /// <param name="commandTimeout">Browser command timeout</param>
        /// <param name="chromeOptions">Browser options</param>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        /// <returns>A new Chrome driver</returns>
        public static IWebDriver GetChromeDriver(TimeSpan commandTimeout, ChromeOptions chromeOptions, string size = "MAXIMIZE")
        {
            return CreateDriver(() =>
            {
                var driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), chromeOptions, commandTimeout);
                SetBrowserSize(driver, size);
                return driver;
            });
        }

        /// <summary>
        /// Initialize a new headless Chrome driver
        /// </summary>
        /// <param name="commandTimeout">Browser command timeout</param>
        /// <param name="headlessChromeOptions">Browser options</param>
        /// <returns>A new headless Chrome driver</returns>
        public static IWebDriver GetHeadlessChromeDriver(TimeSpan commandTimeout, ChromeOptions headlessChromeOptions)
        {
            return CreateDriver(() => new ChromeDriver(ChromeDriverService.CreateDefaultService(), headlessChromeOptions, commandTimeout));
        }

        /// <summary>
        /// Initialize a new Firefox driver
        /// </summary>
        /// <param name="commandTimeout">Browser command timeout</param>
        /// <param name="firefoxOptions">Browser options</param>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        /// <returns>A new Firefox driver</returns>
        public static IWebDriver GetFirefoxDriver(TimeSpan commandTimeout, FirefoxOptions firefoxOptions, string size = "MAXIMIZE")
        {
            return CreateDriver(() =>
            {
                // Add support for encoding 437 that was removed in .net core
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                // Create service and set host.  Setting host directly greatly improves speed.
                var service = FirefoxDriverService.CreateDefaultService();
                service.Host = "::1";

                var driver = new FirefoxDriver(service, firefoxOptions, commandTimeout);
                SetBrowserSize(driver, size);

                return driver;
            });
        }

        /// <summary>
        /// Initialize a new Edge driver
        /// </summary>
        /// <param name="commandTimeout">Browser command timeout</param>
        /// <param name="edgeOptions">Browser options</param>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        /// <returns>A new Edge driver</returns>
        public static IWebDriver GetEdgeDriver(TimeSpan commandTimeout, EdgeOptions edgeOptions, string size = "MAXIMIZE")
        {
            return CreateDriver(() =>
            {
                var driver = new EdgeDriver(GetDriverLocation("MicrosoftWebDriver.exe", GetProgramFilesFolder("Microsoft Web Driver", "MicrosoftWebDriver.exe")), edgeOptions, commandTimeout);
                SetBrowserSize(driver, size);
                return driver;
            });
        }

        /// <summary>
        /// Get a new IE driver
        /// </summary>
        /// <param name="commandTimeout">Browser command timeout</param>
        /// <param name="internetExplorerOptions">Browser options</param>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        /// <returns>A new IE driver</returns>
        public static IWebDriver GetIEDriver(TimeSpan commandTimeout, InternetExplorerOptions internetExplorerOptions, string size = "MAXIMIZE")
        {
            return CreateDriver(() =>
            {
                var driver = new InternetExplorerDriver(GetDriverLocation("IEDriverServer.exe"), internetExplorerOptions, commandTimeout);
                SetBrowserSize(driver, size);

                return driver;
            });
        }

        /// <summary>
        /// Get the default remote driver options - Default values are pulled from the configuration
        /// </summary>
        /// <returns>The remote driver options</returns>
        public static DriverOptions GetDefaultRemoteOptions()
        {
            RemoteBrowserType remoteBrowser = SeleniumConfig.GetRemoteBrowserType();
            string remotePlatform = SeleniumConfig.GetRemotePlatform();
            string remoteBrowserVersion = SeleniumConfig.GetRemoteBrowserVersion();
            Dictionary<string, object> capabilities = SeleniumConfig.GetRemoteCapabilitiesAsObjects();

            return GetRemoteOptions(remoteBrowser, remotePlatform, remoteBrowserVersion, capabilities);
        }

        /// <summary>
        /// Get the remote driver options
        /// </summary>
        /// <param name="remoteBrowser">The remote browser type</param>
        /// <returns>The remote driver options</returns>
        public static DriverOptions GetRemoteOptions(RemoteBrowserType remoteBrowser)
        {
            return GetRemoteOptions(remoteBrowser, string.Empty, string.Empty, new Dictionary<string, string>());
        }

        /// <summary>
        /// Get the remote driver options
        /// </summary>
        /// <param name="remoteBrowser">The remote browser type</param>
        /// <param name="remoteCapabilities">Additional remote capabilities</param>
        /// <returns>The remote driver options</returns>
        public static DriverOptions GetRemoteOptions(RemoteBrowserType remoteBrowser, Dictionary<string, string> remoteCapabilities)
        {
            return GetRemoteOptions(remoteBrowser, string.Empty, string.Empty, remoteCapabilities);
        }

        /// <summary>
        /// Get the remote driver options
        /// </summary>
        /// <param name="remoteBrowser">The remote browser type</param>
        /// <param name="remotePlatform">The remote platform</param>
        /// <param name="remoteBrowserVersion">The remote browser version</param>
        /// <param name="remoteCapabilities">Additional remote capabilities</param>
        /// <returns>The remote driver options</returns>
        public static DriverOptions GetRemoteOptions(RemoteBrowserType remoteBrowser, string remotePlatform, string remoteBrowserVersion, Dictionary<string, string> remoteCapabilities)
        {
            Dictionary<string, object> capabilities = remoteCapabilities.ToDictionary(pair => pair.Key, pair => (object)pair.Value);
            return GetRemoteOptions(remoteBrowser, remotePlatform, remoteBrowserVersion, capabilities);
        }

        /// <summary>
        /// Get the remote driver options
        /// </summary>
        /// <param name="remoteBrowser">The remote browser type</param>
        /// <param name="remoteCapabilities">Additional remote capabilities</param>
        /// <returns>The remote driver options</returns>
        public static DriverOptions GetRemoteOptions(RemoteBrowserType remoteBrowser, Dictionary<string, object> remoteCapabilities)
        {
            return GetRemoteOptions(remoteBrowser, string.Empty, string.Empty, remoteCapabilities);
        }

        /// <summary>
        /// Get the remote driver options
        /// </summary>
        /// <param name="remoteBrowser">The remote browser type</param>
        /// <param name="remotePlatform">The remote platform</param>
        /// <param name="remoteBrowserVersion">The remote browser version</param>
        /// <param name="remoteCapabilities">Additional remote capabilities</param>
        /// <returns>The remote driver options</returns>
        public static DriverOptions GetRemoteOptions(RemoteBrowserType remoteBrowser, string remotePlatform, string remoteBrowserVersion, Dictionary<string, object> remoteCapabilities)
        {
            DriverOptions options = null;

            switch (remoteBrowser)
            {
                case RemoteBrowserType.IE:
                    options = new InternetExplorerOptions();
                    break;

                case RemoteBrowserType.Firefox:
                    options = new FirefoxOptions();
                    break;

                case RemoteBrowserType.Chrome:
                    options = new ChromeOptions();
                    break;

                case RemoteBrowserType.Edge:
                    options = new EdgeOptions();
                    break;

                case RemoteBrowserType.Safari:
                    options = new SafariOptions();
                    break;

                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Remote browser type '{0}' is not supported", remoteBrowser));
            }

            // Make sure the remote capabilities dictionary exists
            if (remoteCapabilities == null)
            {
                remoteCapabilities = new Dictionary<string, object>();
            }

            // Add a platform setting if one was provided
            if (!string.IsNullOrEmpty(remotePlatform) && !remoteCapabilities.ContainsKey("platform"))
            {
                remoteCapabilities.Add("platform", remotePlatform);
            }

            // Add a remote browser setting if one was provided
            if (!string.IsNullOrEmpty(remoteBrowserVersion) && !remoteCapabilities.ContainsKey("version"))
            {
                remoteCapabilities.Add("version", remoteBrowserVersion);
            }

            // Add additional capabilities to the driver options
            options.SetDriverOptions(remoteCapabilities);
            options.SetProxySettings();
            return options;
        }

        /// <summary>
        /// Add additional capabilities to the driver options
        /// </summary>
        /// <param name="driverOptions">The driver option you want to add capabilities to</param>
        /// <param name="additionalCapabilities">Capabilities to add</param>
        public static void SetDriverOptions(this DriverOptions driverOptions, Dictionary<string, object> additionalCapabilities)
        {
            // If there are no additional capabilities just return
            if (additionalCapabilities == null)
            {
                return;
            }

            foreach (KeyValuePair<string, object> keyValue in additionalCapabilities)
            {
                // Make sure there is a value
                if (keyValue.Value != null && (!(keyValue.Value is string) || !string.IsNullOrEmpty(keyValue.Value as string)))
                {
                    switch (driverOptions)
                    {
                        case ChromeOptions chromeOptions:
                            chromeOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value, true);
                            break;
                        case FirefoxOptions firefoxOptions:
                            firefoxOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value, true);
                            break;
                        case InternetExplorerOptions ieOptions:
                            ieOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value, true);
                            break;
                        default:
                            // Edge and Safari do not support marking capabilities as global  - AKA the third parameter
                            driverOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the proxy settings for the driver options (if configured)
        /// </summary>
        /// <param name="options">The driver options</param>
        public static void SetProxySettings(this DriverOptions options)
        {
            if (SeleniumConfig.GetUseProxy())
            {
                SetProxySettings(options, SeleniumConfig.GetProxyAddress());
            }
        }

        /// <summary>
        /// Sets the proxy settings for the driver options
        /// </summary>
        /// <param name="options">The driver options</param>
        /// <param name="proxyAddress">The proxy address</param>
        public static void SetProxySettings(this DriverOptions options, string proxyAddress)
        {
            if (!string.IsNullOrEmpty(proxyAddress))
            {
                Proxy proxy = new Proxy
                {
                    HttpProxy = proxyAddress,
                    SslProxy = proxyAddress
                };
                options.Proxy = proxy;
            }
        }

        /// <summary>
        /// Creates a web driver, but if the creation fails it tries to cleanup after itself
        /// </summary>
        /// <param name="createFunction">Function for creating a web driver</param>
        /// <returns>A web driver</returns>
        public static IWebDriver CreateDriver(Func<IWebDriver> createFunction)
        {
            IWebDriver webDriver = null;

            try
            {
                webDriver = createFunction();
                return webDriver;
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ArgumentException))
                {
                    throw e;
                }
                else
                {
                    try
                    {
                        // Try to cleanup
                        webDriver?.KillDriver();
                    }
                    catch (Exception quitExecption)
                    {
                        throw new Exception("Web driver setup and teardown failed. Your web driver may be out of date", quitExecption);
                    }
                }

                // Log that something went wrong
                throw new Exception("Your web driver may be out of date or unsupported.", e);
            }
        }

        /// <summary>
        /// Sets the browser size based on the provide string value
        /// Browser size is expected to be: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)
        /// MAXIMIZE just maximizes the bowser
        /// DEFAULT does not change the current size
        /// #x# sets a custom size
        /// </summary>
        /// <param name="webDriver">the webDriver from the Browser method</param>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        public static void SetBrowserSize(IWebDriver webDriver, string size)
        {
            size = size.ToUpper();

            if (size == "MAXIMIZE")
            {
                webDriver.Manage().Window.Maximize();
            }
            else if (size != "DEFAULT")
            {
                ExtractSizeFromString(size, out int width, out int height);
                webDriver.Manage().Window.Size = new Size(width, height);
            }
        }

        /// <summary>
        /// Get the browser/browser size as a string
        /// </summary>
        /// <param name="size">Browser size in the following format: MAXIMIZE, DEFAULT, or #x# (such as 1920x1080)</param>
        /// <returns>The browser size as a string - Specifically for headless Chrome options</returns>
        private static string GetHeadlessWindowSizeString(string size)
        {
            if (size == "MAXIMIZE" || size == "DEFAULT")
            {
                // If we need a string default to 1920 by 1080
                return "window-size=1920,1080";
            }
            else
            {
                ExtractSizeFromString(size, out int width, out int height);
                return string.Format("window-size={0},{1}", width, height);
            }
        }

        /// <summary>
        /// Get the window size as a string
        /// </summary>
        /// <param name="size">The size in a #X# format</param>
        /// <param name="width">The browser width</param>
        /// <param name="height">The browser height</param>
        private static void ExtractSizeFromString(string size, out int width, out int height)
        {
            string[] sizes = size.Split('X');

            if (!size.Contains("X") || sizes.Length != 2)
            {
                throw new ArgumentException("Browser size is expected to be in an expected format: 1920x1080");
            }

            if (!int.TryParse(sizes[0], out width) || !int.TryParse(sizes[1], out height))
            {
                throw new InvalidCastException("Length and Width must be a string that is an integer value: 400x400");
            }
        }

        /// <summary>
        /// Get the web driver location
        /// </summary>
        /// <param name="driverFile">The web drive file, including extension</param>
        /// <param name="defaultHintPath">The default location for the specific driver</param>
        /// <param name="mustExist">Do we need to know where this drive is located, if this is true and the file is not found an error will be thrown</param>
        /// <returns>The path to the web driver</returns>
        private static string GetDriverLocation(string driverFile, string defaultHintPath = "", bool mustExist = true)
        {
            // Get the hint path from the app.config
            string hintPath = SeleniumConfig.GetDriverHintPath();

            // Try the hintpath first
            if (!string.IsNullOrEmpty(hintPath) && File.Exists(Path.Combine(hintPath, driverFile)))
            {
                return hintPath;
            }

            // Try the default hit path next
            if (!string.IsNullOrEmpty(defaultHintPath) && File.Exists(Path.Combine(defaultHintPath, driverFile)))
            {
                return Path.Combine(defaultHintPath).ToString();
            }

            // Get the test dll location
            UriBuilder uri = new UriBuilder(Assembly.GetExecutingAssembly().Location);
            string testLocation = Path.GetDirectoryName(Uri.UnescapeDataString(uri.Path));

            // Try the test dll location
            if (File.Exists(Path.Combine(testLocation, driverFile)))
            {
                return testLocation;
            }

            // We didn't find the web driver so throw an error if we need to know where it is
            if (mustExist)
            {
                throw new FileNotFoundException(StringProcessor.SafeFormatter("Unable to find driver for '{0}'", driverFile));
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the programs file folder which contains given file
        /// </summary>
        /// <param name="folderName">The programs file sub folder</param>
        /// <param name="file">The file we are looking for</param>
        /// <returns>The parent folder of the given file or the empty string if the file is not found</returns>
        private static string GetProgramFilesFolder(string folderName, string file)
        {
            // Handle 64 bit systems first
            if (IntPtr.Size == 8 || (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                string path = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432"), folderName, file);
                if (File.Exists(path))
                {
                    return Path.GetDirectoryName(path);
                }

                path = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), folderName, file);
                if (File.Exists(path))
                {
                    return Path.GetDirectoryName(path);
                }
            }
            else
            {
                string path = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), folderName, file);
                if (File.Exists(path))
                {
                    return Path.GetDirectoryName(path);
                }
            }

            return string.Empty;
        }
    }
}
