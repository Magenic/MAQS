﻿//--------------------------------------------------
// <copyright file="DatabaseUnitTestSteps.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>TestSteps class that inherits from BaseEmailTestSteps</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using Magenic.Maqs.SpecFlow.TestSteps;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace SpecFlowExtensionNUnitTests.Steps
{
    /// <summary>
    /// BaseDatabase unit test steps
    /// </summary>
    [Binding]
    public class DatabaseUnitTestSteps : BaseDatabaseTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseUnitTestSteps" /> class
        /// </summary>
        /// <param name="context">The context to pass to the base class</param>
        protected DatabaseUnitTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Class is instantiated
        /// </summary>
        [Given(@"class BaseDatabaseTestSteps")]
        public void GivenClassBaseDatabaseTestSteps()
        {
            Assert.IsNotNull(this);
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"BaseDatabaseTestSteps TestObject is not null")]
        public void ThenBaseDatabaseTestStepsTestObjectIsNotNull()
        {
            Assert.IsNotNull(this.TestObject, "TestObject for BaseDatabaseTestSteps class is null.");
        }

        /// <summary>
        /// Test object exists
        /// </summary>
        [Then(@"TestObject is type DatabaseTestObject")]
        public void ThenTestObjectIsTypeDatabaseTestObject()
        {
            Assert.IsTrue(this.TestObject.GetType().Equals(typeof(DatabaseTestObject)), $"TestObject for BaseDatabaseTestSteps class is the wrong type : {this.TestObject.GetType()}.");
        }

        /// <summary>
        /// ScenarioContext exists
        /// </summary>
        [Then(@"BaseDatabaseTestSteps ScenarioContext is not null")]
        public void ThenScenarioContextIsNotNull()
        {
            Assert.IsNotNull(this.LocalScenarioContext, "LocalScenarioContext for BaseDatabaseTestSteps class is null.");
        }

        /// <summary>
        /// ScenarioContext is valid
        /// </summary>
        [Then(@"BaseDatabaseTestSteps ScenarioContext is type ScenarioContext")]
        public void AndScenarioContextIsTypeScenarioContext()
        {
            Assert.IsTrue(this.LocalScenarioContext.GetType().Equals(typeof(ScenarioContext)), $"LocalScenarioContext for BaseDatabaseTestSteps class is the wrong type : {this.LocalScenarioContext.GetType()}.");
        }

        /// <summary>
        /// DatabaseDriver exists
        /// </summary>
        [Then(@"DatabaseDriver is not null")]
        public void ThenDatabaseDriverIsNotNull()
        {
            Assert.IsNotNull(this.DatabaseDriver, "DatabaseDriver for BaseDatabaseTestSteps class is null.");
        }

        /// <summary>
        /// DatabaseDriver exists
        /// </summary>
        [Then(@"DatabaseDriver is type DatabaseConnectionDriver")]
        public void AndDatabaseDriverIsTypeIWebDriver()
        {
            Assert.IsTrue(this.DatabaseDriver.GetType().Equals(typeof(EventFiringDatabaseDriver)), $"DatabaseDriver for BaseDatabaseTestSteps class is the wrong type : {this.TestObject.DatabaseDriver.GetType()}.");
        }
    }
}
