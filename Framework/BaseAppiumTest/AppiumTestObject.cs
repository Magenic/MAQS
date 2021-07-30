//--------------------------------------------------
// <copyright file="AppiumTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Appium context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Appium test context data
    /// </summary>
    public class AppiumTestObject : BaseTestObject, IAppiumTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumTestObject" /> class
        /// </summary>
        /// <param name="appiumDriver">The test's Appium driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public AppiumTestObject(AppiumDriver<IWebElement> appiumDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(MobileDriverManager).FullName, new MobileDriverManager(() => appiumDriver, this));
            this.SoftAssert = new AppiumSoftAssert(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumTestObject" /> class
        /// </summary>
        /// <param name="appiumDriver">Function for initializing a Appium driver</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public AppiumTestObject(Func<AppiumDriver<IWebElement>> appiumDriver, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(MobileDriverManager).FullName, new MobileDriverManager(appiumDriver, this));
            this.SoftAssert = new AppiumSoftAssert(this);
        }

        /// <summary>
        /// Gets the Appium driver
        /// </summary>
        public AppiumDriver<IWebElement> AppiumDriver
        {
            get
            {
                return this.AppiumManager.GetMobileDriver();
            }
        }

        /// <summary>
        /// Gets the Appium driver manager
        /// </summary>
        public MobileDriverManager AppiumManager
        {
            get
            {
                return this.ManagerStore.GetManager<MobileDriverManager>();
            }
        }

        /// <summary>
        /// Override the Appium driver
        /// </summary>
        /// <param name="appiumDriver">New Appium driver</param>
        public void OverrideWebDriver(AppiumDriver<IWebElement> appiumDriver)
        {
            this.AppiumManager.OverrideDriver(appiumDriver);
        }

        /// <summary>
        /// Override the Appium driver
        /// </summary>
        /// <param name="appiumDriver">New function for initializing a Appium driver</param>
        public void OverrideWebDriver(Func<AppiumDriver<IWebElement>> appiumDriver)
        {
            this.AppiumManager.OverrideDriver(appiumDriver);
        }
    }
}
