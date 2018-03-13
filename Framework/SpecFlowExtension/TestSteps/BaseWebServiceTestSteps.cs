//--------------------------------------------------
// <copyright file="BaseWebServiceTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using web services</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseWebServiceTest;
using TechTalk.SpecFlow;
using MaqsWeb = Magenic.MaqsFramework.BaseWebServiceTest.BaseWebServiceTest;

namespace Magenic.MaqsFramework.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for web service TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_WebService")]
    public class BaseWebServiceTestSteps : ExtendableTestSteps<WebServiceTestObject, MaqsWeb, HttpClientWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWebServiceTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseWebServiceTestSteps(ScenarioContext context) : base(context)
        {
        }
    }
}
