//-----------------------------------------------------
// <copyright file="LazyElementUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Test the lazy element unit tests</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
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
        /// Gets the disabled item
        /// </summary>
        #region LazyElementCreate
        private LazyElement DisabledItem
        {
            get { return new LazyElement(this.TestObject, By.CssSelector("#disabledField INPUT"), "Disabled"); }
        }
        #endregion

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
        #region LazyElementCreateWithParent
        private LazyElement FlowerTableCaptionWithParent
        {
            get { return new LazyElement(this.FlowerTableLazyElement, By.CssSelector("CAPTION > Strong"), "Flower table caption"); }
        }
        #endregion

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
        /// Setup before a test
        /// </summary>
        [TestInitialize]
        public void NavigateToTestPage()
        {
            this.WebDriver.Navigate().GoToUrl(Config.GetValue("WebSiteBase") + "Automation");
            this.WebDriver.Wait().ForPageLoad();
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

            // Make sure the the first found was not the the flower table
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

            // Make sure we are getting ack the same cached element
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
            string value = footer.GetValue();
            IWebElement footerElementBefore = footer.CachedElement;

            // Do the event again and save off the chanced element 
            value = footer.GetValue();
            IWebElement footerElementAfter = footer.CachedElement;

            // Make sure doing a new find returns an element that is not the same as the cached element
            Assert.AreNotEqual(this.WebDriver.FindElement(footer.By), footerElementBefore);
        }

        /// <summary>
        /// Check that the element was cached
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        #region LazyCaching
        public void LazyElementCached()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            // Trigger a find and save off the element
            string value = footer.GetValue();
            IWebElement footerElementBefore = footer.CachedElement;

            // Do the event again and save off the chanced element 
            value = footer.GetValue();
            IWebElement footerElementAfter = footer.CachedElement;

            // Make sure the second event didn't trigger a new find
            Assert.AreEqual(footerElementBefore, footerElementAfter);
        }
        #endregion

        /// <summary>
        /// Stale elements trigger a new find
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        #region LazyStaleCache
        public void LazyCaching()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            // Trigger a find and save off the element
            string value = footer.GetValue();
            IWebElement footerElementBefore = footer.CachedElement;

            // Do the event again and save off the chanced element 
            value = footer.GetValue();
            IWebElement footerElementAfter = footer.CachedElement;

            // Go to another page so the old element will be stale, this will force us to get a new one
            this.WebDriver.Navigate().GoToUrl(Config.GetValue("WebSiteBase") + "Automation/AsyncPage");

            // Trigger a new find, this should be new because the cached element is stale
            value = footer.GetValue();
            Assert.AreNotEqual(footerElementBefore, footer.CachedElement);
        }
        #endregion

        /// <summary>
        /// Verify the get elements trigger new finds - We do this because we are looking for specific states
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetsTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            IWebElement cacheFooter = footer.GetTheVisibleElement();

            // Make sure the second event didn't trigger a new find
            Assert.AreEqual(cacheFooter, footer.CachedElement);
        }

        /// <summary>
        /// Verify the get clickable element triggers new finds - We do this because we are looking for specific states
        /// </summary>
        #region LazyGetClickableTriggerFind
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetClickableTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            IWebElement cacheFooter = footer.GetTheVisibleElement();

            // Make sure get clickable triggers a new find
            Assert.AreNotEqual(footer.CachedElement, footer.GetTheClickableElement());
        }
        #endregion

        /// <summary>
        /// Verify the get existing element triggers new finds - We do this because we are looking for specific states
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        #region LazyGetExistTriggerFind
        public void LazyGetExistTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            IWebElement cacheFooter = footer.GetTheVisibleElement();

            // Make sure get exists triggers a new find
            Assert.AreNotEqual(footer.CachedElement, footer.GetTheExistingElement());
        }
        #endregion

        /// <summary>
        /// Verify the get visible element triggers new finds - We do this because we are looking for specific states
        /// </summary>
        #region LazyGetVisibleTriggerFind
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyGetVisibleTriggerFind()
        {
            // Create the lazy element and use it
            LazyElement footer = new LazyElement(this.TestObject, By.CssSelector("FOOTER P"), "Footer");

            IWebElement cacheFooter = footer.GetTheVisibleElement();

            // Make sure get visible triggers a new find
            Assert.AreNotEqual(footer.CachedElement, footer.GetTheVisibleElement());
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element Clear test
        /// </summary>
        #region LazyElementClear
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
        #endregion

        /// <summary>
        /// Verify Lazy Element Click test
        /// </summary>
        #region LazyElementClick
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementClick()
        {
            this.DialogOneButton.Click();
            Assert.AreEqual(true, this.DialogOne.Displayed);
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element get By test
        /// </summary>
        #region LazyElementGetBy
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetBy()
        {
            By testBy = By.CssSelector("#ItemsToAutomate");
            LazyElement testLazyElement = new LazyElement(this.TestObject, testBy, "TEST");

            Assert.AreEqual(testBy, testLazyElement.By);
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element get of the test object
        /// </summary>
        #region LazyElementGetTestObject
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetTestObject()
        {
            LazyElement testLazyElement = new LazyElement(this.TestObject, By.CssSelector("#ItemsToAutomate"), "TEST");
            Assert.AreEqual(this.TestObject, testLazyElement.TestObject);
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element GetAttribute test
        /// </summary>
        #region LazyElementGetAttribute
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetAttribute()
        {
            Assert.AreEqual("Disabled", this.DisabledItem.GetAttribute("value"));
        }
        #endregion

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
        #region LazyElementGetCssValue
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetCssValue()
        {
            Assert.AreEqual("visible", this.DialogOneButton.GetCssValue("overflow"));
        }
        #endregion

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
        #region LazyElementSendKeys
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSendKeys()
        {
            this.InputBox.SendKeys("test");
            Assert.AreEqual("test", this.InputBox.GetValue());
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element with a parent SendKeys test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception), "The input should be disabled so this will throw an exception.")]
        public void LazyElementSendKeysWithParent()
        {
            this.DisabledInput.SendKeys("test");
        }

        /// <summary>
        /// Verify Lazy Element SendKeys test
        /// </summary>
        #region LazyElementSendSecretKeys
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSendSecretKeys()
        {
            this.InputBox.SendKeys("beforeSuspendTest");
            this.InputBox.Clear();
            this.InputBox.SendSecretKeys("secretKeys");
            this.InputBox.Clear();
            this.InputBox.SendKeys("continueTest");

            FileLogger logger = (FileLogger)this.TestObject.Log;
            string filepath = logger.FilePath;

            Assert.IsTrue(File.ReadAllText(filepath).Contains("beforeSuspendTest"));
            Assert.IsFalse(File.ReadAllText(filepath).Contains("secretKeys"));
            Assert.IsTrue(File.ReadAllText(filepath).Contains("continueTest"));
            File.Delete(filepath);
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element Submit test
        /// </summary>
        #region LazyElementSubmit
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSubmit()
        {
            this.WebDriver.Navigate().GoToUrl(Config.GetValue("WebSiteBase") + "Employees");
            this.WebDriver.Wait().ForClickableElement(By.CssSelector("A[href^='/Employees/Edit/']")).Click();
            this.WebDriver.Wait().ForPageLoad();

            this.SubmitButton.Submit();
            Assert.IsTrue(this.WebDriver.Wait().UntilAbsentElement(By.CssSelector("#[type='submit']")), "Submit did not go away");
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element with parent Submit test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception), "The input should be disabled so this will throw an exception.")]
        public void LazyElementSubmitWithParent()
        {
            this.DisabledInput.Submit();
        }

        /// <summary>
        /// Verify Lazy Element Displayed test
        /// </summary>
        #region LazyElementDisplayed
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementDisplayed()
        {
            Assert.AreEqual(true, this.DialogOneButton.Displayed);
            Assert.AreEqual(false, this.DialogOne.Displayed);
        }
        #endregion

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
        /// Verify Lazy Element Enabled test
        /// </summary>
        #region LazyElementEnabled
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementEnabled()
        {
            Assert.AreEqual(false, this.DisabledItem.Enabled);
            Assert.AreEqual(true, this.InputBox.Enabled);
        }
        #endregion

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
        #region LazyElementSelected
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSelected()
        {
            ElementHandler.SelectDropDownOptionByValue(this.WebDriver, By.CssSelector("#computerParts"), "two");

            Assert.AreEqual(true, this.Selected.Selected);
            Assert.AreEqual(false, this.NotSelected.Selected);
        }
        #endregion

        /// <summary>
        /// Verify Lazy Element Text test
        /// </summary>
        #region LazyElementText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementText()
        {
            Assert.AreEqual("Show dialog", this.DialogOneButton.Text);
        }
        #endregion

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
        #region LazyElementLocation
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementLocation()
        {
            Point point = this.InputBox.Location;
            Assert.IsTrue(point.X > 0 && point.Y > 0, "Unexpected point: " + point);
        }
        #endregion

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
        #region LazyElementSize
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementSize()
        {
            Size size = this.InputBox.Size;
            Assert.IsTrue(size.Width > 0 && size.Height > 0, "Height and/or width are less than 1");
        }
        #endregion

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
        #region LazyElementTagName
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementTagName()
        {
            Assert.AreEqual("input", this.InputBox.TagName);
        }
        #endregion

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
        #region LazyElementVisibleElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetVisibleElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetTheVisibleElement());
        }
        #endregion

        /// <summary>
        /// Verify lazy element get the clickable element
        /// </summary>
        #region LazyElementClickableElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetClickableElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetTheClickableElement());
        }
        #endregion

        /// <summary>
        /// Verify lazy element get the existing element
        /// </summary>
        #region LazyElementExistingElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void LazyElementGetExistingElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetTheExistingElement());
        }
        #endregion
    }
}
