//--------------------------------------------------
// <copyright file="IAppiumTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Appium test object interface</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using OpenQA.Selenium.Appium;
using System;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Appium test object interface
    /// </summary>
    public interface IAppiumTestObject : ITestObject
    {
        /// <summary>
        /// Gets the Appium driver
        /// </summary>
        AppiumDriver AppiumDriver { get; }

        /// <summary>
        /// Gets the Appium driver manager
        /// </summary>
        AppiumDriverManager AppiumManager { get; }

        /// <summary>
        /// Override the Appium driver
        /// </summary>
        /// <param name="appiumDriver">New Appium driver</param>
        void OverrideAppiumDriver(AppiumDriver appiumDriver);

        /// <summary>
        /// Override the Appium driver
        /// </summary>
        /// <param name="appiumDriver">New function for initializing a Appium driver</param>
        void OverrideAppiumDriver(Func<AppiumDriver> appiumDriver);
    }
}