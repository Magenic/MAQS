//-----------------------------------------------------
// <copyright file="SeleniumWebElementTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test the selenium framework</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the selenium framework
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumWebElementTest : BaseSeleniumTest
    {
        /// <summary>
        /// Unit testing site URL - Login page
        /// </summary>
        private static string testSiteUrl = SeleniumConfig.GetWebSiteBase();

        /// <summary>
        /// Unit testing site URL - Async page
        /// </summary>
        private static string testSiteAsyncUrl = testSiteUrl + "Automation/AsyncPage";

        /// <summary>
        /// Unit testing site URL - Automation page
        /// </summary>
        private static string testSiteAutomationUrl = testSiteUrl + "Automation/";

        /// <summary>
        /// Unit testing site URL - About page
        /// </summary>
        private static string testSiteAboutUrl = testSiteUrl + "Home/Contact/";

        /// <summary> 
        /// Body css selector
        /// </summary> 
        private static By bodyCssSelector = By.CssSelector("BODY");

        /// <summary>
        /// Home button css selector
        /// </summary>
        private static By homeButtonCssSelector = By.CssSelector("#homeButton > a");

        /// <summary>
        /// Dropdown selector
        /// </summary>
        private static By asyncDropdownCssSelector = By.CssSelector("#Selector");

        /// <summary>
        /// Dropdown label
        /// </summary>
        private static By asyncOptionsLabel = By.CssSelector("#Label");

        /// <summary>
        /// Dropdown label - hidden once dropdown loads
        /// </summary>
        private static By asyncLoadingLabel = By.CssSelector("#LoadingLabel");

        /// <summary>
        /// Asynchronous div that loads after a delay on Async Testing Page
        /// </summary>
        private static By asyncLoadingTextDiv = By.CssSelector("#loading-div-text");

        /// <summary>
        /// Names label
        /// </summary>
        private static By automationNamesLabel = By.CssSelector("#Dropdown > p > strong > label");

        /// <summary>
        /// Selector that is not in page
        /// </summary>
        private static By notInPage = By.CssSelector("NOTINPAGE");

        /// <summary>
        /// First dialog button
        /// </summary>
        private static By automationShowDialog1 = By.CssSelector("#showDialog1");

        /// <summary>
        /// Text fields
        /// </summary>
        private static By textFields = By.CssSelector("#TextFields");

        /// <summary>
        /// Food table
        /// </summary>
        private static By foodTable = By.CssSelector("#FoodTable");

        /// <summary>
        /// Flower table
        /// </summary>
        private static By flowerTable = By.CssSelector("#FlowerTable TD");

        /// <summary>
        /// The Contact menu
        /// </summary>
        private static By contactMenu = By.CssSelector("#ContactButton");

        /// <summary>
        /// All links
        /// </summary>
        private static By allLinks = By.CssSelector("A");

        /// <summary>
        /// First child of a DIV
        /// </summary>
        private static By firstChildOfDiv = By.CssSelector("DIV :first-child");

        /// <summary>
        /// Asynchronous content DIV
        /// </summary>
        private static By asyncContent = By.CssSelector("#AsyncContent");

        /// <summary>
        /// All labels
        /// </summary>
        private static By allLabels = By.CssSelector("LABEL");

        /// <summary>
        /// Load content container
        /// </summary>
        private static By loadContainer = By.CssSelector("DIV.roundedcorners");

        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void OpenBrowser()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
        }

        /// <summary>
        /// Verify WaitForClickableElement wait works
        /// </summary>
        #region WaitForClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(contactMenu);
            element = element.Wait().ForClickableElement(allLinks);
            Assert.AreEqual("Contact", element.Text);
        }
        #endregion

        /// <summary>
        /// Verify WaitForElementExist wait works
        /// </summary>
        #region WaitForExist
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(contactMenu);
            element = element.Wait().ForElementExist(allLinks);
            Assert.AreEqual("Contact", element.Text);
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
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(contactMenu);
            element = element.Wait().ForVisibleElement(allLinks);
            Assert.AreEqual("Contact", element.Text);
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
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(asyncContent);
            element = element.Wait().ForExactText(allLabels, "Options");
            Assert.AreEqual("Options", element.Text);
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
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(asyncContent);
            element = element.Wait().ForContainsText(allLabels, "ption");
            Assert.AreEqual("Options", element.Text);
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
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(loadContainer);

            // Make sure the element was displayed
            element.Wait().ForElementExist(firstChildOfDiv);

            // Make sure the element went away
            element.Wait().ForAbsentElement(firstChildOfDiv);
        }
        #endregion

        /// <summary>
        /// Verify WaitForAbsentElement wait fails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception))]
        public void WaitForAbsentElementFail()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            element.Wait().ForAbsentElement(homeButtonCssSelector);
        }

        /// <summary>
        /// Verify WaitForAbsentElement wait fails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitOverrideRespected()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(500), TimeSpan.FromMilliseconds(10)));
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);

            DateTime start = DateTime.Now;
            element.Wait().UntilAbsentElement(homeButtonCssSelector);
            TimeSpan span = DateTime.Now - start;

            Assert.IsTrue(span.TotalMilliseconds > 500, "Timed out too quickly");
            Assert.IsTrue(span.TotalMilliseconds < 1000, "Didn't timeout quickly enough");
        }

        /// <summary>
        /// Verify WaitUntilClickableElement wait works
        /// </summary>
        #region WaitUntilClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilClickableElement(automationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilElementExist wait works
        /// </summary>
        #region WaitUntilExist
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilElementExist(automationShowDialog1), "Failed to find element");
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilVisibleElement(automationShowDialog1), "Failed to find element");
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
            this.TestObject.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilExactText(automationShowDialog1, "Show dialog"), "Failed to find element");
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilContainsText(automationShowDialog1, "dialog"), "Failed to find element");
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
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilAbsentElement(notInPage));
        }
        #endregion

        /// <summary>
        /// Verify that WaitForAttributeTextContains throws an exception for instances where the attribute is not found to contain a value.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string within an elements attribute passed.")]
        public void WaitForAttributeContainsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            element.Wait().ForAttributeTextContains(foodTable, "Flower Table", "Summmary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextEquals throws an exception for instances where the attribute is not found.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string equal to an elements attribute passed.")]
        public void WaitForAttributeEqualsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            element.Wait().ForAttributeTextEquals(foodTable, "Flower Table", "Summmary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextContains can find text within attribute after waiting.
        /// </summary>
        #region WaitForAttributeContains
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeContainsFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsNotNull(element.Wait().ForAttributeTextContains(asyncLoadingTextDiv, "block;", "style"));
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
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsNotNull(element.Wait().ForAttributeTextEquals(asyncLoadingTextDiv, "display: block;", "style"));
        }
        #endregion

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeContains()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilAttributeTextContains(asyncLoadingLabel, "none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextEquals works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEquals()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().UntilAttributeTextEquals(asyncLoadingLabel, "display: none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this text inside attribute value within timeout.
        /// </summary>
        #region WaitUntilAttributeContains
        [TestCategory(TestCategories.Selenium)]
        [TestMethod]
        public void WaitUntilAttributeContainsFalse()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsFalse(element.Wait().UntilAttributeTextContains(asyncDropdownCssSelector, "nottherightid", "id"));
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
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsFalse(element.Wait().UntilAttributeTextEquals(asyncLoadingLabel, "display:", "style"));
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);
            Assert.IsTrue(element.Wait().ForClickableElementAndScrollIntoView(automationShowDialog1).Displayed, "Failed to find element");
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);

            Assert.IsTrue(element.Wait().ForClickableElementAndScrollIntoView(automationShowDialog1, 0, 100).Displayed, "Failed to find element or scroll");
            Assert.IsTrue(WebDriver.Wait().ForClickableElementAndScrollIntoView(automationShowDialog1, 0, 100).Displayed, "Failed to find element or scroll");  
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);

            Assert.IsTrue(element.Wait().UntilClickableElementAndScrollIntoView(automationShowDialog1), "Failed to find element");
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);

            Assert.IsTrue(element.Wait().UntilClickableElementAndScrollIntoView(automationShowDialog1, 0, 100), "Failed to find element");
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
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(bodyCssSelector);

            element.Wait().PageLoadThanExecuteScrolling(0, 500);
        }
        #endregion
    }
}