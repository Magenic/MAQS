using Magenic.Maqs.SpecFlow.TestSteps;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using TechTalk.SpecFlow;

namespace $safeprojectname$.Steps
{
    /// <summary>
    /// Steps class for SeleniumSteps
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
        /// Login with the default user
        /// </summary>
        [Given(@"I login with a valid user")]
        public void GivenILoginWithAValidUser()
        {
            string username = Config.GetGeneralValue("DefaultUser");
            string password = Config.GetGeneralValue("DefaultPassword");

            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();

            HomePageModel homepage = page.LoginWithValidCredentials(username, password);
            this.LocalScenarioContext.Set(homepage);
        }

        [Then(@"I am logged into the home page")]
        public void ThenIAmLoggedIntoTheHomePage()
        {
            Assert.IsTrue(this.LocalScenarioContext.Get<HomePageModel>().IsPageLoaded(), "The home page was not loaded");
        }

        [Given(@"I login with a invalid user")]
        public void GivenILoginWithAInvalidUser()
        {
            LoginPageModel page = new LoginPageModel(this.TestObject);
            page.OpenLoginPage();

            bool foundErrorMessage = page.LoginWithInvalidCredentials("badName", "badPass");

            this.LocalScenarioContext.Set(foundErrorMessage, "ErrorMessage");
        }

        [Then(@"An error message is displayed")]
        public void ThenAnErrorMessageIsDisplayed()
        {
            Assert.IsTrue(this.LocalScenarioContext.Get<bool>("ErrorMessage"), "Login error message was not diplayed");
        }
    }
}
