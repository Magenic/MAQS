//--------------------------------------------------
// <copyright file="SoftAssertUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit tests for the soft asserts</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
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
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before we start running unit tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running unit tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        #region SoftAssertAreEqualPasses
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
        #endregion

        #region SoftAssertAreEqualFails
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
        #endregion

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

        #region SoftAssertChecked
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
        #endregion

        /// <summary>
        /// Verify the did soft asserts fail check works
        /// </summary>
        #region SoftAssertDidFail
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertDidFailCheck()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertIsTrueTest", MessageType.GENERIC, true));
            softAssert.IsTrue(1 == 1, "Test1");
            Assert.IsFalse(softAssert.DidSoftAssertsFail());

            softAssert.IsTrue(1 == 2, "Test2");
            Assert.IsTrue(softAssert.DidSoftAssertsFail());
        }
        #endregion

        /// <summary>
        /// Test to verify the Is True method works
        /// </summary>
        #region SoftAssertIsTrue
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertIsTrueTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertIsTrueTest", MessageType.GENERIC, true));
            softAssert.IsTrue(1 == 1, "Test");
            softAssert.FailTestIfAssertFailed();
        }
        #endregion

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
            softAssert.IsTrue(1 == 1, "Test2");
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Test to verify the Is False method works
        /// </summary>
        #region SoftAssertIsFalse
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SoftAssertIsFalseTest()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertIsFalseTest", MessageType.GENERIC, true));
            softAssert.IsFalse(2 == 1, "Test");
            softAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Test to verify that soft asserts will fail a test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(AggregateException))]
        public void SoftAssertIsFalseTestFailure()
        {
            SoftAssert softAssert = new SoftAssert(new FileLogger(LoggingConfig.GetLogDirectory(), "UnitTests.SoftAssertUnitTests.SoftAssertFailTest", MessageType.GENERIC, true));
            softAssert.IsTrue(1 == 2, "Test");
            softAssert.IsTrue(1 == 2, "Test1");
            softAssert.IsTrue(1 == 1, "Test2");
            softAssert.FailTestIfAssertFailed();
        }

        #region VSAssert
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
        #endregion

        #region NUnitAssert
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
        #endregion

        #region VSAssertFail
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
        #endregion

        #region NUnitAssertFail
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
        #endregion
    }
}
