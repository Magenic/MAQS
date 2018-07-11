//--------------------------------------------------
// <copyright file="SeleniumTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Selenium context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using System;

namespace Magenic.Maqs.BaseSeleniumTest
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
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public SeleniumTestObject(IWebDriver webDriver, Logger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(() => webDriver, this));
            this.SoftAssert = new SeleniumSoftAssert(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumTestObject" /> class
        /// </summary>
        /// <param name="getDriver">Function for getting a Selenium web driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public SeleniumTestObject(Func<IWebDriver> getDriver, Logger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(getDriver, this));
            this.SoftAssert = new SeleniumSoftAssert(this);
        }

        /// <summary>
        /// Gets the Selenium driver manager
        /// </summary>
        public SeleniumDriverManager WebManager
        {
            get
            {
                return this.ManagerStore[typeof(SeleniumDriverManager).FullName] as SeleniumDriverManager;
            }
        }

        /// <summary>
        /// Gets the Selenium web driver
        /// </summary>
        public IWebDriver WebDriver
        {
            get
            {
                return this.WebManager.Get();
            }
        }

        /// <summary>
        /// Override the Selenium web driver
        /// </summary>
        /// <param name="webDriver">New web driver</param>
        public void OverrideWebDriver(IWebDriver webDriver)
        {
            this.OverrideDriverManager(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(() => webDriver, this));
        }

        /// <summary>
        /// Override the function for creating a Selenium web driver
        /// </summary>
        /// <param name="getDriver">Function for creating a web driver</param>
        public void OverrideWebDriver(Func<IWebDriver> getDriver)
        {
            this.OverrideDriverManager(typeof(SeleniumDriverManager).FullName, new SeleniumDriverManager(getDriver, this));
        }
    }
}