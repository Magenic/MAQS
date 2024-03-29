﻿//--------------------------------------------------
// <copyright file="BaseTest.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for tests without a system under test object like web drivers or database connections</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using NUnitTestContext = NUnit.Framework.TestContext;
using VSTestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Base for tests without a defined system under test
    /// </summary>
    [TestClass]
#pragma warning disable S2187 // TestCases should contain tests
    public class BaseTest

#pragma warning restore S2187 // TestCases should contain tests
    {
        /// <summary>
        /// The Visual Studio TestContext
        /// </summary>
        private VSTestContext testContextInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTest" /> class
        /// </summary>
        public BaseTest()
        {
            this.LoggedExceptions = new ConcurrentDictionary<string, List<string>>();
            this.BaseTestObjects = new ConcurrentDictionary<string, ITestObject>();

            // Update your config parameters
            if (NUnitTestContext.Parameters != null)
            {
                try
                {
                    Config.UpdateWithNUnitTestParameters(NUnitTestContext.Parameters);
                }
                catch (Exception e)
                {
                    // Test logger is not created yet so write to the console
                    Console.WriteLine("Failed to override NUnit configuration settings because: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Gets or sets the performance timer collection for a test
        /// </summary>
        public IPerfTimerCollection PerfTimerCollection
        {
            get
            {
                return this.TestObject.PerfTimerCollection;
            }

            set
            {
                this.TestObject.PerfTimerCollection = value;
            }
        }

        /// <summary>
        /// Gets or sets the SoftAssert objects
        /// </summary>
        public ISoftAssert SoftAssert
        {
            get
            {
                return this.TestObject.SoftAssert;
            }

            set
            {
                this.TestObject.SoftAssert = value;
            }
        }

        /// <summary>
        /// Gets or sets the testing object
        /// </summary>
        public ILogger Log
        {
            get
            {
                return this.TestObject.Log;
            }

            set
            {
                this.TestObject.Log = value;
            }
        }

        /// <summary>
        /// Gets the driver store
        /// </summary>
        public IManagerStore ManagerStore
        {
            get
            {
                return this.TestObject.ManagerStore;
            }
        }

        /// <summary>
        /// Gets or sets the test object
        /// </summary>
        public ITestObject TestObject
        {
            get
            {
                if (!this.BaseTestObjects.ContainsKey(this.GetFullyQualifiedTestClassName()))
                {
                    this.SetupTestObject();
                }

                return this.BaseTestObjects[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                this.BaseTestObjects.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets or sets the testing object
        /// </summary>
        public List<string> LoggedExceptionList
        {
            get
            {
                // If no logged exception are found return an empty list
                if (!this.LoggedExceptions.ContainsKey(this.GetFullyQualifiedTestClassName()))
                {
                    return new List<string>();
                }

                return this.LoggedExceptions[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                this.LoggedExceptions.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Get the method info from the current TestContext. This property lazy loads the <see cref="MethodInfo"/>
        /// from the assembly. Because the assembly of the test method is unknown, all assemblies in the <see cref="AppDomain"/>
        /// are searched to find the first one that can load the fully qualified test class name of the
        /// <see cref="TestContext"/>.
        /// </summary>
        protected MethodInfo TestMethodInfo
        {
            get
            {
                if (this.IsVSTest())
                {
                    return GetMethodInfoIfUsingSoftAssertExpectedAsserts(
                            this.TestContext.FullyQualifiedTestClassName,
                            this.TestContext.TestName);
                }
                else
                {
                    return GetMethodInfoIfUsingSoftAssertExpectedAsserts(
                            NUnitTestContext.CurrentContext.Test.ClassName,
                            NUnitTestContext.CurrentContext.Test.MethodName);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Visual Studio TextContext
        /// </summary>
        public VSTestContext TestContext
        {
            get
            {
                return this.testContextInstance;
            }

            set
            {
                this.testContextInstance = value;

                try
                {
                    // The test context has been set so update your config parameters
                    Config.UpdateWithVSTestContext(this.testContextInstance);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to override VSTest configuration settings because: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Gets or sets the BaseContext objects
        /// </summary>
        internal ConcurrentDictionary<string, ITestObject> BaseTestObjects { get; set; }

        /// <summary>
        /// Gets the logging enable flag
        /// </summary>
        protected LoggingEnabled LoggingEnabledSetting { get; private set; }

        /// <summary>
        /// Gets or sets the logged exceptions
        /// </summary>
        private ConcurrentDictionary<string, List<string>> LoggedExceptions { get; set; }

        /// <summary>
        /// Setup before a test
        /// </summary>
        [TestInitialize]
        [SetUp]
        public void Setup()
        {
            // Only create a test object if one doesn't exist
            if (!this.BaseTestObjects.ContainsKey(this.GetFullyQualifiedTestClassName()))
            {
                // Create the test object
                this.SetupTestObject();

                if (this.TestMethodInfo != null)
                {
                    this.TestObject.SoftAssert.CaptureTestMethodAttributes(this.TestMethodInfo);
                }
            }
        }

        /// <summary>
        /// Tear down after a test
        /// </summary>
        [TestCleanup]
        [TearDown]
        public void Teardown()
        {
            // Get the Fully Qualified Test Name
            string fullyQualifiedTestName = this.GetFullyQualifiedTestClassName();

            try
            {
                TestResultType resultType = this.GetResultType();
                bool forceTestFailure = false;

                // Switch the test to a failure if we have a soft assert failure
                this.SoftAssert.CheckForExpectedAsserts();
                if (!this.SoftAssert.DidUserCheck() && this.SoftAssert.DidSoftAssertsFail())
                {
                    resultType = TestResultType.FAIL;
                    forceTestFailure = true;
                    this.SoftAssert.LogFinalAssertData();
                }

                // Log the test result
                if (resultType == TestResultType.PASS)
                {
                    this.TryToLog(MessageType.SUCCESS, "Test passed");
                    this.WriteAssociatedFilesNamesToLog();
                }
                else if (resultType == TestResultType.FAIL)
                {
                    this.TryToLog(MessageType.ERROR, "Test failed");
                }
                else if (resultType == TestResultType.INCONCLUSIVE)
                {
                    this.TryToLog(MessageType.ERROR, "Test was inconclusive");
                }
                else
                {
                    this.TryToLog(MessageType.WARNING, "Test had an unexpected result of {0}", this.GetResultText());
                }

                this.GetResultTextNunit();
                this.LogVerbose("Test outcome");
                this.BeforeCleanup(resultType);

                // Cleanup log files we don't want
                try
                {
                    if (this.Log is IFileLogger logger && resultType == TestResultType.PASS
                        && this.LoggingEnabledSetting == LoggingEnabled.ONFAIL)
                    {
                        File.Delete(logger.FilePath);
                    }
                }
                catch (Exception e)
                {
                    this.TryToLog(MessageType.WARNING, "Failed to cleanup log files because: {0}", e.Message);
                }

                IPerfTimerCollection collection = this.TestObject.PerfTimerCollection;
                this.PerfTimerCollection = collection;

                // Write out the performance timers
                collection.Write(this.Log);
                if (collection.FileName != null)
                {
                    this.TestObject.AddAssociatedFile(LoggingConfig.GetLogDirectory() + "\\" + collection.FileName);
                }

                // Attach associated files if we can
                this.AttachAssociatedFiles();

                // Release the logged messages
                this.LoggedExceptions.TryRemove(fullyQualifiedTestName, out List<string> loggedMessages);

                // Force the test to fail
                if (forceTestFailure)
                {
                    throw new AssertFailedException("Test was forced to fail in the cleanup - Likely the result of a soft assert failure.");
                }
            }
            finally
            {
                // Release log
                this.Log?.Dispose();

                // Release the base test object
                this.BaseTestObjects.TryRemove(fullyQualifiedTestName, out ITestObject baseTestObject);
                baseTestObject.Dispose();
            }
        }

        /// <summary>
        /// Create a the test object
        /// </summary>
        protected void SetupTestObject()
        {
            ILogger newLogger = this.CreateAndSetupLogger(GetFileNameWithoutExtension(), LoggingConfig.GetLogType(), LoggingConfig.GetLoggingEnabledSetting(), LoggingConfig.GetFirstChanceHandler());
            this.TestObject = CreateTestObject(newLogger);
        }

        /// <summary>
        /// Create a logger
        /// </summary>
        /// <returns>A logger</returns>
        protected ILogger CreateAndSetupLogger(string logName, string logType, LoggingEnabled enabled, bool firstChanceHandler)
        {
            try
            {
                this.LoggedExceptionList = new List<string>();
                this.LoggingEnabledSetting = enabled;

                // Setup the exception listener
                if (firstChanceHandler)
                {
                    AppDomain.CurrentDomain.FirstChanceException += this.FirstChanceHandler;
                }

                return CreateLogger(logName, logType, LoggingConfig.GetLoggingLevelSetting(), enabled);
            }
            catch (Exception e)
            {
                ILogger newLogger = LoggerFactory.GetConsoleLogger();
                newLogger.LogMessage(MessageType.WARNING, StringProcessor.SafeExceptionFormatter(e));
                return newLogger;
            }
        }

        /// <summary>
        /// Create a logger
        /// </summary>
        /// <returns>A logger</returns>
        protected virtual ILogger CreateLogger(string logName, string logType, MessageType loggingLevel, LoggingEnabled enabled)
        {
            try
            {
                if (enabled != LoggingEnabled.NO)
                {
                    return LoggerFactory.GetLogger(logName, logType, loggingLevel);
                }
                else
                {
                    return LoggerFactory.GetConsoleLogger();
                }
            }
            catch (Exception e)
            {
                ILogger newLogger = LoggerFactory.GetConsoleLogger();
                newLogger.LogMessage(MessageType.WARNING, StringProcessor.SafeExceptionFormatter(e));
                return newLogger;
            }
        }

        /// <summary>
        /// Create test specific test objects
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        protected virtual ITestObject CreateTestObject(ILogger log)
        {
            return new BaseTestObject(log, this.GetFullyQualifiedTestClassName());
        }

        /// <summary>
        /// Get the default file name for the current test
        /// </summary>
        /// <returns>Filename with out the extension</returns>
        protected virtual string GetFileNameWithoutExtension()
        {
            if (LoggingConfig.GetUseShortFileName())
            {
                return $"{this.GetTestName()}{DateTime.UtcNow.ToString("_ffff", CultureInfo.InvariantCulture)}";
            }

            return $"{this.GetFullyQualifiedTestClassName()} - {DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Get the fully qualified test name
        /// </summary>
        /// <returns>The test name including class</returns>
        protected string GetFullyQualifiedTestClassName()
        {
            if (this.IsVSTest())
            {
                return $"{this.TestContext.FullyQualifiedTestClassName}.{this.TestContext.TestName}";
            }

            return NUnitTestContext.CurrentContext.Test.FullName;
        }

        /// <summary>
        /// Get the test name
        /// </summary>
        /// <returns>The test name</returns>
        protected string GetTestName()
        {
            if (this.IsVSTest())
            {
                return this.TestContext.TestName;
            }

            return NUnitTestContext.CurrentContext.Test.Name;
        }

        /// <summary>
        /// Get the type of test result
        /// </summary>
        /// <returns>The test result type</returns>
        protected TestResultType GetResultType()
        {
            if (this.IsVSTest())
            {
                return this.GetResultTypeVS();
            }

            return this.GetResultTypeNunit();
        }

        /// <summary>
        /// Get the test result type as text
        /// </summary>
        /// <returns>The result type as text</returns>
        protected string GetResultText()
        {
            if (this.IsVSTest())
            {
                return this.GetResultTextVS();
            }

            return this.GetResultTextNunit();
        }

        /// <summary>
        /// Try to log a message - Do not fail if the message is not logged
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        protected void TryToLog(MessageType messageType, string message, params object[] args)
        {
            // Get the formatted message
            string formattedMessage = StringProcessor.SafeFormatter(message, args);

            try
            {
                // Write to the log
                this.Log.LogMessage(messageType, formattedMessage);

                // If this was an error and written to a file, add it to the console output as well
                if (messageType == MessageType.ERROR && !(this.Log is ConsoleLogger))
                {
                    Console.WriteLine(formattedMessage);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(formattedMessage);
                Console.WriteLine("Logging failed because: " + e);
            }
        }

        /// <summary>
        /// Log a verbose message and include the automation specific call stack data
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        protected void LogVerbose(string message, params object[] args)
        {
            StringBuilder messages = new StringBuilder();
            messages.AppendLine(StringProcessor.SafeFormatter(message, args));

            var methodInfo = MethodBase.GetCurrentMethod();
            var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;

            foreach (string stackLevel in Environment.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                string trimmed = stackLevel.Trim();
                if (!trimmed.StartsWith("at Microsoft.") && !trimmed.StartsWith("at System.") && !trimmed.StartsWith("at NUnit.") && !trimmed.StartsWith("at " + fullName))
                {
                    messages.AppendLine(stackLevel);
                }
            }

            this.Log.LogMessage(MessageType.VERBOSE, messages.ToString());
        }

        /// <summary>
        /// Steps to do before MAQS starts closing drivers and releasing the logger - If not override nothing is done before logging the results
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected virtual void BeforeCleanup(TestResultType resultType)
        {
        }

        /// <summary>
        /// Check if this is a Visual Studio test
        /// </summary>
        /// <returns>True if Visaul Studio, false if NUnit</returns>
        private bool IsVSTest()
        {
            return this.testContextInstance != null;
        }

        /// <summary>
        /// Listen for any thrown exceptions
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="e">The first chance exception</param>
        private void FirstChanceHandler(object source, FirstChanceExceptionEventArgs e)
        {
            // Config settings for logging are messed up so we cannot safely log to anything but the console
            if (e.Exception is MaqsLoggingConfigException)
            {
                Console.WriteLine(StringProcessor.SafeExceptionFormatter(e.Exception));
                return;
            }

            try
            {
                // Only do this is we are logging
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    return;
                }

                Exception ex = e.Exception;

                // Check for an inner exception or if it is from the NUnit core
                if (ex.InnerException == null || ex.Source.Equals("nunit.core"))
                {
                    // This is not the test run exception we are looking for
                    return;
                }

                // Get the inner exception and specific test name
                Exception inner = ex.InnerException;
                string innerStack = inner.StackTrace ?? string.Empty;

                var message = StringProcessor.SafeExceptionFormatter(inner);
                var messages = this.LoggedExceptionList;

                // Make sure this error is associated with the current test and that we have not logged it yet
                if (innerStack.ToLower().Contains("magenic.maqs") || innerStack.Contains("at " + this.GetFullyQualifiedTestClassName() + "("))
                {
                    // Check if this is a duplicate massage
                    if (messages.Count > 0 && messages.Last().Equals(message))
                    {
                        return;
                    }

                    this.TryToLog(MessageType.ERROR, message);
                    messages.Add(message);
                }
            }
            catch (Exception ex)
            {
                this.TryToLog(MessageType.WARNING, "Failed to log exception because: " + ex.Message);
            }
        }

        /// <summary>
        /// Get the type of test result
        /// </summary>
        /// <returns>The test result type</returns>
        private TestResultType GetResultTypeVS()
        {
            switch (this.TestContext.CurrentTestOutcome)
            {
                case UnitTestOutcome.Passed:
                    return TestResultType.PASS;
                case UnitTestOutcome.Failed:
                    return TestResultType.FAIL;
                case UnitTestOutcome.Inconclusive:
                    return TestResultType.INCONCLUSIVE;
                default:
                    return TestResultType.OTHER;
            }
        }

        /// <summary>
        /// Get the test result type as text
        /// </summary>
        /// <returns>The result type as text</returns>
        private string GetResultTextVS()
        {
            return this.TestContext.CurrentTestOutcome.ToString();
        }

        /// <summary>
        /// Gets the method information from the given class and method name from any assembly
        /// in the current application domain, provide is uses the SoftAssertExpectedAsserts attribute.
        /// </summary>
        /// <param name="className">The fully qualified class name of the method.</param>
        /// <param name="testName">The name of the test method.</param>
        /// <returns>The method information from the test, provided the method uses the SoftAssertExpectedAsserts attribute.</returns>
        private MethodInfo GetMethodInfoIfUsingSoftAssertExpectedAsserts(string className, string testName)
        {
            // Loop over the assemblies
            foreach (var assemblyName in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var loadedAssembly = Assembly.Load(assemblyName.ToString());
                    var classType = loadedAssembly.GetType(className);
                    if (classType != null)
                    {
                        // Get methods and see if soft assert expects are used
                        var methods = this.GetMethods(classType, testName).ToList();
                        var hasSoftAssertExpectedAssertsAttribute = methods.Any(m => m.GetCustomAttributes<SoftAssertExpectedAssertsAttribute>(false).Any());

                        if (!hasSoftAssertExpectedAssertsAttribute)
                        {
                            // The SoftAssertExpectedAsserts attribute was not used so don't return the method information
                            return null;
                        }
                        else if (methods.Count == 1)
                        {
                            // We only have one method that fits so return its information
                            return methods[0];
                        }
                        else if (methods.Count > 1)
                        {
                            // There are multiple methods that match so log the issue and return null
                            this.TryToLog(MessageType.WARNING, $"There are mutliple methods with the name '{testName}'.  This means MAQS will not respect the SoftAssertExpectedAsserts attribute for this test.");
                            return null;
                        }
                    }
                }
                catch
                {
                    //Not all assemblies will load, that is okay.
                    //If the assembly cannot be loaded
                }
            }

            throw new InvalidOperationException($"Unable to find assembly which contains the test named'{testName}' in the class '{className}'");
        }

        /// <summary>
        /// Get related methods
        /// </summary>
        /// <param name="classType">The test class</param>
        /// <param name="testName">The test name</param>
        /// <returns>List of method information for method that match the class and test name</returns>
        private IEnumerable<MethodInfo> GetMethods(Type classType, string testName) =>
            classType.GetMethods().Where(m => m.Name.Equals(testName));

        /// <summary>
        /// Get the type of test result
        /// </summary>
        /// <returns>The test result type</returns>
        private TestResultType GetResultTypeNunit()
        {
            switch (NUnitTestContext.CurrentContext.Result.Outcome.Status)
            {
                case TestStatus.Passed:
                    return TestResultType.PASS;
                case TestStatus.Failed:
                    return TestResultType.FAIL;
                case TestStatus.Inconclusive:
                    return TestResultType.INCONCLUSIVE;
                case TestStatus.Skipped:
                    return TestResultType.SKIP;
                default:
                    return TestResultType.OTHER;
            }
        }

        /// <summary>
        /// Get the test result type as text
        /// </summary>
        /// <returns>The result type as text</returns>
        private string GetResultTextNunit()
        {
            return NUnitTestContext.CurrentContext.Result.Outcome.Status.ToString();
        }

        /// <summary>
        /// Attach all of the files in the associated files that exist to the text context 
        /// </summary>
        private void AttachAssociatedFiles()
        {
            try
            {
                // See if we can add the log file
                if (this.Log is IFileLogger logger && File.Exists(logger.FilePath))
                {
                    // Add the log file
                    AttachAssociatedFile(logger.FilePath);
                }

                // Attach all existing associated files
                foreach (string path in this.TestObject.GetArrayOfAssociatedFiles())
                {
                    if (File.Exists(path))
                    {
                        AttachAssociatedFile(path);
                    }
                }

                // All files were attached so nothing left to do
                return;
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to attach test result file because: " + e.Message);
            }

            // Not all the files were attached so write them to the log instead
            WriteAssociatedFilesNamesToLog();
        }

        /// <summary>
        /// Attach an associated file to the text context
        /// </summary>
        private void AttachAssociatedFile(string path)
        {
            // You can only attach files to VS Unit tests so check that first
            if (this.IsVSTest())
            {
                this.TestContext.AddResultFile(path);
            }
            else
            {
                NUnitTestContext.AddTestAttachment(path);
            }
        }

        /// <summary>
        /// Write list of associated files to the log
        /// </summary>
        private void WriteAssociatedFilesNamesToLog()
        {
            // Not all the files were attached so write them to the log instead
            string[] assocFiles = this.TestObject.GetArrayOfAssociatedFiles();

            if (assocFiles.Length > 0)
            {
                StringBuilder listOfFilesMessage = new StringBuilder();
                listOfFilesMessage.AppendLine("List of Associated Files: ");

                foreach (string assocPath in assocFiles)
                {
                    if (File.Exists(assocPath))
                    {
                        listOfFilesMessage.AppendLine(assocPath);
                    }
                }

                this.TryToLog(MessageType.GENERIC, listOfFilesMessage.ToString());
            }
        }
    }
}
