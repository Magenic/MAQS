//--------------------------------------------------
// <copyright file="SeleniumUtilities.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Utilities class for generic selenium methods</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.Events;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Static class for the selenium utilities
    /// </summary>
    public static class SeleniumUtilities
    {
        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="CaptureScreenshot" lang="C#" />
        /// </example>
        public static bool CaptureScreenshot(this IWebDriver webDriver, Logger log, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = CaptureScreenshot(webDriver, LoggingConfig.GetLogDirectory(), "ScreenCap" + appendName);
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + appendName;
                    path = CaptureScreenshot(webDriver, directory, fileNameWithoutExtension);
                }

                log.LogMessage(MessageType.INFORMATION, "Screenshot saved: " + path);
                return true;
            }
            catch (Exception exception)
            {
                log.LogMessage(MessageType.ERROR, "Screenshot error: {0}", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        public static string CaptureScreenshot(this IWebDriver webDriver, string directory, string fileNameWithoutExtension)
        {
            Screenshot screenShot = ((ITakesScreenshot)webDriver).GetScreenshot();

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, fileNameWithoutExtension + ".png");

            // Save the screenshot
            screenShot.SaveAsFile(path, ImageFormat.Png);

            return path;
        }

        /// <summary>
        /// Get the javaScript executor from a web element or web driver
        /// </summary>
        /// <param name="searchContext">The search context</param>
        /// <returns>The javaScript executor</returns>
        public static IJavaScriptExecutor SearchContextToJavaScriptExecutor(ISearchContext searchContext)
        {
            return (searchContext is IJavaScriptExecutor) ? (IJavaScriptExecutor)searchContext : (IJavaScriptExecutor)SeleniumUtilities.WebElementToWebDriver((IWebElement)searchContext);
        }

        /// <summary>
        /// Get the web driver from a web element or web driver
        /// </summary>
        /// <param name="searchContext">The search context</param>
        /// <returns>The web driver</returns>
        public static IWebDriver SearchContextToWebDriver(ISearchContext searchContext)
        {
            return (searchContext is IWebDriver) ? (IWebDriver)searchContext : SeleniumUtilities.WebElementToWebDriver((IWebElement)searchContext);
        }

        /// <summary>
        /// Get the web driver from a web element
        /// </summary>
        /// <param name="element">The web element</param>
        /// <returns>The web driver</returns>
        public static IWebDriver WebElementToWebDriver(IWebElement element)
        {
            // Extract the web driver from the element
            IWebDriver driver = null;

            try
            {
                // Get the parent driver - this is a protected property so we need to user reflection to access it
                var eventFiringPropertyInfo = element.GetType().GetProperty("ParentDriver", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

                if (eventFiringPropertyInfo != null)
                {
                    // This means we are using an event firing web driver
                    driver = (IWebDriver)eventFiringPropertyInfo.GetValue(element, null);
                }
            }
            catch
            {
                // We failed to get the event firing web driver so they to get the wrapped web driver
                var propertyInfo = element.GetType().GetProperty("WrappedElement");

                if (propertyInfo != null)
                {
                    // This means we are likely using an event firing web driver
                    var value = (IWebElement)propertyInfo.GetValue(element, null);
                    driver = ((IWrapsDriver)value).WrappedDriver;
                }
                else
                {
                    driver = ((IWrapsDriver)element).WrappedDriver;
                }
            }

            return driver;
        }
    }
}
