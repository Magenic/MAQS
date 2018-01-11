//--------------------------------------------------
// <copyright file="HttpClientWrapper.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The basic http client interactions</summary>
//--------------------------------------------------

using Magenic.MaqsFramework.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.MaqsFramework.BaseWebServiceTest
{
    /// <summary>
    /// Wrap basic http client interactions
    /// </summary>
    public class HttpClientWrapper
    {
        /// <summary>
        /// Timeout in seconds
        /// </summary>
        private int timeout = -1;

        /// <summary>
        /// The setup http client function
        /// </summary>
        private Func<Uri, string, HttpClient> setupClientConnection;

        /// <summary>
        /// The cached base http client
        /// </summary>
        private HttpClient cachedBaseClient;

        /// <summary>
        /// The list of supported media types
        /// </summary>
        private List<MediaTypeFormatter> supportedFormatters = new List<MediaTypeFormatter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public HttpClientWrapper(Uri baseAddress)
        {
            this.Initializer(baseAddress, this.supportedFormatters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public HttpClientWrapper(string baseAddress)
        {
            this.Initializer(new Uri(baseAddress), this.supportedFormatters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        /// <param name="customFormatters">Custom list of supported media types</param>
        public HttpClientWrapper(Uri baseAddress, List<MediaTypeFormatter> customFormatters)
        {
            this.Initializer(baseAddress, customFormatters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        /// <param name="customFormatter">Custom supported media types</param>
        public HttpClientWrapper(string baseAddress, MediaTypeFormatter customFormatter)
        {
            List<MediaTypeFormatter> formatters = new List<MediaTypeFormatter>();
            formatters.Add(customFormatter);
            this.Initializer(new Uri(baseAddress), formatters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        /// <param name="customFormatter">The supported media type formatter</param>
        public HttpClientWrapper(Uri baseAddress, MediaTypeFormatter customFormatter)
        {
            List<MediaTypeFormatter> formatters = new List<MediaTypeFormatter>();
            formatters.Add(customFormatter);
            this.Initializer(baseAddress, formatters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpClientWrapper" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        /// <param name="customFormatters">The list of supported media type formatter</param>
        public HttpClientWrapper(string baseAddress, List<MediaTypeFormatter> customFormatters)
        {
            this.Initializer(new Uri(baseAddress), this.supportedFormatters);
        }

        /// <summary>
        /// Gets or sets the base Http client
        /// </summary>
        public HttpClient BaseHttpClient
        {
            get
            {
                if (this.cachedBaseClient == null)
                {
                    this.cachedBaseClient = new HttpClient();
                    this.SetTimeout(ref this.cachedBaseClient);
                }

                return this.cachedBaseClient;
            }

            set
            {
                this.cachedBaseClient = value;
            }
        }

        /// <summary>
        /// Gets the base uri
        /// </summary>
        public Uri BaseUriAddress { get; private set; }

        /// <summary>
        /// Set the supported media formatters
        /// </summary>
        /// <param name="customFormatter">The supported formatters</param>
        public void SetMediaFormatters(params MediaTypeFormatter[] customFormatter)
        {
            this.supportedFormatters = new List<MediaTypeFormatter>(customFormatter);
        }

        /// <summary>
        /// Override the http client setup
        /// </summary>
        /// <param name="setupClientConnection">The override function</param>
        public void OverrideSetupClientConnection(Func<Uri, string, HttpClient> setupClientConnection)
        {
            this.setupClientConnection = setupClientConnection;
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperGets.cs" region="GetWithType" lang="C#" />
        /// </example>
        public T Get<T>(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.GetWithResponse(requestUri, expectedMediaType, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response content as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperGets.cs" region="GetWithString" lang="C#" />
        /// </example>
        public string Get(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.GetWithResponse(requestUri, expectedMediaType, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperGets.cs" region="GetWithImage" lang="C#" />
        /// </example>
        public HttpResponseMessage GetWithResponse(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return this.GetContent(requestUri, expectedMediaType, expectSuccess).Result;
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPost.cs" region="PostWithType" lang="C#" />
        /// </example>
        public T Post<T>(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PostWithResponse(requestUri, expectedMediaType, content, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPost.cs" region="PostWithString" lang="C#" />
        /// </example>
        public string Post(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PostWithResponse(requestUri, expectedMediaType, content, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="contentEncoding">How to encode the post content</param>
        /// <param name="postMediaType">The type of the media being posted</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPost.cs" region="PostWithoutCreatingContent" lang="C#" />
        /// </example>
        public string Post(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PostWithResponse(requestUri, expectedMediaType, content, contentEncoding, postMediaType, contentAsString, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="contentEncoding">How to encode the post content</param>
        /// <param name="postMediaType">The type of the media being posted</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPost.cs" region="PostWithResponse" lang="C#" />
        /// </example>
        public HttpResponseMessage PostWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return this.PostWithResponse(requestUri, expectedMediaType, httpContent, expectSuccess);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPost.cs" region="PostWithResponseContent" lang="C#" />
        /// </example>
        public HttpResponseMessage PostWithResponse(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return this.PostContent(requestUri, expectedMediaType, content, expectSuccess).Result;
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="PutWithType" lang="C#" />
        /// </example>
        public T Put<T>(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PutWithResponse(requestUri, expectedMediaType, content, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="PutWithXML" lang="C#" />
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="PutWithString" lang="C#" />
        /// </example>
        public string Put(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PutWithResponse(requestUri, expectedMediaType, content, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="PutWithStringContent" lang="C#" />
        /// </example>
        public string Put(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PutWithResponse(requestUri, expectedMediaType, content, contentEncoding, postMediaType, contentAsString, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="PutWithResponse" lang="C#" />
        /// </example>
        public HttpResponseMessage PutWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return this.PutWithResponse(requestUri, expectedMediaType, httpContent, expectSuccess);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPut.cs" region="PutWithResponseJSON" lang="C#" />
        /// </example>
        public HttpResponseMessage PutWithResponse(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return this.PutContent(requestUri, expectedMediaType, content, expectSuccess).Result;
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPatch.cs" region="PatchWithType" lang="C#" />
        /// </example>
        public T Patch<T>(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PatchWithResponse(requestUri, expectedMediaType, content, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPatch.cs" region="PatchWithString" lang="C#" />
        /// </example>
        public string Patch(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PatchWithResponse(requestUri, expectedMediaType, content, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPatch.cs" region="PatchWithoutCreatingContent" lang="C#" />
        /// </example>
        public string Patch(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.PatchWithResponse(requestUri, expectedMediaType, content, contentEncoding, postMediaType, contentAsString, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPatch.cs" region="PatchWithResponse" lang="C#" />
        /// </example>
        public HttpResponseMessage PatchWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return this.PatchWithResponse(requestUri, expectedMediaType, httpContent, expectSuccess);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperPatch.cs" region="PatchWithResponseContent" lang="C#" />
        /// </example>
        public HttpResponseMessage PatchWithResponse(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return this.PatchContent(requestUri, expectedMediaType, content, expectSuccess).Result;
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response by deserialized as the <typeparamref name="T"/></returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperDelete.cs" region="DeleteWithType" lang="C#" />
        /// </example>
        public T Delete<T>(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.DeleteWithResponse(requestUri, expectedMediaType, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response as a string</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperDelete.cs" region="DeleteWithXML" lang="C#" />
        /// </example> 
        public string Delete(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.DeleteWithResponse(requestUri, expectedMediaType, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperDelete.cs" region="DeleteWithStringResponse" lang="C#" />
        /// </example> 
        public HttpResponseMessage DeleteWithResponse(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return this.DeleteContent(requestUri, expectedMediaType, expectSuccess).Result;
        }

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="customType">The custom HTTP verb</param>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">Type of media being requested</param>
        /// <param name="content">Content of the message</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>HTTP response message</returns>
        public T Custom<T>(string customType, string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.CustomWithResponse(customType, requestUri, expectedMediaType, content, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <param name="customType">Custom HTTP verb</param>
        /// <param name="requestUri">The request URI </param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">Content of the message</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP response message</returns>
        public string Custom(string customType, string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.CustomWithResponse(customType, requestUri, expectedMediaType, content, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <param name="customType">Custom HTTP Verb</param>
        /// <param name="requestUri">The request URI</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="content">Content of the message</param>
        /// <param name="contentEncoding">How content was encoded</param>
        /// <param name="postMediaType">Media type</param>
        /// <param name="contentAsString">The message content as a string</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP Response message</returns>
        public string Custom(string customType, string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.CustomWithResponse(customType, requestUri, expectedMediaType, content, contentEncoding, postMediaType, contentAsString, expectSuccess);
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <param name="customType">the Custom HTTP Verb</param>
        /// <param name="requestUri">The Request URI</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="content">The Content of the message</param>
        /// <param name="contentEncoding">How content was encoded</param>
        /// <param name="postMediaType">Media type</param>
        /// <param name="contentAsString">The content as a a string</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperCustomVerb.cs" region="CustomWithoutContent" lang="C#" />
        /// </example>
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return this.CustomWithResponse(customType, requestUri, expectedMediaType, httpContent, expectSuccess);
        }   

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <param name="customType">The custom HTTP verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="content">The content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP response message</returns>
        /// <example>
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithWrapperCustomVerb.cs" region="CustomVerbStatusCode" lang="C#" />
        /// </example>
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return this.CustomContent(requestUri, customType, expectedMediaType, content, expectSuccess).Result;
        }

        /// <summary> 
        /// Default client setup - Override this function to include authentication 
        /// </summary>
        /// <param name="baseAddress">The base uri</param>
        /// <param name="mediaType">The type of media to accept</param>
        /// <returns>The http client</returns>
        protected virtual HttpClient SetupClientConnection(Uri baseAddress, string mediaType)
        {
            HttpClient client = this.BaseHttpClient;
            client.BaseAddress = baseAddress;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return this.BaseHttpClient;
        }

        /// <summary>
        /// Do a web service post for the given uri, content and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The post body</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected async virtual Task<HttpResponseMessage> PostContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpClient client = this.setupClientConnection(this.BaseUriAddress, responseMediaType);

            HttpResponseMessage response = await client.PostAsync(requestUri, content).ConfigureAwait(false);

            // Should we check for success
            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Do a web service put for the given uri, content and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The put body</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected async virtual Task<HttpResponseMessage> PutContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpClient client = this.setupClientConnection(this.BaseUriAddress, responseMediaType);

            HttpResponseMessage response = await client.PutAsync(requestUri, content).ConfigureAwait(false);

            // Should we check for success
            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Do a web service put for the given uri, content and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The put body</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected async virtual Task<HttpResponseMessage> PatchContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpMethod method = new HttpMethod("PATCH");

            HttpRequestMessage message = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            HttpClient client = this.setupClientConnection(this.BaseUriAddress, responseMediaType);

            HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);

            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Do a web service call with the given custom verb, content, and return type
        /// </summary>
        /// <param name="requestUri">The request URI</param>
        /// <param name="customVerb">The custom HTTP request verb to be used</param>
        /// <param name="responseMediaType">The expected response type</param>
        /// <param name="content">The content of the message</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP response message</returns>
        protected async virtual Task<HttpResponseMessage> CustomContent(string requestUri, string customVerb, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            HttpMethod method = new HttpMethod(customVerb);

            HttpRequestMessage message = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };

            HttpClient client = this.setupClientConnection(this.BaseUriAddress, responseMediaType);

            HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);

            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;            
        }

        /// <summary>
        /// Do a web service delete for the given uri
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="returnMediaType">The expected response media type</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected async virtual Task<HttpResponseMessage> DeleteContent(string requestUri, string returnMediaType, bool expectSuccess = true)
        {
            HttpClient client = this.setupClientConnection(this.BaseUriAddress, returnMediaType);

            HttpResponseMessage response = await client.DeleteAsync(requestUri).ConfigureAwait(false);

            // Should we check for success
            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Do a web service get for the given uri and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="mediaType">What type of media are we expecting</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected async virtual Task<HttpResponseMessage> GetContent(string requestUri, string mediaType, bool expectSuccess = true)
        {
            HttpClient client = this.setupClientConnection(this.BaseUriAddress, mediaType);

            HttpResponseMessage response = await client.GetAsync(requestUri).ConfigureAwait(false);

            // Should we check for success
            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Create http content
        /// </summary>
        /// <param name="content">The content as a string</param>
        /// <param name="contentEncoding">How to encode the post content</param>
        /// <param name="postMediaType">The type of the media being posted</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The content as StringContent or SteamContent</returns>
        private static HttpContent CreateContent(string content, Encoding contentEncoding, string postMediaType, bool contentAsString)
        {
            HttpContent httpContent;

            if (contentAsString)
            {
                httpContent = WebServiceUtils.MakeStringContent(content, contentEncoding, postMediaType);
            }
            else
            {
                httpContent = WebServiceUtils.MakeStreamContent(content, contentEncoding, postMediaType);
            }

            return httpContent;
        }

        /// <summary>
        /// Ensure the HTTP response was successful, if not throw a user friendly error message
        /// </summary>
        /// <param name="response">The HTTP response</param>
        private static void EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            // Make sure a response was returned
            if (response == null)
            {
                throw new Exception("Response was null");
            }

            // Check if it was a success and if not create a user friendly error message
            if (!response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;

                throw new Exception(
                    StringProcessor.SafeFormatter(
                        "Response did not indicate a success.{0}Response code was: {1} ({2}){0}{3}",
                    Environment.NewLine,
                    (int)response.StatusCode,
                    response.StatusCode,
                    body));
            }
        }

        /// <summary>
        /// Class initialization
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        /// <param name="customFormatters">Custom list of supported media types</param>
        private void Initializer(Uri baseAddress, List<MediaTypeFormatter> customFormatters)
        {
            this.BaseUriAddress = baseAddress;
            this.supportedFormatters = customFormatters;
            this.setupClientConnection = this.SetupClientConnection;
            this.timeout = WebServiceConfig.GetWebServiceTimeout();
        }

        /// <summary>
        /// Set a timeout 
        /// </summary>
        /// <param name="client">The http client</param>
        private void SetTimeout(ref HttpClient client)
        {
            // Only set a timeout if one is provided and longer greater than 0
            if (this.timeout > 0)
            {
                TimeSpan timeout = TimeSpan.FromSeconds(this.timeout);

                if (timeout != client.Timeout)
                {
                    client.Timeout = TimeSpan.FromSeconds(this.timeout);
                }
            }
        }
    }
}