//--------------------------------------------------
// <copyright file="BaseGenericTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for test classes that setup test objects like web drivers or database connections</summary>
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

namespace Magenic.MaqsFramework.Utilities.BaseTest
{
    /// <summary>
    /// Base code for test classes that setup test objects like web drivers or database connections
    /// </summary>
    /// <typeparam name="T">Type of object under test, such as web driver and web service wrapper</typeparam>
    /// <typeparam name="U">Test object type</typeparam>
    [TestClass]
    public abstract class BaseGenericTest<T, U> where T : class where U : BaseTestObject
    {
        /// <summary>
        /// The Visual Studio TestContext
        /// </summary>
        private VSTestContext testContextInstance;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGenericTest{T, U}" /> class
        /// </summary>
        public BaseGenericTest()
        {
            this.ObjectsUnderTest = new ConcurrentDictionary<string, T>();
            this.Loggers = new ConcurrentDictionary<string, Logger>();
            this.LoggedExceptions = new ConcurrentDictionary<string, List<string>>();
            this.SoftAsserts = new ConcurrentDictionary<string, SoftAssert>();
            this.PerfTimerCollectionSet = new ConcurrentDictionary<string, PerfTimerCollection>();
            this.BaseTestObjects = new ConcurrentDictionary<string, U>();
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
        /// Gets or sets the object under test
        /// </summary>
        protected T ObjectUnderTest
        {
            get
            {
                return this.ObjectsUnderTest[this.GetFullyQualifiedTestClassName()];
            }

            set
            {
                string key = this.GetFullyQualifiedTestClassName();
                this.ObjectsUnderTest.AddOrUpdate(this.GetFullyQualifiedTestClassName(), value, (oldkey, oldvalue) => value);
            }
        }

        /// <summary>
        /// Gets or sets the test object 
        /// </summary>
        protected U TestObject
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
        /// Gets or sets the testing object - Selenium driver, web client, database connection, etc.
        /// </summary>
        private ConcurrentDictionary<string, T> ObjectsUnderTest { get; set; }

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
        /// Gets or sets the BaseContext objects
        /// </summary>
        private ConcurrentDictionary<string, U> BaseTestObjects { get; set; }

        /// <summary>
        /// Check if the test object is stored
        /// </summary>
        /// <returns>True if the test object is stored</returns>
        public bool IsObjectUnderTestStored()
        {
            return this.ObjectsUnderTest.ContainsKey(this.GetFullyQualifiedTestClassName());
        }

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

            try
            {
                // Only use event firing if we are logging
                if (LoggingConfig.GetLoggingEnabledSetting() != LoggingEnabled.NO)
                {
                    this.SetupEventFiringTester();
                }
                else
                {
                    this.SetupNoneEventFiringTester();
                }
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.ERROR, "Setup failed because: {0}\r\n{1}", e.Message, e.StackTrace);

                // Make sure we do the standard teardown
                this.Teardown();
                throw e;
            }

            this.PostSetupLogging();
        }

        /// <summary>
        /// Tear down after a test
        /// </summary>
        [TestCleanup]
        [TearDown]
        public void Teardown()
        {
            TestResultType resultType = this.GetResultType();

            // Check if Soft Alerts were checked in the test
            if (!this.SoftAssert.DidUserCheck())
            {
                this.TryToLog(MessageType.WARNING, "User did not check for soft asserts");
            }

            try
            {
                this.BeforeLoggingTeardown(resultType);
            }
            catch (Exception e)
            {
                this.TryToLog(MessageType.WARNING, "Failed before logging teardown because: {0}", e.Message);
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

            // Release the test object
            T testObject;
            this.ObjectsUnderTest.TryRemove(fullyQualifiedTestName, out testObject);
            testObject = null;

            // Find the PerfTimerCollection for this test
            string key = fullyQualifiedTestName;
            if (this.PerfTimerCollectionSet.ContainsKey(key))
            {
                PerfTimerCollection collection = this.PerfTimerCollectionSet[key];

                // Write out the performance timers
                collection.Write(this.Log);

                // Release the perf time collection for the test
                this.PerfTimerCollectionSet.TryRemove(key, out collection);
                collection = null;
            }

            // Release the logged messages
            List<string> loggedMessages;
            this.LoggedExceptions.TryRemove(fullyQualifiedTestName, out loggedMessages);
            loggedMessages = null;

            // Release the logger
            Logger logger;
            this.Loggers.TryRemove(fullyQualifiedTestName, out logger);
            logger = null;

            // Relese the soft assert object
            SoftAssert softAssert;
            this.SoftAsserts.TryRemove(fullyQualifiedTestName, out softAssert);
            softAssert = null;
        }

        /// <summary>
        /// Setup event firing test object 
        /// </summary>
        /// <example>  
        /// This sample shows what an overload for the <see cref="SetupEventFiringTester"/> method may look like
        /// <code> 
        /// protected override void SetupEventFiringTester()
        /// {
        ///    this.WebDriver = this.GetBrowser();
        ///    this.WebDriver = new EventFiringWebDriver(this.WebDriver);
        ///    this.MapEvents((EventFiringWebDriver)this.WebDriver);
        /// }
        /// </code> 
        /// </example> 
        protected abstract void SetupEventFiringTester();

        /// <summary>
        /// Setup none event firing test object 
        /// </summary>
        /// <example>  
        /// This sample shows what an overload for the <see cref="SetupNoneEventFiringTester"/> method may look like
        /// <code> 
        /// protected override void SetupNoneEventFiringTester()
        /// {
        ///    this.WebDriver = this.GetBrowser();
        /// }
        /// </code> 
        /// </example> 
        protected abstract void SetupNoneEventFiringTester();

        /// <summary>
        /// Create a test object
        /// </summary>
        protected abstract void CreateNewTestObject();

        /// <summary>
        /// Overload function for doing post setup logging
        /// </summary>
        /// <remarks> 
        /// If not override no post setup logging will be done 
        /// </remarks> 
        protected virtual void PostSetupLogging()
        {
        }

        /// <summary>
        /// Steps to do before logging teardown results - If not override nothing is done before logging the results
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected virtual void BeforeLoggingTeardown(TestResultType resultType)
        {
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

                string setupNamespace = typeof(BaseGenericTest<T, U>).Namespace;

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
    }
}
