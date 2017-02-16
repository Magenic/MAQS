//-----------------------------------------------------
// <copyright file="FileLoggerUnitTests.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Test the file logger</summary>
//-----------------------------------------------------
using Magenic.MaqsFramework.Utilities.BaseTest;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// File logger unit tests
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class FileLoggerUnitTests
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Gets or sets the test context
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Setup before we start running selenium tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after we are done running selenium tests
        /// </summary>
        [ClassCleanup]
        public static void CleanupAfterClass()
        {
            TestHelper.Cleanup(loggingFolderExistsBeforeRun);
        }

        /// <summary>
        /// Verify the console logger respects hierarchical logging
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"|DataDirectory|\TestData\HierarchicalLoggingData.csv",
            "HierarchicalLoggingData#csv",
            DataAccessMethod.Sequential)]
        public void TestHierarchicalConsoleLogger()
        {
            // Calculate a file path
            string path = Path.Combine(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalConsoleLogger", "txt"));

            // Pipe the console to this file
            using (var cc = new ConsoleCopy(path))
            {
                ConsoleLogger console = new ConsoleLogger();
                this.TestHierarchicalLogging(console, path);
            }
        }

        /// <summary>
        /// Verify the txt file logger respects hierarchical logging
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"|DataDirectory|\TestData\HierarchicalLoggingData.csv",
            "HierarchicalLoggingData#csv",
            DataAccessMethod.Sequential)]
        public void TestHierarchicalTxtFileLogger()
        {
            FileLogger logger = new FileLogger(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalTxtFileLogger", "txt"), MessageType.GENERIC, true);
            this.TestHierarchicalLogging(logger, logger.FilePath);
        }

        /// <summary>
        /// Verify the html file logger respects hierarchical logging
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV",
            @"|DataDirectory|\TestData\HierarchicalLoggingData.csv",
            "HierarchicalLoggingData#csv",
            DataAccessMethod.Sequential)]
        public void TestHierarchicalHtmlFileLogger()
        {
            HtmlFileLogger logger = new HtmlFileLogger(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalHtmlFileLogger", "html"), MessageType.GENERIC, true);
            this.TestHierarchicalLogging(logger, logger.FilePath);
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void TestFileLogger()
        {
            FileLogger logger = new FileLogger(string.Empty, "TestFileLogger");
            logger.LogMessage(MessageType.WARNING, "Hello");
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify the logging suspension functions
        /// </summary>
        [TestMethod]
        [TestCategory("Utilities Unit Tests")]
        public void TestSuspendLogger()
        {
            SoftAssert softAssert = new SoftAssert();
            string count = null;

            // Start logging
            FileLogger logger = new FileLogger(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalTxtFileLogger", "txt"), MessageType.GENERIC, true);
            logger.SetLoggingLevel(MessageType.VERBOSE);

            logger.LogMessage(MessageType.VERBOSE, "HellO");

            // Suspend logging
            logger.SuspendLogging();
            logger.LogMessage(MessageType.ERROR, "GoodByE");

            // Continue logging
            logger.ContinueLogging();
            logger.LogMessage(MessageType.VERBOSE, "BacK");

            // Get the log file content
            string logContents = this.ReadTextFile(logger.FilePath);

            // Verify that logging was active
            count = Regex.Matches(logContents, "HellO").Count.ToString();
            softAssert.AreEqual("1", count, "'HellO' was not found.  Logging Failed");

            // Verify that logging was suspended
            count = Regex.Matches(logContents, "GoodByE").Count.ToString();
            softAssert.AreEqual("0", count, "'GoodByE' was found.  Logging Suspension Failed");

            // Verify that logging was active
            count = Regex.Matches(logContents, "BacK").Count.ToString();
            softAssert.AreEqual("1", count, "'BacK' was not found.  Logging Continue Failed");

            // Fail the test if any soft asserts failed
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void WriteToFileLogger()
        {
            FileLogger logger = new FileLogger(string.Empty, "WriteToFileLogger");
            logger.LogMessage(MessageType.WARNING, "Hello, this is a test.");
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [TestMethod] attribute.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void WriteToExistingFileLogger()
        {
            FileLogger logger = new FileLogger(string.Empty, "WriteToExistingFileLogger", MessageType.GENERIC, true);
            logger.LogMessage(MessageType.WARNING, "This is a test to write to an existing file.");
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify FileLogger constructor creates the correct directory if it does not already exist
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FileLoggerConstructorCreateDirectory()
        {
            FileLogger logger = new FileLogger("FileLoggerCreateDirectory", "FileLoggerCreateDirectory", MessageType.GENERIC, true);
            logger.LogMessage(MessageType.WARNING, "Test to ensure that the file in the created directory can be written to.");
            Assert.IsTrue(File.ReadAllText(logger.FilePath).Contains("Test to ensure that the file in the created directory can be written to."));
            File.Delete(logger.FilePath);
            Directory.Delete(Path.GetDirectoryName(logger.FilePath));
        }

        /// <summary>
        /// Verify that FileLogger can log message without defining a MessageType
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FileLoggerLogMessage()
        {
            FileLogger logger = new FileLogger(string.Empty, "FileLoggerLogMessage", MessageType.GENERIC, true);
            logger.LogMessage("Test to ensure LogMessage works as expected.");
            Assert.IsTrue(File.ReadAllText(logger.FilePath).Contains("Test to ensure LogMessage works as expected."));
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify that FilePath field can be accessed and updated
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FileLoggerSetFilePath()
        {
            FileLogger logger = new FileLogger(string.Empty, "FileLoggerSetFilePath", MessageType.GENERIC, true);
            logger.FilePath = "test file path";
            Assert.AreEqual(logger.FilePath, "test file path");
        }

        /// <summary>
        /// Verify that FileLogger catches and handles errors caused by incorrect filePaths
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FileLoggerCatchThrownException()
        {
            FileLogger logger = new FileLogger(string.Empty, "FileLoggerCatchThrownException", MessageType.GENERIC, true);
            logger.FilePath = "<>";
            logger.LogMessage(MessageType.GENERIC, "test throws error");
        }

        /// <summary>
        /// Verify hierarchical logging is respected
        /// </summary>
        /// <param name="logger">The logger we are checking</param>
        /// <param name="filePath">Where the log output can be found</param>
        private void TestHierarchicalLogging(Logger logger, string filePath)
        {
            // Create a soft assert
            SoftAssert softAssert = new SoftAssert(logger);

            // Get the next row of data
            Dictionary<string, string> rowDictionary = this.GetCurrentRow();

            // Get the log level
            MessageType logLevel = (MessageType)Enum.Parse(typeof(MessageType), rowDictionary["LogLevel"]);
            logger.SetLoggingLevel(logLevel);

            // Set the logger options to set the log level and add log entries to the file
            logger.LogMessage(logLevel, "\nThe Loglevel is set to " + logLevel);

            // Message template
            string logLine = "Test Log item {0}";

            // Log the test messages
            logger.LogMessage(MessageType.VERBOSE, logLine, MessageType.VERBOSE);
            logger.LogMessage(MessageType.INFORMATION, logLine, MessageType.INFORMATION);
            logger.LogMessage(MessageType.GENERIC, logLine, MessageType.GENERIC);
            logger.LogMessage(MessageType.SUCCESS, logLine, MessageType.SUCCESS);
            logger.LogMessage(MessageType.WARNING, logLine, MessageType.WARNING);
            logger.LogMessage(MessageType.ERROR, logLine, MessageType.ERROR);

            // Get the file content
            string logContents = this.ReadTextFile(filePath);

            // Verify that only the logged messages at the log level or below are logged
            foreach (KeyValuePair<string, string> keyValue in rowDictionary)
            {
                if ((keyValue.Key != "Row") && (keyValue.Key != "LogLevel"))
                {
                    // Verify the number of times that the message type is found.
                    string count = Regex.Matches(logContents, string.Format(logLine, keyValue.Key)).Count.ToString();
                    softAssert.AreEqual(keyValue.Value, count, "Looking for " + keyValue.Key);
                }
            }

            // Set the log level so that the soft asserts log
            logger.SetLoggingLevel(MessageType.VERBOSE);

            // Fail test if any soft asserts failed
            softAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Read a file and return it as a string
        /// </summary>
        /// <param name="fileName">The file to read</param>
        /// <returns>The contents of the file</returns>
        private string ReadTextFile(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader textReader = new StreamReader(fileStream))
                {
                    return textReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Get the current row of the test data.
        /// </summary>
        /// <returns> Dictionary of column headers as keys the associated cell values as value</returns>
        private Dictionary<string, string> GetCurrentRow()
        {
            Dictionary<string, string> rowDictionary = new Dictionary<string, string>();

            // Get the column headings and data of the rows
            var columnObject = TestContext.DataRow.Table.Columns;

            // Fix the first column heading of the rows
            columnObject[0].ColumnName = "Row"; // Columns returns a bad key ="<garbage>Row", this fixes it.

            // Convert the column headings and rows into a dictionary
            foreach (var columnName in columnObject)
            {
                rowDictionary.Add(columnName.ToString(), TestContext.DataRow[columnName.ToString()].ToString());
            }

            // return the dictionary with the column headers and row data
            return rowDictionary;
        }

        /// <summary>
        /// Get a unique file name
        /// </summary>
        /// <param name="testName">Prepend text</param>
        /// <param name="extension">The file extension</param>
        /// <returns>A unique file name</returns>
        private string GetFileName(string testName, string extension)
        {
            return StringProcessor.SafeFormatter("UtilitiesUnitTesting.{0}-{1}.{2}", testName, Guid.NewGuid(), extension);
        }
    }
}