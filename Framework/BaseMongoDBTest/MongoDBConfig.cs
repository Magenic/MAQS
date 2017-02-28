//--------------------------------------------------
// <copyright file="MongoDBConfig.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting MongoDB specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;

namespace Magenic.MaqsFramework.BaseMongoDBTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class MongoDBConfig
    {
        /// <summary>
        /// Get the client connection string
        /// </summary>
        /// <returns>The the connection type</returns>
        /// <example>
        /// <code source="../MongoDBUnitTests/MongoDBConfigUnitTests.cs" region="GetConnection" lang="C#" />
        /// </example>
        public static string GetConnectionString()
        {
            return Config.GetValue("MongoConnectionString");
        }

        /// <summary>
        /// Get the database connection string
        /// </summary>
        /// <returns>The database name</returns>
        /// <example>
        /// <code source="../MongoDBUnitTests/MongoDBConfigUnitTests.cs" region="GetDatabaseString" lang="C#" />
        /// </example>
        public static string GetDatabaseString()
        {
            return Config.GetValue("MongoDatabase");
        }

        /// <summary>
        /// Get the mongo collection string
        /// </summary>
        /// <returns>The mongo collection string</returns>
        public static string GetCollectionString()
        {
            return Config.GetValue("MongoCollection");
        }

        /// <summary>
        /// Get the database timeout in seconds
        /// </summary>
        /// <returns>The timeout in seconds from the config file or default of 30 seconds when no app.config key is found</returns>
        /// <example>
        /// <code source="../MongoDBUnitTests/MongoDBConfigUnitTests.cs" region="GetQueryTimeout" lang="C#" />
        /// </example>
        public static int GetQueryTimeout()
        {
            return int.Parse(Config.GetValue("DatabaseTimeout", "30"));
        }
    }
}