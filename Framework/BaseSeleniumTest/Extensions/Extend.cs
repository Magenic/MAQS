//--------------------------------------------------
// <copyright file="Extend.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web driver and element extension</summary>
//--------------------------------------------------
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.UI;
using System.Collections.Concurrent;

namespace Magenic.MaqsFramework.BaseSeleniumTest.Extensions
{
    /// <summary>
    /// Web driver and element extensions
    /// </summary>
    public static class Extend
    {
        /// <summary>
        /// Selenium Web Driver
        /// </summary>
        private static ConcurrentDictionary<IWebDriver, WebDriverWait> waitCollection;

        /// <summary>
        /// Initializes static members of the <see cref="Extensions.Extend" /> class
        /// </summary>
        static Extend()
        {
            waitCollection = new ConcurrentDictionary<IWebDriver, WebDriverWait>();
        }

        /// <summary>
        /// Return the wait extension
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <returns>The wait extension</returns>
        public static Wait Wait(this ISearchContext searchContext)
        {
            IWebDriver driver = (searchContext is IWebDriver) ? (IWebDriver)searchContext : SeleniumUtilities.WebElementToWebDriver((IWebElement)searchContext);
            return new Wait(searchContext, GetWaitDriver(driver));
        }

        /// <summary>
        /// Return the find extension
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <returns>The find extension</returns>
        public static Find Find(this ISearchContext searchContext)
        {
            return new Find(searchContext);
        }

        /// <summary>
        /// Get the WebDriverWait
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <returns>The WebDriverWait</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="GetWaitDriver" lang="C#" />
        /// </example>
        public static WebDriverWait GetWaitDriver(this ISearchContext searchContext)
        {
            // Make sure we have the base driver and not the event firing wrapper
            IWebDriver unwrappedDriver = GetLowLevelDriver(searchContext);

            if (waitCollection.ContainsKey(unwrappedDriver))
            {
                return waitCollection[unwrappedDriver];
            }
            else
            {
                WebDriverWait waiter = SeleniumConfig.GetWaitDriver(unwrappedDriver);
                waitCollection.AddOrUpdate(unwrappedDriver, waiter, (oldkey, oldvalue) => waiter);
                return waiter;
            }
        }

        /// <summary>
        /// Sets the WebDriverWait
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <param name="waiter">The WebDriverWait</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="SetWaitDriver" lang="C#" />
        /// </example>
        public static void SetWaitDriver(this ISearchContext searchContext, WebDriverWait waiter)
        {
            waitCollection.AddOrUpdate(GetLowLevelDriver(searchContext), waiter, (oldkey, oldvalue) => waiter);
        }

        /// <summary>
        /// Reset the WebDriverWait to the default
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="ResetWaitDriver" lang="C#" />
        /// </example>
        public static void ResetWaitDriver(this ISearchContext searchContext)
        {
            // Make sure we have the base driver and not the event firing wrapper
            IWebDriver unwrappedDriver = GetLowLevelDriver(searchContext);

            WebDriverWait waiter = SeleniumConfig.GetWaitDriver(unwrappedDriver);
            waitCollection.AddOrUpdate(unwrappedDriver, waiter, (oldkey, oldvalue) => waiter);
        }

        /// <summary>
        /// Remove the stored wait driver
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <returns>True if the wait driver was removed</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/SeleniumUnitTest.cs" region="RemoveWaitDriver" lang="C#" />
        /// </example>
        public static bool RemoveWaitDriver(this ISearchContext searchContext)
        {
            WebDriverWait temp;
            return waitCollection.TryRemove(GetLowLevelDriver(searchContext), out temp);
        }

        /// <summary>
        /// Get the low level, none event firing, web driver
        /// </summary>
        /// <param name="searchContext">Web driver or element</param>
        /// <returns>the underlying web driver</returns>
        public static IWebDriver GetLowLevelDriver(ISearchContext searchContext)
        {
            IWebDriver driver = SeleniumUtilities.SearchContextToWebDriver(searchContext);
            return driver is EventFiringWebDriver ? ((EventFiringWebDriver)driver).WrappedDriver : driver;
        }
    }
}
