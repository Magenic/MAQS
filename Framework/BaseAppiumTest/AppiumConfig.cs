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

        /*/// <summary>
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
        }*/

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

        /*/// <summary>
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
        /// Gets the AVD Name
        /// </summary>
        /// <returns>AVD Name as a String</returns>
        public static String GetAvdName()
        {
            return Config.GetValue("AVDName");
        }*/

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

        /*/// <summary>
        /// Get App Activity
        /// </summary>
        /// <returns>String of App Activity</returns>
        public static String GetAppActivity()
        {
            return Config.GetValue("AppStartActivity");
        }

        /// <summary>
        /// Get App Path
        /// </summary>
        /// <returns>String of app path value</returns>
        public static String GetAppPath()
        {
            return Config.GetValue("AppPath");
        }
*/
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
                    appiumDriver = new AndroidDriver<IWebElement>(GetMobileHubUrl(), GetMobileCapabilities());
                    break;

                case "IOS":
                    appiumDriver = new IOSDriver<IWebElement>(GetMobileHubUrl(), GetMobileCapabilities());
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
            capabilities.SetCapability(MobileCapabilityType.DeviceName, GetDeviceName());
            capabilities.SetCapability(MobileCapabilityType.PlatformVersion, GetPlatformVersion());
            //capabilities.SetCapability(CapabilityType.Version, GetPlatformVersion());
            capabilities.SetCapability(MobileCapabilityType.PlatformName, GetPlatformName().ToUpper());
            
            //string mobileDeivceOS = GetMobileDeviceOS();

            /*switch (mobileDeivceOS.ToUpper())
            {
                case "ANDROID":

                    if (!GetAvdName().Equals(string.Empty))
                    {
                        capabilities.SetCapability(AndroidMobileCapabilityType.Avd, GetAvdName());
                    }
                    else if (UsingMobileBrowser())
                    {
                    capabilities.SetCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Chrome);
                    } 

                        if (Config.DoesKeyExist("AppStartActivity") && !GetAppActivity().Equals(string.Empty) && !UsingMobileBrowser())
                        {
                        capabilities.SetCapability(AndroidMobileCapabilityType.AppPackage, GetBundleId());
                        capabilities.SetCapability(AndroidMobileCapabilityType.AppActivity, GetAppActivity());
                         }
                        
                    break;

                case "IOS":
                    capabilities.SetCapability(CapabilityType.Platform, "MAC");
                    capabilities.SetCapability(MobileCapabilityType.AutomationName, "XCUITest");
                    capabilities.SetCapability(MobileCapabilityType.Udid, GetMobileDeviceUDID());
                    if (UsingMobileBrowser())
                    {
                        capabilities.SetCapability(MobileCapabilityType.BrowserName, MobileBrowserType.Safari);
                    }
                    else
                    {
                        if (Config.DoesKeyExist("BundleID") && !GetBundleId().Equals(string.Empty))
                        {
                            capabilities.SetCapability(IOSMobileCapabilityType.BundleId, GetBundleId());
                        }
                    }

                    break;

                default:
                    throw new Exception(StringProcessor.SafeFormatter("Mobile OS type '{0}' is not supported", mobileDeivceOS));
            }

            if (Config.DoesKeyExist("AppPath") && !GetAppPath().Equals(string.Empty))
            {
                capabilities.SetCapability(MobileCapabilityType.App, GetAppPath());
            }*/

            capabilities.SetMobileCapabilities();
            
            return capabilities;
        }

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