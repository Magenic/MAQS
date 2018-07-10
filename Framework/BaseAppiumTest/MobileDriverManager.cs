//--------------------------------------------------
// <copyright file="MobileDriverManager.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Mobile driver manager</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Mobile driver manager
    /// </summary>
    public class MobileDriverManager : DriverManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDriverManager"/> class
        /// </summary>
        /// <param name="getDriver">Function for getting an Appium driver</param>
        /// <param name="testObject">The associated test object</param>
        public MobileDriverManager(Func<AppiumDriver<IWebElement>> getDriver, BaseTestObject testObject) : base(getDriver, testObject)
        {
        }

        /// <summary>
        /// Cleanup the Appium driver
        /// </summary>
        public override void Dispose()
        {
            // If we never created the driver we don't have any cleanup to do
            if (!this.IsDriverIntialized())
            {
                return;
            }

            try
            {
                AppiumDriver<IWebElement> driver = this.Get();

                if (driver != null)
                {
                    driver?.Close();
                    driver?.Quit();
                    driver?.Dispose();
                }
            }
            catch (Exception e)
            {
                this.Log.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to close mobile driver because: {0}", e.Message));
            }

            this.BaseDriver = null;
        }

        /// <summary>
        /// Get the Appium driver
        /// </summary>
        /// <returns>The Appium driver</returns>
        public new AppiumDriver<IWebElement> Get()
        {
            return base.Get() as AppiumDriver<IWebElement>;
        }
    }
}
