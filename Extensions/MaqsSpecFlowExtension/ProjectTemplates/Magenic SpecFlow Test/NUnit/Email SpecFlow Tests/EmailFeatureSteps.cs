using SpecFlowMAQSExtension.TestSteps;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    /// <summary>
    /// Steps class for EmailFeatureSteps
    /// To utilize MAQS features for the steps in this class, make sure at add a 'MAQS_Email' tag to the feature file(s)
    /// </summary>
    [Binding]
    public class EmailFeatureSteps : BaseEmailTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailFeatureSteps"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected EmailFeatureSteps(ScenarioContext context) : base(context)
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
    }
}
