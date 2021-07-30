//--------------------------------------------------
// <copyright file="SeleniumTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Selenium context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using OpenQA.Selenium;
using System;

namespace Magenic.Maqs.BaseSeleniumTest
{
    public interface ISeleniumTestObject : ITestObject
    {
        IWebDriver WebDriver { get; }
        SeleniumDriverManager WebManager { get; }

        void OverrideWebDriver(Func<IWebDriver> getDriver);
        void OverrideWebDriver(IWebDriver webDriver);
    }
}