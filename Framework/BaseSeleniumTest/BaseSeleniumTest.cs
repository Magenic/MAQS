//--------------------------------------------------
// <copyright file="BaseSeleniumTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Selenium test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using System;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Generic base Selenium test class
    /// </summary>
    public class BaseSeleniumTest : BaseExtendableTest<SeleniumTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeleniumTest"/> class
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
                return this.TestObject.WebDriver;
            }

            set
            {
                this.TestObject.OverrideWebDriver(value);
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
                    SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.TestObject);

                    if (SeleniumConfig.GetSavePagesourceOnFail())
                    {
                        SeleniumUtilities.SavePageSource(this.WebDriver, this.TestObject, "FinalPageSource");
                    }
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to get screen shot because: {0}", e.Message);
            }
        }

        /// <summary>
        /// Create a Selenium test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new SeleniumTestObject(() => this.GetBrowser(), newLogger, this.GetFullyQualifiedTestClassName());
        }
    }
}