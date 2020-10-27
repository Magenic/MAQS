//--------------------------------------------------
// <copyright file="BaseFrameworkTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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
            tester.SoftAssert.AreEqual(string.Empty, string.Empty);
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
        [SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "<Pending>")]
        public void SoftAssertExceptionWithNoMessage()
        {
            MicroAssert.ThrowsException<SoftAssertException>(() => throw new SoftAssertException(), string.Empty);
        }

        /// <summary>
        /// Soft assert throw with message
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Framework)]
        [SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "<Pending>")]
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
        [SuppressMessage("Minor Code Smell", "S3626:Jump statements should not be redundant", Justification = "<Pending>")]
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
                tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");
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
        /// Test that in core, associated test files get written to the log
        /// </summary>
        [Test]
        [Category(TestCategories.Framework)]
        [Category(TestCategories.NUnit)]
        public void TeardownWritesAssociatedFiles()
        {
            BaseTest tester = this.GetBaseTest();
            tester.TestContext = this.TestContext;
            tester.Setup();
            tester.Log = new FileLogger(string.Empty, $"{Guid.NewGuid()}.txt");

            // get log path
            string logFilePath = ((FileLogger)tester.Log).FilePath;

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
            NUnit.Framework.Assert.IsTrue(messageIsWritten, "The list of files to attach was not written to the log");
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
            tester.SoftAssert.AreEqual(string.Empty, "d");
            tester.Teardown();
        }
    }
}
