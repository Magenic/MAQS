//--------------------------------------------------
// <copyright file="SeleniumDriverManager.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Selenium driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.Events;
using System;
using System.Reflection;
using System.Text;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Selenium driver store
    /// </summary>
    public class SeleniumDriverManager : DriverManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumDriverManager"/> class
        /// </summary>
        /// <param name="getDriver">Function for getting an Selenium web driver</param>
        /// <param name="testObject">The associated test object</param>
        public SeleniumDriverManager(Func<IWebDriver> getDriver, BaseTestObject testObject) : base(getDriver, testObject)
        {
        }

        /// <summary>
        /// Have the driver cleanup after itself
        /// </summary>
        public override void Dispose()
        {
            this.Log.LogMessage(MessageType.VERBOSE, "Start dispose driver");

            // If we never created the driver we don't have any cleanup to do
            if (!this.IsDriverIntialized())
            {
                return;
            }

            try
            {
                IWebDriver driver = this.GetWebDriver();
                driver?.KillDriver();
            }
            catch (Exception e)
            {
                this.Log.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to close web driver because: {0}", e.Message));
            }

            this.BaseDriver = null;
            this.Log.LogMessage(MessageType.VERBOSE, "End dispose driver");
        }

        /// <summary>
        /// Get the web driver
        /// </summary>
        /// <returns>The web driver</returns>
        public IWebDriver GetWebDriver()
        {
            IWebDriver tempDriver;

            if (!this.IsDriverIntialized() && LoggingConfig.GetLoggingEnabledSetting() != LoggingEnabled.NO)
            {
                tempDriver = this.GetDriver() as IWebDriver;
                tempDriver = new EventFiringWebDriver(tempDriver);
                this.MapEvents(tempDriver as EventFiringWebDriver);
                this.BaseDriver = tempDriver;

                // Log the setup
                this.LoggingStartup(tempDriver);
            }

            return this.GetBase() as IWebDriver;
        }

        /// <summary>
        /// Get the web driver
        /// </summary>
        /// <returns>The web driver</returns>
        public override object Get()
        {
            return this.GetWebDriver();
        }

        /// <summary>
        /// Log a verbose message and include the automation specific call stack data
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        protected void LogVerbose(string message, params object[] args)
        {
            StringBuilder messages = new StringBuilder();
            messages.AppendLine(StringProcessor.SafeFormatter(message, args));

            var methodInfo = MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;

            foreach (string stackLevel in Environment.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                string trimmed = stackLevel.Trim();
                if (!trimmed.StartsWith("at Microsoft.") && !trimmed.StartsWith("at System.") && !trimmed.StartsWith("at NUnit.") && !trimmed.StartsWith("at " + fullName))
                {
                    messages.AppendLine(stackLevel);
                }
            }

            this.Log.LogMessage(MessageType.VERBOSE, messages.ToString());
        }

        /// <summary>
        /// Log that the web driver setup
        /// </summary>
        /// <param name="webDriver">The web driver</param>
        private void LoggingStartup(IWebDriver webDriver)
        {
            try
            {
                IWebDriver driver = Extend.GetLowLevelDriver(webDriver);
                string browserType;

                // Get info on what type of brower we are using
                if (driver is RemoteWebDriver)
                {
                    browserType = ((RemoteWebDriver)driver).Capabilities.ToString();
                }
                else
                {
                    browserType = driver.GetType().ToString();
                }

                if (SeleniumConfig.GetBrowserName().Equals("Remote", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Remote driver: " + browserType);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Local driver: " + browserType);
                }

                webDriver.SetWaitDriver(SeleniumConfig.GetWaitDriver(webDriver));
            }
            catch (Exception e)
            {
                this.Log.LogMessage(MessageType.ERROR, "Failed to start driver because: {0}", e.Message);
                Console.WriteLine(StringProcessor.SafeFormatter("Failed to start driver because: {0}", e.Message));
            }
        }

        /// <summary>
        /// Map selenium events to log events
        /// </summary>
        /// <param name="eventFiringDriver">The event firing web driver that we want mapped</param>
        private void MapEvents(EventFiringWebDriver eventFiringDriver)
        {
            LoggingEnabled enbled = LoggingConfig.GetLoggingEnabledSetting();

            if (enbled == LoggingEnabled.YES || enbled == LoggingEnabled.ONFAIL)
            {
                eventFiringDriver.ElementClicked += this.WebDriver_ElementClicked;
                eventFiringDriver.ElementClicking += this.WebDriver_ElementClicking;
                eventFiringDriver.ElementValueChanged += this.WebDriver_ElementValueChanged;
                eventFiringDriver.ElementValueChanging += this.WebDriver_ElementValueChanging;
                eventFiringDriver.FindElementCompleted += this.WebDriver_FindElementCompleted;
                eventFiringDriver.FindingElement += this.WebDriver_FindingElement;
                eventFiringDriver.ScriptExecuted += this.WebDriver_ScriptExecuted;
                eventFiringDriver.ScriptExecuting += this.WebDriver_ScriptExecuting;
                eventFiringDriver.Navigated += this.WebDriver_Navigated;
                eventFiringDriver.Navigating += this.WebDriver_Navigating;
                eventFiringDriver.NavigatedBack += this.WebDriver_NavigatedBack;
                eventFiringDriver.NavigatedForward += this.WebDriver_NavigatedForward;
                eventFiringDriver.NavigatingBack += this.WebDriver_NavigatingBack;
                eventFiringDriver.NavigatingForward += this.WebDriver_NavigatingForward;
                eventFiringDriver.ExceptionThrown += this.WebDriver_ExceptionThrown;
            }
        }

        /// <summary>
        /// Event for webdriver that is navigating forward
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_NavigatingForward(object sender, WebDriverNavigationEventArgs e)
        {
            this.LogVerbose("Navigating to: {0}", e.Url);
        }

        /// <summary>
        /// Event for webdriver that is navigating back
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_NavigatingBack(object sender, WebDriverNavigationEventArgs e)
        {
            this.LogVerbose("Navigating back to: {0}", e.Url);
        }

        /// <summary>
        /// Event for webdriver that has navigated forward
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_NavigatedForward(object sender, WebDriverNavigationEventArgs e)
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Navigated Forward: {0}", e.Url);
        }

        /// <summary>
        /// Event for webdriver that is navigated back
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_NavigatedBack(object sender, WebDriverNavigationEventArgs e)
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Navigated back: {0}", e.Url);
        }

        /// <summary>
        /// Event for webdriver that is navigating
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_Navigating(object sender, WebDriverNavigationEventArgs e)
        {
            this.LogVerbose("Navigating to: {0}", e.Url);
        }

        /// <summary>
        /// Event for webdriver that is script executing
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ScriptExecuting(object sender, WebDriverScriptEventArgs e)
        {
            this.LogVerbose("Script executing: {0}", e.Script);
        }

        /// <summary>
        /// Event for webdriver that is finding an element
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_FindingElement(object sender, FindElementEventArgs e)
        {
            this.LogVerbose("Finding element: {0}", e.FindMethod);
        }

        /// <summary>
        /// Event for webdriver that is changing an element value
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ElementValueChanging(object sender, WebElementEventArgs e)
        {
            this.LogVerbose("Value of element changing: {0}", e.Element);
        }

        /// <summary>
        /// Event for webdriver that is clicking an element
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ElementClicking(object sender, WebElementEventArgs e)
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Element clicking: {0} Text:{1} Location: X:{2} Y:{3}", e.Element, e.Element.Text, e.Element.Location.X, e.Element.Location.Y);
        }

        /// <summary>
        /// Event for webdriver when an exception is thrown
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ExceptionThrown(object sender, WebDriverExceptionEventArgs e)
        {
            // First chance handler catches these when it is a real error - These are typically retry loops
            this.Log.LogMessage(MessageType.VERBOSE, "Exception thrown: {0}", e.ThrownException);
        }

        /// <summary>
        /// Event for webdriver that has navigated
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_Navigated(object sender, WebDriverNavigationEventArgs e)
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Navigated to: {0}", e.Url);
        }

        /// <summary>
        /// Event for webdriver has executed a script
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ScriptExecuted(object sender, WebDriverScriptEventArgs e)
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Script executed: {0}", e.Script);
        }

        /// <summary>
        /// Event for webdriver that is finished finding an element
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_FindElementCompleted(object sender, FindElementEventArgs e)
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Found element: {0}", e.FindMethod);
        }

        /// <summary>
        /// Event for webdriver that has changed an element value
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ElementValueChanged(object sender, WebElementEventArgs e)
        {
            string element = e.Element.GetAttribute("value");
            this.Log.LogMessage(MessageType.INFORMATION, "Element value changed: {0}", element);
        }

        /// <summary>
        /// Event for webdriver that has clicked an element
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event object</param>
        private void WebDriver_ElementClicked(object sender, WebElementEventArgs e)
        {
            try
            {
                this.LogVerbose("Element clicked: {0} Text:{1} Location: X:{2} Y:{3}", e.Element, e.Element.Text, e.Element.Location.X, e.Element.Location.Y);
            }
            catch
            {
                this.LogVerbose("Element clicked");
            }
        }
    }
}
