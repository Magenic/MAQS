//--------------------------------------------------
// <copyright file="SeleniumProxyTests.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Selenium Proxy Tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Contains selenium tests that require a proxy
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SeleniumProxyTests : BaseSeleniumTest
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
        /// Test to assert the created web driver uses the proxy if configured
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Selenium)]
        public void WebDriverUsesProxy()
        {
            DriverOptions options = WebDriverFactory.GetDefaultHeadlessChromeOptions();
            options.SetProxySettings(Config.GetValueForSection(ConfigSection.SeleniumMaqs, "ProxyAddress"));
            this.WebDriver = WebDriverFactory.GetChromeDriver(TimeSpan.FromMilliseconds(61000), (ChromeOptions)options);

            string url = Path.Combine(SeleniumConfig.GetWebSiteBase().Replace("https://", "http://"), "Employees");
            this.WebDriver.Navigate().GoToUrl(url);

            bool proxyUsed = RequestsHistory.Values.Any(r => r.RequestUri.ToString().Contains("magenicautomation.azurewebsites.net/Employees"));
            Assert.IsTrue(proxyUsed, "Failed to assert the proxy was used by the web driver.");
        }

        /// <summary>
        /// Starts a local proxy
        /// </summary>
        private static void StartProxy()
        {
            proxyServer = new ProxyServer();
            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8002, true);
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
