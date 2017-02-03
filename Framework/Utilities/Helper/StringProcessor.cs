//--------------------------------------------------
// <copyright file="StringProcessor.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Help utilities for string processing</summary>
//--------------------------------------------------
using System;
using System.Text;

namespace Magenic.MaqsFramework.Utilities.Data
{
    /// <summary>
    /// Initializes a new instance of the StringProcessor class
    /// </summary>
    public static class StringProcessor
    {
        /// <summary>
        /// Creates a string based on the arguments
        /// If no args are applied, then we want to just return the message
        /// </summary>
        /// <param name="message">The message being used</param>
        /// <param name="args">The arguments being used</param>
        /// <returns>A final string</returns>
        /// <example>
        /// <code source = "../UtilitiesUnitTests/StringProcessorUnitTests.cs" region="StringFormattor" lang="C#" />
        /// </example>
        public static string SafeFormatter(string message, params object[] args)
        {
            try
            {
                return string.Format(message, args);
            }
            catch (Exception)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(message);
                foreach (var arg in args)
                {
                    builder.AppendLine(arg.ToString());
                }

                return builder.ToString();
            }
        }
    }
}
