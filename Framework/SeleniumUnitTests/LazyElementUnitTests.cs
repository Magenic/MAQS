//-----------------------------------------------------
// <copyright file="LazyElementUnitTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Test the lazy element unit tests</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Unit test class for LazyElement
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LazyElementUnitTests : BaseSeleniumTest
    {
        /// <summary>
        /// Gets the div root
        /// </summary>
        private LazyElement DivRoot
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#ItemsToAutomate"), "Div Root"); }
        }

        /// <summary>
        /// Gets the disabled item
        /// </summary>
        private LazyElement DisabledItem
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#disabledField INPUT"), "Disabled"); }
        }

        /// <summary>
        /// Gets an item that does not exist
        /// </summary>
        private LazyElement MissingItem
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#DONOTFINDME"), "Missing element"); }
        }

        /// <summary>
        /// Gets the input box
        /// </summary>
        private LazyElement InputBox
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#TextFields [name='firstname']"), "Input box"); }
        }

        /// <summary>
        /// Gets dialog one
        /// </summary>
        private LazyElement DialogOne
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#CloseButtonShowDialog"), "Dialog"); }
        }

        /// <summary>
        /// Gets the dialog one button
        /// </summary>
        private LazyElement DialogOneButton
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#showDialog1"), "Dialog"); }
        }

        /// <summary>
        /// Gets the submit button
        /// </summary>
        private LazyElement SubmitButton
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("[class='btn btn-default'][type='submit']"), "Submit button"); }
        }

        /// <summary>
        /// Gets the Multi Select for computer parts
        /// </summary>
        private LazyElement ComputerParts
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#computerParts"), "Computer Parts Multi Select"); }
        }

        /// <summary>
        /// Gets an item that is not going to be selected
        /// </summary>
        private LazyElement NotSelected
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#computerParts [value='one']"), "Not selected"); }
        }

        /// <summary>
        /// Gets an item that is going to be selected
        /// </summary>
        private LazyElement Selected
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#computerParts [value='two']"), "Selected"); }
        }

        /// <summary>
        /// Gets a parent element
        /// </summary>
        private LazyElement FlowerTableLazyElement
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#FlowerTable"), "Flower table"); }
        }

        /// <summary>
        /// Gets a child element, the second table caption
        /// </summary>
        private LazyElement FlowerTableCaptionWithParent
        {
            get { return new LazyElement(this.FlowerTableLazyElement, By.CssSelector("CAPTION > Strong"), "Flower table caption"); }
        }

        /// <summary>
        /// Gets a child element that does not exist
        /// </summary>
        private LazyElement MissingChild
        {
            get { return new LazyElement(this.FlowerTableLazyElement, By.CssSelector("MISSING"), "Missing child element"); }
        }

        /// <summary>
        /// Gets the first table caption
        /// </summary>
        private LazyElement FirstTableCaption
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("CAPTION > Strong"), "Clothing table caption"); }
        }

        /// <summary>
        /// Gets the disabled DIV
        /// </summary>
        private LazyElement DisabledDiv
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#disabledField"), "Parent disabled div"); }
        }

        /// <summary>
        /// Gets the disabled input
        /// </summary>
        private LazyElement DisabledInput
        {
            get { return new LazyElement(this.DisabledDiv, By.CssSelector("INPUT"), "Flower table caption"); }
        }

        /// <summary>
        /// Gets the Names Dropdown
        /// </summary>
        private LazyElement NamesDropdown
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#namesDropdown"), "Names Dropdown"); }
        }

        /// <summary>
        /// Setup before a test
        /// </summary>
        [TestInitialize]
        public void NavigateToTestPage()
        {
            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation");
            this.WebDriver.Wait().ForPageLoad();
        }

        /// <summary>
        /// Verify Lazy Element fails as expected if the element is missing
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException), "The input should not exist so this will throw an exception.")]
        public void LazyTimeOutForMissingElement()
        {
            this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
            Assert.AreEqual("THISCHECKSHOULDFAIL", this.MissingItem.Text);
        }

        /// <summary>
        /// Verify Lazy Element fails as expected if the element does not match the expected state
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException), "The input should be disabled so this will throw an exception.")]
        public void LazyTimeOutForElementInWrongState()
        {
            this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
            this.DisabledItem.Click();
        }

        /// <summary>
        /// Verify Lazy time out respects wait driver settings
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyRespectsWaitDriverTimesOut()
        {
            DateTime start = DateTime.Now;

            try
            {
                this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(new SystemClock(), this.WebDriver, TimeSpan.FromSeconds(3), TimeSpan.FromMilliseconds(100)));
                this.DisabledItem.Click();
            }
            catch (TimeoutException)
            {
                TimeSpan duration = DateTime.Now - start;
                Assert.IsTrue(duration < TimeSpan.FromMilliseconds(6000), "The max wait time should be less than 6 seconds but was " + duration);
            }
        }

        /// <summary>
        /// Verify the lazy waiting for message includes what it was waiting for
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyWaitingForElementMessage()
        {
            try
            {
                this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
                this.DisabledItem.Click();
            }
            catch (TimeoutException ex)
            {
                Assert.IsTrue(ex.InnerException.Message.Contains("Waiting for clickable element"), "Message should tell us it timed out 'Waiting for clickable element', but is instead " + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// Verify Lazy Element search respects the parent find by finding mismatch
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyWithParentAndWithoutDontMatch()
        {
            // Make sure we got the table caption we are looking for
            Assert.AreEqual("Flower Table", this.FlowerTableCaptionWithParent.Text);

            // Make sure the first found was not the flower table
            Assert.AreNotEqual(this.FlowerTableCaptionWithParent.Text, this.FirstTableCaption.Text);
        }

        /// <summary>
        /// Verify Lazy Element search respects the parent find by finding match
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyWithParentAndWithoutMatch()
        {
            // Get the lazy element without a parent
            LazyElement flowerTableCaptionWithoutParent = new LazyElement(this.TestObject, By.CssSelector("#FlowerTable CAPTION > Strong"), "Flower table");

            // Make sure we are finding the correct table
            Assert.AreEqual("Flower Table", this.FlowerTableCaptionWithParent.Text);

            // Make sure we got the table caption we are looking for
            Assert.AreEqual(this.FlowerTableCaptionWithParent.Text, flowerTableCaptionWithoutParent.Text);
        }

        /// <summary>
        /// Verify Lazy Element is cached as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]

        public void LazyPreCaching()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            // Make sure we are getting back the same cached element
            Assert.IsNull(footer.CachedElement, "The cached element should be null as we never triggered a find");
        }

        /// <summary>
        /// A new find does not return the cached element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void NewFindDifferentThanCached()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            // Trigger a find and save off the element
            footer.GetValue();
            IWebElement footerElementBefore = footer.CachedElement;

            // Do the event again and save off the changed element
            footer.GetValue();

            // Make sure doing a new find returns an element that is not the same as the cached element
            Assert.AreNotEqual(WebDriver.FindElement(footer.By), footerElementBefore);
        }

        /// <summary>
        /// Check that the element was cached
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementCached()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            // Trigger a find and save off the element
            footer.GetValue();
            IWebElement footerElementBefore = footer.CachedElement;

            // Do the event again and save off the changed element
            footer.GetValue();
            IWebElement footerElementAfter = footer.CachedElement;

            // Make sure the second event didn't trigger a new find
            Assert.AreEqual(footerElementBefore, footerElementAfter);
        }

        /// <summary>
        /// Stale elements trigger a new find
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyCaching()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            // Trigger a find and save off the element
            footer.GetValue();
            IWebElement footerElementBefore = footer.CachedElement;

            // Do the event again and save off the changed element
            footer.GetValue();

            // Go to another page so the old element will be stale, this will force us to get a new one
            WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation/AsyncPage");

            // Trigger a new find, this should be new because the cached element is stale
            footer.GetValue();
            Assert.AreNotEqual(footerElementBefore, footer.CachedElement);
        }

        /// <summary>
        /// Verify the get elements trigger new finds - We do this because we are looking for specific states
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetsTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            IWebElement cacheFooter = footer.GetRawVisibleElement();

            // Make sure the second event didn't trigger a new find
            Assert.AreEqual(cacheFooter, footer.CachedElement);
        }

        /// <summary>
        /// Verify the get clickable element triggers new finds - We do this because we are looking for specific states
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetClickableTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            footer.GetRawVisibleElement();

            // Make sure get clickable triggers a new find
            Assert.AreNotEqual(footer.CachedElement, footer.GetRawClickableElement());
        }

        /// <summary>
        /// Verify the get existing element triggers new finds - We do this because we are looking for specific states
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetExistTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            footer.GetRawVisibleElement();

            // Make sure get exists triggers a new find
            Assert.AreNotEqual(footer.CachedElement, footer.GetRawExistingElement());
        }

        /// <summary>
        /// Verify the get visible element triggers new finds - We do this because we are looking for specific states
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetVisibleTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            footer.GetRawVisibleElement();

            // Make sure get visible triggers a new find
            Assert.AreNotEqual(footer.CachedElement, footer.GetRawVisibleElement());
        }

        /// <summary>
        /// Verify Lazy Element Clear test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementClear()
        {
            // Make sure we can set the value
            this.InputBox.SendKeys("test");
            Assert.AreEqual("test", this.InputBox.GetAttribute("value"));

            // Make sure the value is cleared
            this.InputBox.Clear();
            Assert.AreEqual(string.Empty, this.InputBox.GetAttribute("value"));
        }

        /// <summary>
        /// Verify Lazy Element Click test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementClick()
        {
            this.DialogOneButton.Click();
            Assert.AreEqual(true, this.DialogOne.Displayed);
        }

        /// <summary>
        /// Verify Lazy Element get By test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetBy()
        {
            By testBy = By.CssSelector("#ItemsToAutomate");
            LazyElement testLazyElement = new LazyElement(this.TestObject, testBy, "TEST");

            Assert.AreEqual(testBy, testLazyElement.By);
        }

        /// <summary>
        /// Verify Lazy Element get of the test object
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetTestObject()
        {
            LazyElement testLazyElement = new LazyElement(this.TestObject, By.CssSelector("#ItemsToAutomate"), "TEST");
            Assert.AreEqual(this.TestObject, testLazyElement.TestObject);
        }

        /// <summary>
        /// Verify Lazy Element GetAttribute test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetAttribute()
        {
            Assert.AreEqual("Disabled", this.DisabledItem.GetAttribute("value"));
        }

        /// <summary>
        /// Verify Lazy Element with a parent GetAttribute test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetAttributeWithParent()
        {
            Assert.AreEqual("Disabled", this.DisabledInput.GetAttribute("value"));
        }

        /// <summary>
        /// Verify Lazy Element GetCssValue test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetCssValue()
        {
            Assert.AreEqual("visible", this.DialogOneButton.GetCssValue("overflow"));
        }

        /// <summary>
        /// Verify Lazy Element with parent GetCssValue test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetCssValueWithParent()
        {
            Assert.AreEqual("280px", this.DisabledInput.GetCssValue("max-width"));
        }

        /// <summary>
        /// Verify Lazy Element SendKeys test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSendKeys()
        {
            this.InputBox.SendKeys("test");
            Assert.AreEqual("test", this.InputBox.GetValue());
        }

        /// <summary>
        /// Verify Lazy Element with a parent SendKeys test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException), "The input should be disabled so this will throw an exception.")]
        public void LazyElementSendKeysWithParent()
        {
            this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
            this.DisabledInput.SendKeys("test");
        }

        /// <summary>
        /// Verify Lazy Element SendKeys test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSendSecretKeys()
        {
            this.InputBox.SendKeys("beforeSuspendTest");
            this.InputBox.Clear();
            this.InputBox.SendSecretKeys("secretKeys");
            this.InputBox.Clear();
            this.InputBox.SendKeys("continueTest");

            FileLogger logger = (FileLogger)TestObject.Log;
            string filepath = logger.FilePath;

            Assert.IsTrue(File.ReadAllText(filepath).Contains("beforeSuspendTest"));
            Assert.IsFalse(File.ReadAllText(filepath).Contains("secretKeys"));
            Assert.IsTrue(File.ReadAllText(filepath).Contains("continueTest"));
            File.Delete(filepath);
        }

        /// <summary>
        /// Make sure logging is enabled after an error is thrown
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSendSecretKeysEnableLoggingAfterError()
        {
            string checkLogged = "THISSHOULDBELOGGED";

            try
            {
                this.InputBox.SendSecretKeys(null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentNullException), e.InnerException.GetType());
            }

            this.InputBox.SendKeys(checkLogged);

            FileLogger logger = (FileLogger)TestObject.Log;
            string filepath = logger.FilePath;

            Assert.IsTrue(File.ReadAllText(filepath).Contains(checkLogged));
            File.Delete(filepath);
        }

        /// <summary>
        /// Verify Lazy Element Submit test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSubmit()
        {
            WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Employees");
            WebDriver.Wait().ForClickableElement(By.CssSelector("A[href^='/Employees/Edit/']")).Click();
            WebDriver.Wait().ForPageLoad();

            this.SubmitButton.Submit();
            Assert.IsTrue(WebDriver.Wait().UntilAbsentElement(By.CssSelector("#[type='submit']")), "Submit did not go away");
        }

        /// <summary>
        /// Verify Lazy Element with parent Submit test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException), "The input should be disabled so this will throw an exception.")]
        public void LazyElementSubmitWithParent()
        {
            this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
            this.DisabledInput.Submit();
        }

        /// <summary>
        /// Verify Lazy Element Select Dropdown Option
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSelectDropDownOption()
        {
            this.NamesDropdown.SelectDropDownOption("Ashley");
            Assert.AreEqual("Ashley", this.NamesDropdown.GetSelectedOptionFromDropdown());
        }

        /// <summary>
        /// Verify Lazy Element Select Dropdown Option By Value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSelectDropDownOptionByValue()
        {
            this.NamesDropdown.SelectDropDownOptionByValue("7");
            Assert.AreEqual("Ashley", this.NamesDropdown.GetSelectedOptionFromDropdown());
        }

        /// <summary>
        /// Verify Lazy Element Get Selected Option From Dropdown
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetSelectedOptionFromDropdown()
        {
            Assert.AreEqual("Joe", this.NamesDropdown.GetSelectedOptionFromDropdown());
        }

        /// <summary>
        /// Verify Lazy Element Get Selected Options From dropdown
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetSelectedOptionsFromDropdown()
        {
            this.ComputerParts.SelectDropDownOption("Motherboard");
            this.ComputerParts.SelectDropDownOption("Power Supply");

            var selectedOptions = this.ComputerParts.GetSelectedOptionsFromDropdown();
            Assert.AreEqual("Motherboard", selectedOptions[0]);
            Assert.AreEqual("Power Supply", selectedOptions[1]);
        }

        /// <summary>
        /// Verify Lazy Element Displayed test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementDisplayed()
        {
            Assert.AreEqual(true, this.DialogOneButton.Displayed);
            Assert.AreEqual(false, this.DialogOne.Displayed);
        }

        /// <summary>
        /// Verify Lazy Element with parent Displayed test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementDisplayedWithParent()
        {
            Assert.AreEqual(true, this.DisabledInput.Displayed);
        }

        /// <summary>
        /// Verify Lazy Element property
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementProperty()
        {
            Assert.AreEqual("showDialog1", this.DialogOneButton.GetProperty("id"), "Expected ID to be 'showDialog1'");
        }

        /// <summary>
        /// Verify Lazy Element Enabled test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementEnabled()
        {
            Assert.AreEqual(false, this.DisabledItem.Enabled);
            Assert.AreEqual(true, this.InputBox.Enabled);
        }

        /// <summary>
        /// Verify Lazy Element with parent Enabled test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementEnabledWithParent()
        {
            Assert.AreEqual(false, this.DisabledInput.Enabled);
            Assert.AreEqual(true, this.FlowerTableCaptionWithParent.Enabled);
        }

        /// <summary>
        /// Verify Lazy Element Selected test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSelected()
        {
            ElementHandler.SelectDropDownOptionByValue(this.WebDriver, By.CssSelector("#computerParts"), "two");

            Assert.AreEqual(true, this.Selected.Selected);
            Assert.AreEqual(false, this.NotSelected.Selected);
        }

        /// <summary>
        /// Verify Lazy Element Text test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementText()
        {
            Assert.AreEqual("Show dialog", this.DialogOneButton.Text);
        }

        /// <summary>
        /// Verify Lazy Element with parent Text test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementTextWithParent()
        {
            Assert.AreEqual("Flower Table", this.FlowerTableCaptionWithParent.Text);
        }

        /// <summary>
        /// Verify Lazy Element Location test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementLocation()
        {
            Point point = this.InputBox.Location;
            Assert.IsTrue(point.X > 0 && point.Y > 0, "Unexpected point: " + point);
        }

        /// <summary>
        /// Verify Lazy Element with parent Location test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementLocationWithParent()
        {
            Point earlierPoint = this.InputBox.Location;
            Point laterPoint = this.DisabledInput.Location;

            Assert.IsTrue(laterPoint.X > 0, "Unexpected point: " + laterPoint);
            Assert.IsTrue(earlierPoint.Y < laterPoint.Y, "Unexpected point: " + laterPoint);
        }

        /// <summary>
        /// Verify Lazy Element Size test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSize()
        {
            Size size = this.InputBox.Size;
            Assert.IsTrue(size.Width > 0 && size.Height > 0, "Height and/or width are less than 1");
        }

        /// <summary>
        /// Verify Lazy Element with parent Size test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSizeWithParent()
        {
            Size size = this.DisabledInput.Size;
            Assert.IsTrue(size.Width > 152 && size.Height > 21, "Height of greater than 22 and width of greater than 152, but got " + size);
        }

        /// <summary>
        /// Verify lazy element tag name test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementTagName()
        {
            Assert.AreEqual("input", this.InputBox.TagName);
        }

        /// <summary>
        /// Verify lazy element with parent tag name test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementTagNameWithParent()
        {
            Assert.AreEqual("strong", this.FlowerTableCaptionWithParent.TagName);
        }

        /// <summary>
        /// Verify lazy element get the visible element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetVisibleElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetRawVisibleElement());
        }

        /// <summary>
        /// Verify lazy element get the clickable element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetClickableElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetRawClickableElement());
        }

        /// <summary>
        /// Verify lazy element get the existing element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetExistingElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetRawExistingElement());
        }

        /// <summary>
        /// Verify lazy element exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementExists()
        {
            LazyElement slowLoad = new LazyElement(this.TestObject, By.CssSelector("#AsyncContent[style*='block']"));
            WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation/AsyncPage");

            Assert.IsTrue(slowLoad.Exists, "Element should exist");
        }

        /// <summary>
        /// Verify lazy element doesn't exist
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementDoesntExist()
        {
            this.WebDriver.SetWaitDriver(new WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(2)));

            DateTime start = DateTime.Now;

            // Make sure we return false when the element does not exist
            Assert.IsFalse(MissingItem.Exists, "Expect element not to exist");

            // Make sure the override wait time is respected
            TimeSpan duration = DateTime.Now - start;
            Assert.IsTrue(duration < TimeSpan.FromSeconds(4), "The max wait time should be less than 4 seconds but was " + duration.TotalSeconds);
        }

        /// <summary>
        /// Verify lazy child element doesn't exist
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyChildElementDoesntExist()
        {
            this.WebDriver.SetWaitDriver(new WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(2)));

            DateTime start = DateTime.Now;

            // Make sure we return false when the element does not exist
            Assert.IsFalse(MissingChild.Exists, "Expect child element not to exist");

            // Make sure the override wait time is respected
            TimeSpan duration = DateTime.Now - start;
            Assert.IsTrue(duration < TimeSpan.FromSeconds(4), "The max wait time should be less than 4 seconds but was " + duration.TotalSeconds);
        }

        /// <summary>
        /// Verify lazy element exists
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementExistsNow()
        {
            LazyElement slowLoad = new LazyElement(this.TestObject, By.CssSelector("#AsyncContent[style*='block']"));
            WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation/AsyncPage");

            Assert.IsFalse(slowLoad.ExistsNow, "Element should not exist yet");
        }

        /// <summary>
        /// Verify lazy element to string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementToString()
        {
            // Hard-coded userFriendlyName due to private access on LazyElement
            var stringValue =
                this.FlowerTableLazyElement.By.ToString() + "Flower table";
            Assert.AreEqual(stringValue, this.FlowerTableLazyElement.ToString());
        }

        /// <summary>
        /// Verify lazy element with parent to string
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementWithParentToString()
        {
            // Hard-coded userFriendlyName due to private access on LazyElement
            var stringValue =
                this.FlowerTableLazyElement.By.ToString() + "Flower table" +
                this.FlowerTableCaptionWithParent.By.ToString() + "Flower table caption";
            Assert.AreEqual(stringValue, this.FlowerTableCaptionWithParent.ToString());
        }

        /// <summary>
        /// Verify find element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindElement()
        {
            IWebElement firstElement = this.FlowerTableLazyElement.FindRawElement(By.CssSelector("THEAD TH"));
            Assert.AreEqual("Flowers", firstElement.Text);
        }

        /// <summary>
        /// Find element respects action waits
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException), "The input should be disabled so this will throw an exception.")]
        public void LazyElementFindElementRespectAction()
        {
            IWebElement firstElement = this.DivRoot.FindElement(this.DisabledItem.By);
            this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
            firstElement.Click();
        }

        /// <summary>
        /// Verify find elements
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindElements()
        {
            ReadOnlyCollection<IWebElement> elements = this.FlowerTableLazyElement.FindRawElements(By.CssSelector("THEAD TH"));
            Assert.AreEqual("Color", elements[4].Text);
        }

        /// <summary>
        /// Stacked lazy elements handle staleness
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindElementsStackedWithStale()
        {
            LazyElement lazyRoot = new LazyElement(this.TestObject, By.CssSelector("#ItemsToAutomate"));
            IWebElement secondTable = lazyRoot.FindElements(By.CssSelector("TABLE"))[1];
            IWebElement lastTableHeader = ((LazyElement)secondTable).FindElements(By.CssSelector("THEAD TH"))[4];

            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase());
            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation");

            Assert.AreEqual("Color", lastTableHeader.Text);
        }

        /// <summary>
        /// Find elements are all lazy
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindElementsAreLazy()
        {
            foreach (IWebElement element in this.FlowerTableLazyElement.FindElements(By.CssSelector("THEAD TH")))
            {
                this.SoftAssert.Assert(() => Assert.IsTrue(element is LazyElement));
            }

            this.SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Find elements respects action waits
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(TimeoutException), "The input should be disabled so this will throw an exception.")]
        public void LazyElementFindElementsRespectAction()
        {
            IWebElement firstElement = this.DivRoot.FindElements(this.DisabledItem.By)[0];

            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase());
            this.WebDriver.Navigate().GoToUrl(SeleniumConfig.GetWebSiteBase() + "Automation");

            this.WebDriver.SetWaitDriver(new OpenQA.Selenium.Support.UI.WebDriverWait(this.WebDriver, TimeSpan.FromSeconds(1)));
            firstElement.Click();
        }

        /// <summary>
        /// Stacked get visible
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindElementsGetVisible()
        {
            LazyElement lazyRoot = new LazyElement(this.TestObject, By.CssSelector("#ItemsToAutomate"));
            IWebElement secondTable = lazyRoot.FindElements(By.CssSelector("TABLE"))[1];
            IWebElement getSecondTable = ((LazyElement)secondTable).GetRawVisibleElement();
            Assert.AreEqual(secondTable.Text, getSecondTable.Text);
        }

        /// <summary>
        /// Find Element the run Actions that cast to ILocatable
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindRawElementWorksWithActions()
        {
            IWebElement rawElement = this.DivRoot.FindRawElement(this.DisabledItem.By);

            Actions a1 = new Actions(this.WebDriver);
            a1.KeyDown(rawElement, Keys.Shift).Build().Perform();
            a1.KeyUp(rawElement, Keys.Shift).Build().Perform();
            a1.SendKeys(rawElement, "Hello").Build().Perform();
            a1.MoveToElement(rawElement).Build().Perform();
            a1.MoveToElement(rawElement, 0, 0).Build().Perform();
            a1.MoveToElement(rawElement, 0, 0, MoveToElementOffsetOrigin.Center).Build().Perform();
        }

        /// <summary>
        /// Find Elements the run Actions that cast to ILocatable
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementFindRawElementsWorksWithActions()
        {
            IWebElement rawElement = this.DivRoot.FindRawElements(this.DisabledItem.By)[0];

            Actions a1 = new Actions(this.WebDriver);
            a1.KeyDown(rawElement, Keys.Shift).Build().Perform();
            a1.KeyUp(rawElement, Keys.Shift).Build().Perform();
            a1.SendKeys(rawElement, "Hello").Build().Perform();
            a1.MoveToElement(rawElement).Build().Perform();
            a1.MoveToElement(rawElement, 0, 0).Build().Perform();
            a1.MoveToElement(rawElement, 0, 0, MoveToElementOffsetOrigin.Center).Build().Perform();
        }
    }
}