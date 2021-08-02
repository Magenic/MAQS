//--------------------------------------------------
// <copyright file="IMongoTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds Mongo test object interface</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using MongoDB.Driver;
using System;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Mongo test object interface
    /// </summary>
    public interface IMongoTestObject<T> : ITestObject
    {
        /// <summary>
        /// Gets the Mongo driver
        /// </summary>
        MongoDBDriver<T> MongoDBDriver { get; }

        /// <summary>
        /// Gets the Mongo driver manager
        /// </summary>
        MongoDriverManager<T> MongoDBManager { get; }

        /// <summary>
        /// Override the Mongo driver a collection function
        /// </summary>
        /// <param name="overrideCollectionConnection">The collection function</param>
        void OverrideMongoDBDriver(Func<IMongoCollection<T>> overrideCollectionConnection);

        /// <summary>
        /// Override the Mongo driver settings
        /// </summary>
        /// <param name="driver">New Mongo driver</param>
        void OverrideMongoDBDriver(MongoDBDriver<T> driver);

        /// <summary>
        /// Override the Mongo driver settings
        /// </summary>
        /// <param name="connectionString">Client connection string</param>
        /// <param name="databaseString">Database connection string</param>
        /// <param name="collectionString">Mongo collection string</param>
        void OverrideMongoDBDriver(string connectionString, string databaseString, string collectionString);
    }
}