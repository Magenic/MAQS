using Magenic.Maqs.SpecFlow.TestSteps;
using NUnit.Framework;
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
			CallEndpoint();
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
        /// Calls a fake endpoint
        /// </summary>
        private void CallEndpoint()
        {
            // Calls an endpoint
			string result = this.TestObject.WebServiceDriver.Get("/api/String/1", "text/plain", false);
            Assert.IsTrue(result.Contains("Tomato Soup"), "Was expeting a result with Tomato Soup but instead got - " + result);
        }
    }
}
