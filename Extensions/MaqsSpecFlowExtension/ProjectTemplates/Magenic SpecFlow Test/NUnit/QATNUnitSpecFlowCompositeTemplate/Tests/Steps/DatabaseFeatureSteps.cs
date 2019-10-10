using Magenic.Maqs.SpecFlow.TestSteps;
using NUnit.Framework;
using System.Linq;
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
        /// Queries the database and returns if the expected table exists
        /// </summary>
        /// <returns>True if the state table is found</returns>
        private bool QueryDatabase()
        {
            var table = this.DatabaseDriver.Query("SELECT * FROM information_schema.tables").ToList();
            return table.Any(n => n.TABLE_NAME.Equals("States"));
        }

        //// Store objects
        //[Given(@"initial")]
        //public void GivenInitial()
        //{
        //    OBJECTTYPE statefulObjectName = new OBJECTTYPE(); 
        //    this.LocalScenarioContext.Set(statefulObjectName);

        //    OBJECTTYPE statefulObjectName2 = new OBJECTTYPE();
        //    this.LocalScenarioContext.Add("SpecificName", statefulObjectName2);
        //}

        //// Get objects
        //[When(@"later")]
        //public void WhenLater()
        //{
        //    OBJECTTYPE statefulObjectName = this.LocalScenarioContext.Get<OBJECTTYPE>();
        //    OBJECTTYPE statefulObjectName2 = this.LocalScenarioContext.Get<OBJECTTYPE>("SpecificName");
        //}
    }
}
