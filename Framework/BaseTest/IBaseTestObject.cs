//--------------------------------------------------
// <copyright file="ITestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test object interface</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Test object interface
    /// </summary>
    public interface ITestObject : IDisposable
    {
        /// <summary>
        /// Gets or sets the logger
        /// </summary>
        ILogger Log { get; set; }

        /// <summary>
        /// Gets the manager store
        /// </summary>
        IManagerStore ManagerStore { get; }

        /// <summary>
        /// Gets a dictionary of string key and object value pairs
        /// </summary>
        Dictionary<string, object> Objects { get; }

        /// <summary>
        /// Gets or sets the performance timer collection
        /// </summary>
        IPerfTimerCollection PerfTimerCollection { get; set; }

        /// <summary>
        /// Gets or sets soft assert
        /// </summary>
        ISoftAssert SoftAssert { get; set; }

        /// <summary>
        /// Gets a dictionary of string key value pairs
        /// </summary>
        Dictionary<string, string> Values { get; }

        /// <summary>
        /// Gets assocated files, by file path
        /// </summary>
        HashSet<string> AssociatedFiles { get; }

        /// <summary>
        /// Checks if the file exists and if so attempts to add it to the associated files set
        /// </summary>
        /// <param name="path">path of the file</param>
        /// <returns>True if the file exists and was successfully added, false if the file doesn't exist or was already added</returns>
        bool AddAssociatedFile(string path);

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <param name="key">Key for the new driver</param>
        /// <param name="manager">The new driver manager</param>
        void AddDriverManager(string key, IDriverManager manager);

        /// <summary>
        /// Add a new driver
        /// </summary>
        /// <typeparam name="T">The driver type</typeparam>
        /// <param name="manager">The new driver manager</param>
        /// <param name="overrideIfExists">Should we override if this driver exists.  If it exists and we don't override than an error will be thrown.</param>
        void AddDriverManager<T>(T manager, bool overrideIfExists = false) where T : IDriverManager;

        /// <summary>
        /// Returns an array of the file paths associated with the test object
        /// </summary>
        /// <param name="path">The file path to search for</param>
        /// <returns>Whether the exact file path is contained in the set</returns>
        bool ContainsAssociatedFile(string path);

        /// <summary>
        /// Returns an array of the file paths associated with the test object
        /// </summary>
        /// <returns>An array of the associated files</returns>
        string[] GetArrayOfAssociatedFiles();

        /// <summary>
        /// Get a driver manager of the specific type
        /// </summary>
        /// <typeparam name="T">The type of driver manager</typeparam>
        /// <returns>The driver manager</returns>
        T GetDriverManager<T>() where T : IDriverManager;

        /// <summary>
        /// Override a specific driver
        /// </summary>
        /// <param name="key">The driver key</param>
        /// <param name="manager">The new driver manager</param>
        void OverrideDriverManager(string key, IDriverManager manager);

        /// <summary>
        /// Removes the file path from the associated file set
        /// </summary>
        /// <param name="path">path of the file</param>
        /// <returns>True if the file path was successfully removed, false if the file wasn't in the set</returns>
        bool RemoveAssociatedFile(string path);

        /// <summary>
        /// Sets an object value, will replace if the key already exists
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value to associate with the key</param>
        void SetObject(string key, object value);

        /// <summary>
        /// Sets a string value, will replace if the key already exists
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value to associate with the key</param>
        void SetValue(string key, string value);
    }
}