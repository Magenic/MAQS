//--------------------------------------------------
// <copyright file="ExtendableTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Extendable class for defining a test steps class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TechTalk.SpecFlow;

namespace Magenic.MaqsFramework.SpecFlow
{
    /// <summary>
    /// Abstract a TestSteps class
    /// </summary>
    /// <typeparam name="O">The base test object class</typeparam>
    /// <typeparam name="T">The base test class</typeparam>
    public abstract class ExtendableTestSteps<O, T> : AbstractTestSteps<O> 
        where O : BaseTestObject where T : BaseExtendableTest<O>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendableTestSteps{O,T}"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected ExtendableTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the TestObject
        /// </summary>
        public override O TestObject
        {
            get
            {
                return this.LocalScenarioContext.Get<O>($"MAQSTESTOBJECT");
            }
        }

        /// <summary>
        /// Set up the base test object
        /// </summary>
        internal override void SetupBaseTest()
        {
            // Build/setup a new base test
            T basetest = new T();

            // necessary for MsTest log files to have the correct name
            try
            {
                basetest.TestContext = this.LocalScenarioContext.ScenarioContainer.Resolve<TestContext>();
            }
            catch (MemberAccessException)
            {
                // do nothing on this error (will occur if NUnit is being used)
            }

            basetest.Setup();

            // Save the base test
            this.LocalScenarioContext.Set(basetest, $"MAQSBASETEST");

            // Save the base test object
            this.LocalScenarioContext.Set(basetest.TestObject, $"MAQSTESTOBJECT");
        }

        /// <summary>
        /// Teardown the test object
        /// </summary>
        internal override void TeardownBaseTest()
        {
            this.LocalScenarioContext.Get<T>($"MAQSBASETEST").Teardown();
        }
    }
}
