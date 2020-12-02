//--------------------------------------------------
// <copyright file="ConnectionFactory.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting database specific configuration values</summary>
//--------------------------------------------------
using System;
using System.Data;
using Magenic.Maqs.BaseDatabaseTest.Providers;
using Magenic.Maqs.Utilities.Data;

namespace Magenic.Maqs.BaseDatabaseTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class ConnectionFactory
    {
        /// <summary>
        /// Gets a database connection based on configuration values
        /// </summary>
        /// <typeparam name="T"> The type of connection client</typeparam>
        /// <param name="provider"> The custom provider.  </param>
        /// <param name="connectionString"> The connection String.  </param>
        /// <returns> The database connection client </returns>
        public static IDbConnection GetOpenConnection<T>(IProvider<T> provider, string connectionString = "") where T : class
        {
            return (IDbConnection)provider.SetupDataBaseConnection(!string.IsNullOrWhiteSpace(connectionString) ? connectionString : DatabaseConfig.GetConnectionString());
        }

        /// <summary>
        /// Gets a database connection based on configuration values
        /// </summary>
        /// <returns>The database connection</returns>
        public static IDbConnection GetOpenConnection()
        {
            return GetOpenConnection(DatabaseConfig.GetProviderTypeString(), DatabaseConfig.GetConnectionString());
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
                    throw;
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
        /// Gets the provider based on the provider type.
        /// </summary>
        /// <param name="providerType"> The provider type. </param>
        /// <returns> The <see cref="IDbConnection"/> object </returns>
        /// <exception cref="Exception"> Throws exception if the provider type is not supported </exception>
        private static IProvider<IDbConnection> GetProvider(string providerType)
        {
            IProvider<IDbConnection> provider;
            switch (providerType.ToUpper())
            {
                case "SQL":
                case "SQLSERVER":
                    provider = GetSQLServerProvider();
                    break;
                case "SQLITE":
                    provider = GetSqliteProvider();
                    break;
                case "POSTGRESQL":
                case "POSTGRE":
                    provider = GetPostgreSqlProvider();
                    break;
                case "ODP":
                case "ORACLE":
                    provider = GetOracleProvider();
                    break;
                default:
                    provider = GetCustomProviderType(providerType);
                    break;
            }

            return provider;
        }

        /// <summary>
        /// Get a SQL server provider
        /// </summary>
        /// <returns>Connection provider</returns>
        private static IProvider<IDbConnection> GetSQLServerProvider()
        {
            return new SqlServerProvider();
        }

        /// <summary>
        /// Get a SQL lite provider
        /// </summary>
        /// <returns>Connection provider</returns>
        private static IProvider<IDbConnection> GetSqliteProvider()
        {
            return new SqliteProvider();
        }

        /// <summary>
        /// Get a PostgreSQL provider
        /// </summary>
        /// <returns>Connection provider</returns>
        private static IProvider<IDbConnection> GetPostgreSqlProvider()
        {
            return new PostgreSqlProvider();
        }

        /// <summary>
        /// Get an Oracle SQL provider
        /// </summary>
        /// <returns>Connection provider</returns>
        private static IProvider<IDbConnection> GetOracleProvider()
        {
            return new OracleProvider();
        }

        /// <summary>
        /// Checks if the provider type key value is supported and try to create the type.
        /// </summary>
        /// <param name="providerType"> The fully qualified provider type name. Namespace.TypeName</param>
        /// <returns> The provider</returns>
        private static IProvider<IDbConnection> GetCustomProviderType(string providerType)
        {
            if (string.IsNullOrWhiteSpace(providerType))
            {
                throw new ArgumentException(StringProcessor.SafeFormatter($"Provider type is Empty"));
            }

            IProvider<IDbConnection> provider = null;

            try
            {
                Type type = Type.GetType(providerType);

                if (type != null)
                {
                    provider = (IProvider<IDbConnection>)Activator.CreateInstance(type);
                }
                else
                {
                    foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        type = asm.GetType(providerType);
                        if (type != null)
                        {
                            provider = (IProvider<IDbConnection>)Activator.CreateInstance(type);
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException(StringProcessor.SafeFormatter($"Provider type '{providerType}' is not supported or not a fully qualified type name. <Namespace>.<TypeName>. {e.Message}. {e.StackTrace}"));
            }

            if (provider == null)
            {
                throw new ArgumentException(StringProcessor.SafeFormatter($"Provider type '{providerType}' is not supported or not a fully qualified type name. <Namespace>.<TypeName>."));
            }

            return provider;
        }
    }
}
