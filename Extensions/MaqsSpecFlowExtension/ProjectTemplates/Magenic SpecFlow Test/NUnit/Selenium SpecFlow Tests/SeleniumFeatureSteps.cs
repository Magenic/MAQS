using SpecFlowMAQSExtension.TestSteps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    /// <summary>
    /// Steps class for SeleniumFeatureSteps
    /// To utilize MAQS features for the steps in this class, make sure at add a 'MAQS_Selenium' tag to the feature file(s)
    /// </summary>
    [Binding]
    public class SeleniumFeatureSteps : BaseSeleniumTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeleniumFeatureSteps"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected SeleniumFeatureSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Sample given method
        /// </summary>
        [Given(@"condition")]
        public void GivenCondition()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Sample when method
        /// </summary>
        [When(@"action")]
        public void WhenAction()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Sample then method
        /// </summary>
        [Then(@"verification")]
        public void ThenVerification()
        {
            ScenarioContext.Current.Pending();
        }

        /// <summary>
        /// Open the page
        /// </summary>
        private void OpenPage()
        {
            // sample open login page
            this.TestObject.WebDriver.Navigate().GoToUrl("https://SOMETHING");
        }
    }
}
