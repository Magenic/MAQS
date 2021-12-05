//--------------------------------------------------
// <copyright file="WebServiceDriver.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>The basic http client interactions</summary>
//--------------------------------------------------

using Magenic.Maqs.Utilities.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
            this.HttpClient = HttpClientFactory.GetClient(baseAddress, WebServiceConfig.GetWebServiceTimeout());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriver" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public WebServiceDriver(string baseAddress)
        {
            this.HttpClient = HttpClientFactory.GetClient(new Uri(baseAddress), WebServiceConfig.GetWebServiceTimeout());
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
        public T Get<T>(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Get, requestUri), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        public T Get<T>(string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Get, requestUri), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response content as a string</returns>
        public string Get(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return Send(this.CreateRequest(WebServiceVerb.Get, requestUri), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response content as a string</returns>
        public string Get(string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            return Send(this.CreateRequest(WebServiceVerb.Get, requestUri), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage GetWithResponse(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Get, requestUri), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service get
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage GetWithResponse(string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Get, requestUri), expectedMediaType, expectedStatus);
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
        public T Post<T>(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Post, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        public T Post<T>(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Post, requestUri, content), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        public string Post(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return Send(this.CreateRequest(WebServiceVerb.Post, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectedStatus">Assert a specific status was returned</param>
        /// <returns>The response body as a string</returns>
        public string Post(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send(this.CreateRequest(WebServiceVerb.Post, requestUri, content), expectedMediaType, expectedStatus);
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
        public string Post(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return Send(this.CreateRequest(WebServiceVerb.Post, requestUri, httpContent), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="contentEncoding">How to encode the post content</param>
        /// <param name="postMediaType">The type of the media being posted</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The response body as a string</returns>
        public string Post(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return Send(this.CreateRequest(WebServiceVerb.Post, requestUri, httpContent), expectedMediaType, expectedStatus);
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
        public HttpResponseMessage PostWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Post, requestUri, httpContent), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="contentEncoding">How to encode the post content</param>
        /// <param name="postMediaType">The type of the media being posted</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The response body as a string</returns>
        public HttpResponseMessage PostWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Post, requestUri, httpContent), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PostWithResponse(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Post, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service post
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The post content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PostWithResponse(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Post, requestUri, content), expectedMediaType, expectedStatus);
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
        public T Put<T>(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Put, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        public T Put<T>(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Put, requestUri, content), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        public string Put(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return Send(this.CreateRequest(WebServiceVerb.Put, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response body as a string</returns>
        public string Put(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send(this.CreateRequest(WebServiceVerb.Put, requestUri, content), expectedMediaType, expectedStatus);
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
        public string Put(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            return this.Custom(WebServiceVerb.Put, requestUri, expectedMediaType, content, contentEncoding, postMediaType, contentAsString, expectSuccess);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The response body as a string</returns>
        public string Put(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            return this.Custom(WebServiceVerb.Put, requestUri, expectedMediaType, content, contentEncoding, postMediaType, expectedStatus, contentAsString);
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
        public HttpResponseMessage PutWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Put, requestUri, httpContent), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PutWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Put, requestUri, httpContent), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PutWithResponse(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Put, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service put
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PutWithResponse(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Put, requestUri, content), expectedMediaType, expectedStatus);
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
        public T Patch<T>(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Patch, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        public T Patch<T>(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Patch, requestUri, content), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response body as a string</returns>
        public string Patch(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return Send(this.CreateRequest(WebServiceVerb.Patch, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response body as a string</returns>
        public string Patch(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send(this.CreateRequest(WebServiceVerb.Patch, requestUri, content), expectedMediaType, expectedStatus);
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
        public string Patch(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            return this.Custom(WebServiceVerb.Patch, requestUri, expectedMediaType, content, contentEncoding, postMediaType, contentAsString, expectSuccess);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The response body as a string</returns>
        public string Patch(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            return this.Custom(WebServiceVerb.Patch, requestUri, expectedMediaType, content, contentEncoding, postMediaType, expectedStatus, contentAsString);
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
        public HttpResponseMessage PatchWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Patch, requestUri, httpContent), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="contentEncoding">How to encode the put content</param>
        /// <param name="postMediaType">The type of the media being put</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">If true pass content as StringContent, else pass as StreamContent</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PatchWithResponse(string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Patch, requestUri, httpContent), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PatchWithResponse(string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Patch, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service patch
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">The put content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage PatchWithResponse(string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Patch, requestUri, content), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response by deserialized as the <typeparamref name="T"/></returns>
        public T Delete<T>(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Delete, requestUri), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response by deserialized as the <typeparamref name="T"/></returns>
        public T Delete<T>(string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            return Send<T>(this.CreateRequest(WebServiceVerb.Delete, requestUri), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response as a string</returns>
        public string Delete(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return Send(this.CreateRequest(WebServiceVerb.Delete, requestUri), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response as a string</returns>
        public string Delete(string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            return Send(this.CreateRequest(WebServiceVerb.Delete, requestUri), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage DeleteWithResponse(string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Delete, requestUri), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service delete
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The http response message</returns>
        public HttpResponseMessage DeleteWithResponse(string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            return SendWithResponse(this.CreateRequest(WebServiceVerb.Delete, requestUri), expectedMediaType, expectedStatus);
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
            return Send<T>(this.CreateRequest(customType, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="customType">The custom HTTP verb</param>
        /// <param name="requestUri">The request uri</param>
        /// <param name="expectedMediaType">Type of media being requested</param>
        /// <param name="content">Content of the message</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>HTTP response message</returns>
        public T Custom<T>(string customType, string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send<T>(this.CreateRequest(customType, requestUri, content), expectedMediaType, expectedStatus);
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
            return Send(this.CreateRequest(customType, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service call with a custom verb
        /// </summary>
        /// <param name="customType">Custom HTTP verb</param>
        /// <param name="requestUri">The request URI </param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="content">Content of the message</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The HTTP response message</returns>
        public string Custom(string customType, string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return Send(this.CreateRequest(customType, requestUri, content), expectedMediaType, expectedStatus);
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
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">The message content as a string</param>
        /// <returns>The HTTP Response message</returns>
        public string Custom(string customType, string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            HttpResponseMessage response = this.CustomWithResponse(customType, requestUri, expectedMediaType, content, contentEncoding, postMediaType, expectedStatus, contentAsString);
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
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
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, bool contentAsString = true, bool expectSuccess = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(customType, requestUri, httpContent), expectedMediaType, expectSuccess);
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
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <param name="contentAsString">The content as a a string</param>
        /// <returns>The HTTP response message</returns>
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, string content, Encoding contentEncoding, string postMediaType, HttpStatusCode expectedStatus, bool contentAsString = true)
        {
            HttpContent httpContent = CreateContent(content, contentEncoding, postMediaType, contentAsString);
            return SendWithResponse(this.CreateRequest(customType, requestUri, httpContent), expectedMediaType, expectedStatus);
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
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, HttpContent content, bool expectSuccess = true)
        {
            return SendWithResponse(this.CreateRequest(customType, requestUri, content), expectedMediaType, expectSuccess);
        }

        /// <summary>
        /// Execute a web service call 
        /// </summary>
        /// <param name="customType">The HTTP verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="content">The content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The HTTP response message</returns>
        public HttpResponseMessage CustomWithResponse(string customType, string requestUri, string expectedMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            return SendWithResponse(this.CreateRequest(customType, requestUri, content), expectedMediaType, expectedStatus);
        }

        /// <summary>
        /// Execute a web service send with a web request
        /// </summary>
        /// <param name="httpRequestMessage">The request message</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP Response message</returns>
        public string Send(HttpRequestMessage httpRequestMessage, string expectedMediaType, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.SendWithResponse(httpRequestMessage, expectedMediaType, expectSuccess);
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Execute a web service send with a web request
        /// </summary>
        /// <param name="httpRequestMessage">The request message</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The HTTP Response message</returns>
        public string Send(HttpRequestMessage httpRequestMessage, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            HttpResponseMessage response = this.SendWithResponse(httpRequestMessage, expectedMediaType, expectedStatus);
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Execute a web service send with a web request
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="httpRequestMessage">The request message</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        public T Send<T>(HttpRequestMessage httpRequestMessage, string expectedMediaType, bool expectSuccess = true)
        {
            HttpResponseMessage response = this.SendWithResponse(httpRequestMessage, expectedMediaType, expectSuccess);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service send with a web request
        /// </summary>
        /// <typeparam name="T">The expected response type</typeparam>
        /// <param name="httpRequestMessage">The request message</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The response deserialized as - <typeparamref name="T"/></returns>
        public T Send<T>(HttpRequestMessage httpRequestMessage, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            HttpResponseMessage response = this.SendWithResponse(httpRequestMessage, expectedMediaType, expectedStatus);
            return WebServiceUtils.DeserializeResponse<T>(response, this.supportedFormatters);
        }

        /// <summary>
        /// Execute a web service send with a web request
        /// </summary>
        /// <param name="httpRequestMessage">The request message</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A reponse message</returns>
        public virtual HttpResponseMessage SendWithResponse(HttpRequestMessage httpRequestMessage, string expectedMediaType, bool expectSuccess = true)
        {
            // Make sure the Http client accepts the requested media tyoe
            this.AddAcceptIfNotPresent(expectedMediaType);
            if (string.IsNullOrEmpty(WebServiceConfig.GetHttpClientVersion()))
            {
                httpRequestMessage.Version = new Version(WebServiceConfig.GetHttpClientVersion());
            }
            HttpResponseMessage response = SendAsync(httpRequestMessage).GetAwaiter().GetResult();

            if (expectSuccess)
            {
                EnsureSuccessStatusCode(response);
            }

            return response;
        }

        /// <summary>
        /// Execute a web service send with a web request
        /// </summary>
        /// <param name="httpRequestMessage">The request message</param>
        /// <param name="expectedMediaType">The type of media you are expecting back</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>A reponse message</returns>
        public virtual HttpResponseMessage SendWithResponse(HttpRequestMessage httpRequestMessage, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            // Make sure the Http client accepts the requested media tyoe
            this.AddAcceptIfNotPresent(expectedMediaType);

            HttpResponseMessage response = SendAsync(httpRequestMessage).GetAwaiter().GetResult();

            EnsureStatusCodesMatch(response, expectedStatus);

            return response;
        }

        /// <summary>
        /// Send an async request
        /// </summary>
        /// <param name="message">Message being sent</param>
        /// <returns>The HTTP response message</returns>
        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage message)
        {
            var cts = new CancellationTokenSource();
            try
            {
                return await this.HttpClient.SendAsync(message, cts.Token).ConfigureAwait(false);
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken != cts.Token)
                {
                    // This is likely a timeout error
                    throw new TimeoutException("Service call likely exceeded the timeout: " + this.HttpClient.Timeout.ToString(), ex);
                }

                // This was not a timeout
                throw;
            }
        }

        /// <summary>
        /// Create http request message
        /// </summary>
        /// <param name="methodVerb">The HTTP request verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <returns>A http request message</returns>
        private HttpRequestMessage CreateRequest(string methodVerb, string requestUri)
        {
            HttpMethod method = new HttpMethod(methodVerb);
            HttpRequestMessage message = new HttpRequestMessage(method, requestUri);

            return message;
        }

        /// <summary>
        /// Create http request message
        /// </summary>
        /// <param name="methodVerb">The HTTP request verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="content">The content</param>
        /// <returns>A http request message</returns>
        private HttpRequestMessage CreateRequest(string methodVerb, string requestUri, HttpContent content)
        {
            HttpRequestMessage message = CreateRequest(methodVerb, requestUri);
            message.Content = content;
            return message;
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
                string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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
        /// Ensure the HTTP response has specified status, if not throw a user friendly error message
        /// </summary>
        /// <param name="response">The HTTP response</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        private static void EnsureStatusCodesMatch(HttpResponseMessage response, HttpStatusCode expectedStatus)
        {
            // Make sure a response was returned
            if (response == null)
            {
                throw new HttpRequestException("Response was null");
            }

            // Check if it was a success and if not create a user friendly error message
            if (response.StatusCode != expectedStatus)
            {
                string body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                throw new HttpRequestException(
                    StringProcessor.SafeFormatter(
                        "Response status did not match expected.{0}Response code was: {1} ({2}){0}Expected code was: {3} ({4}){0}{5}",
                    Environment.NewLine,
                    (int)response.StatusCode,
                    response.StatusCode,
                    (int)expectedStatus,
                    expectedStatus,
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