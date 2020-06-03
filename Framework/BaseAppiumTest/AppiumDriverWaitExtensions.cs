﻿//--------------------------------------------------
// <copyright file="AppiumDriverWaitExtensions.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>This is the Appium driver wait extensions class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Magenic.Maqs.BaseAppiumTest
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

#pragma warning disable S3963 // "static" fields should be initialized inline
        /// <summary>
        /// Initializes static members of the <see cref="AppiumDriverWaitExtensions" /> class
        /// </summary>
        static AppiumDriverWaitExtensions()
        {
            waitCollection = new ConcurrentDictionary<AppiumDriver<IWebElement>, WebDriverWait>();
        }
#pragma warning restore S3963 // "static" fields should be initialized inline

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
                WebDriverWait waiter = AppiumUtilities.GetDefaultWaitDriver(driver);
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
            return waitCollection.TryRemove(driver, out WebDriverWait temp);
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
                throw new TimeoutException(StringProcessor.SafeFormatter("The element '{0}' is still present.", by.ToString()));
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
                throw new TimeoutException("Page load took longer than timeout configuration");
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
                    driver.FindElement(By.XPath("/*"));

                    // Get the page source
                    string newSource = driver.PageSource;

                    // Make sure the source has not changed and it actually has content
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
    }
}
