//--------------------------------------------------
// <copyright file="EmailUnitTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>TestSteps class that inherits from BaseEmailTestSteps</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseEmailTest;
using NUnit.Framework;
using SpecFlowMAQSExtension.TestSteps;
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
        /// EmailWrapper exists
        /// </summary>
        [Then(@"BaseEmailTestSteps EmailWrapper is not null")]
        public void ThenEmailWrapperIsNotNull()
        {
            Assert.IsNotNull(this.TestObject.EmailWrapper, "EmailWrapper for EmailTestObject class is null.");
        }

        /// <summary>
        /// EmailWrapper exists
        /// </summary>
        [Then(@"EmailWrapper is type EventFiringEmailConnectionWrapper")]
        public void AndEmailWrapperIsTypeEventFiringEmailConnectionWrapper()
        {
            Assert.IsTrue(this.TestObject.EmailWrapper.GetType().Equals(typeof(EventFiringEmailConnectionWrapper)), $"EmailWrapper for EmailTestObject class is the wrong type : {this.TestObject.EmailWrapper.GetType()}.");
        }
    }
}
