//--------------------------------------------------
// <copyright file="FluentElement.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the FluentElement class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using OpenQA.Selenium;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace Magenic.MaqsFramework.BaseSeleniumTest.Extensions
{
    /// <summary>
    /// Wrapper for dynamically finding and interacting with elements
    /// </summary>
    public class FluentElement
    {
        /// <summary>
        /// The Selenium test object
        /// </summary>
        private SeleniumTestObject testObject;

        /// <summary>
        /// A user friendly name, for logging purposes
        /// </summary>
        private string userFriendlyName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentElement" /> class
        /// </summary>
        /// <param name="testObject">The base Selenium test object</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementCreate" lang="C#" />
        /// </example>
        public FluentElement(SeleniumTestObject testObject, By locator, string userFriendlyName)
        {
            this.testObject = testObject;
            this.By = locator;
            this.userFriendlyName = userFriendlyName;
        }

        /// <summary>
        /// Gets a the 'by' selector for the element
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementGetBy" lang="C#" />
        /// </example>
        public By By { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the fluent element is enabled
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementEnabled" lang="C#" />
        /// </example>
        public bool Enabled
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Enabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the fluent element is selected
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementSelected" lang="C#" />
        /// </example>
        public bool Selected
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Selected;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the fluent element is displayed
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementDisplayed" lang="C#" />
        /// </example>
        public bool Displayed
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Displayed;
            }
        }

        /// <summary>
        /// Gets the fluent element's tag name
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementTagName" lang="C#" />
        /// </example>
        public string TagName
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).TagName;
            }
        }

        /// <summary>
        /// Gets the fluent element's text
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementText" lang="C#" />
        /// </example>
        public string Text
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Text;
            }
        }

        /// <summary>
        /// Gets the fluent element's location
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementLocation" lang="C#" />
        /// </example>
        public Point Location
        {
            get
            {
                return this.GetElement(this.GetTheVisibleElement).Location;
            }
        }

        /// <summary>
        /// Gets the fluent element's size
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementSize" lang="C#" />
        /// </example>
        public Size Size
        {
            get
            {
                return this.GetElement(this.GetTheVisibleElement).Size;
            }
        }

        /// <summary>
        /// Click the fluent element 
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementClick" lang="C#" />
        /// </example>
        public void Click()
        {
            IWebElement element = this.GetElement(this.GetTheClickableElement);
            this.ExecuteEvent(() => element.Click(), "Click");
        }

        /// <summary>
        /// Send keys to the fluent element
        /// </summary>
        /// <param name="keys">The keys to send to the fluent element</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementSendKeys" lang="C#" />
        /// </example>
        public void SendKeys(string keys)
        {
            IWebElement element = this.GetElement(this.GetTheVisibleElement);
            this.ExecuteEvent(() => element.SendKeys(keys), "SendKeys");
        }

        /// <summary>
        /// Clear the fluent element 
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementClear" lang="C#" />
        /// </example>
        public void Clear()
        {
            IWebElement element = this.GetElement(this.GetTheVisibleElement);
            this.ExecuteEvent(() => element.Clear(), "Clear");
        }

        /// <summary>
        /// Submit the fluent element 
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementSubmit" lang="C#" />
        /// </example>
        public void Submit()
        {
            IWebElement element = this.GetElement(this.GetTheExistingElement);
            this.ExecuteEvent(() => element.Submit(), "Submit");
        }

        /// <summary>
        /// Gets the value for the given attribute
        /// </summary>
        /// <param name="attributeName">The given attribute name</param>
        /// <returns>The attribute value</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementGetAttribute" lang="C#" />
        /// </example>
        public string GetAttribute(string attributeName)
        {
            return this.GetElement(this.GetTheExistingElement).GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the current value of an element - Useful for get input box text
        /// </summary>
        /// <returns>The element's current value</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementSendKeys" lang="C#" />
        /// </example>
        public string GetValue()
        {
            return this.GetElement(this.GetTheVisibleElement).GetAttribute("value");
        }

        /// <summary>
        /// Gets the CSS value for the given attribute
        /// </summary>
        /// <param name="attributeName">The given attribute name</param>
        /// <returns>The CSS value</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementGetCssValue" lang="C#" />
        /// </example>
        public string GetCssValue(string attributeName)
        {
            return this.GetElement(this.GetTheExistingElement).GetCssValue(attributeName);
        }

        /// <summary>
        /// Wait for and get the visible web element
        /// </summary>
        /// <returns>The web visible web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementVisibleElement" lang="C#" />
        /// </example>
        public IWebElement GetTheVisibleElement()
        {
            return this.testObject.WebDriver.Wait().ForVisibleElement(this.By);
        }

        /// <summary>
        /// Wait for and get the clickable web element
        /// </summary>
        /// <returns>The web clickable web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementClickableElement" lang="C#" />
        /// </example>
        public IWebElement GetTheClickableElement()
        {
            return this.testObject.WebDriver.Wait().ForClickableElement(this.By);
        }

        /// <summary>
        /// Wait for and get the web element
        /// </summary>
        /// <returns>The web web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementExistingElement" lang="C#" />
        /// </example>
        public IWebElement GetTheExistingElement()
        {
            return this.testObject.WebDriver.Wait().ForElementExist(this.By);
        }

        /// <summary>
        /// Get a web element
        /// </summary>
        /// <param name="getElement">The get web element function</param>
        /// <returns>The web element</returns>
        [ExcludeFromCodeCoverage] 
        private IWebElement GetElement(Func<IWebElement> getElement)
        {
            try
            {
                this.testObject.Log.LogMessage(MessageType.VERBOSE, "Performing fluent wrapper find on: " + this.By);
                return getElement();
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
                this.testObject.Log.LogMessage(MessageType.VERBOSE, "Performing fluent wrapper action: " + caller);
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
