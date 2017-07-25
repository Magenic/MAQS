//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Low level framework tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Framework unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseFrameworkTests : BaseTestUnitTests.BaseFrameworkTests
    {
        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        public new void SoftAssertWithNoFailure()
        {
            base.SoftAssertWithNoFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(System.Exception))]
        public new void SoftAssertWithFailure()
        {
            base.SoftAssertWithFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        public new void SoftAssertNUnitWithNoFailure()
        {
            base.SoftAssertWithNoFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        public new void SoftAssertNUnitWithFailure()
        {
            base.SoftAssertNUnitWithFailure();
        }

        /// <summary>
        /// Override the base test object
        /// </summary>
        /// <returns>The base test as base Selenium</returns>
        protected override BaseTest GetBaseTest()
        {
            return new BaseSeleniumTest();
        }
    }
}
