//--------------------------------------------------
// <copyright file="SeleniumUtilities.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Utilities class for generic selenium methods</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using Selenium.Axe;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Static class for the selenium utilities
    /// </summary>
    public static class SeleniumUtilities
    {
        /// <summary>
        /// Capture a screenshot during execution and associate to the testObject
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The test object to associate and log to</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        public static bool CaptureScreenshot(this IWebDriver webDriver, SeleniumTestObject testObject, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(testObject.Log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = CaptureScreenshot(webDriver, testObject, LoggingConfig.GetLogDirectory(), "ScreenCap" + appendName, GetScreenShotFormat());
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)testObject.Log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + appendName;
                    path = CaptureScreenshot(webDriver, testObject, directory, fileNameWithoutExtension, GetScreenShotFormat());
                }

                testObject.Log.LogMessage(MessageType.INFORMATION, "Screenshot saved: " + path);
                return true;
            }
            catch (Exception exception)
            {
                testObject.Log.LogMessage(MessageType.ERROR, "Screenshot error: {0}", exception.ToString());
                return false;
            }
        }

        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The test object to associate the screenshot with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <param name="imageFormat">Optional Screenshot Image format parameter; Default imageFormat is PNG</param>
        /// <returns>Path to the log file</returns>
        public static string CaptureScreenshot(this IWebDriver webDriver, SeleniumTestObject testObject, string directory, string fileNameWithoutExtension, ScreenshotImageFormat imageFormat = ScreenshotImageFormat.Png)
        {
            Screenshot screenShot = ((ITakesScreenshot)webDriver).GetScreenshot();

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, fileNameWithoutExtension + "." + imageFormat.ToString());

            // Save the screenshot
            screenShot.SaveAsFile(path, imageFormat);
            testObject.AddAssociatedFile(path);

            return path;
        }

        /// <summary>
        /// To capture a screenshot during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the image was successful</returns>
        [Obsolete("CaptureScreenshot that does not take a SeleniumTestObject parameter is deprecated")]
        public static bool CaptureScreenshot(this IWebDriver webDriver, Logger log, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = CaptureScreenshot(webDriver, LoggingConfig.GetLogDirectory(), "ScreenCap" + appendName, GetScreenShotFormat());
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + appendName;
                    path = CaptureScreenshot(webDriver, directory, fileNameWithoutExtension, GetScreenShotFormat());
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
        /// <param name="imageFormat">Optional Screenshot Image format parameter; Default imageFormat is PNG</param>
        /// <returns>Path to the log file</returns>
        [Obsolete("CaptureScreenshot that does not take a SeleniumTestObject parameter is deprecated")]
        public static string CaptureScreenshot(this IWebDriver webDriver, string directory, string fileNameWithoutExtension, ScreenshotImageFormat imageFormat = ScreenshotImageFormat.Png)
        {
            Screenshot screenShot = ((ITakesScreenshot)webDriver).GetScreenshot();

            // Make sure the directory exists
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Calculate the file name
            string path = Path.Combine(directory, fileNameWithoutExtension + "." + imageFormat.ToString());

            // Save the screenshot
            screenShot.SaveAsFile(path, imageFormat);

            return path;
        }

        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the file with</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        public static bool SavePageSource(this IWebDriver webDriver, SeleniumTestObject testObject, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(testObject.Log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = SavePageSource(webDriver, testObject, LoggingConfig.GetLogDirectory(), "PageSource" + appendName);
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)testObject.Log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + "_PS" + appendName;

                    path = SavePageSource(webDriver, testObject, directory, fileNameWithoutExtension);
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
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the file with</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        public static string SavePageSource(this IWebDriver webDriver, SeleniumTestObject testObject, string directory, string fileNameWithoutExtension)
        {
            // Save the current page source into a string
            string pageSource = webDriver.PageSource;

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
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="testObject">The test object which contains the web driver and logger you wish to use</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CheckAccessibility(this SeleniumTestObject testObject, bool throwOnViolation = false)
        {
            CheckAccessibility(testObject.WebDriver, testObject.Log, throwOnViolation);
        }

        /// <summary>
        /// Run axe accessibility and log the results 
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CheckAccessibility(this IWebDriver webDriver, Logger logger, bool throwOnViolation = false)
        {
            MessageType type = logger.GetLoggingLevel();

            // Look at passed
            if (type == MessageType.VERBOSE && GetReadableAxeResults("Passes", webDriver, webDriver.Analyze().Passes, out string axeText))
            {
                logger.LogMessage(MessageType.VERBOSE, axeText);
            }

            // Look at inapplicable
            if (type == MessageType.VERBOSE && GetReadableAxeResults("Inapplicable", webDriver, webDriver.Analyze().Inapplicable, out axeText))
            {
                logger.LogMessage(MessageType.VERBOSE, axeText);
            }

            // Look at incomplete
            if (type > MessageType.SUCCESS && GetReadableAxeResults("Incomplete", webDriver, webDriver.Analyze().Incomplete, out axeText))
            {
                logger.LogMessage(MessageType.INFORMATION, axeText);
            }

            // Look at violations
            if (GetReadableAxeResults("Violations", webDriver, webDriver.Analyze().Violations, out axeText))
            {
                if (throwOnViolation)
                {
                    throw new ApplicationException(axeText);
                }
                else
                {
                    logger.LogMessage(MessageType.WARNING, axeText);
                }
            }
        }

        /// <summary>
        /// Parses scanned accessibility results
        /// </summary>
        /// <param name="typeOfScan">Type of scan</param>
        /// <param name="webDriver">Web driver the scan was run on</param>
        /// <param name="scannedResults">The scan results</param>
        /// <param name="messageString">Pretty scan results</param>
        /// <returns>True if the scan found anything</returns>
        public static bool GetReadableAxeResults(string typeOfScan, IWebDriver webDriver, AxeResultItem[] scannedResults, out string messageString)
        {
            StringBuilder message = new StringBuilder();
            int axeRules = scannedResults.Length;

            if (axeRules == 0)
            {
                messageString = string.Empty;
                return false;
            }

            message.AppendLine($"ACCESSIBILITY CHECK");
            message.AppendLine($"{typeOfScan} check for '{webDriver.Url}'");
            message.AppendLine($"Found {axeRules} items");
            message.AppendLine(string.Empty);

            int loops = 1;

            foreach (var element in scannedResults)
            {
                message.AppendLine($@"{loops++}: {element.Help}");
                message.AppendLine($@"{"\t"}Description: {element.Description}");
                message.AppendLine($@"{"\t"}Help URL: {element.HelpUrl}");
                message.AppendLine($@"{"\t"}Impact: {element.Impact}");
                message.AppendLine($@"{"\t"}Tags: {string.Join(", ", element.Tags)}");

                foreach (var item in element.Nodes)
                {
                    message.AppendLine($@"{"\t"}{"\t"}HTML element: {item.Html}");

                    foreach (string target in item.Target)
                    {
                        message.AppendLine($@"{"\t"}{"\t"}Selector: {target}");
                    }
                }

                message.AppendLine(string.Empty);
                message.AppendLine(string.Empty);
            }

            messageString = message.ToString().Trim();
            return true;
        }

        /// <summary>
        /// To capture a page source during execution
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="log">The logger being used</param>
        /// <param name="appendName">Appends a name to the end of a filename</param>
        /// <returns>Boolean if the save of the page source was successful</returns>
        [Obsolete("SavePageSource that does not take a SeleniumTestObject parameter is deprecated")]
        public static bool SavePageSource(this IWebDriver webDriver, Logger log, string appendName = "")
        {
            try
            {
                string path = string.Empty;

                // Check if we are using a file logger
                if (!(log is FileLogger))
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = SavePageSource(webDriver, LoggingConfig.GetLogDirectory(), "PageSource" + appendName);
                }
                else
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + "_PS" + appendName;

                    path = SavePageSource(webDriver, directory, fileNameWithoutExtension);
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
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="directory">The directory file path</param>
        /// <param name="fileNameWithoutExtension">Filename without extension</param>
        /// <returns>Path to the log file</returns>
        [Obsolete("SavePageSource that does not take a SeleniumTestObject parameter is deprecated")]
        public static string SavePageSource(this IWebDriver webDriver, string directory, string fileNameWithoutExtension)
        {
            // Save the current page source into a string
            string pageSource = webDriver.PageSource;

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

        /// <summary>
        /// Get the javaScript executor from a web element or web driver
        /// </summary>
        /// <param name="searchContext">The search context</param>
        /// <returns>The javaScript executor</returns>
        public static IJavaScriptExecutor SearchContextToJavaScriptExecutor(ISearchContext searchContext)
        {
            return (searchContext is IJavaScriptExecutor) ? (IJavaScriptExecutor)searchContext : (IJavaScriptExecutor)WebElementToWebDriver((IWebElement)searchContext);
        }

        /// <summary>
        /// Get the web driver from a web element or web driver
        /// </summary>
        /// <param name="searchContext">The search context</param>
        /// <returns>The web driver</returns>
        public static IWebDriver SearchContextToWebDriver(ISearchContext searchContext)
        {
            return (searchContext is IWebDriver) ? (IWebDriver)searchContext : WebElementToWebDriver((IWebElement)searchContext);
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

            // Get the parent driver - this is a protected property so we need to user reflection to access it
            var eventFiringPropertyInfo = element.GetType().GetProperty("ParentDriver", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty);

            if (eventFiringPropertyInfo != null)
            {
                // This means we are using an event firing web driver
                driver = (IWebDriver)eventFiringPropertyInfo.GetValue(element, null);
            }
            else
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

        /// <summary>
        /// Gets the Screenshot Format to save images
        /// </summary>
        /// <returns>Desired ImageFormat Type</returns>
        public static ScreenshotImageFormat GetScreenShotFormat()
        {
            switch (SeleniumConfig.GetImageFormat().ToUpper())
            {
                case "BMP":
                    return ScreenshotImageFormat.Bmp;
                case "GIF":
                    return ScreenshotImageFormat.Gif;
                case "JPEG":
                    return ScreenshotImageFormat.Jpeg;
                case "PNG":
                    return ScreenshotImageFormat.Png;
                case "TIFF":
                    return ScreenshotImageFormat.Tiff;
                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter("ImageFormat '{0}' is not a valid option", SeleniumConfig.GetImageFormat()));
            }
        }

        /// <summary>
        /// Make sure the web driver is shut down
        /// </summary>
        /// <param name="driver">The web driver</param>
        public static void KillDriver(this IWebDriver driver)
        {
            try
            {
                driver?.Close();
            }
            finally
            {
                driver?.Quit();
            }
        }

        /// <summary>
        /// Set the script and page timeouts using the default configuration timeout
        /// </summary>
        /// <param name="driver">Driver who's timeouts you want set</param>
        public static void SetTimeouts(IWebDriver driver)
        {
            SetTimeouts(driver, SeleniumConfig.GetTimeoutTime());
        }

        /// <summary>
        /// Set the script and page timeouts
        /// </summary>
        /// <param name="driver">Driver who's timeouts you want set</param>
        /// <param name="timeoutTime">Page load and JavaScript timeouts</param>
        public static void SetTimeouts(IWebDriver driver, TimeSpan timeoutTime)
        {
            driver.Manage().Timeouts().PageLoad = timeoutTime;
            driver.Manage().Timeouts().AsynchronousJavaScript = timeoutTime;
        }
    }
}
