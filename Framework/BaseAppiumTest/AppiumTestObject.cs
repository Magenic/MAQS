//--------------------------------------------------
// <copyright file="AppiumTestObject.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Appium context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Appium test context data
    /// </summary>
    public class AppiumTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumTestObject" /> class
        /// </summary>
        /// <param name="appiumDriver">The test's Appium driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="perfTimerCollection" >The test's performance timer collection</param>
        public AppiumTestObject(AppiumDriver<IWebElement> appiumDriver, Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection) : base(logger, softAssert, perfTimerCollection)
        {
            this.AppiumDriver = appiumDriver;
        }

        /// <summary>
        /// Gets the Appium driver
        /// </summary>
        public AppiumDriver<IWebElement> AppiumDriver { get; private set; }
    }
}
