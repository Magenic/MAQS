//--------------------------------------------------
// <copyright file="BaseTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
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
    public class BaseTest
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
            this.BaseTestObjects = new ConcurrentDictionary<string, BaseTestObject>();
        }

        /// <summary>
        /// Gets or sets the performance timer collection for a test 
        /// </summary>
        public PerfTimerCollection PerfTimerCollection
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
        public SoftAssert SoftAssert
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
        public Logger Log
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
            }
        }

        /// <summary>
        /// Gets or sets the test object 
        /// </summary>
        public BaseTestObject TestObject
        {
            get
            {
                if (!this.BaseTestObjects.ContainsKey(this.GetFullyQualifiedTestClassName()))
                {
                    this.CreateNewTestObject();
                }

                return this.BaseTestObjects[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.BaseTestObjects.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets the driver store
        /// </summary>
        public ManagerDictionary ManagerStore
        {
            get
            {
                return this.TestObject.ManagerStore;
            }
        }

        /// <summary>
        /// Gets or sets the BaseContext objects
        /// </summary>
        internal ConcurrentDictionary<string, BaseTestObject> BaseTestObjects { get; set; }

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
            // Update configuration with propeties passes in by the test context
            this.UpdateConfigParameters();

            // Create the test object
            this.CreateNewTestObject();
        }

        /// <summary>
        /// Tear down after a test
        /// </summary>
        [TestCleanup]
        [TearDown]
        public void Teardown()
        {
            TestResultType resultType = this.GetResultType();
            bool forceTestFailure = false;

            // Switch the test to a failure if we have a soft assert failure
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

            this.BeforeLoggingTeardown(resultType);

            // Cleanup log files we don't want
            try
            {
                if (this.Log is FileLogger && resultType == TestResultType.PASS
                    && this.LoggingEnabledSetting == LoggingEnabled.ONFAIL)
                {
                    File.Delete(((FileLogger)this.Log).FilePath);
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to cleanup log files because: {0}", e.Message);
            }

            // Get the Fully Qualified Test Name
            string fullyQualifiedTestName = this.GetFullyQualifiedTestClassName();
            PerfTimerCollection collection = this.TestObject.PerfTimerCollection;

            // Write out the performance timers
            collection.Write(this.Log);
            if (collection.FileName != null)
            {
                this.TestObject.AddAssociatedFile(collection.FileName);
            }

            // Attach log and screen shot if we can
            this.AttachLogAndSceenshot(fullyQualifiedTestName);

            // Release the logged messages
            this.LoggedExceptions.TryRemove(fullyQualifiedTestName, out List<string> loggedMessages);
            loggedMessages = null;

            // Release the base test object
            this.BaseTestObjects.TryRemove(fullyQualifiedTestName, out BaseTestObject baseTestObject);

            // Create console logger to log subsequent messages
            this.TestObject = new BaseTestObject(new ConsoleLogger(), this.GetFullyQualifiedTestClassName());

            baseTestObject.Dispose();
            baseTestObject = null;

            // Force the test to fail
            if (forceTestFailure)
            {
                throw new AssertFailedException("Test was forced to fail in the cleanup - Likely the result of a soft assert failure.");
            }
        }

        /// <summary>
        /// Create a logger
        /// </summary>
        /// <returns>A logger</returns>
        protected Logger CreateLogger()
        {
            this.LoggedExceptionList = new List<string>();
            this.LoggingEnabledSetting = LoggingConfig.GetLoggingEnabledSetting();

            // Setup the exception listener
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.FirstChanceException += this.FirstChanceHandler;

            if (this.LoggingEnabledSetting != LoggingEnabled.NO)
            {
                return LoggingConfig.GetLogger(
                    StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    this.GetFullyQualifiedTestClassName(),
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));
            }
            else
            {
                return new ConsoleLogger();
            }
        }

        /// <summary>
        /// Get the fully qualified test name
        /// </summary>
        /// <returns>The test name including class</returns>
        protected string GetFullyQualifiedTestClassName()
        {
            if (this.testContextInstance != null)
            {
                return this.GetFullyQualifiedTestClassNameVS();
            }

            return this.GetFullyQualifiedTestClassNameNunit();
        }

        /// <summary>
        /// Get the type of test result
        /// </summary>
        /// <returns>The test result type</returns>
        protected TestResultType GetResultType()
        {
            if (this.testContextInstance != null)
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
            if (this.testContextInstance != null)
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
        /// Create a Selenium test object
        /// </summary>
        protected virtual void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new BaseTestObject(newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
        }

        /// <summary>
        /// Steps to do before logging teardown results - If not override nothing is done before logging the results
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected virtual void BeforeLoggingTeardown(TestResultType resultType)
        {
        }

        /// <summary>
        /// Get the fully qualified test name
        /// </summary>
        /// <returns>The test name including class</returns>
        private string GetFullyQualifiedTestClassNameVS()
        {
            return StringProcessor.SafeFormatter("{0}.{1}", this.TestContext.FullyQualifiedTestClassName, this.TestContext.TestName);
        }

        /// <summary>
        /// Listen for any thrown exceptions
        /// </summary>
        /// <param name="source">Source object</param>
        /// <param name="e">The first chance exception</param>
        private void FirstChanceHandler(object source, FirstChanceExceptionEventArgs e)
        {
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

                string message = inner.Message + Environment.NewLine + innerStack;
                List<string> messages = this.LoggedExceptionList;

                // Make sure this error is associated with the current test and that we have not logged it yet
                if (innerStack.ToLower().Contains("magenic.maqs") ||
                    (innerStack.Contains("at " + this.GetFullyQualifiedTestClassName() + "(") && !messages.Contains(message)))
                {
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
        /// Get the fully qualified test name
        /// </summary>
        /// <returns>The test name including class</returns>
        private string GetFullyQualifiedTestClassNameNunit()
        {
            return NUnitTestContext.CurrentContext.Test.FullName;
        }

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
        /// Update config settings with override parameters
        /// </summary>
        private void UpdateConfigParameters()
        {
            Dictionary<string, string> passedInParameters = new Dictionary<string, string>();

            try
            {
                if (this.testContextInstance != null)
                {
                    // Update configuration settings for Visual Studio unit test
                    List<string> propeties = new List<string>();
                    IDictionary<string, object> contextProperties = (IDictionary<string, object>)this.testContextInstance.GetType().InvokeMember("Properties", BindingFlags.GetProperty, null, this.testContextInstance, null);

                    // Get a list of framework reserved properties so we can exclude them
                    foreach (var property in this.testContextInstance.GetType().GetProperties())
                    {
                        propeties.Add(property.Name);
                    }

                    foreach (KeyValuePair<string, object> property in contextProperties)
                    {
                        if (!propeties.Contains(property.Key) && property.Value is string)
                        {
                            // Add the override properties
                            passedInParameters.Add(property.Key, property.Value as string);
                        }
                    }
                }
                else
                {
                    // Update parameters for an NUnit unit test
                    TestParameters parameters = NUnitTestContext.Parameters;

                    foreach (string propertyName in parameters.Names)
                    {
                        // Add the override properties
                        passedInParameters.Add(propertyName, parameters[propertyName]);
                    }
                }

                // Update configuration values
                Config.AddTestSettingValues(passedInParameters);
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed override configuration settings because: " + e.Message);
            }
        }

        /// <summary>
        /// For VS unit tests attach the log and screen shot if they exist
        /// </summary>
        /// <param name="fullyQualifiedTestName">The fully qualified test name</param>
        private void AttachLogAndSceenshot(string fullyQualifiedTestName)
        {
            bool filesWereAttached = false;
#if NET471
            try
            {
                // This only works for VS unit test so check that first
                if (this.testContextInstance != null)
                {
                    // Only attach log if it is a file logger and we can find it
                    if (this.Log is FileLogger && File.Exists(((FileLogger)this.Log).FilePath))
                    {
                        string path = ((FileLogger)this.Log).FilePath;
                        this.TestObject.AddAssociatedFile(path);
                    }

                    // Attach all existing associated files
                    string[] associatedFiles = this.TestObject.GetArrayOfAssociatedFiles();
                    foreach (string path in associatedFiles)
                    {
                        if (File.Exists(path))
                        {
                            this.TestContext.AddResultFile(path);
                        }
                    }

                    filesWereAttached = true;
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to attach test result file because: " + e.Message);
            }
#endif
            if (this.Log is FileLogger && File.Exists(((FileLogger)this.Log).FilePath) && filesWereAttached == false)
            {
                string path = ((FileLogger)this.Log).FilePath;
                this.TestObject.RemoveAssociatedFile(path);
                string listOfFilesMessage = "List of Associated Files: " + Environment.NewLine;
                string[] associatedFiles = this.TestObject.GetArrayOfAssociatedFiles();
                foreach (string assocPath in associatedFiles)
                {
                    if (File.Exists(assocPath))
                    {
                        listOfFilesMessage += assocPath + Environment.NewLine;
                    }
                }

                this.TryToLog(MessageType.GENERIC, listOfFilesMessage);
            }
        }
    }
}
