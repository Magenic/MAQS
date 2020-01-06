// <copyright file="MongoDBDriver.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the driver for the mongo collection object</summary>
//--------------------------------------------------
using MongoDB.Driver;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Class to wrap the IMongoCollection and related helper functions
    /// </summary>
    /// <typeparam name="T">Generic T-Document</typeparam>
    public class MongoDBDriver<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBDriver{T}" /> class
        /// </summary>
        /// <param name="collection">The collection object</param>
        public MongoDBDriver(IMongoCollection<T> collection)
        {
            this.Collection = collection;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBDriver{T}" /> class
        /// </summary>
        /// <param name="connectionString">Server address</param>
        /// <param name="databaseString">Name of the database</param>
        /// <param name="collectionString">Name of the collection</param>
        public MongoDBDriver(string connectionString, string databaseString, string collectionString)
        {
            this.Collection = MongoFactory.GetCollection<T>(connectionString, databaseString, collectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBDriver{T}" /> class
        /// </summary>
        /// <param name="collectionString">Name of the collection</param>
        public MongoDBDriver(string collectionString)
        {
            this.Collection = MongoFactory.GetCollection<T>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), collectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBDriver{T}" /> class
        /// </summary>
        public MongoDBDriver()
        {
            this.Collection = MongoFactory.GetDefaultCollection<T>();
        }

        /// <summary>
        /// Gets the client object
        /// </summary>
        public IMongoClient Client
        {
            get
            {
                return this.Database.Client;
            }
        }

        /// <summary>
        /// Gets the database object
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                return this.Collection.Database;
            }
        }

        /// <summary>
        ///  Gets the collection object
        /// </summary>
        public IMongoCollection<T> Collection { get; private set; }

        /// <summary>
        /// List all of the items in the collection
        /// </summary>
        /// <returns>List of the items in the collection</returns>
        public virtual List<T> ListAllCollectionItems()
        {
            return this.Collection.Find<T>(_ => true).ToList();
        }

        /// <summary>
        /// Checks if the collection contains any records
        /// </summary>
        /// <returns>True if the collection is empty, false otherwise</returns>
        public virtual bool IsCollectionEmpty()
        {
            return !this.Collection.Find<T>(_ => true).Any();
        }

        /// <summary>
        /// Counts all of the items in the collection
        /// </summary>
        /// <returns>Number of items in the collection</returns>
        public virtual int CountAllItemsInCollection()
        {
            return int.Parse(this.Collection.CountDocuments(_ => true).ToString());
        }
    }
}
