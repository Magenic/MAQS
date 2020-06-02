//--------------------------------------------------
// <copyright file="SeleniumUtilities.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Utilities class for generic selenium methods</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
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
                var htmlLogger = testObject.Log as HtmlFileLogger;
                var fileLogger = testObject.Log as FileLogger;

                // Check if we are using an HTMl logger
                if (htmlLogger != null)
                {
                    var writer = new StreamWriter(htmlLogger.FilePath, true);

                    // Since this is a HTML File logger we need to add a card with the image in it
                    writer.WriteLine(StringProcessor.SafeFormatter(
                        "<div class='collapse col-12 show' data-logtype='IMAGE'><div class='card'><div class='card-body'><h6 class='card-subtitle mb-1'>2020-01-16 18:57:47.184-05:00</h6></div><a class='pop' href='#'><img class='card-img-top rounded' src='data:image/png;base64, {0}'style='width: 200px;'></a></div></div>",
                        ((ITakesScreenshot)webDriver).GetScreenshot().AsBase64EncodedString));
                    writer.Flush();
                    writer.Close();
                } // Check if we are using a file logger
                else if (fileLogger != null)
                {
                    // Calculate the file name
                    string fullpath = ((FileLogger)testObject.Log).FilePath;
                    string directory = Path.GetDirectoryName(fullpath);
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullpath) + appendName;
                    path = CaptureScreenshot(webDriver, testObject, directory, fileNameWithoutExtension, GetScreenShotFormat());
                }
                else
                {
                    // Since this is not a file logger we will need to use a generic file name
                    path = CaptureScreenshot(webDriver, testObject, LoggingConfig.GetLogDirectory(), "ScreenCap" + appendName, GetScreenShotFormat());
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
        /// Create a HTML accessibility report for an entire web page
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CreateAccessibilityHtmlReport(this IWebDriver webDriver, SeleniumTestObject testObject, bool throwOnViolation = false)
        {
            CreateAccessibilityHtmlReport(webDriver, testObject, () => webDriver.Analyze(), throwOnViolation);
        }

        /// <summary>
        /// Create a HTML accessibility report for a specific web element and all of it's children
        /// </summary>
        /// <param name="webDriver">The WebDriver</param>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <param name="element">The WebElement you want to use as the root for your accessibility scan</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CreateAccessibilityHtmlReport(this IWebDriver webDriver, SeleniumTestObject testObject, IWebElement element, bool throwOnViolation = false)
        {
            // If we are using a lazy element go get the raw element instead
            LazyElement raw = element as LazyElement;

            if (raw != null)
            {
                element = ((LazyElement)element).GetRawExistingElement();
            }

            CreateAccessibilityHtmlReport(element, testObject, () => webDriver.Analyze(element), throwOnViolation);
        }

        /// <summary>
        /// Create a HTML accessibility report
        /// </summary>
        /// <param name="context">The scan context, this is either a web driver or web element</param>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <param name="getResults">Function for getting the accessibility scan results</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CreateAccessibilityHtmlReport(this ISearchContext context, SeleniumTestObject testObject, Func<AxeResult> getResults, bool throwOnViolation = false)
        {
            // If we are using a lazy element go get the raw element instead
            LazyElement raw = context as LazyElement;

            if (raw != null)
            {
                context = ((LazyElement)context).GetRawExistingElement();
            }

            // Check to see if the logger is not verbose and not already suspended
            bool restoreLogging = testObject.Log.GetLoggingLevel() != MessageType.VERBOSE && testObject.Log.GetLoggingLevel() != MessageType.SUSPENDED;

            AxeResult results;
            string report = GetAccessibilityReportPath(testObject);
            testObject.Log.LogMessage(MessageType.INFORMATION, "Running accessibility check");

            try
            {
                // Suspend logging if we are not verbose or already suspended
                if (restoreLogging)
                {
                    testObject.Log.SuspendLogging();
                }

                results = getResults();
                context.CreateAxeHtmlReport(results, report);
            }
            finally
            {
                // Restore logging if we suspended it
                if(restoreLogging)
                {
                    testObject.Log.ContinueLogging();
                }
            }

            // Add the report
            testObject.AddAssociatedFile(report);
            testObject.Log.LogMessage(MessageType.INFORMATION, $"Ran accessibility check and created HTML report: {report} ");

            // Throw exception if we found violations and we want that to cause an error
            if (throwOnViolation &&  results.Violations.Length > 0)
            {
                throw new ApplicationException($"Accessibility violations, see: {report} for more details.");
            }

            // Throw exception if the accessibility check had any errors
            if (results.Error.Length > 0)
            {
                throw new ApplicationException($"Accessibility check failure, see: {report} for more details.");
            }
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
            if (type >= MessageType.SUCCESS)
            {
                CheckAccessibilityPasses(webDriver, logger, MessageType.SUCCESS);
            }

            // Look at incomplete
            if (type >= MessageType.INFORMATION)
            {
                CheckAccessibilityIncomplete(webDriver, logger, MessageType.INFORMATION);
            }

            // Look at inapplicable
            if (type >= MessageType.VERBOSE)
            {
                CheckAccessibilityInapplicable(webDriver, logger, MessageType.VERBOSE);
            }

            // Look at violations
            MessageType messageType = throwOnViolation ? MessageType.ERROR : MessageType.WARNING;
            CheckAccessibilityViolations(webDriver, logger, messageType, throwOnViolation);
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="checkType">What kind of check is being run</param>
        /// <param name="getResults">Function for getting Axe results</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnResults">Throw error if any results are found</param>
        public static void CheckAccessibility(this IWebDriver webDriver, Logger logger, string checkType, Func<AxeResultItem[]> getResults, MessageType loggingLevel, bool throwOnResults = false)
        {
            logger.LogMessage(MessageType.INFORMATION, "Running accessibility check");

            if (GetReadableAxeResults(checkType, webDriver, getResults(), out string axeText) && throwOnResults)
            {
                throw new ApplicationException(axeText);
            }
            else
            {
                logger.LogMessage(loggingLevel, axeText);
            }
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        public static void CheckAccessibilityPasses(this IWebDriver webDriver, Logger logger, MessageType loggingLevel)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Passes.ToString(), () => webDriver.Analyze().Passes, loggingLevel);
        }

        ///AccessibilityCheckType
        ///
        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnInapplicable">Should inapplicable cause and exception to be thrown</param>
        public static void CheckAccessibilityInapplicable(this IWebDriver webDriver, Logger logger, MessageType loggingLevel, bool throwOnInapplicable = false)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Inapplicable.ToString(), () => webDriver.Analyze().Inapplicable, loggingLevel, throwOnInapplicable);
        }

        ///AccessibilityCheckType
        ///
        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnIncomplete">Should incomplete cause and exception to be thrown</param>
        public static void CheckAccessibilityIncomplete(this IWebDriver webDriver, Logger logger, MessageType loggingLevel, bool throwOnIncomplete = false)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Incomplete.ToString(), () => webDriver.Analyze().Incomplete, loggingLevel, throwOnIncomplete);
        }

        /// <summary>
        /// Run axe accessibility and log the results
        /// </summary>
        /// <param name="webDriver">The web driver that is on the page you want to run the accessibility check on</param>
        /// <param name="logger">Where you want the check logged to</param>
        /// <param name="loggingLevel">What level should logging the check take, this gets used if the check doesn't throw an exception</param>
        /// <param name="throwOnViolation">Should violations cause and exception to be thrown</param>
        public static void CheckAccessibilityViolations(this IWebDriver webDriver, Logger logger, MessageType loggingLevel, bool throwOnViolation = false)
        {
            CheckAccessibility(webDriver, logger, AccessibilityCheckType.Violations.ToString(), () => webDriver.Analyze().Violations, loggingLevel, throwOnViolation);
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

            message.AppendLine($"ACCESSIBILITY CHECK");
            message.AppendLine($"{typeOfScan} check for '{webDriver.Url}'");
            message.AppendLine($"Found {axeRules} items");

            if (axeRules == 0)
            {
                messageString = message.ToString().Trim();
                return false;
            }

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
            IWebDriver driver;
            
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

        /// <summary>
        /// Get a unique file name that we can user for the accessibility HTML report
        /// </summary>
        /// <param name="testObject">The TestObject to associate the report with</param>
        /// <returns>A unique HTML file name, includes full path</returns>
        private static string GetAccessibilityReportPath(SeleniumTestObject testObject)
        {
            string logDirectory = testObject.Log is FileLogger ? Path.GetDirectoryName(((FileLogger)testObject.Log).FilePath) : LoggingConfig.GetLogDirectory();
            string reportBaseName = testObject.Log is FileLogger ? Path.GetFileNameWithoutExtension(((FileLogger)testObject.Log).FilePath) + "_Axe" : "AxeReport";
            string reportFile = Path.Combine(logDirectory, reportBaseName + ".html");
            int reportNumber = 0;

            while (File.Exists(reportFile))
            {
                reportFile = Path.Combine(logDirectory, reportBaseName + reportNumber++ + ".html");
            }

            return reportFile;
        }
    }
}
