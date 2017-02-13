//--------------------------------------------------
// <copyright file="MongoDBElements.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test database configuration test</summary>
//--------------------------------------------------
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Stores the elements used in MongoDB
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoDBElements
    {
        /// <summary>
        /// Gets or sets specific string LoginID used in mongoDB
        /// </summary>
        [BsonElement("lid")]
        public string LoginID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is some boolean value in the model
        /// </summary>
        [BsonElement("isChanged")]
        public bool IsChanged { get; set; }

        /// <summary>
        /// Gets or sets the order value in the model
        /// </summary>
        [BsonElement("order")]
        public int Order { get; set; }

        ////[BsonElement("")]
        ////public ObjectId constant { get; set; }

        /// <summary>
        /// Gets or sets the other unused elements in MongoDB
        /// </summary>
        [BsonExtraElements]
        public BsonDocument ExtraElements { get; set; }
    }
}