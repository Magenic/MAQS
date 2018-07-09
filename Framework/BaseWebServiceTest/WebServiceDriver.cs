//--------------------------------------------------
// <copyright file="WebServiceDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The basic http client interactions</summary>
//--------------------------------------------------

using Magenic.Maqs.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Wrap basic http client interactions
    /// </summary>
    public class WebServiceDriver
    {
        /// <summary>
        /// The list of supported media types
        /// </summary>
        private List<MediaTypeFormatter> supportedFormatters = new List<MediaTypeFormatter>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriver" /> class
        /// </summary>
        /// <param name="httpClient">An http client</param>
        public WebServiceDriver(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriver" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public WebServiceDriver(Uri baseAddress)
        {
            this.HttpClient = new HttpClient
            {
                BaseAddress = baseAddress,
                Timeout = WebServiceConfig.GetWebServiceTimeout()
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriver" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public WebServiceDriver(string baseAddress)
        {
            this.HttpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress),
                Timeout = WebServiceConfig.GetWebServiceTimeout()
            };
        }

        /// <summary>
        /// Gets or sets the base http client
        /// </summary>
        public HttpClient HttpClient { get; set; }

        /// <summary>
        /// Set the supported media formatters
        /// </summary>
        /// <param name="customFormatter">The supported formatters</param>
        public void SetCustomMediaFormatters(params MediaTypeFormatter[] customFormatter)
        {
            this.supportedFormatters = new List<MediaTypeFormatter>(customFormatter);
        }

        /// <summary>
        /// Set the supported media formatters
        /// </summary>
        /// <param name="customFormatter">The supported formatters</param>
        public void SetCustomMediaFormatters(List<MediaTypeFormatter> customFormatter)
        {
            this.supportedFormatters = customFormatter;
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverGets.cs" region="GetWithType" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverGets.cs" region="GetWithString" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverGets.cs" region="GetWithImage" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPost.cs" region="PostWithType" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPost.cs" region="PostWithString" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPost.cs" region="PostWithoutCreatingContent" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPost.cs" region="PostWithResponse" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPost.cs" region="PostWithResponseContent" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPut.cs" region="PutWithType" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPut.cs" region="PutWithXML" lang="C#" />
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPut.cs" region="PutWithString" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPut.cs" region="PutWithStringContent" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPut.cs" region="PutWithResponse" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPut.cs" region="PutWithResponseJSON" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPatch.cs" region="PatchWithType" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPatch.cs" region="PatchWithString" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPatch.cs" region="PatchWithoutCreatingContent" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPatch.cs" region="PatchWithResponse" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverPatch.cs" region="PatchWithResponseContent" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverDelete.cs" region="DeleteWithType" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverDelete.cs" region="DeleteWithXML" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverDelete.cs" region="DeleteWithStringResponse" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverCustomVerb.cs" region="CustomWithoutContent" lang="C#" />
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
        /// <code source = "../WebServiceTesterUnitTesting/WebServiceWithDriverCustomVerb.cs" region="CustomVerbStatusCode" lang="C#" />
        /// </example>
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return this.CustomContent(requestUri, customType, expectedMediaType, content, expectSuccess).Result;
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
            this.AddAcceptIfNotPresent(responseMediaType);
            HttpResponseMessage response = await this.HttpClient.PostAsync(requestUri, content).ConfigureAwait(false);

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
            this.AddAcceptIfNotPresent(responseMediaType);
            HttpResponseMessage response = await this.HttpClient.PutAsync(requestUri, content).ConfigureAwait(false);

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

            this.AddAcceptIfNotPresent(responseMediaType);
            HttpResponseMessage response = await this.HttpClient.SendAsync(message).ConfigureAwait(false);

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

            this.AddAcceptIfNotPresent(responseMediaType);
            HttpResponseMessage response = await this.HttpClient.SendAsync(message).ConfigureAwait(false);

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
            HttpResponseMessage response = await this.HttpClient.DeleteAsync(requestUri).ConfigureAwait(false);

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
            this.AddAcceptIfNotPresent(mediaType);
            HttpResponseMessage response = await this.HttpClient.GetAsync(requestUri).ConfigureAwait(false);

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
                throw new HttpRequestException("Response was null");
            }

            // Check if it was a success and if not create a user friendly error message
            if (!response.IsSuccessStatusCode)
            {
                string body = response.Content.ReadAsStringAsync().Result;

                throw new HttpRequestException(
                    StringProcessor.SafeFormatter(
                        "Response did not indicate a success.{0}Response code was: {1} ({2}){0}{3}",
                    Environment.NewLine,
                    (int)response.StatusCode,
                    response.StatusCode,
                    body));
            }
        }

        /// <summary>
        /// Add accept media type if it isn't already added
        /// </summary>
        /// <param name="mediaType">Media type to add</param>
        private void AddAcceptIfNotPresent(string mediaType)
        {
            // Make sure a media type was passed in
            if (string.IsNullOrEmpty(mediaType))
            {
                return;
            }

            // Look for the media type
            foreach (MediaTypeWithQualityHeaderValue header in this.HttpClient.DefaultRequestHeaders.Accept)
            {
                if (header.MediaType.Equals(mediaType, StringComparison.CurrentCultureIgnoreCase))
                {
                    // Type was found so return
                    return;
                }
            }

            // Add the type
            this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
        }
    }
}