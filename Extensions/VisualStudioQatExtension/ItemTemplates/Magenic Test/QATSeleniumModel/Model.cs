using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
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
        private const string PageUrl = "https://SOMETHING";

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
        /// Sample lazy element
        /// </summary>
        private LazyElement Sample
        {
            get { return new LazyElement(this.testObject, By.CssSelector("#CSS_ID"), "Sample message"); }
        }

        /// <summary>
        /// Open the page
        /// </summary>
        public void OpenPage()
        {
            // sample open login page
            this.testObject.WebDriver.Navigate().GoToUrl(PageUrl);
            this.AssertPageLoaded();
        }

        /// <summary>
        /// Verify we are on the login page
        /// </summary>
        public void AssertPageLoaded()
        {
			//Assert depends on what testing framework is being used
            //Assert.IsTrue(
            //    this.webDriver.Url.Equals(PageUrl, System.StringComparison.CurrentCultureIgnoreCase),
            //    "Expected to be on '{0}', but was on '{1}' instead",
            //    PageUrl,
            //    this.webDriver.Url);
        }
    }
}
