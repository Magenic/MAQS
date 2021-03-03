﻿//--------------------------------------------------
// <copyright file="BaseEmailTestSteps.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using email</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using TechTalk.SpecFlow;
using MaqsEmail = Magenic.Maqs.BaseEmailTest.BaseEmailTest;

namespace Magenic.Maqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for email TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Email")]
    public class BaseEmailTestSteps : ExtendableTestSteps<EmailTestObject, MaqsEmail>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEmailTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseEmailTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the email driver from the test object
        /// </summary>
        protected EmailDriver EmailDriver
        {
            get { return this.TestObject.EmailDriver; }
        }
    }
}
