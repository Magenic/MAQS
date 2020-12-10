//--------------------------------------------------
// <copyright file="ElementHandlerUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Unit tests for the ElementHandler class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ElementHandlerUnitTests : SauceLabsBaseSeleniumTest
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
        /// Unit Test for creating a sorted comma delimited string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CreateSortedCommaDelimitedStringFromWebElementsTest()
        {
            string expectedText = "Hard Drive, Keyboard, Monitor, Motherboard, Mouse, Power Supply";
            NavigateToUrl();
            Assert.AreEqual(expectedText, WebDriver.CreateCommaDelimitedString(computerPartsListOptions, true), "Expected string does not match actual");
        }

        /// <summary>
        /// Unit Test for creating a comma delimited string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CreateCommaDelimitedStringFromWebElementsTest()
        {
            string expectedText = "Motherboard, Power Supply, Hard Drive, Monitor, Mouse, Keyboard";
            NavigateToUrl();
            Assert.AreEqual(expectedText, WebDriver.CreateCommaDelimitedString(computerPartsListOptions), "Expected string does not match actual");
        }

        /// <summary>
        /// Unit test for entering text into a textbox and getting text from a textbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetTextBoxAndVerifyValueTest()
        {
            string expectedValue = "Tester";
            NavigateToUrl();
            WebDriver.SetTextBox(firstNameTextBox, expectedValue);
            string actualValue = WebDriver.GetElementAttribute(firstNameTextBox);
            VerifyText(actualValue, expectedValue);
        }

        /// <summary>
        /// Check that SetTextBox throws correct exception with an empty input string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(ArgumentException))]
        public void SetTextBoxThrowException()
        {
            NavigateToUrl();
            WebDriver.SetTextBox(firstNameTextBox, string.Empty);
        }

        /// <summary>
        /// Unit Test for checking a radio button
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CheckRadioButtonTest()
        {
            NavigateToUrl();
            WebDriver.ClickButton(femaleRadioButton, false);
            Assert.IsTrue(WebDriver.Wait().ForClickableElement(femaleRadioButton).Selected, "Radio button was not selected");
        }

        /// <summary>
        /// Test ClickButton called with WaitForButtonToDisappear as true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ClickButtonWaitForButtonToDisappear()
        {
            NavigateToUrl();
            WebDriver.ClickButton(showDialog, false);
            Assert.IsTrue(WebDriver.Wait().ForClickableElement(closeShowDialog).Displayed, "Show Dialog box was not displayed");

            WebDriver.ClickButton(closeShowDialog, true);
            Assert.IsFalse(WebDriver.FindElement(closeShowDialog).Displayed, "Show Dialog box was not closed");
        }

        /// <summary>
        /// Unit Test for checking a checkbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CheckCheckBoxTest()
        {
            NavigateToUrl();
            WebDriver.CheckCheckBox(checkbox, true);
            Assert.IsTrue(WebDriver.Wait().ForClickableElement(checkbox).Selected, "Checkbox was not enabled");

            WebDriver.CheckCheckBox(checkbox, false);
            Assert.IsFalse(WebDriver.Wait().ForClickableElement(checkbox).Selected, "Checkbox was enabled");
        }

        /// <summary>
        /// Unit Test for get element attribute function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetElementAttributeTest()
        {
            string expectedText = "https://magenicautomation.azurewebsites.net/Swagger";
            NavigateToUrl();
            string actualText = WebDriver.GetElementAttribute(swaggerLinkBy, "href");
            VerifyText(actualText, expectedText);
        }

        /// <summary>
        /// Unit Test for selecting an item from a dropdown and getting the selected item from a dropdown (By actual value)
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectItemFromDropDownTest()
        {
            string expectedSelection = "Emily";
            NavigateToUrl();
            WebDriver.SelectDropDownOption(nameDropdown, expectedSelection);
            string actualSelection = WebDriver.GetSelectedOptionFromDropdown(nameDropdown);
            VerifyText(actualSelection, expectedSelection);
        }

        /// <summary>
        /// Unit Test for selecting an item from a dropdown and getting the selected item from a dropdown (By list value)
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectItemFromDropDownByValueTest()
        {
            string expectedSelection = "Jack";
            NavigateToUrl();
            WebDriver.SelectDropDownOptionByValue(nameDropdown, "two");
            string actualSelection = WebDriver.GetSelectedOptionFromDropdown(nameDropdown);
            VerifyText(actualSelection, expectedSelection);
        }

        /// <summary>
        /// Unit Test for selecting multiple items from a list box and getting all selected items in a list box(By actual value)
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectMultipleItemsFromListBoxTest()
        {
            StringBuilder results = new StringBuilder();

            List<string> itemsToSelect = new List<string>
            {
                "Monitor",
                "Hard Drive",
                "Keyboard"
            };

            NavigateToUrl();
            WebDriver.SelectMultipleElementsFromListBox(computerPartsList, itemsToSelect);
            List<string> selectedItems = WebDriver.GetSelectedOptionsFromDropdown(computerPartsList);
            ListProcessor.ListOfStringsComparer(itemsToSelect, selectedItems, results);
            if (results.Length > 0)
            {
                Assert.Fail(results.ToString());
            }
        }

        /// <summary>
        /// Unit Test for selecting multiple items from a list box and getting all selected items in a list box(By list value)
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SelectMultipleItemsFromListBoxTestByValue()
        {
            List<string> itemsToSelect = new List<string>
            {
                "one",
                "four",
                "five"
            };

            NavigateToUrl();
            WebDriver.SelectMultipleElementsFromListBoxByValue(computerPartsList, itemsToSelect);
            List<string> selectedItems = WebDriver.GetSelectedOptionsFromDropdown(computerPartsList);

            if (selectedItems.Count != 3)
            {
                Assert.Fail("Does not contain 3 elements: " + selectedItems.ToString());
            }
        }

        /// <summary>
        /// Unit test for ClickElementByJavaScript using a hover dropdown, where dropdown is not visible
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ClickElementByJavascriptFromHoverDropdown()
        {
            NavigateToUrl();
            WebDriver.ClickElementByJavaScript(employeeButton);
            WebDriver.Wait().ForPageLoad();
            WebDriver.Wait().ForExactText(employeePageTitle, "Index");
        }

        /// <summary>
        /// Test to verify scrolling into view
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoView()
        {
            NavigateToUrl();
            WebDriver.ScrollIntoView(checkbox);
        }

        /// <summary>
        /// Test to verify scrolling into view via web element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewWebElement()
        {
            NavigateToUrl();
            var element = this.WebDriver.FindElement(checkbox);
            WebDriver.ScrollIntoView(element);
        }

        /// <summary>
        /// Test to verify scrolling into view
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewWithCoords()
        {
            NavigateToUrl();
            WebDriver.ScrollIntoView(checkbox, 50, 0);
        }

        /// <summary>
        /// Test to verify scrolling into view
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ExecutingScrolling()
        {
            NavigateToUrl();
            WebDriver.ExecuteScrolling(50, 0);
        }

        /// <summary>
        /// Unit test for ClickElementByJavaScript where the element is not present
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NoSuchElementException), "A JavaScript click that should have failed inappropriately passed.")]
        public void ClickElementByJavascriptFromHoverDropdownNotFound()
        {
            NavigateToUrl();
            WebDriver.ClickElementByJavaScript(By.CssSelector(".NotPresent"));
        }

        /// <summary>
        /// Verify slow type text is correctly typed
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SlowTypeTest()
        {
            NavigateToUrl();
            WebDriver.SlowType(firstNameTextBox, "Test input slowtype");
            Assert.AreEqual("Test input slowtype", WebDriver.Wait().ForClickableElement(firstNameTextBox).GetAttribute("value"));
        }

        /// <summary>
        /// Verify Send Secret Keys suspends logging
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SendSecretTextSuspendLoggingTest()
        {
            NavigateToUrl();
            WebDriver.FindElement(firstNameTextBox).SendKeys("somethingTest");
            WebDriver.FindElement(firstNameTextBox).Clear();
            WebDriver.SendSecretKeys(firstNameTextBox, "secretKeys", Log);

            FileLogger logger = (FileLogger)TestObject.Log;
            string filepath = logger.FilePath;

            Assert.IsFalse(File.ReadAllText(filepath).Contains("secretKeys"));
            File.Delete(filepath);
        }

        /// <summary>
        /// Verify that logging gets turned back on after secret key throws an error
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SendSecretTextEnablesLoggingAfterError()
        {
            string checkLogged = "THISSHOULDBELOGGED";
            NavigateToUrl();
            Assert.ThrowsException<ArgumentNullException>(() => this.WebDriver.SendSecretKeys(firstNameTextBox, null, this.Log));
            this.Log.LogMessage(checkLogged);

            FileLogger logger = (FileLogger)TestObject.Log;
            string filepath = logger.FilePath;

            Assert.IsTrue(File.ReadAllText(filepath).Contains(checkLogged));
            File.Delete(filepath);
        }

        /// <summary>
        /// Verify Send Secret Keys re-enables after suspending logging
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SendSecretTextContinueLoggingTest()
        {
            NavigateToUrl();
            WebDriver.SendSecretKeys(firstNameTextBox, "secretKeys", Log);
            WebDriver.FindElement(firstNameTextBox).Clear();
            WebDriver.FindElement(firstNameTextBox).SendKeys("somethingTest");

            FileLogger logger = (FileLogger)TestObject.Log;
            string filepath = logger.FilePath;

            Assert.IsTrue(File.ReadAllText(filepath).Contains("somethingTest"));
            File.Delete(filepath);
        }

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
            WebDriver.Navigate().GoToUrl(siteAutomationUrl);
            WebDriver.Wait().ForPageLoad();
        }
    }
}
