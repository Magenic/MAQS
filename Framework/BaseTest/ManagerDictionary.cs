//--------------------------------------------------
// <copyright file="ManagerDictionary.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Dictionary for handling driver managers</summary>
//--------------------------------------------------
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Driver manager dictionary 
    /// </summary>
    public class ManagerDictionary : Dictionary<string, DriverManager>, IDisposable
    {
        /// <summary>
        /// Get the driver for the associated driver manager
        /// </summary>
        /// <typeparam name="T">Type of driver</typeparam>
        /// <param name="key">Key for the manager</param>
        /// <returns>The managed driver</returns>
        public T GetDriver<T>(string key)
        {
            return (T)this[key].Get();
        }

        /// <summary>
        /// Get a driver
        /// </summary>
        /// <typeparam name="T">The type of driver</typeparam>
        /// <typeparam name="U">The type of driver manager</typeparam>
        /// <returns>The driver</returns>
        public T GetDriver<T, U>() where U : DriverManager
        {
            return (T)this[typeof(U).FullName].Get();
        }

        /// <summary>
        /// Add a manager
        /// </summary>
        /// <param name="manager">The manager</param>
        public void Add(DriverManager manager)
        {
            this.Add(manager.GetType().FullName, manager);
        }

        /// <summary>
        /// Add or replace a manager
        /// </summary>
        /// <param name="manager">The manager</param>
        public void AddOrOverride(DriverManager manager)
        {
            this.AddOrOverride(manager.GetType().FullName, manager);
        }

        /// <summary>
        /// Add or replace a manager
        /// </summary>
        /// <param name="key">Key for storing the manager</param>
        /// <param name="manager">The manager</param>
        public void AddOrOverride(string key, DriverManager manager)
        {
            this.Remove(key);
            this.Add(key, manager);
        }

        /// <summary>
        /// Remove a driver manager
        /// </summary>
        /// <param name="key">Key for the manager you want removed</param>
        /// <returns>True if the manager was removed</returns>
        public new bool Remove(string key)
        {
            if (this.ContainsKey(key))
            {
                this[key].Dispose();
            }

            return base.Remove(key);
        }

        /// <summary>
        /// Remove a driver manager
        /// </summary>
        /// <param name="type">The type of manager</param>
        /// <returns>True if the manager was removed</returns>
        public bool Remove(Type type)
        {
            string key = type.FullName;

            if (this.ContainsKey(key))
            {
                this[key].Dispose();
            }

            return base.Remove(key);
        }

        /// <summary>
        /// Clear the dictionary
        /// </summary>
        public new void Clear()
        {
            foreach (KeyValuePair<string, DriverManager> driver in this)
            {
                driver.Value.Dispose();
            }

            base.Clear();
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            this.Clear();
        }
    }
}