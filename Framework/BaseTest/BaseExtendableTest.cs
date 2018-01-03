//--------------------------------------------------
// <copyright file="BaseExtendableTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for test classes that setup test objects like web drivers or database connections</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using VSTestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Magenic.MaqsFramework.BaseTest
{
    /// <summary>
    /// Base code for test classes that setup test objects like web drivers or database connections
    /// </summary>
    /// <typeparam name="T">Type of object under test, such as web driver and web service wrapper</typeparam>
    /// <typeparam name="U">Test object type</typeparam>
    [TestClass]
    public abstract class BaseExtendableTest<T, U> : BaseTest where T : class where U : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseExtendableTest{T, U}" /> class
        /// </summary>
        public BaseExtendableTest()
        {
            this.ObjectsUnderTest = new ConcurrentDictionary<string, T>();
        }

        /// <summary>
        /// Gets or sets the object under test
        /// </summary>
        protected T ObjectUnderTest
        {
            get
            {
                return this.ObjectsUnderTest[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.ObjectsUnderTest.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets or sets the test object 
        /// </summary>
        protected new U TestObject
        {
            get
            {
                return (U)base.TestObject;
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.BaseTestObjects.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets or sets the testing object - Selenium driver, web client, database connection, etc.
        /// </summary>
        private ConcurrentDictionary<string, T> ObjectsUnderTest { get; set; }

        /// <summary>
        /// Check if the test object is stored
        /// </summary>
        /// <returns>True if the test object is stored</returns>
        public bool IsObjectUnderTestStored()
        {
            return this.ObjectsUnderTest.ContainsKey(this.GetFullyQualifiedTestClassName());
        }

        /// <summary>
        /// Setup before a test
        /// </summary>
        [TestInitialize]
        [SetUp]
        public new void Setup()
        {
            // Do base generic setup
            base.Setup();

            try
            {
                // Only use event firing if we are logging
                if (LoggingConfig.GetLoggingEnabledSetting() != LoggingEnabled.NO)
                {
                    this.SetupEventFiringTester();
                }
                else
                {
                    this.SetupNoneEventFiringTester();
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.ERROR, "Setup failed because: {0}\r\n{1}", e.Message, e.StackTrace);

                // Make sure we do the standard teardown
                this.Teardown();
                throw e;
            }

            this.PostSetupLogging();
        }
         
        /// <summary>
        /// Tear down after a test
        /// </summary>
        [TestCleanup]
        [TearDown]
        public new void Teardown()
        {
            try
            {
                this.BeforeLoggingTeardown(this.GetResultType());
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed before logging teardown because: {0}", e.Message);
            }

            try
            {
                // Do default teardown
                base.Teardown();
            }
            finally
            {
                // Get the Fully Qualified Test Name
                string fullyQualifiedTestName = this.GetFullyQualifiedTestClassName();

                // Release the test object
                T testObject;
                this.ObjectsUnderTest.TryRemove(fullyQualifiedTestName, out testObject);
                testObject = null;
            }
        }

        /// <summary>
        /// Setup event firing test object 
        /// </summary>
        /// <example>  
        /// This sample shows what an overload for the <see cref="SetupEventFiringTester"/> method may look like
        /// <code> 
        /// protected override void SetupEventFiringTester()
        /// {
        ///    this.WebDriver = this.GetBrowser();
        ///    this.WebDriver = new EventFiringWebDriver(this.WebDriver);
        ///    this.MapEvents((EventFiringWebDriver)this.WebDriver);
        /// }
        /// </code> 
        /// </example> 
        protected abstract void SetupEventFiringTester();

        /// <summary>
        /// Setup none event firing test object 
        /// </summary>
        /// <example>  
        /// This sample shows what an overload for the <see cref="SetupNoneEventFiringTester"/> method may look like
        /// <code> 
        /// protected override void SetupNoneEventFiringTester()
        /// {
        ///    this.WebDriver = this.GetBrowser();
        /// }
        /// </code> 
        /// </example> 
        protected abstract void SetupNoneEventFiringTester();

        /// <summary>
        /// Create a test object
        /// </summary>
        protected override abstract void CreateNewTestObject();

        /// <summary>
        /// Overload function for doing post setup logging
        /// </summary>
        /// <remarks> 
        /// If not override no post setup logging will be done 
        /// </remarks> 
        protected virtual void PostSetupLogging()
        {
        }

        /// <summary>
        /// Steps to do before logging teardown results - If not override nothing is done before logging the results
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected virtual void BeforeLoggingTeardown(TestResultType resultType)
        {
        }
    }
}
