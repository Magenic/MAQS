//--------------------------------------------------
// <copyright file="SeleniumUnitTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>TestSteps class that inherits from BaseSeleniumTestSteps</summary>
//--------------------------------------------------
using Magenic.Maqs.SpecFlow.TestSteps;
using Magenic.MaqsFramework.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Support.Events;
using TechTalk.SpecFlow;

namespace SpecFlowExtensionUnitTests.Steps
{
    /// <summary>
    /// BaseSelenium unit test steps
    /// </summary>
    [Binding]
    public class SeleniumUnitTestSteps : BaseSeleniumTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumUnitTestSteps" /> class
        /// </summary>
        /// <param name="context">The context to pass to the base class</param>
        protected SeleniumUnitTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Class is instantiated
        /// </summary>
        [Given(@"class BaseSeleniumTestSteps")]
        public void GivenClassBaseSeleniumTestSteps()
        {
            Assert.IsNotNull(this);
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"BaseSeleniumTestSteps TestObject is not null")]
        public void ThenTestObjectIsNotNull()
        {
            Assert.IsNotNull(this.TestObject, "TestObject for BaseSeleniumTestSteps class is null.");
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"TestObject is type SeleniumTestObject")]
        public void AndTestObjectIsTypeSeleniumTestObject()
        {
            Assert.IsTrue(this.TestObject.GetType().Equals(typeof(SeleniumTestObject)), $"TestObject for BaseSeleniumTestSteps class is the wrong type : {this.TestObject.GetType()}.");
        }

        /// <summary>
        /// WebDriver exists
        /// </summary>
        [Then(@"BaseSeleniumTestSteps WebDriver is not null")]
        public void ThenWebDriverIsNotNull()
        {
            Assert.IsNotNull(this.TestObject.WebDriver, "WebDriver for BaseSeleniumTestSteps class is null.");
        }

        /// <summary>
        /// WebDriver exists
        /// </summary>
        [Then(@"WebDriver is type EventFiringWebDriver")]
        public void AndWebDriverIsTypeIWebDriver()
        {
            Assert.IsTrue(this.TestObject.WebDriver.GetType().Equals(typeof(EventFiringWebDriver)), $"WebDriver for BaseSeleniumTestSteps class is the wrong type : {this.TestObject.WebDriver.GetType()}.");
        }

        /// <summary>
        /// ScenarioContext exists
        /// </summary>
        [Then(@"BaseSeleniumTestSteps ScenarioContext is not null")]
        public void ThenScenarioContextIsNotNull()
        {
            Assert.IsNotNull(this.LocalScenarioContext, "LocalScenarioContext for BaseSeleniumTestSteps class is null.");
        }

        /// <summary>
        /// ScenarioContext is valid
        /// </summary>
        [Then(@"BaseSeleniumTestSteps ScenarioContext is type ScenarioContext")]
        public void AndScenarioContextIsTypeScenarioContext()
        {
            Assert.IsTrue(this.LocalScenarioContext.GetType().Equals(typeof(ScenarioContext)), $"LocalScenarioContext for BaseSeleniumTestSteps class is the wrong type : {this.LocalScenarioContext.GetType()}.");
        }
    }
}
