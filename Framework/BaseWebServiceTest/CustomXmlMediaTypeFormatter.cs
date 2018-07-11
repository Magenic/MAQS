//--------------------------------------------------
// <copyright file="CustomXmlMediaTypeFormatter.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Custom XML formatter</summary>
//--------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Xml.Serialization;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Create a custom xml media type formatter
    /// </summary>
    public class CustomXmlMediaTypeFormatter : XmlMediaTypeFormatter
    {
        /// <summary>
        /// A list of object type and serializer mappings
        /// </summary>
        private readonly Dictionary<Type, XmlSerializer> serializers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomXmlMediaTypeFormatter" /> class
        /// </summary>
        /// <param name="xmlMediaType">The media type</param>
        /// <param name="types">Class types to pull serializer from</param>
        public CustomXmlMediaTypeFormatter(string xmlMediaType, params Type[] types)
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(xmlMediaType));
            this.serializers = new Dictionary<Type, XmlSerializer>();

            // Loop over the list of XML services
            foreach (Type type in types)
            {
                // Only include serializable types
                if (type.IsSerializable)
                {
                    this.serializers.Add(type, new XmlSerializer(type));
                }
            }
        }

        /// <summary>
        /// Get the serializer for the given type
        /// </summary>
        /// <param name="type">The type to be serialized</param>
        /// <param name="value">value - not used</param>
        /// <param name="content">content - not used</param>
        /// <returns>The serializer</returns>
        protected override object GetSerializer(Type type, object value, HttpContent content)
        {
            return this.serializers[type];
        }

        /// <summary>
        /// Get the deserializer for the given type
        /// </summary>
        /// <param name="type">The type to be deserialized</param>
        /// <param name="content">content - not used</param>
        /// <returns>The deserializer</returns>
        protected override object GetDeserializer(Type type, HttpContent content)
        {
            return this.serializers[type];
        }
    }
}
