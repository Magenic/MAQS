//--------------------------------------------------
// <copyright file="AbstractLazyIWebElement.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>This is the abstract LazyElement class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Magenic.Maqs.BaseSeleniumTest.Extensions
{
    /// <summary>
    /// Abstract structure for dynamically finding and interacting with elements
    /// </summary>
    public abstract class AbstractLazyIWebElement : IWebElement
    {
        /// <summary>
        /// The index in cases where the selector finds multiple elements
        /// </summary>
        private readonly int? elementIndex;

        /// <summary>
        /// A user friendly name, for logging purposes
        /// </summary>
        private readonly string userFriendlyName;

        /// <summary>
        /// The parent lazy element
        /// </summary>
        private readonly AbstractLazyIWebElement parent;

        /// <summary>
        /// Get the timeout
        /// </summary>
        /// <returns>The timeout</returns>
        protected TimeSpan TimeoutTime()
        {
            return this.WaitDriver().Timeout;
        }

        /// <summary>
        /// Get the wait time
        /// </summary>
        /// <returns>The wait time</returns>
        protected TimeSpan WaitTime()
        {
            return this.WaitDriver().PollingInterval;
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="AbstractLazyIWebElement" /> class
        /// </summary>
        /// <param name="testObject">The base test object</param>
        /// <param name="webDriver">The assoicated web driver</param>
        /// <param name="waitDriver">The assoicated web driver wait</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        protected AbstractLazyIWebElement(BaseTestObject testObject, IWebDriver webDriver, Func<WebDriverWait> waitDriver, By locator, [CallerMemberName] string userFriendlyName = null)
        {
            this.TestObject = testObject;
            this.WebDriver = webDriver;
            this.WaitDriver = waitDriver;
            this.Log = testObject.Log;
            this.By = locator;
            this.userFriendlyName = userFriendlyName;
            Extend.SetWaitDriver(this.WebDriver, this.WaitDriver());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLazyIWebElement" /> class
        /// </summary>
        /// <param name="parent">The parent lazy element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        protected AbstractLazyIWebElement(AbstractLazyIWebElement parent, By locator, [CallerMemberName] string userFriendlyName = null) : this(parent.TestObject, parent.WebDriver, parent.WaitDriver, locator, userFriendlyName)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractLazyIWebElement" /> class
        /// </summary>
        /// <param name="parent">The parent lazy element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="cachedElement">The cached web element</param>
        /// <param name="index">The index of the element - Used if the by finds multiple elements</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        protected AbstractLazyIWebElement(AbstractLazyIWebElement parent, By locator, IWebElement cachedElement, int index, [CallerMemberName] string userFriendlyName = null) : this(parent.TestObject, parent.WebDriver, parent.WaitDriver, locator, userFriendlyName)
        {
            this.parent = parent;
            this.CachedElement = cachedElement;
            this.elementIndex = index;
        }

        /// <summary>
        /// Gets a the 'by' selector for the element
        /// </summary>
        public By By { get; private set; }

        /// <summary>
        /// Gets the test object for the element
        /// </summary>
        public BaseTestObject TestObject { get; private set; }

        /// <summary>
        /// Gets the web driver
        /// </summary>
        public IWebDriver WebDriver { get; private set; }

        /// <summary>
        /// Gets the wait driver function
        /// </summary>
        public Func<WebDriverWait> WaitDriver { get; private set; }

        /// <summary>
        /// Gets the logger
        /// </summary>
        public Logger Log { get; private set; }

        /// <summary>
        /// Gets a cached copy of the element or null if we haven't already found the element
        /// </summary>
        public IWebElement CachedElement { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the lazy element is enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Check to see if the lazy element '{this.userFriendlyName}' is enabled");

                return GenericWait.Wait<bool>(
                    () =>
                {
                    return this.GetElement(this.GetRawExistingElement).Enabled;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lazy element is selected
        /// </summary>
        public bool Selected
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Checking to see if the lazy element '{this.userFriendlyName}' is selected");

                return GenericWait.Wait<bool>(
                    () =>
                {
                    return this.GetElement(this.GetRawExistingElement).Selected;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lazy element is displayed
        /// </summary>
        public bool Displayed
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Checking to see if the lazy element '{this.userFriendlyName}' is displayed");

                return GenericWait.Wait<bool>(
                    () =>
                {
                    return this.GetElement(this.GetRawExistingElement).Displayed;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lazy element exists
        /// </summary>
        public bool Exists
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Checking to see if the lazy element '{this.userFriendlyName}' exists");

                return GenericWait.Wait(
                    () =>
                {
                    this.GetElement(this.GetRawExistingElement);
                    return true;
                },
                this.WaitTime(),
                this.TimeoutTime(), false);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lazy element is displayed
        /// </summary>
        public bool ExistsNow
        {
            get
            {
                WebDriverWait waiter = this.WebDriver.GetWaitDriver();

                try
                {
                    this.WebDriver.SetWaitDriver(new WebDriverWait(this.WebDriver, TimeSpan.Zero));
                    this.Log.LogMessage(MessageType.INFORMATION, $"Checking to see if the lazy element '{this.userFriendlyName}' exists now");
                    return this.GetElement(this.GetRawExistingElement) != null;
                }
                catch
                {
                    this.Log.LogMessage(MessageType.INFORMATION, $"Lazy element does not '{this.userFriendlyName}' exist now");
                    return false;
                }
                finally
                {
                    this.WebDriver.SetWaitDriver(waiter);
                }
            }
        }

        /// <summary>
        /// Gets the lazy element's tag name
        /// </summary>
        public string TagName
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Getting the tag name for lazy element '{this.userFriendlyName}'");

                return GenericWait.Wait<string>(
                    () =>
                {
                    return this.GetElement(this.GetRawExistingElement).TagName;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Gets the lazy element's text
        /// </summary>
        public string Text
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Getting the text for lazy element '{this.userFriendlyName}'");

                return GenericWait.Wait(
                    () =>
                {
                    return this.GetElement(this.GetRawExistingElement).Text;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Gets the lazy element's location
        /// </summary>
        public Point Location
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Getting the location for lazy element '{this.userFriendlyName}'");

                return GenericWait.Wait(
                    () =>
                {
                    return this.GetElement(this.GetRawVisibleElement).Location;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Gets the lazy element's size
        /// </summary>
        public Size Size
        {
            get
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Getting the size of lazy element '{this.userFriendlyName}'");

                return GenericWait.Wait(
                    () =>
                {
                    return this.GetElement(this.GetRawVisibleElement).Size;
                },
                this.WaitTime(),
                this.TimeoutTime());
            }
        }

        /// <summary>
        /// Click the lazy element
        /// </summary>
        public void Click()
        {
            this.Log.LogMessage(MessageType.ACTION, $"Click '{this.userFriendlyName}'");

            GenericWait.Wait(
                () =>
            {
                IWebElement element = this.GetElement(this.GetRawClickableElement);
                this.ExecuteEvent(() => element.Click(), "Click");
                return true;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Send keys to the lazy element
        /// </summary>
        /// <param name="text">The text to send to the lazy element</param>
        public void SendKeys(string text)
        {
            this.Log.LogMessage(MessageType.ACTION, $"Send keys '{text}' to '{this.userFriendlyName}'");

            GenericWait.Wait(
                () =>
            {
                IWebElement element = this.GetElement(this.GetRawVisibleElement);
                this.ExecuteEvent(() => element.SendKeys(text), "SendKeys");
                return true;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Send Secret keys with no logging
        /// </summary>
        /// <param name="keys">The keys to send</param>
        public void SendSecretKeys(string keys)
        {
            this.Log.LogMessage(MessageType.ACTION, $"Send secret keys to '{this.userFriendlyName}'");

            IWebElement element = this.GetElement(this.GetRawVisibleElement);
            try
            {
                this.TestObject.Log.SuspendLogging();
                this.ExecuteEvent(() => element.SendKeys(keys), "SendKeys");
                this.TestObject.Log.ContinueLogging();
            }
            catch (Exception e)
            {
                this.TestObject.Log.ContinueLogging();
                this.TestObject.Log.LogMessage(MessageType.ERROR, "Exception during sending secret keys: " + e.Message + Environment.NewLine + e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Clear the lazy element
        /// </summary>
        public void Clear()
        {
            this.Log.LogMessage(MessageType.ACTION, $"Clear '{this.userFriendlyName}'");

            GenericWait.Wait(
                () =>
            {
                IWebElement element = this.GetElement(this.GetRawVisibleElement);
                this.ExecuteEvent(() => element.Clear(), "Clear");
                return true;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Submit the lazy element
        /// </summary>
        public void Submit()
        {
            this.Log.LogMessage(MessageType.ACTION, $"Submit '{this.userFriendlyName}'");

            GenericWait.Wait(
                () =>
            {
                IWebElement element = this.GetElement(this.GetRawExistingElement);
                this.ExecuteEvent(() => element.Submit(), "Submit");
                return true;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Select the dropdown option from the lazy element
        /// </summary>
        /// <param name="option">the option to select</param>
        public void SelectDropDownOption(string option)
        {
            this.Log.LogMessage(MessageType.ACTION, $"Select option: {option} from '{this.userFriendlyName}'");

            GenericWait.Wait(
                () =>
            {
                IWebElement element = this.GetElement(this.GetRawVisibleElement);
                this.ExecuteEvent(() => new SelectElement(element).SelectByText(option), "Select DropDown Option");
                return true;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Select the dropdown option by value from the Lazy element
        /// </summary>
        /// <param name="value">the value to select</param>
        public void SelectDropDownOptionByValue(string value)
        {
            this.Log.LogMessage(MessageType.ACTION, $"Select value: {value} from '{this.userFriendlyName}'");

            GenericWait.Wait(
                () =>
            {
                IWebElement element = this.GetElement(this.GetRawVisibleElement);
                this.ExecuteEvent(() => new SelectElement(element).SelectByValue(value), "Select DropDown Option By Value");
                return true;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Gets the Selected dropdown option from the Lazy element
        /// </summary>
        /// <returns>The selected option</returns>
        public string GetSelectedOptionFromDropdown()
        {
            this.Log.LogMessage(MessageType.INFORMATION, $"Getting Selected Option for lazy element '{this.userFriendlyName}'");

            return GenericWait.Wait<string>(
                () =>
            {
                return new SelectElement(this.GetElement(this.GetRawExistingElement)).SelectedOption.Text;
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Gets the Selected dropdown option from the Lazy element
        /// </summary>
        /// <returns>The selected option</returns>
        public List<string> GetSelectedOptionsFromDropdown()
        {
            this.Log.LogMessage(MessageType.INFORMATION, $"Getting Selected Options for lazy element '{this.userFriendlyName}'");

            return GenericWait.Wait<List<string>>(
                () =>
                {
                    List<string> selectedItems = new List<string>();
                    var elements = new SelectElement(this.GetElement(this.GetRawExistingElement)).AllSelectedOptions;

                    foreach (IWebElement element in elements)
                    {
                        if (element != null)
                        {
                            selectedItems.Add(element.Text);
                        }
                    }

                    return selectedItems;
                },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Gets the value for the given attribute
        /// </summary>
        /// <param name="attributeName">The given attribute name</param>
        /// <returns>The attribute value</returns>
        public string GetAttribute(string attributeName)
        {
            this.Log.LogMessage(MessageType.INFORMATION, $"Getting attribute '{attributeName}' for lazy element '{this.userFriendlyName}'");

            return GenericWait.Wait<string>(
                () =>
            {
                return this.GetElement(this.GetRawExistingElement).GetAttribute(attributeName);
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Gets the current value of an element - Useful for get input box text
        /// </summary>
        /// <returns>The element's current value</returns>
        public string GetValue()
        {
            this.Log.LogMessage(MessageType.INFORMATION, $"Getting value for lazy element '{this.userFriendlyName}'");

            return GenericWait.Wait<string>(
                () =>
            {
                return this.GetElement(this.GetRawVisibleElement).GetAttribute("value");
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Gets the CSS value for the given attribute
        /// </summary>
        /// <param name="propertyName">The given attribute/property name</param>
        /// <returns>The CSS value</returns>
        public string GetCssValue(string propertyName)
        {
            this.Log.LogMessage(MessageType.INFORMATION, $"Getting '{propertyName}' css value for lazy element '{this.userFriendlyName}'");

            return GenericWait.Wait<string>(
                () =>
            {
                return this.GetElement(this.GetRawExistingElement).GetCssValue(propertyName);
            },
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Wait for and get the visible web element
        /// </summary>
        /// <returns>The web visible web element</returns>
        public IWebElement GetRawVisibleElement()
        {
            if (this.elementIndex == null)
            {




                this.CachedElement = (this.parent == null) ? this.WebDriver.Wait().ForVisibleElement(this.By) :
                this.parent.GetRawExistingElement().Wait().ForVisibleElement(this.By);
            }
            else
            {
                this.CachedElement = this.GetRawIndexed((element => element.Displayed), "be visible");
            }

            return this.CachedElement;
        }

        /// <summary>
        /// Wait for and get the clickable web element
        /// </summary>
        /// <returns>The web clickable web element</returns>
        public IWebElement GetRawClickableElement()
        {
            if (this.elementIndex == null)
            {
                this.CachedElement = (this.parent == null) ? this.WebDriver.Wait().ForClickableElement(this.By) :
                    this.parent.GetRawExistingElement().Wait().ForClickableElement(this.By);
            }
            else
            {
                this.CachedElement = this.GetRawIndexed((element => element.Displayed && element.Enabled), "be clickable");
            }

            return this.CachedElement;
        }

        /// <summary>
        /// Wait for and get the web element
        /// </summary>
        /// <returns>The web element</returns>
        public IWebElement GetRawExistingElement()
        {
            if (this.elementIndex == null)
            {
                this.CachedElement = (this.parent == null) ? this.WebDriver.Wait().ForElementExist(this.By) :
                    this.parent.GetRawExistingElement().Wait().ForElementExist(this.By);
            }
            else
            {
                this.CachedElement = this.GetRawIndexed((element => element != null), "exist");
            }

            return this.CachedElement;
        }

        /// <summary>
        /// Get the element at the indexed value
        /// </summary>
        /// <returns>The element at the indexed value</returns>
        private IWebElement GetRawIndexed(Func<IWebElement, bool> matchesState, string expectedState)
        {
            return GenericWait.Wait<IWebElement>(
(Func<IWebElement>)(() =>
            {
                ReadOnlyCollection<IWebElement> elements = (this.parent == null) ? this.WebDriver.FindElements(this.By) :
                this.parent.GetRawExistingElement().FindElements(this.By);
                IWebElement element = elements[this.elementIndex ?? 0];

                if (!matchesState(element))
                {
                    throw new InvalidElementStateException($"Expected element to {expectedState}");
                }

                return element;
            }),
            this.WaitTime(),
            this.TimeoutTime());
        }

        /// <summary>
        /// Gets the value of a JavaScript property of this element
        /// </summary>
        /// <param name="propertyName">The name JavaScript the JavaScript property to get the value of</param>
        /// <returns> The JavaScript property's current value. Returns a null if the value is not set or the property does not exist</returns>
        public string GetProperty(string propertyName)
        {
            return this.GetRawExistingElement().GetProperty(propertyName);
        }

        /// <summary>
        /// Finds the first OpenQA.Selenium.IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>The first matching OpenQA.Selenium.IWebElement on the current context</returns>
        public IWebElement FindRawElement(By by)
        {
            return this.GetRawExistingElement().FindElement(by);
        }

        /// <summary>
        /// Finds the first lazy element using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>The first matching lazy element on the current context</returns>
        public IWebElement FindElement(By by)
        {
            return this.FindElement(by, "Child element");
        }

        /// <summary>
        /// Finds the first OpenQA.Selenium.IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>The first matching OpenQA.Selenium.IWebElement on the current context</returns>
        public abstract IWebElement FindElement(By by, string userFriendlyName);

        /// <summary>
        /// Finds all OpenQA.Selenium.IWebElement within the current context using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>All web elements matching the current criteria, or an empty list if nothing matches</returns>
        public ReadOnlyCollection<IWebElement> FindRawElements(By by)
        {
            return this.GetRawExistingElement().FindElements(by);
        }

        /// <summary>
        /// Finds all lazy elements within the current context using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>All web elements matching the current criteria, or an empty list if nothing matches</returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return this.FindElements(by, "Child elements");
        }

        /// <summary>
        /// Finds all OpenQA.Selenium.IWebElement within the current context using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>All web elements matching the current criteria, or an empty list if nothing matches</returns>
        public abstract ReadOnlyCollection<IWebElement> FindElements(By by, string userFriendlyName);

        /// <summary>
        /// Returns a string that represents the current object
        /// </summary>
        /// <returns>String of the current object</returns>
        public override string ToString()
        {
            string temp = string.Empty;

            // Check if LazyElement has a parent
            // If so, prefix parent's ToString()
            if (this.parent != null)
            {
                temp += this.parent.ToString();
            }

            return temp + this.By.ToString() + this.userFriendlyName;
        }

        /// <summary>
        /// Get a web element
        /// </summary>
        /// <param name="getElement">The get web element function</param>
        /// <returns>The web element</returns>
        [ExcludeFromCodeCoverage]
        private IWebElement GetElement(Func<IWebElement> getElement)
        {
            // Try to use cached element
            if (this.CachedElement != null)
            {
                try
                {
#pragma warning disable S1481
                    // Do this to make sure we waited for the element to exist
                    bool visible = this.CachedElement.Displayed;
#pragma warning restore S1481
                    return this.CachedElement;
                }
                catch (Exception e)
                {
                    this.TestObject.Log.LogMessage(MessageType.VERBOSE, "Re-finding element because: " + e.Message);
                }
            }

            try
            {
                this.TestObject.Log.LogMessage(MessageType.VERBOSE, "Performing lazy driver find on: " + this.By);
                this.CachedElement = getElement();
                return this.CachedElement;
            }
            catch (Exception e)
            {
                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.AppendLine("Failed to find: " + this.userFriendlyName);
                messageBuilder.AppendLine("Locator: " + this.By);
                messageBuilder.AppendLine("Because: " + e.Message);

                throw new Exception(messageBuilder.ToString(), e);
            }
        }

        /// <summary>
        /// Execute an element action
        /// </summary>
        /// <param name="elementAction">The element action</param>
        /// <param name="caller">Name of the action, for logging purposes</param>
        [ExcludeFromCodeCoverage]
        private void ExecuteEvent(Action elementAction, string caller)
        {
            try
            {
                this.TestObject.Log.LogMessage(MessageType.VERBOSE, "Performing lazy driver action: " + caller);
                elementAction.Invoke();
            }
            catch (Exception e)
            {
                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.AppendLine("Failed to " + caller + ": " + this.userFriendlyName);
                messageBuilder.AppendLine("Locator: " + this.By);
                messageBuilder.AppendLine("Because: " + e.Message);

                throw new Exception(messageBuilder.ToString(), e);
            }
        }
    }
}