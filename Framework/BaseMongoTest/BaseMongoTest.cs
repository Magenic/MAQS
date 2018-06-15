//--------------------------------------------------
// <copyright file="BaseMongoTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base MongoDB test class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;

namespace Magenic.MaqsFramework.BaseMongoTest
{
    /// <summary>
    /// Generic base MongoDB test class
    /// </summary>
    /// <typeparam name="T">The mongo collection type</typeparam>
    public class BaseMongoTest<T> : BaseExtendableTest<MongoTestObject<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMongoTest{T}"/> class.
        /// Setup the database client for each test class
        /// </summary>
        public BaseMongoTest()
        {
        }

        /// <summary>
        /// Gets or sets the web service wrapper
        /// </summary>
        public MongoDBDriver<T> MongoDBWrapper
        {
            get
            {
                return this.TestObject.MongoDBWrapper;
            }

            set
            {
                this.TestObject.OverrideMongoDBWrapper(value);
            }
        }

        /// <summary>
        /// Override the Mongo wrapper
        /// </summary>
        /// <param name="wrapper">New Mongo wrapper</param>
        public void OverrideConnectionWrapper(MongoDBDriver<T> wrapper)
        {
            this.TestObject.OverrideMongoDBWrapper(wrapper);
        }

        /// <summary>
        /// Override the Mongo wrapper
        /// </summary>
        /// <param name="connectionString">Client connection string</param>
        /// <param name="databaseString">Database connection string</param>
        /// <param name="collectionString">Mongo collection string</param>
        public void OverrideConnectionWrapper(string connectionString, string databaseString, string collectionString)
        {
            this.TestObject.OverrideMongoDBWrapper(connectionString, databaseString, collectionString);
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
        /// Create a MongoDB test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new MongoTestObject<T>(this.GetBaseConnectionString(), this.GetBaseDatabaseString(), this.GetBaseCollectionString(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
        }
    }
}