//--------------------------------------------------
// <copyright file="WebServiceUnitTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>TestSteps class that inherits from BaseWebServiceTestSteps</summary>
//--------------------------------------------------
using Magenic.Maqs.SpecFlow.TestSteps;
using Magenic.Maqs.BaseWebServiceTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace SpecFlowExtensionUnitTests.Steps
{
    /// <summary>
    /// BaseWebService unit test steps
    /// </summary>
    [Binding]
    public class WebServiceUnitTestSteps : BaseWebServiceTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceUnitTestSteps" /> class
        /// </summary>
        /// <param name="context">The context to pass to the base class</param>
        protected WebServiceUnitTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Class is instantiated
        /// </summary>
        [Given(@"class BaseWebServiceTestSteps")]
        public void GivenClassBaseWebServiceTestSteps()
        {
            Assert.IsNotNull(this);
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"BaseWebServiceTestSteps TestObject is not null")]
        public void ThenBaseWebServiceTestStepsTestObjectIsNotNull()
        {
            Assert.IsNotNull(this.TestObject, "TestObject for BaseWebServiceTestSteps class is null.");
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"TestObject is type WebServiceTestObject")]
        public void ThenTestObjectIsTypeWebServiceTestObject()
        {
            Assert.IsTrue(this.TestObject.GetType().Equals(typeof(WebServiceTestObject)), $"TestObject for BaseWebServiceTestSteps class is the wrong type : {this.TestObject.GetType()}.");
        }

        /// <summary>
        /// ScenarioContext exists
        /// </summary>
        [Then(@"BaseWebServiceTestSteps ScenarioContext is not null")]
        public void ThenScenarioContextIsNotNull()
        {
            Assert.IsNotNull(this.LocalScenarioContext, "LocalScenarioContext for BaseWebServiceTestSteps class is null.");
        }

        /// <summary>
        /// ScenarioContext is valid
        /// </summary>
        [Then(@"BaseWebServiceTestSteps ScenarioContext is type ScenarioContext")]
        public void AndScenarioContextIsTypeScenarioContext()
        {
            Assert.IsTrue(this.LocalScenarioContext.GetType().Equals(typeof(ScenarioContext)), $"LocalScenarioContext for BaseWebServiceTestSteps class is the wrong type : {this.LocalScenarioContext.GetType()}.");
        }

        /// <summary>
        /// WebServiceWrapper exists
        /// </summary>
        [Then(@"BaseWebServiceTestSteps WebServiceWrapper is not null")]
        public void ThenWebDriverIsNotNull()
        {
            Assert.IsNotNull(this.TestObject.WebServiceDriver, "WebServiceDriver for BaseWebServiceTestSteps class is null.");
        }

        /// <summary>
        /// WebServiceWrapper exists
        /// </summary>
        [Then(@"BaseWebServiceTestSteps WebServiceWrapper is type EventFiringWebServiceDriver")]
        public void AndWebDriverIsTypeWebServiceDriver()
        {
            Assert.IsTrue(this.TestObject.WebServiceDriver.GetType().Equals(typeof(EventFiringWebServiceDriver)), $"WebServiceDriver for BaseWebServiceTestSteps class is the wrong type : {this.TestObject.WebServiceDriver.GetType()}.");
        }
    }
}
