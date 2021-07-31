//--------------------------------------------------
// <copyright file="ManagerDictionary.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
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
    public class ManagerStore : IManagerStore
    {
        /// <summary>
        /// Dictionary to hold all driver managers
        /// </summary>
        private readonly Dictionary<string, IDriverManager> managerDictionary = new Dictionary<string, IDriverManager>();

        /// <summary>
        /// Gets the count of managed drivers
        /// </summary>
        public int Count => managerDictionary.Count;

        /// <summary>
        /// Get the driver for the associated driver manager
        /// </summary>
        /// <typeparam name="T">Type of driver</typeparam>
        /// <param name="key">Key for the manager</param>
        /// <returns>The managed driver</returns>
        public T GetDriver<T>(string key)
        {
            return (T)managerDictionary[key].Get();
        }

        /// <summary>
        /// Get a driver
        /// </summary>
        /// <typeparam name="T">The type of driver</typeparam>
        /// <typeparam name="U">The type of driver manager</typeparam>
        /// <returns>The driver</returns>
        public T GetDriver<T, U>() where U : IDriverManager
        {
            return (T)GetManager<U>().Get();
        }

        public IDriverManager GetManager()
        {
            return GetManager<IDriverManager>();
        }

        public IDriverManager GetManager(string key)
        {
            return GetManager<IDriverManager>(key);
        }

        public T GetManager<T>() where T : IDriverManager
        {
            return (T)managerDictionary[typeof(T).FullName];
        }

        public T GetManager<T>(string key) where T : IDriverManager
        {
            return (T)managerDictionary[key];
        }

        /// <summary>
        /// Add a manager
        /// </summary>
        /// <param name="manager">The manager</param>
        public void Add(IDriverManager manager)
        {
            managerDictionary.Add(manager.GetType().FullName, manager);
        }

        /// <summary>
        /// Add a manager
        /// </summary>
        /// <param name="key">Key for storing the manager</param>
        /// <param name="manager">The manager</param>
        public void Add(string key, IDriverManager manager)
        {
            managerDictionary.Add(key, manager);
        }

        /// <summary>
        /// Add or replace a manager
        /// </summary>
        /// <param name="manager">The manager</param>
        public void AddOrOverride(IDriverManager manager)
        {
            this.AddOrOverride(manager.GetType().FullName, manager);
        }

        /// <summary>
        /// Add or replace a manager
        /// </summary>
        /// <param name="key">Key for storing the manager</param>
        /// <param name="manager">The manager</param>
        public void AddOrOverride(string key, IDriverManager manager)
        {
            managerDictionary[key]?.Dispose();
            managerDictionary.Remove(key);
            managerDictionary.Add(key, manager);
        }

        /// <summary>
        /// Remove a driver manager
        /// </summary>
        /// <param name="key">Key for the manager you want removed</param>
        /// <returns>True if the manager was removed</returns>
        public bool Remove(string key)
        {
            if (managerDictionary.ContainsKey(key))
            {
                managerDictionary[key].Dispose();
            }

            return managerDictionary.Remove(key);
        }

        /// <summary>
        /// Remove a driver manager
        /// </summary>
        /// <param name="type">The type of manager</param>
        /// <returns>True if the manager was removed</returns>
        public bool Remove(Type type)
        {
            string key = type.FullName;

            if (managerDictionary.ContainsKey(key))
            {
                managerDictionary[key].Dispose();
            }

            return managerDictionary.Remove(key);
        }

        /// <summary>
        /// Clear the dictionary
        /// </summary>
        public void Clear()
        {
            foreach (KeyValuePair<string, IDriverManager> driver in managerDictionary)
            {
                driver.Value.Dispose();
            }

            managerDictionary.Clear();
        }

        /// <summary>
        /// Cleanup the driver
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleanup the driver
        /// </summary>
        /// <param name="disposing">Dispose managed objects</param>
        protected virtual void Dispose(bool disposing)
        {
            // Only dealing with managed objects
            if (disposing)
            {
                // Make sure all of the individual drivers are disposed
                foreach (IDriverManager singleDrive in managerDictionary.Values)
                {
                    singleDrive?.Dispose();
                }

                managerDictionary.Clear();
            }
        }

        public bool Contains(string key)
        {
            return managerDictionary.ContainsKey(key);
        }

        public bool Contains(Type type)
        {
            return this.Contains(type.FullName);
        }

        public bool Contains<T>() where T : IDriverManager
        {
            return this.Contains(typeof(T).FullName);
        }
    }
}