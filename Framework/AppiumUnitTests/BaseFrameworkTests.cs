﻿//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Low level framework tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace AppiumUnitTests
{
    /// <summary>
    /// Framework unit test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
    public class BaseFrameworkTests : BaseTestUnitTests.BaseFrameworkTests
    {
        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public new void SoftAssertWithNoFailure()
        {
            base.SoftAssertWithNoFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        [ExpectedException(typeof(AssertFailedException))]
        public new void SoftAssertWithFailure()
        {
            BaseTest tester = this.GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();
            tester.Log = new ConsoleLogger();

            tester.SoftAssert.Assert(() => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("d", string.Empty));
            tester.Teardown();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public new void SoftAssertNUnitWithNoFailure()
        {
            base.SoftAssertWithNoFailure();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public new void SoftAssertNUnitWithFailure()
        {
            base.SoftAssertNUnitWithFailure();
        }

        /// <summary>
        /// Override the base test object
        /// </summary>
        /// <returns>The base test as base Appium</returns>
        protected override BaseTest GetBaseTest()
        {
            return new BaseAppiumTest();
        }
    }
}
