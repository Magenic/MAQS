using System;

namespace WebServiceModel
{
    /// <summary>
    /// Class for product
    /// </summary>
    [Serializable]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/AutomationTestSite.Models")]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Product", Namespace = "http://schemas.datacontract.org/2004/07/AutomationTestSite.Models", IsNullable = false)]
    public class ProductXml
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the product name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the product category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the product price
        /// </summary>
        public double Price { get; set; }
    }
}
