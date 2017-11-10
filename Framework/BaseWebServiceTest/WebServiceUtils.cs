//--------------------------------------------------
// <copyright file="WebServiceUtils.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Web service utilies</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Magenic.MaqsFramework.BaseWebServiceTest
{
    /// <summary>
    /// Web service utilities
    /// </summary>
    public class WebServiceUtils
    {
        /// <summary>
        /// Make http string content
        /// </summary>
        /// <param name="body">The content as a string</param>
        /// <param name="contentEncoding">How to encode the content</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The string content</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="MakeStringContent" lang="C#" />
        /// </example>
        public static StringContent MakeStringContent(string body, Encoding contentEncoding, string mediaType)
        {
            return new StringContent(body, contentEncoding, mediaType);
        }

        /// <summary>
        /// Make http stream content
        /// </summary>
        /// <param name="body">The content as a string</param>
        /// <param name="contentEncoding">How to encode the content</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The stream content</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="MakeStreamContent" lang="C#" />
        /// </example>
        public static StreamContent MakeStreamContent(string body, Encoding contentEncoding, string mediaType)
        {
            Stream stream = StringToStream(body, contentEncoding);
            StreamContent streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return streamContent;
        }

        /// <summary>
        /// Make http stream content
        /// </summary>
        /// <param name="body">The content as a Stream</param>
        /// <param name="contentEncoding">How to encode the content</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The stream content</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="MakeStreamContent" lang="C#" />
        /// </example>
        public static StreamContent MakeStreamContent(Stream body, Encoding contentEncoding, string mediaType)
        {
            StreamContent streamContent = new StreamContent(body);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            return streamContent;
        }

        /// <summary>
        /// Make http stream content
        /// </summary>
        /// <param name="body">The content as a string</param>
        /// <param name="contentEncoding">How to encode the content</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The stream content</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="MakeStreamContent" lang="C#" />
        /// </example>
        public static StreamContent MakeNonStandardStreamContent(string body, Encoding contentEncoding, string mediaType)
        {
            Stream stream = StringToStream(body, contentEncoding);
            return MakeNonStandardStreamContent(stream, mediaType);
        }

        /// <summary>
        /// Make non-standard http stream content
        /// </summary>
        /// <param name="body">The content as a stream</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The stream content</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="MakeStreamContent" lang="C#" />
        /// </example>
        public static StreamContent MakeNonStandardStreamContent(Stream body, string mediaType)
        {
            StreamContent streamContent = new StreamContent(body);
            streamContent.Headers.TryAddWithoutValidation("Content-Type", mediaType);
            return streamContent;
        }

        /// <summary>
        /// Deserialize the XML document body of a HTTP response
        /// </summary>
        /// <typeparam name="T">The deserialized type</typeparam>
        /// <param name="message">The HTTP response</param>
        /// <returns>The XML document body deserialized</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperGets.cs" region="DeserializeXmlDocument" lang="C#" />
        /// </example>
        public static T DeserializeXmlDocument<T>(HttpResponseMessage message)
        {
            string responseBody = message.Content.ReadAsStringAsync().Result;
            var reader = XmlReader.Create(StringToStream(responseBody), new XmlReaderSettings() { ConformanceLevel = ConformanceLevel.Document });
            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }

        /// <summary>
        /// Deserialize the JSON body of a HTTP response
        /// </summary>
        /// <typeparam name="T">The deserialized type</typeparam>
        /// <param name="message">The HTTP response</param>
        /// <returns>The JSON body deserialized</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperGets.cs" region="DeserializeJson" lang="C#" />
        /// </example>
        public static T DeserializeJson<T>(HttpResponseMessage message)
        {
            string responseBody = message.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// Make http string content
        /// </summary>
        /// <typeparam name="T">The body object type</typeparam>
        /// <param name="body">The content as a string</param>
        /// <param name="contentEncoding">How to encode the content</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The string content</returns>
        public static StringContent MakeStringContent<T>(T body, Encoding contentEncoding, string mediaType)
        {
            if (mediaType.ToUpper().Contains("XML"))
            {
                return MakeStringContent(GetXmlObjectAsString(body, contentEncoding), contentEncoding, mediaType);
            }
            else if (mediaType.ToUpper().Contains("JSON"))
            {
                return new StringContent(JsonConvert.SerializeObject(body), contentEncoding, mediaType);
            }
            else
            {
                throw new Exception(StringProcessor.SafeFormatter("Only Xml and Json conversions are currently support, {0} does not appear to be either", mediaType));
            }
        }

        /// <summary>
        /// Make http stream content
        /// </summary>
        /// <typeparam name="T">The body object type</typeparam>
        /// <param name="body">The content as a string</param>
        /// <param name="contentEncoding">How to encode the content</param>
        /// <param name="mediaType">The type of media</param>
        /// <returns>The stream content</returns>
        public static StreamContent MakeStreamContent<T>(T body, Encoding contentEncoding, string mediaType)
        {
            if (mediaType.ToUpper().Contains("XML"))
            {
                return MakeStreamContent(GetXmlObjectAsString(body, contentEncoding), contentEncoding, mediaType);
            }
            else if (mediaType.ToUpper().Contains("JSON"))
            {
                return MakeStreamContent(JsonConvert.SerializeObject(body), contentEncoding, mediaType);
            }
            else
            {
                throw new Exception(StringProcessor.SafeFormatter("Only Xml and Json conversions are currently support, {0} does not appear to be either", mediaType));
            }
        }

        /// <summary>
        /// Get an XML object as a string
        /// </summary>
        /// <typeparam name="T">The type of xml object</typeparam>
        /// <param name="body">The xml object</param>
        /// <param name="contentEncoding">How the string should be encoded</param>
        /// <returns>The xml object as a string</returns>
        public static string GetXmlObjectAsString<T>(T body, Encoding contentEncoding)
        {
            MemoryStream memoryStream = new MemoryStream();
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Encoding = contentEncoding;
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.CloseOutput = false;
            xmlWriterSettings.OmitXmlDeclaration = false;

            XmlWriter xw = XmlWriter.Create(memoryStream, xmlWriterSettings);
            XmlSerializer s = new XmlSerializer(typeof(T));
            s.Serialize(xw, body);

            memoryStream.Position = 0;
            TextReader reader = new StreamReader(memoryStream, contentEncoding);

            return reader.ReadToEnd();
        }

        /// <summary>
        /// Deserialize the body of a HTTP response
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="response">The HTTP response</param>
        /// <param name="supportedFormatters">List of supported media formats</param>
        /// <returns>The deserialized HTTP body</returns>
        public static T DeserializeResponse<T>(HttpResponseMessage response, List<MediaTypeFormatter> supportedFormatters)
        {
            try
            {
                // Save off a new list of formats
                List<MediaTypeFormatter> tempList = new List<MediaTypeFormatter>(supportedFormatters);

                // Check to see if formatters are provided, if not try to provide onw
                if (tempList.Count == 0 && response.Content.Headers.ContentType != null)
                {
                    string mediaType = response.Content.Headers.ContentType.MediaType.ToLower();

                    if (mediaType.Contains("xml"))
                    {
                        tempList.Add(new CustomXmlMediaTypeFormatter(response.Content.Headers.ContentType.MediaType, typeof(T)));
                    }
                    else if (mediaType.Contains("json"))
                    {
                        tempList.Add(new JsonMediaTypeFormatter());
                    }
                }

                return response.Content.ReadAsAsync<T>(tempList).Result;
            }
            catch (Exception e)
            {
                // Create a useful error message
                string body = response.Content.ReadAsStringAsync().Result;

                throw new Exception(
                    StringProcessor.SafeFormatter(
                        "Response could not be deserialized to a {0} object.{1}Body:{1}{2}{1}",
                    typeof(T).FullName,
                    Environment.NewLine,
                    body),
                    e);
            }
        }

        /// <summary>
        /// Turn a string into a steam
        /// </summary>
        /// <param name="stringContent">The string to turn into a stream</param>
        /// <param name="contentEncoding">How to encode the string</param>
        /// <returns>The string as a stream.  The stream position is set to 0</returns>
        private static Stream StringToStream(string stringContent, Encoding contentEncoding = null)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = contentEncoding == null ? new StreamWriter(stream) : new StreamWriter(stream, contentEncoding);
            writer.Write(stringContent);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
