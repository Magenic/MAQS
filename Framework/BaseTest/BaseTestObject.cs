//--------------------------------------------------
// <copyright file="BaseTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds base context data</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using System;
using System.Collections.Generic;
using System.IO;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Base test context data
    /// </summary>
    public class BaseTestObject : ITestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="baseTestObject">An existing base test object</param>
        public BaseTestObject(ITestObject baseTestObject)
        {
            this.Log = baseTestObject.Log;
            this.SoftAssert = baseTestObject.SoftAssert;
            this.PerfTimerCollection = baseTestObject.PerfTimerCollection;
            this.Values = baseTestObject.Values;
            this.Objects = baseTestObject.Objects;
            this.ManagerStore = baseTestObject.ManagerStore;
            this.AssociatedFiles = baseTestObject.AssociatedFiles;

            baseTestObject.Log.LogMessage(MessageType.INFORMATION, "Setup test object " + this.PerfTimerCollection.TestName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified name</param>
        public BaseTestObject(ILogger logger, string fullyQualifiedTestName) : this(logger, new SoftAssert(logger), new PerfTimerCollection(logger, fullyQualifiedTestName), fullyQualifiedTestName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="collection">The test's performance timer collection</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public BaseTestObject(ILogger logger, ISoftAssert softAssert, IPerfTimerCollection collection, string fullyQualifiedTestName)
        {
            this.Log = logger;
            this.SoftAssert = softAssert;
            this.PerfTimerCollection = collection;
            this.Values = new Dictionary<string, string>();
            this.Objects = new Dictionary<string, object>();
            this.ManagerStore = new ManagerStore();
            this.AssociatedFiles = new HashSet<string>();

            logger.LogMessage(MessageType.INFORMATION, "Setup test object for " + fullyQualifiedTestName);
        }

        /// <summary>
        /// Gets or sets the logger
        /// </summary>
        public ILogger Log { get; set; }

        /// <summary>
        /// Gets or sets the performance timer collection
        /// </summary>
        public IPerfTimerCollection PerfTimerCollection { get; set; }

        /// <summary>
        /// Gets or sets soft assert
        /// </summary>
        public ISoftAssert SoftAssert { get; set; }

        /// <summary>
        /// Gets a dictionary of string key value pairs
        /// </summary>
        public Dictionary<string, string> Values { get; private set; }

        /// <summary>
        /// Gets assocated files
        /// </summary>
        public HashSet<string> AssociatedFiles { get; private set; }

        /// <summary>
        /// Gets a dictionary of string key and object value pairs
        /// </summary>
        public Dictionary<string, object> Objects { get; private set; }

        /// <summary>
        /// Gets the manager store
        /// </summary>
        public IManagerStore ManagerStore { get; private set; }

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
        public T GetDriverManager<T>() where T : IDriverManager
        {
            return this.ManagerStore.GetManager<T>();
        }

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <typeparam name="T">The driver type</typeparam>
        /// <param name="manager">The new driver manager</param>
        /// <param name="overrideIfExists">Should we override if this driver exists.  If it exists and we don't override than an error will be thrown.</param>
        public void AddDriverManager<T>(T manager, bool overrideIfExists = false) where T : IDriverManager
        {
            if (overrideIfExists)
            {
               this.OverrideDriverManager(manager);
            }
            else
            {
                this.AddDriverManager(typeof(T).FullName, manager);
            }
        }

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <param name="key">Key for the new driver</param>
        /// <param name="manager">The new driver manager</param>
        public void AddDriverManager(string key, IDriverManager manager)
        {
            this.ManagerStore.Add(key, manager);
        }

        /// <summary>
        /// Checks if the file exists and if so attempts to add it to the associated files set
        /// </summary>
        /// <param name="path">path of the file</param>
        /// <returns>True if the file exists and was successfully added, false if the file doesn't exist or was already added</returns>
        public bool AddAssociatedFile(string path)
        {
            if (File.Exists(path))
            {
                return this.AssociatedFiles.Add(path);
            }

            return false;
        }

        /// <summary>
        /// Removes the file path from the associated file set
        /// </summary>
        /// <param name="path">path of the file</param>
        /// <returns>True if the file path was successfully removed, false if the file wasn't in the set</returns>
        public bool RemoveAssociatedFile(string path)
        {
            return this.AssociatedFiles.Remove(path);
        }

        /// <summary>
        /// Returns an array of the file paths associated with the test object
        /// </summary>
        /// <returns>An array of the associated files</returns>
        public string[] GetArrayOfAssociatedFiles()
        {
            string[] associatedFiles = new string[this.AssociatedFiles.Count];
            this.AssociatedFiles.CopyTo(associatedFiles, 0);
            return associatedFiles;
        }

        /// <summary>
        /// Returns an array of the file paths associated with the test object
        /// </summary>
        /// <param name="path">The file path to search for</param>
        /// <returns>Whether the exact file path is contained in the set</returns>
        public bool ContainsAssociatedFile(string path)
        {
            return this.AssociatedFiles.Contains(path);
        }

        /// <summary>
        /// Override a driver manager
        /// </summary>
        /// <param name="manager">The new driver manager</param>
        public void OverrideDriverManager<T>(T manager) where T : IDriverManager
        {
            this.ManagerStore.AddOrOverride(manager);
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
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.ManagerStore is null)
            {
                return;
            }

            this.Log.LogMessage(MessageType.VERBOSE, "Start dispose");

            this.ManagerStore.Dispose();

            this.Log.LogMessage(MessageType.VERBOSE, "End dispose");
        }
    }
}
