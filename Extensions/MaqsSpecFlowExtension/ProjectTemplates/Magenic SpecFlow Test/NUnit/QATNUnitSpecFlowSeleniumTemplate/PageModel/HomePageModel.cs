﻿using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using OpenQA.Selenium;

namespace $safeprojectname$
{
    /// <summary>
    /// Page object for the $safeitemname$ page
    /// </summary>
    public class $safeitemname$ : BaseSeleniumPageModel
    {
        /// <summary>
        /// The page url
        /// </summary>
        private static readonly string PageUrl = SeleniumConfig.GetWebSiteBase() + "Static/Training3/HomePage.html";

        /// <summary>
        /// Initializes a new instance of the <see cref="$safeitemname$" /> class.
        /// </summary>
        /// <param name="testObject">The selenium test object</param>
        public $safeitemname$(SeleniumTestObject testObject) : base(testObject)
        {
        }

        /// <summary>
        /// Gets welcome message
        /// </summary>
        private LazyElement WelcomeMessage
        {
            get { return this.GetLazyElement(By.CssSelector("#WelcomeMessage"), "Welcome message"); }
        }

        /// <summary>
        /// Check if the home page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public override bool IsPageLoaded()
        {
            return this.WelcomeMessage.Displayed;
        }
    }
}

