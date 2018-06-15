//--------------------------------------------------
// <copyright file="EventFiringMongoDBDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing mongoDB collection interactions</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Collections.Generic;

namespace Magenic.MaqsFramework.BaseMongoTest
{
    /// <summary>
    /// Wrap basic firing database interactions
    /// </summary>
    /// <typeparam name="T">The mongo collection type</typeparam>
    public class EventFiringMongoDBDriver<T> : MongoDBDriver<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringMongoDBDriver{T}" /> class
        /// </summary>
        /// <param name="connectionString">The mongoDB client connection string</param>
        /// <param name="databaseName">the mongo database name string</param>
        /// <param name="collectionString">the mongo database collection string</param>
        public EventFiringMongoDBDriver(string connectionString, string databaseName, string collectionString)
            : base(connectionString, databaseName, collectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringMongoDBDriver{T}" /> class
        /// </summary>
        /// <param name="collectionString">Name of the collection</param>
        public EventFiringMongoDBDriver(string collectionString)
             : base(collectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringMongoDBDriver{T}" /> class
        /// </summary>
        public EventFiringMongoDBDriver() : base()
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
        /// List all of the items in the collection
        /// </summary>
        /// <returns>List of the items in the collection</returns>
        public override List<T> ListAllCollectionItems()
        {
            try
            {
                this.RaiseEvent("list all collection items");
                return base.ListAllCollectionItems();
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Checks if the collection contains any records
        /// </summary>
        /// <returns>True if the collection is empty, false otherwise</returns>
        public override bool IsCollectionEmpty()
        {
            try
            {
                this.RaiseEvent("Is collection empty");
                return base.IsCollectionEmpty();
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Counts all of the items in the collection
        /// </summary>
        /// <returns>Number of items in the collection</returns>
        public override int CountAllItemsInCollection()
        {
            try
            {
                this.RaiseEvent("Count all items in collection");
                return base.CountAllItemsInCollection();
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
        private void RaiseEvent(string actionType)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Performing {0}.", actionType));
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