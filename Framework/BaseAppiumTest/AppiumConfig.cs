//--------------------------------------------------
// <copyright file="AppiumConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium Configuration class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Appium Configuration class
    /// </summary>
    public static class AppiumConfig
    {
        /// <summary>
        ///  Static name for the Appium configuration section
        /// </summary>
        private const string APPIUMSECTIION = "AppiumMaqs";

        /// <summary>
        ///  AppiumCapsMaqs configuration section
        /// </summary>
        private const string MOBILECAPS = "AppiumCapsMaqs";

        /// <summary>
        /// Get the mobile OS type
        /// </summary>
        /// <returns>The Mobile OS type</returns>
        public static string GetPlatformName()
        {
            return Config.GetValueForSection(APPIUMSECTIION, "PlatformName");
        }

        /// <summary>
        /// Get the OS Version
        /// </summary>
        /// <returns>OS Version</returns>
        public static string GetPlatformVersion()
        {
            return Config.GetValueForSection(APPIUMSECTIION, "PlatformVersion");
        }

        /// <summary>
        /// Get the Device Name
        /// </summary>
        /// <returns>Device Name</returns>
        public static string GetDeviceName()
        {
            return Config.GetValueForSection(APPIUMSECTIION, "DeviceName");
        }

        /// <summary>
        /// Get the mobile device
        /// <para>If no browser is provide in the project configuration file we default to Android</para>
        /// </summary>
        /// <returns>The appium driver</returns>
        public static AppiumDriver<IWebElement> MobileDevice()
        {
            return MobileDevice(GetPlatformName());
        }

        /// <summary>
        /// Get mobile hub url
        /// </summary>
        /// <returns>Mobile Hub Uri</returns>
        public static Uri GetMobileHubUrl()
        {
            return new Uri(Config.GetValueForSection(APPIUMSECTIION, "MobileHubUrl"));
        }

        /// <summary>
        /// Get the initialize Appium timeout timespan
        /// </summary>
        /// <returns>The initialize timeout</returns>
        public static TimeSpan GetCommandTimeout()
        {
            string value = Config.GetValueForSection(APPIUMSECTIION, "MobileCommandTimeout", "60000");

            if (!int.TryParse(value, out int timeout))
            {
                throw new ArgumentException("MobileCommandTimeout in " + APPIUMSECTIION + " should be a number but the current value is: " + value);
            }

            return TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Get if we should save page source on fail
        /// </summary>
        /// <returns>True if we want to save page source on fail</returns>
        public static bool GetSavePagesourceOnFail()
        {
            return Config.GetValueForSection(APPIUMSECTIION, "SavePagesourceOnFail").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get if we should save screenshots on soft alert fails
        /// </summary>
        /// <returns>True if we want to save screenshots on soft alert fails</returns>
        public static bool GetSoftAssertScreenshot()
        {
            return Config.GetValueForSection(APPIUMSECTIION, "SoftAssertScreenshot").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get the appium driver based for the provided mobile OS
        /// </summary>
        /// <param name="platformName">The browser type we want to use</param>
        /// <returns>An AppiumDriver</returns>
        public static AppiumDriver<IWebElement> MobileDevice(string platformName)
        {
            AppiumDriver<IWebElement> appiumDriver;
            switch (platformName.ToUpper())
            {
                case "ANDROID":
                    appiumDriver = new AndroidDriver<IWebElement>(GetMobileHubUrl(), GetMobileOptions(), GetCommandTimeout());
                    break;

                case "IOS":
                    appiumDriver = new IOSDriver<IWebElement>(GetMobileHubUrl(), GetMobileOptions(), GetCommandTimeout());
                    break;

                case "WIN":
                case "WINDOWS":
                    appiumDriver = new WindowsDriver<IWebElement>(GetMobileHubUrl(), GetMobileOptions(), GetCommandTimeout());
                    break;

                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("Mobile OS type '{0}' is not supported", platformName));
            }

            return appiumDriver;
        }

        /// <summary>
        /// Get the wait default wait driver
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        /// <returns>An WebDriverWait</returns>
        public static WebDriverWait GetWaitDriver(AppiumDriver<IWebElement> driver)
        {
            int waitTime = Convert.ToInt32(Config.GetValueForSection(APPIUMSECTIION, "MobileWaitTime", "0"));
            int timeoutTime = Convert.ToInt32(Config.GetValueForSection(APPIUMSECTIION, "MobileTimeout", "0"));

            return new WebDriverWait(new SystemClock(), driver, TimeSpan.FromMilliseconds(timeoutTime), TimeSpan.FromMilliseconds(waitTime));
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        public static void SetTimeouts(AppiumDriver<IWebElement> driver)
        {
            int timeoutTime = Convert.ToInt32(Config.GetValueForSection(APPIUMSECTIION, "MobileTimeout", "0"));
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMilliseconds(timeoutTime);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMilliseconds(timeoutTime);
        }

        /// <summary>
        /// Get the mobile options
        /// </summary>
        /// <returns>The mobile options</returns>
        private static AppiumOptions GetMobileOptions()
        {
            AppiumOptions options = new AppiumOptions();

            options.AddAdditionalCapability(MobileCapabilityType.DeviceName, GetDeviceName());
            options.AddAdditionalCapability(MobileCapabilityType.PlatformVersion, GetPlatformVersion());
            options.AddAdditionalCapability(MobileCapabilityType.PlatformName, GetPlatformName().ToUpper());
            options.SetMobileOptions();

            return options;
        }

        /// <summary>
        /// Reads the AppiumCapsMaqs section and appends to the driver options
        /// </summary>
        /// <param name="appiumOptions">The driver options to make this an extension method</param>
        /// <returns>The altered <see cref="DriverOptions"/> driver options</returns>
        private static AppiumOptions SetMobileOptions(this AppiumOptions appiumOptions)
        {
            Dictionary<string, string> remoteCapabilitySection = Config.GetSection(ConfigSection.AppiumCapsMaqs);

            if (remoteCapabilitySection == null)
            {
                return appiumOptions;
            }

            foreach (KeyValuePair<string, string> keyValue in remoteCapabilitySection)
            {
                if (remoteCapabilitySection[keyValue.Key].Length > 0)
                {
                    appiumOptions.AddAdditionalCapability(keyValue.Key, keyValue.Value);
                }
            }

            return appiumOptions;
        }
    }
}
