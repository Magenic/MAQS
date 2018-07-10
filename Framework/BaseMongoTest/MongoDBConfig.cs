//--------------------------------------------------
// <copyright file="MongoDBConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting MongoDB specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Helper;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class MongoDBConfig
    {
        /// <summary>
        ///  Static name for the mongo configuration section
        /// </summary>
        private const string MONGOSECTION = "MongoMaqs";

        /// <summary>
        /// Get the client connection string
        /// </summary>
        /// <returns>The the connection type</returns>
        /// <example>
        /// <code source="../MongoDBUnitTests/MongoDBConfigUnitTests.cs" region="GetConnection" lang="C#" />
        /// </example>
        public static string GetConnectionString()
        {
            return Config.GetValueForSection(MONGOSECTION, "MongoConnectionString");
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
            return Config.GetValueForSection(MONGOSECTION, "MongoDatabase");
        }

        /// <summary>
        /// Get the mongo collection string
        /// </summary>
        /// <returns>The mongo collection string</returns>
        public static string GetCollectionString()
        {
            return Config.GetValueForSection(MONGOSECTION, "MongoCollection");
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
            return int.Parse(Config.GetValueForSection(MONGOSECTION, "MongoTimeout", "30"));
        }
    }
}