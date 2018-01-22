//--------------------------------------------------
// <copyright file="BaseDatabaseTestSteps.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using databases</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseDatabaseTest;
using TechTalk.SpecFlow;

namespace SpecFlowMAQSExtension.TestSteps
{
    /// <summary>
    /// Base for database TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Database")]
    public class BaseDatabaseTestSteps : ExtendableTestSteps<DatabaseTestObject, BaseDatabaseTest, DatabaseConnectionWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDatabaseTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseDatabaseTestSteps(ScenarioContext context) : base(context)
        {
        }
    }
}
