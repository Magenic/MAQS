//--------------------------------------------------
// <copyright file="AppiumConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium Configuration class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Specialized;
using System.Configuration;
using OpenQA.Selenium;

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Appium Configuration class
    /// </summary>
    public static class AppiumConfig
    {
        /// <summary>
        ///  Static field for AppiumCapsMaqs configuration section.
        /// </summary>
        private static string mobileCapabilities = "AppiumCapsMaqs";
        
        /// <summary>
        /// Get the mobile OS type
        /// </summary>
        /// <returns>The Mobile OS type</returns>
        public static string GetPlatformName()
        {
            return Config.GetValue("PlatformName");
        }

        /// <summary>
        /// Get the OS Version
        /// </summary>
        /// <returns>OS Version</returns>
        public static string GetPlatformVersion()
        {
            return Config.GetValue("PlatformVersion");
        }

        /// <summary>
        /// Get the Device Name
        /// </summary>
        /// <returns>Device Name</returns>
        public static string GetDeviceName()
        {
            return Config.GetValue("DeviceName");
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
            return new Uri(Config.GetValue("MobileHubUrl"));
        }

        /// <summary>
        /// Get the initialize Appium timeout timespan
        /// </summary>
        /// <returns>The initialize timeout</returns>
        public static TimeSpan GetCommandTimeout()
        {
            int timeout;

            string value = Config.GetValue("AppiumCommandTimeout", "60");
            
            if (!int.TryParse(value, out timeout))
            {
                throw new Exception("AppiumCommandTimeout should be a number but the current value is: " + value);
            }

            return TimeSpan.FromSeconds(timeout);
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
                    appiumDriver = new AndroidDriver<AppiumWebElement>(GetMobileHubUrl(), GetMobileCapabilities(), GetCommandTimeout());
                    break;

                case "IOS":
                    appiumDriver = new IOSDriver<AppiumWebElement>(GetMobileHubUrl(), GetMobileCapabilities(), GetCommandTimeout());
                    break;

                default:
                    throw new Exception(StringProcessor.SafeFormatter("Mobile OS type '{0}' is not supported", platformName));
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
            int waitTime = Convert.ToInt32(Config.GetValue("WaitTime", "0"));
            int timeoutTime = Convert.ToInt32(Config.GetValue("Timeout", "0"));

            return new WebDriverWait(new SystemClock(), driver, TimeSpan.FromMilliseconds(timeoutTime), TimeSpan.FromMilliseconds(waitTime));
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        public static void SetTimeouts(AppiumDriver<IWebElement> driver)
        {
            int timeoutTime = Convert.ToInt32(Config.GetValue("Timeout", "0"));
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMilliseconds(timeoutTime);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromMilliseconds(timeoutTime);
        }

        /// <summary>
        /// Get the mobile desired capability
        /// </summary>
        /// <returns>The mobile desired capability</returns>
        private static DesiredCapabilities GetMobileCapabilities()
        {
            DesiredCapabilities capabilities = new DesiredCapabilities();
            capabilities.SetCapability(MobileCapabilityType.DeviceName, GetDeviceName());
            capabilities.SetCapability(MobileCapabilityType.PlatformVersion, GetPlatformVersion());
            capabilities.SetCapability(MobileCapabilityType.PlatformName, GetPlatformName().ToUpper());
            capabilities.SetMobileCapabilities();
            
            return capabilities;
        }

        /// <summary>
        /// Sets mobile specific capabilities from the configuration
        /// </summary>
        /// <param name="desiredCapabilities"> Capabilities object passed in</param>
        /// <returns>Custom mobile capabilities object</returns>
        private static DesiredCapabilities SetMobileCapabilities(this DesiredCapabilities desiredCapabilities)
        {
            var mobileCapabilitySection = ConfigurationManager.GetSection(mobileCapabilities) as NameValueCollection;
            if (mobileCapabilitySection == null)
            {
                return desiredCapabilities;
            }

            var keys = mobileCapabilitySection.AllKeys;
            foreach (var key in keys)
            {
                if (mobileCapabilitySection[key].Length > 0)
                {
                    desiredCapabilities.SetCapability(key, mobileCapabilitySection[key]);
                }
            }
            return desiredCapabilities;
        }
    }
}
