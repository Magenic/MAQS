//-----------------------------------------------------
// <copyright file="SeleniumUnitTest.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
        }

        /// <summary>
        /// Verify WaitForClickableElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForClickableElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element = WebDriver.Wait().ForClickableElement(HomeButtonCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify WaitForVisibleElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForVisibleElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element = WebDriver.Wait().ForVisibleElement(AsyncDropdownCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify WaitForExactText wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForExactText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element = WebDriver.Wait().ForExactText(AsyncOptionsLabel, "Options");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify WaitForContainsText wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForContainsText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element = WebDriver.Wait().ForContainsText(AutomationNamesLabel, "Name");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify WaitForAbsentElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAbsentElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForAbsentElement(NotInPage);
        }

        /// <summary>
        /// Verify WaitForAbsentElement wait fails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException))]
        public void WaitForAbsentElementFail()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            WebDriver.Wait().ForAbsentElement(HomeButtonCssSelector);
        }

        /// <summary>
        /// Verify WaitForPageLoad wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForPageLoad()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
        }

        /// <summary>
        /// Verify WaitUntilPageLoad wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilPageLoad()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsTrue(WebDriver.Wait().UntilPageLoad(), "Page failed to load");
        }

        /// <summary>
        /// Verify WaitUntilClickableElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilClickableElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilClickableElement(AutomationShowDialog1), "Failed to find element");
        }

        /// <summary>
        /// Verify WaitUntilVisibleElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilVisibleElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilVisibleElement(AutomationShowDialog1), "Failed to find element");
        }

        /// <summary>
        /// Verify WaitUntilExactText wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilExactText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilExactText(AutomationShowDialog1, "Show dialog"), "Failed to find element");
        }

        /// <summary>
        /// Verify WaitUntilContainsText wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilContainsText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilContainsText(AutomationShowDialog1, "dialog"), "Failed to find element");
        }

        /// <summary>
        /// Verify TryWaitForAttributeTextContains wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForAttributeTextContains()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            bool found = WebDriver.Wait().TryForAttributeTextContains(AsyncLoadingTextDiv, "none;", "style", element: out IWebElement element);
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
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForAttributeTextContains(FoodTable, "Flower Table", "Summary", out element);
            Assert.IsFalse(found, "True was unexpectedly returned");
            Assert.IsNull(element, "Element was not null");
        }

        /// <summary>
        /// Verify TryWaitForAttributeTextEquals wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForAttributeTextEquals()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForAttributeTextEquals(AsyncLoadingTextDiv, "display: block;", "style", out element);
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
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForAttributeTextEquals(FoodTable, "Flower Table", "Summary", out element);
            Assert.IsFalse(found, "True was unexpectedly returned");
            Assert.IsNull(element, "Element was not null");
        }

        /// <summary>
        /// Verify TryWaitForClickableElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForClickableElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForClickableElement(HomeButtonCssSelector, out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScrollIntoViewElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForClickableElementAndScrollIntoView(AutomationShowDialog1, out element);
            Assert.IsTrue(found, "False was returned.");
            Assert.IsNotNull(element, "Failed to find element.");
        }

        /// <summary>
        /// Verify TryWaitForContainsText wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForContainsText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForContainsText(AutomationNamesLabel, "Name", out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify TryWaitForElementExist wait works
        /// </summary>
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void TryWaitForElementExist()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForElementExist(HomeButtonCssSelector, out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify TryWaitUntilExactText wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitUntilExactText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForExactText(AsyncOptionsLabel, "Options", out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify TryWaitForVisibleElement wait works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryWaitForVisibleElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            IWebElement element;
            bool found = WebDriver.Wait().TryForVisibleElement(AsyncDropdownCssSelector, out element);
            Assert.IsTrue(found, "False was returned");
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify findElement works - validating a specific selector exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsNotNull(WebDriver.FindElement(HomeButtonCssSelector), "Element was not found");
        }

        /// <summary>
        /// Verify WaitForElementExist wait works
        /// </summary>
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void WaitForElementExist()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            IWebElement element = WebDriver.Wait().ForElementExist(HomeButtonCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }

        /// <summary>
        /// Verify WaitUntilElementExist wait works
        /// </summary>
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void WaitUntilElementExist()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilElementExist(AutomationShowDialog1), "Failed to find element");
        }

        /// <summary>
        /// Test for the wait until absent
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAbsentElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsTrue(WebDriver.Wait().UntilAbsentElement(NotInPage));
        }

        /// <summary>
        /// Verify ElemList throws an exception when an Element is not on the page
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void ElemListThrowException()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element = WebDriver.Find().Element(NotInPage);

            Assert.Fail($"Test should have thrown an unfound error, but found element {element} instead");
        }

        /// <summary>
        /// Verify findElement works - validating a specific selector is not found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementNotFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsNull(WebDriver.Find().Element(NotInPage, false), "Element was not found");
        }

        /// <summary>
        /// Verify findElement works - validating a specific selector is found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            IWebElement element = WebDriver.Find().Element(AutomationNamesLabel);
            Assert.AreEqual("Names", element.Text);
        }

        /// <summary>
        /// Verify findElements works - validating that there are 3 found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementsFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            var list = WebDriver.Find().Elements(DropdownToggleClassSelector);
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
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            var list = WebDriver.Find().Elements(NotInPage, false);
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
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            WebDriver.Find().Elements(NotInPage);
        }

        /// <summary>
        /// Verify FindElementWithText = Validate null is returned if the element is not found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithTextElementNotFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsNull(WebDriver.Find().ElementWithText(NotInPage, "notInPage", false), "Element was not found");
        }

        /// <summary>
        /// Verify FindElementWithText - Validating specific text is found within a specific selector
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            string text = WebDriver.FindElement(AutomationShowDialog1).Text;
            Assert.IsNotNull(WebDriver.Find().ElementWithText(AutomationShowDialog1, text), "Element was not found");
        }

        /// <summary>
        /// Verify FindElementWithText - Validating specific text is NOT found within a specific selector
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithTextNotFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            Assert.IsNull(WebDriver.Find().ElementWithText(HomeButtonCssSelector, "#notfound", false), "Element was not found");
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating the correct index is returned for a specific Selector and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithText()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(3, WebDriver.Find().IndexOfElementWithText(FlowerTable, "Red"));
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating index is not returned for a specific Selector and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithTextNotFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(WebDriver.Find().IndexOfElementWithText(FlowerTable, "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validate that index of -1 is returned if an empty list is returned by ElemList
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithTextWithNotFoundElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(WebDriver.Find().IndexOfElementWithText(NotInPage, "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating the correct index is returned for a specific collection and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexofElementInCollection()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(0, WebDriver.Find().IndexOfElementWithText(WebDriver.FindElements(FlowerTable), "10 in"));
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating -1 is returned for a specific collection and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementInCollectionNotFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.AreEqual(WebDriver.Find().IndexOfElementWithText(WebDriver.FindElements(FlowerTable), "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - NotFoundException is thrown when an Empty input list is entered with assert == true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindIndexOfElementInCollectionEmptyInputList()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Find().IndexOfElementWithText(new List<IWebElement>(), "#notfound", true);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - NotFoundException is thrown when the element is not found and assert == true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindIndexOfElementInCollectionTextNotFoundAssertIsTrue()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Find().IndexOfElementWithText(WebDriver.FindElements(FlowerTable), "#notfound", true);
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertTest()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.AreEqual("Automation - Magenic Automation Test Site", WebDriver.Title, "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTest()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.IsFalse("Automation".Equals(WebDriver.Title), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueTest()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.IsTrue(WebDriver.Title.Contains("Automation"), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertEmptyMessageTest()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.IsTrue(WebDriver.Title.Contains("Automation"), "", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(AggregateException))]
        public void SeleniumSoftAssertIsTrueFalseBoolean()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            var result = SoftAssert.IsTrue(!WebDriver.Title.Contains("Automation"), "", "Title is incorrect");
            Assert.IsFalse(result);
            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify that a screenshot is taken if the SeleniumSoftAssert.IsTrue gets a false condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueFalseCondition()
        {
            this.WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "SeleniumSoftAssertIsTrueFalseCondition.txt", MessageType.GENERIC, true);
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
            string logLocation = ((FileLogger)Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Png";

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
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
            string logLocation = ((FileLogger)Log).FilePath;
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

            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
            string logLocation = ((FileLogger)Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " testSoftAssert" + " (1).Png";

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
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
            string logLocation = ((FileLogger)Log).FilePath;
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

            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
            string logLocation = ((FileLogger)Log).FilePath;
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
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
            string logLocation = ((FileLogger)Log).FilePath;
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
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(TestObject);
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
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            SoftAssert.AreEqual("Wrong Title", WebDriver.Title, "Title Test", "Title is incorrect");
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
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Wait().ForAttributeTextContains(FoodTable, "Flower Table", "Summary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextEquals throws an exception for instances where the attribute is not found.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string equal to an elements attribute passed.")]
        public void WaitForAttributeEqualsDontFind()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Wait().ForAttributeTextEquals(FoodTable, "Flower Table", "Summary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextContains can find text within attribute after waiting.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeContainsFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsNotNull(WebDriver.Wait().ForAttributeTextContains(AsyncLoadingTextDiv, "block;", "style"));
        }

        /// <summary>
        /// Verify that WaitForAttributeTextEquals can find an attribute value after waiting.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeEqualsFound()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);

            Assert.IsNotNull(WebDriver.Wait().ForAttributeTextEquals(AsyncLoadingTextDiv, "display: block;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeContains()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsTrue(WebDriver.Wait().UntilAttributeTextContains(AsyncLoadingLabel, "none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextEquals works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEquals()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsTrue(WebDriver.Wait().UntilAttributeTextEquals(AsyncLoadingLabel, "display: none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this text inside attribute value within timeout.
        /// </summary>
        [TestCategory(TestCategories.Selenium)]
        [TestMethod]
        public void WaitUntilAttributeContainsFalse()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsFalse(WebDriver.Wait().UntilAttributeTextContains(AsyncDropdownCssSelector, "nottherightid", "id"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this attribute value within timeout.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEqualsFalse()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAsyncUrl);
            Assert.IsFalse(WebDriver.Wait().UntilAttributeTextEquals(AsyncLoadingLabel, "display:", "style"));
        }

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().ForClickableElementAndScrollIntoView(AutomationShowDialog1).Displayed, "Failed to find element");
        }

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewWithOffsetElement()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().ForClickableElementAndScrollIntoView(AutomationShowDialog1, 0, 100).Displayed, "Failed to find element or scroll");
        }

        /// <summary>
        /// Test method to check the element can scroll into the view boolean
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewBoolean()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilClickableElementAndScrollIntoView(AutomationShowDialog1), "Failed to find element");
        }

        /// <summary>
        /// Test method to check the element can scroll into the view boolean
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewBooleanOffset()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            Assert.IsTrue(WebDriver.Wait().UntilClickableElementAndScrollIntoView(AutomationShowDialog1, 0, 100), "Failed to find element");
        }

        /// <summary>
        /// Test method to scroll to an offset
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollByOffset()
        {
            WebDriver.Navigate().GoToUrl(TestSiteAutomationUrl);
            WebDriver.Wait().PageLoadThanExecuteScrolling(0, 500);
        }

        /// <summary>
        /// Test for the get wait driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriver()
        {
            WebDriverWait driver = WebDriver.GetWaitDriver();
            Assert.AreEqual(20, driver.Timeout.Seconds);
            Assert.AreEqual(1, driver.PollingInterval.Seconds);
        }

        /// <summary>
        ///  Test for resetting the wait driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriverResetWaitDriver()
        {
            WebDriverWait defaultWaitDriver = SeleniumConfig.GetWaitDriver(WebDriver); // default waitdriver
            WebDriver.ResetWaitDriver();
            WebDriverWait resetWaitDriver = WebDriver.GetWaitDriver(); // webdrivers default webdriver

            Assert.AreEqual(defaultWaitDriver.Timeout, resetWaitDriver.Timeout);
            Assert.AreEqual(defaultWaitDriver.Message, resetWaitDriver.Message);
            Assert.AreEqual(defaultWaitDriver.PollingInterval, resetWaitDriver.PollingInterval);
        }

        /// <summary>
        /// Test for GetWaitDriver - test for waitDriver not in the waitCollection
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriverNotInWaitCollection()
        {
            WebDriver.RemoveWaitDriver();
            WebDriverWait driver = WebDriver.GetWaitDriver();

            Assert.AreEqual(20, driver.Timeout.Seconds);
            Assert.AreEqual(1, driver.PollingInterval.Seconds);
        }

        /// <summary>
        /// Test for setting the wait driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetWaitDriver()
        {
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), WebDriver, TimeSpan.FromMilliseconds(10000), TimeSpan.FromMilliseconds(10)));
            WebDriver.Wait().ForPageLoad();
        }

        /// <summary>
        /// Test for removing the wait driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void RemoveWaitDriver()
        {
            WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), WebDriver, TimeSpan.FromMilliseconds(10000), TimeSpan.FromMilliseconds(10)));
            WebDriver.Navigate().GoToUrl(TestSiteUrl);
            WebDriver.Wait().ForPageLoad();
            bool removed = WebDriver.RemoveWaitDriver();
            Assert.IsTrue(removed);
        }

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
            TestObject.SetValue("1", "one");

            Assert.AreEqual("one", TestObject.Values["1"]);
            string outValue;
            Assert.IsFalse(TestObject.Values.TryGetValue("2", out outValue), "Didn't expect to get value for key '2', but got " + outValue);
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
            TestObject.SetObject("1", builder);

            Assert.AreEqual(TestObject.Objects["1"], builder);

            object outObject;
            Assert.IsFalse(TestObject.Objects.TryGetValue("2", out outObject), "Didn't expect to get value for key '2'");

            builder.Append("123");

            Assert.AreEqual(((StringBuilder)TestObject.Objects["1"]).ToString(), builder.ToString());
        }

        /// <summary>
        /// Verify that SetupNoneEventFiringTester sets the proper webdriver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSetupNoneEventFiringTester()
        {
            // Take down the default driver
            WebDriver?.KillDriver();

            // This driver must manually be taken down
            var differentDriver = WebDriverFactory.GetDefaultBrowser();

            try
            {
                Assert.AreEqual(differentDriver.ToString(), Extend.GetLowLevelDriver(WebDriver).ToString());
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
            SeleniumTestObject newTestObject = TestObject;

            var expectedWebDriver = WebDriver.ToString();
            var actualWebDriver = newTestObject.WebDriver.ToString();
            var expectedLog = Log.ToString();
            var actualLog = newTestObject.Log.ToString();
            var expectedSoftAssert = SoftAssert.ToString();
            var actualSoftAssert = newTestObject.SoftAssert.ToString();
            var expectedPerf = PerfTimerCollection.ToString();
            var actualPerf = newTestObject.PerfTimerCollection.ToString();

            // Need to quit webdriver here in order for remote browser to close browser correctly.
            newTestObject.WebDriver.Quit();
            Assert.AreEqual(expectedWebDriver, actualWebDriver);
            Assert.AreEqual(expectedLog, actualLog);
            Assert.AreEqual(expectedSoftAssert, actualSoftAssert);
            Assert.AreEqual(expectedPerf, actualPerf);
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
            Assert.IsTrue(log.Contains("Navigate Forward: "));
            Assert.IsTrue(log.Contains("Navigate back: "));
        }

        /// <summary>
        /// Helper function to remove created custom file logs and return contained string
        /// </summary>
        /// <returns>string contained in custom log</returns>
        protected string GetAndRemoveCustomFileLog()
        {
            FileLogger outputLog = (FileLogger)Log;
            string log = File.ReadAllText(outputLog.FilePath);
            try
            {
                File.Delete(outputLog.FilePath);
                string screenShotLocation = outputLog.FilePath.Substring(0, outputLog.FilePath.LastIndexOf('.')) + ".Png";
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