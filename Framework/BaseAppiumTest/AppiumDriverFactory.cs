//--------------------------------------------------
// <copyright file="AppiumDriverFactory.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Factory for creating mobile drivers</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Static class creating Appium drivers
    /// </summary>
    public static class AppiumDriverFactory
    {
        /// <summary>
        /// Get the default Appium driver based on the test run configuration 
        /// </summary>
        /// <returns>An AppiumDriver</returns>
        public static AppiumDriver<IWebElement> GetDefaultMobileDriver()
        {
            return GetDefaultMobileDriver(AppiumConfig.GetDeviceType());
        }

        /// <summary>
        /// Get the default Appium driver based on the test run configuration 
        /// </summary>
        /// <param name="platformName">The platform type we want to use</param>
        /// <returns>An AppiumDriver</returns>
        public static AppiumDriver<IWebElement> GetDefaultMobileDriver(PlatformType deviceType)
        {
            AppiumDriver<IWebElement> appiumDriver;

            Uri mobileHub = AppiumConfig.GetMobileHubUrl();
            TimeSpan timeout = AppiumConfig.GetCommandTimeout();
            AppiumOptions options = GetDefaultMobileOptions();

            switch (deviceType)
            {
                case PlatformType.Android:
                    appiumDriver = GetAndroidDriver(mobileHub, options, timeout);
                    break;

                case PlatformType.iOS:
                    appiumDriver = GetIOSDriver(mobileHub, options, timeout);
                    break;

                case PlatformType.Windows:
                    appiumDriver = GetWindowsDriver(mobileHub, options, timeout);
                    break;

                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Mobile OS type '{0}' is not supported", deviceType));
            }

            appiumDriver.SetDefaultTimeouts();

            return appiumDriver;
        }

        /// <summary>
        /// Get a Android Appium driver
        /// </summary>
        /// <param name="mobileHub">Path to the mobile hub</param>
        /// <param name="options">Appium options</param>
        /// <param name="timeout">Command timeout</param>
        /// <returns>The Appium driver</returns>
        public static AppiumDriver<IWebElement> GetAndroidDriver(Uri mobileHub, AppiumOptions options, TimeSpan timeout)
        {
            return CreateDriver(() =>
            {
                var driver = new AndroidDriver<IWebElement>(mobileHub, options, timeout);
                return driver;
            });
        }

        /// <summary>
        /// Get a iOS Appium driver
        /// </summary>
        /// <param name="mobileHub">Path to the mobile hub</param>
        /// <param name="options">Appium options</param>
        /// <param name="timeout">Command timeout</param>
        /// <returns>The Appium driver</returns>
        public static AppiumDriver<IWebElement> GetIOSDriver(Uri mobileHub, AppiumOptions options, TimeSpan timeout)
        {
            return CreateDriver(() =>
            {
                var driver = new IOSDriver<IWebElement>(mobileHub, options, timeout);
                return driver;
            });
        }

        /// <summary>
        /// Get a Windows Appium driver
        /// </summary>
        /// <param name="mobileHub">Path to the mobile hub</param>
        /// <param name="options">Appium options</param>
        /// <param name="timeout">Command timeout</param>
        /// <returns>The Appium driver</returns>
        public static AppiumDriver<IWebElement> GetWindowsDriver(Uri mobileHub, AppiumOptions options, TimeSpan timeout)
        {
            return CreateDriver(() =>
            {
                var driver = new WindowsDriver<IWebElement>(mobileHub, options, timeout);
                return driver;
            });
        }

        /// <summary>
        /// Creates an Appium driver, but if the creation fails it tries to cleanup after itself
        /// </summary>
        /// <param name="createFunction">Function for creating a driver</param>
        /// <returns>An Appium driver</returns>
        public static AppiumDriver<IWebElement> CreateDriver(Func<AppiumDriver<IWebElement>> createFunction)
        {
            AppiumDriver<IWebElement> appiumDriver = null;

            try
            {
                appiumDriver = createFunction();
                return appiumDriver;
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
                        appiumDriver?.KillDriver();
                    }
                    catch (Exception quitExecption)
                    {
                        throw new Exception("Appium driver setup and teardown failed. Your driver may be out of date", quitExecption);
                    }
                }

                // Log that something went wrong
                throw new Exception("Your driver may be out of date or unsupported.", e);
            }
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        public static void SetDefaultTimeouts(this AppiumDriver<IWebElement> driver)
        {
            driver.SetTimeouts(AppiumConfig.GetMobileTimeout());
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        public static void SetTimeouts(this AppiumDriver<IWebElement> driver, TimeSpan timeout)
        {
            driver.Manage().Timeouts().AsynchronousJavaScript = timeout;
            driver.Manage().Timeouts().PageLoad = timeout;
        }

        /// <summary>
        /// Get the mobile options
        /// </summary>
        /// <returns>The mobile options</returns>
        public static AppiumOptions GetDefaultMobileOptions(Dictionary<string, object> capabilities)
        {
            AppiumOptions options = new AppiumOptions();

            options.SetMobileOptions(capabilities);

            return options;
        }

        /// <summary>
        /// Get the mobile options
        /// </summary>
        /// <returns>The mobile options</returns>
        public static AppiumOptions GetDefaultMobileOptions()
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability(MobileCapabilityType.DeviceName, AppiumConfig.GetDeviceName());
            options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, AppiumConfig.GetPlatformVersion());
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, AppiumConfig.GetPlatformName().ToUpper());
            options.SetMobileOptions(AppiumConfig.GetCapabilitiesAsObjects());

            return options;
        }

        #region Obsolete SavePageSource
        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        [Obsolete("SavePageSource that does not take a AppiumTestObject parameter is deprecated")]
        public static bool SavePageSource(this AppiumDriver<IWebElement> appiumDriver, Logger log, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = SavePageSource(appiumDriver, LoggingConfig.GetLogDirectory(), "PageSource" + appendName);
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + "_PS" + appendName;

                    path = SavePageSource(appiumDriver, directory, fileNameWithoutExtension);
                }

                log.LogMessage(MessageType.INFORMATION, "Page Source saved: " + path);
                return true;
            }
            catch (Exception exception)
            {
                log.LogMessage(MessageType.ERROR, "Page Source error: {0}", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// To capture Page Source during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        [Obsolete("SavePageSource that does not take a AppiumTestObject parameter is deprecated")]
        public static string SavePageSource(this AppiumDriver<IWebElement> appiumDriver, string directory, string fileNameWithoutExtension)
        {
            // Save the current page source into a string
            string pageSource = appiumDriver.PageSource;

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, fileNameWithoutExtension + ".txt");

            // Create new instance of Streamwriter and Auto Flush after each call
            StreamWriter writer = new StreamWriter(path, false)
            {
                AutoFlush = true
            };

            // Write page source to a new file
            writer.Write(pageSource);
            writer.Close();

            return path;
        }
        #endregion

        #region Obsolete CaptureScreenshot
        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        [Obsolete("CaptureScreenshot that does not take a AppiumTestObject parameter is deprecated")]
        public static bool CaptureScreenshot(this AppiumDriver<IWebElement> appiumDriver, Logger log, string appendName = "")
        {
            try
            {
                if (!(log is FileLogger))
                {
                    return false;
                }

                string fullpath = ((FileLogger)log).FilePath;
                string directory = Path.GetDirectoryName(fullpath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + appendName;
                CaptureScreenshot(appiumDriver, directory, fileNameWithoutExtension);

                log.LogMessage(MessageType.INFORMATION, "Screenshot saved");
                return true;
            }
            catch (Exception exception)
            {
                log.LogMessage(MessageType.ERROR, "Screenshot error: {0}", exception.InnerException.ToString());
                return false;
            }
        }

        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        [Obsolete("CaptureScreenshot that does not take a AppiumTestObject parameter is deprecated")]
        public static void CaptureScreenshot(this AppiumDriver<IWebElement> appiumDriver, string directory, string fileNameWithoutExtension)
        {
            Screenshot screenshot = ((ITakesScreenshot)appiumDriver).GetScreenshot();

            string path = Path.Combine(directory, fileNameWithoutExtension + ".png");

            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }
        #endregion

        /// <summary>
        /// Reads the AppiumCapsMaqs section and appends to the driver options
        /// </summary>
        /// <param name="appiumOptions">The driver options to make this an extension method</param>
        /// <returns>The altered <see cref="DriverOptions"/> driver options</returns>
        private static AppiumOptions SetMobileOptions(this AppiumOptions appiumOptions, Dictionary<string, object> capabilities)
        {
            if (capabilities == null)
            {
                return appiumOptions;
            }

            foreach (KeyValuePair<string, object> keyValue in capabilities)
            {
                if (keyValue.Value != null && (!(keyValue.Value is string) || !string.IsNullOrEmpty(keyValue.Value as string)))
                {
                    appiumOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value);
                }
            }

            return appiumOptions;
        }
    }
}