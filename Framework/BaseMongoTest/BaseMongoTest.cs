//--------------------------------------------------
// <copyright file="BaseMongoTest.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base MongoDB test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using MongoDB.Driver;
using System;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Generic base MongoDB test class
    /// </summary>
    /// <typeparam name="T">The mongo collection type</typeparam>
    public class BaseMongoTest<T> : BaseExtendableTest<IMongoTestObject<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMongoTest{T}"/> class.
        /// Setup the database client for each test class
        /// </summary>
        public BaseMongoTest()
        {

        }

        /// <summary>
        /// Gets or sets the web service driver
        /// </summary>
        public MongoDBDriver<T> MongoDBDriver
        {
            get
            {
                return this.TestObject.MongoDBDriver;
            }

            set
            {
                this.TestObject.OverrideMongoDBDriver(value);
            }
        }

        /// <summary>
        /// Override the Mongo driver - does not lazy load
        /// </summary>
        /// <param name="driver">New Mongo driver</param>
        public void OverrideConnectionDriver(MongoDBDriver<T> driver)
        {
            this.TestObject.OverrideMongoDBDriver(driver);
        }

        /// <summary>
        /// Override the Mongo driver - respects lazy loading
        /// </summary>
        /// <param name="overrideCollectionConnection">The new collection connection</param>
        public void OverrideConnectionDriver(Func<IMongoCollection<T>> overrideCollectionConnection)
        {
            this.TestObject.OverrideMongoDBDriver(overrideCollectionConnection);
        }

        /// <summary>
        /// Override the Mongo driver  - respects lazy loading
        /// </summary>
        /// <param name="connectionString">Client connection string</param>
        /// <param name="databaseString">Database connection string</param>
        /// <param name="collectionString">Mongo collection string</param>
        public void OverrideConnectionDriver(string connectionString, string databaseString, string collectionString)
        {
            this.TestObject.OverrideMongoDBDriver(connectionString, databaseString, collectionString);
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual string GetBaseConnectionString()
        {
            return MongoDBConfig.GetConnectionString();
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual string GetBaseDatabaseString()
        {
            return MongoDBConfig.GetDatabaseString();
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual string GetBaseCollectionString()
        {
            return MongoDBConfig.GetCollectionString();
        }

        /// <summary>
        /// Create a test object
        /// </summary>
        /// <param name="log">Assocatied logger</param>
        /// <returns>The email test object</returns>
        protected override IMongoTestObject<T> CreateSpecificTestObject(ILogger log)
        {
            return new MongoTestObject<T>(this.GetBaseConnectionString(), this.GetBaseDatabaseString(), this.GetBaseCollectionString(), log, this.GetFullyQualifiedTestClassName());
        }
    }
}