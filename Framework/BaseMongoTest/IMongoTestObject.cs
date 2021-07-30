//--------------------------------------------------
// <copyright file="MongoTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds MongoDB context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using MongoDB.Driver;
using System;

namespace Magenic.Maqs.BaseMongoTest
{
    public interface IMongoTestObject<T> : ITestObject
    {
        MongoDBDriver<T> MongoDBDriver { get; }
        MongoDriverManager<T> MongoDBManager { get; }

        void OverrideMongoDBDriver(Func<IMongoCollection<T>> overrideCollectionConnection);
        void OverrideMongoDBDriver(MongoDBDriver<T> driver);
        void OverrideMongoDBDriver(string connectionString, string databaseString, string collectionString);
    }
}