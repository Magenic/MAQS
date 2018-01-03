//--------------------------------------------------
// <copyright file="ListProcessor.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting selenium specific configuration values</summary>
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace Magenic.MaqsFramework.Utilities.Data
{
    /// <summary>
    /// Contains methods for processing lists
    /// </summary>
    public static class ListProcessor
    {
        /// <summary>
        /// Create a comma delimited string from a list of strings
        /// </summary>
        /// <param name="stringList">List of strings</param>
        /// <param name="sort">True to create an alphabetically sorted comma delimited string</param>
        /// <returns>Returns a comma delimited string</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ListProcessorUnitTests.cs" region="CommaDelimited" lang="C#" />
        /// </example>
        public static string CreateCommaDelimitedString(List<string> stringList, bool sort = false)
        {
            bool firstElement = true;
            string commaDelimitedString = string.Empty;

            if (sort)
            {
                stringList.Sort();
            }

            foreach (string text in stringList)
            {
                if (firstElement)
                {
                    commaDelimitedString = text;
                    firstElement = false;
                }
                else
                {
                    commaDelimitedString += StringProcessor.SafeFormatter(", {0}", text);
                }
            }

            return commaDelimitedString;
        }

        /// <summary>
        /// Compares two lists to see if they contain the same values
        /// </summary>
        /// <param name="expectedList">First list of strings to compare</param>
        /// <param name="actualList">Second list of strings to compare</param>
        /// <param name="results">StringBuilder to hold failed results</param>
        /// <param name="verifyOrder">If True, verify the two lists have values in the same order</param>
        /// <returns>True if the lists are the same</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/ListProcessorUnitTests.cs" region="ListProcessorCompare" lang="C#" />
        /// </example>
        public static bool ListOfStringsComparer(List<string> expectedList, List<string> actualList, StringBuilder results, bool verifyOrder = false)
        {
            if (expectedList.Count != actualList.Count)
            {
                results.Append(
                    StringProcessor.SafeFormatter(
                    "The following lists are not the same size: Expected {0} [{1}] {2} and got {3} [{4}]",
                    Environment.NewLine,
                    CreateCommaDelimitedString(expectedList),
                    Environment.NewLine,
                    Environment.NewLine,
                    CreateCommaDelimitedString(actualList)));
            }

            // Clone the first list 
            List<string> clonedList = new List<string>(expectedList.Count);
            foreach (string text in expectedList)
            {
                clonedList.Add(text);
            }

            for (int i = 0; i < actualList.Count; i++)
            {
                string expectedValue = actualList[i];
                if (!verifyOrder)
                {
                    if (!clonedList.Contains(expectedValue))
                    {
                        results.Append(StringProcessor.SafeFormatter("[{0}] was not found in the list but was expected{1}", expectedValue, Environment.NewLine));
                    }
                    else
                    {
                        // Remove these values from the list to make sure duplicates are handled correctly
                        clonedList.RemoveAt(clonedList.IndexOf(expectedValue));
                    }
                }
                else if (clonedList[i] == null || !clonedList[i].Equals(expectedValue))
                {
                    results.Append(StringProcessor.SafeFormatter("Expected [{0}] but found [{1}]{2}", expectedValue, clonedList[i], Environment.NewLine));
                }
            }

            return results.Length == 0;
        }
    }
}
