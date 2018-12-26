//--------------------------------------------------
// <copyright file="BaseExtendableTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for test classes that setup test objects like web drivers or database connections</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Base code for test classes that setup test objects like web drivers or database connections
    /// </summary>
    /// <typeparam name="T">Test object type</typeparam>
    [TestClass]
    public abstract class BaseExtendableTest<T> : BaseTest where T : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseExtendableTest{T}" /> class
        /// </summary>
        protected BaseExtendableTest()
        {
        }

        /// <summary>
        /// Gets or sets the test object 
        /// </summary>
        protected new T TestObject
        {
            get
            {
                return (T)base.TestObject;
            }

            set
            {
                this.BaseTestObjects.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Setup before a test
        /// </summary>
        [TestInitialize]
        [SetUp]
        public new void Setup()
        {
            // Do base generic setup
            base.Setup();
        }

        /// <summary>
        /// Create a test object
        /// </summary>
        protected override abstract void CreateNewTestObject();
    }
}
