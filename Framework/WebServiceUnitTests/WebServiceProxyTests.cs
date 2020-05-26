//--------------------------------------------------
// <copyright file="WebServiceProxyTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Web Service Proxy Tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Contains web service tests that require a proxy to be set up
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebServiceProxyTests : BaseWebServiceTest
    {
        /// <summary>
        /// The proxy request history
        /// </summary>
        private static readonly IDictionary<int, Titanium.Web.Proxy.Http.Request> RequestsHistory = new ConcurrentDictionary<int, Titanium.Web.Proxy.Http.Request>();

        /// <summary>
        /// The proxy response history
        /// </summary>
        private static readonly IDictionary<int, Titanium.Web.Proxy.Http.Response> ResponsesHistory = new ConcurrentDictionary<int, Titanium.Web.Proxy.Http.Response>();

        /// <summary>
        /// The proxy server
        /// </summary>
        private static ProxyServer proxyServer;

        /// <summary>
        /// Class initialization
        /// </summary>
        /// <param name="context">Test context</param>
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            StartProxy();
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            proxyServer.Stop();
        }

        /// <summary>
        /// Test to assert the web service driver uses the proxy if configured
        /// </summary>
        [TestMethod]
        public void WebServiceDriverUsesProxy()
        {
            string url = WebServiceConfig.GetWebServiceUri().Replace("https://", "http://");
            this.WebServiceDriver.HttpClient.BaseAddress = new Uri(url);

            this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", "application/json");

            bool proxyUsed = RequestsHistory.Values.Any(r => r.RequestUri.ToString().Contains("/api/XML_JSON/GetAllProducts"));
            Assert.IsTrue(proxyUsed, "Failed to assert the proxy was used by the Web Service Driver.");
        }

        /// <summary>
        /// Get if proxy should be used
        /// </summary>
        /// <returns>True if should use proxy</returns>
        protected override bool GetUseProxy()
        {
            return true;
        }

        /// <summary>
        /// Starts a local proxy
        /// </summary>
        private static void StartProxy()
        {
            proxyServer = new ProxyServer();
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8001, true);
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();
            proxyServer.BeforeRequest += OnRequestCaptureTrafficEventHandler;
            proxyServer.BeforeResponse += OnResponseCaptureTrafficEventHandler;
        }

        /// <summary>
        /// Captures proxy traffic on response
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        /// <returns>The task</returns>
        private static async Task OnResponseCaptureTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(
            () =>
            {
                if (!ResponsesHistory.ContainsKey(e.HttpClient.Response.GetHashCode()) && e.HttpClient != null && e.HttpClient.Response != null)
                {
                    ResponsesHistory.Add(e.HttpClient.Response.GetHashCode(), e.HttpClient.Response);
                }
            });

        /// <summary>
        /// Captures proxy traffic on request
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Event args</param>
        /// <returns>The task</returns>
        private static async Task OnRequestCaptureTrafficEventHandler(object sender, SessionEventArgs e) => await Task.Run(
            () =>
            {
                if (!RequestsHistory.ContainsKey(e.HttpClient.Request.GetHashCode()) && e.HttpClient != null && e.HttpClient.Request != null)
                {
                    RequestsHistory.Add(e.HttpClient.Request.GetHashCode(), e.HttpClient.Request);
                }
            });
    }
}
