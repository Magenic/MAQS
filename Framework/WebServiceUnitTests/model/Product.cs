﻿//--------------------------------------------------
// <copyright file="Product.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Object for string products</summary>
//--------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebServiceTesterUnitTesting.Model
{
    /// <summary>
    /// Class for product
    /// </summary>
    [ExcludeFromCodeCoverage]
    [Serializable]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.datacontract.org/2004/07/AutomationTestSite.Models")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.datacontract.org/2004/07/AutomationTestSite.Models", IsNullable = false)]
    public class Product
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
