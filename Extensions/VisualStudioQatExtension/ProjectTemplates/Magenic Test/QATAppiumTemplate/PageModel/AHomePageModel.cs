using Magenic.MaqsFramework.BaseAppiumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
namespace $safeprojectname$
{
    /// <summary>
    /// Page object for AHomePageModel
    /// </summary>
    public abstract class AHomePageModel
    {
        /// <summary>
        /// Appium test object
        /// </summary>
        protected AppiumTestObject TestObject;

        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement GreetingMessage { get; }

        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement TimeDisc { get; }

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        protected abstract FluentMobileElement Time { get; }

        /// <summary>
        /// Get username text from label
        /// </summary>
        /// <returns>username text string</returns>
        public string GetGreetingMessage()
        {
            return GreetingMessage.Text;
        }

        /// <summary>
        /// Get username text from label
        /// </summary>
        /// <returns>username text string</returns>
        public string GetTimeDiscription()
        {
            return TimeDisc.Text;
        }

        /// <summary>
        /// Get password text from label
        /// </summary>
        /// /// <returns>password text string</returns>
        public string GetTime()
        {
            return Time.Text;
        }
    }
}