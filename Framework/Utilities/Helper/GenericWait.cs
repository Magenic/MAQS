//--------------------------------------------------
// <copyright file="GenericWait.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Generic wait</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Collections.Generic;
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
        /// Waits for a function with a return type T to return a value that is to an argument of the same type.  If it times out it returns the value of the function.
        /// </summary>
        /// <typeparam name="T">Type returned</typeparam>
        /// <param name="waitForTrue">Function that returns type T</param>
        /// <param name="comparativeValue">value of the same type as T</param>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitUntilFunctionEqualsExpected" lang="C#" />
        /// </example>
        /// <returns>if it returned before the timeout occurred</returns>
        public static T WaitUntilMatch<T>(Func<T> waitForTrue, T comparativeValue)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;
            
            // Checks if the two values are equal
            bool paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);
            
            // While the params are not equal & the timout hasn't met, keep checking
            while (!paramsAreEqual && (DateTime.Now - start) < timeoutFromConfig)
            {
                // Check if they are equal (running them through another function because we can't use an operator with T
                paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

                // If they aren't, wait
                Thread.Sleep(retryTimeFromConfig);
            }

            // return the value regardless
            return waitForTrue();
        }

        /// <summary>
        /// Waits for a function with a return type T to return a value that is to an argument of the same type.  If it times out it returns the value of the function.
        /// </summary>
        /// <typeparam name="T">Type returned</typeparam>
        /// <param name="waitForTrue">Function that returns type T</param>
        /// <param name="retryTime">time to wait between retries</param>
        /// <param name="timeout">how long before timing out</param>
        /// <param name="comparativeValue">value of the same type as T</param>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitUntilFunctionEqualsExpected" lang="C#" />
        /// </example>
        /// <returns>if it returned before the timeout occurred</returns>
        public static T WaitUntilMatch<T>(Func<T> waitForTrue, TimeSpan retryTime, TimeSpan timeout, T comparativeValue)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;

            // Checks if the two values are equal
            bool paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

            // While the params are not equal & the timout hasn't met, keep checking
            while (!paramsAreEqual && (DateTime.Now - start) < timeout)
            {
                // Check if they are equal (running them through another function because we can't use an operator with T
                paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

                // If they aren't, wait
                Thread.Sleep(retryTime);
            }

            // return the value regardless
            return waitForTrue();
        }

        /// <summary>
        /// Waits for a Function with a type T to return a value that is equal to a comparative value of type T
        /// </summary>
        /// <typeparam name="T">The type the method returns</typeparam>
        /// <param name="waitForTrue">Method to wait for</param>
        /// <param name="comparativeValue">The value to compare to what comes out of waitForTrue</param>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitForFunctionEqualsExpected" lang="C#" />
        /// </example>
        public static void WaitForMatch<T>(Func<T> waitForTrue, T comparativeValue)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;

            // Checks if the two values are equal
            bool paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

            // While the params are not equal & the timout hasn't met, keep checking
            while (!paramsAreEqual && (DateTime.Now - start) < timeoutFromConfig)
            {
                // Check if they are equal (running them through another function because we can't use an operator with T
                paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

                // If they aren't, wait
                Thread.Sleep(retryTimeFromConfig);
            }

            if (!paramsAreEqual)
            {
                throw new Exception("Timed out waiting for " + waitForTrue.Method.Name + " to return expected value of " + typeof(T) + ": " + comparativeValue);
            }
        }

        /// <summary>
        /// Waits for a Function with a type T to return a value that is equal to a comparative value of type T
        /// </summary>
        /// <typeparam name="T">The type the method returns</typeparam>
        /// <param name="waitForTrue">Method to wait for</param>
        /// <param name="retryTime">time to wait between retries</param>
        /// <param name="timeout">how long before timing out</param>
        /// <param name="comparativeValue">The value to compare to what comes out of waitForTrue</param>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/GenericWaitTests.cs" region="WaitForFunctionEqualsExpected" lang="C#" />
        /// </example>
        public static void WaitForMatch<T>(Func<T> waitForTrue, TimeSpan retryTime, TimeSpan timeout, T comparativeValue)
        {
            // Set start time and exception holder
            DateTime start = DateTime.Now;

            // Checks if the two values are equal
            bool paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

            // While the params are not equal & the timout hasn't met, keep checking
            while (!paramsAreEqual && (DateTime.Now - start) < timeout)
            {
                // Check if they are equal (running them through another function because we can't use an operator with T
                paramsAreEqual = ParamsEqual(waitForTrue(), comparativeValue);

                // If they aren't, wait
                Thread.Sleep(retryTime);
            }

            if (!paramsAreEqual)
            {
                throw new Exception("Timed out waiting for " + waitForTrue.Method.Name + " to return expected value of " + typeof(T) + ": " + comparativeValue);
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

        /// <summary>
        /// Checks that the objects all match
        /// </summary>
        /// <param name="param">objects passed in</param>
        /// <returns>parameters are all equal as a boolean</returns>
        private static bool ParamsEqual(params object[] param)
        {
            // For each item
            foreach (var item in param)
            {
                // and each item
                foreach (var item2 in param)
                {
                    // Compare each item
                    if (item != item2)
                    {
                        // If any do not match, then they are not equal
                        return false;
                    }
                }
            }

            // If we get here, then we had no mismatches
            return true;
        }
    }
}
