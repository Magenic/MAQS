using Magenic.Maqs.SpecFlow.TestSteps;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using PageModel;
using System;
using TechTalk.SpecFlow;

namespace $safeprojectname$
{
    /// <summary>
    /// Steps class for AppiumFeatureSteps
    /// To utilize MAQS features for the steps in this class, make sure at add a 'MAQS_Appium' tag to the feature file(s)
    /// </summary>
    [Binding]
    public class AppiumFeatureSteps : BaseAppiumTestSteps
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppiumFeatureSteps"/> class.
        /// </summary>
        /// <param name="context">The scenario context.</param>
        protected AppiumFeatureSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Login in as a specific user type
        /// </summary>
        /// <param name="userType">The type of user</param>
        [Given(@"I login as the '(.*)' user")]
        public void GivenILogInAsTheUser(string userType)
        {
            ALoginPageModel startingPage = GetLoginPage();
            AHomePageModel homePage = startingPage.LoginWithValidCredentials(GetUserName(userType), GetUserPass(userType));
            this.LocalScenarioContext.Set(homePage);
        }

        /// <summary>
        /// Verify the home page is loaded
        /// </summary>
        [Then(@"The home page is loaded")]
        public void ThenTheHomePageIsLoaded()
        {
            Assert.IsFalse(string.IsNullOrEmpty(this.LocalScenarioContext.Get<AHomePageModel>().GetGreetingMessage()), "Expected a welcome message on load");
        }

        /// <summary>
        /// Get the login page
        /// </summary>
        /// <returns>The login page</returns>
        private ALoginPageModel GetLoginPage()
        {
            Type driverType = this.TestObject.AppiumDriver.GetType();
            if (driverType.Name == typeof(IOSDriver<AppiumWebElement>).Name)
            {
                return new IOSLoginPageModel(this.TestObject);
            }
            else if (driverType.Name == typeof(AndroidDriver<AppiumWebElement>).Name)
            {
                return new AndroidLoginPageModel(this.TestObject);
            }
            else
            {
                throw new NotSupportedException($"This OS type: {driverType.ToString()} is not supported.");
            }
        }

        /// <summary>
        /// Get the user name for the given type
        /// </summary>
        /// <param name="typeOfUser">The user type</param>
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
        /// Get the user password for the given type
        /// </summary>
        /// <param name="typeOfUser">The user type</param>
        /// <returns>The password</returns>
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
