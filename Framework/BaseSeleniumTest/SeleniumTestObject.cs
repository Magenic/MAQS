//--------------------------------------------------
// <copyright file="SeleniumTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Selenium context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;
using OpenQA.Selenium;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Selenium test context data
    /// </summary>
    public class SeleniumTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTestObject" /> class
        /// </summary>
        /// <param name="webDriver">The test's Selenium web driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="perfTimerCollection">The test's performance timer collection</param>
        public SeleniumTestObject(IWebDriver webDriver, Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection) : base(logger, softAssert, perfTimerCollection)
        {
            this.WebDriver = webDriver;
        }

        /// <summary>
        /// Gets the web driver
        /// </summary>
        public IWebDriver WebDriver { get; private set; }
    }
}