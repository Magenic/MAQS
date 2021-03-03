﻿//--------------------------------------------------
// <copyright file="BaseDatabaseTestSteps.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base teststeps code for tests using databases</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest;
using TechTalk.SpecFlow;
using MaqsDatabase = Magenic.Maqs.BaseDatabaseTest.BaseDatabaseTest;

namespace Magenic.Maqs.SpecFlow.TestSteps
{
    /// <summary>
    /// Base for database TestSteps classes
    /// </summary>
    [Binding, Scope(Tag = "MAQS_Database")]
    public class BaseDatabaseTestSteps : ExtendableTestSteps<DatabaseTestObject, MaqsDatabase>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDatabaseTestSteps" /> class
        /// </summary>
        /// <param name="context">The scenario context.</param>
        public BaseDatabaseTestSteps(ScenarioContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the database driver from the test object
        /// </summary>
        protected DatabaseDriver DatabaseDriver
        {
            get { return this.TestObject.DatabaseDriver; }
        }
    }
}
