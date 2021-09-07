//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
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
using System.IO;
using MicroAssert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BaseTestUnitTests
{
    /// <summary>
    /// Framework unit test class
    /// </summary>
    [TestClass]
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
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

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.Assert(() => Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(string.Empty, string.Empty));
            tester.Teardown();
        }

        /// <summary>
        ///  Base test does soft assert check
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        [ExpectedException(typeof(AssertFailedException))]
        public void SoftAssertWithFailureBase()
        {
            this.SoftAssertWithFailure();
        }

        /// <summary>
        ///  Soft assert throw without message
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void SoftAssertExceptionWithNoMessage()
        {
            MicroAssert.ThrowsException<SoftAssertException>(() => throw new SoftAssertException(), string.Empty);
        }

        /// <summary>
        /// Soft assert throw with message
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void SoftAssertExceptionWithMessage()
        {
            string error = "ERROR";
            MicroAssert.ThrowsException<SoftAssertException>(() => throw new SoftAssertException(error), error);
        }

        /// <summary>
        /// Soft assert throw with message and inner exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        public void SoftAssertExceptionWithInnerException()
        {
            string error = "ERROR";
            MicroAssert.ThrowsException<SoftAssertException>(() => throw new SoftAssertException(error, new Exception()), error);
        }

        /// <summary>
        /// Soft assert is expecting to be called at least once for key "one".
        /// </summary>
        [TestMethod]
        [SoftAssertExpectedAsserts("one")]
        [Category(TestCategories.Framework)]
        [ExpectedException(typeof(AssertFailedException))]
        public void SoftAssertDoesNotAssertExpected()
        {
            var tester = GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();
            tester.Teardown();
        }

        /// <summary>
        /// Can test object be used to create other test objects
        /// </summary>
        [TestMethod]
        [Category(TestCategories.Framework)]
        public void TestObject()
        {
            var tester = GetBaseTest();

            var testObject = tester.TestObject;
            var newTestObject = new BaseTestObject(testObject);

            MicroAssert.AreEqual(testObject.Log, newTestObject.Log);
            MicroAssert.AreEqual(testObject.SoftAssert, newTestObject.SoftAssert);
            MicroAssert.AreEqual(testObject.PerfTimerCollection, newTestObject.PerfTimerCollection);
            MicroAssert.AreEqual(testObject.Values, newTestObject.Values);
            MicroAssert.AreEqual(testObject.Objects, newTestObject.Objects);
            MicroAssert.AreEqual(testObject.ManagerStore, newTestObject.ManagerStore);
            MicroAssert.AreEqual(testObject.AssociatedFiles, newTestObject.AssociatedFiles);
        }

        /// <summary>
        /// Can test object be used to create other test objects
        /// </summary>
        [TestMethod]
        [Category(TestCategories.Framework)]
        public void TestObjectWithLogger()
        {
            var tester = GetBaseTest();

            var testObject = tester.TestObject;
            var newTestObject = new BaseTestObject(testObject.Log, "TEST");

            MicroAssert.AreEqual(testObject.Log, newTestObject.Log);
            MicroAssert.AreNotEqual(testObject.SoftAssert, newTestObject.SoftAssert);
            MicroAssert.AreEqual("TEST", newTestObject.PerfTimerCollection.TestName);
            MicroAssert.AreNotEqual(testObject.PerfTimerCollection, newTestObject.PerfTimerCollection);
            MicroAssert.AreNotEqual(testObject.Values, newTestObject.Values);
            MicroAssert.AreNotEqual(testObject.Objects, newTestObject.Objects);
            MicroAssert.AreNotEqual(testObject.ManagerStore, newTestObject.ManagerStore);
            MicroAssert.AreNotEqual(testObject.AssociatedFiles, newTestObject.AssociatedFiles);
        }

        /// <summary>
        /// Soft assert is expect assert with key "one" to be called.
        /// Throws exception on FailTestIfAssertFailed()
        /// </summary>
        [TestMethod]
        [SoftAssertExpectedAsserts("one")]
        [Category(TestCategories.Framework)]
        [ExpectedException(typeof(AggregateException))]
        public void FailSoftAssertIfExpectedAssertsAreNotCalled()
        {
            var tester = GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();

            tester.SoftAssert.FailTestIfAssertFailed();
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

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.Assert(() => MicroAssert.AreEqual(string.Empty, string.Empty));
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

                tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
                tester.SoftAssert.Assert(() => MicroAssert.AreEqual("d", string.Empty));
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
        /// Test that in core, associated test files don't get written to the log if they exist
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public void TeardownDoesNotWriteAssociatedFiles()
        {
            BaseTest tester = this.GetBaseTest();
            tester.Setup();

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");

            // get log path
            string logFilePath = ((IFileLogger)tester.Log).FilePath;

            // create test files
            Directory.CreateDirectory("TeardownTest");

            File.Create(@"TeardownTest/AssocFileToAttach1.txt").Dispose();

#pragma warning disable S3966 // Objects should not be disposed more than once
            File.Create(@"TeardownTest/AssocFileToAttach2.txt").Dispose();
#pragma warning restore S3966 // Objects should not be disposed more than once

            // add associated files
            tester.TestObject.AddAssociatedFile(@"TeardownTest/AssocFileToAttach1.txt");
            tester.TestObject.AddAssociatedFile(@"TeardownTest/AssocFileToAttach2.txt");

            // call the teardown to add associated files
            tester.Teardown();

            // test the list of associated files is written to the log
            bool messageIsWritten = false;

            using (StreamReader sr = File.OpenText(logFilePath))
            {
                string[] lines = File.ReadAllLines(logFilePath);

                for (int x = 0; x < lines.Length - 1; x++)
                {
                    if (lines[x] == "GENERIC:	List of Associated Files: ")
                    {
                        messageIsWritten = true;

                        NUnit.Framework.Assert.AreEqual(@"TeardownTest/AssocFileToAttach1.txt", lines[x + 1]);
                        NUnit.Framework.Assert.AreEqual(@"TeardownTest/AssocFileToAttach2.txt", lines[x + 2]);
                    }
                }
            }

            // cleanup the test files
            File.Delete(logFilePath);
            Directory.Delete("TeardownTest", true);
            NUnit.Framework.Assert.IsFalse(messageIsWritten, "The list of files to attach should not be written to the log");
        }

        [TestMethod]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public void SoftAssertAssertSuccess()
        {
            var tester = GetBaseTest();
            tester.Setup();

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.Assert(() => { }, "one");
            tester.Teardown();
            MicroAssert.IsFalse(tester.SoftAssert.DidSoftAssertsFail());
            NUnit.Framework.Assert.IsFalse(tester.SoftAssert.DidSoftAssertsFail());
        }

        [TestMethod]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public void SoftAssertAssertFailed()
        {
            var tester = GetBaseTest();
            tester.Setup();

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.Assert(() => throw new Exception("broke"), "Name1");
            tester.SoftAssert.Assert(() => throw new Exception("broke again"), "Name2");
            try
            {
                tester.SoftAssert.FailTestIfAssertFailed();
                MicroAssert.Fail();
                NUnit.Framework.Assert.Fail();
            }
            catch (AggregateException aggregateException)
            {
                MicroAssert.AreEqual(
                    2,
                    aggregateException.InnerExceptions.Count,
                    "Incorrect number of inner exceptions in Soft Assert");
                NUnit.Framework.Assert.AreEqual(
                    2,
                    aggregateException.InnerExceptions.Count,
                    "Incorrect number of inner exceptions in Soft Assert");
            }
        }

        [TestMethod]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public void SoftAssertAssertFails()
        {
            var tester = GetBaseTest();
            tester.Setup();

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.AssertFails(() => throw new Exception("broke"), typeof(Exception), "one");
            tester.Teardown();
        }

        [TestMethod]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public void SoftAssertAssertFailsFailed()
        {
            var tester = GetBaseTest();
            tester.Setup();

            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.AssertFails(() => throw new Exception("broke"), typeof(AggregateException), "one");
            try
            {
                tester.SoftAssert.FailTestIfAssertFailed();

            }
            catch (AggregateException aggregateException)
            {
                MicroAssert.AreEqual(
                    1,
                    aggregateException.InnerExceptions.Count,
                    "Incorrect number of inner exceptions in Soft Assert");
                NUnit.Framework.Assert.AreEqual(
                    1,
                    aggregateException.InnerExceptions.Count,
                    "Incorrect number of inner exceptions in Soft Assert");
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

        /// <summary>
        /// Soft assert with failure functionality
        /// </summary>
        protected void SoftAssertWithFailure()
        {
            BaseTest tester = this.GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();
            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
            tester.SoftAssert.Assert(() => MicroAssert.AreEqual("d", string.Empty));
            tester.Teardown();
        }
    }
}
