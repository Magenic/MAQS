//--------------------------------------------------
// <copyright file="DatabaseConfig.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting database specific configuration values</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseDatabaseTest.Providers;
using Magenic.Maqs.Utilities.Helper;
using System.Data;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        ///  Static name for the database configuration section
        /// </summary>
        private const string DATABASESECTIION = "DatabaseMaqs";

        /// <summary>
        /// Get the database connection string
        /// </summary>
        /// <returns>The connection string</returns>
        public static string GetConnectionString()
        {
            return Config.GetValueForSection(DATABASESECTIION, "DataBaseConnectionString");
        }

        /// <summary>
        /// Get the database provider type string
        /// </summary>
        /// <returns>The provider type string</returns>
        public static string GetProviderTypeString()
        {
            return Config.GetValueForSection(DATABASESECTIION, "DataBaseProviderType");
        }

        /// <summary>
        /// Gets the database connection based on configuration values
        /// </summary>
        /// <typeparam name="T"> The type of connection client</typeparam>
        /// <param name="provider"> The custom provider.  </param>
        /// <param name="connectionString"> The connection String.  </param>
        /// <returns> The database connection client </returns>
        public static IDbConnection GetOpenConnection<T>(IProvider<T> provider, string connectionString = "") where T : class
        {
            return ConnectionFactory.GetOpenConnection(provider, connectionString);
        }

        /// <summary>
        /// Gets the database connection based on configuration values
        /// </summary>
        /// <returns>The database connection</returns>
        public static IDbConnection GetOpenConnection()
        {
            return ConnectionFactory.GetOpenConnection(GetProviderTypeString(), GetConnectionString());
        }

        /// <summary>
        /// Gets the database connection
        /// </summary>
        /// <param name="providerType"> The provider Type to create. </param>
        /// <param name="connectionString"> The connection String. </param>
        /// <returns> The database connection </returns>
        public static IDbConnection GetOpenConnection(string providerType, string connectionString)
        {
            return ConnectionFactory.GetOpenConnection(providerType, connectionString);
        }
    }
}
