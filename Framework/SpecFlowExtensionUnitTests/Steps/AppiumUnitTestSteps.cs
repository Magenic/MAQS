//--------------------------------------------------
// <copyright file="AppiumUnitTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>TestSteps class that inherits from BaseAppiumTestSteps</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.SpecFlow.TestSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using TechTalk.SpecFlow;

namespace SpecFlowExtensionUnitTests.Steps
{
    /// <summary>
    /// BaseAppium unit test steps
    /// </summary>
    [Binding]
    public class AppiumUnitTestSteps : BaseAppiumTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumUnitTestSteps" /> class
        /// </summary>
        /// <param name="context">The context to pass to the base class</param>
        protected AppiumUnitTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Class is instantiated
        /// </summary>
        [Given(@"class BaseAppiumTestSteps")]
        public void GivenClassBaseAppiumTestSteps()
        {
            Assert.IsNotNull(this);
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"BaseAppiumTestSteps TestObject is not null")]
        public void ThenBaseAppiumTestStepsTestObjectIsNotNull()
        {
            Assert.IsNotNull(this.TestObject, "TestObject for BaseAppiumTestSteps class is null.");
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"TestObject is type AppiumTestObject")]
        public void ThenTestObjectIsTypeAppiumTestObject()
        {
            Assert.IsTrue(this.TestObject.GetType().Equals(typeof(AppiumTestObject)), $"TestObject for BaseAppiumTestSteps class is the wrong type : {this.TestObject.GetType()}.");
        }

        /// <summary>
        /// ScenarioContext exists
        /// </summary>
        [Then(@"BaseAppiumTestSteps ScenarioContext is not null")]
        public void ThenScenarioContextIsNotNull()
        {
            Assert.IsNotNull(this.LocalScenarioContext, "LocalScenarioContext for BaseAppiumTestSteps class is null.");
        }

        /// <summary>
        /// ScenarioContext is valid
        /// </summary>
        [Then(@"BaseAppiumTestSteps ScenarioContext is type ScenarioContext")]
        public void AndScenarioContextIsTypeScenarioContext()
        {
            Assert.IsTrue(this.LocalScenarioContext.GetType().Equals(typeof(ScenarioContext)), $"LocalScenarioContext for BaseAppiumTestSteps class is the wrong type : {this.LocalScenarioContext.GetType()}.");
        }

        /// <summary>
        /// AppiumDriver exists
        /// </summary>
        [Then(@"AppiumDriver is not null")]
        public void ThenAppiumDriverIsNotNull()
        {
            Assert.IsNotNull(this.TestObject.AppiumDriver, "AppiumDriver for BaseAppiumTestSteps class is null.");
        }

        /// <summary>
        /// AppiumDriver exists
        /// </summary>
        [Then(@"AppiumDriver is type AppiumDriver")]
        public void AndAppiumDriverIsTypeEventFiringAppiumDriver()
        {
            Assert.IsTrue(this.TestObject.AppiumDriver.GetType().Equals(typeof(AppiumDriver<AppiumWebElement>)), $"AppiumDriver for BaseAppiumTestSteps class is the wrong type : {this.TestObject.AppiumDriver.GetType()}.");
        }

        /// <summary>
        /// Finds the element with the given by
        /// </summary>
        /// <param name="by">THe by to search with</param>
        /// <returns>The AppiumWebElement found</returns>
        private AppiumWebElement FindElement(By by)
        {
            return (AppiumWebElement)this.TestObject.AppiumDriver.FindElement(by);
        }
    }
}
