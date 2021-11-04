//--------------------------------------------------
// <copyright file="ActionBuilder.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
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
            IWebElement element = webDriver.Wait().ForElementExist(by);
            builder.MoveToElement(element).Build().Perform();
        }

        /// <summary>
        /// Performs a hover over on an element
        /// </summary>
        /// <param name="element">The web element</param>
        public static void HoverOver(this IWebElement element)
        {
            Actions builder = new Actions(SeleniumUtilities.SearchContextToWebDriver(element));
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
            IWebElement sourceElement = webDriver.Wait().ForElementExist(element);

            sourceElement.DragAndDropToOffset(pixelsOffset, 0);
        }

        /// <summary>
        /// Slider method which will take an offset of X pixels
        /// </summary>
        /// <param name="element">Element to be used</param>
        /// <param name="pixelsOffset">Integer of pixels to be moved (Positive or negative)</param>
        public static void SlideElement(this IWebElement element, int pixelsOffset)
        {
            element.DragAndDropToOffset(pixelsOffset, 0);
        }

        /// <summary>
        /// Drag and drop an element
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="source">Element to drag and drop</param>
        /// <param name="destination">Where to drop the element</param>
        public static void DragAndDrop(this IWebDriver webDriver, By source, By destination)
        {
            IWebElement sourceElement = webDriver.Wait().ForElementExist(source);
            IWebElement destinationElement = webDriver.Wait().ForElementExist(destination);

            sourceElement.DragAndDrop(destinationElement);
        }

        /// <summary>
        /// Drag and drop an element
        /// </summary>
        /// <param name="source">Element to drag and drop</param>
        /// <param name="destination">Where to drop the element</param>
        public static void DragAndDrop(this IWebElement source, IWebElement destination)
        {
            Actions builder = new Actions(SeleniumUtilities.SearchContextToWebDriver(source));
            builder.DragAndDrop(source, destination).Build().Perform();
        }

        /// <summary>
        /// Drag and drop an element, plus or minus and X and Y offsets
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="source">Element to drag and drop</param>
        /// <param name="destination">Where to drop the element, plus or minus the offsets</param>
        /// <param name="pixelsXOffset">Integer of pixels to be moved (Positive or negative) horizontally</param>
        /// <param name="pixelsYOffset">Integer of pixels to be moved (Positive or negative) vertically </param>
        public static void DragAndDropToOffset(this IWebDriver webDriver, By source, By destination, int pixelsXOffset, int pixelsYOffset)
        {
            IWebElement sourceElement = webDriver.Wait().ForElementExist(source);
            IWebElement destinationElement = webDriver.Wait().ForElementExist(destination);

            DragAndDropToOffset(sourceElement, destinationElement, pixelsXOffset, pixelsYOffset);
        }

        /// <summary>
        /// Drag and drop an item to a destination, plus or minus and X and Y offsets
        /// </summary>
        /// <param name="source">Element to drag and drop</param>
        /// <param name="destination">Where to drop the element, plus or minus the offsets</param>
        /// <param name="pixelsXOffset">Integer of pixels to be moved (Positive or negative) horizontally</param>
        /// <param name="pixelsYOffset">Integer of pixels to be moved (Positive or negative) vertically </param>
        public static void DragAndDropToOffset(IWebElement source, IWebElement destination, int pixelsXOffset, int pixelsYOffset)
        {
            Actions builder = new Actions(SeleniumUtilities.SearchContextToWebDriver(source));

            // Move to element goes to the top left corner so compensate for that 
            int horizontalOffset = (destination.Size.Width / 2) + pixelsXOffset;
            int verticalOffset = (destination.Size.Height / 2) + pixelsYOffset;

            builder.ClickAndHold(source).MoveToElement(destination, horizontalOffset, verticalOffset).Release().Build().Perform();
        }

        /// <summary>
        /// Drag and drop an element to an X and Y offsets
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="source">Element to drag and drop</param>
        /// <param name="pixelsXOffset">Integer of pixels to be moved (Positive or negative) horizontally</param>
        /// <param name="pixelsYOffset">Integer of pixels to be moved (Positive or negative) vertically </param>
        public static void DragAndDropToOffset(this IWebDriver webDriver, By source, int pixelsXOffset, int pixelsYOffset)
        {
            IWebElement sourceElement = webDriver.Wait().ForElementExist(source);

            sourceElement.DragAndDropToOffset(pixelsXOffset, pixelsYOffset);
        }

        /// <summary>
        /// Drag and drop an element to an X and Y offsets
        /// </summary>
        /// <param name="source">Element to drag and drop</param>
        /// <param name="pixelsXOffset">Integer of pixels to be moved (Positive or negative) horizontally</param>
        /// <param name="pixelsYOffset">Integer of pixels to be moved (Positive or negative) vertically </param>
        public static void DragAndDropToOffset(this IWebElement source, int pixelsXOffset, int pixelsYOffset)
        {
            Actions builder = new Actions(SeleniumUtilities.SearchContextToWebDriver(source));

            builder.DragAndDropToOffset(source, pixelsXOffset, pixelsYOffset).Build().Perform();
        }

        /// <summary>
        /// Performs a right-click on an element
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="by">By selector for the element</param>
        public static void RightClick(this IWebDriver webDriver, By by)
        {
            IWebElement element = webDriver.Wait().ForClickableElement(by);
            element.RightClick();
        }

        /// <summary>
        /// Performs a right-click on an element
        /// </summary>
        /// <param name="element">The web element</param>
        public static void RightClick(this IWebElement element)
        {
            Actions builder = new Actions(SeleniumUtilities.SearchContextToWebDriver(element));
            builder.ContextClick(element).Build().Perform();
        }

        /// <summary>
        /// Performs a right-click on an element
        /// </summary>
        /// <param name="webDriver">The IWebDriver</param>
        /// <param name="by">By selector for the element</param>
        public static void DoubleClick(this IWebDriver webDriver, By by)
        {
            IWebElement element = webDriver.Wait().ForClickableElement(by);
            element.DoubleClick();
        }

        /// <summary>
        /// Performs a right-click on an element
        /// </summary>
        /// <param name="element">The web element</param>
        public static void DoubleClick(this IWebElement element)
        {
            Actions builder = new Actions(SeleniumUtilities.SearchContextToWebDriver(element));
            builder.DoubleClick(element).Build().Perform();
        }
    }
}
