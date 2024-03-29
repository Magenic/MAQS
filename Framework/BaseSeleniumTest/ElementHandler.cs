﻿//--------------------------------------------------
// <copyright file="ElementHandler.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseSeleniumTest
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
        public static string GetSelectedOptionFromDropdown(this ISearchContext searchContext, By by)
        {
            return GetSelectedOptionFromDropdown(searchContext.Wait().ForClickableElement(by));
        }

        /// <summary>
        /// Get selected option from dropdown
        /// </summary>
        /// <param name="element">Web element</param>
        /// <returns>Text of the selected option in drop down</returns>
        public static string GetSelectedOptionFromDropdown(this IWebElement element)
        {
            SelectElement select = new SelectElement(element);
            return select.SelectedOption.Text;
        }

        /// <summary>
        /// Get a list of the items selected in a drop down
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector</param>
        /// <returns>List<string> of selected items in dropdown</string></returns>
        public static List<string> GetSelectedOptionsFromDropdown(this ISearchContext searchContext, By by)
        {
            List<string> selectedItems = new List<string>();
            SelectElement select = new SelectElement(searchContext.Wait().ForClickableElement(by));

            // Get a list of IWebElement objects for all selected options from the dropdown
            List<IWebElement> elements = (List<IWebElement>)select.AllSelectedOptions;

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
        public static string GetElementAttribute(this ISearchContext searchContext, By by, string attribute = "value")
        {
            return GetElementAttribute(searchContext.Wait().ForVisibleElement(by), attribute);
        }

        /// <summary>
        /// Get the value of a specific attribute for an element
        /// </summary>
        /// <param name="element">Web element</param>
        /// <param name="attribute">The attribute to get the value for</param>
        /// <returns>The text in the textbox</returns>
        public static string GetElementAttribute(this IWebElement element, string attribute = "value")
        {
            return element.GetAttribute(attribute);
        }

        /// <summary>
        /// Check or Uncheck a checkbox NOTE: If checkbox is already in desired state, this method takes no action
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="check">True to check the checkbox. False to uncheck the checkbox</param>
        public static void CheckCheckBox(this ISearchContext searchContext, By by, bool check)
        {
            IWebElement element = searchContext.Wait().ForClickableElement(by);
            CheckCheckBox(element, check);
        }

        /// <summary>
        /// Check or Uncheck a checkbox NOTE: If checkbox is already in desired state, this method takes no action
        /// </summary>
        /// <param name="element">Web element</param>
        /// <param name="check">True to check the checkbox. False to uncheck the checkbox</param>
        public static void CheckCheckBox(this IWebElement element, bool check)
        {
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
        public static void SelectMultipleElementsFromListBox(this ISearchContext searchContext, By by, List<string> elementsTextToSelect)
        {
            SelectMultipleElementsFromListBox(searchContext.Wait().ForClickableElement(by), elementsTextToSelect);
        }

        /// <summary>
        /// Select multiple items from a list box
        /// </summary>
        /// <param name="element">Web element</param>
        /// <param name="elementsTextToSelect">List items as strings to select from list box</param>
        public static void SelectMultipleElementsFromListBox(this IWebElement element, List<string> elementsTextToSelect)
        {
            SelectElement selectItem = new SelectElement(element);

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
        public static void SelectDropDownOption(this ISearchContext searchContext, By by, string option)
        {
            SelectDropDownOption(searchContext.Wait().ForClickableElement(by), option);
        }

        /// <summary>
        /// Select an option from a drop down using displayed text
        /// </summary>
        /// <param name="element">The web element</param>
        /// <param name="option">The option to select in drop down</param>
        public static void SelectDropDownOption(this IWebElement element, string option)
        {
            SelectElement select = new SelectElement(element);
            select.SelectByText(option);
        }

        /// <summary>
        /// Select an option from a drop down using the value attribute
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="value">The value attribute for the option to select</param>
        public static void SelectDropDownOptionByValue(this ISearchContext searchContext, By by, string value)
        {
            SelectDropDownOptionByValue(searchContext.Wait().ForClickableElement(by), value);
        }

        /// <summary>
        /// Select an option from a drop down using the value attribute
        /// </summary>
        /// <param name="element">The web element</param>
        /// <param name="value">The value attribute for the option to select</param>
        public static void SelectDropDownOptionByValue(this IWebElement element, string value)
        {
            SelectElement select = new SelectElement(element);
            select.SelectByValue(value);
        }

        /// <summary>
        /// Enter text into a textbox. NOTE: This function clears the textbox before entering text
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector for the element</param>
        /// <param name="textToEnter">Text to enter into the textbox</param>
        /// <param name="tabOff">True to press the Tab key after entering text</param>
        public static void SetTextBox(this ISearchContext searchContext, By by, string textToEnter, bool tabOff = true)
        {
            SetTextBox(searchContext.Wait().ForVisibleElement(by), textToEnter, tabOff);
        }

        /// <summary>
        /// Enter text into a textbox. NOTE: This function clears the textbox before entering text
        /// </summary>
        /// <param name="element">Web element</param>
        /// <param name="textToEnter">Text to enter into the textbox</param>
        /// <param name="tabOff">True to press the Tab key after entering text</param>
        public static void SetTextBox(this IWebElement element, string textToEnter, bool tabOff = true)
        {
            if (textToEnter != string.Empty && textToEnter != null)
            {
                if (tabOff)
                {
                    textToEnter += Keys.Tab;
                }

                element.Clear();
                element.SendKeys(textToEnter);
            }
            else
            {
                throw new ArgumentException("String is either null or empty");
            }
        }

        /// <summary>
        /// Method to click an element via JavaScript
        /// Used for scenarios where normal click can't reach, such as hidden or hover triggered elements.
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">The By element to use</param>
        public static void ClickElementByJavaScript(this ISearchContext searchContext, By by)
        {
            IWebElement element = searchContext.FindElement(by);

            IJavaScriptExecutor executor = SeleniumUtilities.SearchContextToJavaScriptExecutor(searchContext);
            executor.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// Method to click an element via JavaScript
        /// Used for scenarios where normal click can't reach, such as hidden or hover triggered elements.
        /// </summary>
        /// <param name="element">Element to click</param>
        public static void ClickElementByJavaScript(this IWebElement element)
        {
            IJavaScriptExecutor executor = SeleniumUtilities.SearchContextToJavaScriptExecutor(element);
            executor.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="by">By selector</param>
        public static void ScrollIntoView(this ISearchContext searchContext, By by)
        {
            IWebElement element = searchContext.FindElement(by);
            ScrollIntoView(searchContext, element);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="element">IWebElement</param>
        public static void ScrollIntoView(this IWebElement element)
        {
            var driver = SeleniumUtilities.SearchContextToWebDriver(element);
            driver.ScrollIntoView(element);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="element">IWebElement</param>
        public static void ScrollIntoView(this ISearchContext searchContext, IWebElement element)
        {
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
        public static void ScrollIntoView(this ISearchContext searchContext, By by, int x, int y)
        {
            ScrollIntoView(searchContext, by);
            ExecuteScrolling(searchContext, x, y);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="element">Web element</param>
        /// <param name="x">Horizontal direction</param>
        /// <param name="y">Vertical direction</param>
        public static void ScrollIntoView(this IWebElement element, int x, int y)
        {
            var driver = SeleniumUtilities.SearchContextToWebDriver(element);
            ScrollIntoView(element);
            ExecuteScrolling(driver, x, y);
        }

        /// <summary>
        /// JavaScript method to scroll an element into the view
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="x">Horizontal direction</param>
        /// <param name="y">Vertical direction</param>
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
        public static void SlowType(this ISearchContext searchContext, By by, string textToEnter)
        {
            SlowType(searchContext.Wait().ForClickableElement(by), textToEnter);
        }

        /// <summary>
        /// Method to slowly type a string
        /// Used for scenarios where normal SendKeys types too quickly and causes issues with a website
        /// </summary>
        /// <param name="element">Web element</param>
        /// <param name="textToEnter">The string being entered into the text input field</param>
        public static void SlowType(this IWebElement element, string textToEnter)
        {
            foreach (char singleLetter in textToEnter)
            {
                element.SendKeys(singleLetter.ToString());
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
        public static void SendSecretKeys(this ISearchContext searchContext, By by, string textToEnter, ILogger logger)
        {
            SendSecretKeys(searchContext.Wait().ForClickableElement(by), textToEnter, logger);
        }

        /// <summary>
        /// Method used to send secret keys without logging
        /// </summary>
        /// <param name="secretElement">Web element</param>
        /// <param name="textToEnter">The string being entered into the text input field</param>
        /// <param name="logger">The Logging object</param>
        public static void SendSecretKeys(this IWebElement secretElement, string textToEnter, ILogger logger)
        {
            try
            {
                logger.SuspendLogging();
                secretElement.SendKeys(textToEnter);
                logger.ContinueLogging();
            }
            catch (Exception e)
            {
                logger.ContinueLogging();
                logger.LogMessage(MessageType.ERROR, $"Exception during sending secret keys: {e.Message}{ Environment.NewLine}{ e.StackTrace}");
                throw;
            }
        }
    }
}
