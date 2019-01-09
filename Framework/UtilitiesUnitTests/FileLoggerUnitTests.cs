//-----------------------------------------------------
// <copyright file="FileLoggerUnitTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Test the file logger</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// File logger unit tests
    /// </summary>
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    [ExcludeFromCodeCoverage]
    public class FileLoggerUnitTests
    {
        /// <summary>
        /// Gets logging level test data
        /// </summary>
        public static IEnumerable LoggingLevels
        {
            get
            {
                yield return new TestCaseData("VERBOSE", new Dictionary<string, int> { { "VERBOSE", 1 }, { "INFORMATION", 1 }, { "GENERIC", 1 }, { "SUCCESS", 1 }, { "WARNING", 1 }, { "ERROR", 1 } });
                yield return new TestCaseData("INFORMATION", new Dictionary<string, int> { { "VERBOSE", 0 }, { "INFORMATION", 1 }, { "GENERIC", 1 }, { "SUCCESS", 1 }, { "WARNING", 1 }, { "ERROR", 1 } });
                yield return new TestCaseData("GENERIC", new Dictionary<string, int> { { "VERBOSE", 0 }, { "INFORMATION", 0 }, { "GENERIC", 1 }, { "SUCCESS", 1 }, { "WARNING", 1 }, { "ERROR", 1 } });
                yield return new TestCaseData("SUCCESS", new Dictionary<string, int> { { "VERBOSE", 0 }, { "INFORMATION", 0 }, { "GENERIC", 0 }, { "SUCCESS", 1 }, { "WARNING", 1 }, { "ERROR", 1 } });
                yield return new TestCaseData("WARNING", new Dictionary<string, int> { { "VERBOSE", 0 }, { "INFORMATION", 0 }, { "GENERIC", 0 }, { "SUCCESS", 0 }, { "WARNING", 1 }, { "ERROR", 1 } });
                yield return new TestCaseData("ERROR", new Dictionary<string, int> { { "VERBOSE", 0 }, { "INFORMATION", 0 }, { "GENERIC", 0 }, { "SUCCESS", 0 }, { "WARNING", 0 }, { "ERROR", 1 } });
                yield return new TestCaseData("SUSPENDED", new Dictionary<string, int> { { "VERBOSE", 0 }, { "INFORMATION", 0 }, { "GENERIC", 0 }, { "SUCCESS", 0 }, { "WARNING", 0 }, { "ERROR", 0 } });
            }
        }

        /// <summary>
        /// Verify the console logger respects hierarchical logging
        /// </summary>
        /// <param name="logLevel">The type of logging</param>
        /// <param name="levels">What should appear for each level</param>
        [Test]
        [TestCaseSource("LoggingLevels")]
        [Category(TestCategories.Utilities)]
        public void TestHierarchicalConsoleLogger(string logLevel, Dictionary<string, int> levels)
        {
            // Calculate a file path
            string path = Path.Combine(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalConsoleLogger" + logLevel, "txt"));

            // Pipe the console to this file
            using (ConsoleCopy consoleCopy = new ConsoleCopy(path))
            {
                ConsoleLogger console = new ConsoleLogger();
                this.TestHierarchicalLogging(console, path, logLevel, levels);
            }

            File.Delete(path);
        }

        /// <summary>
        /// Verify the txt file logger respects hierarchical logging
        /// </summary>
        /// <param name="logLevel">The type of logging</param>
        /// <param name="levels">What should appear for each level</param>
        [Test]
        [TestCaseSource("LoggingLevels")]
        [Category(TestCategories.Utilities)]
        public void TestHierarchicalTxtFileLogger(string logLevel, Dictionary<string, int> levels)
        {
            FileLogger logger = new FileLogger(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalTxtFileLogger" + logLevel, "txt"), MessageType.GENERIC, true);
            this.TestHierarchicalLogging(logger, logger.FilePath, logLevel, levels);

            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify the html file logger respects hierarchical logging
        /// </summary>
        /// <param name="logLevel">The type of logging</param>
        /// <param name="levels">What should appear for each level</param>
        [Test]
        [TestCaseSource("LoggingLevels")]
        [Category(TestCategories.Utilities)]
        public void TestHierarchicalHtmlFileLogger(string logLevel, Dictionary<string, int> levels)
        {
            HtmlFileLogger logger = new HtmlFileLogger(LoggingConfig.GetLogDirectory(), this.GetFileName("TestHierarchicalHtmlFileLogger" + logLevel, "html"), MessageType.GENERIC, true);
            this.TestHierarchicalLogging(logger, logger.FilePath, logLevel, levels);

            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [Test] attribute.
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void TestFileLogger()
        {
            FileLogger logger = new FileLogger(string.Empty, "TestFileLogger");
            logger.LogMessage(MessageType.WARNING, "Hello");
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify the logging suspension functions
        /// </summary>
        [Test]
        [Category("Utilities Unit Tests")]
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
        /// must have the [Test] attribute.
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void WriteToFileLogger()
        {
            FileLogger logger = new FileLogger(string.Empty, "WriteToFileLogger");
            logger.LogMessage(MessageType.WARNING, "Hello, this is a test.");
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Base Test Method.
        /// Each test method that you want to run
        /// must have the [Test] attribute.
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void WriteToExistingFileLogger()
        {
            FileLogger logger = new FileLogger(string.Empty, "WriteToExistingFileLogger", MessageType.GENERIC, true);
            logger.LogMessage(MessageType.WARNING, "This is a test to write to an existing file.");
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify FileLogger constructor creates the correct directory if it does not already exist
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void FileLoggerConstructorCreateDirectory()
        {
            FileLogger logger = new FileLogger(Path.Combine(LoggingConfig.GetLogDirectory(), "FileLoggerCreateDirectory"), "FileLoggerCreateDirectory", MessageType.GENERIC, true);
            logger.LogMessage(MessageType.WARNING, "Test to ensure that the file in the created directory can be written to.");
            Assert.IsTrue(File.ReadAllText(logger.FilePath).Contains("Test to ensure that the file in the created directory can be written to."));
            File.Delete(logger.FilePath);
            Directory.Delete(Path.GetDirectoryName(logger.FilePath));
        }

        /// <summary>
        /// Verify that FileLogger can log message without defining a MessageType
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void FileLoggerLogMessage()
        {
            FileLogger logger = new FileLogger(string.Empty, "FileLoggerLogMessage", MessageType.INFORMATION, true);
            logger.LogMessage("Test to ensure LogMessage works as expected.");
            Assert.IsTrue(File.ReadAllText(logger.FilePath).Contains("Test to ensure LogMessage works as expected."));
            File.Delete(logger.FilePath);
        }

        /// <summary>
        /// Verify that FilePath field can be accessed and updated
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void FileLoggerSetFilePath()
        {
            FileLogger logger = new FileLogger(string.Empty, "FileLoggerSetFilePath", MessageType.GENERIC, true)
            {
                FilePath = "test file path"
            };

            Assert.AreEqual("test file path", logger.FilePath);
        }

        /// <summary>
        /// Verify that FileLogger catches and handles errors caused by incorrect filePaths
        /// </summary>
        [Test]
        [Category(TestCategories.Utilities)]
        public void FileLoggerCatchThrownException()
        {
            FileLogger logger = new FileLogger(string.Empty, "FileLoggerCatchThrownException", MessageType.GENERIC, true)
            {
                FilePath = "<>"
            };

            logger.LogMessage(MessageType.GENERIC, "test throws error");
        }

        /// <summary>
        /// Verify hierarchical logging is respected
        /// </summary>
        /// <param name="logger">The logger we are checking</param>
        /// <param name="filePath">Where the log output can be found</param>
        /// <param name="logLevelText">The type of logging</param>
        /// <param name="levels">What should appear for each level</param>
        private void TestHierarchicalLogging(Logger logger, string filePath, string logLevelText, Dictionary<string, int> levels)
        {
            // Create a soft assert
            SoftAssert softAssert = new SoftAssert(logger);

            // Get the log level
            MessageType logLevel = (MessageType)Enum.Parse(typeof(MessageType), logLevelText);
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

            // Give the write time
            Thread.Sleep(250);

            // Get the file content
            string logContents = this.ReadTextFile(filePath);

            // Verify that only the logged messages at the log level or below are logged
            foreach (KeyValuePair<string, int> keyValue in levels)
            {
                if ((keyValue.Key != "Row") && (keyValue.Key != "LogLevel"))
                {
                    // Verify the number of times that the message type is found.
                    int count = Regex.Matches(logContents, string.Format(logLine, keyValue.Key)).Count;
                    softAssert.AreEqual(keyValue.Value.ToString(), count.ToString(), "Looking for " + keyValue.Key);
                }
            }

            // Set the log level so that the soft asserts log
            logger.SetLoggingLevel(MessageType.VERBOSE);

            // Fail test if any soft asserts failed
            softAssert.FailTestIfAssertFailed(logContents);
        }

        /// <summary>
        /// Read a file and return it as a string
        /// </summary>
        /// <param name="fileName">The file to read</param>
        /// <returns>The contents of the file</returns>
        private string ReadTextFile(string fileName)
        {
            Stream fileStream = null;
            string returnValue = string.Empty;

            try
            {
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                using (StreamReader textReader = new StreamReader(fileStream))
                {
                    fileStream = null;
                    returnValue = textReader.ReadToEnd();
                }
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }

            return returnValue;
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