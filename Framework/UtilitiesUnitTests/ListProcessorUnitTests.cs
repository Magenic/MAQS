//--------------------------------------------------
// <copyright file="ListProcessorUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Unit Tests for the ListProcessor class
    /// </summary>
    [TestClass]
    public class ListProcessorUnitTests
    {
        /// <summary>
        /// Unit Test for creating a comma delimited string
        /// </summary>
        #region CommaDelimited
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CreateCommaDelimitedStringTest()
        {
            List<string> stringList = new List<string>();
            stringList.Add("Maine");
            stringList.Add("Massachusetts");
            stringList.Add("New Hampshire");
            stringList.Add("Connecticut");
            stringList.Add("Rhode Island");
            stringList.Add("Vermont");
            string expectedText = "Maine, Massachusetts, New Hampshire, Connecticut, Rhode Island, Vermont";

            string actualText = ListProcessor.CreateCommaDelimitedString(stringList);

            if (!expectedText.Equals(actualText))
            {
                Assert.Fail(StringProcessor.SafeFormatter("Expected string [{0}] does not match Actual string [{1}]", expectedText, actualText));
            }
        }
        #endregion

        /// <summary>
        /// Unit Test for creating a sorted comma delimited string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CreateSortedCommaDelimitedStringTest()
        {
            List<string> stringList = new List<string>();
            stringList.Add("Maine");
            stringList.Add("Massachusetts");
            stringList.Add("New Hampshire");
            stringList.Add("Connecticut");
            stringList.Add("Rhode Island");
            stringList.Add("Vermont");
            string expectedText = "Connecticut, Maine, Massachusetts, New Hampshire, Rhode Island, Vermont";

            string actualText = ListProcessor.CreateCommaDelimitedString(stringList, true);

            if (!expectedText.Equals(actualText))
            {
                Assert.Fail(StringProcessor.SafeFormatter("Expected string [{0}] does not match Actual string [{1}]", expectedText, actualText));
            }
        }

        /// <summary>
        /// Unit Test for comparing two lists of strings
        /// </summary>
        #region ListProcessorCompare
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ListOfStringsComparerTest()
        {
            StringBuilder results = new StringBuilder();
            List<string> expectedList = new List<string>();
            List<string> actualList = new List<string>();
            expectedList.Add("Maine");
            expectedList.Add("Massachusetts");
            expectedList.Add("New Hampshire");
            expectedList.Add("Connecticut");
            expectedList.Add("Rhode Island");
            expectedList.Add("Vermont");

            actualList.Add("Massachusetts");
            actualList.Add("Connecticut");
            actualList.Add("Rhode Island");
            actualList.Add("Vermont");
            actualList.Add("Maine");
            actualList.Add("New Hampshire");

            ListProcessor.ListOfStringsComparer(expectedList, actualList, results);

            if (results.Length > 0)
            {
                Assert.Fail("{0}{1}", results.ToString(), Environment.NewLine);
            }
        }
        #endregion

        /// <summary>
        /// Unit Test for comparing two lists of strings by order
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ListOfStringsComparerByOrderTest()
        {
            StringBuilder results = new StringBuilder();
            List<string> expectedList = new List<string>();
            List<string> actualList = new List<string>();
            expectedList.Add("Maine");
            expectedList.Add("Massachusetts");
            expectedList.Add("New Hampshire");
            expectedList.Add("Connecticut");
            expectedList.Add("Rhode Island");
            expectedList.Add("Vermont");

            actualList.Add("Maine");
            actualList.Add("Massachusetts");
            actualList.Add("New Hampshire");
            actualList.Add("Connecticut");
            actualList.Add("Rhode Island");
            actualList.Add("Vermont");

            ListProcessor.ListOfStringsComparer(expectedList, actualList, results, true);

            if (results.Length > 0)
            {
                Assert.Fail("{0}{1}", results.ToString(), Environment.NewLine);
            }
        }

        /// <summary>
        /// Verify that ListOfStringsComparer handles lists of unequal length as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ListOfStringsComparerUnequalLengths()
        {
            StringBuilder results = new StringBuilder();
            List<string> expectedList = new List<string>();
            List<string> actualList = new List<string>();
            expectedList.Add("A");
            expectedList.Add("B");

            actualList.Add("A");
            bool isEqual = ListProcessor.ListOfStringsComparer(expectedList, actualList, results, true);
            Assert.IsTrue(results.ToString().Contains("The following lists are not the same size:"));
            Assert.IsFalse(isEqual);
        }

        /// <summary>
        /// Verify that ListOfStringsComparer handles not finding an item in the expected list correctly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ListOfStringsComparerItemNotFound()
        {
            StringBuilder results = new StringBuilder();
            List<string> expectedList = new List<string>();
            List<string> actualList = new List<string>();
            expectedList.Add("A");
            expectedList.Add("B");

            actualList.Add("A");
            actualList.Add("C");
            bool isEqual = ListProcessor.ListOfStringsComparer(expectedList, actualList, results, false);
            Assert.IsTrue(results.ToString().Contains("[C] was not found in the list but was expected"));
            Assert.IsFalse(isEqual);
        }

        /// <summary>
        /// Verify that ListOfStringsComparer handles inequality between lists as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void ListOfStringsComparerItemNotMatching()
        {
            StringBuilder results = new StringBuilder();
            List<string> expectedList = new List<string>();
            List<string> actualList = new List<string>();
            expectedList.Add("A");
            expectedList.Add("B");

            actualList.Add("A");
            actualList.Add("C");
            bool isEqual = ListProcessor.ListOfStringsComparer(expectedList, actualList, results, true);
            Assert.IsTrue(results.ToString().Contains("Expected [C] but found [B]"));
            Assert.IsFalse(isEqual);
        }
    }
}
