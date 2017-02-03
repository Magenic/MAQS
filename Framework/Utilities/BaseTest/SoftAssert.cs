//--------------------------------------------------
// <copyright file="SoftAssert.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the SoftAssert class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Logging;
using System;
using System.Collections.Generic;

namespace Magenic.MaqsFramework.Utilities.BaseTest
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
        /// Boolean if the user checked for failures
        /// </summary>
        private bool didUserCheckForFailures = false;

        /// <summary>
        /// List of all asserted exceptions 
        /// </summary>
        private List<string> listOfExceptions = new List<string>();

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
        /// Gets a value indicating whether the boolean if the user checks for failures at the end of the test.
        /// </summary>
        /// <returns>If the user checked for failures.  If the number of asserts is 0, it returns true.</returns>
        public bool DidUserCheck()
        {
            if (this.NumberOfAsserts > 0)
            {
                return this.didUserCheckForFailures;
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
        public bool DidSoftAssertsFail()
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
        public virtual bool AreEqual(string expectedText, string actualText, string softAssertName, string message)
        {
            Action test = () =>
            {
                if (expectedText != actualText)
                {
                    if (string.IsNullOrEmpty(message))
                    {
                        throw new Exception(StringProcessor.SafeFormatter("SoftAssert.AreEqual failed for {0}.  Expected '{1}' but got '{2}'", softAssertName, expectedText, actualText));
                    }

                    throw new Exception(StringProcessor.SafeFormatter("SoftAssert.AreEqual failed for {0}.  Expected '{1}' but got '{2}'.  {3}", softAssertName, expectedText, actualText, message));
                }
            };
            return this.InvokeTest(test, expectedText, actualText, message);
        }

        /// <summary>
        /// Soft assert for IsTrue
        /// </summary>
        /// <param name="condition">Boolean condition</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>Boolean if condition is met</returns>
        public virtual bool IsTrue(bool condition, string softAssertName, string failureMessage = "")
        {
            Action test = () =>
            {
                if (!condition)
                {
                    if (string.IsNullOrEmpty(failureMessage))
                    {
                        throw new Exception(StringProcessor.SafeFormatter("SoftAssert.IsTrue failed for: {0}", softAssertName));
                    }

                    throw new Exception(StringProcessor.SafeFormatter("SoftAssert.IsTrue failed for: {0}. {1}", softAssertName, failureMessage));
                }
            };
            return this.InvokeTest(test, softAssertName, failureMessage);
        }

        /// <summary>
        /// Soft assert for IsFalse
        /// </summary>
        /// <param name="condition">Boolean condition</param>
        /// <param name="softAssertName">Soft assert name</param>
        /// <param name="failureMessage">Failure message</param>
        /// <returns>Boolean if condition is met</returns>
        public virtual bool IsFalse(bool condition, string softAssertName, string failureMessage = "")
        {
            Action test = () =>
            {
                if (condition)
                {
                    if (string.IsNullOrEmpty(failureMessage))
                    {
                        throw new Exception(StringProcessor.SafeFormatter("SoftAssert.IsFalse failed for: {0}", softAssertName));
                    }

                    throw new Exception(StringProcessor.SafeFormatter("SoftAssert.IsFalse failed for: {0}. {1}", softAssertName, failureMessage));
                }
            };
            return this.InvokeTest(test, softAssertName, failureMessage);
        }

        /// <summary>
        /// Log final assert count summary
        /// </summary>
        public void LogFinalAssertData()
        {
            this.Log.LogMessage(
                MessageType.INFORMATION,
                "Total number of Asserts: {0}. Passed Asserts = {1} Failed Asserts = {2}",
                this.NumberOfAsserts,
                this.NumberOfPassedAsserts,
                this.NumberOfFailedAsserts);

            if (this.listOfExceptions.Count > 0)
            {
                this.Log.LogMessage(MessageType.ERROR, "List of failed exceptions:");

                foreach (string exception in this.listOfExceptions)
                {
                    // Will log all the exceptions that were caught in Asserts to the log file.
                    this.Log.LogMessage(MessageType.ERROR, exception);
                }
            }
            else
            {
                // There are no exceptions that were caught in Asserts.
                this.Log.LogMessage(MessageType.INFORMATION, "There are no failed exceptions in the Asserts.");
            }
        }

        /// <summary>
        /// Fail test if there were one or more failures
        /// </summary>
        public void FailTestIfAssertFailed()
        {
            this.LogFinalAssertData();
            this.didUserCheckForFailures = true;

            if (this.DidSoftAssertsFail())
            {
                string errors = string.Join(Environment.NewLine, this.listOfExceptions);
                throw new AggregateException("Soft Asserts failed:" + Environment.NewLine + errors + Environment.NewLine + "*See log for more details");
            }
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
            this.didUserCheckForFailures = false;
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
            this.didUserCheckForFailures = false;
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
