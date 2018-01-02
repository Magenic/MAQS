//--------------------------------------------------
// <copyright file="AppiumUtilities.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium utilities class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.IO;

namespace Magenic.MaqsFramework.BaseAppiumTest
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
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        public static bool CaptureScreenshot(this AppiumDriver<AppiumWebElement> appiumDriver, Logger log, string appendName = "")
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
        public static void CaptureScreenshot(this AppiumDriver<AppiumWebElement> appiumDriver, string directory, string fileNameWithoutExtension)
        {
            Screenshot screenshot = ((ITakesScreenshot)appiumDriver).GetScreenshot();

            string path = Path.Combine(directory, fileNameWithoutExtension + ".png");

            screenshot.SaveAsFile(path, ScreenshotImageFormat.Png);
        }

        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="appiumDriver">The AppiumDriver</param>
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        /// <example>
        /// <code source = "../AppiumUnitTests/AppiumUtilitiesTests.cs" region="SavePageSource" lang="C#" />
        /// </example>
        public static bool SavePageSource(this AppiumDriver<AppiumWebElement> appiumDriver, Logger log, string appendName = "")
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
        public static string SavePageSource(this AppiumDriver<AppiumWebElement> appiumDriver, string directory, string fileNameWithoutExtension)
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
    }
}