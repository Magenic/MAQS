//--------------------------------------------------
// <copyright file="AppiumTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Appium context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace Magenic.Maqs.BaseAppiumTest
{
    public interface IAppiumTestObject : ITestObject
    {
        AppiumDriver<IWebElement> AppiumDriver { get; }
        MobileDriverManager AppiumManager { get; }

        void OverrideWebDriver(AppiumDriver<IWebElement> appiumDriver);
        void OverrideWebDriver(Func<AppiumDriver<IWebElement>> appiumDriver);
    }
}