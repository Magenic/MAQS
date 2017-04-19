//-----------------------------------------------------
// <copyright file="SeleniumUnitTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test the selenium framework</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the selenium framework
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumUnitTest : BaseSeleniumTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Unit testing site URL - Login page
        /// </summary>
        private static string testSiteUrl = SeleniumConfig.GetWebSiteBase();

        /// <summary>
        /// Unit testing site URL - Async page
        /// </summary>
        private static string testSiteAsyncUrl = testSiteUrl + "Automation/AsyncPage";

        /// <summary>
        /// Unit testing site URL - Automation page
        /// </summary>
        private static string testSiteAutomationUrl = testSiteUrl + "Automation/";

        /// <summary>
        /// Unit testing site URL - About page
        /// </summary>
        private static string testSiteAboutUrl = testSiteUrl + "Home/Contact/";

        /// <summary>
        /// Home button css selector
        /// </summary>
        private static By homeButtonCssSelector = By.CssSelector("#homeButton > a");

        /// <summary>
        /// Dropdown selector
        /// </summary>
        private static By asyncDropdownCssSelector = By.CssSelector("#Selector");

        /// <summary>
        /// Dropdown label
        /// </summary>
        private static By asyncOptionsLabel = By.CssSelector("#Label");

        /// <summary>
        /// Dropdown label - hidden once dropdown loads
        /// </summary>
        private static By asyncLoadingLabel = By.CssSelector("#LoadingLabel");

        /// <summary>
        /// Asynchronous div that loads after a delay on Async Testing Page
        /// </summary>
        private static By asyncLoadingTextDiv = By.CssSelector("#loading-div-text");

        /// <summary>
        /// Names label
        /// </summary>
        private static By automationNamesLabel = By.CssSelector("#Dropdown > p > strong > label");

        /// <summary>
        /// Selector that is not in page
        /// </summary>
        private static By notInPage = By.CssSelector("NOTINPAGE");

        /// <summary>
        /// First dialog button
        /// </summary>
        private static By automationShowDialog1 = By.CssSelector("#showDialog1");

        /// <summary>
        /// Text fields
        /// </summary>
        private static By textFields = By.CssSelector("#TextFields");

        /// <summary>
        /// Food table
        /// </summary>
        private static By foodTable = By.CssSelector("#FoodTable");

        /// <summary>
        /// Flower table
        /// </summary>
        private static By flowerTable = By.CssSelector("#FlowerTable TD");

        /// <summary>
        /// Setup before running tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void OpenBrowser()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
        }

        /// <summary>
        /// Verify WaitForClickableElement wait works
        /// </summary>
        #region WaitForClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForClickableElement(homeButtonCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForVisibleElement wait works
        /// </summary>
        #region WaitForVisible
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForVisibleElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForVisibleElement(asyncDropdownCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForExactText wait works
        /// </summary>
        #region WaitForExactText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForExactText()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            IWebElement element = this.WebDriver.Wait().ForExactText(asyncOptionsLabel, "Options");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForContainsText wait works
        /// </summary>
        #region WaitForContainsText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForContainsText()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Wait().ForContainsText(automationNamesLabel, "Name");
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitForAbsentElement wait works
        /// </summary>
        #region WaitForAbsentElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAbsentElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForAbsentElement(notInPage);
        }
        #endregion

        /// <summary>
        /// Verify WaitForAbsentElement wait fails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception))]
        public void WaitForAbsentElementFail()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Wait().ForAbsentElement(homeButtonCssSelector);
        }

        /// <summary>
        /// Verify WaitForPageLoad wait works
        /// </summary>
        #region WaitForPageLoad
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForPageLoad()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilPageLoad wait works
        /// </summary>
        #region WaitUntilPageLoad
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilPageLoad()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilPageLoad(), "Page failed to load");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilClickableElement wait works
        /// </summary>
        #region WaitUntilClickable
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilClickableElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilClickableElement(automationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilVisibleElement wait works
        /// </summary>
        #region WaitUntilVisible
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilVisibleElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilVisibleElement(automationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilExactText wait works
        /// </summary>
        #region WaitUntilExact
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilExactText()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilExactText(automationShowDialog1, "Show dialog"), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilContainsText wait works
        /// </summary>
        #region WaitUntilContains
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilContainsText()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilContainsText(automationShowDialog1, "dialog"), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Verify findElement works - validating a specific selector exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            Assert.IsNotNull(this.WebDriver.FindElement(homeButtonCssSelector), "Element was not found");
        }

        /// <summary>
        /// Verify WaitForElementExist wait works
        /// </summary>
        #region WaitForExist
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void WaitForElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            IWebElement element = this.WebDriver.Wait().ForElementExist(homeButtonCssSelector);
            Assert.IsNotNull(element, "Null element was returned");
        }
        #endregion

        /// <summary>
        /// Verify WaitUntilElementExist wait works
        /// </summary>
        #region WaitUntilExist
        [TestMethod]
        [TestCategory("Selenium Unit Tests")]
        public void WaitUntilElementExist()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilElementExist(automationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test for the wait until absent
        /// </summary>
        #region WaitUntilAbsent
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAbsentElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilAbsentElement(notInPage));
        }
        #endregion

        /// <summary>
        /// Verify ElemList throws an exception when an Element is not on the page
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void ElemListThrowException()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Find().Element(notInPage);
        }

        /// <summary>
        /// Verify findElement works - validating a specific selector is not found
        /// </summary>
        #region FindElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            Assert.IsNull(this.WebDriver.Find().Element(notInPage, false), "Element was not found");
        }
        #endregion

        /// <summary>
        /// Verify findElement works - validating a specific selector is found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebElement element = this.WebDriver.Find().Element(automationNamesLabel);
            Assert.AreEqual(element.Text, "Names");
        }

        /// <summary>
        /// Verify FindElementWithText = Validate null is returned if the element is not found
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithTextElementNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsNull(this.WebDriver.Find().ElementWithText(notInPage, "notInPage", false), "Element was not found");
        }

        /// <summary>
        /// Verify FindElementWithText - Validating specific text is found within a specific selector
        /// </summary>
        #region FindElementWithText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithText()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            string text = this.WebDriver.FindElement(automationShowDialog1).Text;
            Assert.IsNotNull(this.WebDriver.Find().ElementWithText(automationShowDialog1, text), "Element was not found");
        }
        #endregion

        /// <summary>
        /// Verify FindElementWithText - Validating specific text is NOT found within a specific selector
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindElementWithTextNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            Assert.IsNull(this.WebDriver.Find().ElementWithText(homeButtonCssSelector, "#notfound", false), "Element was not found");
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating the correct index is returned for a specific Selector and text
        /// </summary>
        #region FindIndexFromText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithText()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(flowerTable, "Red"), 3);
        }
        #endregion

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating index is not returned for a specific Selector and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithTextNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(flowerTable, "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validate that index of -1 is returned if an empty list is returned by ElemList
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementWithTextWithNotFoundElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(notInPage, "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating the correct index is returned for a specific collection and text
        /// </summary>
        #region FindIndexWithText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexofElementInCollection()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(this.WebDriver.FindElements(flowerTable), "10 in"), 0);
        }
        #endregion

        /// <summary>
        /// Verify FindIndexOfElementWithText works - Validating -1 is returned for a specific collection and text
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FindIndexOfElementInCollectionNotFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.AreEqual(this.WebDriver.Find().IndexOfElementWithText(this.WebDriver.FindElements(flowerTable), "#notfound", false), -1);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - NotFoundException is thrown when an Empty input list is entered with assert == true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindIndexOfElementInCollectionEmptyInputList()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.WebDriver.Find().IndexOfElementWithText(new List<IWebElement>(), "#notfound", true);
        }

        /// <summary>
        /// Verify FindIndexOfElementWithText works - NotFoundException is thrown when the element is not found and assert == true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException))]
        public void FindIndexOfElementInCollectionTextNotFoundAssertIsTrue()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.WebDriver.Find().IndexOfElementWithText(this.WebDriver.FindElements(flowerTable), "#notfound", true);
        }

        /// <summary>
        /// Verify CaptureScreenshot works - Validating that the screenshot was created
        /// </summary>
        #region CaptureScreenshot
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshot()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.Log);
            string filePath = Path.ChangeExtension(((FileLogger)this.Log).FilePath, ".Jpeg");
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            File.Delete(filePath);
        }
        #endregion

        /// <summary>
        /// Verify CaptureScreenshot works with console logger - Validating that the screenshot was created
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshotWithConsoleLogger()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();

            // Create a console logger and calculate the file location
            ConsoleLogger consoleLogger = new ConsoleLogger();
            string expectedPath = Path.Combine(LoggingConfig.GetLogDirectory(), "ScreenCapDelete.Jpeg");

            // Take a screenshot
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, consoleLogger, "Delete");

            // Make sure we got the screenshot and than cleanup
            Assert.IsTrue(File.Exists(expectedPath), "Fail to find screenshot");
            File.Delete(expectedPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot properly handles exceptions and returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotThrownException()
        {
            FileLogger tempLogger = new FileLogger();
            tempLogger.FilePath = "<>"; // illegal file path

            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            bool successfullyCaptured = SeleniumUtilities.CaptureScreenshot(this.WebDriver, tempLogger);
            Assert.IsFalse(successfullyCaptured);
        }

        /// <summary>
        /// Verify that CaptureScreenshot creates Directory if it does not exist already 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotNoExistingDirectory()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(this.WebDriver, "TempTestDirectory", "TempTestFilePath", LoggingConfig.GetScreenShotFormat());
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        #region SoftAssertAreEqual
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertTest()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            SoftAssert.AreEqual("Automation - Magenic Automation Test Site", this.WebDriver.Title, "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        #region SoftAssertIsFalse
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTest()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            SoftAssert.IsFalse("Automation".Equals(this.WebDriver.Title), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Method to check for soft asserts
        /// </summary>
        #region SoftAssertIsTrue
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueTest()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            SoftAssert.IsTrue(this.WebDriver.Title.Contains("Automation"), "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Verify that a screenshot is taken if the SeleniumSoftAssert.IsTrue gets a false condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsTrueFalseCondition()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.Log = new FileLogger(string.Empty, "SeleniumSoftAssertIsTrueFalseConditionFileLog.txt", MessageType.GENERIC, true);
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.WebDriver, this.Log);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " (1).Jpeg";

            bool isFalse = seleniumSoftAssert.IsTrue(false, "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(screenShotLocation), "Fail to find screenshot");
            File.Delete(screenShotLocation);
            File.Delete(logLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that a screenshot is taken if the SeleniumSoftAssert.IsFalse gets a true condition and the logger is set to log screenshots
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertIsFalseTrueCondition()
        {
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.WebDriver, this.Log);
            string logLocation = ((FileLogger)this.Log).FilePath;
            string screenShotLocation = logLocation.Substring(0, logLocation.LastIndexOf('.')) + " (1).Jpeg";

            bool isFalse = seleniumSoftAssert.IsFalse(true, "testSoftAssert", "message");

            Assert.IsTrue(File.Exists(screenShotLocation), "Fail to find screenshot");
            File.Delete(screenShotLocation);

            Assert.IsFalse(isFalse);
        }

        /// <summary>
        /// Verify that SeleniumSoftAssert.AreEqual works as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSoftAssertAreEqual()
        {
            SeleniumSoftAssert seleniumSoftAssert = new SeleniumSoftAssert(this.WebDriver, this.Log);
            bool isTrue = seleniumSoftAssert.AreEqual("test string", "test string", "test message");
            Assert.IsTrue(isTrue);
        }

        /// <summary>
        /// Test WebElementToDriver with an unwrappedDriver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WebElementToWebDriverUnwrappedDriver()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            IWebDriver driver = ((IWrapsDriver)this.WebDriver).WrappedDriver;
            IWebElement element = driver.FindElement(automationShowDialog1);
            
            IWebDriver basedriver = SeleniumUtilities.WebElementToWebDriver(element);
            Assert.AreEqual("OpenQA.Selenium.PhantomJS.PhantomJSDriver", basedriver.GetType().ToString());
        }

        /// <summary>
        /// Test to check if the soft assert fails.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(AggregateException))]
        public void SeleniumSoftAssertExpectFail()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            SoftAssert.AreEqual("Wrong Title", this.WebDriver.Title, "Title Test", "Title is incorrect");
            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify that WaitForAttributeTextContains throws an exception for instances where the attribute is not found to contain a value.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string within an elements attribute passed.")]
        public void WaitForAttributeContainsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.WebDriver.Wait().ForAttributeTextContains(foodTable, "Flower Table", "Summmary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextEquals throws an exception for instances where the attribute is not found.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(NotFoundException), "An attribute check that should have failed to find the given string equal to an elements attribute passed.")]
        public void WaitForAttributeEqualsDontFind()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.WebDriver.Wait().ForAttributeTextEquals(foodTable, "Flower Table", "Summmary");
        }

        /// <summary>
        /// Verify that WaitForAttributeTextContains can find text within attribute after waiting.
        /// </summary>
        #region WaitForAttributeContains
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeContainsFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            Assert.IsNotNull(this.WebDriver.Wait().ForAttributeTextContains(asyncLoadingTextDiv, "block;", "style"));
        }
        #endregion

        /// <summary>
        /// Verify that WaitForAttributeTextEquals can find an attribute value after waiting.
        /// </summary>
        #region WaitForAttributeEquals
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitForAttributeEqualsFound()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);

            Assert.IsNotNull(this.WebDriver.Wait().ForAttributeTextEquals(asyncLoadingTextDiv, "display: block;", "style"));
        }
        #endregion

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeContains()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilAttributeTextContains(asyncLoadingLabel, "none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextEquals works with async objects
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEquals()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilAttributeTextEquals(asyncLoadingLabel, "display: none;", "style"));
        }

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this text inside attribute value within timeout.
        /// </summary>
        #region WaitUntilAttributeContains
        [TestCategory(TestCategories.Selenium)]
        [TestMethod]
        public void WaitUntilAttributeContainsFalse()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            Assert.IsFalse(this.WebDriver.Wait().UntilAttributeTextContains(asyncDropdownCssSelector, "nottherightid", "id"));
        }
        #endregion

        /// <summary>
        /// Verify that the WaitUntilAttributeTextContains method returns false for objects that don't have this attribute value within timeout.
        /// </summary>
        #region WaitUntilAttributeEquals
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WaitUntilAttributeEqualsFalse()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAsyncUrl);
            Assert.IsFalse(this.WebDriver.Wait().UntilAttributeTextEquals(asyncLoadingLabel, "display:", "style"));
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        #region WaitForAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElementAndScrollIntoView(automationShowDialog1).Displayed, "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view element
        /// </summary>
        #region WaitForClickableAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewWithOffsetElement()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().ForClickableElementAndScrollIntoView(automationShowDialog1, 0, 100).Displayed, "Failed to find element or scroll");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view boolean
        /// </summary>
        #region WaitUntilClickableAndScroll
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewBoolean()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilClickableElementAndScrollIntoView(automationShowDialog1), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test method to check the element can scroll into the view boolean
        /// </summary>
        #region WaitUntilClickableAndScrollWithOffset
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollIntoViewBooleanOffset()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            Assert.IsTrue(this.WebDriver.Wait().UntilClickableElementAndScrollIntoView(automationShowDialog1, 0, 100), "Failed to find element");
        }
        #endregion

        /// <summary>
        /// Test method to scroll to an offset
        /// </summary>
        #region ExecuteScrolling
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void ScrollByOffset()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.WebDriver.Wait().PageLoadThanExecuteScrolling(0, 500);
        }
        #endregion

        /// <summary>
        /// Test for the get wait driver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriver()
        {
            #region GetWaitDriver
            WebDriverWait driver = this.WebDriver.GetWaitDriver();
            #endregion
            Assert.AreEqual(driver.Timeout.Seconds, 10);
            Assert.AreEqual(driver.PollingInterval.Seconds, 1);
        }

        /// <summary>
        ///  Test for resetting the wait driver
        /// </summary>
        #region ResetWaitDriver
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriverResetWaitDriver()
        {
            WebDriverWait defaultWaitDriver = SeleniumConfig.GetWaitDriver(this.WebDriver); // default waitdriver
            this.WebDriver.ResetWaitDriver();
            WebDriverWait resetWaitDriver = this.WebDriver.GetWaitDriver(); // webdrivers default webdriver

            Assert.AreEqual(defaultWaitDriver.Timeout, resetWaitDriver.Timeout);
            Assert.AreEqual(defaultWaitDriver.Message, resetWaitDriver.Message);
            Assert.AreEqual(defaultWaitDriver.PollingInterval, resetWaitDriver.PollingInterval);
        }
        #endregion

        /// <summary>
        /// Test for GetWaitDriver - test for waitDriver not in the waitCollection 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetWaitDriverNotInWaitCollection()
        {
            bool removeDriver = this.WebDriver.RemoveWaitDriver();
            WebDriverWait driver = this.WebDriver.GetWaitDriver();

            Assert.AreEqual(driver.Timeout.Seconds, 10);
            Assert.AreEqual(driver.PollingInterval.Seconds, 1);
        }

        /// <summary>
        /// Test for setting the wait driver
        /// </summary>
        #region SetWaitDriver
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SetWaitDriver()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(10000), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Wait().ForPageLoad();
        }
        #endregion

        /// <summary>
        /// Test for removing the wait driver
        /// </summary>
        #region RemoveWaitDriver
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void RemoveWaitDriver()
        {
            this.WebDriver.SetWaitDriver(new WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromMilliseconds(10000), TimeSpan.FromMilliseconds(10)));
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            bool removed = this.WebDriver.RemoveWaitDriver();
            Assert.IsTrue(removed == true);
        }
        #endregion

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.WebDriver, this.WebDriver, "Web drivers don't match");
        }

        /// <summary>
        /// Make sure test object values are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumTestObjectValuesCanBeUsed()
        {
            this.TestObject.SetValue("1", "one");

            Assert.AreEqual(this.TestObject.Values["1"], "one");
            string outValue;
            Assert.IsFalse(this.TestObject.Values.TryGetValue("2", out outValue), "Didn't expect to get value for key '2', but got " + outValue);
        }

        /// <summary>
        /// Make sure the test object objects are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumTestObjectObjectssCanBeUsed()
        {
            StringBuilder builder = new StringBuilder();
            this.TestObject.SetObject("1", builder);

            Assert.AreEqual(this.TestObject.Objects["1"], builder);

            object outObject;
            Assert.IsFalse(this.TestObject.Objects.TryGetValue("2", out outObject), "Didn't expect to get value for key '2'");

            builder.Append("123");

            Assert.AreEqual(((StringBuilder)this.TestObject.Objects["1"]).ToString(), builder.ToString());
        }

        /// <summary>
        /// Verify that BaseSeleniumTest.Dispose() properly closes existing TestObject, and thrown exception is caught if driver is closed already
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumDispose()
        {
            this.Dispose();
            var webdriver = this.WebDriver;
            Assert.AreEqual(webdriver.WindowHandles.Count, 0);

            this.Dispose(); // calls Dispose again to throw exception, which is supressed
        }

        /// <summary>
        /// Verify that BaseSeleniumTest.GetSoftAssert() returns the BaseTest.SoftAssert if there is no existing TestObject
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [TestCategory(TestCategories.Utilities)]
        public void SeleniumGetSoftAssertBase()
        {
            this.Teardown(); // delete the existing TestObject
            SoftAssert baseSoftAssert = this.GetSoftAssert();
            Assert.AreEqual(baseSoftAssert.ToString(), "Magenic.MaqsFramework.BaseTest.SoftAssert");
        }

        /// <summary>
        /// Verify that PostSetupLogging catches thrown exception if there is no existing TestObject
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumPostSetupLoggingThrownException()
        {
            this.Teardown(); // delete the existing TestObject
            this.Log = new FileLogger(string.Empty, "SeleniumPostSetupLoggingThrowExceptionFileLog.txt", MessageType.GENERIC, true);
            this.PostSetupLogging(); // exception is caught

            string log = this.GetAndRemoveCustomFileLog();
            Assert.IsTrue(log.Contains("Failed to start driver because: "));
        }

        /// <summary>
        /// Verify That BeforeLoggingTeardown catches thrown exception if there is no existing TestObject
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumBeforeLoggingTeardownThrowException()
        {
            this.Teardown();
            TestResultType trt = TestResultType.FAIL;
            this.Log = new FileLogger(string.Empty, "SeleniumBeforeLoggingTeardownThrowExceptionFileLog.txt", MessageType.GENERIC, true);
            this.BeforeLoggingTeardown(trt);

            string log = this.GetAndRemoveCustomFileLog();
            Assert.IsTrue(log.Contains("Failed to get screen shot because: "));
        }

        /// <summary>
        /// Verify that SetupNoneEventFiringTester sets the proper webdriver
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumSetupNoneEventFiringTester()
        {
            this.SetupNoneEventFiringTester();
            IWebDriver browser = SeleniumConfig.Browser();
            Assert.AreEqual(browser.ToString(), this.WebDriver.ToString());
        }

        /// <summary>
        /// Verify that CreateNewTestObject creates the correct Test Object 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumCreateNewTestObject()
        {
            this.CreateNewTestObject();
            SeleniumTestObject newTestObject = this.TestObject;

            Assert.AreEqual(this.WebDriver.ToString(), newTestObject.WebDriver.ToString());
            Assert.AreEqual(this.Log.ToString(), newTestObject.Log.ToString());
            Assert.AreEqual(this.SoftAssert.ToString(), newTestObject.SoftAssert.ToString());
            Assert.AreEqual(this.PerfTimerCollection.ToString(), newTestObject.PerfTimerCollection.ToString());
        }

        /// <summary>
        /// Verify that Selenium Navigation Event Listeners log proper messages
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void SeleniumNavigationEventListeners()
        {
            this.Log = new FileLogger(string.Empty, "SeleniumNavigatorEventListenersFileLog.txt", MessageType.VERBOSE, true);
            this.WebDriver.Navigate().GoToUrl(testSiteAutomationUrl);
            this.WebDriver.Navigate().Back();
            this.WebDriver.Navigate().Forward();

            string log = this.GetAndRemoveCustomFileLog();
            Assert.IsTrue(log.Contains("Navigating to: "));
            Assert.IsTrue(log.Contains("Navigating back to: "));
            Assert.IsTrue(log.Contains("Navigated Forward: "));
            Assert.IsTrue(log.Contains("Navigated back: "));
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the bitmap image format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotBmpFormat()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(this.WebDriver, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Bmp);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(screenShotPath), ".Bmp", "The screenshot format was not in '.Bmp' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Graphics Interchange Format format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotGifFormat()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(this.WebDriver, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Gif);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(screenShotPath), ".Gif", "The screenshot format was not in '.Gif' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Joint Photographic Experts Group format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotJpegFormat()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(this.WebDriver, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Jpeg);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(screenShotPath), ".Jpeg", "The screenshot format was not in '.Jpeg' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Portable Network Graphics format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotPngFormat()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(this.WebDriver, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Png);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(screenShotPath), ".Png", "The screenshot format was not in '.Png' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify that CaptureScreenshot captured is in the Tagged Image File Format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void CaptureScreenshotTiffFormat()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            string screenShotPath = SeleniumUtilities.CaptureScreenshot(this.WebDriver, "TempTestDirectory", "TempTestFilePath", ScreenshotImageFormat.Tiff);
            Assert.IsTrue(File.Exists(screenShotPath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(screenShotPath), ".Tiff", "The screenshot format was not in '.Tiff' format");
            File.Delete(screenShotPath);
        }

        /// <summary>
        /// Verify CaptureScreenshot works - With Modified ImageFormat Config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void TryScreenshotImageFormat()
        {
            this.WebDriver.Navigate().GoToUrl(testSiteUrl);
            this.WebDriver.Wait().ForPageLoad();
            SeleniumUtilities.CaptureScreenshot(this.WebDriver, this.Log);
            string filePath = Path.ChangeExtension(((FileLogger)this.Log).FilePath, Config.GetValue("ImageFormat").ToString());
            Assert.IsTrue(File.Exists(filePath), "Fail to find screenshot");
            Assert.AreEqual(Path.GetExtension(filePath), "." + Config.GetValue("ImageFormat").ToString(), "The screenshot format was not in correct Format format");
            File.Delete(filePath);
        }

        /// <summary>
        /// Verify the GetScreenShotFormat function has default value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetImageFormatDefaultFormat()
        {
            Assert.AreEqual(LoggingConfig.GetScreenShotFormat("XXYYZZ"), ScreenshotImageFormat.Png, "The Incorrect Default Image Format was returned, expected: " + ScreenshotImageFormat.Png.ToString());
        }

        /// <summary>
        /// Verify the GetScreenShotFormat function gets the correct value from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void GetImageFormatFromConfig()
        {
            Assert.AreEqual(LoggingConfig.GetScreenShotFormat(), ScreenshotImageFormat.Jpeg, "The Incorrect Image Format was returned, expected: " + Config.GetValue("ImageFormat"));
        }

        /// <summary>
        /// Helper function to remove created custom file logs and return contained string
        /// </summary>
        /// <returns>string contained in custom log</returns>
        protected string GetAndRemoveCustomFileLog()
        {
            FileLogger outputLog = (FileLogger)this.Log;
            string log = File.ReadAllText(outputLog.FilePath);
            try
            {
                File.Delete(outputLog.FilePath);
                string screenShotLocation = outputLog.FilePath.Substring(0, outputLog.FilePath.LastIndexOf('.')) + ".png";
                File.Delete(screenShotLocation);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine("File not found exception " + e);
            }

            this.Log = new FileLogger();
            return log;
        }
    }
}