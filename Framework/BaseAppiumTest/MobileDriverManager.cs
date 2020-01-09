//--------------------------------------------------
// <copyright file="MobileDriverManager.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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
        /// Get the Appium driver
        /// </summary>
        /// <returns>The Appium driver</returns>
        public AppiumDriver<IWebElement> GetMobileDriver()
        {
            return GetBase() as AppiumDriver<IWebElement>;
        }

        /// <summary>
        /// Get the Appium driver
        /// </summary>
        /// <returns>The Appium driver</returns>
        public override object Get()
        {
            return this.GetMobileDriver();
        }

        /// <summary>
        /// Cleanup the Appium driver
        /// </summary>
        protected override void DriverDispose()
        {
            // If we never created the driver we don't have any cleanup to do
            if (!this.IsDriverIntialized())
            {
                return;
            }

            try
            {
                AppiumDriver<IWebElement> driver = this.GetMobileDriver();
                driver?.KillDriver();
            }
            catch (Exception e)
            {
                this.Log.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to close mobile driver because: {0}", e.Message));
            }

            this.BaseDriver = null;
        }
    }
}
