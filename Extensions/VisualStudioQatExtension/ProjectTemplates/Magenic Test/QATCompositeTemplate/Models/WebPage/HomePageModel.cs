using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
using OpenQA.Selenium;

namespace $safeprojectname$
{
    /// <summary>
    /// Page object for the Automation page
    /// </summary>
    public class $safeitemname$ : BasePageModel
    {
        /// <summary>
        /// The page url
        /// </summary>
        private static string PageUrl = Config.GetValue("WebSiteBase") + "Static/Training3/HomePage.html";

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
        private FluentElement WelcomeMessage
        {
            get { return new FluentElement(this.testObject, By.CssSelector("#WelcomeMessage"), "Welcome message"); }
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

