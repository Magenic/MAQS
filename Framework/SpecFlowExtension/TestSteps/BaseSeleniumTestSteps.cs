//--------------------------------------------------
// <copyright file="BaseSeleniumTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using selenium</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using MaqsSelenium = Magenic.MaqsFramework.BaseSeleniumTest.BaseSeleniumTest;

namespace Magenic.Maqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for selenium TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Selenium")]
    public class BaseSeleniumTestSteps : ExtendableTestSteps<SeleniumTestObject, MaqsSelenium>
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
