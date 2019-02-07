//--------------------------------------------------
// <copyright file="BaseSeleniumPageModel.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Selenium page model class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Magenic.Maqs.BaseSeleniumTest
{
    /// <summary>
    /// Base Selenium page model
    /// </summary>
    public abstract class BaseSeleniumPageModel
    {
        /// <summary>
        /// Store of LazyElements for the page
        /// </summary>
        private readonly Dictionary<string, LazyElement> lazyElementStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSeleniumPageModel"/> class.
        /// </summary>
        /// <param name="testObject">The Selenium test object</param>
        protected BaseSeleniumPageModel(SeleniumTestObject testObject)
        {
            this.TestObject = testObject;
            this.WebDriver = testObject.WebDriver;
            this.lazyElementStore = new Dictionary<string, LazyElement>();
        }

        /// <summary>
        /// Gets the webdriver from the test object
        /// </summary>
        protected IWebDriver WebDriver { get; private set; }

        /// <summary>
        /// Gets the log from the test object
        /// </summary>
        protected Logger Log
        {
            get { return this.TestObject.Log; }
        }

        /// <summary>
        /// Gets the performance timer collection from the test object
        /// </summary>
        protected PerfTimerCollection PerfTimerCollection
        {
            get { return this.TestObject.PerfTimerCollection; }
        }

        /// <summary>
        /// Gets or sets the Selenium test object
        /// </summary>
        protected SeleniumTestObject TestObject { get; set; }

        /// <summary>
        /// Override the webdriver 
        /// This allows you to use something other than the default tests object webdriver.
        /// </summary>
        /// <param name="webDriver">The override webdriver</param>
        public void OverrideWebDriver(IWebDriver webDriver)
        {
            this.WebDriver = webDriver;
        }

        /// <summary>
        /// Check if the page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public abstract bool IsPageLoaded();
        
        /// <summary>
        /// Gets LazyElement from page model's lazy element store if it exists, otherwise
        /// initializes a new instance of the LazyElement and adds it to the lazy element store
        /// </summary>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>The LazyElement</returns>
        protected LazyElement GetLazyElement(By locator, [CallerMemberName] string userFriendlyName = null)
        {
            string lazyElementStoreKey = locator.ToString() + userFriendlyName;

            if (!this.lazyElementStore.ContainsKey(lazyElementStoreKey))
            {
                this.lazyElementStore.Add(lazyElementStoreKey, new LazyElement(this.TestObject, locator, userFriendlyName));
            }

            return this.lazyElementStore[lazyElementStoreKey];
        }

        /// <summary>
        /// Gets LazyElement from page model's lazy element store if it exists, otherwise
        /// initializes a new instance of the LazyElement and adds it to the lazy element store
        /// </summary>
        /// <param name="parent">The LazyElement parent element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>The LazyElement</returns>
        protected LazyElement GetLazyElement(LazyElement parent, By locator, [CallerMemberName] string userFriendlyName = null)
        {
            string lazyElementStoreKey = parent.ToString() + locator.ToString() + userFriendlyName;

            if (!this.lazyElementStore.ContainsKey(lazyElementStoreKey))
            {
                this.lazyElementStore.Add(lazyElementStoreKey, new LazyElement(parent, locator, userFriendlyName));
            }

            return this.lazyElementStore[lazyElementStoreKey];
        }
    }
}
