﻿//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriver.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing basic http client interactions</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.Maqs.BaseWebServiceTest
{
    /// <summary>
    /// Wrap basic http client interactions
    /// </summary>
    public class EventFiringWebServiceDriver : WebServiceDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringWebServiceDriver" /> class
        /// </summary>
        /// <param name="httpClient">An http client</param>
        public EventFiringWebServiceDriver(HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringWebServiceDriver" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public EventFiringWebServiceDriver(Uri baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringWebServiceDriver" /> class
        /// </summary>
        /// <param name="baseAddress">The base web service uri</param>
        public EventFiringWebServiceDriver(string baseAddress) : base(baseAddress)
        {
        }

        /// <summary>
        /// Web service event
        /// </summary>
        public event EventHandler<string> WebServiceEvent;

        /// <summary>
        /// Web service error event
        /// </summary>
        public event EventHandler<string> WebServiceErrorEvent;

        /// <summary>
        /// Web service event
        /// </summary>
        /// <param name="message">The event message</param>
        protected virtual void OnEvent(string message)
        {
            this.WebServiceEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Web service error event
        /// </summary>
        /// <param name="message">The event error message</param>
        protected virtual void OnErrorEvent(string message)
        {
            this.WebServiceErrorEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Do a web service post for the given uri, content and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The post body</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected override async Task<HttpResponseMessage> PostContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            try
            {
                this.RaiseEvent(content, "Post", requestUri, responseMediaType);
                HttpResponseMessage response = base.PostContent(requestUri, responseMediaType, content, expectSuccess).Result;
                this.RaiseEvent("Post", response);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                this.RaiseErrorMessage(e);
                throw e;
            }
        }

        /// <summary>
        /// Make a HTTP call using the custom verb to the URI with the given verb, media response type and content
        /// </summary>
        /// <param name="requestUri">The request URI</param>
        /// <param name="customVerb">Custom HTTP Verb</param>
        /// <param name="responseMediaType">Response media type</param>
        /// <param name="content">Content of the message</param>
        /// <param name="expectSuccess">Assert a success status code was returned</param>
        /// <returns>The HTTP response message</returns>
        protected override async Task<HttpResponseMessage> CustomContent(string requestUri, string customVerb, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            try
            {
                this.RaiseEvent(content, customVerb, requestUri, responseMediaType);
                HttpResponseMessage response = base.CustomContent(requestUri, customVerb, responseMediaType, content, expectSuccess).Result;
                this.RaiseEvent(customVerb, response);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                this.RaiseErrorMessage(e);
                throw e;
            }
        }

        /// <summary>
        /// Do a web service put for the given uri, content and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The put body</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected override async Task<HttpResponseMessage> PutContent(string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            try
            {
                this.RaiseEvent(content, "Put", requestUri, responseMediaType);
                HttpResponseMessage response = base.PutContent(requestUri, responseMediaType, content, expectSuccess).Result;
                this.RaiseEvent("Put", response);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                this.RaiseErrorMessage(e);
                throw e;
            }
        }

        /// <summary>
        /// Do a web service delete for the given uri
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="returnMediaType">The expected response media type</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected override async Task<HttpResponseMessage> DeleteContent(string requestUri, string returnMediaType, bool expectSuccess = true)
        {
            try
            {
                this.RaiseEvent("Delete", requestUri, returnMediaType);
                HttpResponseMessage response = base.DeleteContent(requestUri, returnMediaType, expectSuccess).Result;
                this.RaiseEvent("Delete", response);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                this.RaiseErrorMessage(e);
                throw e;
            }
        }

        /// <summary>
        /// Do a web service get for the given uri and media type
        /// </summary>
        /// <param name="requestUri">The request uri</param>
        /// <param name="mediaType">What type of media are we expecting</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>A http response message</returns>
        protected override async Task<HttpResponseMessage> GetContent(string requestUri, string mediaType, bool expectSuccess = true)
        {
            try
            {
                this.RaiseEvent("Get", requestUri, mediaType);
                HttpResponseMessage response = base.GetContent(requestUri, mediaType, expectSuccess).Result;
                this.RaiseEvent("Get", response);
                return await Task.FromResult(response);
            }
            catch (Exception e)
            {
                this.RaiseErrorMessage(e);
                throw e;
            }
        }

        /// <summary>
        /// Raise an action message without content
        /// </summary>
        /// <param name="actionType">The action type - Get, Post, etc.</param>
        /// <param name="requestUri">The request uri</param>
        /// <param name="mediaType">The type of media being requested</param>
        private void RaiseEvent(string actionType, string requestUri, string mediaType)
        {
            this.OnEvent(StringProcessor.SafeFormatter("Sending {0} request to base: '{1}' endpoint: '{2}' with the media type: '{3}'", actionType, this.HttpClient.BaseAddress, requestUri, mediaType));
        }

        /// <summary>
        /// Raise an action message with content
        /// </summary>
        /// <param name="content">The content</param>
        /// <param name="actionType">The action type - Get, Post, etc.</param>
        /// <param name="requestUri">The request uri</param>
        /// <param name="mediaType">The type of media being requested</param>
        private void RaiseEvent(HttpContent content, string actionType, string requestUri, string mediaType)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(StringProcessor.SafeFormatter("Sending {0} request with content to base: '{1}' endpoint: '{2}' with the media type: '{3}'", actionType, this.HttpClient.BaseAddress, requestUri, mediaType));

                mediaType = mediaType.ToUpper();

                if (mediaType.Contains("TEXT") || mediaType.Contains("XML") || mediaType.Contains("JSON"))
                {
                    message.AppendLine("Content:");
                    if(content != null)
                    {
                        message.AppendLine(content.ReadAsStringAsync().Result);
                    }
                }

                this.OnEvent(message.ToString());
            }
            catch (Exception e)
            {
                this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
            }
        }

        /// <summary>
        /// Raise an action message for a response
        /// </summary>
        /// <param name="actionType">The action type - Get, Post, etc.</param>
        /// <param name="response">The response</param>
        private void RaiseEvent(string actionType, HttpResponseMessage response)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                HttpRequestMessage responseMessage = response.RequestMessage;
                message.AppendLine(StringProcessor.SafeFormatter("Received {0} response from {1}", actionType, responseMessage.RequestUri));
                message.AppendLine(StringProcessor.SafeFormatter("Returned {0}({1})", response.ReasonPhrase, (int)response.StatusCode));

                // Only pull contect if we are returned content
                if (response.Content.Headers.ContentType != null)
                {
                    string mediaType = response.Content.Headers.ContentType.MediaType.ToUpper();

                    // Only add the text if we know it is human readable
                    if (mediaType.Contains("TEXT") || mediaType.Contains("XML") || mediaType.Contains("JSON"))
                    {
                        message.AppendLine("Content:");
                        if (response.Content != null)
                        {
                            message.AppendLine(response.Content.ReadAsStringAsync().Result);
                        }
                    }
                }

                this.OnEvent(message.ToString());
            }
            catch (Exception e)
            {
                this.OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
            }
        }

        /// <summary>
        /// Raise an exception message
        /// </summary>
        /// <param name="e">The exception</param>
        private void RaiseErrorMessage(Exception e)
        {
            this.OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0} {1} {2}", e.Message, Environment.NewLine, e.ToString()));
        }
    }
}