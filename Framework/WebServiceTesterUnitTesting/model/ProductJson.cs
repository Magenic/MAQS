//--------------------------------------------------
// <copyright file="ProductJson.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Product definition for Json</summary>
//--------------------------------------------------
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace WebServiceTesterUnitTesting.Model
{
    /// <summary>
    /// Definition of a product in jason
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ProductJson
    {
        /// <summary>
        /// Gets or sets the product ID
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product category
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the product price
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }
    }
}
