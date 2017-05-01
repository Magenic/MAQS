using Magenic.MaqsFramework.BaseSeleniumTest;

namespace $safeprojectname$
{
    /// <summary>
    /// Base Page Model
    /// </summary>
    public abstract class BasePage
    {
        /// <summary>
        /// Selenium test object
        /// </summary>
        protected SeleniumTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePage" /> class.
        /// </summary>
        /// <param name="testObject">The selenium test object</param>
        public BasePage(SeleniumTestObject testObject)
        {
            this.testObject = testObject;
        }

        /// <summary>
        /// Check if the page has been loaded
        /// </summary>
        /// <returns>True if the page was loaded</returns>
        public abstract bool IsPageLoaded();
    }
}