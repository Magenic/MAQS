//--------------------------------------------------
// <copyright file="SoftAssertUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Unit tests for the soft asserts</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Test class for soft asserts
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SoftAssertUnitTests
    {
        /// <summary>
        /// Examples of Soft Assert fails that would pass a test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertFailsValidTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertFailsValidTest"));
            softAssert.AssertFails(() => this.MethodThrowsNullException(), typeof(NullReferenceException), "Assert Method Throws Explicit Exception", "Failed to assert that method threw a NullReferenceException");

            int one = 1;
            softAssert.AssertFails(
                () => 
            {
                one = 0;
                var result = 9 / one;
                Assert.Fail($"Result should have thrown an error but is {result} instead");
            }, 
            typeof(DivideByZeroException), 
            "Assert  action throws divide by zero exception",
            "Failed to assert that we couldn't divide by zero");

            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Examples of Soft Assert fails that would fail a test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void SoftAssertFailsInvalidTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertFailsInvalidTest"));
            softAssert.AssertFails(() => this.MethodThrowsNullException(), typeof(NotImplementedException), "Assert Method Throws Explicit Exception", "Failed to assert that method threw a NotImplementedException");

            int one = 1;
            softAssert.AssertFails(
                () =>
            {
                one = 0;
                var result = 9 / one;
                Assert.Fail($"Result should have thrown an error but is {result} instead");
            },
            typeof(NullReferenceException), 
            "Assert  dividing by zero throws a null reference", 
            "Failed to assert that we couldn't divide by zero");

            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Tests for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertValidTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertValidTest"));
            softAssert.AreEqual("Yes", "Yes", "Utilities Soft Assert", "Message is not equal");
            softAssert.AreEqual("YesAgain", "YesAgain", "Utilities Soft Assert 2");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Tests for soft assert failures
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void SoftAssertFailTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertFailTest"));
            softAssert.AreEqual("Yes", "No", "Utilities Soft Assert", "Message is not equal");
            softAssert.AreEqual("Yes", "NoAgain", "Utilities Soft Assert 2");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Will return true if no asserts are done
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertVerifyUserCheck()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertVerifyUserCheck"));
            Assert.IsTrue(softAssert.DidUserCheck());
        }

        /// <summary>
        /// Test to verify that the did user check will be set back to false if they check for failures
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertVerifyCheckForFailures()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertVerifyCheckForFailures"));
            softAssert.AreEqual("Yes", "Yes", "Utilities Soft Assert", "Message is not equal");

            softAssert.FailTestIfAssertFailed();
            Assert.IsTrue(softAssert.DidUserCheck());

            softAssert.AreEqual("Yes", "Yes", "Utilities Soft Assert", "Message is not equal");
            Assert.IsFalse(softAssert.DidUserCheck());
        }

        /// <summary>
        /// Verify the did soft asserts fail check works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertDidFailCheck()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertIsTrueTest", MessageType.GENERIC, true));
            softAssert.IsTrue(true, "Test1");
            Assert.IsFalse(softAssert.DidSoftAssertsFail());

            softAssert.IsTrue(1 == 2, "Test2");
            Assert.IsTrue(softAssert.DidSoftAssertsFail());
        }

        /// <summary>
        /// Test to verify the Is True method works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertIsTrueTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertIsTrueTest", MessageType.GENERIC, true));
            softAssert.IsTrue(true, "Test");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Test to verify that soft asserts will fail a test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void SoftAssertIsTrueTestFailure()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertFailTest", MessageType.GENERIC, true));
            softAssert.IsTrue(1 == 2, "Test");
            softAssert.IsTrue(1 == 2, "Test1");
            softAssert.IsTrue(true, "Test2");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Test to verify the Is False method works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertIsFalseTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertIsFalseTest", MessageType.GENERIC, true));
            softAssert.IsFalse(2 == 1, "Test");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify soft asserts can handle a VSUnit assert that passes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void AcceptVSAsserts()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.AcceptVSAsserts"));
            softAssert.Assert(() => Assert.AreEqual("a", "a"));
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify soft asserts can handle a NUnit assert that passes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void AcceptNUnitAsserts()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.AcceptNUnitAsserts"));
            softAssert.Assert(() => NUnit.Framework.Assert.AreEqual("a", "a"));
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify soft asserts capture VSUnit assert failures
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void CapturesVSAssertFail()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.RespectVSFailsFails"));
            softAssert.Assert(() => Assert.AreEqual("a", "b"));

            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify soft asserts capture Nunit assert failures
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void CapturesNUnitAssertFail()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.RespectNUnitFails"));
            softAssert.Assert(() => NUnit.Framework.Assert.AreEqual("a", "b"));

            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Expect an Assert and call one assert.
        /// SoftAssert should not throw an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertManuallySetExpectedAsserts()
        {
            SoftAssert softAssert = new SoftAssert(
                new FileLogger(LoggingConfig.GetLogDirectory(),
                "UnitTests.SoftAssertManuallySetExpectedAssert"));
            softAssert.AddExpectedAsserts("one");
            softAssert.Assert(() => { }, "one");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Expect SoftAssert to fail because the expected assert was never called.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void SoftAssertManuallySetExpectedAssertsFails()
        {
            SoftAssert softAssert = new SoftAssert(
                new FileLogger(LoggingConfig.GetLogDirectory(),
                "UnitTests.SoftAssertManuallySetExpectedAssertsFails"));
            softAssert.AddExpectedAsserts("one");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Throws a null reference exception
        /// </summary>
        private void MethodThrowsNullException()
        {
            throw new NullReferenceException("Reference is null");
        }
    }
}
