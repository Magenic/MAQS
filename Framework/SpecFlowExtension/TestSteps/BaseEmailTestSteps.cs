//--------------------------------------------------
// <copyright file="BaseEmailTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using email</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseEmailTest;
using TechTalk.SpecFlow;

namespace SpecFlowMAQSExtension.TestSteps
{
    /// <summary>
    /// Base for email TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Email")]
    public class BaseEmailTestSteps : ExtendableTestSteps<EmailTestObject, BaseEmailTest, EmailConnectionWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEmailTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseEmailTestSteps(ScenarioContext context) : base(context)
        {
        }
    }
}
