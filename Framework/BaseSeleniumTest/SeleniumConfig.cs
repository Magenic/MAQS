//--------------------------------------------------
// <copyright file="SeleniumConfig.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class SeleniumConfig
    {
        /// <summary>
        /// Static field for RemoteSeleniumCapsMaqs configuration section.
        /// </summary>
        private static string remoteCapabilities = "RemoteSeleniumCapsMaqs";

        /// <summary>
        /// Get the browser
        /// <para>If no browser is provide in the project configuration file we default to Chrome</para>
        /// <para>Browser are maximized by default</para>
        /// </summary>
        /// <returns>The web driver</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="GetBrowser" lang="C#" />
        /// </example>
        public static IWebDriver Browser()
        {
            return Browser(GetBrowserName());
        }

        /// <summary>
        /// Get the webdriver based for the provided browser
        /// <para>Browser are maximized by default</para>
        /// </summary>
        /// <param name="browser">The browser type we want to use</param>
        /// <returns>An IWebDriver</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="GetBrowserWithString" lang="C#" />
        /// </example>
        public static IWebDriver Browser(string browser)
        {
            IWebDriver webDriver = null;

            try
            {
                switch (browser.ToUpper())
                {
                    case "INTERNET EXPLORER":
                    case "INTERNETEXPLORER":
                    case "IE":
                        webDriver = new InternetExplorerDriver(GetDriverLocation("IEDriverServer.exe"));
                        break;

                    case "FIREFOX":
                        FirefoxDriverService service = FirefoxDriverService.CreateDefaultService(GetDriverLocation("geckodriver.exe"), "geckodriver.exe");
                        FirefoxOptions firefoxOptions = new FirefoxOptions();
                        firefoxOptions.Profile = new FirefoxProfile();
                        webDriver = new FirefoxDriver(service, firefoxOptions, GetTimeoutTime());
                        break;

                    case "CHROME":
                        ChromeOptions chromeOptions = new ChromeOptions();
                        chromeOptions.AddArgument("test-type");
                        chromeOptions.AddArguments("--disable-web-security");
                        chromeOptions.AddArguments("--allow-running-insecure-content");
                        chromeOptions.AddArguments("--disable-extensions");
                        webDriver = new ChromeDriver(GetDriverLocation("chromedriver.exe"), chromeOptions);
                        break;

                    case "HEADLESSCHROME":
                        ChromeOptions headlessChromeOptions = new ChromeOptions();
                        headlessChromeOptions.AddArgument("test-type");
                        headlessChromeOptions.AddArguments("--disable-web-security");
                        headlessChromeOptions.AddArguments("--allow-running-insecure-content");
                        headlessChromeOptions.AddArguments("--disable-extensions");
                        headlessChromeOptions.AddArguments("-headless");
                        headlessChromeOptions.AddArguments("window-size=1920,1080");
                        webDriver = new ChromeDriver(GetDriverLocation("chromedriver.exe"), headlessChromeOptions);
                        break;

                    case "EDGE":
                        EdgeOptions edgeOptions = new EdgeOptions();
                        edgeOptions.PageLoadStrategy = EdgePageLoadStrategy.Normal;

                        webDriver = new EdgeDriver(GetDriverLocation("MicrosoftWebDriver.exe", GetProgramFilesFolder("Microsoft Web Driver", "MicrosoftWebDriver.exe")), edgeOptions);
                        break;

                    case "PHANTOMJS":
                        PhantomJSOptions phantomOptions = new PhantomJSOptions();
                        phantomOptions.AddAdditionalCapability("phantomjs.page.settings.userAgent", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:26.0) Gecko/20100101 Firefox/26.0");
                        webDriver = new PhantomJSDriver(GetDriverLocation("phantomjs.exe"), phantomOptions);
                        break;

                    case "REMOTE":
                        webDriver = new RemoteWebDriver(new Uri(Config.GetValue("HubUrl")), GetRemoteCapabilities());
                        break;

                    default:
                        throw new Exception(StringProcessor.SafeFormatter("Browser type '{0}' is not supported", browser));
                }

                // Maximize the browser and than return it
                webDriver.Manage().Window.Maximize();
                return webDriver;
            }
            catch (Exception e)
            {
                // Make sure we have a web driver
                if (webDriver != null)
                {
                    try
                    {
                        // Try to cleanup
                        webDriver.Quit();
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
        /// Get the browser type name - Example: Chrome
        /// </summary>
        /// <returns>The browser type</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="GetBrowserName" lang="C#" />
        /// </example>
        public static string GetBrowserName()
        {
            return Config.GetValue("Browser", "Chrome");
        }

        /// <summary>
        /// Get the hint path for the web driver
        /// </summary>
        /// <returns>The hint path for the web driver</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="GetDriverHintPath" lang="C#" />
        /// </example>
        public static string GetDriverHintPath()
        {
            string defaultPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return Config.GetValue("WebDriverHintPath", defaultPath);
        }

        /// <summary>
        /// Get the remote browser type name
        /// </summary>
        /// <returns>The browser type being used on grid</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="GetRemoteName" lang="C#" />
        /// </example>
        public static string GetRemoteBrowserName()
        {
            return Config.GetValue("RemoteBrowser", "Chrome");
        }

        /// <summary>
        /// Get the remote browser version
        /// </summary>
        /// <returns>The browser version to run against on grid</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="RemoteVersion" lang="C#" />
        /// </example>
        public static string GetRemoteBrowserVersion()
        {
            return Config.GetValue("RemoteBrowserVersion");
        }

        /// <summary>
        /// Get the remote platform type name
        /// </summary>
        /// <returns>The platform (or OS) to run remote tests against</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="RemotePlatform" lang="C#" />
        /// </example>
        public static string GetRemotePlatform()
        {
            return Config.GetValue("RemotePlatform");
        }

        /// <summary>
        /// Get the wait default wait driver
        /// </summary>
        /// <param name="driver">Brings in an IWebDriver</param>
        /// <returns>An WebDriverWait</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="WaitDriver" lang="C#" />
        /// </example>
        public static WebDriverWait GetWaitDriver(IWebDriver driver)
        {
            return new WebDriverWait(new SystemClock(), driver, GetTimeoutTime(), GetWaitTime());
        }

        /// <summary>
        /// Get the web site base url
        /// </summary>
        /// <returns>The web site base url</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="GetWebsiteBase" lang="C#" />
        /// </example>
        public static string GetWebSiteBase()
        {
            return Config.GetValue("WebSiteBase");
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Brings in an IWebDriver</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumConfigTests.cs" region="SetTimeouts" lang="C#" />
        /// </example>
        public static void SetTimeouts(IWebDriver driver)
        {
            TimeSpan timeoutTime = GetTimeoutTime();
            driver.Manage().Timeouts().PageLoad = timeoutTime;
            driver.Manage().Timeouts().AsynchronousJavaScript = timeoutTime;
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
            string hintPath = GetDriverHintPath();

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
                throw new Exception(StringProcessor.SafeFormatter("Unable to find driver for '{0}'", driverFile));
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

        /// <summary>
        /// Get the remote desired capability
        /// </summary>
        /// <returns>The remote desired capability</returns>
        private static DesiredCapabilities GetRemoteCapabilities()
        {
            DesiredCapabilities capabilities = null;
            string remoteBrowser = GetRemoteBrowserName();
            string remotePlatform = GetRemotePlatform();
            string remoteBrowserVersion = GetRemoteBrowserVersion();

            switch (remoteBrowser.ToUpper())
            {
                case "INTERNET EXPLORER":
                case "INTERNETEXPLORER":
                case "IE":
                    capabilities = DesiredCapabilities.InternetExplorer();
                    break;

                case "FIREFOX":
                    capabilities = DesiredCapabilities.Firefox();
                    break;

                case "CHROME":
                    capabilities = DesiredCapabilities.Chrome();
                    break;

                case "EDGE":
                    capabilities = DesiredCapabilities.Edge();
                    break;

                case "SAFARI":
                    capabilities = DesiredCapabilities.Safari();
                    break;

                default:
                    throw new Exception(StringProcessor.SafeFormatter("Remote browser type '{0}' is not supported", remoteBrowser));
            }

            // Add a platform setting if one was provided
            if (remotePlatform.Length > 0)
            {
                capabilities.SetCapability("platform", remotePlatform);
            }

            // Add a remote browser setting if one was provided
            if (remoteBrowserVersion.Length > 0)
            {
                capabilities.SetCapability("version", remoteBrowserVersion);
            }

            // Add RemoteCapabilites section if it exists
            capabilities.SetRemoteCapabilities();

            return capabilities;
        }

        /// <summary>
        /// Reads the RemoteSeleniumCapsMaqs section and appends to the DesiredCapabilities
        /// </summary>
        /// <param name="dc">The Desired Capabilities to make this an extension method</param>
        /// <returns>The altered <see cref="DesiredCapabilities"/> Desired Capabilities</returns>
        private static DesiredCapabilities SetRemoteCapabilities(this DesiredCapabilities dc)
        {
            var remoteCapabilitySection = ConfigurationManager.GetSection(remoteCapabilities) as NameValueCollection;
            if (remoteCapabilitySection == null)
            {
                return dc;
            }

            var keys = remoteCapabilitySection.AllKeys;
            foreach (var key in keys)
            {
                if (remoteCapabilitySection[key].Length > 0)
                {
                    dc.SetCapability(key, remoteCapabilitySection[key]);
                }
            }

            return dc;
        }

        /// <summary>
        /// Get the timeout timespan
        /// </summary>
        /// <returns>The timeout time span</returns>
        private static TimeSpan GetTimeoutTime()
        {
            int timeoutTime = Convert.ToInt32(Config.GetValue("Timeout", "0"));
            return TimeSpan.FromMilliseconds(timeoutTime);
        }

        /// <summary>
        /// Get the wait timespan
        /// </summary>
        /// <returns>The wait time span</returns>
        private static TimeSpan GetWaitTime()
        {
            int waitTime = Convert.ToInt32(Config.GetValue("WaitTime", "0"));
            return TimeSpan.FromMilliseconds(waitTime);
        }
    }
}