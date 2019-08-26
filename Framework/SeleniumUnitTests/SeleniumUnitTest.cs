﻿//-----------------------------------------------------
// <copyright file="SeleniumUnitTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Test the selenium framework</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the selenium framework
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumUnitTest : BaseSeleniumTest
    {
        /// <summary>
        /// Unit testing site URL - Login page
        /// </summary>
        private static readonly string TestSiteUrl = SeleniumConfig.GetWebSiteBase();

        /// <summary>
        /// Unit testing site URL - Async page
        /// </summary>
        private static readonly string TestSiteAsyncUrl = TestSiteUrl + "Automation/AsyncPage";

        /// <summary>
        /// Unit testing site URL - Automation page
        /// </summary>
        private static readonly string TestSiteAutomationUrl = TestSiteUrl + "Automation/";

        /// <summary>
        /// Home button css selector
        /// </summary>
        private static readonly By HomeButtonCssSelector = By.CssSelector("#homeButton > a");

        /// <summary>
        /// Home button css selector
        /// </summary>
        private static readonly By DropdownToggleClassSelector = By.ClassName("dropdown-toggle");

        /// <summary>
        /// Dropdown selector
        /// </summary>
        private static readonly By AsyncDropdownCssSelector = By.CssSelector("#Selector");

        /// <summary>
        /// Dropdown label
        /// </summary>
        private static readonly By AsyncOptionsLabel = By.CssSelector("#Label");

        /// <summary>
        /// Dropdown label - hidden once dropdown loads
        /// </summary>
        private static readonly By AsyncLoadingLabel = By.CssSelector("#LoadingLabel");

        /// <summary>
        /// Asynchronous div that loads after a delay on Async Testing Page
        /// </summary>
        private static readonly By AsyncLoadingTextDiv = By.CssSelector("#loading-div-text");

        /// <summary>
        /// Names label
        /// </summary>
        private static readonly By AutomationNamesLabel = By.CssSelector("#Dropdown > p > strong > label");

        /// <summary>
        /// Selector that is not in page
        /// </summary>
        private static readonly By NotInPage = By.CssSelector("NOTINPAGE");

        /// <summary>
        /// First dialog button
        /// </summary>
        private static readonly By AutomationShowDialog1 = By.CssSelector("#showDialog1");

        /// <summary>
        /// Food table
        /// </summary>
        private static readonly By FoodTable = By.CssSelector("#FoodTable");

        /// <summary>
        /// Flower table
        /// </summary>
        private static readonly By FlowerTable = By.CssSelector("#FlowerTable TD");

        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void OpenBrowser()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
        }

        /// <summary>
        /// Verify WaitForClickableElement wait works
        /// </summary>
        #region WaitForClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForClickableElement(HomeButtonCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForVisibleElement wait works
        /// </summary>
        #region WaitForVisible
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForVisibleElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(AsyncDropdownCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForExactText wait works
        /// </summary>
        #region WaitForExactText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForExactText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForExactText(AsyncOptionsLabel, "Options");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForContainsText wait works
        /// </summary>
        #region WaitForContainsText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForContainsText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForContainsText(AutomationNamesLabel, "Name");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForAbsentElement wait works
        /// </summary>
        #region WaitForAbsentElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAbsentElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            this.WebDriver.Wait().ForAbsentElement(NotInPage);
        }
        #endregion

        /// <summary>
        /// Verify WaitForAbsentElement wait fails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException))]
        public void WaitForAbsentElementFail()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Wait().ForAbsentElement(HomeButtonCssSelector);
        }

        /// <summary>
        /// Verify WaitForPageLoad wait works
        /// </summary>
        #region WaitForPageLoad
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForPageLoad()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilPageLoad wait works
        /// </summary>
        #region WaitUntilPageLoad
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilPageLoad()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilPageLoad(), "Page failed to load");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilClickableElement wait works
        /// </summary>
        #region WaitUntilClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilClickableElement(AutomationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilVisibleElement wait works
        /// </summary>
        #region WaitUntilVisible
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilVisibleElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilVisibleElement(AutomationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilExactText wait works
        /// </summary>
        #region WaitUntilExact
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilExactText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilExactText(AutomationShowDialog1, "Show dialog"), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilContainsText wait works
        /// </summary>
        #region WaitUntilContains
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilContainsText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilContainsText(AutomationShowDialog1, "dialog"), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitForAttributeTextContains wait works
        /// </summary>
        #region TryWaitForAttributeTextContains
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForAttributeTextContains()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForAttributeTextContains(AsyncLoadingTextDiv, "none;", "style", element: out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify TryWaitForAttributeTextContains wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForAttributeTextContainsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForAttributeTextContains(FoodTable, "Flower Table", "Summary", out element);
            Assert.IsFalse(found, "True was unexpectedly returned");
            Assert.IsNull(element, "Element was not null");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitForAttributeTextEquals wait works
        /// </summary>
        #region TryWaitForAttributeTextEquals
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForAttributeTextEquals()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForAttributeTextEquals(AsyncLoadingTextDiv, "display: block;", "style", out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify TryWaitForAttributeTextEquals wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForAttributeTextEqualsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForAttributeTextEquals(FoodTable, "Flower Table", "Summary", out element);
            Assert.IsFalse(found, "True was unexpectedly returned");
            Assert.IsNull(element, "Element was not null");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitForClickableElement wait works
        /// </summary>
        #region TryWaitForClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForClickableElement(HomeButtonCssSelector, out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        #region TryWaitForAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScrollIntoViewElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForClickableElementAndScrollIntoView(AutomationShowDialog1, out element);
            Assert.IsTrue(found, "False was returned.");
            Assert.IsNotNull(element, "Failed to find element.");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitForContainsText wait works
        /// </summary>
        #region TryWaitForContainsText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForContainsText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForContainsText(AutomationNamesLabel, "Name", out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitForElementExist wait works
        /// </summary>
        #region TryWaitForExist
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void TryWaitForElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForElementExist(HomeButtonCssSelector, out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitUntilExactText wait works
        /// </summary>
        #region TryWaitForExact
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitUntilExactText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForExactText(AsyncOptionsLabel, "Options", out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify TryWaitForVisibleElement wait works
        /// </summary>
        #region TryWaitForVisible
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForVisibleElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = this.WebDriver.Wait().TryForVisibleElement(AsyncDropdownCssSelector, out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify findElement works - validating a specific selector exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsNotNull(this.WebDriver.FindElement(HomeButtonCssSelector), "Element was not found");
        }

        /// <summary>
        /// Verify WaitForElementExist wait works
        /// </summary>
        #region WaitForExist
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void WaitForElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForElementExist(HomeButtonCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilElementExist wait works
        /// </summary>
        #region WaitUntilExist
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void WaitUntilElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilElementExist(AutomationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test for the wait until absent
        /// </summary>
        #region WaitUntilAbsent
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAbsentElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilAbsentElement(NotInPage));
        }
        #endregion

        /// <summary>
        /// Verify ElemList throws an exception when an Element is not on the page
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void ElemListThrowException()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element = this.WebDriver.Find().Element(NotInPage);

            Assert.Fail($"Test should have thrown an unfound error, but found element {element} instead");
        }

        /// <summary>
        /// Verify findElement works - validating a specific selector is not found
        /// </summary>
        #region FindElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsNull(this.WebDriver.Find().Element(NotInPage, false), "Element was not found");
        }
        #endregion

        /// <summary>
        /// Verify findElement works - validating a specific selector is found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element = this.WebDriver.Find().Element(AutomationNamesLabel);
            Assert.AreEqual("Names", element.Text);
        }

        /// <summary>
        /// Verify findElements works - validating that there are 3 found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementsFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            var list = this.WebDriver.Find().Elements(DropdownToggleClassSelector);
            Assert.AreEqual(3, list.Count, "There are 3 elements with dropdown classes");

            Assert.IsTrue(list.FirstOrDefault(x => x.Text == "Manage").Displayed);
            Assert.IsTrue(list.FirstOrDefault(x => x.Text == "Automation").Displayed);
            Assert.IsTrue(list.FirstOrDefault(x => x.Text == "Training").Displayed);
        }

        /// <summary>
        /// Verify findElements works - validating that there none found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementsNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            var list = this.WebDriver.Find().Elements(NotInPage,false);
            Assert.IsNull(list, "Element was not found");
        }

        /// <summary>
        /// Verify Find.Elements() throws an exception when there are no Elements existing on the page
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindElementsNotFoundThrowException()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Find().Elements(NotInPage);
        }

        /// <summary>
        /// Verify FindElementWithText = Validate null is returned if the element is not found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithTextElementNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsNull(this.WebDriver.Find().ElementWithText(NotInPage, "notInPage", false), "Element was not found");
        }

        /// <summary>
        /// Verify FindElementWithText - Validating specific text is found within a specific selector
        /// </summary>
        #region FindElementWithText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            string text = this.WebDriver.FindElement(AutomationShowDialog1).Text;
            Assert.IsNotNull(this.WebDriver.Find().ElementWithText(AutomationShowDialog1, text), "Element was not found");
        }
        #endregion

        /// <summary>
        /// Verify FindElementWithText - Validating specific text is NOT found within a specific selector
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithTextNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsNull(this.WebDriver.Find().ElementWithText(HomeButtonCssSelector, "#notfound", false), "Element was not found");
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating the correct index is returned for a specific Selector and text
        /// </summary>
        #region FindIndexFromText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithText()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(3, this.WebDriver.Find().IndexOfElementWithText(FlowerTable, "Red"));
        }
        #endregion

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating index is not returned for a specific Selector and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithTextNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(FlowerTable, "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validate that index of -1 is returned if an empty list is returned by ElemList
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithTextWithNotFoundElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(NotInPage, "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating the correct index is returned for a specific collection and text
        /// </summary>
        #region FindIndexWithText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexofElementInCollection()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(0, this.WebDriver.Find().IndexOfElementWithText(this.WebDriver.FindElements(FlowerTable), "10 in"));
        }
        #endregion

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating -1 is returned for a specific collection and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementInCollectionNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(this.WebDriver.FindElements(FlowerTable), "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - NotFoundException is thrown when an Empty input list is entered with assert == true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindIndexOfElementInCollectionEmptyInputList()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.Find().IndexOfElementWithText(new List<IWebElement>(), "#notfound", true);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - NotFoundException is thrown when the element is not found and assert == true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindIndexOfElementInCollectionTextNotFoundAssertIsTrue()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.Find().IndexOfElementWithText(this.WebDriver.FindElements(FlowerTable), "#notfound", true);
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        #region SoftAssertAreEqual
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertTest()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.AreEqual("Automation - Magenic Automation Test Site", this.WebDriver.Title, "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        #region SoftAssertIsFalse
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTest()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.IsFalse("Automation".Equals(this.WebDriver.Title), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        #region SoftAssertIsTrue
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueTest()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.IsTrue(this.WebDriver.Title.Contains("Automation"), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Verify that a screenshot is taken if the SeleniumSoftAssert.IsTrue gets a false condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueFalseCondition()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "SeleniumSoftAssertIsTrueFalseCondition.txt", MessageType.GENERIC, true);
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Jpeg";

            bool isFalse = seleniumSoftAssert.IsTrue(false, "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(screenShotLocation), "Fail to find screenshot");
            File.Delete(screenShotLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is saved if the SeleniumSoftAssert.IsTrue gets a false condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueFalseConditionPageSource()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "SeleniumSoftAssertIsTrueFalseConditionPageSource.txt", MessageType.GENERIC, true);
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = seleniumSoftAssert.IsTrue(false, "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(pageSourceLocation), "Fail to find page source");
            File.Delete(pageSourceLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that a screenshot is taken if the SeleniumSoftAssert.IsFalse gets a true condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTrueCondition()
        {
            // Make sure we initialized the web driver
            Assert.IsNotNull(this.WebDriver);

            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Jpeg";

            bool isFalse = seleniumSoftAssert.IsFalse(true, "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(screenShotLocation), "Fail to find screenshot");
            File.Delete(screenShotLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that a screenshot is not taken if no browser is initialized and the SeleniumSoftAssert.IsFalse gets a true condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTrueConditionNoBrowser()
        {
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Jpeg";

            bool isFalse = seleniumSoftAssert.IsFalse(true, "testSoftAssert", "message");

            Assert.IsFalse(File.Exists(screenShotLocation), "Should not have taken screenshot");
            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is saved if the SeleniumSoftAssert.IsFalse gets a true condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTrueConditionPageSource()
        {
            // Make sure we initialized the web driver
            Assert.IsNotNull(this.WebDriver);

            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = seleniumSoftAssert.IsFalse(true, "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(pageSourceLocation), "Fail to find page source");
            File.Delete(pageSourceLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that page source is not saved if no browser is initialized and the SeleniumSoftAssert.IsFalse gets a true condition and the logger is set to save Page Source
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTrueConditionPageSourceNoBrowser()
        {
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string pageSourceLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + "_PS (1).txt";

            bool isFalse = seleniumSoftAssert.IsFalse(true, "testSoftAssert", "message");

            Assert.IsTrue(!File.Exists(pageSourceLocation), "Should not have captured page source");
            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that SeleniumSoftAssert.AreEqual works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertAreEqual()
        {
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.TestObject);
            bool isTrue = seleniumSoftAssert.AreEqual("test string", "test string", "test message");
            Assert.IsTrue(isTrue);
        }

        /// <summary>
        /// Test to check if the soft assert fails.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(AggregateException))]
        public void SeleniumSoftAssertExpectFail()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.AreEqual("Wrong Title", this.WebDriver.Title, "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify that WaitForAttributeTextContains throws an exception for instances where the attribute is not found to contain a value.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string within an elements attribute passed.")]
        public void WaitForAttributeContainsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.Wait().ForAttributeTextContains(FoodTable, "Flower Table", "Summary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextEquals throws an exception for instances where the attribute is not found.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string equal to an elements attribute passed.")]
        public void WaitForAttributeEqualsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.Wait().ForAttributeTextEquals(FoodTable, "Flower Table", "Summary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextContains can find text within attribute after waiting.
        /// </summary>
        #region WaitForAttributeContains
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeContainsFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsNotNull(this.WebDriver.Wait().ForAttributeTextContains(AsyncLoadingTextDiv, "block;", "style"));
        }
        #endregion

        /// <summary>
        /// Verify that WaitForAttributeTextEquals can find an attribute value after waiting.
        /// </summary>
        #region WaitForAttributeEquals
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeEqualsFound()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);

            Assert.IsNotNull(this.WebDriver.Wait().ForAttributeTextEquals(AsyncLoadingTextDiv, "display: block;", "style"));
        }
        #endregion

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeContains()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilAttributeTextContains(AsyncLoadingLabel, "none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextEquals works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEquals()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilAttributeTextEquals(AsyncLoadingLabel, "display: none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this text inside attribute value within timeout.
        /// </summary>
        #region WaitUntilAttributeContains
        [TestCategory(TestCategories.Selenium)]
        [TestMethod]
        public void WaitUntilAttributeContainsFalse()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsFalse(this.WebDriver.Wait().UntilAttributeTextContains(AsyncDropdownCssSelector, "nottherightid", "id"));
        }
        #endregion

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this attribute value within timeout.
        /// </summary>
        #region WaitUntilAttributeEquals
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEqualsFalse()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsFalse(this.WebDriver.Wait().UntilAttributeTextEquals(AsyncLoadingLabel, "display:", "style"));
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        #region WaitForAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElementAndScrollIntoView(AutomationShowDialog1).Displayed, "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        #region WaitForClickableAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewWithOffsetElement()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElementAndScrollIntoView(AutomationShowDialog1, 0, 100).Displayed, "Failed to find element or scroll");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view boolean
        /// </summary>
        #region WaitUntilClickableAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewBoolean()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilClickableElementAndScrollIntoView(AutomationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view boolean
        /// </summary>
        #region WaitUntilClickableAndScrollWithOffset
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewBooleanOffset()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilClickableElementAndScrollIntoView(AutomationShowDialog1, 0, 100), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test method to scroll to an offset
        /// </summary>
        #region ExecuteScrolling
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollByOffset()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.Wait().PageLoadThanExecuteScrolling(0, 500);
        }
        #endregion

        /// <summary>
        /// Test for the get wait driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriver()
        {
            #region GetWaitDriver
            WebDriverWait driver = this.WebDriver.GetWaitDriver();
            #endregion
            Assert.AreEqual(20, driver.Timeout.Seconds);
            Assert.AreEqual(1, driver.PollingInterval.Seconds);
        }

        /// <summary>
        ///  Test for resetting the wait driver
        /// </summary>
        #region ResetWaitDriver
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriverResetWaitDriver()
        {
            WebDriverWait defaultWaitDriver = SeleniumConfig.GetWaitDriver(this.WebDriver); // default waitdriver
            this.WebDriver.ResetWaitDriver();
            WebDriverWait resetWaitDriver = this.WebDriver.GetWaitDriver(); // webdrivers default webdriver

            Assert.AreEqual(defaultWaitDriver.Timeout, resetWaitDriver.Timeout);
            Assert.AreEqual(defaultWaitDriver.Message, resetWaitDriver.Message);
            Assert.AreEqual(defaultWaitDriver.PollingInterval, resetWaitDriver.PollingInterval);
        }
        #endregion

        /// <summary>
        /// Test for GetWaitDriver - test for waitDriver not in the waitCollection 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriverNotInWaitCollection()
        {
            this.WebDriver.RemoveWaitDriver();
            WebDriverWait driver = this.WebDriver.GetWaitDriver();

            Assert.AreEqual(20, driver.Timeout.Seconds);
            Assert.AreEqual(1, driver.PollingInterval.Seconds);
        }

        /// <summary>
        /// Test for setting the wait driver
        /// </summary>
        #region SetWaitDriver
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetWaitDriver()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(10000), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Wait().ForPageLoad();
        }
        #endregion

        /// <summary>
        /// Test for removing the wait driver
        /// </summary>
        #region RemoveWaitDriver
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void RemoveWaitDriver()
        {
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(10000), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Navigate().GoToUrl(TestSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            bool removed = this.WebDriver.RemoveWaitDriver();
            Assert.IsTrue(removed);
        }
        #endregion

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.WebDriver, this.WebDriver, "Web drivers don't match");
        }

        /// <summary>
        /// Make sure test object values are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumTestObjectValuesCanBeUsed()
        {
            this.TestObject.SetValue("1", "one");

            Assert.AreEqual("one", this.TestObject.Values["1"]);
            string outValue;
            Assert.IsFalse(this.TestObject.Values.TryGetValue("2", out outValue), "Didn't expect to get value for key '2', but got " + outValue);
        }

        /// <summary>
        /// Make sure the test object objects are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumTestObjectObjectssCanBeUsed()
        {
            StringBuilder builder = new StringBuilder();
            this.TestObject.SetObject("1", builder);

            Assert.AreEqual(this.TestObject.Objects["1"], builder);

            object outObject;
            Assert.IsFalse(this.TestObject.Objects.TryGetValue("2", out outObject), "Didn't expect to get value for key '2'");

            builder.Append("123");

            Assert.AreEqual(((StringBuilder)this.TestObject.Objects["1"]).ToString(), builder.ToString());
        }

        /// <summary>
        /// Verify that SetupNoneEventFiringTester sets the proper webdriver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSetupNoneEventFiringTester()
        {
            // Take down the default driver
            this.WebDriver?.KillDriver();

            // This driver must manually be taken down
            var differentDriver = WebDriverFactory.GetDefaultBrowser();

            try
            {
                Assert.AreEqual(differentDriver.ToString(), Extend.GetLowLevelDriver(this.WebDriver).ToString());
            }
            finally
            {
                differentDriver?.KillDriver();
            }
        }

        /// <summary>
        /// Verify that CreateNewTestObject creates the correct Test Object 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumCreateNewTestObject()
        {
            this.CreateNewTestObject();
            SeleniumTestObject newTestObject = this.TestObject;

            Assert.AreEqual(this.WebDriver.ToString(), newTestObject.WebDriver.ToString());
            Assert.AreEqual(this.Log.ToString(), newTestObject.Log.ToString());
            Assert.AreEqual(this.SoftAssert.ToString(), newTestObject.SoftAssert.ToString());
            Assert.AreEqual(this.PerfTimerCollection.ToString(), newTestObject.PerfTimerCollection.ToString());
        }

        /// <summary>
        /// Verify that Selenium Navigation Event Listeners log proper messages
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumNavigationEventListeners()
        {
            this.Log = new FileLogger(string.Empty, "SeleniumNavigatorEventListenersFileLog.txt", MessageType.VERBOSE, true);
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.WebDriver.Navigate().Back();
            this.WebDriver.Navigate().Forward();

            string log = this.GetAndRemoveCustomFileLog();
            Assert.IsTrue(log.Contains("Navigating to: "));
            Assert.IsTrue(log.Contains("Navigating back to: "));
            Assert.IsTrue(log.Contains("Navigated Forward: "));
            Assert.IsTrue(log.Contains("Navigated back: "));
        }

        /// <summary>
        /// Helper function to remove created custom file logs and return contained string
        /// </summary>
        /// <returns>string contained in custom log</returns>
        protected string GetAndRemoveCustomFileLog()
        {
            FileLogger outputLog = (FileLogger)this.Log;
            string log = File.ReadAllText(outputLog.FilePath);
            try
            {
                File.Delete(outputLog.FilePath);
                string screenShotLocation = outputLog.FilePath.Substring(0, outputLog.FilePath.LastIndexOf('.')) + ".png";
                File.Delete(screenShotLocation);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found exception " + e);
            }

            this.Log = new FileLogger();
            return log;
        }
    }
}