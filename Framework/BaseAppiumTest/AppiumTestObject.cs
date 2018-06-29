//--------------------------------------------------
// <copyright file="AppiumTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Appium context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

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
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public AppiumTestObject(AppiumDriver<IWebElement> appiumDriver, Logger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(AppiumDriver<IWebElement>).FullName,  new MobileDriverManager(() => appiumDriver, this));
            this.SoftAssert = new AppiumSoftAssert(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumTestObject" /> class
        /// </summary>
        /// <param name="appiumDriver">Function for initializing a Appium driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public AppiumTestObject(Func<AppiumDriver<IWebElement>> appiumDriver, Logger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(AppiumDriver<IWebElement>).FullName, new MobileDriverManager(appiumDriver, this));
            this.SoftAssert = new AppiumSoftAssert(this);
        }

        /// <summary>
        /// Gets the Appium driver
        /// </summary>
        public AppiumDriver<IWebElement> AppiumDriver
        {
            get
            {
                return this.ManagerStore[typeof(AppiumDriver<IWebElement>).FullName].Get() as AppiumDriver<IWebElement>;
            }
        }

        /// <summary>
        /// Override the Appium driver
        /// </summary>
        /// <param name="appiumDriver">New Appium driver</param>
        public void OverrideWebDriver(AppiumDriver<IWebElement> appiumDriver)
        {
            this.OverrideDriverManager(typeof(AppiumDriver<IWebElement>).FullName, new MobileDriverManager(() => appiumDriver, this));
        }

        /// <summary>
        /// Override the Appium driver
        /// </summary>
        /// <param name="appiumDriver">New function for initializing a Appium driver</param>
        public void OverrideWebDriver(Func<AppiumDriver<IWebElement>> appiumDriver)
        {
            this.OverrideDriverManager(typeof(AppiumDriver<IWebElement>).FullName, new MobileDriverManager(appiumDriver, this));
        }
    }
}
