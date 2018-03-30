using SpecFlowMAQSExtension.TestSteps;
using System.Data;
using TechTalk.SpecFlow;

namespace $safeprojectname$.Steps
{
    /// <summary>
    /// Steps class for DatabaseFeatureSteps
    /// To utilize MAQS features for the steps in this class, make sure at add a 'MAQS_Database' tag to the feature file(s)
    /// </summary>
    [Binding]
    public class DatabaseFeatureSteps : BaseDatabaseTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseFeatureSteps"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected DatabaseFeatureSteps(ScenarioContext context) : base(context)
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
        /// Queries the database and returns the table
        /// </summary>
        /// <param name="query">The query string</param>
        /// <returns>The data table returned</returns>
        private DataTable QueryDatabase(string query)
        {
            return this.TestObject.DatabaseWrapper.QueryAndGetDataTable(query);
        }
    }
}
