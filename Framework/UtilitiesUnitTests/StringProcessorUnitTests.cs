//--------------------------------------------------
// <copyright file="StringProcessorUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Unit tests for the string processor</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Initializes a new instance of the StringProcessorUnitTests class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StringProcessorUnitTests
    {
        /// <summary>
        /// Test method for checking JSON strings
        /// </summary>

        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void StringFormatterCheckForJson()
        {
            string message = StringProcessor.SafeFormatter("{This is a test for JSON}");
            Assert.AreEqual("{This is a test for JSON}\r\n", message);
        }

        /// <summary>
        /// Test method for checking string format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void StringFormatterCheckForStringFormat()
        {
            string message = StringProcessor.SafeFormatter("This {0} should return {1}", "Test", "Test");
            Assert.AreEqual("This Test should return Test", message);
        }

        /// <summary>
        /// Verify that StringProcessor.SafeFormatter handles errors in the message as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void StringFormatterThrowException()
        {
            string message = StringProcessor.SafeFormatter("This {0} should return {5}", "Test", "Test", "Test");
            Assert.IsTrue(message.Contains("This {0} should return {5}"));
        }

        /// <summary>
        /// Single nested exception to test with aggregation
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void NestedExceptionSafeFormatter()
        {
            FormatException fe = new FormatException("Format Exception");
            InvalidOperationException ioe = new InvalidOperationException("Invalid Operation Exception");
            AggregateException ae = new AggregateException(fe, ioe);
            string formattedException = StringProcessor.SafeExceptionFormatter(ae);
            Assert.IsTrue(formattedException.Contains("Format Exception"));
            Assert.IsTrue(formattedException.Contains("Invalid Operation Exception"));
            Assert.IsTrue(formattedException.Contains("One or more errors occurred"));
        }

        /// <summary>
        /// Nested aggregation exceptions to validate string data
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void DoubleNestedExceptionSafeFormatter()
        {
            FormatException fe = new FormatException("Format Exception");
            InvalidOperationException ioe = new InvalidOperationException("Invalid Operation Exception");
            AggregateException ae = new AggregateException(fe, ioe);
            AggregateException ae1 = new AggregateException(ae);
            string formattedException = StringProcessor.SafeExceptionFormatter(ae1);
            Assert.IsTrue(formattedException.Contains("Format Exception"));
            Assert.IsTrue(formattedException.Contains("Invalid Operation Exception"));
            Assert.IsTrue(formattedException.Contains("One or more errors occurred"));
        }

        /// <summary>
        /// Single exception to validate string data
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SingleExceptionSafeFormatter()
        {
            FormatException fe = new FormatException("Format Exception");
            string formattedException = StringProcessor.SafeExceptionFormatter(fe);
            Assert.IsTrue(formattedException.Contains("Format Exception"));
        }

        /// <summary>
        /// Tests the stack trace is formatted and included in the string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ThrowSingleExceptionSafeFormatter()
        {
            FormatException fe = new FormatException("Format Exception");
            try
            {
                throw fe;
            }
            catch (Exception e)
            {
                string formattedException = StringProcessor.SafeExceptionFormatter(e);
                Assert.IsTrue(formattedException.Contains("Format Exception"));
                Assert.IsTrue(formattedException.Contains("at UtilitiesUnitTesting.StringProcessorUnitTests.ThrowSingleExceptionSafeFormatter()"));
            }
        }

        /// <summary>
        /// Nested aggregation exceptions to validate stack trace is included
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ThrowDoubleNestedExceptionSafeFormatter()
        {
            FormatException fe = new FormatException("Format Exception");
            InvalidOperationException ioe = new InvalidOperationException("Invalid Operation Exception");
            AggregateException ae = new AggregateException(fe, ioe);
            AggregateException ae1;
            try
            {
                throw ae;
            }
            catch (Exception e)
            {
                ae1 = new AggregateException(e);
            }

            string formattedException = StringProcessor.SafeExceptionFormatter(ae1);
            Assert.IsTrue(formattedException.Contains("at UtilitiesUnitTesting.StringProcessorUnitTests.ThrowDoubleNestedExceptionSafeFormatter()"));
            Assert.IsTrue(formattedException.Contains("Format Exception"));
            Assert.IsTrue(formattedException.Contains("Invalid Operation Exception"));
            Assert.IsTrue(formattedException.Contains("One or more errors occurred"));
        }
    }
}