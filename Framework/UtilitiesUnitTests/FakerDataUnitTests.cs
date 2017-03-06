//-----------------------------------------------------
// <copyright file="FakerDataUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test the Faker Data Functions</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// FakerData.CS Unit Tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FakerDataUnitTests
    {
        /// <summary>
        /// Validates the instant specific time is in the correct format
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void InstantSpecificTime()
        {
            string time = FakerData.GenerateInstantSpecificTime();
            Assert.IsTrue(Regex.IsMatch(time, @"\d{2}[/]\d{2}[/]\d{4}[ ]\d{2}[:]\d{2}[:]\d{2}"));
        }

        /// <summary>
        /// Validates the Id is formatted and is a id
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void UniqueIdFormatted()
        {
            Guid result;
            Assert.IsTrue(Guid.TryParse(FakerData.GenerateUniqueId(), out result));
        }

        /// <summary>
        /// Validates the id is removed of '-'s
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GuiduniqueIdNonFormatted()
        {
            Assert.IsFalse(Regex.IsMatch(FakerData.GenerateUniqueId(false), @"[-]+"));
        }

        /// <summary>
        /// Validates the Phone Number is formatted
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PhoneNumberFormatted()
        {
            string regexFormat = @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}";
            Assert.IsTrue(Regex.IsMatch(FakerData.GenerateUSPhoneNumber(true), regexFormat));
        }

        /// <summary>
        /// Validates the Phone Number is not formatted
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PhoneNumberUnformated()
        {
            string regexFormat = @"\d{10}";
            Assert.IsTrue(Regex.IsMatch(FakerData.GenerateUSPhoneNumber(), regexFormat));
        }

        /// <summary>
        /// Validates the Social Security Number is Formatted
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SocialSecurityNumberFormatted()
        {
            string regexFormat = @"\d{3}-{0,1}\d{2}-{0,1}\d{4}";
            Assert.IsTrue(Regex.IsMatch(FakerData.GenerateSocialSecurityNumber(true), regexFormat));
        }

        /// <summary>
        /// Validates the Social Security Number is unformatted
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void SocialSecurityNumberUnformatted()
        {
            string regexFormat = @"\d{9}";
            string a = FakerData.GenerateSocialSecurityNumber();
            Assert.IsTrue(Regex.IsMatch(FakerData.GenerateSocialSecurityNumber(), regexFormat));
        }

        /// <summary>
        /// Validates a string is returned that is within the string list
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GeneralRandomizerString()
        {
            List<string> stringList = new List<string>() { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            string randomString = FakerData.GeneralRandomizer(stringList);
            Assert.IsTrue(stringList.Contains(randomString));
        }

        /// <summary>
        /// Validates an integer is returned that is within the integer list
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void GeneralRandomizerInt()
        {
            List<int> intList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
            int randomInt = FakerData.GeneralRandomizer(intList);
            Assert.IsTrue(intList.Contains(randomInt));
        }
    }
}
