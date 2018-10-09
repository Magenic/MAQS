using Magenic.Maqs.BaseAppiumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using OpenQA.Selenium;
namespace Models.Mobile
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
        /// The greeting message element 'By' finder
        /// </summary>
        protected abstract LazyMobileElement GreetingMessage { get; }

        /// <summary>
        /// The time description input element 'By' finder
        /// </summary>
        protected abstract LazyMobileElement TimeDisc { get; }

        /// <summary>
        /// The time element 'By' finder
        /// </summary>
        protected abstract LazyMobileElement Time { get; }

        /// <summary>
        /// Get greeting text from label
        /// </summary>
        /// <returns>Greeting text string</returns>
        public string GetGreetingMessage()
        {
            return GreetingMessage.Text;
        }

        /// <summary>
        /// Get the time description text from label
        /// </summary>
        /// <returns>Time description text string</returns>
        public string GetTimeDiscription()
        {
            return TimeDisc.Text;
        }

        /// <summary>
        /// Get time from label
        /// </summary>
        /// /// <returns>Time text string</returns>
        public string GetTime()
        {
            return Time.Text;
        }
    }
}