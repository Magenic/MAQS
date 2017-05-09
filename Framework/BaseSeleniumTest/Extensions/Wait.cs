//--------------------------------------------------
// <copyright file="Wait.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Wait extension for web drivers and elements</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace Magenic.MaqsFramework.BaseSeleniumTest.Extensions
{
    /// <summary>
    /// Web driver wait extension methods
    /// </summary>
    public class Wait
    {
        /// <summary>
        /// The search context item
        /// </summary>
        private ISearchContext searchItem;

        /// <summary>
        /// The wait driver
        /// </summary>
        private WebDriverWait webDriverWait;

        /// <summary>
        /// Initializes a new instance of the <see cref="Wait"/> class.
        /// </summary>
        /// <param name="searchItem">The search context item</param>
        /// <param name="webDriverWait">The wait driver</param>
        internal Wait(ISearchContext searchItem, WebDriverWait webDriverWait)
        {
            this.searchItem = searchItem;
            this.webDriverWait = webDriverWait;
        }

        /// <summary>
        /// Wait for the element to be clickable
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForClickable" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForClickable" lang="C#" />
        /// </example>
        public IWebElement ForClickableElement(By by)
        {
            return this.webDriverWait.Until(ElementIsClickable(by, this.searchItem));
        }

        /// <summary>
        /// Wait for the element to be visible
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForVisible" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForVisible" lang="C#" />
        /// </example>
        public IWebElement ForVisibleElement(By by)
        {
            return this.webDriverWait.Until(ElementIsVisible(by, this.searchItem));
        }

        /// <summary>
        /// Wait for the element to exist
        /// </summary>
        /// <param name="by">Css Selector</param>
        /// <returns>The web element</returns>
        /// /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForExist" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForExist" lang="C#" />
        /// </example>
        public IWebElement ForElementExist(By by)
        {
            return this.webDriverWait.Until(ElementDoesExist(by, this.searchItem));
        }

        /// <summary>
        /// Wait for the element to have specific (case sensitive) text
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The exact text we expect the element have - Case sensitive</param>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForExactText" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForExactText" lang="C#" />
        /// </example>
        public IWebElement ForExactText(By by, string text)
        {
            return this.webDriverWait.Until(ElementHasExpectedText(by, text, this.searchItem));
        }

        /// <summary>
        /// Wait for the element to contain the expected (case insensitive) text
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The text we expect the element to contain - Case insensitive</param>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForContainsText" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForContainsText" lang="C#" />
        /// </example>
        public IWebElement ForContainsText(By by, string text)
        {
            return this.webDriverWait.Until(ElementContainsExpectedText(by, text, this.searchItem));
        }

        /// <summary>
        /// Wait for an element to not appear on the page - It can be gone or just not displayed
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForAbsentElement" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForAbsentElement" lang="C#" />
        /// </example>
        public void ForAbsentElement(By by)
        {
            if (!this.UntilAbsentElement(by))
            {
                throw new Exception(StringProcessor.SafeFormatter("The element '{0}' is still present.", by.ToString()));
            }
        }

        /// <summary>
        /// Wait for the page to load
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForPageLoad" lang="C#" />
        /// </example>
        public void ForPageLoad()
        {
            if (!this.UntilPageLoad())
            {
                throw new Exception("Page load took longer than timeout configuration");
            }
        }

        /// <summary>
        /// Wait for the element attributes to contain the correct text value
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="textValue">Text String expected within value of attribute</param>
        /// <param name="attribute">Attribute name as a String</param>
        /// <returns>Element if the attribute contained given string</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForAttributeContains" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForAttributeContains" lang="C#" />
        /// </example>
        public IWebElement ForAttributeTextContains(By by, string textValue, string attribute)
        {
            try
            {
                return this.webDriverWait.Until(AttributeContainsExpectedText(by, textValue, attribute, this.searchItem));
            }
            catch
            {
                throw new NotFoundException(StringProcessor.SafeFormatter("The element attribute {0} inside '{1}' with the value of {2} was not found", attribute, by.ToString(), textValue));
            }
        }

        /// <summary>
        /// Wait for the element attributes to equal the correct text value
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="textValue">Text String expected to equal value of attribute</param>
        /// <param name="attribute">Attribute name as a String</param>
        /// <returns>Element if the attribute equals given string</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForAttributeEquals" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForAttributeEquals" lang="C#" />
        /// </example>
        public IWebElement ForAttributeTextEquals(By by, string textValue, string attribute)
        {
            try
            {
                return this.webDriverWait.Until(AttributeEqualsExpectedText(by, textValue, attribute, this.searchItem));
            }
            catch
            {
                throw new NotFoundException(StringProcessor.SafeFormatter("The element attribute {0} inside '{1}' with the value of {2} was not found", attribute, by.ToString(), textValue));
            }
        }

        /// <summary>
        /// Wait for the element to be visible and scrolls into view
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForAndScroll" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForAndScroll" lang="C#" />
        /// </example>
        public IWebElement ForClickableElementAndScrollIntoView(By by)
        {
            IWebElement element = this.ForClickableElement(by);
            ElementHandler.ScrollIntoView(this.searchItem, by);
            return element;
        }

        /// <summary>
        /// Wait for the element to be visible and scrolls into view
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="x">Horizontal offset</param>
        /// <param name="y">Vertical offset</param>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitForClickableAndScroll" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitForClickableAndScroll" lang="C#" />
        /// </example>
        public IWebElement ForClickableElementAndScrollIntoView(By by, int x, int y)
        {
            IWebElement element = this.ForClickableElement(by);
            ElementHandler.ExecuteScrolling(element, x, y);
            return element;
        }

        /// <summary>
        /// Wait for the element to be clickable
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>True if the element is clickable</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilClickable" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilClickable" lang="C#" />
        /// </example>
        public bool UntilClickableElement(By by)
        {
            return this.DoWaitUntilCheck(ElementIsClickable, by, this.searchItem);
        }

        /// <summary>
        /// Wait for the element to be visible
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>True if the element is visible</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilVisible" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilVisible" lang="C#" />
        /// </example>
        public bool UntilVisibleElement(By by)
        {
            return this.DoWaitUntilCheck(ElementIsVisible, by, this.searchItem);
        }

        /// <summary>
        /// Wait for the element to exist
        /// </summary>
        /// <param name="by">Css Selector</param>
        /// <returns>True if the element exists</returns>
        /// /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilExist" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilExist" lang="C#" />
        /// </example>
        public bool UntilElementExist(By by)
        {
            return this.DoWaitUntilCheck(ElementDoesExist, by, this.searchItem);
        }

        /// <summary>
        /// Wait for the element to have specific (case sensitive) text
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The exact text we expect the element have - Case sensitive</param>
        /// <returns>True if the element has the exact text</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilExact" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilExact" lang="C#" />
        /// </example>
        public bool UntilExactText(By by, string text)
        {
            return this.DoWaitUntilCheck(ElementHasExpectedText, by, text, this.searchItem);
        }

        /// <summary>
        /// Wait for the element to contain the expected (case insensitive) text
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The text we expect the element to contain - Case insensitive</param>
        /// <returns>True if the element contains the text</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilContains" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilContains" lang="C#" />
        /// </example>
        public bool UntilContainsText(By by, string text)
        {
            return this.DoWaitUntilCheck(ElementContainsExpectedText, by, text, this.searchItem);
        }

        /// <summary>
        /// Wait for an element to not appear on the page - It can be gone or just not displayed
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>True if the element is not visible on the page</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilAbsent" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilAbsent" lang="C#" />
        /// 
        /// SeleniumUnitTests
        /// 
        /// </example>
        public bool UntilAbsentElement(By by)
        {
            DateTime start = DateTime.Now;

            do
            {
                // Give the system some time before checking if an element is missing
                Thread.Sleep(this.webDriverWait.PollingInterval);

                try
                {
                    if (!this.searchItem.FindElement(by).Displayed)
                    {
                        // The element on the page is present, but not visible
                        return true;
                    }
                }
                catch (NoSuchElementException)
                {
                    // The element is missing all together
                    return true;
                }
                catch (InvalidElementStateException)
                {
                    // The element is being cached but not present
                    return true;
                }
                catch
                {
                    // Could be several exceptions
                }
            }
            while ((DateTime.Now - start) < this.webDriverWait.Timeout);

            return false;
        }

        /// <summary>
        /// Wait for the page to load
        /// </summary>
        /// <returns>True if the page finished loading</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilPageLoad" lang="C#" />
        /// </example>
        public bool UntilPageLoad()
        {
            DateTime start = DateTime.Now;
            string source = string.Empty;
            IWebDriver driver = SeleniumUtilities.SearchContextToWebDriver(this.searchItem);

            do
            {
                // Give the system a second before checking if the page is updating
                Thread.Sleep(TimeSpan.FromSeconds(1));

                try
                {
                    // Find any element
                    driver.FindElement(By.CssSelector("*"));

                    // Get the page source
                    string newSource = driver.PageSource;

                    // Make sure the souce has not changed and it actully has content
                    if (!string.IsNullOrEmpty(source) && source.Equals(newSource))
                    {
                        return true;
                    }

                    source = newSource;
                }
                catch
                {
                    // Could be several exceptions - Don't really care as it may just be the page loading
                }
            }
            while ((DateTime.Now - start) < this.webDriverWait.Timeout);

            // Page was still loading
            return false;
        }

        /// <summary>
        /// Do a wait for on an attributes text contains
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">Text String expected within value of attribute</param>
        /// <param name="attribute">Attribute name as a String</param>
        /// <returns>Boolean of whether attribute contained given string</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilAttributeContains" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilAttributeContains" lang="C#" />
        /// </example>
        public bool UntilAttributeTextContains(By by, string text, string attribute)
        {
            return this.DoWaitUntilCheck(AttributeContainsExpectedText, by, text, attribute, this.searchItem);
        }

        /// <summary>
        /// Do a wait for on an attributes text equals
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">Text String expected to equal value of attribute</param>
        /// <param name="attribute">Attribute name as a String</param>
        /// <returns>Boolean of whether check passed</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilAttributeEquals" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilAttributeEquals" lang="C#" />
        /// </example>
        public bool UntilAttributeTextEquals(By by, string text, string attribute)
        {
            return this.DoWaitUntilCheck(AttributeEqualsExpectedText, by, text, attribute, this.searchItem);
        }

        /// <summary>
        /// Waits for the element to be visible and scrolls into view
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>True if the element is visible and scrolled into view</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilClickableAndScroll" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilClickableAndScroll" lang="C#" />
        /// </example>
        public bool UntilClickableElementAndScrollIntoView(By by)
        {
            if (this.UntilClickableElement(by))
            {
                ElementHandler.ScrollIntoView(this.searchItem, by);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Waits for the element to be visible and scrolls into view
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="x">Horizontal offset</param>
        /// <param name="y">Vertical offset</param>
        /// <returns>True if the element is visible and scrolled into view</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="WaitUntilClickableAndScrollWithOffset" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="WaitUntilClickableAndScrollWithOffset" lang="C#" />
        /// </example>
        public bool UntilClickableElementAndScrollIntoView(By by, int x, int y)
        {
            if (this.UntilClickableElement(by))
            {
                ElementHandler.ScrollIntoView(this.searchItem, by, x, y);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Wait for the page load and scroll by an offset of x and y
        /// </summary>
        /// <param name="x">Horizontal offset</param>
        /// <param name="y">Vertical offset</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="ExecuteScrolling" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/SeleniumWebElementTest.cs" region="ExecuteScrolling" lang="C#" />
        /// </example>
        public void PageLoadThanExecuteScrolling(int x, int y)
        {
            this.ForPageLoad();
            ElementHandler.ExecuteScrolling(this.searchItem, x, y);
        }

        /// <summary>
        /// Wait until check where we are checking text is within the value of attribute given
        /// </summary>
        /// <param name="by">'by' selector for the element</param>     
        /// <param name="textValue">String value that the elements attribute is expected to contain</param>
        /// <param name="attribute">Attribute name as a String</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Element if the check passed</returns>
        private static Func<IWebDriver, IWebElement> AttributeContainsExpectedText(By by, string textValue, string attribute, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                var elementValue = element.GetAttribute(attribute);
                return (elementValue != null && elementValue.ToUpper().Contains(textValue.ToUpper())) ? element : null;
            };
        }

        /// <summary>
        /// Do a wait until check where we are checking text equals value of attribute given
        /// </summary>
        /// <param name="by">'by' selector for the element</param>     
        /// <param name="textValue">String value that the element's attribute is expected to equal</param>
        /// <param name="attribute">Attribute name as a String</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Element if the check passed</returns>
        private static Func<IWebDriver, IWebElement> AttributeEqualsExpectedText(By by, string textValue, string attribute, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                var elementValue = element.GetAttribute(attribute);
                return (elementValue != null && elementValue.Equals(textValue)) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element is clickable
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Success if the element is clickable</returns>
        private static Func<ISearchContext, IWebElement> ElementIsClickable(By by, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element is visible
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Success if the element is visible</returns>
        private static Func<IWebDriver, IWebElement> ElementIsVisible(By by, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                return (element != null && element.Displayed) ? element : null;
            };
        }

        /// <summary>
        /// Checks if the element exists
        /// </summary>
        /// <param name="by">Css Selector</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Success if the element exists</returns>
        private static Func<IWebDriver, IWebElement> ElementDoesExist(By by, ISearchContext searchContext)
        {
            return driver =>
            {
                return searchContext.FindElement(by);
            };
        }

        /// <summary>
        /// Check if an element is enabled
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Success if the element is enabled</returns>
        private static Func<IWebDriver, IWebElement> ElementIsEnabled(By by, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                return (element != null && element.Enabled) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element has exactly the expected text - Case sensitive
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The expected text</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Success if the element has the expected text</returns>
        private static Func<IWebDriver, IWebElement> ElementHasExpectedText(By by, string text, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                ElementIsEnabled(by, searchContext);
                return (element != null && element.Displayed && element.Text.Equals(text)) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element contains the expected text - Case insensitive
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The expected text</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>Success if the element contains the expected text</returns>
        private static Func<IWebDriver, IWebElement> ElementContainsExpectedText(By by, string text, ISearchContext searchContext)
        {
            return driver =>
            {
                var element = searchContext.FindElement(by);
                ElementIsEnabled(by, searchContext);
                return (element != null && element.Displayed && element.Text.ToUpper().Contains(text.ToUpper())) ? element : null;
            };
        }

        /// <summary>
        /// Do a wait until check
        /// </summary>
        /// <param name="conditionCode">Function code to be executed by the block until successful or wait times out</param>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>True if the check passed</returns>
        private bool DoWaitUntilCheck(Func<By, ISearchContext, Func<IWebDriver, IWebElement>> conditionCode, By by, ISearchContext searchContext)
        {
            try
            {
                this.webDriverWait.Until(conditionCode(by, this.searchItem));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Do a wait until check where we are checking text
        /// </summary>
        /// <param name="conditionCode">The wait until check function</param>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">Text string to pass to the function given</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>True if the check passed</returns>
        private bool DoWaitUntilCheck(Func<By, string, ISearchContext, Func<IWebDriver, IWebElement>> conditionCode, By by, string text, ISearchContext searchContext)
        {
            try
            {
                this.webDriverWait.Until(conditionCode(by, text, this.searchItem));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Do a wait Until the element attributes to have the correct text value
        /// </summary>
        /// <param name="conditionCode">The wait until check function</param>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="textOne">First text string expected for function</param>
        /// <param name="textTwo">Second text string expected for function</param>
        /// <param name="searchContext">Search context  - either web driver or web element</param>
        /// <returns>True if check passed</returns>
        private bool DoWaitUntilCheck(Func<By, string, string, ISearchContext, Func<IWebDriver, IWebElement>> conditionCode, By by, string textOne, string textTwo, ISearchContext searchContext)
        {
            try
            {
                this.webDriverWait.Until(conditionCode(by, textOne, textTwo, this.searchItem));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
