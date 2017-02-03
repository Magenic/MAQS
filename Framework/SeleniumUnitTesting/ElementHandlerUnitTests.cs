//--------------------------------------------------
// <copyright file="ElementHandlerUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Unit tests for the ElementHandler class
    /// </summary>
    [TestClass]
    public class ElementHandlerUnitTests : BaseSeleniumTest
    {
        /// <summary>
        /// Url for the site
        /// </summary>
        private static string siteUrl = SeleniumConfig.GetWebSiteBase();

        /// <summary>
        /// Automation site url
        /// </summary>
        private static string siteAutomationUrl = siteUrl + "Automation/";

        /// <summary>
        /// Options for computer parts list
        /// </summary>
        private static By computerPartsListOptions = By.CssSelector("#computerParts > option");

        /// <summary>
        /// Swagger link
        /// </summary>
        private static By swaggerLinkBy = By.CssSelector("#SwaggerPageLink > a");

        /// <summary>
        /// First name textbox
        /// </summary>
        private static By firstNameTextBox = By.CssSelector("#TextFields > p:nth-child(1) > input[type=\"text\"]");

        /// <summary>
        /// Female radio button
        /// </summary>
        private static By femaleRadioButton = By.CssSelector("#FemaleRadio");

        /// <summary>
        /// First checkbox
        /// </summary>
        private static By checkbox = By.CssSelector("#Checkbox1");

        /// <summary>
        /// Name dropdown list
        /// </summary>
        private static By nameDropdown = By.CssSelector("#namesDropdown");

        /// <summary>
        /// Computer parts list
        /// </summary>
        private static By computerPartsList = By.CssSelector("#computerParts");

        /// <summary>
        /// Manage dropdown selector
        /// </summary>
        private static By manageDropdown = By.CssSelector("body > div.navbar.navbar-inverse.navbar-fixed-top > div > div.navbar-collapse.collapse > ul > li:nth-child(2) > a");

        /// <summary>
        /// Employee link
        /// </summary>
        private static By employeeButton = By.CssSelector("#EmployeeButton > a");

        /// <summary>
        /// Employee page title
        /// </summary>
        private static By employeePageTitle = By.CssSelector("body > div.container.body-content > h2");

        /// <summary>
        /// Show dialog button
        /// </summary>
        private static By showDialog = By.CssSelector("#showDialog1");

        /// <summary>
        /// Close show dialog box button
        /// </summary>
        private static By closeShowDialog = By.CssSelector("#CloseButtonShowDialog");

        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before we start running selenium tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running selenium tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Unit Test for creating a sorted comma delimited string
        /// </summary>
        #region SortFromWebElements
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CreateSortedCommaDelimitedStringFromWebElementsTest()
        {
            string expectedText = "Hard Drive, Keyboard, Monitor, Motherboard, Mouse, Power Supply";
            this.NavigateToUrl();
            Assert.AreEqual(expectedText, this.WebDriver.CreateCommaDelimitedString(computerPartsListOptions, true), "Expected string does not match actual");
        }
        #endregion

        /// <summary>
        /// Unit Test for creating a comma delimited string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CreateCommaDelimitedStringFromWebElementsTest()
        {
            string expectedText = "Motherboard, Power Supply, Hard Drive, Monitor, Mouse, Keyboard";
            this.NavigateToUrl();
            Assert.AreEqual(expectedText, this.WebDriver.CreateCommaDelimitedString(computerPartsListOptions), "Expected string does not match actual");
        }

        /// <summary>
        /// Unit test for entering text into a textbox and getting text from a textbox
        /// </summary>
        #region GetAttribute
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetTextBoxAndVerifyValueTest()
        {
            string expectedValue = "Tester";
            this.NavigateToUrl();
            this.WebDriver.SetTextBox(firstNameTextBox, expectedValue);
            string actualValue = this.WebDriver.GetElementAttribute(firstNameTextBox);
            VerifyText(actualValue, expectedValue);
        }
        #endregion

        /// <summary>
        /// Check that SetTextBox throws correct exception with an empty input string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception))]
        public void SetTextBoxThrowException()
        {
            this.NavigateToUrl();
            this.WebDriver.SetTextBox(firstNameTextBox, string.Empty);
        }

        /// <summary>
        /// Unit Test for checking a radio button
        /// </summary>
        #region ClickButton
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CheckRadioButtonTest()
        {
            this.NavigateToUrl();
            this.WebDriver.ClickButton(femaleRadioButton, false);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElement(femaleRadioButton).Selected, "Radio button was not selected");
        }
        #endregion

        /// <summary>
        /// Test ClickButton called with WaitForButtonToDisappear as true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ClickButtonWaitForButtonToDisappear()
        {
            this.NavigateToUrl();
            this.WebDriver.ClickButton(showDialog, false);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElement(closeShowDialog).Displayed, "Show Dialog box was not displayed");

            this.WebDriver.ClickButton(closeShowDialog, true);
            Assert.IsFalse(this.WebDriver.FindElement(closeShowDialog).Displayed, "Show Dialog box was not closed");
        }

        /// <summary>
        /// Unit Test for checking a checkbox
        /// </summary>
        #region CheckCheckbox
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CheckCheckBoxTest()
        {
            this.NavigateToUrl();
            this.WebDriver.CheckCheckBox(checkbox, true);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElement(checkbox).Selected, "Checkbox was not enabled");

            this.WebDriver.CheckCheckBox(checkbox, false);
            Assert.IsFalse(this.WebDriver.Wait().ForClickableElement(checkbox).Selected, "Checkbox was enabled");
        }
        #endregion

        /// <summary>
        /// Unit Test for get element attribute function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetElementAttributeTest()
        {
            string expectedText = "http://magenicautomation.azurewebsites.net/Swagger";
            this.NavigateToUrl();
            string actualText = this.WebDriver.GetElementAttribute(swaggerLinkBy, "href");
            VerifyText(actualText, expectedText);
        }

        /// <summary>
        /// Unit Test for selecting an item from a dropdown and getting the selected item from a dropdown (By actual value)
        /// </summary>
        #region SelectItemDropdown
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectItemFromDropDownTest()
        {
            string expectedSelection = "Emily";
            this.NavigateToUrl();
            this.WebDriver.SelectDropDownOption(nameDropdown, expectedSelection);
            string actualSelection = this.WebDriver.GetSelectedOptionFromDropdown(nameDropdown);
            VerifyText(actualSelection, expectedSelection);
        }
        #endregion

        /// <summary>
        /// Unit Test for selecting an item from a dropdown and getting the selected item from a dropdown (By list value)
        /// </summary>
        #region DropdownByValue
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectItemFromDropDownByValueTest()
        {
            string expectedSelection = "Jack";
            this.NavigateToUrl();
            this.WebDriver.SelectDropDownOptionByValue(nameDropdown, "two");
            string actualSelection = this.WebDriver.GetSelectedOptionFromDropdown(nameDropdown);
            VerifyText(actualSelection, expectedSelection);
        }
        #endregion

        /// <summary>
        /// Unit Test for selecting multiple items from a list box and getting all selected items in a list box(By actual value)
        /// </summary>
        #region SelectMultipleItems
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectMultipleItemsFromListBoxTest()
        {
            StringBuilder results = new StringBuilder();
            List<string> itemsToSelect = new List<string>();
            itemsToSelect.Add("Monitor");
            itemsToSelect.Add("Hard Drive");
            itemsToSelect.Add("Keyboard");
            this.NavigateToUrl();
            this.WebDriver.SelectMultipleElementsFromListBox(computerPartsList, itemsToSelect);
            List<string> selectedItems = this.WebDriver.GetSelectedOptionsFromDropdown(computerPartsList);
            ListProcessor.ListOfStringsComparer(itemsToSelect, selectedItems, results);
            if (results.Length > 0)
            {
                Assert.Fail(results.ToString());
            }
        }
        #endregion

        /// <summary>
        /// Unit Test for selecting multiple items from a list box and getting all selected items in a list box(By list value)
        /// </summary>
        #region SelectMultipleElementsByValue
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectMultipleItemsFromListBoxTestByValue()
        {
            List<string> itemsToSelect = new List<string>();
            itemsToSelect.Add("one");
            itemsToSelect.Add("four");
            itemsToSelect.Add("five");
            this.NavigateToUrl();
            this.WebDriver.SelectMultipleElementsFromListBoxByValue(computerPartsList, itemsToSelect);
            List<string> selectedItems = this.WebDriver.GetSelectedOptionsFromDropdown(computerPartsList);

            if (selectedItems.Count != 3)
            {
                Assert.Fail("Does not contain 3 elements: " + selectedItems.ToString());
            }
        }
        #endregion

        /// <summary>
        /// Unit test for ClickElementByJavaScript using a hover dropdown, where dropdown is not visible
        /// </summary>
        #region ClickByJavascript
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ClickElementByJavascriptFromHoverDropdown()
        {
            this.NavigateToUrl();
            this.WebDriver.ClickElementByJavaScript(employeeButton);
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.Wait().ForExactText(employeePageTitle, "Index");
        }
        #endregion

        /// <summary>
        /// Test to verify scrolling into view
        /// </summary>
        #region ScrollIntoView
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoView()
        {
            this.NavigateToUrl();
            this.WebDriver.ScrollIntoView(checkbox);
        }
        #endregion

        /// <summary>
        /// Test to verify scrolling into view
        /// </summary>
        #region ScrollIntoViewWithCoords
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewWithCoords()
        {
            this.NavigateToUrl();
            this.WebDriver.ScrollIntoView(checkbox, 50, 0);
        }
        #endregion

        /// <summary>
        /// Test to verify scrolling into view
        /// </summary>
        #region ExecuteScrolling
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ExecutingScrolling()
        {
            this.NavigateToUrl();
            this.WebDriver.ExecuteScrolling(50, 0);
        }
        #endregion

        /// <summary>
        /// Unit test for ClickElementByJavaScript where the element is not present
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NoSuchElementException), "A JavaScript click that should have failed inappropriately passed.")]
        public void ClickElementByJavascriptFromHoverDropdownNotFound()
        {
            this.NavigateToUrl();
            this.WebDriver.ClickElementByJavaScript(By.CssSelector(".NotPresent"));
        }

        /// <summary>
        /// Verify slow type text is correctly typed
        /// </summary>
        #region SlowType
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SlowTypeTest()
        {
            this.NavigateToUrl();
            this.WebDriver.SlowType(firstNameTextBox, "Test input slowtype");
            Assert.AreEqual("Test input slowtype", this.WebDriver.Wait().ForClickableElement(firstNameTextBox).GetAttribute("value"));
        }
        #endregion

        /// <summary>
        /// Verify two strings are equal. If not fail test
        /// </summary>
        /// <param name="actualValue">Actual displayed text</param>
        /// <param name="expectedValue">Expected text</param>
        private static void VerifyText(string actualValue, string expectedValue)
        {
            Assert.AreEqual(expectedValue, actualValue, "Values are not equal");
        }

        /// <summary>
        /// Navigate to test page url and wait for page to load
        /// </summary>
        private void NavigateToUrl()
        {
            this.WebDriver.Navigate().GoToUrl(siteAutomationUrl);
            this.WebDriver.Wait().ForPageLoad();
        }
    }
}
