//--------------------------------------------------
// <copyright file="BaseAppiumPageModel.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base Appium page model class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Base Appium page model
    /// </summary>
    public abstract class BaseAppiumPageModel
    {
        /// <summary>
        /// Store of LazyMobileElement for the page
        /// </summary>
        private readonly Dictionary<string, LazyMobileElement> lazyElementStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseAppiumPageModel"/> class.
        /// </summary>
        /// <param name="testObject">The Appium test object</param>
        protected BaseAppiumPageModel(AppiumTestObject testObject)
        {
            this.TestObject = testObject;
            this.AppiumDriver = testObject.AppiumDriver;
            this.lazyElementStore = new Dictionary<string, LazyMobileElement>();
        }

        /// <summary>
        /// Gets the webdriver from the test object
        /// </summary>
        protected AppiumDriver<IWebElement> AppiumDriver { get; private set; }

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
        /// Gets or sets the Appium test object
        /// </summary>
        protected AppiumTestObject TestObject { get; set; }

        /// <summary>
        /// Override the driver 
        /// This allows you to use something other than the default tests object driver.
        /// </summary>
        /// <param name="appiumDriver">The override driver</param>
        public void OverrideDriver(AppiumDriver<IWebElement> appiumDriver)
        {
            this.AppiumDriver = appiumDriver;
        }

        /// <summary>
        /// Check if the page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public abstract bool IsPageLoaded();

        /// <summary>
        /// Gets LazyMobileElement from page model's lazy element store if it exists, otherwise
        /// initializes a new instance of the LazyElement and adds it to the lazy element store
        /// </summary>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>The LazyMobileElement</returns>
        protected LazyMobileElement GetLazyElement(By locator, [CallerMemberName] string userFriendlyName = null)
        {
            string lazyElementStoreKey = $"{locator}{userFriendlyName}";

            if (!this.lazyElementStore.ContainsKey(lazyElementStoreKey))
            {
                this.lazyElementStore.Add(lazyElementStoreKey, new LazyMobileElement(this.TestObject, locator, userFriendlyName));
            }

            return this.lazyElementStore[lazyElementStoreKey];
        }

        /// <summary>
        /// Gets LazyMobileElement from page model's lazy element store if it exists, otherwise
        /// initializes a new instance of the LazyElement and adds it to the lazy element store
        /// </summary>
        /// <param name="parent">The LazyElement parent element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>The LazyMobileElement</returns>
        protected LazyMobileElement GetLazyElement(LazyMobileElement parent, By locator, [CallerMemberName] string userFriendlyName = null)
        {
            string lazyElementStoreKey = $"{parent.ToString()}{locator}{userFriendlyName}";

            if (!this.lazyElementStore.ContainsKey(lazyElementStoreKey))
            {
                this.lazyElementStore.Add(lazyElementStoreKey, new LazyMobileElement(parent, locator, userFriendlyName));
            }

            return this.lazyElementStore[lazyElementStoreKey];
        }
    }
}
