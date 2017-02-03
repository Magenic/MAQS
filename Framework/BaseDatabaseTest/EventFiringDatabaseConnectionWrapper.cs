//--------------------------------------------------
// <copyright file="EventFiringDatabaseConnectionWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing database interactions</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Wrap basic firing database interactions
    /// </summary>
    public class EventFiringDatabaseConnectionWrapper : DatabaseConnectionWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringDatabaseConnectionWrapper" /> class
        /// </summary>
        /// <param name="connectionString">The database connection string</param>
        public EventFiringDatabaseConnectionWrapper(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringDatabaseConnectionWrapper" /> class
        /// </summary>
        /// <param name="setupDataBaseConnectionOverride">The database connection override</param>
        public EventFiringDatabaseConnectionWrapper(Func<SqlConnection> setupDataBaseConnectionOverride)
            : base(setupDataBaseConnectionOverride)
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
        /// Run a query
        /// </summary>
        /// <param name="query">The SQL query string</param>
        /// <returns>The result data table</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="QueryAndGetTableData" lang="C#" />
        /// </example>
        public override DataTable QueryAndGetDataTable(string query)
        {
            try
            {
                this.RaiseEvent("query", query);
                return base.QueryAndGetDataTable(query);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Run non query SQL
        /// </summary>
        /// <param name="nonQuery">SQL that does not return query results</param>
        /// <returns>The number of affected rows</returns>
        public override int NonQueryAndGetRowsAffected(string nonQuery)
        {
            try
            {
                this.RaiseEvent("non-query", nonQuery);
                return base.NonQueryAndGetRowsAffected(nonQuery);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
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
        public override int RunActionProcedure(string procedureName, params SqlParameter[] parameters)
        {
            StringBuilder parameterText = new StringBuilder();
            parameterText.AppendLine(procedureName);

            foreach (SqlParameter parameter in parameters)
            {
                parameterText.AppendLine(StringProcessor.SafeFormatter("{0} {1}", parameter.ParameterName, parameter.Value.ToString()));
            }

            this.RaiseEvent("action procedure", this.GetProcedureNameAndParameters(procedureName, parameters));
            return base.RunActionProcedure(procedureName, parameters);
        }

        /// <summary>
        /// Runs a procedure that returns query results
        /// </summary>
        /// <param name="procedureName">The procedure name</param>
        /// <param name="parameters">The procedure parameters</param>
        /// <returns>A DataTable containing the results of the procedure</returns>
        /// /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="RunQueryProcedure" lang="C#" />
        /// </example>
        public override DataTable RunQueryProcedure(string procedureName, params SqlParameter[] parameters)
        {
            this.RaiseEvent("query procedure", this.GetProcedureNameAndParameters(procedureName, parameters));
            return base.RunQueryProcedure(procedureName, parameters);
        }

        /// <summary>
        /// Checks if a stored procedure exists
        /// </summary>
        /// <param name="procedureName"> the procedure name</param>
        /// <returns>A boolean representing whether the procedure was found</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseUnitTestsWithWrapper.cs" region="ProcedureExists" lang="C#" />
        /// </example>
        public override bool CheckForProcedure(string procedureName)
        {
            this.RaiseEvent("check for procedure", procedureName);
            return base.CheckForProcedure(procedureName);
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
            if (this.DatabaseEvent != null)
            {
                this.DatabaseEvent(this, message);
            }
        }

        /// <summary>
        /// Database error event
        /// </summary>
        /// <param name="message">The event error message</param>
        protected virtual void OnErrorEvent(string message)
        {
            if (this.DatabaseErrorEvent != null)
            {
                this.DatabaseErrorEvent(this, message);
            }
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
        /// Raise an exception message
        /// </summary>
        /// <param name="e">The exception</param>
        private void RaiseErrorMessage(Exception e)
        {
            this.OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0}{1}{2}", e.Message, Environment.NewLine, e.ToString()));
        }

        /// <summary>
        /// Create user friendly string with the procedure name, parameters and the parameter values
        /// </summary>
        /// <param name="procedureName">The procedure name</param>
        /// <param name="parameters">The procedure parameters</param>
        /// <returns>A user friendly string</returns>
        private string GetProcedureNameAndParameters(string procedureName, params SqlParameter[] parameters)
        {
            StringBuilder parameterText = new StringBuilder();
            parameterText.Append(procedureName);

            if (parameters.Length > 0)
            {
                parameterText.AppendLine(string.Empty);
                parameterText.AppendLine("Parameters");

                foreach (SqlParameter parameter in parameters)
                {
                    parameterText.AppendLine(StringProcessor.SafeFormatter("{0} {1}", parameter.ParameterName, parameter.Value.ToString()));
                }
            }

            return parameterText.ToString().Trim();
        }
    }
}
