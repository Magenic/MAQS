// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestProvider.cs" company="Magenic">
//   Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>
//   The test provider classed used to test custom providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Data.SqlClient;
using Magenic.MaqsFramework.BaseDatabaseTest.Providers;

namespace DatabaseUnitTests
{
    /// <summary>
    /// The test provider class for testing
    /// </summary>
    public class TestProvider : IProvider<SqlConnection>
    {
        /// <summary>
        /// Method used to setup a SQL connection client
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="SqlConnection"/> connection client. </returns>
        public SqlConnection SetupDataBaseConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = connectionString
            };

            return connection;
        }
    }
}
