using Magenic.Maqs.SpecFlow.TestSteps;
using Models.WebService;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace MaqsSpecFlowCompositeDemo.Steps
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
        /// Given I get the product for a 
        /// </summary>
        /// <param name="id"></param>
        [Given(@"I get product XML (.*)")]
        public void GivenIGetProductXML(int id)
        {
            Product result = this.TestObject.WebServiceDriver.Get<Product>($"/api/XML_JSON/GetProduct/{id}", "application/xml", false);
            this.LocalScenarioContext.Set(result);
            
        }

        [Given(@"I get product JSON (.*)")]
        public void GivenIGetProductJSON(int id)
        {
            ProductJson result = this.TestObject.WebServiceDriver.Get<ProductJson>($"/api/XML_JSON/GetProduct/{id}", "application/json", false);
            this.LocalScenarioContext.Set<Product>(result);
        }


        [Then(@"The result ID is (.*)")]
        public void ThenTheResultIDIs(int id)
        {
            Assert.AreEqual(id, this.LocalScenarioContext.Get<Product>().Id, $"Expected to get product {id}");
        }
    }
}
