//--------------------------------------------------
// <copyright file="DatabaseConnectionWrapper.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The basic database interactions</summary>
//--------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Wraps the basic database interactions
    /// </summary>
    public class DatabaseConnectionWrapper : IDisposable
    {
        /// <summary>
        /// when the connection is created it is held here
        /// </summary>
        private SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionWrapper" /> class
        /// </summary>
        /// <param name="connectionString">The base database connection string</param>
        public DatabaseConnectionWrapper(string connectionString)
        {
            this.connection = this.SetupDataBaseConnection(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseConnectionWrapper" /> class
        /// </summary>
        /// <param name="setupDataBaseConnectionOverride">A function that returns the database connection</param>
        public DatabaseConnectionWrapper(Func<SqlConnection> setupDataBaseConnectionOverride)
        {
            this.connection = setupDataBaseConnectionOverride();
        }

        /// <summary>
        /// Dispose of the database connection
        /// </summary>
        public virtual void Dispose()
        {
            // Make sure the connection exists and is open before trying to close it
            if (this.connection != null && this.connection.State == ConnectionState.Open)
            {
                this.connection.Close();
            }
        }

        /// <summary>
        /// Runs a query
        /// </summary>
        /// <param name="query"> the SQL query the test provides</param>
        /// <returns>A DataTable containing the results of the query</returns>
        /// <returns>The result data table</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="QueryAndGetTableData" lang="C#" />
        /// </example>
        public virtual DataTable QueryAndGetDataTable(string query)
        {
            SqlCommand command = new SqlCommand(query, this.connection);
            command.CommandTimeout = DatabaseConfig.GetQueryTimeout();

            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            return table;
        }

        /// <summary>
        /// Run non query SQL
        /// </summary>
        /// <param name="nonQuery">SQL that does not return query results</param>
        /// <returns>The number of affected rows</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="NonQuerySqlCall" lang="C#" />
        /// </example>
        public virtual int NonQueryAndGetRowsAffected(string nonQuery)
        {
            SqlCommand command = new SqlCommand(nonQuery, this.connection);
            command.CommandTimeout = DatabaseConfig.GetQueryTimeout();

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="procedureName"> the procedure name</param>
        /// <returns>A boolean representing whether the procedure was found</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="ProcedureExists" lang="C#" />
        /// </example>
        public virtual bool CheckForProcedure(string procedureName)
        {
            string query = "SELECT COUNT(*) FROM sysobjects WHERE name=@sprocName";
            SqlCommand command = new SqlCommand(query, this.connection);
            SqlParameter sprocName = command.Parameters.Add("@sprocName", SqlDbType.VarChar, 100);
            sprocName.Value = procedureName;
            command.CommandTimeout = DatabaseConfig.GetQueryTimeout();

            int count = (int)command.ExecuteScalar();
            return count > 0;
        }

        /// <summary>
        /// Runs a procedure that does an action and returns the number of elements affected
        /// </summary>
        /// <param name="procedureName">The procedure name</param>
        /// <param name="parameters">The procedure parameters</param>
        /// <returns>The number of rows affected</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="RunActionProcedure" lang="C#" />
        /// </example>
        public virtual int RunActionProcedure(string procedureName, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(procedureName, this.connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = DatabaseConfig.GetQueryTimeout();

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Runs a procedure that returns query results
        /// </summary>
        /// <param name="procedureName">The procedure name</param>
        /// <param name="parameters">The procedure parameters</param>
        /// <returns>A DataTable containing the results of the procedure</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="RunQueryProcedure" lang="C#" />
        /// </example>
        public virtual DataTable RunQueryProcedure(string procedureName, params SqlParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(procedureName, this.connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = DatabaseConfig.GetQueryTimeout();

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            return table;
        }

        /// <summary> 
        /// Default database connection setup - Override this function to create your own connection
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        /// <returns>The http client</returns>
        protected virtual SqlConnection SetupDataBaseConnection(string connectionString)
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = connectionString;
            connection.Open();

            return connection;
        }
    }
}
