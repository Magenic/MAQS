using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.SpecFlow.TestSteps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace MaqsSpecFlowCompositeDemo.Steps
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

        [Given(@"I connect to the email server")]
        public void GivenIConnectToTheEmailServer()
        {
            // Test cannot pass until you connect it to a real email
            Assert.Inconclusive();

            this.LocalScenarioContext.Set(this.TestObject.EmailDriver);
            Assert.IsTrue(this.TestObject.EmailDriver.CanAccessEmailAccount(), "Email account was not accessible");
        }

        [Then(@"I can access the email account")]
        public void ThenICanAccessTheEmailAccount()
        {
            Assert.IsTrue(this.LocalScenarioContext.Get<EmailDriver>().CanAccessEmailAccount(), "Email account was not accessible");
        }
    }
}
