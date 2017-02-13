//--------------------------------------------------
// <copyright file="BaseSeleniumTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Selenium test class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.BaseTest;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using System;
using System.Text;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Generic base Selenium test class
    /// </summary>
    public class BaseSeleniumTest : BaseGenericTest<IWebDriver, SeleniumTestObject>, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeleniumTest"/> class.
        /// Setup the web driver for each test class
        /// </summary>
        public BaseSeleniumTest()
        {
        }

        /// <summary>
        /// Gets or sets the WebDriver
        /// </summary>
        public IWebDriver WebDriver
        {
            get
            {
                return (IWebDriver)this.ObjectUnderTest;
            }

            set
            {
                this.ObjectUnderTest = value;
            }
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the webdriver if it hasn't already been
        /// </summary>
        /// <param name="disposing">Are we disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.IsObjectUnderTestStored() && disposing)
            {
                // dispose managed resources
                try
                {
                    if (this.WebDriver != null)
                    {
                        this.WebDriver.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(StringProcessor.SafeFormatter("Failed to close because: {0}", e.Message));
                }
            }
        }

        /// <summary>
        /// The default get web driver function
        /// </summary>
        /// <returns>The web driver</returns>
        protected virtual IWebDriver GetBrowser()
        {
            return SeleniumConfig.Browser();
        }

        /// <summary>
        /// Method to get a new soft assert object
        /// </summary>
        /// <returns>A soft assert object</returns>
        protected override SoftAssert GetSoftAssert()
        {
            if (this.IsObjectUnderTestStored())
            {
                return new SeleniumSoftAssert(this.WebDriver, this.Log);
            }

            return base.GetSoftAssert();
        }

        /// <summary>
        /// Log info about the web driver setup
        /// </summary>
        protected override void PostSetupLogging()
        {
            try
            {
                if (SeleniumConfig.GetBrowserName().Equals("Remote", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Remote driver: {0}", SeleniumConfig.GetRemoteBrowserName());
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Loaded driver: {0}", SeleniumConfig.GetBrowserName());
                }

                this.WebDriver.SetWaitDriver(SeleniumConfig.GetWaitDriver(this.WebDriver));
            }
            catch (Exception e)
            {
                this.Log.LogMessage(MessageType.ERROR, "Failed to start driver because: {0}", e.Message);
                Console.WriteLine(StringProcessor.SafeFormatter("Failed to start driver because: {0}", e.Message));
            }
        }

        /// <summary>
        /// Setup the event firing web driver
        /// </summary>
        protected override void SetupEventFiringTester()
        {
            this.WebDriver = this.GetBrowser();
            try
            {
                this.WebDriver = new EventFiringWebDriver(this.WebDriver);
                this.MapEvents((EventFiringWebDriver)this.WebDriver);
            }
            catch
            {
                this.Log.LogMessage(MessageType.WARNING, "Cannot cast driver: {0} as an event firing driver", this.WebDriver.GetType().ToString());
            }
        }

        /// <summary>
        /// Setup the normal web driver - the none event firing implementation
        /// </summary>
        protected override void SetupNoneEventFiringTester()
        {
            this.WebDriver = this.GetBrowser();
        }

        /// <summary>
        /// Take a screen shot if needed and tear down the web driver
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected override void BeforeLoggingTeardown(TestResultType resultType)
        {
            // Try to take a screen shot
            try
            {
                if (this.Log is FileLogger && resultType != TestResultType.PASS && this.LoggingEnabledSetting != LoggingEnabled.NO)
                {
                    SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.Log);
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to get screen shot because: {0}", e.Message);
            }

            this.TryToLog(MessageType.INFORMATION, "Closing webDriver");

            try
            {
                // Clear the waiter
                this.WebDriver.RemoveWaitDriver();

                // Quite
                this.WebDriver.Quit();
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to quit because: {0}", e.Message);
            }

            try
            {
                this.WebDriver.Dispose();
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to dispose because: {0}", e.Message);
            }
        }

        /// <summary>
        /// Create a Selenium test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new SeleniumTestObject(this.WebDriver, this.Log, this.SoftAssert, this.PerfTimerCollection);
        }

        /// <summary>
        /// Map selenium events to log events
        /// </summary>
        /// <param name="eventFiringDriver">The event firing web driver that we want mapped</param>
        private void MapEvents(EventFiringWebDriver eventFiringDriver)
        {
            if (this.LoggingEnabledSetting == LoggingEnabled.YES || this.LoggingEnabledSetting == LoggingEnabled.ONFAIL)
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