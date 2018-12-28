//--------------------------------------------------
// <copyright file="SoftAssert.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the SoftAssert class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// SoftAssert class
    /// </summary>
    /// <example>
    /// <code source="../SeleniumUnitTesting/SeleniumUnitTest.cs" region="SoftAssertAreEqual" lang="C#" />
    /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertIsTrue" lang="C#" />
    /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertIsFalse" lang="C#" />
    /// </example>
    public class SoftAssert
    {
        /// <summary>
        /// List of all asserted exceptions 
        /// </summary>
        private readonly List<string> listOfExceptions = new List<string>();

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
        /// Gets if the user checked for failures
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertChecked" lang="C#" />
        /// </example>
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertDidFail" lang="C#" />
        /// </example>
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertAreEqualPasses" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertAreEqualFails" lang="C#" />
        /// </example>
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertAreEqualPasses" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertAreEqualFails" lang="C#" />
        /// </example>
        public virtual bool AreEqual(string expectedText, string actualText, string softAssertName, string message)
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertIsTrue" lang="C#" />
        /// </example>
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertIsFalse" lang="C#" />
        /// </example>
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="VSAssertFail" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="NUnitAssertFail" lang="C#" />
        /// </example>
        public void FailTestIfAssertFailed()
        {
            this.FailTestIfAssertFailed("*See log for more details");
        }

        /// <summary>
        /// Fail test if there were one or more failures
        /// </summary>
        /// <param name="message">Customer error message</param>
        public void FailTestIfAssertFailed(string message)
        {
            this.LogFinalAssertData();
            this.DidUserCheckForFailures = true;

            if (this.DidSoftAssertsFail())
            {
                string errors = string.Join(Environment.NewLine, this.listOfExceptions);
                throw new AggregateException("Soft Asserts failed:" + Environment.NewLine + errors + Environment.NewLine + message);
            }
        }

        /// <summary>
        /// Wrap an assert inside a soft assert
        /// </summary>
        /// <param name="assertFunction">The assert function</param>
        /// <returns>True if the asset passed</returns>
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="VSAssert" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="NUnitAssert" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="VSAssertFail" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="NUnitAssertFail" lang="C#" />
        /// </example>
        public bool Assert(Action assertFunction)
        {
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
        /// <example>
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertFailsPasses" lang="C#" />
        /// <code source="../UtilitiesUnitTests/SoftAssertUnitTests.cs" region="SoftAssertFailsFails" lang="C#" />
        /// </example>
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
    }
}
