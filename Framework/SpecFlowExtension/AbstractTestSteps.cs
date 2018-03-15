//--------------------------------------------------
// <copyright file="AbstractTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Abstract class with TestSteps functions</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using System;
using TechTalk.SpecFlow;

namespace Magenic.MaqsFramework.SpecFlow
{
    /// <summary>
    /// The abstract TestSteps class definition
    /// </summary>
    /// <typeparam name="O">The BaseTestObject type for the given TestSteps class</typeparam>
    public abstract class AbstractTestSteps<O> : Steps where O : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTestSteps{O}" /> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected AbstractTestSteps(ScenarioContext context)
        {
            this.LocalScenarioContext = context ?? throw new ArgumentNullException(nameof(context));

            // used for debugging parallel execution
            Guid uid = Guid.NewGuid();
            this.LocalScenarioContext.Add(uid.ToString(), uid.ToString());
        }

        /// <summary>
        /// Gets the TestObject
        /// </summary>
        public abstract O TestObject { get; }

        /// <summary>
        /// Gets or sets the SpecFlow Scenario Context.
        /// </summary>
        public ScenarioContext LocalScenarioContext { get; set; }

        /// <summary>
        /// Setup before a step to Setup BaseTest
        /// </summary>
        [BeforeStep]
        internal void BeforeStepSetup()
        {
            // Verify MAQS is in use
            if (this.LocalScenarioContext.ContainsKey("MAQS"))
            {
                return;
            }

            // Verify that the TestObject has not already been setup
            if (!this.LocalScenarioContext.ContainsValue($"MAQSSETUP"))
            {
                this.SetupBaseTest();

                // Add MAQS Setup flag
                this.LocalScenarioContext.Set($"MAQSSETUP");
            }
        }

        /// <summary>
        /// Teardown BaseTest
        /// </summary>
        [AfterScenario]
        internal void TeardownAfterScenario()
        {
            // Run Test Teardown for MAQS if setup occurred
            if (this.LocalScenarioContext.ContainsValue($"MAQSSETUP"))
            {
                this.TeardownBaseTest();
            }
        }

        /// <summary>
        /// Sets up the base test class
        /// </summary>
        internal abstract void SetupBaseTest();

        /// <summary>
        /// Tears down the base test class
        /// </summary>
        internal abstract void TeardownBaseTest();
    }
}
