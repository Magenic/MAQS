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
        /// Cached copy of the driver
        /// </summary>
        private WebServiceDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriverManager" /> class
        /// </summary>
        /// <param name="getDriver">Function for creating an http client</param>
        /// <param name="testObject">The associated test object</param>
        public WebServiceDriverManager(Func<HttpClient> getDriver, BaseTestObject testObject) : base(getDriver, testObject)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceDriverManager" /> class
        /// </summary>
        /// <param name="getDriver">Function for creating an http client</param>
        /// <param name="testObject">The associated test object</param>
        public WebServiceDriverManager(WebServiceDriver getDriver, BaseTestObject testObject) : base(() => getDriver, testObject)
        {
            this.driver = getDriver;
        }

        /// <summary>
        /// Dispose of the driver
        /// </summary>
        public override void Dispose()
        {
            this.BaseDriver = null;
        }

        /// <summary>
        /// Override the http driver
        /// </summary>
        /// <param name="driver">A new http driver</param>
        public void OverrideDriver(WebServiceDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Get the http driver
        /// </summary>
        /// <returns>The http driver</returns>
        public WebServiceDriver GetWebServiceDriver()
        {
            if (this.driver == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting web service driver");
                    this.driver = new WebServiceDriver(GetBase() as HttpClient);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing web service driver");
                    this.driver = new EventFiringWebServiceDriver(GetBase() as HttpClient);
                    this.MapEvents(this.driver as EventFiringWebServiceDriver);
                }
            }

            return this.driver;
        }

        /// <summary>
        /// Get the service driver
        /// </summary>
        /// <returns>The web service driver</returns>
        public override object Get()
        {
            return this.GetWebServiceDriver();
        }

        /// <summary>
        /// Map web service events to log events
        /// </summary>
        /// <param name="eventFiringDriver">The event firing web client driver that we want mapped</param>
        public void MapEvents(EventFiringWebServiceDriver eventFiringDriver)
        {
            eventFiringDriver.WebServiceEvent += this.WebService_Event;
            eventFiringDriver.WebServiceErrorEvent += this.WebService_Error;
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
