//--------------------------------------------------
// <copyright file="Base.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Core Base unit tests</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreUnitTests
{
    /// <summary>
    /// Simple base test
    /// </summary>
    [TestClass]
    public class Base : BaseTest
    {
        /// <summary>
        /// Can a basic test run
        /// </summary>
        [TestMethod]
        public void CanRunTest()
        {
            Assert.IsNotNull(this.TestObject);
        }
    }
}
