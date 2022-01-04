//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriver.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>The event firing basic http client interactions</summary>
//--------------------------------------------------
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
        /// Web service verbose event
        /// </summary>
        public event EventHandler<string> WebServiceVerboseEvent;

        /// <summary>
        /// Web service event
        /// </summary>
        /// <param name="message">The event message</param>
        protected virtual void OnEvent(string message)
        {
            WebServiceEvent?.Invoke(this, message?.Trim());
        }

        /// <summary>
        /// Web service action event
        /// </summary>
        /// <param name="message">The event message</param>
        protected virtual void OnActionEvent(string message)
        {
            WebServiceActionEvent?.Invoke(this, message?.Trim());
        }

        /// <summary>
        /// Web service error event
        /// </summary>
        /// <param name="message">The event error message</param>
        protected virtual void OnErrorEvent(string message)
        {
            WebServiceErrorEvent?.Invoke(this, message?.Trim());
        }

        /// <summary>?
        /// Web service verbose event
        /// </summary>
        /// <param name="message">The event verbose message</param>
        protected virtual void OnVerboseEvent(string message)
        {
            WebServiceVerboseEvent?.Invoke(this, message?.Trim());
        }

        /// <summary>
        /// Send a request and get a response message back
        /// </summary>
        /// <param name="httpRequestMessage">The request</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectSuccess">Assert a success code was returned</param>
        /// <returns></returns>
        public override HttpResponseMessage SendWithResponse(HttpRequestMessage httpRequestMessage, string expectedMediaType, bool expectSuccess = true)
        {
            RaiseSendEvent(httpRequestMessage);
            HttpResponseMessage response = base.SendWithResponse(httpRequestMessage, expectedMediaType, expectSuccess);
            RaiseEvent(httpRequestMessage.Method.ToString(), response);

            return response;
        }

        /// <summary>
        /// Send a request and get a response message back
        /// </summary>
        /// <param name="httpRequestMessage">The request</param>
        /// <param name="expectedMediaType">The type of media being requested</param>
        /// <param name="expectedStatus">Assert a specific status code was returned</param>
        /// <returns></returns>
        public override HttpResponseMessage SendWithResponse(HttpRequestMessage httpRequestMessage, string expectedMediaType, HttpStatusCode expectedStatus)
        {
            RaiseSendEvent(httpRequestMessage);
            HttpResponseMessage response = base.SendWithResponse(httpRequestMessage, expectedMediaType, expectedStatus);
            RaiseEvent(httpRequestMessage.Method.ToString(), response);

            return response;
        }

        /// <summary>
        /// Raise an request action message
        /// </summary>
        /// <param name="httpRequestMessage">The request message</param>
        private void RaiseSendEvent(HttpRequestMessage httpRequestMessage)
        {
            try
            {
                // Action logging information 
                StringBuilder message = new StringBuilder();
                message.AppendLine($"Sending {httpRequestMessage.Method} request to {this.HttpClient.BaseAddress} at endpoint {httpRequestMessage.RequestUri}");
                BuildContentMessage(message, httpRequestMessage.Content);
                OnActionEvent(message.ToString());

                // Verbose logging information 
                message = new StringBuilder();
                message.AppendLine("Request details:");
                message.AppendLine($"Base address: {this.HttpClient.BaseAddress}");
                message.AppendLine(httpRequestMessage.ToString().Trim());
                message.AppendLine($"Default header:  {this.HttpClient.DefaultRequestHeaders.ToString().Trim()}");
                message.AppendLine($"Timeout: {this.HttpClient.Timeout}");
                message.AppendLine($"Max buffer: {this.HttpClient.MaxResponseContentBufferSize}");
                OnVerboseEvent(message.ToString());
            }
            catch (Exception e)
            {
                RaiseLoggingErrorMessage(e);
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
                // Action logging information 
                StringBuilder message = new StringBuilder();
                HttpRequestMessage responseMessage = response.RequestMessage;
                message.AppendLine($"Received {actionType} response from {responseMessage.RequestUri}");
                message.AppendLine($"Returned {response.ReasonPhrase}({(int)response.StatusCode})");

                // Only pull content if we are returned content
                if (response.Content.Headers.ContentType != null)
                {
                    BuildContentMessage(message, response.Content);
                }

                OnEvent(message.ToString());

                // Verbose logging information 
                OnVerboseEvent($"Response details:{ Environment.NewLine}{response}{Environment.NewLine}From Request:{Environment.NewLine}{response.RequestMessage}");
            }
            catch (Exception e)
            {
                RaiseLoggingErrorMessage(e);
            }
        }

        /// <summary>
        /// Raise a logging exception message
        /// </summary>
        /// <param name="e">The exception</param>
        private void RaiseLoggingErrorMessage(Exception e)
        {
            OnErrorEvent($"Failed to log event because: {e.Message} {Environment.NewLine} {e}");
        }

        /// <summary>
        /// Build the content massage
        /// </summary>
        /// <param name="message">String builder for building the message</param>
        /// <param name="content">The content we are building a message for</param>
        private void BuildContentMessage(StringBuilder message, HttpContent content)
        {
            message.AppendLine("Content:");

            if (content == null)
            {
                message.AppendLine("  **Content is null**");
                return;
            }

            string mediaType = content.Headers?.ContentType?.MediaType;

            if (string.IsNullOrEmpty(mediaType))
            {
                message.AppendLine("  **Writing content with a null or empty media type to the log is not supported**");
            }
            else
            {
                mediaType = mediaType.ToUpper();

                if (mediaType.Contains("TEXT") || mediaType.Contains("XML") || mediaType.Contains("JSON") || mediaType.Contains("HTML"))
                {
                    message.AppendLine(content.ReadAsStringAsync().Result);
                }
                else
                {
                    message.AppendLine($"  **Writing '{mediaType}' content to the log is not supported**");
                }
            }
        }
    }
}