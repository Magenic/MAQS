//--------------------------------------------------
// <copyright file="AppiumUtilities.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
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
    }
}