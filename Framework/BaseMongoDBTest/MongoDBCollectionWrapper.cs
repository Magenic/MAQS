using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseMongoDBTest
{
    public class MongoDBCollectionWrapper<T> : Magenic.MaqsFramework.BaseMongoDBTest.BaseMongoDBTest
    {
        /// <summary>
        /// stores the mongo Collection
        /// </summary>
        private IMongoCollection<T> collection;

        public MongoDBCollectionWrapper(IMongoDatabase database, string collectionName){
            this.collection = database.GetCollection<T>(collectionName);
        }

        public MongoDBCollectionWrapper() {
            this.collection = this.GetMongoDBConnection().GetCollection<T>(this.)
        }

        public List<T> ListAllCollectionItems() {
            return this.collection.Find<T>(_ => true).ToList();
        }

        

        

    }
}
