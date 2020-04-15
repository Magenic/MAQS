//--------------------------------------------------
// <copyright file="AppiumUtilities.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium utilities class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Static class for the appium utilities
    /// </summary>
    public static class AppiumUtilities
    {
        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="testObject">The TestObject to associate the screenshot with</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        public static bool CaptureScreenshot(this AppiumDriver<IWebElement> appiumDriver, AppiumTestObject testObject, string appendName = "")
        {
            try
            {
                if (!(testObject.Log is FileLogger))
                {
                    return false;
                }

                string fullpath = ((FileLogger)testObject.Log).FilePath;
                string directory = Path.GetDirectoryName(fullpath);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + appendName;
                CaptureScreenshot(appiumDriver, testObject, directory, fileNameWithoutExtension);

                testObject.Log.LogMessage(MessageType.INFORMATION, "Screenshot saved");
                return true;
            }
            catch (Exception exception)
            {
                exception = exception.InnerException ?? exception;
                testObject.Log.LogMessage(MessageType.ERROR, "Screenshot error: {0}", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="testObject">The TestObject to associate the screenshot with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        public static void CaptureScreenshot(this AppiumDriver<IWebElement> appiumDriver, AppiumTestObject testObject, string directory, string fileNameWithoutExtension)
        {
            Screenshot screenshot = ((ITakesScreenshot)appiumDriver).GetScreenshot();

            string path = Path.Combine(directory, fileNameWithoutExtension + ".png");
            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);

            testObject.AddAssociatedFile(path);
        }

        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="testObject">The TestObject to associate the file and log with</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        public static bool SavePageSource(this AppiumDriver<IWebElement> appiumDriver, AppiumTestObject testObject, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(testObject.Log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = SavePageSource(appiumDriver, testObject, LoggingConfig.GetLogDirectory(), "PageSource" + appendName);
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)testObject.Log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + "_PS" + appendName;

                    path = SavePageSource(appiumDriver, testObject, directory, fileNameWithoutExtension);
                }

                testObject.Log.LogMessage(MessageType.INFORMATION, "Page Source saved: " + path);
                return true;
            }
            catch (Exception exception)
            {
                testObject.Log.LogMessage(MessageType.ERROR, "Page Source error: {0}", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// To capture Page Source during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="testObject">The TestObject to associate the file and log with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        public static string SavePageSource(this AppiumDriver<IWebElement> appiumDriver, AppiumTestObject testObject, string directory, string fileNameWithoutExtension)
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

            testObject.AddAssociatedFile(path);
            return path;
        }

        /// <summary>
        /// Make sure the driver is shut down
        /// </summary>
        /// <param name="driver">The driver</param>
        public static void KillDriver(this AppiumDriver<IWebElement> driver)
        {
            try
            {
                driver?.Quit();
            }
            finally
            {
                driver?.Dispose();
            }
        }

        /// <summary>
        /// Get the wait default wait driver
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        /// <returns>An WebDriverWait</returns>
        public static WebDriverWait GetDefaultWaitDriver(AppiumDriver<IWebElement> driver)
        {
            return GetWaitDriver(driver, AppiumConfig.GetMobileTimeout(), AppiumConfig.GetMobileWaitTime());
        }

        /// <summary>
        /// Get the wait default wait driver
        /// </summary>
        /// <param name="driver">Brings in an AppiumDriver</param>
        /// <param name="timeoutTime">How long is the timeout</param>
        /// <param name="waitTime">How long to wait before rechecking</param>
        /// <returns>A web driver wait</returns>
        public static WebDriverWait GetWaitDriver(AppiumDriver<IWebElement> driver, TimeSpan timeoutTime, TimeSpan waitTime)
        {
            return new WebDriverWait(new SystemClock(), driver, timeoutTime, waitTime);
        }
    }
}