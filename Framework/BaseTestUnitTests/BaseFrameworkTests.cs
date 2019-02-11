//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Low level framework tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
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
        /// Gets or sets the text context
        /// </summary>
        public Microsoft.VisualStudio.TestTools.UnitTesting.TestContext TestContext { get; set; }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
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
        [TestCategory(TestCategories.Framework)]
        [ExpectedException(typeof(AssertFailedException))]
        [NonParallelizable]
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
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
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
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
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
                NUnit.Framework.Assert.AreEqual(typeof(AssertFailedException), e.GetType());
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
