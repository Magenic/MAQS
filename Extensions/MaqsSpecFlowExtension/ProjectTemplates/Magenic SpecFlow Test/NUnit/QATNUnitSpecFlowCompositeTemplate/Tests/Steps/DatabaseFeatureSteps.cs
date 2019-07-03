using Magenic.Maqs.SpecFlow.TestSteps;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace MaqsSpecFlowCompositeDemo.Steps
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


        [Given(@"I query the information schema for tables")]
        public void GivenIQueryTheInformationSchemaForTables()
        {
            // Test cannot pass until you connect it to a real database
            Assert.Inconclusive("Test cannot pass until you connect it to a real database");

            this.LocalScenarioContext.Set(this.TestObject.DatabaseDriver.Query("SELECT * FROM information_schema.tables"), "tables");
        }

        [Then(@"I get back a table count of (.*)")]
        public void ThenIGetBackATableCountOf(int count)
        {
            Assert.AreEqual(count, this.LocalScenarioContext.Get<IEnumerable<dynamic>>("tables").Count(), $"Expected {count} tables");
        }
    }
}
