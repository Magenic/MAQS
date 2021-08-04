//--------------------------------------------------
// <copyright file="AppiumDriverFactory.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Factory for creating mobile drivers</summary>
//--------------------------------------------------
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;

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
        /// <param name="deviceType">The platform type we want to use</param>
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
                    throw new ArgumentException($"Mobile OS type '{deviceType}' is not supported");
            }

            // Check options to see if we are doing browser or app tests
            var allOption = options.ToDictionary();
            bool hasBrowserName = allOption.Any(kvp => kvp.Key.ToLower().Contains("browsername"));
            bool hasApp = allOption.Any(kvp => kvp.Key.ToLower().Contains("app"));
            bool hasBundleId = allOption.Any(kvp => kvp.Key.ToLower().Contains("bundleid"));

            // Only browser automation supports setting the associated timeouts
            if (hasBrowserName && !(hasApp || hasBundleId))
            {
                appiumDriver.SetDefaultTimeouts();
            }

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
                    throw;
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
                        throw new WebDriverException("Appium driver setup and teardown failed. Your driver may be out of date", quitExecption);
                    }
                }

                // Log that something went wrong
                throw new WebDriverException("Your driver may be out of date or unsupported.", e);
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
        /// <param name="timeout">The new timeout</param>
        public static void SetTimeouts(this AppiumDriver<IWebElement> driver, TimeSpan timeout)
        {
            driver.Manage().Timeouts().AsynchronousJavaScript = timeout;
            driver.Manage().Timeouts().PageLoad = timeout;
        }

        /// <summary>
        /// Get the mobile options
        /// </summary>
        /// <param name="capabilities">The mobile driver capabilities</param>
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

        /// <summary>
        /// Reads the AppiumCapsMaqs section and appends to the driver options
        /// </summary>
        /// <param name="appiumOptions">The driver options to make this an extension method</param>
        /// <param name="capabilities">The mobile driver capabilities</param>
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