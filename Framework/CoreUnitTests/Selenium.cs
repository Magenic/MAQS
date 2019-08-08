//--------------------------------------------------
// <copyright file="Selenium.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Core Selenium unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;

namespace CoreUnitTests
{
    /// <summary>
    /// Simple Selenium test
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Selenium)]
    [ExcludeFromCodeCoverage]
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
