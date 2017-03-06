//--------------------------------------------------
// <copyright file="ActionBuilder.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Magenic.MaqsFramework.BaseSeleniumTest
{
    /// <summary>
    /// Contains methods for interactions using selenium Actions class
    /// </summary>
    public static class ActionBuilder
    {
        /// <summary>
        /// Performs a hover over on an element
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="by">By selector for the element</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ActionBuilderUnitTests.cs" region="HoverOver" lang="C#" />
        /// </example>
        public static void HoverOver(this IWebDriver webDriver, By by)
        {
            Actions builder = new Actions(webDriver);
            IWebElement element = webDriver.Wait().ForClickableElement(by);
            builder.MoveToElement(element).Build().Perform();
        }

        /// <summary>
        /// Press non alphanumeric key. Ex. Control, Home, etc.
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="key">The key to press. NOTE: Use the Keys class</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ActionBuilderUnitTests.cs" region="PressModifier" lang="C#" />
        /// </example>
        public static void PressModifierKey(this IWebDriver webDriver, string key)
        {
            Actions builder = new Actions(webDriver);
            builder.SendKeys(key).Build().Perform();
        }

        /// <summary>
        /// Slider method which will take an offset of X pixels
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="element">Element to be used</param>
        /// <param name="pixelsOffset">Integer of pixels to be moved (Positive or negative)</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ActionBuilderUnitTests.cs" region="SlideElement" lang="C#" />
        /// </example>
        public static void SlideElement(this IWebDriver webDriver, By element, int pixelsOffset)
        {
            Actions builder = new Actions(webDriver);
            builder.DragAndDropToOffset(webDriver.FindElement(element), pixelsOffset, 0).Build().Perform();
        }

        /// <summary>
        /// Performs a right-click on an element
        /// Known Issue: This does not currently work for PhantomJS due to known issue with Right-Click. Issue may be resolved in future versions of PhantomJS.
        /// URL for Issue: <c>https://github.com/ariya/phantomjs/issues/11429</c>
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="by">By selector for the element</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/ActionBuilderUnitTests.cs" region="RightClick" lang="C#" />
        /// </example>
        public static void RightClick(this IWebDriver webDriver, By by)
        {
            Actions builder = new Actions(webDriver);
            IWebElement element = webDriver.Wait().ForClickableElement(by);
            builder.ContextClick(element).Build().Perform();
        }
    }
}
