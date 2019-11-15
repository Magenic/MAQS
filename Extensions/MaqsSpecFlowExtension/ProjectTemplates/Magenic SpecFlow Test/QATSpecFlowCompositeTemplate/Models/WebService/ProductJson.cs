using Newtonsoft.Json;

namespace Models.WebService
{
    /// <summary>
    /// Definition of a product in json
    /// </summary>
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
