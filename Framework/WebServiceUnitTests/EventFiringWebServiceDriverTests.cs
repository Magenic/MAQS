//--------------------------------------------------
// <copyright file="EventFiringWebServiceDriverTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test the EventFiringWebServiceDriver class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Test the EventFiringClientDriver class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EventFiringWebServiceDriverTests : EventFiringWebServiceDriver
    {
        /// <summary>
        /// Default baseAddress for default constructor
        /// </summary>
        private static readonly string baseAddress = WebServiceConfig.GetWebServiceUri();

        /// <summary>
        /// Number of information events events fired
        /// </summary>
        private static int InformationEvents = 0;

        /// <summary>
        /// Number of action events fired
        /// </summary>
        private static int ActionEvents = 0;

        /// <summary>
        /// Number of error events fired
        /// </summary>
        private static int ErrorEvents = 0;

        /// <summary>
        /// Number of verbose fired
        /// </summary>
        private static int VerboseEvents = 0;

        /// <summary>
        /// Last event message sent
        /// </summary>
        private static string LatestMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringWebServiceDriverTests"/> class
        /// </summary>
        public EventFiringWebServiceDriverTests() : base(baseAddress)
        {
            this.WebServiceEvent += this.WebService_Event;
            this.WebServiceActionEvent += this.WebService_Action;
            this.WebServiceErrorEvent += this.WebService_Error;
            this.WebServiceVerboseEvent += this.WebService_Verbose;
        }

        /// <summary>
        /// Verify that send throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendWithResponseThrowException()
        {
            SendWithResponse(null, null);
        }

        /// <summary>
        /// Verify that sent throws proper exception 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SendWithResponseAndReturnCodeThrowException()
        {
            SendWithResponse(null, null, HttpStatusCode.OK);
        }

        /// <summary>
        /// Verify that we can send
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void SendWithResponse()
        {
            var result = SendWithResponse(GetValidRequestMessage(), MediaType.AppJson, true);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify that we can send with expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void SendWithResponseAndReturnCode()
        {
            var result = SendWithResponse(GetValidRequestMessage(), MediaType.AppXml, HttpStatusCode.OK);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        /// <summary>
        /// Verify that CustomContent throws proper exception
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.WebService)]
        public void UriRespected()
        {
            EventFiringWebServiceDriver standAlone = new EventFiringWebServiceDriver(new Uri(baseAddress));
            Assert.IsTrue(standAlone.HttpClient.BaseAddress.Equals(baseAddress), $"Expected {baseAddress} but got {standAlone.HttpClient.BaseAddress}");
        }

        /// <summary>
        /// Verify events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseEvent()
        {
            string message = "Event" + Guid.NewGuid();
            TestSingleEvent(OnEvent, message, ref InformationEvents);
        }

        /// <summary>
        /// Verify action events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseAction()
        {
            string message = "Action" + Guid.NewGuid();
            TestSingleEvent(OnActionEvent, message, ref ActionEvents);
        }

        /// <summary>
        /// Verify verbose events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseVerbose()
        {
            string message = "Verbose" + Guid.NewGuid();
            TestSingleEvent(OnVerboseEvent, message, ref VerboseEvents);
        }

        /// <summary>
        /// Verify error events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseError()
        {
            string message = "Error" + Guid.NewGuid();
            TestSingleEvent(OnErrorEvent, message, ref ErrorEvents);
        }

        /// <summary>
        /// Verify null events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseNullEvent()
        {
            OnEvent(null);
        }

        /// <summary>
        /// Verify null action events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseNullActionEvent()
        {
            OnActionEvent(null);
        }

        /// <summary>
        /// Verify null error events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseNullError()
        {
            OnErrorEvent(null);
        }

        /// <summary>
        /// Verify null verbose events are raised
        /// </summary>
        [TestMethod]
        [DoNotParallelize]
        [TestCategory(TestCategories.WebService)]
        public void RaiseNullVerbose()
        {
            OnVerboseEvent(null);
        }

        /// <summary>
        /// Test a single event is raised correctly
        /// </summary>
        /// <param name="sendMessage">The send event</param>
        /// <param name="message">Event massage</param>
        /// <param name="eventRef">The releated event count</param>
        private void TestSingleEvent(Action<string> sendMessage, string message, ref int eventRef)
        {
            eventRef = 0;
            sendMessage(message);

            Assert.AreEqual(message, LatestMessage);
            Assert.AreEqual(1, eventRef);
        }

        /// <summary>
        /// Get a valid request message
        /// </summary>
        /// <returns>A valid request message</returns>
        private HttpRequestMessage GetValidRequestMessage()
        {
            Product product = new Product
            {
                Category = "ff",
                Id = 4,
                Name = "ff",
                Price = 3.25f
            };

            return new HttpRequestMessage(new HttpMethod(WebServiceVerb.Post), "/api/XML_JSON/Post")
            {
                Content = WebServiceUtils.MakeStreamContent(product, Encoding.UTF8, "application/xml")
            };
        }

        /// <summary>
        /// Web service event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void WebService_Event(object sender, string message)
        {
            LatestMessage = message;
            InformationEvents++;
        }

        /// <summary>
        /// Web service action event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void WebService_Action(object sender, string message)
        {
            LatestMessage = message;
            ActionEvents++;
        }

        /// <summary>
        /// Web service error event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void WebService_Error(object sender, string message)
        {
            LatestMessage = message;
            ErrorEvents++;
        }

        /// <summary>
        /// Web service verbose event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void WebService_Verbose(object sender, string message)
        {
            LatestMessage = message;
            VerboseEvents++;
        }
    }
}
