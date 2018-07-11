//--------------------------------------------------
// <copyright file="BaseMongoTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base MongoDB test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;

namespace Magenic.Maqs.BaseMongoTest
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
        /// Override the Mongo driver
        /// </summary>
        /// <param name="driver">New Mongo driver</param>
        public void OverrideConnectionDriver(MongoDBDriver<T> driver)
        {
            this.TestObject.OverrideMongoDBDriver(driver);
        }

        /// <summary>
        /// Override the Mongo driver
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
        /// Create a MongoDB test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new MongoTestObject<T>(this.GetBaseConnectionString(), this.GetBaseDatabaseString(), this.GetBaseCollectionString(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
        }
    }
}