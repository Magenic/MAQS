//--------------------------------------------------
// <copyright file="LazyMobileElement.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>This is the LazyMobileElement class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Driver for dynamically finding and interacting with elements
    /// </summary>
    public class LazyMobileElement : AbstractLazyIWebElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LazyMobileElement" /> class
        /// </summary>
        /// <param name="testObject">The base Appium test object</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        public LazyMobileElement(AppiumTestObject testObject, By locator, [CallerMemberName] string userFriendlyName = null) : base(testObject, testObject.AppiumDriver, () => testObject.AppiumDriver.GetWaitDriver(), locator, userFriendlyName)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyMobileElement" /> class
        /// </summary>
        /// <param name="parent">The parent lazy element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        public LazyMobileElement(LazyMobileElement parent, By locator, [CallerMemberName] string userFriendlyName = null) : base(parent, locator, userFriendlyName)
        {

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LazyMobileElement" /> class
        /// </summary>
        /// <param name="parent">The parent lazy element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="cachedElement">The cached web element</param>
        /// <param name="index">The index of the element - Used if the by finds multiple elements</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        private LazyMobileElement(LazyMobileElement parent, By locator, IWebElement cachedElement, int index, [CallerMemberName] string userFriendlyName = null) : base(parent, locator, cachedElement, index, userFriendlyName)
        {

        }

        /// <summary>
        /// Finds the first IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>The first matching OpenQA.Selenium.IWebElement on the current context</returns>
        public override IWebElement FindElement(By by, string userFriendlyName)
        {
            return new LazyMobileElement(this, by, $"{userFriendlyName}");
        }

        /// <summary>
        /// Finds all IWebElement within the current context using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <returns>All web elements matching the current criteria, or an empty list if nothing matches</returns>
        public override ReadOnlyCollection<IWebElement> FindElements(By by, string userFriendlyName)
        {
            int index = 0;
            List<IWebElement> elements = new List<IWebElement>();
            foreach (IWebElement element in this.GetRawExistingElement().FindElements(by))
            {
                elements.Add(new LazyMobileElement(this, by, element, index, $"{userFriendlyName} - {index++}"));
            }

            return elements.AsReadOnly();
        }
    }
}