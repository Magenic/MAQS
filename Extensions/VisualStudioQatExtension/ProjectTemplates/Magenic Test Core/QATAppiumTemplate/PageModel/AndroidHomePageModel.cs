using System;
using Magenic.MaqsFramework.BaseAppiumTest;
using OpenQA.Selenium;
namespace $safeprojectname$
{
    /// <summary>
    /// Page object for AndroidHomePageModel inheriting from the AHomePageModel
    /// </summary>
    public class AndroidHomePageModel : AHomePageModel
    {
        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected override LazyMobileElement GreetingMessage
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("Welcome"), "Welcome Label"); }
        }

        /// <summary>
        /// The user name input element 'By' finder
        /// </summary>
        protected override LazyMobileElement TimeDisc
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("TimeDesc"), "Timer Label"); }
        }

        /// <summary>
        /// The password input element 'By' finder
        /// </summary>
        protected override LazyMobileElement Time
        {
            get { return new LazyMobileElement(this.TestObject, By.Id("Time"), "Timer"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidHomePageModel" /> class.
        /// </summary>
        /// <param name="testObject">The appium test object</param>
        public AndroidHomePageModel(AppiumTestObject testObject)
        {
            this.TestObject = testObject;
        }
    }
}