//--------------------------------------------------
// <copyright file="WebServiceDriverManager.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Web service driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Net.Http;

namespace Magenic.Maqs.WebServiceTester
{
    /// <summary>
    /// Web service driver
    /// </summary>
    public class WebServiceDriverManager : DriverManager
    {
        /// <summary>
        /// Cached copy of the wrapper
        /// </summary>
        private WebServiceDriver wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriverManager" /> class
        /// </summary>
        /// <param name="getDriver">Function for creating an http client</param>
        /// <param name="testObject">The associated test object</param>
        public WebServiceDriverManager(Func<HttpClient> getDriver, BaseTestObject testObject) : base(getDriver, testObject)
        {
        }

        /// <summary>
        /// Dispose of the driver
        /// </summary>
        public override void Dispose()
        {
            this.BaseDriver = null;
        }

        /// <summary>
        /// Override the http wrapper
        /// </summary>
        /// <param name="wrapper">A new http wrapper</param>
        public void OverrideWrapper(WebServiceDriver wrapper)
        {
            this.wrapper = wrapper;
        }

        /// <summary>
        /// Get the http wrapper
        /// </summary>
        /// <returns>The http wrapper</returns>
        public new WebServiceDriver Get()
        {
            if (this.wrapper == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting web service wrapper");
                    this.wrapper = new WebServiceDriver(base.Get() as HttpClient);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing web service wrapper");
                    this.wrapper = new EventFiringWebServiceDriver(base.Get() as HttpClient);
                    this.MapEvents(this.wrapper as EventFiringWebServiceDriver);
                }
            }

            return this.wrapper;
        }

        /// <summary>
        /// Map web service events to log events
        /// </summary>
        /// <param name="eventFiringWrapper">The event firing web client wrapper that we want mapped</param>
        public void MapEvents(EventFiringWebServiceDriver eventFiringWrapper)
        {
            eventFiringWrapper.WebServiceEvent += this.WebService_Event;
            eventFiringWrapper.WebServiceErrorEvent += this.WebService_Error;
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
