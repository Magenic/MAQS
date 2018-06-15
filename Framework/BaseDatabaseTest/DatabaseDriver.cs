//--------------------------------------------------
// <copyright file="DatabaseDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The basic database interactions</summary>
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Wraps the basic database interactions
    /// </summary>
    public class DatabaseDriver : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDriver" /> class
        /// </summary>
        public DatabaseDriver()
        {
            this.Connection = DatabaseConfig.GetOpenConnection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDriver" /> class
        /// </summary>
        /// <param name="providerType">The provider</param>
        /// <param name="connectionString">The base database connection string</param>
        public DatabaseDriver(string providerType, string connectionString)
        {
            this.Connection = DatabaseConfig.GetOpenConnection(providerType, connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseDriver" /> class
        /// </summary>
        /// <param name="dataBaseConnectionOverride">A database connection</param>
        public DatabaseDriver(IDbConnection dataBaseConnectionOverride)
        {
            this.Connection = dataBaseConnectionOverride;
        }

        /// <summary>
        /// Gets the connection client
        /// </summary>
        public IDbConnection Connection { get; private set; }

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int Execute(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return this.Connection.Execute(sql, param, transaction, commandTimeout, commandType);
        }

        /// <summary>
        /// Submits a query to the database
        /// </summary>
        /// <param name="sql">The SQL to execute for the query</param>
        /// <param name="param">The parameters to pass</param>
        /// <param name="transaction">The transaction to use, if any</param>
        /// <param name="buffered">Whether to buffer results in memory</param>
        /// <param name="commandTimeout">The command timeout (in seconds)</param>
        /// <param name="commandType">The type of command to execute</param>
        /// <returns>Return a sequence of dynamic objects with properties matching the columns.</returns>
        public virtual IEnumerable<dynamic> Query(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return this.Connection.Query(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        /// <summary>
        /// Executes a query, returning the data typed as T.
        /// </summary>
        /// <typeparam name="T">The type to return</typeparam>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="buffered">Whether to buffer results in memory.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns> A sequence of data of the supplied type; if a basic type (integer, string, etc) is
        ///     queried then the data from the first column in assumed, otherwise an instance
        ///    is created per row, and a direct column-name===member-name mapping is assumed
        /// (case insensitive).</returns>
        public virtual IEnumerable<T> Query<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return this.Connection.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
        }

        /// <summary>
        ///  Inserts an entity into table "T" and returns identity id or number of inserted rows if inserting a list.
        /// </summary>
        /// <typeparam name="T">The table to insert into</typeparam>
        /// <param name="entityToInsert">Entity to insert, can be list of entities</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <param name="items">Any items to log</param>
        /// <returns>Identity of inserted entity, or number of inserted rows if inserting a lists</returns>
        public virtual long Insert<T>(
            T entityToInsert,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            params string[] items)
            where T : class
        {
            return this.Connection.Insert<T>(entityToInsert, transaction, commandTimeout);
        }

        /// <summary>
        /// Delete entity in table "T".
        /// </summary>
        /// <typeparam name="T">The table</typeparam>
        /// <param name="entityToDelete">Entity to delete</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <param name="items">Any items to log</param>
        /// <returns>true if deleted, false if not found</returns>
        public virtual bool Delete<T>(
            T entityToDelete,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            params string[] items)
            where T : class
        {
            return this.Connection.Delete<T>(entityToDelete, transaction, commandTimeout);
        }

        /// <summary>
        ///  Updates entity in table "T", checks if the entity is modified if the entity is tracked by the Get() extension.
        /// </summary>
        /// <typeparam name="T">The table to update</typeparam>
        /// <param name="entityToUpdate">Entity to be updated</param>
        /// <param name="transaction">The transaction to run under, null (the default) if none</param>
        /// <param name="commandTimeout">Number of seconds before command execution timeout</param>
        /// <param name="items">Any items to log</param>
        /// <returns>true if updated, false if not found or not modified (tracked entities)</returns>
        public virtual bool Update<T>(
            T entityToUpdate,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            params string[] items)
            where T : class
        {
            return this.Connection.Update<T>(entityToUpdate, transaction, commandTimeout);
        }

        /// <summary>
        /// Dispose of the database connection
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the database connection
        /// </summary>
        /// <param name="disposing">Is the object being disposed</param>
        protected virtual void Dispose(bool disposing)
        {
            // Make sure the connection exists and is open before trying to close it
            if (disposing && this.Connection?.State == ConnectionState.Open)
            {
                this.Connection.Close();
                this.Connection.Dispose();
            }
        }
    }
}
