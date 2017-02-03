//--------------------------------------------------
// <copyright file="BaseWebServiceTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base web service test class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Magenic.MaqsFramework.BaseWebServiceTest
{
    /// <summary>
    /// Generic base web service test class
    /// </summary>
    public class BaseWebServiceTest : BaseGenericTest<HttpClientWrapper, WebServiceTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseWebServiceTest"/> class.
        /// Setup the web service client for each test class
        /// </summary>
        public BaseWebServiceTest()
        {
        }

        /// <summary>
        /// Gets or sets the web service wrapper
        /// </summary>
        public HttpClientWrapper WebServiceWrapper
        {
            get
            {
                return (HttpClientWrapper)this.ObjectUnderTest;
            }

            set
            {
                this.ObjectUnderTest = value;
            }
        }

        /// <summary>
        /// Map web service events to log events
        /// </summary>
        /// <param name="eventFiringWrapper">The event firing web client wrapper that we want mapped</param>
        public void MapEvents(EventFiringHttpClientWrapper eventFiringWrapper)
        {
            if (this.LoggingEnabledSetting == LoggingEnabled.YES || this.LoggingEnabledSetting == LoggingEnabled.ONFAIL)
            {
                eventFiringWrapper.WebServiceEvent += this.WebService_Event;
                eventFiringWrapper.WebServiceErrorEvent += this.WebService_Error;
            }
        }

        /// <summary>
        /// Get the http client
        /// </summary>
        /// <param name="baseAddress">The base web service Uri</param>
        /// <param name="mediaType">The type of media expected</param>
        /// <returns>The http client</returns>
        protected virtual HttpClient GetHttpClient(Uri baseAddress, string mediaType)
        {
            HttpClient client = this.WebServiceWrapper.BaseHttpClient;

            if (client.BaseAddress == null || !client.BaseAddress.Equals(baseAddress))
            {
                try
                {
                    client.BaseAddress = baseAddress;
                }
                catch (InvalidOperationException e)
                {
                    // Provide a better error message
                    throw new InvalidOperationException(string.Format("Tried to change base URl from '{0}' to '{1}'", client.BaseAddress, baseAddress), e);
                }
            }

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

            return client;
        }

        /// <summary>
        /// Get the base web service url
        /// </summary>
        /// <returns>The base web service url</returns>
        protected virtual string GetBaseWebServiceUrl()
        {
            return WebServiceConfig.GetWebServiceUri();
        }

        /// <summary>
        /// Setup the event firing web service wrapper and map the events to the logger
        /// </summary>
        protected override void SetupEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing web service wrapper");

            this.ObjectUnderTest = this.GetEventFiringHttpClientWrapper();
            this.MapEvents((EventFiringHttpClientWrapper)this.ObjectUnderTest);
        }

        /// <summary>
        /// Setup the none event firing web service wrapper
        /// </summary>
        protected override void SetupNoneEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting web service wrapper");
            this.ObjectUnderTest = this.GetHttpClientWrapper();
        }

        /// <summary>
        /// Create a web service test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new WebServiceTestObject(this.WebServiceWrapper, this.Log, this.SoftAssert, this.PerfTimerCollection);
        }

        /// <summary>
        /// The default get web service wrapper function
        /// </summary>
        /// <returns>The web service wrapper</returns>
        private HttpClientWrapper GetHttpClientWrapper()
        {
            HttpClientWrapper wrapper = new HttpClientWrapper(this.GetBaseWebServiceUrl());
            wrapper.OverrideSetupClientConnection(this.GetHttpClient);

            return wrapper;
        }

        /// <summary>
        /// The default get event firing web service wrapper function
        /// </summary>
        /// <returns>The event firing web service wrapper</returns>
        private EventFiringHttpClientWrapper GetEventFiringHttpClientWrapper()
        {
            EventFiringHttpClientWrapper wrapper = new EventFiringHttpClientWrapper(this.GetBaseWebServiceUrl());
            wrapper.OverrideSetupClientConnection(this.GetHttpClient);

            return wrapper;
        }

        /// <summary>
        /// Web service event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void WebService_Event(object sender, string message)
        {
            this.Log.LogMessage(MessageType.INFORMATION, message);
        }

        /// <summary>
        /// Web service error event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void WebService_Error(object sender, string message)
        {
            this.Log.LogMessage(MessageType.ERROR, message);
        }
    }
}
