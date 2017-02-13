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
        /// Gets and item that is going to be selected
        /// </summary>
        private FluentElement Selected
        {
            get { return new FluentElement(this.TestObject, By.CssSelector("#computerParts [value='two']"), "Selected"); }
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
        /// Verify Fluent Element Submit test
        /// </summary>
        #region FluentElementSubmit
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void FluentElementSubmit()
        {
            this.WebDriver.Navigate().GoToUrl(Config.GetValue("WebSiteBase") + "Employees/Edit/380");
            this.WebDriver.Wait().ForPageLoad();

            this.SubmitButton.Submit();
            Assert.IsTrue(this.WebDriver.Wait().UntilAbsentElement(By.CssSelector("#[type='submit']")), "Submit did not go away");
        }
        #endregion

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
