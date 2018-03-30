//--------------------------------------------------
// <copyright file="FluentElement.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the FluentElement class</summary>
//--------------------------------------------------
using OpenQA.Selenium;
using System;

namespace Magenic.MaqsFramework.BaseSeleniumTest.Extensions
{
    /// <summary>
    /// Wrapper for dynamically finding and interacting with elements
    /// </summary>
    [Obsolete("FluentElement is deprecated, please use LazyElement instead.")]
    public class FluentElement : LazyElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentElement" /> class
        /// </summary>
        /// <param name="testObject">The base Selenium test object</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        public FluentElement(SeleniumTestObject testObject, By locator, string userFriendlyName) : base(testObject, locator, userFriendlyName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentElement" /> class
        /// </summary>
        /// <param name="parent">The parent fluent element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        public FluentElement(FluentElement parent, By locator, string userFriendlyName) : base(parent, locator, userFriendlyName)
        {
        }
    }
}
