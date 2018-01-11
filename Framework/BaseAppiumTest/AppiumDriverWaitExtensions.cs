//--------------------------------------------------
// <copyright file="AppiumDriverWaitExtensions.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium driver wait extensions class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Magenic.MaqsFramework.BaseAppiumTest
{
    /// <summary>
    /// Appium driver wait extension methods
    /// </summary>
    public static class AppiumDriverWaitExtensions
    {
        /// <summary>
        /// Wait collection for the Appium Driver
        /// </summary>
        private static ConcurrentDictionary<AppiumDriver<IWebElement>, WebDriverWait> waitCollection;

        /// <summary>
        /// Initializes static members of the <see cref="AppiumDriverWaitExtensions" /> class
        /// </summary>
        static AppiumDriverWaitExtensions()
        {
            waitCollection = new ConcurrentDictionary<AppiumDriver<IWebElement>, WebDriverWait>();
        }

        /// <summary>
        /// Get the WebDriverWait
        /// </summary>
        /// <param name="driver">The appium driver</param>
        /// <returns>The WebDriverWait</returns>
        public static WebDriverWait GetWaitDriver(this AppiumDriver<IWebElement> driver)
        {
            if (waitCollection.ContainsKey(driver))
            {
                return waitCollection[driver];
            }
            else
            {
                WebDriverWait waiter = AppiumConfig.GetWaitDriver(driver);
                waitCollection.AddOrUpdate(driver, waiter, (oldkey, oldvalue) => waiter);
                return waiter;
            }
        }

        /// <summary>
        /// Set wait driver
        /// </summary>
        /// <param name="driver">The appium driver</param>
        /// <param name="waiter">Web Driver Wait</param>
        public static void SetWaitDriver(this AppiumDriver<IWebElement> driver, WebDriverWait waiter)
        {
            waitCollection.AddOrUpdate(driver, waiter, (oldkey, oldvalue) => waiter);
        }

        /// <summary>
        /// Remove the stored wait driver
        /// </summary>
        /// <param name="driver">The appium driver</param>
        /// <returns>True if the wait driver was removed</returns>
        public static bool RemoveWaitDriver(this AppiumDriver<IWebElement> driver)
        {
            WebDriverWait temp;
            return waitCollection.TryRemove(driver, out temp);
        }

        /// <summary>
        /// Wait for an element to not appear on the page - It can be gone or just not displayed
        /// </summary>
        /// <param name="driver">The appium driver</param>
        /// <param name="by">'by' selector for the element</param>
        public static void WaitForAbsentElement(this AppiumDriver<IWebElement> driver, By by)
        {
            if (!WaitUntilAbsentElement(driver, by))
            {
                throw new Exception(StringProcessor.SafeFormatter("The element '{0}' is still present.", by.ToString()));
            }
        }

        /// <summary>
        /// Wait for the page to load
        /// </summary>
        /// <param name="driver">The appium driver</param>
        public static void WaitForPageLoad(this AppiumDriver<IWebElement> driver)
        {
            if (!WaitUntilPageLoad(driver))
            {
                throw new Exception("Page load took longer than timeout configuration");
            }
        }

        /// <summary>
        /// Wait for an element to not appear on the page - It can be gone or just not displayed
        /// </summary>
        /// <param name="driver">The appium driver</param>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>True if the element is not visible on the page</returns>
        public static bool WaitUntilAbsentElement(this AppiumDriver<IWebElement> driver, By by)
        {
            DateTime start = DateTime.Now;

            do
            {
                // Give the system some time before checking if an element is missing
                Thread.Sleep(GetWaitDriver(driver).PollingInterval);

                try
                {
                    if (!driver.FindElement(by).Displayed)
                    {
                        // The element on the page is present, but not visible
                        return true;
                    }
                }
                catch (NoSuchElementException)
                {
                    // The element is missing all together
                    return true;
                }
                catch
                {
                    // Could be several exceptions
                }
            }
            while ((DateTime.Now - start) < GetWaitDriver(driver).Timeout);

            return false;
        }

        /// <summary>
        /// Wait for the page to load
        /// </summary>
        /// <param name="driver">The appium driver</param>
        /// <returns>True if the page finished loading</returns>
        public static bool WaitUntilPageLoad(this AppiumDriver<IWebElement> driver)
        {
            DateTime start = DateTime.Now;
            string source = string.Empty;

            do
            {
                // Give the system a second before checking if the page is updating
                Thread.Sleep(TimeSpan.FromSeconds(1));

                try
                {
                    // Find any element
                    driver.FindElement(By.CssSelector("*"));

                    // Get the page source
                    string newSource = driver.PageSource;

                    // Make sure the souce has not changed and it actully has content
                    if (!string.IsNullOrEmpty(source) && source.Equals(newSource))
                    {
                        return true;
                    }

                    source = newSource;
                }
                catch
                {
                    // Could be several exceptions - Don't really care as it may just be the page loading
                }
            }
            while ((DateTime.Now - start) < GetWaitDriver(driver).Timeout);

            // Page was still loading
            return false;
        }

        /// <summary>
        /// Check if an element is clickable
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>Success if the element is clickable</returns>
        private static Func<AppiumDriver<IWebElement>, IWebElement> ElementIsClickable(By by)
        {
            return driver =>
            {
                var element = driver.FindElement(by);
                return (element != null && element.Displayed && element.Enabled) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element is visible
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>Success if the element is visible</returns>
        private static Func<AppiumDriver<IWebElement>, IWebElement> ElementIsVisible(By by)
        {
            return driver =>
            {
                var element = driver.FindElement(by);
                return (element != null && element.Displayed) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element is enabled
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <returns>Success if the element is enabled</returns>
        private static Func<AppiumDriver<IWebElement>, IWebElement> ElementIsEnabled(By by)
        {
            return driver =>
            {
                var element = driver.FindElement(by);
                return (element != null && element.Enabled) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element has exactly the expected text - Case sensitive
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The expected text</param>
        /// <returns>Success if the element has the expected text</returns>
        private static Func<AppiumDriver<IWebElement>, IWebElement> ElementHasExpectedText(By by, string text)
        {
            return driver =>
            {
                var element = driver.FindElement(by);
                ElementIsEnabled(by);
                return (element != null && element.Displayed && element.Text.Equals(text)) ? element : null;
            };
        }

        /// <summary>
        /// Check if an element contains the expected text - Case insensitive
        /// </summary>
        /// <param name="by">'by' selector for the element</param>
        /// <param name="text">The expected text</param>
        /// <returns>Success if the element contains the expected text</returns>
        private static Func<AppiumDriver<IWebElement>, IWebElement> ElementContainsExpectedText(By by, string text)
        {
            return driver =>
            {
                var element = driver.FindElement(by);
                ElementIsEnabled(by);
                return (element != null && element.Displayed && element.Text.ToUpper().Contains(text.ToUpper())) ? element : null;
            };
        }
    }
}
