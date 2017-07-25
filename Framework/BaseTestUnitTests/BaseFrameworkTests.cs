//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Low level framework tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;

namespace BaseTestUnitTests
{
    /// <summary>
    /// Framework unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseFrameworkTests
    {
        /// <summary>
        /// The test context
        /// </summary>
        private Microsoft.VisualStudio.TestTools.UnitTesting.TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the text context
        /// </summary>
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext
        {
            get { return this.testContextInstance; }
            set { this.testContextInstance = value; }
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertWithNoFailure()
        {
            BaseTest tester = this.GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();
            tester.Log = new ConsoleLogger();
            tester.SoftAssert.AreEqual(string.Empty, string.Empty);
            tester.Teardown();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(System.Exception))]
        public void SoftAssertWithFailure()
        {
            BaseTest tester = this.GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();
            tester.Log = new ConsoleLogger();
            tester.SoftAssert.AreEqual(string.Empty, "d");
            tester.Teardown();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        public void SoftAssertNUnitWithNoFailure()
        {
            BaseTest tester = this.GetBaseTest();

            tester.Setup();
            tester.Log = new ConsoleLogger();
            tester.SoftAssert.AreEqual(string.Empty, string.Empty);
            tester.Teardown();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        public void SoftAssertNUnitWithFailure()
        {
            try
            {
                BaseTest tester = this.GetBaseTest();
                tester.Setup();
                tester.Log = new ConsoleLogger();
                tester.SoftAssert.AreEqual(string.Empty, "d");
                tester.Teardown();
                NUnit.Framework.Assert.Fail("Teardown should have thrown exception");
            }
            catch (Exception e)
            {
                // Assert we got the error we were looking for
                NUnit.Framework.Assert.AreEqual(typeof(Exception), e.GetType());
            }
        }

        /// <summary>
        /// Get a new base test
        /// </summary>
        /// <returns>A new base test</returns>
        protected virtual BaseTest GetBaseTest()
        {
            return new BaseTest();
        }
    }
}
