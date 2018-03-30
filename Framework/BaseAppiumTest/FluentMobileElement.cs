//--------------------------------------------------
// <copyright file="FluentMobileElement.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the FluentMobileElement class</summary>
//--------------------------------------------------
using OpenQA.Selenium;
using System;

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Wrapper for dynamically finding and interacting with elements
    /// </summary>
    [Obsolete("FluentMobileElement is deprecated, please use LazyMobileElement instead.")]
    public class FluentMobileElement : LazyMobileElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMobileElement" /> class
        /// </summary>
        /// <param name="testObject">The base Selenium test object</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementCreate" lang="C#" />
        /// </example>
        public FluentMobileElement(AppiumTestObject testObject, By locator, string userFriendlyName) : base(testObject, locator, userFriendlyName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentMobileElement" /> class
        /// </summary>
        /// <param name="parent">The parent fluent element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/FluentElementUnitTests.cs" region="FluentElementCreateWithParent" lang="C#" />
        /// </example>
        public FluentMobileElement(FluentMobileElement parent, By locator, string userFriendlyName) : base(parent, locator, userFriendlyName)
        {
        }
    }
}