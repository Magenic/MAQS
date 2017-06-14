//--------------------------------------------------
// <copyright file="ElementHandler.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Contains functions for interacting with IWebElement objects
    /// </summary>
    public static class ElementHandler
    {
        /// <summary>
        /// Get selected option from dropdown
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <returns>Text of the selected option in drop down</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SelectItemDropdown" lang="C#" />
        /// </example>
        public static string GetSelectedOptionFromDropdown(this ISearchContext searchContext, By by)
        {
            SelectElement select = new SelectElement(searchContext.Wait().ForClickableElement(by));
            return select.SelectedOption.Text;
        }

        /// <summary>
        /// Get a list of the items selected in a drop down
        /// </summary>
         /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector</param>
        /// <returns>List<string> of selected items in dropdown</string></returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SelectMultipleItems" lang="C#" />
        /// </example>
        public static List<string> GetSelectedOptionsFromDropdown(this ISearchContext searchContext, By by)
        {
            List<IWebElement> elements = null;
            List<string> selectedItems = new List<string>();
            SelectElement select = new SelectElement(searchContext.Wait().ForClickableElement(by));

            // Get a list of IWebElement objects for all selected options from the dropdown
            elements = (List<IWebElement>)select.AllSelectedOptions;

            // Add the text of each element that is not null to the selectedItems list
            foreach (IWebElement element in elements)
            {
                if (element != null)
                {
                    selectedItems.Add(element.Text);
                }
            }

            return selectedItems;
        }

        /// <summary>
        /// Get the value of a specific attribute for an element
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="attribute">The attribute to get the value for</param>
        /// <returns>The text in the textbox</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="GetAttribute" lang="C#" />
        /// </example>
        public static string GetElementAttribute(this ISearchContext searchContext, By by, string attribute = "value")
        {
            return searchContext.Wait().ForVisibleElement(by).GetAttribute(attribute);
        }

        /// <summary>
        /// Check or Uncheck a checkbox NOTE: If checkbox is already in desired state, this method takes no action
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="check">True to check the checkbox. False to uncheck the checkbox</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="CheckCheckbox" lang="C#" />
        /// </example>
        public static void CheckCheckBox(this ISearchContext searchContext, By by, bool check)
        {
            IWebElement element = searchContext.Wait().ForClickableElement(by);

            if (check && !element.Selected)
            {
                element.Click();
            }
            else if (!check && element.Selected)
            {
                element.Click();
            }
        }

        /// <summary>
        /// Create a comma delimited string from a list of IWebElement objects
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the elements</param>
        /// <param name="sort">True to create an alphabetically sorted comma delimited string</param>
        /// <returns>Returns a comma delimited string</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SortFromWebElements" lang="C#" />
        /// </example>
        public static string CreateCommaDelimitedString(this ISearchContext searchContext, By by, bool sort = false)
        {
            List<string> unsortedList = new List<string>();

            foreach (IWebElement element in searchContext.FindElements(by))
            {
                unsortedList.Add(element.Text.Trim());
            }

            return ListProcessor.CreateCommaDelimitedString(unsortedList, sort);
        }

        /// <summary>
        /// Clicks an element
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="waitForButtonToDisappear">If True, wait for element to disappear. If False, Do not wait</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="ClickButton" lang="C#" />
        /// </example>
        public static void ClickButton(this ISearchContext searchContext, By by, bool waitForButtonToDisappear)
        {
            searchContext.Wait().ForClickableElement(by).Click();

            if (waitForButtonToDisappear)
            {
                searchContext.Wait().ForAbsentElement(by);
            }
        }

        /// <summary>
        /// Select multiple items from a list box
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="elementsTextToSelect">List items as strings to select from list box</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SelectMultipleItems" lang="C#" />
        /// </example>
        public static void SelectMultipleElementsFromListBox(this ISearchContext searchContext, By by, List<string> elementsTextToSelect)
        {
            SelectElement selectItem = new SelectElement(searchContext.Wait().ForClickableElement(by));

            // Select all desired items in the listbox
            foreach (string text in elementsTextToSelect)
            {
                selectItem.SelectByText(text);
            }
        }

        /// <summary>
        /// Select multiple items from a list box
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="values">List items as strings to select from list box</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SelectMultipleElementsByValue" lang="C#" />
        /// </example>
        public static void SelectMultipleElementsFromListBoxByValue(this ISearchContext searchContext, By by, List<string> values)
        {
            SelectElement selectItem = new SelectElement(searchContext.Wait().ForClickableElement(by));

            // Select all desired items in the listbox
            foreach (string value in values)
            {
                selectItem.SelectByValue(value);
            }
        }

        /// <summary>
        /// Select an option from a drop down using displayed text
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="option">The option to select in drop down</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SelectItemDropdown" lang="C#" />
        /// </example>
        public static void SelectDropDownOption(this ISearchContext searchContext, By by, string option)
        {
            SelectElement select = new SelectElement(searchContext.Wait().ForClickableElement(by));
            select.SelectByText(option);
        }

        /// <summary>
        /// Select an option from a drop down using the value attribute
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="value">The value attribute for the option to select</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="DropdownByValue" lang="C#" />
        /// </example>
        public static void SelectDropDownOptionByValue(this ISearchContext searchContext, By by, string value)
        {
            SelectElement select = new SelectElement(searchContext.Wait().ForClickableElement(by));
            select.SelectByValue(value);
        }

        /// <summary>
        /// Enter text into a textbox. NOTE: This function clears the textbox before entering text
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="textToEnter">Text to enter into the textbox</param>
        /// <param name="tabOff">True to press the Tab key after entering text</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="GetAttribute" lang="C#" />
        /// </example>
        public static void SetTextBox(this ISearchContext searchContext, By by, string textToEnter, bool tabOff = true)
        {
            if (textToEnter != string.Empty && textToEnter != null)
            {
                if (tabOff)
                {
                    textToEnter += Keys.Tab;
                }

                IWebElement element = searchContext.Wait().ForVisibleElement(by);
                element.Clear();
                element.SendKeys(textToEnter);
            }
            else
            {
                throw new Exception("String is either null or empty");
            }
        }

        /// <summary>
        /// Method to click an element via JavaScript
        /// Used for scenarios where normal click can't reach, such as hidden or hover triggered elements.
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">The By element to use</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="ClickByJavascript" lang="C#" />
        /// </example>
        public static void ClickElementByJavaScript(this ISearchContext searchContext, By by)
        {
            IWebElement element = searchContext.FindElement(by);

            IJavaScriptExecutor executor = SeleniumUtilities.SearchContextToJavaScriptExecutor(searchContext);
            executor.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="ScrollIntoView" lang="C#" />
        /// </example>
        public static void ScrollIntoView(this ISearchContext searchContext, By by)
        {
            IWebElement element = searchContext.FindElement(by);

            IJavaScriptExecutor executor = SeleniumUtilities.SearchContextToJavaScriptExecutor(searchContext);
            executor.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By element</param>
        /// <param name="x">Horizontal direction</param>
        /// <param name="y">Vertical direction</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="ScrollIntoViewWithCoords" lang="C#" />
        /// </example>
        public static void ScrollIntoView(this ISearchContext searchContext, By by, int x, int y)
        {
            ScrollIntoView(searchContext, by);
            ExecuteScrolling(searchContext, x, y);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="x">Horizontal direction</param>
        /// <param name="y">Vertical direction</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="ExecuteScrolling" lang="C#" />
        /// </example>
        public static void ExecuteScrolling(this ISearchContext searchContext, int x, int y)
        {
            string scrollCommand = string.Format("scroll({0}, {1});", x, y);
            IJavaScriptExecutor executor = SeleniumUtilities.SearchContextToJavaScriptExecutor(searchContext);

            executor.ExecuteScript(scrollCommand);
        }

        /// <summary>
        /// Method to slowly type a string
        /// Used for scenarios where normal SendKeys types too quickly and causes issues with a website
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector</param>
        /// <param name="textToEnter">The string being entered into the text input field</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SlowType" lang="C#" />
        /// </example>
        public static void SlowType(this ISearchContext searchContext, By by, string textToEnter)
        {
            foreach (char singleLetter in textToEnter.ToCharArray())
            {
                searchContext.Wait().ForClickableElement(by).SendKeys(singleLetter.ToString());
                System.Threading.Thread.Sleep(500);
            }
        }

        /// <summary>
        /// Method used to send secret keys without logging
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector</param>
        /// <param name="textToEnter">The string being entered into the text input field</param>
        /// <param name="logger">The Logging object</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ElementHandlerUnitTests.cs" region="SendSecretKeys" lang="C#" />
        /// </example>
        public static void SendSecretKeys(this ISearchContext searchContext, By by, string textToEnter, Logger logger)
        {
            try
            {
                IWebElement secretElement = searchContext.Wait().ForClickableElement(by);
                logger.SuspendLogging();
                secretElement.SendKeys(textToEnter);
                logger.ContinueLogging();
            }
            catch (Exception e)
            {
                logger.ContinueLogging();
                logger.LogMessage(MessageType.ERROR, "An error occured: " + e);
                throw new Exception("Exception durring sending secret keys: " + e.Message);
            }
        }
    }
}
