using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Magenic.Maqs.SpecFlow.TestSteps;
using TechTalk.SpecFlow;
using OpenQA.Selenium.Appium.iOS;
using System;
using Models.Appium;
using OpenQA.Selenium.Appium.Android;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.BaseTest;
using NUnit.Framework;

namespace MaqsSpecFlowCompositeDemo.Steps
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

        [Given(@"I launch the app")]
        public void GivenILaunchTheApp()
        {
            // Test cannot pass until you connect it to a mobile device
            Assert.Inconclusive("Test cannot pass until you connect it to a mobile device");

            ALoginPageModel startingPage;

            Type driverType = this.TestObject.AppiumDriver.GetType();
            if (driverType.Name == typeof(IOSDriver<AppiumWebElement>).Name)
            {
                startingPage = new IOSLoginPageModel(this.TestObject);
            }
            else if (driverType.Name == typeof(AndroidDriver<AppiumWebElement>).Name)
            {
                startingPage = new AndroidLoginPageModel(this.TestObject);
            }
            else
            {
                throw new NotSupportedException($"This OS type: {driverType.ToString()} is not supported.");
            }

            this.LocalScenarioContext.Set(startingPage);
        }

        [When(@"I login as a valid user")]
        public void WhenILoginAsAValidUser()
        {
            string username = Config.GetGeneralValue("DefaultUser");
            string password = Config.GetGeneralValue("DefaultPassword");

            this.LocalScenarioContext.Set(this.LocalScenarioContext.Get<ALoginPageModel>().LoginWithValidCredentials(username, password));
            this.LocalScenarioContext.Set(username, "UserName");
        }

        [Then(@"the homepage is loaded")]
        public void ThenTheHomepageIsLoaded()
        {
            AHomePageModel homePage = this.LocalScenarioContext.Get<AHomePageModel>();
            string userName = this.LocalScenarioContext.Get<string>("UserName");
            string expectedGreeting = $"Welcome {userName}!";
            string expectedTimeDescription = "The current time is:";
            SoftAssert softAssert = this.TestObject.SoftAssert;

            this.TestObject.SoftAssert.AreEqual(expectedGreeting, homePage.GetGreetingMessage());
            softAssert.AreEqual(expectedTimeDescription, homePage.GetTimeDiscription());
            softAssert.IsTrue(DateTime.TryParse(homePage.GetTime(), out DateTime time), "Time Parsing");
            softAssert.Assert(() => Assert.IsTrue(time.Ticks > DateTime.MinValue.Ticks, "MinTime"));

            softAssert.FailTestIfAssertFailed();
        }
    }
}
