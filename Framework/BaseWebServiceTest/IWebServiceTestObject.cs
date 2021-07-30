//--------------------------------------------------
// <copyright file="WebServiceTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds web service context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using System;
using System.Net.Http;

namespace Magenic.Maqs.BaseWebServiceTest
{
    public interface IWebServiceTestObject : ITestObject
    {
        WebServiceDriver WebServiceDriver { get; }
        WebServiceDriverManager WebServiceManager { get; }

        void OverrideWebServiceDriver(Func<HttpClient> httpClient);
        void OverrideWebServiceDriver(HttpClient httpClient);
        void OverrideWebServiceDriver(WebServiceDriver webServiceDriver);
    }
}