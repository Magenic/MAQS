//--------------------------------------------------
// <copyright file="BaseAppiumTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using appium</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseAppiumTest;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using TechTalk.SpecFlow;
using MaqsAppium = Magenic.MaqsFramework.BaseAppiumTest.BaseAppiumTest;

namespace Magenic.MaqsFramework.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for appium TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Appium")]
    public class BaseAppiumTestSteps : ExtendableTestSteps<AppiumTestObject, MaqsAppium, AppiumDriver<IWebElement>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAppiumTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseAppiumTestSteps(ScenarioContext context) : base(context)
        {
        }
    }
}
