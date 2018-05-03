//--------------------------------------------------
// <copyright file="EventFiringDatabaseConnectionWrapper.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing database interactions</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Wrap basic firing database interactions
    /// </summary>
    public class EventFiringDatabaseConnectionWrapper : DatabaseConnectionWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringDatabaseConnectionWrapper"/> class
        /// </summary>
        /// <param name="connectionType"> The connection Type. </param>
        /// <param name="connectionString"> The database connection string </param>
        public EventFiringDatabaseConnectionWrapper(string connectionType, string connectionString)
            : base(connectionType, connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringDatabaseConnectionWrapper" /> class
        /// </summary>
        /// <param name="setupDataBaseConnectionOverride">The database connection override</param>
        public EventFiringDatabaseConnectionWrapper(Func<IDbConnection> setupDataBaseConnectionOverride)
            : base(setupDataBaseConnectionOverride)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringDatabaseConnectionWrapper"/> class.
        /// </summary>
        protected EventFiringDatabaseConnectionWrapper()
        {
        }

        /// <summary>
        /// Database event
        /// </summary>
        public event EventHandler<string> DatabaseEvent;

        /// <summary>
        /// database error event
        /// </summary>
        public event EventHandler<string> DatabaseErrorEvent;

        /// <summary>
        /// Execute parameterized SQL.
        /// </summary>
        /// <param name="sql">The SQL to execute for the query.</param>
        /// <param name="param">The parameters to pass, if any.</param>
        /// <param name="transaction">The transaction to use, if any.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns>The number of rows affected.</returns>
        public override int Execute(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            try
            {
                this.RaiseEvent("execute", sql);
                return base.Execute(sql, param, transaction, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Submits a query to the database
        /// </summary>
        /// <param name="sql">The SQL to execute in the query.</param>
        /// <param name="param">The parameters to pass.</param>
        /// <param name="transaction">The transaction to use.</param>
        /// <param name="buffered">Whether to buffer results in memory.</param>
        /// <param name="commandTimeout">The command timeout (in seconds).</param>
        /// <param name="commandType">The type of command to execute.</param>
        /// <returns>Return a sequence of dynamic objects with properties matching the columns.</returns>
        public override IEnumerable<dynamic> Query(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            try
            {
                this.RaiseEvent("query", sql);
                return base.Query(sql, param, transaction, buffered, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
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
        public override IEnumerable<T> Query<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            bool buffered = true,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            try
            {
                this.RaiseEvent("query", sql);
                return base.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
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
        public override long Insert<T>(
            T entityToInsert,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            params string[] items)
        {
            try
            {
                this.RaiseEvent("insert", items);
                return base.Insert<T>(entityToInsert, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
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
        public override bool Delete<T>(
            T entityToDelete,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            params string[] items)
        {
            try
            {
                this.RaiseEvent("delete", items);
                return base.Delete<T>(entityToDelete, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
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
        public override bool Update<T>(
            T entityToUpdate,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            params string[] items)
        {
            try
            {
                this.RaiseEvent("update", items);
                return base.Update<T>(entityToUpdate, transaction, commandTimeout);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Dispose of the database connection
        /// </summary>
        public override void Dispose()
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Releasing connection"));
                base.Dispose();
                this.OnEvent(StringProcessor.SafeFormatter("Released connection"));
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Database event
        /// </summary>
        /// <param name="message">event message</param>
        protected virtual void OnEvent(string message)
        {
            this.DatabaseEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Database error event
        /// </summary>
        /// <param name="message">The event error message</param>
        protected virtual void OnErrorEvent(string message)
        {
            this.DatabaseErrorEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Raise an event message
        /// </summary>
        /// <param name="actionType">The type of action</param>
        /// <param name="query">The query string</param>
        private void RaiseEvent(string actionType, string query)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Performing {0} with:\r\n{1}", actionType, query));
            }
            catch (Exception e)
            {
                this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
            }
        }

        /// <summary>
        /// Raise an event message
        /// </summary>
        /// <param name="actionType">The type of action</param>
        /// <param name="items">The items to log</param>
        private void RaiseEvent(string actionType, params string[] items)
        {
            try
            {
                StringBuilder builder = new StringBuilder();

                foreach (var item in items)
                {
                    builder.AppendLine(item);
                }
                
                this.OnEvent(StringProcessor.SafeFormatter("Performing {0} with:\r\n{1}", actionType, builder.ToString()));
            }
            catch (Exception e)
            {
                this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
            }
        }

        /// <summary>
        /// Raise an exception message
        /// </summary>
        /// <param name="e">The exception</param>
        private void RaiseErrorMessage(Exception e)
        {
            this.OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0}{1}{2}", e.Message, Environment.NewLine, e.ToString()));
        }
    }
}
