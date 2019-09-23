//--------------------------------------------------
// <copyright file="WebServiceProxyTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Web Service Proxy Tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using WebServiceTesterUnitTesting.Model;

namespace WebServiceTesterUnitTesting
{
    /// <summary>
    /// Contains web service tests that require a proxy to be set up
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    [DoNotParallelize]
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
            OverrideConfigUseProxy(true);
            StartProxy();
        }

        /// <summary>
        /// Class cleanup
        /// </summary>
        [ClassCleanup]
        public static void ClassCleanup()
        {
            proxyServer.Stop();
            OverrideConfigUseProxy(false);
        }

        /// <summary>
        /// Test to assert the web service driver uses the proxy if configured
        /// </summary>
        [TestMethod]
        public void WebServiceDriverUsesProxy()
        {
            this.WebServiceDriver.GetWithResponse("/api/XML_JSON/GetAllProducts", "application/json");

            bool proxyUsed = RequestsHistory.Values.Any(r => r.RequestUri.ToString().Contains("/api/XML_JSON/GetAllProducts"));
            Assert.IsTrue(proxyUsed, "Failed to assert the proxy was used by the Web Service Driver.");
        }

        /// <summary>
        /// Starts a local proxy
        /// </summary>
        private static void StartProxy()
        {
            proxyServer = new ProxyServer();
            proxyServer.CertificateManager.TrustRootCertificate(true);
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8001, true);
            proxyServer.AddEndPoint(explicitEndPoint);
            proxyServer.Start();
            proxyServer.BeforeRequest += OnRequestCaptureTrafficEventHandler;
            proxyServer.BeforeResponse += OnResponseCaptureTrafficEventHandler;
        }

        /// <summary>
        /// Override the Use Proxy key of the config
        /// </summary>
        /// <param name="useProxy">Yes if true</param>
        private static void OverrideConfigUseProxy(bool useProxy)
        {
            string value = useProxy ? "Yes" : "No";
            Dictionary<string, string> overrides = new Dictionary<string, string> { { "UseProxy", value } };
            Config.AddTestSettingValues(overrides, ConfigSection.WebServiceMaqs, true);
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
