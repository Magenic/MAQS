//--------------------------------------------------
// <copyright file="GenericWaitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit tests for the generic wait</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Test class for soft asserts
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GenericWaitTests
    {
        /// <summary>
        /// Constant test string
        /// </summary>
        private const string TESTSTRING = "Test String";

        /// <summary>
        /// Test override retry time
        /// </summary>
        private static readonly TimeSpan TESTRETRY = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// Test override time out time
        /// </summary>
        private static readonly TimeSpan TESTTIMEOUT = TimeSpan.FromMilliseconds(500);

        /// <summary>
        /// Counter for unit tests
        /// </summary>
        private static int number = 0;

        /// <summary>
        /// Bool for unit tests
        /// </summary>
        private static bool initialReturnValue = false;

        /// <summary>
        /// Test wait until with no parameters works when the wait function returns true
        /// </summary>
        #region WaitUntil
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PassNoParamUntilTest()
        {
            int loop = 0;

            Assert.IsTrue(GenericWait.WaitUntil(() => loop++ > 3), "Failed no parameter test");
        }
        #endregion

        /// <summary>
        /// Test wait for with no parameters works when the wait function returns true
        /// </summary>
        #region WaitFor
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassNoParamForTest()
        {
            initialReturnValue = false;
            GenericWait.WaitFor(IsNotParamTest);
        }
        #endregion

        /// <summary>
        /// Test wait until with one parameter works when the wait function returns true
        /// </summary>
        #region WaitUntilWithType
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassStringUntilTest()
        {
            number = 0;
            Assert.IsTrue(GenericWait.WaitUntil<string>(this.IsParamTestString, TESTSTRING + "3"), "Failed single parameter test");
        }
        #endregion

        #region WaitUntilFunctionEqualsExpected
        /// <summary>
        /// Test wait until function returns expected value, then returns the value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassStringsEqual()
        {
            number = 0;
            Assert.IsTrue(GenericWait.WaitUntilMatch<string>(this.FunctionTestString, "Test String3").Equals("Test String3"), "Failed expected parameter test.");
        }

        /// <summary>
        /// Test wait until function returns expected value, then returns the value
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassStringsEqualOverride()
        {
            number = 0;
            Assert.IsTrue(GenericWait.WaitUntilMatch<string>(this.FunctionTestString, TESTRETRY, TESTTIMEOUT, "Test String3").Equals("Test String3"), "Failed expected parameter test.");
        }
        #endregion

        #region WaitForFunctionEqualsExpected
        /// <summary>
        /// Test wait until function returns expected value, throws an exception if a timeout occurs
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassStringWaitFor()
        {
            int number = 0;
            GenericWait.WaitForMatch<string>(() => TESTSTRING + ++number, TESTSTRING + "3");
        }

        /// <summary>
        /// Tests waits checking that the function returns a value equal to the input value until the input test retry and test timeout before throwing an exception.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassStringWaitForOverride()
        {
            number = 0;
            GenericWait.WaitForMatch<string>(this.FunctionTestString, TESTRETRY, TESTTIMEOUT, TESTSTRING + "3");
        }
        #endregion

        /// <summary>
        /// Test wait for with one parameter works when the wait function returns true
        /// </summary>
        #region WaitForWithType
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DoNotParallelize]
        public void PassStringForTest()
        {
            int number = 0;
            GenericWait.WaitFor<string>((p) => p.Equals(TESTSTRING + number++), TESTSTRING + "3");
        }
        #endregion

        /// <summary>
        /// Test wait until with an array of parameters works when the wait function returns true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PassObjectArrayUntilTest()
        {
            List<object> objects = new List<object>();
            objects.Add((object)"one");
            objects.Add((object)new Dictionary<int, Guid>());

            Assert.IsTrue(GenericWait.WaitUntil<object[]>(this.IsTwoParameters, objects.ToArray()), "Failed parameter array test");
        }

        /// <summary>
        /// Test wait for with an array of parameters works when the wait function returns true
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PassObjectArrayForTest()
        {
            List<object> objects = new List<object>();
            objects.Add((object)"one");
            objects.Add((object)new Dictionary<int, Guid>());

            GenericWait.WaitFor<object[]>(this.IsTwoParameters, objects.ToArray());
        }

        /// <summary>
        /// Test wait until with a single parameter works when the wait function returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FailStringUntilTest()
        {
            Assert.IsFalse(GenericWait.WaitUntil<string>(this.IsParamTestString, "Bad"), "Failed sigle parameter test");
        }

        /// <summary>
        /// Test wait until with a parameter array works when the wait function returns false
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FailObjectArrayUntilTest()
        {
            List<object> objects = new List<object>();
            Assert.IsFalse(GenericWait.WaitUntil<object[]>(this.IsTwoParameters, objects.ToArray()), "Failed parameter array test");
        }

        /// <summary>
        /// Test wait for with no parameters returns the function exception when the check times out
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(NotImplementedException))]
        public void ThrowExceptionWithoutParamTest()
        {
            GenericWait.WaitFor(this.ThrowError);
        }

        /// <summary>
        /// Test wait for with no parameters returns the a timeout exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(TimeoutException))]
        public void ThrowTimeoutExceptionWithoutParamTest()
        {
            GenericWait.WaitFor(this.IsParamTest);
        }

        /// <summary>
        /// Test wait for with a parameter returns the function exception when the check times out
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(SystemException))]
        public void ThrowExceptionWithParamTest()
        {
            GenericWait.WaitFor<string>(this.ThrowError, TESTSTRING);
        }

        /// <summary>
        /// Test wait for with parameters returns the a timeout exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(TimeoutException))]
        public void ThrowTimeoutExceptionWithParamTest()
        {
            GenericWait.WaitFor<object[]>(this.IsTwoParameters, (new List<object>()).ToArray());
        }

        /// <summary>
        /// Test wait without parameters returns function exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(NotImplementedException))]
        public void ThrowExceptionWithoutParamWithCustomTimesTest()
        {
            GenericWait.Wait(this.ThrowError, TESTRETRY, TESTTIMEOUT, true);
        }

        /// <summary>
        /// Test wait with parameters returns function exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(SystemException))]
        public void ThrowExceptionWithParamWithCustomTimesTest()
        {
            GenericWait.Wait<string>(this.ThrowError, TESTRETRY, TESTTIMEOUT, true, "Anything");
        }

        /// <summary>
        /// Verify custom timeout without parameters works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CustomTimeoutWithoutParamTest()
        {
            DateTime start = DateTime.Now;
            TimeSpan max = TESTTIMEOUT + TESTRETRY;

            GenericWait.Wait(this.IsParamTest, TESTRETRY, TESTTIMEOUT, false);
            TimeSpan duration = DateTime.Now - start;

            Assert.IsTrue(duration < max, "The max wait time should be no more than " + max + " but was " + duration);
        }

        /// <summary>
        /// Verify custom timeout with parameters works
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void CustomTimeoutWithParamTest()
        {
            DateTime start = DateTime.Now;
            TimeSpan max = TESTTIMEOUT + TESTRETRY;

            GenericWait.Wait<string>(this.IsParamTestString, TESTRETRY, TESTTIMEOUT, false, "bad");
            TimeSpan duration = DateTime.Now - start;

            Assert.IsTrue(duration < max, "The max wait time should be no more than " + max + " but was " + duration);
        }

        /// <summary>
        /// Verify that Wait with input parameter throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(TimeoutException))]
        public void WaitForFunctionWithInputExceptionThrown()
        {
            GenericWait.Wait<bool, string>(this.ThrowError, TESTRETRY, TESTTIMEOUT, "input");
        }

        /// <summary>
        /// Verify that WaitFor returns the correct value of its called function
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void WaitForTest()
        {
            Assert.IsFalse(GenericWait.WaitFor<bool>(this.IsParamTest));
        }

        /// <summary>
        /// Verify that Wait without input parameter throws expected exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(TimeoutException))]
        public void WaitForFunctionWithoutInputExceptionThrown()
        {
            GenericWait.Wait<bool>(this.ThrowError, TESTRETRY, TESTTIMEOUT);
        }

        /// <summary>
        /// Test function that always returns true
        /// </summary>
        /// <returns>Always returns true</returns>
        private static bool IsNotParamTest()
        {
            initialReturnValue = !initialReturnValue;
            return !initialReturnValue;
        }
        
        /// <summary>
        /// Test function that always returns false
        /// </summary>
        /// <returns>Always returns false</returns>
        private bool IsParamTest()
        {
            return false;
        }

        /// <summary>
        /// Test function that checks if the test string passed in is the same as the constant test string
        /// </summary>
        /// <param name="testString">The test string</param>
        /// <returns>True if the constant and passed in test strings match</returns>
        private bool IsParamTestString(string testString)
        {
            return testString.Equals(TESTSTRING + number++);
        }

        /// <summary>
        /// Test function that checks if the object array passed in is in the form expected 
        /// </summary>
        /// <param name="parameters">Object array</param>
        /// <returns>True if the array is in the form expected</returns>
        private bool IsTwoParameters(object[] parameters)
        {
            if (parameters.Length == 2 && parameters[0] is string && parameters[1] is Dictionary<int, Guid>)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Test function that always returns a specific string
        /// </summary>
        /// <returns>Always returns a specific string</returns>
        private string FunctionTestString()
        {
            return TESTSTRING + number++;
        }

        /// <summary>
        /// Test function that always throws a not implemented exception
        /// </summary>
        /// <returns>Always throws an exception</returns>
        private bool ThrowError()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Test function that always throws a not implemented exception
        /// </summary>
        /// <param name="testString">Test string</param>
        /// <returns>Always throws an exception</returns>
        private bool ThrowError(string testString)
        {
            throw new SystemException();
        }
    }
}
