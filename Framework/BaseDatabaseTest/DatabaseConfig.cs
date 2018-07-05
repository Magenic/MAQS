//--------------------------------------------------
// <copyright file="DatabaseConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting database specific configuration values</summary>
//--------------------------------------------------
using System;
using System.Data;
using Magenic.Maqs.BaseDatabaseTest.Providers;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Get the database connection string
        /// </summary>
        /// <returns>The connection string</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseConfigUnitTests.cs" region="GetOpenConnection" lang="C#" />
        /// </example>
        public static string GetConnectionString()
        {
            return Config.GetValue("DataBaseConnectionString");
        }

        /// <summary>
        /// Get the database connection string
        /// </summary>
        /// <returns>The connection string</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseConfigUnitTests.cs" region="GetOpenConnection" lang="C#" />
        /// </example>
        public static string GetProviderTypeString()
        {
            return Config.GetValue("DataBaseProviderType");
        }

        /// <summary>
        /// Get the database timeout in seconds
        /// </summary>
        /// <returns>The timeout in seconds from the config file or default of 30 seconds when no app.config key is found</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseConfigUnitTests.cs" region="GetQueryTimeout" lang="C#" />
        /// </example>
        public static int GetQueryTimeout()
        {
            return int.Parse(Config.GetValue("DatabaseTimeout", "30"));
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
            return (IDbConnection)provider.SetupDataBaseConnection(!string.IsNullOrWhiteSpace(connectionString) ? connectionString : GetConnectionString());
        }

        /// <summary>
        /// Gets the database connection based on configuration values
        /// </summary>
        /// <returns>The database connection</returns>
        public static IDbConnection GetOpenConnection()
        {
            return GetOpenConnection(GetProviderTypeString(), GetConnectionString());
        }

        /// <summary>
        /// Gets the database connection
        /// </summary>
        /// <param name="providerType"> The provider Type to create. </param>
        /// <param name="connectionString"> The connection String. </param>
        /// <returns> The database connection </returns>
        public static IDbConnection GetOpenConnection(string providerType, string connectionString)
        {
            IDbConnection connection = null;

            try
            {
                connection = GetProvider(providerType).SetupDataBaseConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ArgumentException))
                {
                    throw e;
                }

                try
                {
                    // Try to cleanup
                    connection?.Dispose();
                }
                catch (Exception quitExecption)
                {
                    throw new Exception("Connection setup and teardown failed", quitExecption);
                }

                // Log that something went wrong
                throw new Exception("Connection setup failed.", e);
            }
        }

        /// <summary>
        /// Invokes the function to return the connection client
        /// </summary>
        /// <param name="setupDataBaseConnectionOverride"> The setup data base connection override. </param>
        /// <returns> The <see cref="IDbConnection"/>. client </returns>
        internal static IDbConnection GetOpenConnection(Func<IDbConnection> setupDataBaseConnectionOverride)
        {
            return setupDataBaseConnectionOverride();
        }

        /// <summary>
        /// Gets the provider based on the provider type.
        /// </summary>
        /// <param name="providerType"> The provider type. </param>
        /// <returns> The <see cref="IProvider"/> object </returns>
        /// <exception cref="Exception"> Throws exception if the provider type is not supported </exception>
        private static IProvider<IDbConnection> GetProvider(string providerType)
        {
            IProvider<IDbConnection> provider = null;

            switch (providerType.ToUpper())
            {
                case "SQL":
                case "SQLSERVER":
                    provider = new SQLServerProvider();
                    break;
                case "SQLITE":
                    provider = new SqliteProvider();
                    break;

                case "POSTGRESQL":
                case "POSTGRE":
                    provider = new PostgreSqlProvider();
                    break;

                default:
                    throw new ArgumentException(StringProcessor.SafeFormatter($"Provider type '{providerType}' is not supported"));
            }

            return provider;
        }
    }
}
