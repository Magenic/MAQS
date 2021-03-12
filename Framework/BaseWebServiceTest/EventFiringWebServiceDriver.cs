//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriver.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing basic http client interactions</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

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
        /// Web service action event
        /// </summary>
        public event EventHandler<string> WebServiceActionEvent;

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
            WebServiceEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Web service action event
        /// </summary>
        /// <param name="message">The event message</param>
        protected virtual void OnActionEvent(string message)
        {
            WebServiceActionEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Web service error event
        /// </summary>
        /// <param name="message">The event error message</param>
        protected virtual void OnErrorEvent(string message)
        {
            WebServiceErrorEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Execute a web service call 
        /// </summary>
        /// <param name="methodVerb">The HTTP verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP response message</returns>
        protected override HttpResponseMessage CallWithResponse(string methodVerb, string requestUri, string expectedMediaType, bool expectSuccess = true)
        {
            try
            {
                RaiseEvent(methodVerb, requestUri, expectedMediaType);
                HttpResponseMessage response = base.CallWithResponse(methodVerb, requestUri, expectedMediaType, expectSuccess);
                RaiseEvent(methodVerb, response);
                return response;
            }
            catch (Exception e)
            {
                RaiseErrorMessage(e);
                throw;
            }
        }

        /// <summary>
        /// Execute a web service call 
        /// </summary>
        /// <param name="methodVerb">The HTTP verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="expectedMediaType">The expected media type</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The HTTP response message</returns>
        protected override HttpResponseMessage CallWithResponse(string methodVerb, string requestUri, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            try
            {
                RaiseEvent(methodVerb, requestUri, expectedMediaType);
                HttpResponseMessage response = base.CallWithResponse(methodVerb, requestUri, expectedMediaType, expectedStatus);
                RaiseEvent(methodVerb, response);
                return response;
            }
            catch (Exception e)
            {
                RaiseErrorMessage(e);
                throw;
            }

        }

        /// <summary>
        /// Execute a web service call 
        /// </summary>
        /// <param name="methodVerb">The HTTP verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The content</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns>The HTTP response message</returns>
        protected override HttpResponseMessage CallContentWithResponse(string methodVerb, string requestUri, string responseMediaType, HttpContent content, bool expectSuccess = true)
        {
            try
            {
                RaiseEvent(content, methodVerb, requestUri, responseMediaType);
                HttpResponseMessage response = base.CallContentWithResponse(methodVerb, requestUri, responseMediaType, content, expectSuccess);
                RaiseEvent(methodVerb, response);
                return response;
            }
            catch (Exception e)
            {
                RaiseErrorMessage(e);
                throw;
            }
        }

        /// <summary>
        /// Execute a web service call 
        /// </summary>
        /// <param name="methodVerb">The HTTP verb</param>
        /// <param name="requestUri">The requested URI</param>
        /// <param name="responseMediaType">The response media type</param>
        /// <param name="content">The content</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns>The HTTP response message</returns>
        protected override HttpResponseMessage CallContentWithResponse(string methodVerb, string requestUri, string responseMediaType, HttpContent content, HttpStatusCode expectedStatus)
        {
            try
            {
                RaiseEvent(content, methodVerb, requestUri, responseMediaType);
                HttpResponseMessage response = base.CallContentWithResponse(methodVerb, requestUri, responseMediaType, content, expectedStatus);
                RaiseEvent(methodVerb, response);
                return response;
            }
            catch (Exception e)
            {
                RaiseErrorMessage(e);
                throw;
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
            OnActionEvent(StringProcessor.SafeFormatter("Send {0} request to base: '{1}' endpoint: '{2}' with the media type: '{3}'", actionType, HttpClient.BaseAddress, requestUri, mediaType));
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
                message.AppendLine(StringProcessor.SafeFormatter("Send {0} request with content to base: '{1}' endpoint: '{2}' with the media type: '{3}'", actionType, HttpClient.BaseAddress, requestUri, mediaType));

                BuildContentMessage(message, mediaType, content);

                OnActionEvent(message.ToString());
            }
            catch (Exception e)
            {
                OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
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

                // Only pull content if we are returned content
                if (response.Content.Headers.ContentType != null)
                {
                    string mediaType = response.Content.Headers.ContentType.MediaType;
                    BuildContentMessage(message, mediaType, response.Content);
                }

                OnEvent(message.ToString());
            }
            catch (Exception e)
            {
                OnErrorEvent(StringProcessor.SafeFormatter("Failed to log event because: {0}", e.ToString()));
            }
        }

        /// <summary>
        /// Raise an exception message
        /// </summary>
        /// <param name="e">The exception</param>
        private void RaiseErrorMessage(Exception e)
        {
            OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0} {1} {2}", e.Message, Environment.NewLine, e.ToString()));
        }

        /// <summary>
        /// Build the content massage
        /// </summary>
        /// <param name="message">String builder for building the message</param>
        /// <param name="mediaType">Content media type</param>
        /// <param name="content">The content we are building a message for</param>
        private void BuildContentMessage(StringBuilder message, string mediaType, HttpContent content)
        {
            message.AppendLine("Content:");
            message.AppendLine($"  Content Media Type: {mediaType}");
            message.AppendLine("  Content Text:");
            if (mediaType != null)
            { 
                mediaType = mediaType.ToUpper();
                
                if (mediaType.Contains("TEXT") || mediaType.Contains("XML") || mediaType.Contains("JSON") || mediaType.Contains("HTML"))
                {
                    if (content != null)
                    {
                        message.AppendLine(content.ReadAsStringAsync().Result);
                    }
                }
                else
                {
                    message.AppendLine("  **Writting this kind of content to the log is not supported**");
                }
            }
            else
            {
                message.AppendLine("  **Content is NULL**");
            }
        }
    }
}