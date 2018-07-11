//--------------------------------------------------
// <copyright file="SqliteProvider.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>SQLiteProvider class</summary>
//--------------------------------------------------

using Microsoft.Data.Sqlite;

namespace Magenic.Maqs.BaseDatabaseTest.Providers
{
    /// <summary>
    /// The Sqlite provider.
    /// </summary>
    public class SqliteProvider : IProvider<SqliteConnection>
    {
        /// <summary>
        /// Method used to create a new connection for SQLite databases
        /// </summary>
        /// <param name="connectionString"> The connection string. </param>
        /// <returns> The <see cref="SqliteConnection"/> connection </returns>
        public SqliteConnection SetupDataBaseConnection(string connectionString)
        {
            SqliteConnection connection = new SqliteConnection(connectionString);
            SQLitePCL.Batteries.Init();

            return connection;
        }
    }
}
