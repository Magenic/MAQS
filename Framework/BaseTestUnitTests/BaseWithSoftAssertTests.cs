//--------------------------------------------------
// <copyright file="BaseWithSoftAssertTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Tests soft assert interactions with base test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using NUnitAssert = NUnit.Framework.Assert;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace BaseTestUnitTests
{
    /// <summary>
    /// Base test tests
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class BaseWithSoftAssertTests : BaseTest
    {
        // Cached config settings
        private static Dictionary<string, string> general;

        /// <summary>
        /// Run class setup
        /// </summary>
        /// <param name="context">The test context</param>
        [ClassInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
        public static void ClassInit(TestContext context)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            general = Config.GetSectionDictionary(ConfigSection.MagenicMaqs);
            Config.AddTestSettingValues("Log", "OnFail", "MagenicMaqs");
            Config.AddTestSettingValues("LogType", "txt", "MagenicMaqs");
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            Config.AddTestSettingValues(general, ConfigSection.MagenicMaqs);
        }

        /// <summary>
        /// Class setup for NUnit
        /// </summary>
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            ClassInit(null);
        }

        /// <summary>
        /// Class cleanup for NUnit
        /// </summary>
        [OneTimeTearDown]
        public void RunAfterAnyTests()
        {
            ClassCleanup();
        }

        /// <summary>
        /// Test duplicate name
        /// </summary>
        [TestMethod]
        public void DuplicateTestName()
        {
            AssertMultipleMethodMessage(this.Log as FileLogger);
            Assert.IsTrue(true);
        }

        /// <summary>
        /// Also test duplicate name
        /// </summary>
        /// <param name="boolean">True or false</param>
        [TestCase(true)]
        [SoftAssertExpectedAsserts("one")]
        public void DuplicateTestName(bool boolean)
        {
            AssertMultipleMethodMessage(this.Log as FileLogger);
            Assert.IsTrue(boolean);
        }

        /// <summary>
        /// Attribute expect an assert and call one assert.
        /// SoftAssert should not throw an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [SoftAssertExpectedAsserts("one")]
        public void AttributeExpectedSingle()
        {
            this.SoftAssert.Assert(() => { }, "one", "AssertionMethod");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Expect an Assert and call one assert.
        /// SoftAssert should throw an aggregate exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [SoftAssertExpectedAsserts("one")]
        [ExpectedException(typeof(System.AggregateException))]
        public void AttributeExpectedSingleMismatch()
        {
            this.SoftAssert.Assert(() => { }, "two", "AssertionMethod");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Inline expect an assert and call one assert.
        /// SoftAssert should not throw an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void InLineExpectedSingle()
        {
            this.SoftAssert.AddExpectedAsserts("one");
            this.SoftAssert.Assert(() => { }, "one", "AssertionMethod");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Expect one assert and run a different assert
        /// SoftAssert should throw an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(System.AggregateException))]
        public void InLineExpectedSingleMismatch()
        {
            this.SoftAssert.AddExpectedAsserts("two");
            this.SoftAssert.Assert(() => { }, "one", "AssertionMethod");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Attribute expect two asserts, add a duplicate expect, and run the two asserts
        /// SoftAssert should not throw an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [SoftAssertExpectedAsserts("one", "two")]
        public void AttributeExpectedMultipleWithDuplicateInLine()
        {
            this.SoftAssert.AddExpectedAsserts("one");
            this.SoftAssert.Assert(() => { }, "one");
            this.SoftAssert.Assert(() => { }, "two", "AssertionMethod");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Expect an assert from the test attribute and inline expect.
        /// SoftAssert should not throw an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [SoftAssertExpectedAsserts("one")]
        public void AttributeAndInLineExpectedMultiple()
        {
            this.SoftAssert.AddExpectedAsserts("two");
            this.SoftAssert.Assert(() => { }, "one", "AssertionMethod");
            this.SoftAssert.Assert(() => { }, "two");
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// For NUnit expect one assert and run a different assert
        /// SoftAssert should throw an exception.
        /// </summary>
        [Test]
        [TestCategory(TestCategories.Utilities)]
        public void NUnitMismatchExpectThrowsMissingAssert()
        {
            this.SoftAssert.AddExpectedAsserts("two");
            this.SoftAssert.Assert(() => { }, "one", "AssertionMethod");

            NUnitAssert.Throws<System.AggregateException>(this.SoftAssert.FailTestIfAssertFailed);
        }

        /// <summary>
        /// For NUnit expect two asserts, add a duplicate expect, and run the two asserts
        /// SoftAssert should not throw an exception.
        /// </summary>
        [Test]
        [TestCategory(TestCategories.Utilities)]
        [SoftAssertExpectedAsserts("two", "one")]
        public void NUnitMultipleExpectWithDuplicate()
        {
            this.SoftAssert.AddExpectedAsserts("two");
            this.SoftAssert.Assert(() => { }, "two");
            this.SoftAssert.Assert(() => { }, "one");

            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Assert that we got the mutliple methods message
        /// </summary>
        /// <param name="logger">Assoicated file logger</param>
        private void AssertMultipleMethodMessage(FileLogger logger)
        {
            string logText = File.ReadAllText(logger.FilePath);
            Assert.IsTrue(logText.Contains("There are mutliple methods with the name"));
        }
    }
}

