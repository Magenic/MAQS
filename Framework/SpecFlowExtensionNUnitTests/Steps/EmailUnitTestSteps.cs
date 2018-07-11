//--------------------------------------------------
// <copyright file="EmailUnitTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>TestSteps class that inherits from BaseEmailTestSteps</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.SpecFlow.TestSteps;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SpecFlowExtensionNUnitTests.Steps
{
    /// <summary>
    /// BaseEmail unit test steps
    /// </summary>
    [Binding]
    public class EmailUnitTestSteps : BaseEmailTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailUnitTestSteps" /> class
        /// </summary>
        /// <param name="context">The context to pass to the base class</param>
        protected EmailUnitTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Class is instantiated
        /// </summary>
        [Given(@"class BaseEmailTestSteps")]
        public void GivenClassBaseEmailTestSteps()
        {
            Assert.IsNotNull(this);
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"BaseEmailTestSteps TestObject is not null")]
        public void ThenBaseEmailTestStepsTestObjectIsNotNull()
        {
            Assert.IsNotNull(this.TestObject, "TestObject for BaseEmailTestSteps class is null.");
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"TestObject is type EmailTestObject")]
        public void ThenTestObjectIsTypeEmailTestObject()
        {
            Assert.IsTrue(this.TestObject.GetType().Equals(typeof(EmailTestObject)), $"TestObject for BaseEmailTestSteps class is the wrong type : {this.TestObject.GetType()}.");
        }

        /// <summary>
        /// ScenarioContext exists
        /// </summary>
        [Then(@"BaseEmailTestSteps ScenarioContext is not null")]
        public void ThenScenarioContextIsNotNull()
        {
            Assert.IsNotNull(this.LocalScenarioContext, "LocalScenarioContext for EmailTestObject class is null.");
        }

        /// <summary>
        /// ScenarioContext is valid
        /// </summary>
        [Then(@"BaseEmailTestSteps ScenarioContext is type ScenarioContext")]
        public void AndScenarioContextIsTypeScenarioContext()
        {
            Assert.IsTrue(this.LocalScenarioContext.GetType().Equals(typeof(ScenarioContext)), $"LocalScenarioContext for EmailTestObject class is the wrong type : {this.LocalScenarioContext.GetType()}.");
        }

        /// <summary>
        /// EmailDriver exists
        /// </summary>
        [Then(@"BaseEmailTestSteps EmailDriver is not null")]
        public void ThenEmailDriverIsNotNull()
        {
            Assert.IsNotNull(this.TestObject.EmailDriver, "EmailDriver for EmailTestObject class is null.");
        }

        /// <summary>
        /// EmailDriver exists
        /// </summary>
        [Then(@"EmailDriver is type EventFiringEmailConnectionDriver")]
        public void AndEmailDriverIsTypeEventFiringEmailConnectionDriver()
        {
            Assert.IsTrue(this.TestObject.EmailDriver.GetType().Equals(typeof(EventFiringEmailDriver)), $"EmailDriver for EmailTestObject class is the wrong type : {this.TestObject.EmailDriver.GetType()}.");
        }
    }
}
