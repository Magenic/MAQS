//--------------------------------------------------
// <copyright file="ISoftAssert.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Soft assert interface</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Reflection;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Interface for soft assert
    /// </summary>
    public interface ISoftAssert
    {
        /// <summary>
        /// Add expected assertions to be called by this soft assert instance.
        /// </summary>
        /// <param name="expectedAsserts">Expected Assertions to be called.</param>
        void AddExpectedAsserts(params string[] expectedAsserts);

        /// <summary>
        /// Wrap an assert inside a soft assert
        /// </summary>
        /// <param name="assertFunction">The assert function</param>
        /// <returns>True if the asset passed</returns>
        bool Assert(Action assertFunction);

        /// <summary>
        /// Wrap an assert inside a soft assert
        /// </summary>
        /// <param name="assertFunction">The assert function</param>
        /// <param name="failureMessage">Message to log</param>
        /// <param name="assertName">Soft assert name or name of expected assert being called.</param>
        /// <returns>True if the asset passed</returns>
        bool Assert(Action assertFunction, string assertName, string failureMessage = "");

        /// <summary>
        /// Wrap an assert that is expected to fail and the expected failure
        /// </summary>
        /// <param name="assertFunction">The assert function</param>
        /// <param name="expectedException">The type of expected exception</param>
        /// <param name="assertName">soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>True if the assert failed</returns>
        bool AssertFails(Action assertFunction, Type expectedException, string assertName, string failureMessage = "");

        /// <summary>
        /// Look for SoftAssertExpectedAssert attribute on the test method.
        /// </summary>
        /// <param name="testMethod">The Method information of the currently running test.</param>
        void CaptureTestMethodAttributes(MethodInfo testMethod);

        /// <summary>
        /// Check if there are any failed soft asserts.
        /// </summary>
        /// <returns>True if there are failed soft asserts</returns>
        bool DidSoftAssertsFail();

        /// <summary>
        /// Gets a value indicating whether the boolean if the user checks for failures at the end of the test.
        /// </summary>
        /// <returns>If the user checked for failures.  If the number of asserts is 0, it returns true.</returns>
        bool DidUserCheck();

        /// <summary>
        /// Check that expceted assertion were run
        /// </summary>
        void CheckForExpectedAsserts();

        /// <summary>
        /// Fail test if there were one or more failures
        /// </summary>
        void FailTestIfAssertFailed();

        /// <summary>
        /// Fail test if there were one or more failures
        /// </summary>
        /// <param name="message">Customer error message</param>
        void FailTestIfAssertFailed(string message);

        /// <summary>
        /// Log final assert count summary
        /// </summary>
        void LogFinalAssertData();

        /// <summary>
        /// Override the logger
        /// </summary>
        /// <param name="log">The new logger</param>
        void OverrideLogger(ILogger log);
    }
}