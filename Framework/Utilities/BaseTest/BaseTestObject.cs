//--------------------------------------------------
// <copyright file="BaseTestObject.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Holds base context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;
using System.Collections.Generic;

namespace Magenic.MaqsFramework.Utilities.BaseTest
{
    /// <summary>
    /// Base test context data
    /// </summary>
    public abstract class BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseTestObject" /> class
        /// </summary>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="perfTimerCollection">The test's performance timer collection</param>
        protected BaseTestObject(Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection)
        {
            this.Log = logger;
            this.SoftAssert = softAssert;
            this.PerfTimerCollection = perfTimerCollection;
            this.Values = new Dictionary<string, string>();
            this.Objects = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets the logger
        /// </summary>
        public Logger Log { get; private set; }

        /// <summary>
        /// Gets the performance timer collection
        /// </summary>
        public PerfTimerCollection PerfTimerCollection { get; private set; }

        /// <summary>
        /// Gets soft assert
        /// </summary>
        public SoftAssert SoftAssert { get; private set; }

        /// <summary>
        /// Gets a dictionary of string key value pairs
        /// </summary>
        public Dictionary<string, string> Values { get; private set; }

        /// <summary>
        /// Gets a dictionary of string key and object value pairs
        /// </summary>
        public Dictionary<string, object> Objects { get; private set; }

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
    }
}
