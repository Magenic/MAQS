using Magenic.Maqs.SpecFlow.TestSteps;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PageModel;
using System;
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
        /// Login with a specify user type
        /// </summary>
        /// <param name="userType">The user type</param>
        [Given(@"I login as the '(.*)' user")]
        public void GivenILoginAsTheUser(string userType)
        {
            LoginPageModel model = new LoginPageModel(this.TestObject);
            model.OpenLoginPage();
            HomePageModel home = model.LoginWithValidCredentials(GetUserName(userType), GetUserPass(userType));
            this.LocalScenarioContext.Add("home", home);
        }

        /// <summary>
        /// Verify that we are on the home page
        /// </summary>
        [Then(@"The home page is loaded")]
        public void ThenTheHomePageIsLoaded()
        {
            Assert.IsTrue(this.LocalScenarioContext.Get<HomePageModel>("home").IsPageLoaded(), "Page was not loaded");
        }

        /// <summary>
        /// Get the user name for the specified user type
        /// </summary>
        /// <param name="typeOfUser">The user tpe</param>
        /// <returns>The user name</returns>
        private string GetUserName(string typeOfUser)
        {
            switch (typeOfUser.ToLower().Trim())
            {
                case "standard":
                    return Config.GetGeneralValue("standardUserName");
                default:
                    throw new ArgumentException($"'{typeOfUser}' is not a valid user type");
            }
        }

        /// <summary>
        /// Get the userpassword for the specified user type
        /// </summary>
        /// <param name="typeOfUser">The user tpe</param>
        /// <returns>The user password</returns>
        private string GetUserPass(string typeOfUser)
        {
            switch (typeOfUser.ToLower().Trim())
            {
                case "standard":
                    return Config.GetGeneralValue("standardUserPassword");
                default:
                    throw new ArgumentException($"'{typeOfUser}' is not a valid user type");
            }

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

