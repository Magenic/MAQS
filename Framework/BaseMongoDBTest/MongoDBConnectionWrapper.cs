//--------------------------------------------------
// <copyright file="MongoDBConnectionWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>The basic MongoDB database interactions</summary>
//--------------------------------------------------
using MongoDB.Driver;
using System;

namespace Magenic.MaqsFramework.BaseMongoDBTest
{
    /// <summary>
    /// Wraps the basic database interactions
    /// </summary>
    public class MongoDBConnectionWrapper
    {
        /// <summary>
        /// the mongoClient through the mongoURL is created here
        /// </summary>
        private IMongoClient client;
        
        /// <summary>
        /// database to get is created here
        /// </summary>
        private IMongoDatabase database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBConnectionWrapper" /> class
        /// </summary>
        /// <param name="connectionString">The base server connection string</param>
        /// <param name="databaseName">the mongo database name string</param>
        public MongoDBConnectionWrapper(string connectionString, string databaseName)
        {
            this.client = this.SetupMongoDBClient(connectionString);
            this.database = this.SetUpMongoDBDatabase(databaseName);
        }

        /// <summary>
        /// Returns the MongoDB database 
        /// </summary>
        /// <returns>accessed mongoDB database from the client</returns>
        public IMongoDatabase ReturnMongoDBDatabase()
        {
            return this.database;
        }

        /// <summary> 
        /// Default client connection setup - Override this function to create your own connection
        /// </summary>
        /// <param name="connectionString">The mongo database client name string</param>
        /// <returns>The mongo database client</returns>
        protected virtual IMongoClient SetupMongoDBClient(string connectionString)
        { 
            MongoUrl mongoURL = new MongoUrl(connectionString);
            return new MongoClient(mongoURL);
        }

        /// <summary>
        /// returns a database from the mongo client
        /// </summary>
        /// <param name="databaseName">the mongo database name string</param>
        /// <returns>The mongo database client</returns>
        protected virtual IMongoDatabase SetUpMongoDBDatabase(string databaseName)
        {
            return this.client.GetDatabase(databaseName);
        }
    }
}