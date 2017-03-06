//--------------------------------------------------
// <copyright file="GenericWait.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Generic wait</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Threading;

namespace Magenic.MaqsFramework.Utilities.Helper
{
    /// <summary>
    /// Generic wait class
    /// </summary>
    public static class GenericWait
    {
        /// <summary>
        /// Default retry time for the configuration file
        /// </summary>
        private static TimeSpan retryTimeFromConfig = TimeSpan.FromMilliseconds(Convert.ToInt32(Config.GetValue("WaitTime", "0")));

        /// <summary>
        /// Default timeout time from the configuration file
        /// </summary>
        private static TimeSpan timeoutFromConfig = TimeSpan.FromMilliseconds(Convert.ToInt32(Config.GetValue("Timeout", "0")));

        /// <summary>
        /// Wait until the wait for true function returns true or times out
        /// </summary>
        /// <typeparam name="T">The type of the parameter to pass to the wait for true function</typeparam>
        /// <param name="waitForTrue">The function we are waiting to return true</param>
        /// <param name="arg">Parameter to pass to the wait for true function</param>
        /// <returns>True if the waitForTrue function returned true before the timeout</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitUntilWithType" lang="C#" />
        /// </example>
        public static bool WaitUntil<T>(Func<T, bool> waitForTrue, T arg)
        {
            return Wait(waitForTrue, retryTimeFromConfig, timeoutFromConfig, false, arg);
        }

        /// <summary>
        /// Wait until the wait for true function returns true or times out
        /// </summary>
        /// <param name="waitForTrue">The function we are waiting to return true</param>
        /// <returns>True if the wait for true function returned true before timing out</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitUntil" lang="C#" />
        /// </example>
        public static bool WaitUntil(Func<bool> waitForTrue)
        {
            return Wait(waitForTrue, retryTimeFromConfig, timeoutFromConfig, false);
        }

        /// <summary>
        /// Wait until the wait for true function returns true, an exception will be thrown if the wait times out
        /// </summary>
        /// <param name="waitForTrue">The function we are waiting to return true</param>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitFor" lang="C#" />
        /// </example>
        public static void WaitFor(Func<bool> waitForTrue)
        {
            if (!Wait(waitForTrue, retryTimeFromConfig, timeoutFromConfig, true))
            {
                throw new TimeoutException(StringProcessor.SafeFormatter("Timed out waiting for '{0}' to return true", waitForTrue.Method.Name));
            }
        }

        /// <summary>
        /// Wait until the wait for true function returns true, an exception will be thrown if the wait times out
        /// </summary>
        /// <typeparam name="T">The type of parameter to pass in the the wait for true function</typeparam>
        /// <param name="waitForTrue">The function we are waiting to return true</param>
        /// <param name="arg">Parameter to pass to the wait for true function</param>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitForWithType" lang="C#" />
        /// </example>
        public static void WaitFor<T>(Func<T, bool> waitForTrue, T arg)
        {
            if (!Wait(waitForTrue, retryTimeFromConfig, timeoutFromConfig, true, arg))
            {
                throw new TimeoutException(StringProcessor.SafeFormatter("Timed out waiting for '{0}' to return true", waitForTrue.Method.Name));
            }
        }

        /// <summary>
        /// Wait until the wait for function returns the expected type, an exception will be thrown if the wait times out
        /// </summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <param name="waitFor">The wait for function</param>
        /// <returns>The wait for function return value</returns>
        public static T WaitFor<T>(Func<T> waitFor)
        {
            return Wait(waitFor, retryTimeFromConfig, timeoutFromConfig);
        }

        /// <summary>
        /// Wait until the wait for function returns the expected type, an exception will be thrown if the wait times out
        /// </summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <typeparam name="U">Wait for argument type</typeparam>
        /// <param name="waitFor">The wait for function</param>
        /// <param name="arg">The wait for function argument</param>
        /// <returns>The wait for function return value</returns>
        public static T WaitFor<T, U>(Func<U, T> waitFor, U arg)
        {
            return Wait(waitFor, retryTimeFromConfig, timeoutFromConfig, arg);
        }

        /// <summary>
        /// Wait until the wait for true function returns true or times out
        /// </summary>
        /// <typeparam name="T">The type of the parameter to pass to the wait for true function</typeparam>
        /// <param name="waitForTrue">The function we are waiting to return true</param>
        /// <param name="retryTime">How long do we wait before retrying the wait for true function</param>
        /// <param name="timeout">Max timeout for the check</param>
        /// <param name="throwException">If the last check failed because of an exception should we throw the exception</param>
        /// <param name="arg">Parameter to pass to the wait for true function</param>
        /// <returns>True if the wait for true function returned true before timing out</returns>
        public static bool Wait<T>(Func<T, bool> waitForTrue, TimeSpan retryTime, TimeSpan timeout, bool throwException, T arg)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;
            Exception exception = null;

            do
            {
                try
                {
                    // Clear out old exception
                    exception = null;

                    // Check if the function returns true
                    if (waitForTrue(arg))
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    // Save of the exception if we want to throw exceptions
                    if (throwException)
                    {
                        exception = e;
                    }
                }

                // Give the system a second before checking if the page is updating
                Thread.Sleep(retryTime);
            }
            while ((DateTime.Now - start) < timeout);

            // Check if we had an excetions 
            if (throwException && exception != null)
            {
                throw exception;
            }

            // We timed out waiting for the function to return true
            return false;
        }

        /// <summary>
        /// Wait until the wait for true function returns true or times out
        /// </summary>
        /// <param name="waitForTrue">The function we are waiting to return true</param>
        /// <param name="retryTime">How long do we wait before retrying the wait for true function</param>
        /// <param name="timeout">Max timeout for the check</param>
        /// <param name="throwException">If the last check failed because of an exception should we throw the exception</param>
        /// <returns>True if the wait for true function returned true before timing out</returns>
        public static bool Wait(Func<bool> waitForTrue, TimeSpan retryTime, TimeSpan timeout, bool throwException)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;
            Exception exception = null;

            do
            {
                try
                {
                    // Clear out old exception
                    exception = null;

                    // Check if the function returns true
                    if (waitForTrue())
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    // Save of the exception if we want to throw exceptions
                    if (throwException)
                    {
                        exception = e;
                    }
                }

                // Give the system a second before checking if the page is updating
                Thread.Sleep(retryTime);
            }
            while ((DateTime.Now - start) < timeout);

            // Check if we had an excetions 
            if (throwException && exception != null)
            {
                throw exception;
            }

            // We timed out waiting for the function to return true
            return false;
        }

        /// <summary>
        /// Wait until the wait for function returns the expected type, an exception will be thrown if the wait times out
        /// </summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <param name="waitFor">The wait for function</param>
        /// <param name="retryTime">How long do we wait before retrying the wait for true function</param>
        /// <param name="timeout">Max timeout for the check</param>
        /// <returns>Return value of the wait for function</returns>
        public static T Wait<T>(Func<T> waitFor, TimeSpan retryTime, TimeSpan timeout)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;
            Exception exception = null;

            do
            {
                try
                {
                    // Clear out old exception
                    exception = null;
                    return waitFor();
                }
                catch (Exception e)
                {
                    exception = e;
                }

                // Give the system a second before checking if the page is updating
                Thread.Sleep(retryTime);
            }
            while ((DateTime.Now - start) < timeout);

            throw new Exception("Timed out waiting for " + waitFor.Method.Name + " to return", exception);
        }

        /// <summary>
        /// Wait until the wait for function returns the expected type, an exception will be thrown if the wait times out
        /// </summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <typeparam name="U">Wait for argument type</typeparam>
        /// <param name="waitFor">The wait for function</param>
        /// <param name="retryTime">How long do we wait before retrying the wait for true function</param>
        /// <param name="timeout">Max timeout for the check</param>
        /// <param name="arg">Arguments to pass into the wait for function</param>
        /// <returns>Return value of the wait for function</returns>
        public static T Wait<T, U>(Func<U, T> waitFor, TimeSpan retryTime, TimeSpan timeout, U arg)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;
            Exception exception = null;

            do
            {
                try
                {
                    // Clear out old exception
                    exception = null;

                    return waitFor(arg);
                }
                catch (Exception e)
                {
                    exception = e;
                }

                // Give the system a second before checking if the page is updating
                Thread.Sleep(retryTime);
            }
            while ((DateTime.Now - start) < timeout);

            throw new Exception("Timed out waiting for " + waitFor.Method.Name + " to return", exception);
        }
    }
}
