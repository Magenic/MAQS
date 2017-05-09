//-----------------------------------------------------
// <copyright file="FluentElementUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test the fluent element unit tests</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.BaseSeleniumTest.Extensions;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Unit test class for FluentElement
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FluentElementUnitTests : BaseSeleniumTest
    {
        /// <summary>
        /// Gets the disabled item
        /// </summary>
        #region FluentElementCreate
        private FluentElement DisabledItem
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#disabledField INPUT"), "Disabled"); }
        }
        #endregion

        /// <summary>
        /// Gets the input box
        /// </summary>
        private FluentElement InputBox
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#TextFields [name='firstname']"), "Input box"); }
        }

        /// <summary>
        /// Gets dialog one
        /// </summary>
        private FluentElement DialogOne
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#CloseButtonShowDialog"), "Dialog"); }
        }

        /// <summary>
        /// Gets the dialog one button
        /// </summary>
        private FluentElement DialogOneButton
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#showDialog1"), "Dialog"); }
        }

        /// <summary>
        /// Gets the submit button
        /// </summary>
        private FluentElement SubmitButton
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("[class='btn btn-default'][type='submit']"), "Submit button"); }
        }

        /// <summary>
        /// Gets an item that is not going to be selected
        /// </summary>
        private FluentElement NotSelected
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#computerParts [value='one']"), "Not selected"); }
        }

        /// <summary>
        /// Gets an item that is going to be selected
        /// </summary>
        private FluentElement Selected
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#computerParts [value='two']"), "Selected"); }
        }

        /// <summary>
        /// Gets a parent element
        /// </summary>
        private FluentElement FlowerTableFluentElement
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#FlowerTable"), "Flower table"); }
        }

        /// <summary>
        /// Gets a child element, the second table caption
        /// </summary>
        #region FluentElementCreateWithParent
        private FluentElement FlowerTableCaptionWithParent
        {
            get { return new FluentElement(this.FlowerTableFluentElement, By.CssSelector("CAPTION > Strong"), "Flower table caption"); }
        }
        #endregion

        /// <summary>
        /// Gets the first table caption
        /// </summary>
        private FluentElement FirstTableCaption
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("CAPTION > Strong"), "Clothing table caption"); }
        }

        /// <summary>
        /// Gets the disabled DIV
        /// </summary>
        private FluentElement DisabledDiv
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#disabledField"), "Parent disabled div"); }
        }

        /// <summary>
        /// Gets the disabled input
        /// </summary>
        private FluentElement DisabledInput
        {
            get { return new FluentElement(this.DisabledDiv, By.CssSelector("INPUT"), "Flower table caption"); }
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
        /// Verify Fluent Element search respects the parent find by finding mismatch
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentWithParentAndWithoutDontMatch()
        {
            // Make sure we got the table caption we are looking for
            Assert.AreEqual("Flower Table", this.FlowerTableCaptionWithParent.Text);

            // Make sure the the first found was not the the flower table
            Assert.AreNotEqual(this.FlowerTableCaptionWithParent.Text, this.FirstTableCaption.Text);
        }

        /// <summary>
        /// Verify Fluent Element search respects the parent find by finding match
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentWithParentAndWithoutMatch()
        {
            // Get the fluent element without a parent
            FluentElement flowerTableCaptionWithoutParent = new FluentElement(this.TestObject, By.CssSelector("#FlowerTable CAPTION > Strong"), "Flower table");

            // Make sure we are finding the correct table
            Assert.AreEqual("Flower Table", this.FlowerTableCaptionWithParent.Text);

            // Make sure we got the table caption we are looking for
            Assert.AreEqual(this.FlowerTableCaptionWithParent.Text, flowerTableCaptionWithoutParent.Text);
        }

        /// <summary>
        /// Verify Fluent Element Clear test
        /// </summary>
        #region FluentElementClear
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementClear()
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
        /// Verify Fluent Element Click test
        /// </summary>
        #region FluentElementClick
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementClick()
        {
            this.DialogOneButton.Click();
            Assert.AreEqual(true, this.DialogOne.Displayed);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element get By test
        /// </summary>
        #region FluentElementGetBy
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetBy()
        {
            By testBy = By.CssSelector("#ItemsToAutomate");
            FluentElement testFluentElement = new FluentElement(this.TestObject, testBy, "TEST");

            Assert.AreEqual(testBy, testFluentElement.By);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element get of the test object
        /// </summary>
        #region FluentElementGetTestObject
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetTestObject()
        {
            FluentElement testFluentElement = new FluentElement(this.TestObject, By.CssSelector("#ItemsToAutomate"), "TEST");
            Assert.AreEqual(this.TestObject, testFluentElement.TestObject);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element GetAttribute test
        /// </summary>
        #region FluentElementGetAttribute
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetAttribute()
        {
            Assert.AreEqual("Disabled", this.DisabledItem.GetAttribute("value"));
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with a parent GetAttribute test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetAttributeWithParent()
        {
            Assert.AreEqual("Disabled", this.DisabledInput.GetAttribute("value"));
        }

        /// <summary>
        /// Verify Fluent Element GetCssValue test
        /// </summary>
        #region FluentElementGetCssValue
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetCssValue()
        {
            Assert.AreEqual("visible", this.DialogOneButton.GetCssValue("overflow"));
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent GetCssValue test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetCssValueWithParent()
        {
            Assert.AreEqual("280px", this.DisabledInput.GetCssValue("max-width"));
        }

        /// <summary>
        /// Verify Fluent Element SendKeys test
        /// </summary>
        #region FluentElementSendKeys
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementSendKeys()
        {
            this.InputBox.SendKeys("test");
            Assert.AreEqual("test", this.InputBox.GetValue());
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with a parent SendKeys test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception), "The input should be disabled so this will throw an exception.")]
        public void FluentElementSendKeysWithParent()
        {
            this.DisabledInput.SendKeys("test");
        }

        /// <summary>
        /// Verify Fluent Element Submit test
        /// </summary>
        #region FluentElementSubmit
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementSubmit()
        {
            this.WebDriver.Navigate().GoToUrl(Config.GetValue("WebSiteBase") + "Employees");
            this.WebDriver.Wait().ForClickableElement(By.CssSelector("A[href^='/Employees/Edit/']")).Click();
            this.WebDriver.Wait().ForPageLoad();

            this.SubmitButton.Submit();
            Assert.IsTrue(this.WebDriver.Wait().UntilAbsentElement(By.CssSelector("#[type='submit']")), "Submit did not go away");
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent Submit test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(Exception), "The input should be disabled so this will throw an exception.")]
        public void FluentElementSubmitWithParent()
        {
            this.DisabledInput.Submit();
        }

        /// <summary>
        /// Verify Fluent Element Displayed test
        /// </summary>
        #region FluentElementDisplayed
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementDisplayed()
        {
            Assert.AreEqual(true, this.DialogOneButton.Displayed);
            Assert.AreEqual(false, this.DialogOne.Displayed);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent Displayed test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementDisplayedWithParent()
        {
            Assert.AreEqual(true, this.DisabledInput.Displayed);
        }

        /// <summary>
        /// Verify Fluent Element Enabled test
        /// </summary>
        #region FluentElementEnabled
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementEnabled()
        {
            Assert.AreEqual(false, this.DisabledItem.Enabled);
            Assert.AreEqual(true, this.InputBox.Enabled);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent Enabled test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementEnabledWithParent()
        {
            Assert.AreEqual(false, this.DisabledInput.Enabled);
            Assert.AreEqual(true, this.FlowerTableCaptionWithParent.Enabled);
        }

        /// <summary>
        /// Verify Fluent Element Selected test
        /// </summary>
        #region FluentElementSelected
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementSelected()
        {
            ElementHandler.SelectDropDownOptionByValue(this.WebDriver, By.CssSelector("#computerParts"), "two");

            Assert.AreEqual(true, this.Selected.Selected);
            Assert.AreEqual(false, this.NotSelected.Selected);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element Text test
        /// </summary>
        #region FluentElementText
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementText()
        {
            Assert.AreEqual("Show dialog", this.DialogOneButton.Text);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent Text test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementTextWithParent()
        {
            Assert.AreEqual("Flower Table", this.FlowerTableCaptionWithParent.Text);
        }

        /// <summary>
        /// Verify Fluent Element Location test
        /// </summary>
        #region FluentElementLocation
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementLocation()
        {
            Point point = this.InputBox.Location;
            Assert.IsTrue(point.X > 0 && point.Y > 0, "Unexpected point: " + point);
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent Location test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementLocationWithParent()
        {
            Point earlierPoint = this.InputBox.Location;
            Point laterPoint = this.DisabledInput.Location;

            Assert.IsTrue(laterPoint.X > 0, "Unexpected point: " + laterPoint);
            Assert.IsTrue(earlierPoint.Y < laterPoint.Y, "Unexpected point: " + laterPoint);
        }

        /// <summary>
        /// Verify Fluent Element Size test
        /// </summary>
        #region FluentElementSize
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementSize()
        {
            Size size = this.InputBox.Size;
            Assert.IsTrue(size.Width > 0 && size.Height > 0, "Height and/or width are less than 1");
        }
        #endregion

        /// <summary>
        /// Verify Fluent Element with parent Size test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementSizeWithParent()
        {
            Size size = this.DisabledInput.Size;
            Assert.IsTrue(size.Width == 153 && size.Height == 22, "Height of 22 and width of 153, but got " + size);
        }

        /// <summary>
        /// Verify fluent element tag name test
        /// </summary>
        #region FluentElementTagName
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementTagName()
        {
            Assert.AreEqual("input", this.InputBox.TagName);
        }
        #endregion

        /// <summary>
        /// Verify fluent element with parent tag name test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementTagNameWithParent()
        {
            Assert.AreEqual("strong", this.FlowerTableCaptionWithParent.TagName);
        }

        /// <summary>
        /// Verify fluent element get the visible element
        /// </summary>
        #region FluentElementVisibleElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetVisibleElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetTheVisibleElement());
        }
        #endregion

        /// <summary>
        /// Verify fluent element get the clickable element
        /// </summary>
        #region FluentElementClickableElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetClickableElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetTheClickableElement());
        }
        #endregion

        /// <summary>
        /// Verify fluent element get the existing element
        /// </summary>
        #region FluentElementExistingElement
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementGetExistingElement()
        {
            Assert.AreNotEqual(null, this.InputBox.GetTheExistingElement());
        }
        #endregion
    }
}
