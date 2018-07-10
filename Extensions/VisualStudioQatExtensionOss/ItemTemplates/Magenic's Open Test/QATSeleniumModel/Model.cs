using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using OpenQA.Selenium;

namespace $rootnamespace$
{
	/// <summary>
    /// Page object for $safeitemname$
    /// </summary>
    public class $safeitemname$
    {
        /// <summary>
        /// The page url
        /// </summary>
        private static readonly string PageUrl = SeleniumConfig.GetWebSiteBase() + "PAGE.html";

        /// <summary>
        /// Selenium test object
        /// </summary>
        private SeleniumTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="$safeitemname$" /> class.
        /// </summary>
        /// <param name="testObject">The selenium test object</param>
        public $safeitemname$(SeleniumTestObject testObject)
        {
            this.testObject = testObject;
        }

        /// <summary>
        /// Gets the sample element
        /// </summary>
        private LazyElement Sample
        {
            get { return new LazyElement(this.testObject, By.CssSelector("#CSS_ID"), "SAMPLE"); }
        }

        /// <summary>
        /// Check if the page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public bool IsPageLoaded()
        {
            return this.Sample.Displayed;
        }
    }
	
}
