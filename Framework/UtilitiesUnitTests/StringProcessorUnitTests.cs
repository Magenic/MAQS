//--------------------------------------------------
// <copyright file="StringProcessorUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit tests for the string processor</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        #region StringFormattor
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void StringFormatterCheckForJson()
        {
            string message = StringProcessor.SafeFormatter("{This is a test for JSON}");
            Assert.AreEqual("{This is a test for JSON}\r\n", message);
        }
        #endregion

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
    }
}
