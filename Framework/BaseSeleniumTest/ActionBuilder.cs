﻿//--------------------------------------------------
// <copyright file="ActionBuilder.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Magenic.Maqs.BaseSeleniumTest
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
        public static void SlideElement(this IWebDriver webDriver, By element, int pixelsOffset)
        {
            Actions builder = new Actions(webDriver);
            builder.DragAndDropToOffset(webDriver.FindElement(element), pixelsOffset, 0).Build().Perform();
        }

        /// <summary>
        /// Performs a right-click on an element
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="by">By selector for the element</param>
        public static void RightClick(this IWebDriver webDriver, By by)
        {
            Actions builder = new Actions(webDriver);
            IWebElement element = webDriver.Wait().ForClickableElement(by);
            builder.ContextClick(element).Build().Perform();
        }
    }
}
