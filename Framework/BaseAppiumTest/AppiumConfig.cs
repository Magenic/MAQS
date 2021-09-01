//--------------------------------------------------
// <copyright file="AppiumConfig.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium Configuration class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Appium Configuration class
    /// </summary>
    public static class AppiumConfig
    {
        /// <summary>
        /// Loads when class is loaded
        /// </summary>
        static AppiumConfig()
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
                    "PlatformName",
                    "PlatformVersion",
                    "DeviceName"
                }
            };
            Config.Validate(ConfigSection.AppiumMaqs, validator);
        }

        /// <summary>
        /// Get the mobile OS type
        /// </summary>
        /// <returns>The Mobile OS type</returns>
        public static string GetPlatformName()
        {
            return Config.GetValueForSection(ConfigSection.AppiumMaqs, "PlatformName");
        }

        /// <summary>
        /// Get the OS Version
        /// </summary>
        /// <returns>OS Version</returns>
        public static string GetPlatformVersion()
        {
            return Config.GetValueForSection(ConfigSection.AppiumMaqs, "PlatformVersion");
        }

        /// <summary>
        /// Get the Device Name
        /// </summary>
        /// <returns>Device Name</returns>
        public static string GetDeviceName()
        {
            return Config.GetValueForSection(ConfigSection.AppiumMaqs, "DeviceName");
        }

        /// <summary>
        /// Get mobile hub url
        /// </summary>
        /// <returns>Mobile Hub Uri</returns>
        public static Uri GetMobileHubUrl()
        {
            return new Uri(Config.GetValueForSection(ConfigSection.AppiumMaqs, "MobileHubUrl"));
        }

        /// <summary>
        /// Get the wait timespan
        /// </summary>
        /// <returns>The wait time span</returns>
        public static TimeSpan GetMobileWaitTime()
        {
            return TimeSpan.FromMilliseconds(Convert.ToInt32(Config.GetValueForSection(ConfigSection.AppiumMaqs, "MobileWaitTime", "0")));
        }

        /// <summary>
        /// Get the timeout timespan
        /// </summary>
        /// <returns>The timeout time span</returns>
        public static TimeSpan GetMobileTimeout()
        {
            return TimeSpan.FromMilliseconds(Convert.ToInt32(Config.GetValueForSection(ConfigSection.AppiumMaqs, "MobileTimeout", "0")));
        }

        /// <summary>
        /// Get the initialize Appium timeout timespan
        /// </summary>
        /// <returns>The initialize timeout</returns>
        public static TimeSpan GetMobileCommandTimeout()
        {
            string value = Config.GetValueForSection(ConfigSection.AppiumMaqs, "MobileCommandTimeout", "60000");

            if (!int.TryParse(value, out int timeout))
            {
                throw new ArgumentException($"MobileCommandTimeout in {ConfigSection.AppiumMaqs} should be a number but the current value is: {value}");
            }

            return TimeSpan.FromMilliseconds(timeout);
        }

        /// <summary>
        /// Get if we should save page source on fail
        /// </summary>
        /// <returns>True if we want to save page source on fail</returns>
        public static bool GetSavePagesourceOnFail()
        {
            return Config.GetValueForSection(ConfigSection.AppiumMaqs, "SavePagesourceOnFail").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get if we should save screenshots on soft alert fails
        /// </summary>
        /// <returns>True if we want to save screenshots on soft alert fails</returns>
        public static bool GetSoftAssertScreenshot()
        {
            return Config.GetValueForSection(ConfigSection.AppiumMaqs, "SoftAssertScreenshot").Equals("Yes", StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Get the capabilities as a dictionary
        /// </summary>
        /// <returns>Dictionary of capabilities</returns>
        public static Dictionary<string, string> GetCapabilitiesAsStrings()
        {
            return Config.GetSectionDictionary(ConfigSection.AppiumCapsMaqs);
        }

        /// <summary>
        /// Get the capabilities as a dictionary
        /// </summary>
        /// <returns>Dictionary of capabilities</returns>
        public static Dictionary<string, object> GetCapabilitiesAsObjects()
        {
            return GetCapabilitiesAsStrings().ToDictionary(pair => pair.Key, pair => (object)pair.Value);
        }

        /// <summary>
        /// Get the remote browser type
        /// </summary>
        /// <returns>The remote browser type</returns>
        public static PlatformType GetDeviceType()
        {
            return GetDeviceType(GetPlatformName());
        }

        /// <summary>
        /// Get the platform type
        /// </summary>
        /// <param name="platform">Type of device</param>
        /// <returns>The platform type</returns>
        public static PlatformType GetDeviceType(string platform)
        {
            switch (platform.ToUpper().Trim())
            {
                case "ANDROID":
                    return PlatformType.Android;
                case "IOS":
                    return PlatformType.iOS;
                case "WIN":
                case "WINDOWS":
                    return PlatformType.Windows;
                default:
                    throw new ArgumentException($"Device type '{platform}' is not supported");
            }
        }
    }
}
