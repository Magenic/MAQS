//--------------------------------------------------
// <copyright file="BaseAppiumTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Appium test class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium.Appium;
using System;

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Generic base Appium test class
    /// </summary>
    public class BaseAppiumTest : BaseExtendableTest<AppiumDriver<AppiumWebElement>, AppiumTestObject>, IDisposable
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
        public AppiumDriver<AppiumWebElement> AppiumDriver
        {
            get { return this.ObjectUnderTest; }
            set { this.ObjectUnderTest = value; }
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
        /// Dispose of the appium driver if it hasn't already been
        /// </summary>
        /// <param name="disposing">disposing boolean</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.IsObjectUnderTestStored() && disposing)
            {
                try
                {
                    this.AppiumDriver.Close();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(StringProcessor.SafeFormatter("Failed to close because: {0}", exception.Message));
                }
            }
        }

        /// <summary>
        /// The default get appium driver function
        /// </summary>
        /// <returns>The appium driver</returns>
        protected virtual AppiumDriver<AppiumWebElement> GetMobileDevice()
        {
            return AppiumConfig.MobileDevice();
        }

        /// <summary>
        /// Method to get a new soft assert object
        /// </summary>
        /// <returns>A soft assert object</returns>
        protected override SoftAssert GetSoftAssert()
        {
            if (this.IsObjectUnderTestStored())
            {
                return new AppiumSoftAssert(this.AppiumDriver, this.Log);
            }

            return base.GetSoftAssert();
        }

        /// <summary>
        /// Log info about the appium driver setup
        /// </summary>
        protected override void PostSetupLogging()
        {
            try
            {
                this.Log.LogMessage(MessageType.INFORMATION, "Loaded Mobile Driver: {0}", AppiumConfig.GetPlatformName());
                this.AppiumDriver.SetWaitDriver(AppiumConfig.GetWaitDriver(this.AppiumDriver));
            }
            catch (Exception exception)
            {
                this.Log.LogMessage(MessageType.ERROR, "Failed to start driver because: {0}", exception.Message);
                Console.WriteLine(StringProcessor.SafeFormatter("Failed to start driver because: {0}", exception.Message));
            }
        }

        /// <summary>
        /// Setup the event firing appium driver.  Calling setup for non-event firing driver because event firing is not supported
        /// </summary>
        protected override void SetupEventFiringTester()
        {
            this.SetupNoneEventFiringTester();
        }

        /// <summary>
        /// Setup the normal appium driver - the none event firing implementation
        /// </summary>
        protected override void SetupNoneEventFiringTester()
        {
            this.AppiumDriver = this.GetMobileDevice();
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
                if (this.Log is FileLogger && resultType != TestResultType.PASS &&
                    this.LoggingEnabledSetting != LoggingEnabled.NO)
                {
                    AppiumUtilities.CaptureScreenshot(this.AppiumDriver, this.Log);
                }
            }
            catch (Exception exception)
            {
                this.TryToLog(MessageType.WARNING, "Failed to get screen shot because: {0}", exception.Message);
            }

            this.TryToLog(MessageType.INFORMATION, "Closing Appium Driver");

            // Removes wait driver and quits appium driver
            try
            {
                this.AppiumDriver.RemoveWaitDriver();
                this.AppiumDriver.Quit();
            }
            catch (Exception exception)
            {
                this.TryToLog(MessageType.WARNING, "Failed to quit because: {0}", exception.Message);
            }

            // Disposing of appium driver
            try
            {
                this.AppiumDriver.Dispose();
            }
            catch (Exception exception)
            {
                this.TryToLog(MessageType.WARNING, "Failed to dispose because: {0}", exception.Message);
            }
        }

        /// <summary>
        /// Create an Appium test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new AppiumTestObject(this.AppiumDriver, this.Log, this.SoftAssert, this.PerfTimerCollection);
        }
    }
}
