using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Magenic.Maqs.SpecFlow.TestSteps;
using TechTalk.SpecFlow;

namespace $safeprojectname$.Steps
{
    /// <summary>
    /// Steps class for AppiumFeatureSteps
    /// To utilize MAQS features for the steps in this class, make sure at add a 'MAQS_Appium' tag to the feature file(s)
    /// </summary>
    [Binding]
    public class AppiumFeatureSteps : BaseAppiumTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumFeatureSteps"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected AppiumFeatureSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Sample given method
        /// </summary>
        [Given(@"condition")]
        public void GivenCondition()
        {
            // ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Sample when method
        /// </summary>
        [When(@"action")]
        public void WhenAction()
        {
            // ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Sample then method
        /// </summary>
        [Then(@"verification")]
        public void ThenVerification()
        {
            // ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Finds the element with the given by
        /// </summary>
        /// <param name="by">The by to search with</param>
        /// <returns>The IWebElement found</returns>
        private IWebElement FindElement(By by)
        {
            return this.TestObject.AppiumDriver.FindElement(by);
        }
    }
}
