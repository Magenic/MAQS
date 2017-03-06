//-----------------------------------------------------
// <copyright file="FileLoggerUnitTests.cs" company="Magenic">
//  Copyright 2015 Magenic, All rights Reserved
// </copyright>
// <summary>Test the file logger</summary>
//-----------------------------------------------------

using System;
using Magenic.MaqsFramework.BaseSeleniumTest;
using Magenic.MaqsFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace UnitTests
{
    /// <summary>
    /// File logger unit tests
    /// </summary>
    [TestClass]
    public class FileLoggerUnitTests
    {
        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        public void TestFileLogger()
        {
            FileLogger logger = new FileLogger(false);
            logger.LogMessage(MessageType.WARNING, "Hello");
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        public void WriteToFileLogger()
        {
            FileLogger logger = new FileLogger(false);
            logger.LogMessage(MessageType.WARNING, "Hello, this is a test.");
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        public void WriteToExistingFileLogger()
        {
            FileLogger logger = new FileLogger(true);
            logger.LogMessage(MessageType.WARNING, "This is a test to write to an existing file.");
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        public void TestRTFFileLogger()
        {
            RtfFileLogger logger = new RtfFileLogger(false);
            logger.LogMessage(MessageType.WARNING, "Hello");
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        public void WriteToExistingRTFFileLogger()
        {
            RtfFileLogger logger = new RtfFileLogger(true);
            logger.LogMessage(MessageType.WARNING, "This is a test to write to an existing file.");
        }
    }
}