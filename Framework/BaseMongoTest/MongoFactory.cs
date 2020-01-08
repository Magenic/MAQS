//--------------------------------------------------
// <copyright file="MongoFactory.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Mongo database driver factory</summary>
//--------------------------------------------------
using MongoDB.Driver;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Mongo database factory
    /// </summary>
    public static class MongoFactory
    {
        /// <summary>
        /// Get the email client using connection information from the test run configuration 
        /// </summary>
        /// <returns>The email connection</returns>
        public static IMongoCollection<T> GetDefaultCollection<T>()
        {
            return GetCollection<T>(MongoDBConfig.GetConnectionString(), MongoDBConfig.GetDatabaseString(), MongoDBConfig.GetCollectionString());
        }

        /// <summary>
        /// Get the email client using connection information from the test run configuration 
        /// </summary>
        /// <returns>The email connection</returns>
        public static IMongoCollection<T> GetCollection<T>(string connectionString, string databaseString, string collectionString)
        {
            var connection = new MongoClient(new MongoUrl(connectionString));
            var database = connection.GetDatabase(databaseString);
            return database.GetCollection<T>(collectionString);
        }

        /// <summary>
        /// Get the email client using connection information from the test run configuration 
        /// </summary>
        /// <returns>The email connection</returns>
        public static IMongoCollection<T> GetCollection<T>(string connectionString, string databaseString, MongoDatabaseSettings settings, string collectionString)
        {
            var connection = new MongoClient(new MongoUrl(connectionString));
            var database = connection.GetDatabase(databaseString, settings);
            return database.GetCollection<T>(collectionString);
        }
    }
}
