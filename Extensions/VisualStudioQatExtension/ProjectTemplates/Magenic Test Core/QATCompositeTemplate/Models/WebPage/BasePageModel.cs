using Magenic.MaqsFramework.BaseSeleniumTest;

namespace Models
{
    /// <summary>
    /// Base Page Model
    /// </summary>
    public abstract class BasePageModel
    {
        /// <summary>
        /// Selenium test object
        /// </summary>
        protected SeleniumTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageModel" /> class.
        /// </summary>
        /// <param name="testObject">The selenium test object</param>
        public BasePageModel(SeleniumTestObject testObject)
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