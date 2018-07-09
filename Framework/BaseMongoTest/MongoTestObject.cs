//--------------------------------------------------
// <copyright file="MongoTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds MongoDB context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;

namespace Magenic.Maqs.BaseMongoTest
{
    /// <summary>
    /// Mongo test context data
    /// </summary>
    /// <typeparam name="T">The Mongo collection type</typeparam>
    public class MongoTestObject<T> : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoTestObject{T}" /> class
        /// </summary>
        /// <param name="connectionString">Client connection string</param>
        /// <param name="databaseString">Database connection string</param>
        /// <param name="collectionString">Mongo collection string</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public MongoTestObject(string connectionString, string databaseString, string collectionString, Logger logger, SoftAssert softAssert, string fullyQualifiedTestName) : base(logger, softAssert, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(MongoDriverManager<T>).FullName, new MongoDriverManager<T>(connectionString, databaseString, collectionString, this));
        }

        /// <summary>
        /// Gets the Mongo driver manager
        /// </summary>
        public MongoDriverManager<T> MongoDBManager
        {
            get
            {
                return this.ManagerStore[typeof(MongoDriverManager<T>).FullName] as MongoDriverManager<T>;
            }
        }

        /// <summary>
        /// Gets the Mongo driver
        /// </summary>
        public MongoDBDriver<T> MongoDBDriver
        {
            get
            {
                return this.MongoDBManager.Get();
            }
        }

        /// <summary>
        /// Override the Mongo driver settings
        /// </summary>
        /// <param name="connectionString">Client connection string</param>
        /// <param name="databaseString">Database connection string</param>
        /// <param name="collectionString">Mongo collection string</param>
        public void OverrideMongoDBDriver(string connectionString, string databaseString, string collectionString)
        {
            this.ManagerStore.Remove(typeof(MongoDriverManager<T>).FullName);
            this.ManagerStore.Add(typeof(MongoDriverManager<T>).FullName, new MongoDriverManager<T>(connectionString, databaseString, collectionString, this));
        }

        /// <summary>
        /// Override the Mongo driver settings
        /// </summary>
        /// <param name="driver">New Mongo driver</param>
        public void OverrideMongoDBDriver(MongoDBDriver<T> driver)
        {
            this.MongoDBManager.OverrideDriver(driver);
        }
    }
}
