//--------------------------------------------------
// <copyright file="Selenium.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Core Selenium unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseSeleniumTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreUnitTests
{
    /// <summary>
    /// Simple Selenium test
    /// </summary>
    [TestClass]
    public class Selenium : BaseSeleniumTest
    {
        /// <summary>
        /// Run a very very simple selenium test
        /// </summary>
        [TestMethod]
        public void CanRunSeleniumTest()
        {
            Assert.IsNotNull(this.TestObject.WebDriver);
        }       
    }
}
