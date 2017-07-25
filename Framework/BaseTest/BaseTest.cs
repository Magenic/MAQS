//--------------------------------------------------
// <copyright file="BaseTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for tests without a system under test object like web drivers or database connections</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text;
using NUnitTestContext = NUnit.Framework.TestContext;
using VSTestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Magenic.MaqsFramework.BaseTest
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
            this.Loggers = new ConcurrentDictionary<string, Logger>();
            this.LoggedExceptions = new ConcurrentDictionary<string, List<string>>();
            this.SoftAsserts = new ConcurrentDictionary<string, SoftAssert>();
            this.PerfTimerCollectionSet = new ConcurrentDictionary<string, PerfTimerCollection>();
            this.BaseTestObjects = new ConcurrentDictionary<string, BaseTestObject>();
        }

        /// <summary>
        /// Gets or sets the performance timer collection for a test 
        /// </summary>
        public PerfTimerCollection PerfTimerCollection
        {
            get
            {
                string key = this.GetFullyQualifiedTestClassName();
                if (!this.PerfTimerCollectionSet.ContainsKey(key))
                {
                    this.PerfTimerCollection = new PerfTimerCollection(this.Log, key);
                }

                return this.PerfTimerCollectionSet[key];
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.PerfTimerCollectionSet.AddOrUpdate(key, value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets or sets the SoftAssert objects
        /// </summary>
        public SoftAssert SoftAssert
        {
            get
            {
                if (!this.SoftAsserts.ContainsKey(this.GetFullyQualifiedTestClassName()))
                {
                    this.SoftAssert = this.GetSoftAssert();
                }

                return this.SoftAsserts[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.SoftAsserts.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets or sets the testing object
        /// </summary>
        public Logger Log
        {
            get
            {
                // If no logger is provided fall back to the console logger
                if (!this.Loggers.ContainsKey(this.GetFullyQualifiedTestClassName()))
                {
                    return new ConsoleLogger();
                }

                return this.Loggers[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.Loggers.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
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
                string key = this.GetFullyQualifiedTestClassName();
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
        /// Gets or sets the BaseContext objects
        /// </summary>
        internal ConcurrentDictionary<string, BaseTestObject> BaseTestObjects { get; set; }

        /// <summary>
        /// Gets or sets the test object 
        /// </summary>
        protected BaseTestObject TestObject
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
        /// Gets the logging enable flag
        /// </summary>
        protected LoggingEnabled LoggingEnabledSetting { get; private set; }

        /// <summary>
        /// Gets or sets the Dictionary of performance timer collections (for multi-threaded test execution)
        /// </summary>
        private ConcurrentDictionary<string, PerfTimerCollection> PerfTimerCollectionSet { get; set; }

        /// <summary>
        /// Gets or sets the logging objects
        /// </summary>
        private ConcurrentDictionary<string, Logger> Loggers { get; set; }

        /// <summary>
        /// Gets or sets the logged exceptions
        /// </summary>
        private ConcurrentDictionary<string, List<string>> LoggedExceptions { get; set; }

        /// <summary>
        /// Gets or sets the soft assert objects
        /// </summary>
        private ConcurrentDictionary<string, SoftAssert> SoftAsserts { get; set; }

        /// <summary>
        /// Setup before a test
        /// </summary>
        [TestInitialize]
        [SetUp]
        public void Setup()
        {
            // Update configuration with propeties passes in by the test context
            this.UpdateConfigParameters();

            // Make sure the logging works
            this.SetupLogging();
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

            if (this.PerfTimerCollectionSet.ContainsKey(fullyQualifiedTestName))
            {
                PerfTimerCollection collection = this.PerfTimerCollectionSet[fullyQualifiedTestName];

                // Write out the performance timers
                collection.Write(this.Log);

                // Release the perf time collection for the test
                this.PerfTimerCollectionSet.TryRemove(fullyQualifiedTestName, out collection);
                collection = null;
            }

            // Attach log and screen shot if we can
            this.AttachLogAndSceenshot(fullyQualifiedTestName);

            // Release the logged messages
            List<string> loggedMessages;
            this.LoggedExceptions.TryRemove(fullyQualifiedTestName, out loggedMessages);
            loggedMessages = null;

            // Relese the soft assert object
            SoftAssert softAssert;
            this.SoftAsserts.TryRemove(fullyQualifiedTestName, out softAssert);
            softAssert = null;

            // Release the logger
            Logger logger;
            this.Loggers.TryRemove(fullyQualifiedTestName, out logger);
            logger = null;

            // Relese the base test object
            BaseTestObject baseTestObject;
            this.BaseTestObjects.TryRemove(fullyQualifiedTestName, out baseTestObject);
            baseTestObject = null;

            // Force the test to fail
            if (forceTestFailure)
            {
                throw new Exception("Test was forced to fail in the cleanup - Likely the result of a soft assert failure.");
            }
        }

        /// <summary>
        /// Method to get a new soft assert object
        /// </summary>
        /// <returns>A soft assert object</returns>
        protected virtual SoftAssert GetSoftAssert()
        {
            return new SoftAssert(this.Log);
        }

        /// <summary>
        /// Setup logging data
        /// </summary>
        protected void SetupLogging()
        {
            this.LoggedExceptionList = new List<string>();
            this.LoggingEnabledSetting = LoggingConfig.GetLoggingEnabledSetting();

            // Setup the exception listener
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.FirstChanceException += this.FirstChanceHandler;

            if (this.LoggingEnabledSetting != LoggingEnabled.NO)
            {
                this.Log = LoggingConfig.GetLogger(
                    StringProcessor.SafeFormatter(
                    "{0} - {1}",
                    this.GetFullyQualifiedTestClassName(),
                    DateTime.UtcNow.ToString("yyyy-MM-dd-hh-mm-ss-ffff", CultureInfo.InvariantCulture)));
            }
            else
            {
                this.Log = new ConsoleLogger();
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

            var methodInfo = System.Reflection.MethodBase.GetCurrentMethod();
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
            this.TestObject = new BaseTestObject(this.Log, this.SoftAssert, this.PerfTimerCollection);
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
                if (!this.DoesLoggerExist())
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
                string innerStack = inner.StackTrace == null ? string.Empty : inner.StackTrace;

                string message = inner.Message + Environment.NewLine + innerStack;
                List<string> messages = this.LoggedExceptionList;

                string setupNamespace = typeof(BaseTest).Namespace;

                // Make sure this error is associated with the current test and that we have not logged it yet
                if (innerStack.Contains(setupNamespace) ||
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
        /// Does a logger exist for the test type
        /// </summary>
        /// <returns>True if a logger exists</returns>
        private bool DoesLoggerExist()
        {
            return this.Loggers.ContainsKey(this.GetFullyQualifiedTestClassName());
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

                    // Get a list of framework reserved properties so we can exclude them
                    foreach (var property in this.testContextInstance.GetType().GetProperties())
                    {
                        propeties.Add(property.Name);
                    }

                    foreach (DictionaryEntry property in this.testContextInstance.Properties)
                    {
                        if (!propeties.Contains(property.Key as string) && property.Value is string)
                        {
                            // Add the override properties
                            passedInParameters.Add(property.Key as string, property.Value as string);
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
            try
            {
                // This only works for VS unit test so check that first
                if (this.testContextInstance != null)
                {
                    // Only attach if we can find the log file
                    if (this.Loggers.ContainsKey(fullyQualifiedTestName) && this.Loggers[fullyQualifiedTestName] is FileLogger && File.Exists(((FileLogger)this.Loggers[fullyQualifiedTestName]).FilePath))
                    {
                        string path = ((FileLogger)this.Loggers[fullyQualifiedTestName]).FilePath;
                        string nameWithoutExtension = Path.GetFileNameWithoutExtension(path);

                        // Find all files that share the same base file name - file name without extension
                        foreach (string file in Directory.GetFiles(Path.GetDirectoryName(path), fullyQualifiedTestName + "*", SearchOption.TopDirectoryOnly))
                        {
                            if (nameWithoutExtension.Equals(Path.GetFileNameWithoutExtension(file), StringComparison.CurrentCultureIgnoreCase))
                            {
                                this.TestContext.AddResultFile(file);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed to attach log or screenshot because: " + e.Message);
            }
        }
    }
}
