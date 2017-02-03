//--------------------------------------------------
// <copyright file="ListProcessorUnitTests.cs" company="Magenic">
//  Copyright 2015 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    /// <summary>
    /// Unit Tests for the ListProcessor class
    /// </summary>
    [TestClass]
    public class ListProcessorUnitTests : BaseVisualStudioTest
    {
        /// <summary>
        /// Unit Test for creating a comma delimited string
        /// </summary>
        [TestMethod]
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
                Assert.Fail(string.Format("Expected string [{0}] does not match Actual string [{1}]", expectedText, actualText));
            }
        }

        /// <summary>
        /// Unit Test for creating a sorted comma delimited string
        /// </summary>
        [TestMethod]
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
                Assert.Fail(string.Format("Expected string [{0}] does not match Actual string [{1}]", expectedText, actualText));
            }
        }

        /// <summary>
        /// Unit Test for comparing two lists of strings
        /// </summary>
        [TestMethod]
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

        /// <summary>
        /// Unit Test for comparing two lists of strings by order
        /// </summary>
        [TestMethod]
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
    }
}
