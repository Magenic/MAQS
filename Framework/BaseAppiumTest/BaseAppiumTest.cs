﻿//--------------------------------------------------
// <copyright file="BaseAppiumTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Appium test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Generic base Appium test class
    /// </summary>
    public class BaseAppiumTest : BaseExtendableTest<AppiumTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAppiumTest"/> class.
        /// Setup the web driver for each test class
        /// </summary>
        public BaseAppiumTest()
        {
        }

        /// <summary>
        /// Gets or sets the AppiumDriver
        /// </summary>
        public AppiumDriver<IWebElement> AppiumDriver
        {
            get
            {
                return this.TestObject.AppiumDriver;
            }

            set
            {
                this.TestObject.OverrideWebDriver(value);
            }
        }

        /// <summary>
        /// The default get appium driver function
        /// </summary>
        /// <returns>The appium driver</returns>
        protected virtual AppiumDriver<IWebElement> GetMobileDevice()
        {
            return AppiumDriverFactory.GetDefaultMobileDriver();
        }

        /// <summary>
        /// Take a screen shot if needed and tear down the appium driver
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected override void BeforeLoggingTeardown(TestResultType resultType)
        {
            try
            {
                // Captures screenshot if test result is not a pass and logging is enabled
                if (this.TestObject.GetDriverManager<MobileDriverManager>().IsDriverIntialized() && this.Log is FileLogger && resultType != TestResultType.PASS &&
                    this.LoggingEnabledSetting != LoggingEnabled.NO)
                {
                    AppiumUtilities.CaptureScreenshot(this.AppiumDriver, this.TestObject, " Final");

                    if (AppiumConfig.GetSavePagesourceOnFail())
                    {
                        AppiumUtilities.SavePageSource(this.AppiumDriver, this.TestObject, "FinalPageSource");
                    }
                }
            }
            catch (Exception exception)
            {
                this.TryToLog(MessageType.WARNING, "Failed to get screen shot because: {0}", exception.Message);
            }
        }

        /// <summary>
        /// Create an Appium test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new AppiumTestObject(() => this.GetMobileDevice(), this.CreateLogger(), this.GetFullyQualifiedTestClassName());
        }
    }
}
