﻿//--------------------------------------------------
// <copyright file="ActionBuilderUnitTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Unit tests for the ActionBuilder class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ActionBuilderUnitTests : SauceLabsBaseSeleniumTest
    {
        /// <summary>
        /// Url for the site
        /// </summary>
        private static readonly string siteUrl = SeleniumConfig.GetWebSiteBase();

        /// <summary>
        /// Url for the automation page
        /// </summary>
        private static readonly string siteAutomationUrl = siteUrl + "Automation/";

        /// <summary>
        /// Manage dropdown selector
        /// </summary>
        private static readonly By manageDropdown = By.CssSelector("body > div.navbar.navbar-inverse.navbar-fixed-top > div > div.navbar-collapse.collapse > ul > li:nth-child(2) > a");

        /// <summary>
        /// Employee link
        /// </summary>
        private static readonly By employeeButton = By.CssSelector("#EmployeeButton > a");

        /// <summary>
        /// Automation page header
        /// </summary>
        private static readonly By automationPageHeader = By.CssSelector("body > div.container.body-content > h2");

        /// <summary>
        /// Dialog button on automation page
        /// </summary>
        private static readonly By dialogButton2 = By.CssSelector("#showDialog2");

        /// <summary>
        /// Employee page title
        /// </summary>
        private static readonly By employeePageTitle = By.CssSelector("body > div.container.body-content > h2");

        /// <summary>
        /// Slider object
        /// </summary>
        private static readonly By slider = By.CssSelector("#slider > span");

        /// <summary>
        /// Label for the text
        /// </summary>
        private static readonly By sliderLabelNumber = By.CssSelector("#sliderNumber");

        /// <summary>
        /// Element with context menu for testing right click
        /// </summary>
        private static readonly By rightClickableElementWithContextMenu = By.CssSelector("#rightclickspace");

        /// <summary>
        /// Text within context menu triggered by right click on specific element
        /// </summary>
        private static readonly By rightClickContextSaveText = By.CssSelector("#RightClickSaveText");

        /// <summary>
        /// Unit test for the Hover Over function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void HoverOverTest()
        {
            this.NavigateToUrl(siteUrl);
            this.WebDriver.HoverOver(manageDropdown);
            this.WebDriver.Wait().ForClickableElement(employeeButton).Click();
            this.WebDriver.Wait().ForExactText(employeePageTitle, "Index");
        }

        /// <summary>
        /// Unit test for the Press Modifier Key function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void PressModifierKeyTest()
        {
            this.NavigateToUrl(siteAutomationUrl);
            this.WebDriver.PressModifierKey(Keys.End);
            this.WebDriver.Wait().ForVisibleElement(dialogButton2);
            this.WebDriver.PressModifierKey(Keys.Home);
            Assert.AreEqual("Elements to be automated", this.WebDriver.Wait().ForVisibleElement(automationPageHeader).Text, "Elements are not the same");
        }

        /// <summary>
        /// Unit test for the slider action
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void MoveSliderTest()
        {
            this.WebDriver.Navigate().GoToUrl(siteAutomationUrl);
            this.WebDriver.Wait().ForPageLoad();
            this.WebDriver.SlideElement(slider, 50);
            Assert.AreEqual("4", this.WebDriver.FindElement(sliderLabelNumber).GetAttribute("value"));
        }

        /// <summary>
        /// Check that a right-click context menu can be triggered successfully
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void RightClickToTriggerContextMenu()
        {
            this.NavigateToUrl(siteAutomationUrl);

            // Trigger a right click event
            this.WebDriver.RightClick(rightClickableElementWithContextMenu);

            // Check that context menu appeared
            Assert.IsTrue(this.WebDriver.FindElement(rightClickContextSaveText).Displayed);
        }

        /// <summary>
        /// Check that a right-click fails on non-existent element
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        [ExpectedException(typeof(WebDriverTimeoutException), "Right-Click did not raise appropriate exception on element not being found")]
        public void RightClickToTriggerContextMenuNotFound()
        {
            this.NavigateToUrl(siteAutomationUrl);

            // Trigger a right click event against non-existent element
            this.WebDriver.RightClick(By.CssSelector(".none"));
        }

        /// <summary>
        /// Navigate to test page url and wait for page to load
        /// </summary>
        /// <param name="url">The URL to navigate to</param>
        private void NavigateToUrl(string url)
        {
            this.WebDriver.Navigate().GoToUrl(url);
            this.WebDriver.Wait().ForPageLoad();
        }
    }
}
