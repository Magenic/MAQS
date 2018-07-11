using Magenic.Maqs.SpecFlow.TestSteps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    /// <summary>
    /// Steps class for WebServiceFeatureSteps
    /// To utilize MAQS features for the steps in this class, make sure at add a 'MAQS_WebService' tag to the feature file(s)
    /// </summary>
    [Binding]
    public class WebServiceFeatureSteps : BaseWebServiceTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceFeatureSteps"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected WebServiceFeatureSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Sample given method
        /// </summary>
        [Given(@"condition")]
        public void GivenCondition()
        {
            // Add condition code...
        }

        /// <summary>
        /// Sample when method
        /// </summary>
        [When(@"action")]
        public void WhenAction()
        {
            // Add action code...
        }

        /// <summary>
        /// Sample then method
        /// </summary>
        [Then(@"verification")]
        public void ThenVerification()
        {
            // Add verification code...
        }

        /// <summary>
        /// Calls a fake endpoint
        /// </summary>
        private void CallEndpoint()
        {
            // calls the fake endpoint
            this.TestObject.WebServiceDriver.Get("fake/endpoint", "application/json", true);
        }
    }
}
