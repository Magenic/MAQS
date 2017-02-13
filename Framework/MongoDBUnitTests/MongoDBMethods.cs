//--------------------------------------------------
// <copyright file="MongoDBMethods.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Mongo database methods used for testing
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoDBMethods
    {
        /// <summary>
        /// stores the mongo Collection
        /// </summary>
        private IMongoCollection<MongoDBElements> collection;

        /// <summary>
        /// Test BSonDocument type of collection
        /// </summary>
        private IMongoCollection<BsonDocument> collection2;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDBMethods"/> class 
        /// </summary>
        /// <param name="database">connection string for mongo database client</param>
        /// <param name="collectionName">name of the collection to access</param>
        public MongoDBMethods(IMongoDatabase database, string collectionName)
        {
            this.collection = database.GetCollection<MongoDBElements>(collectionName);
            this.collection2 = database.GetCollection<BsonDocument>(collectionName);
            ////this.collection = mongoConnection.database.GetCollection<MongoDBElements>(collectionName);
        }

        /// <summary>
        /// Gets the loginID from searched query
        /// </summary>
        /// <param name="currentLoginID">loginID that you are looking for</param>
        /// <returns>loginID is returned in string form</returns>
        public string GetLoginID(string currentLoginID)
        {
            var filter = Builders<MongoDBElements>.Filter.Eq(c => c.LoginID, currentLoginID);
            Assert.IsNotNull(
                this.collection.Find(filter).ToList().First().LoginID,
                "LoginID not found");
            return this.collection.Find(filter).ToList().First().LoginID;
        }

        /// <summary>
        /// Edits the loginID from searched query
        /// </summary>
        /// <param name="currentLoginID">loginID that you are looking for</param>
        /// <param name="newLoginID">loginID that current loginID is changed to</param>
        public void EditLoginID(string currentLoginID, string newLoginID)
        {
            var filter = Builders<MongoDBElements>.Filter.Where(c => c.LoginID.Equals(currentLoginID));
            Assert.IsNotNull(
                this.collection.Find(filter).ToList().FirstOrDefault(),
                "LoginID not found");
            var update = Builders<MongoDBElements>.Update.Set(c => c.LoginID, newLoginID);
            this.collection.UpdateOne(filter, update);
        }

        /// <summary>
        /// Counts the items in the specific collection
        /// </summary>
        /// <param name="currentLoginID">used to search for the </param>
        /// <returns>the number of items in the collection</returns>
        public int CountItemsInCollection(string currentLoginID)
        {
            var count = this.collection.Count(Builders<MongoDBElements>.Filter.Eq(c => c.LoginID, "test1"));
            return int.Parse(count.ToString());
        }

        /// <summary>
        /// determines if the collection is empty
        /// </summary>
        /// <returns>boolean on if collection is empty, true = not empty, false = empty</returns>
        public bool IsCollectionEmpty()
        {
            return this.collection.Database.ListCollections().Any();
        }

        /// <summary>
        /// gets a list of the items in the collection
        /// </summary>
        /// <returns>list of everything in the collection</returns>
        public List<string> ListItemsInCollection()
        {
            List<string> collectionList = new List<string>();

            foreach (var item in this.collection.Database.ListCollectionsAsync().Result.ToList())
            {
                collectionList.Add(item.ToString());
            }

            return collectionList;
        }

        /// <summary>
        /// Searches for specific item in mongoDB
        /// </summary>
        /// <param name="searchCategory"> The index to search on</param>
        /// <param name="searchText"> The value to search for</param>
        /// <returns>The list of values found by the query</returns>
        public List<MongoDBElements> QueryMongo(string searchCategory, string searchText)
        {
            var filter = Builders<MongoDBElements>.Filter.Eq(searchCategory, searchText);
            return this.collection.Find(filter).ToList().ToList();
        }

        /// <summary>
        /// Searches for specific item in mongoDB
        /// </summary>
        /// <param name="searchCategory"> The index to search on</param>
        /// <param name="searchText"> The value to search for</param>
        /// <returns>The list of values found by the query</returns>
        public List<MongoDBElements> QueryMongo(string searchCategory, bool searchText)
        {
            var filter = Builders<MongoDBElements>.Filter.Eq(searchCategory, searchText);
            return this.collection.Find(filter).ToList();
        }

        /// <summary>
        /// Searches for specific item in mongoDB
        /// </summary>
        /// <param name="searchCategory"> The index to search on</param>
        /// <param name="searchText"> The value to search for</param>
        /// <returns>The list of values found by the query</returns>
        public List<MongoDBElements> QueryMongo(string searchCategory, int searchText)
        {
            var filter = Builders<MongoDBElements>.Filter.Eq(searchCategory, searchText);
            return this.collection.Find(filter).ToList();
        }

        /// <summary>
        /// Searches for specific item in mongoDB
        /// </summary>
        /// <param name="searchCategory"> The index to search on</param>
        /// <param name="searchText"> The value to search for</param>
        /// <returns>The list of values found by the query</returns>
        public List<BsonDocument> QueryMongo2(string searchCategory, bool searchText)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(searchCategory, searchText);
            return this.collection2.Find(filter).ToList();
        }

        /// <summary>
        /// Searches for specific item in mongoDB
        /// </summary>
        /// <param name="searchCategory"> The index to search on</param>
        /// <param name="searchText"> The value to search for</param>
        /// <returns>The list of values found by the query</returns>
        public List<BsonDocument> QueryMongo2(string searchCategory, string searchText)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(searchCategory, searchText);
            return this.collection2.Find(filter).ToList();
        }

        /// <summary>
        /// Searches for specific item in mongoDB
        /// </summary>
        /// <param name="searchCategory"> The index to search on</param>
        /// <param name="searchText"> The value to search for</param>
        /// <returns>The list of values found by the query</returns>
        public List<BsonDocument> QueryMongo2(string searchCategory, int searchText)
        {
            var filter = Builders<BsonDocument>.Filter.Eq(searchCategory, searchText);
            return this.collection2.Find(filter).ToList();
        }

        /// <summary>
        /// Test function
        /// </summary>
        public void MongoTest()
        {
            /*List<BsonDocument> docs = new List<BsonDocument>();
            var filter = new BsonDocument();
            var count = 0;
            var cursor = this.collection2.Find(filter);
            
            while (cursor.)
            {
                var batch = cursor.Current;
                foreach (var document in batch)
                {
                    // process document
                    count++;
                    docs.Add(document);
                }
            }
            
            return docs;*/
            var documents = this.collection2.Find(_ => true).ToList();
            var document1 = documents[0]["lid"];
            Debugger.Break();
        }
    }
}