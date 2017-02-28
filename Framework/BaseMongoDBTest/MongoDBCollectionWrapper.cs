// <copyright file="MongoDBCollectionWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the wrapper for the mongo collection object</summary>
//--------------------------------------------------
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Magenic.MaqsFramework.BaseMongoDBTest
{
    /// <summary>
    /// Class to wrap the IMongoCollection and related helper functions
    /// </summary>
    /// <typeparam name="T">Generic T-Document</typeparam>
    public class MongoDBCollectionWrapper<T>
    {
        /// <summary>
        /// The mongo client object
        /// </summary>
        private IMongoClient client;

        /// <summary>
        /// The mongo database object
        /// </summary>
        private IMongoDatabase database;

        /// <summary>
        /// The mongo collection object
        /// </summary>
        private IMongoCollection<T> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBCollectionWrapper{T}" /> class
        /// </summary>
        /// <param name="connectionString">Server address</param>
        /// <param name="databaseString">Name of the database</param>
        /// <param name="collectionString">Name of the collection</param>
        public MongoDBCollectionWrapper(string connectionString, string databaseString, string collectionString)
        {
            this.client = new MongoClient(new MongoUrl(connectionString));
            this.database = this.client.GetDatabase(databaseString);
            this.collection = this.database.GetCollection<T>(collectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBCollectionWrapper{T}" /> class
        /// </summary>
        /// <param name="collectionString">Name of the collection</param>
        public MongoDBCollectionWrapper(string collectionString)
        {
            this.client = new MongoClient(new MongoUrl(MongoDBConfig.GetConnectionString()));
            this.database = this.client.GetDatabase(MongoDBConfig.GetDatabaseString());
            this.collection = this.database.GetCollection<T>(collectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBCollectionWrapper{T}" /> class
        /// </summary>
        public MongoDBCollectionWrapper()
        {
            this.client = new MongoClient(new MongoUrl(MongoDBConfig.GetConnectionString()));
            this.database = this.client.GetDatabase(MongoDBConfig.GetDatabaseString());
            this.collection = this.database.GetCollection<T>(MongoDBConfig.GetCollectionString());
        }

        /// <summary>
        /// Gets or sets the client object
        /// </summary>
        public IMongoClient Client
        {
            get
            {
                return this.client;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the database object
        /// </summary>
        public IMongoDatabase Database
        {
            get
            {
                return this.database;
            }

            set
            {
            }
        }

        /// <summary>
        ///  Gets or sets the collection object
        /// </summary>
        public IMongoCollection<T> Collection
        {
            get
            {
                return this.collection;
            }

            set
            {
            }
        }

        /// <summary>
        /// List all of the items in the collection
        /// </summary>
        /// <returns>List of the items in the collection</returns>
        public virtual List<T> ListAllCollectionItems()
        {
            return this.collection.Find<T>(_ => true).ToList();
        }

        /// <summary>
        /// Checks if the collection contains any records
        /// </summary>
        /// <returns>True if the collection is empty, false otherwise</returns>
        public virtual bool IsCollectionEmpty()
        {
            return !this.collection.Find<T>(_ => true).Any();
        }

        /// <summary>
        /// Counts all of the items in the collection
        /// </summary>
        /// <returns>Number of items in the collection</returns>
        public virtual int CountAllItemsInCollection()
        {
            return int.Parse(this.collection.Count(_ => true).ToString());
        }
    }
}
