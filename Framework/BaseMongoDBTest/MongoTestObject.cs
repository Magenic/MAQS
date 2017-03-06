//--------------------------------------------------
// <copyright file="MongoTestObject.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Holds MongoDB context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;

namespace Magenic.MaqsFramework.BaseMongoDBTest
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
        /// <param name="mongoCollection">The test's mongo collection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="perfTimerCollection">The test's performance timer collection</param>
        public MongoTestObject(MongoDBCollectionWrapper<T> mongoCollection, Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection) : base(logger, softAssert, perfTimerCollection)
        {
            this.MongoDBCollectionWrapper = mongoCollection;
        }

        /// <summary>
        /// Gets The Mongo Collection wrapper that is held in the test object
        /// </summary>
        public MongoDBCollectionWrapper<T> MongoDBCollectionWrapper { get; private set; }
    }
}
