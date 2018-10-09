using Magenic.Maqs.BaseAppiumTest;
using OpenQA.Selenium;
namespace $safeprojectname$
{
    /// <summary>
    /// Page object for IOSHomePageModel inheriting from the AHomePageModel
    /// </summary>
    public class IOSHomePageModel : AHomePageModel
    {
        /// <summary>
        /// The greeting message element 'By' finder
        /// </summary>
        protected override LazyMobileElement GreetingMessage
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("Welcome"), "Welcome Label"); }
        }

        /// <summary>
        /// The time description element 'By' finder
        /// </summary>
        protected override LazyMobileElement TimeDisc
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("TimeDesc"), "Timer Label"); }
        }

        /// <summary>
        /// The time element 'By' finder
        /// </summary>
        protected override LazyMobileElement Time
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("Time"), "Timer"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IOSHomePageModel" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public IOSHomePageModel(AppiumTestObject testObject)
        {
            this.TestObject = testObject;
        }
    }
}