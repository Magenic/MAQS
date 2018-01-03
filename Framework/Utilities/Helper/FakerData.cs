//--------------------------------------------------
// <copyright file="FakerData.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Class for creating fake data for testing</summary>
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Magenic.MaqsFramework.Utilities.Data
{
    /// <summary>
    /// Generates Unique Faker Data for testing
    /// </summary>
    public static class FakerData
    {
        /// <summary>
        /// Random variable to use throughout
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Returns the current time in the MM/DD/YYYY HH:MM:SSSS format
        /// </summary>
        /// <returns>current time string</returns>
        public static string GenerateInstantSpecificTime()
        {
            return DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }

        /// <summary>
        /// Returns a unique ID as a string
        /// </summary>
        /// <param name="formatted">If formatted or not</param>
        /// <returns>ID string</returns>
        public static string GenerateUniqueId(bool formatted = true)
        {
            if (formatted)
            {
                return Guid.NewGuid().ToString();
            }

            return Guid.NewGuid().ToString().Replace("-", string.Empty);
        }

        /// <summary>
        /// Returns a random phoneNumber - formatted or unformatted 
        /// </summary>
        /// <param name="formatted">Unformatted by default</param>
        /// <returns>Phone number as string</returns>
        public static string GenerateUSPhoneNumber(bool formatted = false)
        {
            string phoneNumber = "(" + random.Next(201, 999).ToString() + ")" +
                random.Next(100, 999).ToString() + "-" +
                random.Next(1000, 9999).ToString();

            if (formatted)
            {
                return phoneNumber;
            }

            return Regex.Replace(phoneNumber, "[^0-9]", string.Empty);
        }

        /// <summary>
        /// Returns a random prefix for a name
        /// </summary>
        /// <param name="withDashes">without dashes by default</param>
        /// <returns>Social Security string</returns>
        public static string GenerateSocialSecurityNumber(bool withDashes = false)
        {
            string social = random.Next(100, 999).ToString() + "-"
                    + random.Next(10, 99).ToString() + "-"
                    + random.Next(1000, 9999).ToString();

            if (withDashes)
            {
                return social;
            }

            return Regex.Replace(social, "[^0-9]", string.Empty);
        }

        /// <summary>
        /// Takes in a list and returns a random Value
        /// </summary>
        /// <typeparam name="T">Any type of List</typeparam>
        /// <param name="stringList">Generic string list of any type</param>
        /// <returns>Random value from the list</returns>
        public static T GeneralRandomizer<T>(List<T> stringList)
        {
            return (T)stringList[random.Next(stringList.Count - 1)];
        }
    }
}