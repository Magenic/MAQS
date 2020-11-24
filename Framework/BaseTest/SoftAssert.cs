//--------------------------------------------------
// <copyright file="SoftAssert.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>This is the SoftAssert class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// SoftAssert class
    /// </summary>
    public class SoftAssert
    {
        /// <summary>
        /// List of all asserted exceptions
        /// </summary>
        private readonly List<string> listOfExceptions = new List<string>();

        /// <summary>
        /// Keys of the asserts that need to be called with soft assert.
        /// </summary>
        private readonly HashSet<string> _expectedAsserts = new HashSet<string>();

        /// <summary>
        /// Keys of the asserts that have been called with soft assert.
        /// </summary>
        private readonly HashSet<string> _calledAsserts = new HashSet<string>();

        /// <summary>
        /// Initializes a new instance of the SoftAssert class.
        /// Setup the Logger
        /// </summary>
        /// <param name="logger">Logger to be used</param>
        public SoftAssert(Logger logger)
        {
            this.Log = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SoftAssert"/> class
        /// </summary>
        public SoftAssert()
        {
            this.Log = new ConsoleLogger();
        }

        /// <summary>
        /// Gets a value indicating whether the user checked for failures
        /// </summary>
        protected bool DidUserCheckForFailures { get; private set; } = false;

        /// <summary>
        /// Gets a count of total number of Asserts
        /// </summary>
        protected int NumberOfAsserts { get; private set; }

        /// <summary>
        /// Gets a count of total number of Passed Asserts
        /// </summary>
        protected int NumberOfPassedAsserts { get; private set; }

        /// <summary>
        /// Gets a count of total number of Failed Asserts
        /// </summary>
        protected int NumberOfFailedAsserts { get; private set; }

        /// <summary>
        /// Gets the logger being used
        /// </summary>
        protected Logger Log { get; private set; }

        /// <summary>
        /// Override the logger
        /// </summary>
        /// <param name="log">The new logger</param>
        public void OverrideLogger(Logger log)
        {
            this.Log = log;
        }

        /// <summary>
        /// Gets a value indicating whether the boolean if the user checks for failures at the end of the test.
        /// </summary>
        /// <returns>If the user checked for failures.  If the number of asserts is 0, it returns true.</returns>
        public virtual bool DidUserCheck()
        {
            if (this.NumberOfAsserts > 0)
            {
                return this.DidUserCheckForFailures;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Check if there are any failed soft asserts.
        /// </summary>
        /// <returns>True if there are failed soft asserts</returns>
        public virtual bool DidSoftAssertsFail()
        {
            return this.NumberOfFailedAsserts > 0;
        }

        /// <summary>
        /// Asserts if two strings are equal
        /// </summary>
        /// <param name="expectedText">Expected value of the string </param>
        /// <param name="actualText">Actual value of the string</param>
        /// <param name="message">Message to be used when logging</param>
        /// <returns>Boolean if they are equal</returns>
        [Obsolete("SoftAssert.AreEqual will be deprecated in MAQS 7.0.  Please use SoftAssert.Assert() instead")]
        public virtual bool AreEqual(string expectedText, string actualText, string message = "")
        {
            return this.AreEqual(expectedText, actualText, string.Empty, message);
        }

        /// <summary>
        /// Asserts if two strings are equal
        /// </summary>
        /// <param name="expectedText">Expected value of the string </param>
        /// <param name="actualText">Actual value of the string</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="message">Message to be used when logging</param>
        /// <returns>Boolean if they are equal</returns>
        [Obsolete("SoftAssert.AreEqual will be deprecated in MAQS 7.0.  Please use SoftAssert.Assert() instead")]
        public virtual bool AreEqual(string expectedText, string actualText, string softAssertName, string message = "")
        {
            void test()
            {
                if (expectedText != actualText)
                {
                    if (string.IsNullOrEmpty(message))
                    {
                        throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.AreEqual failed for {0}.  Expected '{1}' but got '{2}'", softAssertName, expectedText, actualText));
                    }

                    throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.AreEqual failed for {0}.  Expected '{1}' but got '{2}'.  {3}", softAssertName, expectedText, actualText, message));
                }
            }

            return this.InvokeTest(test, expectedText, actualText, message);
        }

        /// <summary>
        /// Soft assert for IsTrue
        /// </summary>
        /// <param name="condition">Boolean condition</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>Boolean if condition is met</returns>
        [Obsolete("SoftAssert.IsTrue will be deprecated in MAQS 7.0.  Please use SoftAssert.Assert() instead")]

        public virtual bool IsTrue(bool condition, string softAssertName, string failureMessage = "")
        {
            void test()
            {
                if (!condition)
                {
                    if (string.IsNullOrEmpty(failureMessage))
                    {
                        throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.IsTrue failed for: {0}", softAssertName));
                    }

                    throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.IsTrue failed for: {0}. {1}", softAssertName, failureMessage));
                }
            }

            return this.InvokeTest(test, softAssertName, failureMessage);
        }

        /// <summary>
        /// Soft assert for IsFalse
        /// </summary>
        /// <param name="condition">Boolean condition</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>Boolean if condition is met</returns>
        [Obsolete("SoftAssert.IsFalse will be deprecated in MAQS 7.0.  Please use SoftAssert.Assert() instead")]

        public virtual bool IsFalse(bool condition, string softAssertName, string failureMessage = "")
        {
            void test()
            {
                if (condition)
                {
                    if (string.IsNullOrEmpty(failureMessage))
                    {
                        throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.IsFalse failed for: {0}", softAssertName));
                    }

                    throw new SoftAssertException(StringProcessor.SafeFormatter("SoftAssert.IsFalse failed for: {0}. {1}", softAssertName, failureMessage));
                }
            }

            return this.InvokeTest(test, softAssertName, failureMessage);
        }

        /// <summary>
        /// Log final assert count summary
        /// </summary>
        public virtual void LogFinalAssertData()
        {
            StringBuilder message = new StringBuilder();
            MessageType type;

            message.AppendLine(StringProcessor.SafeFormatter(
                "Total number of Asserts: {0}. {3}Passed Asserts = {1} {3}Failed Asserts = {2}{3}",
                this.NumberOfAsserts,
                this.NumberOfPassedAsserts,
                this.NumberOfFailedAsserts,
                Environment.NewLine));

            if (this.listOfExceptions.Count > 0)
            {
                type = MessageType.ERROR;
                message.AppendLine("List of failed exceptions:");

                foreach (string exception in this.listOfExceptions)
                {
                    // Will log all the exceptions that were caught in Asserts to the log file.
                    message.AppendLine(exception);
                }
            }
            else
            {
                // There are no exceptions that were caught in Asserts.
                type = MessageType.INFORMATION;
                message.AppendLine("There are no failed exceptions in the Asserts.");
            }

            this.Log.LogMessage(type, message.ToString().TrimEnd());
        }

        /// <summary>
        /// Fail test if there were one or more failures
        /// </summary>
        [AssertionMethod]
        public void FailTestIfAssertFailed()
        {
            this.FailTestIfAssertFailed("*See log for more details");
        }

        /// <summary>
        /// Fail test if there were one or more failures
        /// </summary>
        /// <param name="message">Customer error message</param>
        [AssertionMethod]
        public void FailTestIfAssertFailed(string message)
        {
            this.CheckForExpectedAsserts();
            this.LogFinalAssertData();
            this.DidUserCheckForFailures = true;

            if (this.DidSoftAssertsFail())
            {
                string errors = string.Join(Environment.NewLine, this.listOfExceptions);
                throw new AggregateException("Soft Asserts failed:" + Environment.NewLine + errors + Environment.NewLine + message);
            }
        }

        internal void CheckForExpectedAsserts()
        {
            foreach(var expectedAssert in _expectedAsserts)
            {
                if(!_calledAsserts.Contains(expectedAssert))
                {
                    this.NumberOfAsserts++;
                    this.NumberOfFailedAsserts++;
                    this.listOfExceptions.Add($"Error: failed to call assert with key {expectedAssert}");
                }
            }
        }

        /// <summary>
        /// Wrap an assert inside a soft assert
        /// </summary>
        /// <param name="assertFunction">The assert function</param>
        /// <param name="assertCalledKey">Key of expected assert being called.</param>
        /// <returns>True if the asset passed</returns>
        public bool Assert(Action assertFunction, string assertCalledKey = null)
        {
            if(!string.IsNullOrEmpty(assertCalledKey) && _expectedAsserts.Any())
            {
                _calledAsserts.Add(assertCalledKey);
            }

            // Resetting every time we invoke a test to verify the user checked for failures
            this.DidUserCheckForFailures = false;
            bool result = false;

            try
            {
                assertFunction.Invoke();
                this.NumberOfPassedAsserts = ++this.NumberOfPassedAsserts;
                result = true;
                this.Log.LogMessage(MessageType.SUCCESS, "SoftAssert passed for: {0}.", assertFunction.Method.Name);
            }
            catch (Exception ex)
            {
                this.NumberOfFailedAsserts = ++this.NumberOfFailedAsserts;
                result = false;
                this.Log.LogMessage(MessageType.WARNING, "SoftAssert failed for: {0}. {1}", assertFunction.Method.Name, ex.Message);
                this.listOfExceptions.Add(ex.Message);
            }
            finally
            {
                this.NumberOfAsserts = ++this.NumberOfAsserts;
            }

            return result;
        }

        /// <summary>
        /// Wrap an assert that is expected to fail and the expected failure
        /// </summary>
        /// <param name="assertFunction">The assert function</param>
        /// <param name="expectedException">The type of expected exception</param>
        /// <param name="assertName">soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>True if the assert failed</returns>
        public bool AssertFails(Action assertFunction, Type expectedException, string assertName, string failureMessage = "")
        {
            // Resetting every time we invoke a test to verify the user checked for failures
            this.DidUserCheckForFailures = false;
            bool result = false;

            try
            {
                assertFunction.Invoke();
                this.NumberOfFailedAsserts = ++this.NumberOfFailedAsserts;
                result = false;
                this.Log.LogMessage(MessageType.WARNING, "SoftAssert failed for assert {0}:  {1} passed.  Expected failure type {2}.", assertName, assertFunction.Method.Name, expectedException);
            }
            catch (Exception ex)
            {
                if (ex.GetType().Equals(expectedException))
                {
                    this.NumberOfPassedAsserts = ++this.NumberOfPassedAsserts;
                    result = true;
                    this.Log.LogMessage(MessageType.SUCCESS, "SoftAssert passed for assert {0}: {1}.", assertName, assertFunction.Method.Name);
                }
                else
                {
                    this.NumberOfFailedAsserts = ++this.NumberOfFailedAsserts;
                    result = false;
                    this.Log.LogMessage(MessageType.WARNING, "SoftAssert failed for assert {0}: {1}. Expected failure:{2} Actual failure: {3}", assertName, assertFunction.Method.Name, expectedException, ex.Message);
                    this.listOfExceptions.Add(ex.Message);
                }
            }
            finally
            {
                this.NumberOfAsserts = ++this.NumberOfAsserts;
            }

            return result;
        }

        /// <summary>
        /// Executes the assert type passed as parameter and updates the total assert count
        /// </summary>
        /// <param name="test">Test method Action </param>
        /// <param name="expectedText">Expected value of the string </param>
        /// <param name="actualText">Actual value of the string</param>
        /// <param name="message">Test Name or Message</param>
        /// <returns>Boolean if the assert is true</returns>
        private bool InvokeTest(Action test, string expectedText, string actualText, string message)
        {
            // Resetting every time we invoke a test to verify the user checked for failures
            this.DidUserCheckForFailures = false;
            bool result = false;

            try
            {
                test.Invoke();
                this.NumberOfPassedAsserts = ++this.NumberOfPassedAsserts;
                result = true;
                this.LogMessage(expectedText, actualText, message, result);
            }
            catch (Exception ex)
            {
                this.NumberOfFailedAsserts = ++this.NumberOfFailedAsserts;
                result = false;
                this.LogMessage(expectedText, actualText, message, result);
                this.listOfExceptions.Add(ex.Message);
            }
            finally
            {
                this.NumberOfAsserts = ++this.NumberOfAsserts;
            }

            return result;
        }

        /// <summary>
        /// Executes the assert type passed as parameter and updates the total assert count
        /// </summary>
        /// <param name="test">Test method Action </param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="message">Test Name or Message</param>
        /// <returns>Boolean if the assert is true</returns>
        private bool InvokeTest(Action test, string softAssertName, string message)
        {
            // Resetting every time we invoke a test to verify the user checked for failures
            this.DidUserCheckForFailures = false;
            bool result = false;

            try
            {
                test.Invoke();
                this.NumberOfPassedAsserts = ++this.NumberOfPassedAsserts;
                result = true;
                this.Log.LogMessage(MessageType.SUCCESS, "SoftAssert passed for: {0}.", softAssertName);
            }
            catch (Exception ex)
            {
                this.NumberOfFailedAsserts = ++this.NumberOfFailedAsserts;
                result = false;
                this.Log.LogMessage(MessageType.WARNING, "SoftAssert failed for: {0}. {1}", softAssertName, message);
                this.listOfExceptions.Add(ex.Message);
            }
            finally
            {
                this.NumberOfAsserts = ++this.NumberOfAsserts;
            }

            return result;
        }

        /// <summary>
        /// Logs the message to the logger
        /// </summary>
        /// <param name="expectedText">Expected value of the string </param>
        /// <param name="actualText">Actual value of the string</param>
        /// <param name="message">Test Name or Message</param>
        /// <param name="result">Decides the message type to be logged</param>
        private void LogMessage(string expectedText, string actualText, string message, bool result)
        {
            if (result)
            {
                this.Log.LogMessage(MessageType.SUCCESS, StringProcessor.SafeFormatter("Soft Assert '{0}' passed. Expected Value = '{1}', Actual Value = '{2}'.", message, expectedText, actualText));
            }
            else
            {
                this.Log.LogMessage(MessageType.WARNING, StringProcessor.SafeFormatter("Soft Assert '{0}' failed. Expected Value = '{1}', Actual Value = '{2}'.", message, expectedText, actualText));
            }
        }

        /// <summary>
        /// Add expected assertions to be called by this soft assert instance.
        /// </summary>
        /// <param name="expectedAsserts">Expected Assertions to be called.</param>
        public void AddExpectedAsserts(params string[] expectedAsserts)
        {
            foreach (var expectedAssert in expectedAsserts)
            {
                _expectedAsserts.Add(expectedAssert);
            }
        }

        /// <summary>
        /// Look for SoftAssertExpectedAssert attribute on the test method.
        /// </summary>
        /// <param name="testMethod">The Method information of the currently running test.</param>
        public void CaptureTestMethodAttributes(MethodInfo testMethod)
        {
            foreach (var attr in Attribute.GetCustomAttributes(testMethod).Where(a => a is SoftAssertExpectedAssertsAttribute))
            {
                var expectedAssertAttribute = attr as SoftAssertExpectedAssertsAttribute;
                AddExpectedAsserts(expectedAssertAttribute.ExpectedAssertKeys);
            }
        }
    }
}
