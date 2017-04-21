//--------------------------------------------------
// <copyright file="AppiumConfig.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
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

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Appium Configuration class
    /// </summary>
    public static class AppiumConfig
    {
        /// <summary>
        /// Get the mobile OS type
        /// </summary>
        /// <returns>The Mobile OS type</returns>
        public static string GetMobileDeviceOS()
        {
            return Config.GetValue("MobileOSType");
        }

        /// <summary>
        /// Get mobile device udid
        /// <para>If no udid is provide in the project configuration file we default to empty string</para>
        /// </summary>
        /// <returns>Device UDID string</returns>
        public static string GetMobileDeviceUDID()
        {
            return Config.GetValue("DeviceUDID");
        }

        /// <summary>
        /// Get the bundle id
        /// </summary>
        /// <returns>bundle id</returns>
        public static string GetBundleId()
        {
            return Config.GetValue("BundleID");
        }

        /// <summary>
        /// Get the OS Version
        /// </summary>
        /// <returns>OS Version</returns>
        public static string GetOSVersion()
        {
            return Config.GetValue("OSVersion");
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
        /// Whether or not the mobile browser is being used
        /// </summary>
        /// <returns>boolean for using browser</returns>
        public static bool UsingMobileBrowser()
        {
            if (Config.GetValue("MobileBrowser").ToUpper().Equals("YES"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the mobile device
        /// <para>If no browser is provide in the project configuration file we default to Android</para>
        /// </summary>
        /// <returns>The appium driver</returns>
        public static AppiumDriver<AppiumWebElement> MobileDevice()
        {
            return MobileDevice(GetMobileDeviceOS());
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
        /// Get the appium driver based for the provided mobile OS
        /// </summary>
        /// <param name="mobileDeviceOS">The browser type we want to use</param>
        /// <returns>An AppiumDriver</returns>
        public static AppiumDriver<AppiumWebElement> MobileDevice(string mobileDeviceOS)
        {
            AppiumDriver<AppiumWebElement> appiumDriver;
            switch (mobileDeviceOS.ToUpper())
            {
                case "ANDROID":
                    appiumDriver = new AndroidDriver<AppiumWebElement>(GetMobileHubUrl(), GetMobileCapabilities());
                    break;

                case "IOS":
                    appiumDriver = new IOSDriver<AppiumWebElement>(GetMobileHubUrl(), GetMobileCapabilities());
                    break;

                default:
                    throw new Exception(StringProcessor.SafeFormatter("Mobile OS type '{0}' is not supported", mobileDeviceOS));
            }

            return appiumDriver;
        }

        /// <summary>
        /// Get the wait default wait driver
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        /// <returns>An WebDriverWait</returns>
        public static WebDriverWait GetWaitDriver(AppiumDriver<AppiumWebElement> driver)
        {
            int waitTime = Convert.ToInt32(Config.GetValue("WaitTime", "0"));
            int timeoutTime = Convert.ToInt32(Config.GetValue("Timeout", "0"));

            return new WebDriverWait(new SystemClock(), driver, TimeSpan.FromMilliseconds(timeoutTime), TimeSpan.FromMilliseconds(waitTime));
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        public static void SetTimeouts(AppiumDriver<AppiumWebElement> driver)
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
            string mobileDeivceOS = GetMobileDeviceOS();

            switch (mobileDeivceOS.ToUpper())
            {
                case "ANDROID":
                    //capabilities = DesiredCapabilities.Android();
                    if (!GetAvdName().Equals(String.Empty))
                    {
                        capabilities.SetCapability("avd", GetAvdName());
                    }
                    break;

                case "IOS":
                    capabilities = DesiredCapabilities.IPhone();
                    capabilities.SetCapability("udid", GetMobileDeviceUDID());
                    break;

                default:
                    throw new Exception(StringProcessor.SafeFormatter("Mobile OS type '{0}' is not supported", mobileDeivceOS));
            }

            capabilities.SetCapability("deviceName", GetDeviceName());
            capabilities.SetCapability(CapabilityType.Version, GetOSVersion());
            //capabilities.SetCapability(MobileCapabilityType.PlatformVersion, GetOSVersion());
            capabilities.SetCapability(CapabilityType.Platform, GetMobileDeviceOS().ToUpper());

            if (UsingMobileBrowser())
            {
                switch (mobileDeivceOS.ToUpper())
                {
                    case "ANDROID":
                        capabilities.SetCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Chrome);
                        break;

                    case "IOS":

                        capabilities.SetCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Safari);
                        capabilities.SetCapability(CapabilityType.Platform, "MAC");
                        break;
                }
            }
            else
            {
                capabilities.SetCapability("appId", GetBundleId());
                //capabilities.SetCapability(CapabilityType.BrowserName, GetMobileDeviceOS().ToUpper());
                capabilities.SetCapability(MobileCapabilityType.AutomationName, "Appium");
                
            }

            return capabilities;
        }

        /// <summary>
        /// Gets the AVD Name
        /// </summary>
        /// <returns>AVD Name as a String</returns>
        public static String GetAvdName()
        {
            return Config.GetValue("AVDName");
        }
    }
}