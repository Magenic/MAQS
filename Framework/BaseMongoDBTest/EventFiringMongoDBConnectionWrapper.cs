//--------------------------------------------------
// <copyright file="EventFiringMongoDBConnectionWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing mongoDB collection interactions</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;

namespace Magenic.MaqsFramework.BaseMongoDBTest
{
    /// <summary>
    /// Wrap basic firing database interactions
    /// </summary>
    public class EventFiringMongoDBConnectionWrapper : MongoDBConnectionWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringMongoDBConnectionWrapper" /> class
        /// </summary>
        /// <param name="connectionString">The mongoDB client connection string</param>
        /// <param name="databaseName">the mongo database name string</param>
        public EventFiringMongoDBConnectionWrapper(string connectionString, string databaseName)
            : base(connectionString, databaseName)
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
    }
}