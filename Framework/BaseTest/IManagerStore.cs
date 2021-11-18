//--------------------------------------------------
// <copyright file="IManagerStore.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>manager store interface</summary>
//--------------------------------------------------
using System;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Manager store interface
    /// </summary>
    public interface IManagerStore : IDisposable
    {
        /// <summary>
        /// Add a driver manager - Should fail if manager exists
        /// </summary>
        /// <param name="manager">Driver manager to add</param>
        void Add(IDriverManager manager);

        /// <summary>
        /// Add keyed driver manager - Should fail if manager exists
        /// </summary>
        /// <param name="key">Key name</param>
        /// <param name="manager">Driver manager to add</param>
        void Add(string key, IDriverManager manager);

        /// <summary>
        /// Add or place a driver manager
        /// </summary>
        /// <param name="manager">Driver manager to add</param>
        void AddOrOverride(IDriverManager manager);

        /// <summary>
        /// Add or replace keyed driver manager
        /// </summary>
        /// <param name="key">Key name</param>
        /// <param name="manager">Driver manager to add</param>
        void AddOrOverride(string key, IDriverManager manager);

        /// <summary>
        /// Cleanup after all drivers and clear out all managers
        /// </summary>
        void Clear();

        /// <summary>
        /// Check if the managers contains the keyed manager
        /// </summary>
        /// <param name="key">Key name</param>
        /// <returns>True if the store contains the key</returns>
        bool Contains(string key);

        /// <summary>
        /// Check if the managers contains the unkeyed manager of the the given type
        /// </summary>
        /// <typeparam name="T">Manager type</typeparam>
        /// <returns>True if the store contains the unkeyed type</returns>
        bool Contains<T>() where T : IDriverManager;

        /// <summary>
        ///  Gets the number of manager in the store
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Get unkeyed driver of type 'T' from manager of type 'U'
        /// </summary>
        /// <typeparam name="T">The type of driver</typeparam>
        /// <typeparam name="U">The type of driver manager</typeparam>
        /// <returns>The driver from the associated driver manager</returns>
        T GetDriver<T, U>() where U : IDriverManager;

        /// <summary>
        /// Get keyed driver for the associated keyed driver manager
        /// </summary>
        /// <typeparam name="T">Type of driver</typeparam>
        /// <param name="key">Key for the manager</param>
        /// <returns>The managed driver</returns>
        T GetDriver<T>(string key);

        /// <summary>
        /// Get unkeyed manager of type 'T'
        /// </summary>
        /// <typeparam name="T">The type of driver manager</typeparam>
        /// <returns>Unkeyed manager as type 'T'</returns>
        T GetManager<T>() where T : IDriverManager;

        /// <summary>
        /// Get keyed manager of type 'T'
        /// </summary>
        /// <typeparam name="T">The type of driver manager</typeparam>
        /// <returns>Keyed manager as type 'T'</returns>
        T GetManager<T>(string key) where T : IDriverManager;

        /// <summary>
        /// Get keyed manager
        /// </summary>
        /// <param name="key">Key for the manager</param>
        /// <returns>Keyed manager</returns>
        IDriverManager GetManager(string key);

        /// <summary>
        /// Remove keyed manager
        /// </summary>
        /// <param name="key">Key of the manager to remove</param>
        /// <returns>True if the manager was removed</returns>
        bool Remove(string key);

        /// <summary>
        /// Remove unkeyed manager
        /// </summary>
        /// <typeparam name="T">Type of unkeyed manager</typeparam>
        /// <returns>True if the manager was removed</returns>
        bool Remove<T>() where T : IDriverManager;

        /// <summary>
        ///  Gets the value associated with the specified key
        /// </summary>
        /// <typeparam name="T">Driver type</typeparam>
        /// <param name="key">Key name</param>
        /// <param name="driver">Driver to return</param>
        /// <returns>True if it finds the driver based on key</returns>
        bool TryGetDriver<T>(string key, out T driver);

        /// <summary>
        ///  Gets the value associated with the specified key
        /// </summary>
        /// <typeparam name="T">Driver type</typeparam>
        /// <param name="key">Key name</param>
        /// <param name="driver">Driver to return</param>
        /// <returns>True if it finds the driver based on key</returns>
        bool TryGetManager<T>(string key, out T driver) where T : IDriverManager;
    }
}
