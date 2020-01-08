//--------------------------------------------------
// <copyright file="OracleProvider.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>OracleProvider class</summary>
//--------------------------------------------------

using Oracle.ManagedDataAccess.Client;

namespace Magenic.Maqs.BaseDatabaseTest.Providers
{
    /// <summary>
    /// The SQL Oracle server provider.
    /// </summary>
    public class OracleProvider : IProvider<OracleConnection>
    {
        /// <summary>
        /// Method used to create a new connection for Oracle databases
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="OracleConnection"/> connection client. </returns>
        OracleConnection IProvider<OracleConnection>.SetupDataBaseConnection(string connectionString)
        {
            OracleConnection connection = new OracleConnection
            {
                ConnectionString = connectionString
            };

            return connection;
        }
    }
}
