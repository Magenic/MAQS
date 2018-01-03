//--------------------------------------------------
// <copyright file="WebServiceTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds web service context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;

namespace Magenic.MaqsFramework.BaseWebServiceTest
{
    /// <summary>
    /// Web service test context data
    /// </summary>
    public class WebServiceTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WebServiceTestObject" /> class
        /// </summary>
        /// <param name="httpClientWrapper">The test's http client wrapper</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="perfTimerCollection">The test's performance timer collection</param>
        public WebServiceTestObject(HttpClientWrapper httpClientWrapper, Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection) : base(logger, softAssert, perfTimerCollection)
        {
            this.WebServiceWrapper = httpClientWrapper;
        }

        /// <summary>
        /// Gets the web driver
        /// </summary>
        public HttpClientWrapper WebServiceWrapper { get; private set; }
    }
}