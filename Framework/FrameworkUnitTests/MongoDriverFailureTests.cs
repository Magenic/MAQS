//--------------------------------------------------
// <copyright file="MongoDriverFailureTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Mongo database driver failure tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseMongoTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Test the Mongo driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.MongoDB)]
    [ExcludeFromCodeCoverage]
    public class MongoDriverFailureTests : BaseMongoTest<BsonDocument>
    {
        /// <summary>
        /// Test driver call fails correctly
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void ListAllCollectionItemsFailure()
        {
            this.MongoDBDriver.ListAllCollectionItems();
        }

        /// <summary>
        /// Test driver call fails correctly
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void IsCollectionEmptyFailure()
        {
            this.MongoDBDriver.IsCollectionEmpty();
        }

        /// <summary>
        /// Test driver call fails correctly
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void CountAllItemsInCollectionFailure()
        {
            this.MongoDBDriver.CountAllItemsInCollection();
        }


    }
}
