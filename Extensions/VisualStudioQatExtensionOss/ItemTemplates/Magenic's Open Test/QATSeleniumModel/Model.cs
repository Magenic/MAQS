using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
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
        private static readonly string PageUrl = Config.GetValue("WebSiteBase") + "PAGE.html";

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
        private FluentElement Sample
        {
            get { return new FluentElement(this.testObject, By.CssSelector("#CSS_ID"), "SAMPLE"); }
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
