//--------------------------------------------------
// <copyright file="BaseExtendableTest.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for test classes that setup test objects like web drivers or database connections</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Base code for test classes that setup test objects like web drivers or database connections
    /// </summary>
    /// <typeparam name="T">Test object type</typeparam>
    [TestClass]
    public abstract class BaseExtendableTest<T> : BaseTest where T : ITestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseExtendableTest{T}" /> class
        /// </summary>
        protected BaseExtendableTest() : base()
        {
        }

        /// <summary>
        /// Gets or sets the test object 
        /// </summary>void CreateNewTestObject
        protected new T TestObject
        {
            get
            {
                return (T)base.TestObject;
            }

            set
            {
                this.BaseTestObjects.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
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
        }

        /// <summary>
        /// Create a Selenium test object
        /// </summary>
        protected new void SetupTestObject()
        {
            ILogger newLogger = this.CreateAndSetupLogger(GetFileNameWithoutExtension(), LoggingConfig.GetLogType(), LoggingConfig.GetLoggingEnabledSetting(), LoggingConfig.GetFirstChanceHandler());
            this.TestObject = CreateSpecificTestObject(newLogger);
        }

        /// <summary>
        /// Create a test object
        /// </summary>
        /// <param name="log">Assocatied logger</param>
        /// <returns>The Selenium test object</returns>
        protected override ITestObject CreateTestObject(ILogger log)
        {
            return this.CreateSpecificTestObject(log);
        }

        /// <summary>
        /// Create the test object
        /// </summary>
        /// <param name="log">Log to assoicate with the test object</param>
        /// <returns>Specific test object type</returns>
        protected abstract T CreateSpecificTestObject(ILogger log);
    }
}
