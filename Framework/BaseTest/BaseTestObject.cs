//--------------------------------------------------
// <copyright file="BaseTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds base context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;
using System;
using System.Collections.Generic;

namespace Magenic.MaqsFramework.BaseTest
{
    /// <summary>
    /// Base test context data
    /// </summary>
    public class BaseTestObject : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public BaseTestObject(Logger logger, SoftAssert softAssert, string fullyQualifiedTestName)
        {
            this.Log = logger;
            this.SoftAssert = softAssert;
            this.PerfTimerCollection = new PerfTimerCollection(logger, fullyQualifiedTestName);
            this.Values = new Dictionary<string, string>();
            this.Objects = new Dictionary<string, object>();
            this.DriverStore = new Dictionary<string, DriverManager>();

            logger.LogMessage(MessageType.INFORMATION, "Setup test object for " + fullyQualifiedTestName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified name</param>
        public BaseTestObject(Logger logger, string fullyQualifiedTestName)
        {
            this.Log = logger;
            this.SoftAssert = new SoftAssert(this.Log);
            this.PerfTimerCollection = new PerfTimerCollection(logger, fullyQualifiedTestName);
            this.Values = new Dictionary<string, string>();
            this.Objects = new Dictionary<string, object>();
            this.DriverStore = new Dictionary<string, DriverManager>();

            logger.LogMessage(MessageType.INFORMATION, "Setup test object for " + fullyQualifiedTestName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="baseTestObject">An existing base test object</param>
        public BaseTestObject(BaseTestObject baseTestObject)
        {
            this.Log = baseTestObject.Log;
            this.SoftAssert = baseTestObject.SoftAssert;
            this.PerfTimerCollection = baseTestObject.PerfTimerCollection;
            this.Values = baseTestObject.Values;
            this.Objects = baseTestObject.Objects;
            this.DriverStore = baseTestObject.DriverStore;

            baseTestObject.Log.LogMessage(MessageType.INFORMATION, "Setup test object");
        }

        /// <summary>
        /// Gets or sets the logger
        /// </summary>
        public Logger Log { get; set; }

        /// <summary>
        /// Gets or sets the performance timer collection
        /// </summary>
        public PerfTimerCollection PerfTimerCollection { get; set; }

        /// <summary>
        /// Gets or sets soft assert
        /// </summary>
        public SoftAssert SoftAssert { get; set; }

        /// <summary>
        /// Gets a dictionary of string key value pairs
        /// </summary>
        public Dictionary<string, string> Values { get; private set; }

        /// <summary>
        /// Gets a dictionary of string key and object value pairs
        /// </summary>
        public Dictionary<string, object> Objects { get; private set; }

        /// <summary>
        /// Gets a dictionary of string key and driver value pairs
        /// </summary>
        public Dictionary<string, DriverManager> DriverStore { get; private set; }

        /// <summary>
        /// Sets a string value, will replace if the key already exists
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value to associate with the key</param>
        public void SetValue(string key, string value)
        {
            if (this.Values.ContainsKey(key))
            {
                this.Values[key] = value;
            }
            else
            {
                this.Values.Add(key, value);
            }
        }

        /// <summary>
        /// Sets an object value, will replace if the key already exists
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value to associate with the key</param>
        public void SetObject(string key, object value)
        {
            if (this.Objects.ContainsKey(key))
            {
                this.Objects[key] = value;
            }
            else
            {
                this.Objects.Add(key, value);
            }
        }

        /// <summary>
        /// Get a driver manager of the specific type
        /// </summary>
        /// <typeparam name="T">The type of driver manager</typeparam>
        /// <returns>The driver manager</returns>
        public T GetDriver<T>() where T : DriverManager
        {
            return this.DriverStore[typeof(T).FullName] as T;
        }

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <typeparam name="T">The driver type</typeparam>
        /// <param name="driver">The new driver</param>
        /// <param name="overrideIfExists">Should we override if this driver exists.  If it exists and we don't override than an error will be thrown.</param>
        public void AddDriver<T>(T driver, bool overrideIfExists = false) where T : DriverManager
        {
            if (overrideIfExists)
            {
                this.OverrideDriver(typeof(T).FullName, driver);
            }
            else
            {
                this.AddDriver(typeof(T).FullName, driver);
            }
        }

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <param name="key">Key for the new driver</param>
        /// <param name="driver">The new driver</param>
        public void AddDriver(string key, DriverManager driver)
        {
            this.DriverStore.Add(key, driver);
        }

        /// <summary>
        /// Override a specific driver
        /// </summary>
        /// <param name="key">The driver key</param>
        /// <param name="driver">The new driver</param>
        public void OverrideDriver(string key, DriverManager driver)
        {
            if (this.DriverStore.ContainsKey(key))
            {
                this.DriverStore[key].Dispose();
                this.DriverStore[key] = driver;
            }
            else
            {
                this.DriverStore.Add(key, driver);
            }
        }

        /// <summary>
        /// Dispose the of the driver store
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the of the driver store
        /// </summary>
        /// <param name="disposing">True if you want to release managed resources</param>
        public void Dispose(bool disposing)
        {
            if (!disposing || this.DriverStore is null)
            {
                return;
            }

            this.Log.LogMessage(MessageType.VERBOSE, "Start dispose");

            // Make sure all of the individual drivers are disposed
            foreach (DriverManager singleDrive in this.DriverStore.Values)
            {
                if (singleDrive != null)
                {
                    singleDrive.Dispose();
                }
            }

            this.DriverStore = null;

            this.Log.LogMessage(MessageType.VERBOSE, "End dispose");
        }
    }
}
