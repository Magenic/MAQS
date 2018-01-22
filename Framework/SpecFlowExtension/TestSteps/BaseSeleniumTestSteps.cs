//--------------------------------------------------
// <copyright file="BaseSeleniumTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using selenium</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace SpecFlowMAQSExtension.TestSteps
{
    /// <summary>
    /// Base for selenium TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Selenium")]
    public class BaseSeleniumTestSteps : ExtendableTestSteps<SeleniumTestObject, BaseSeleniumTest, IWebDriver>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeleniumTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseSeleniumTestSteps(ScenarioContext context) : base(context)
        {
        }
    }
}
