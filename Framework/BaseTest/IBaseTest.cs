//--------------------------------------------------
// <copyright file="BaseTest.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base code for tests without a system under test object like web drivers or database connections</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseTest
{
    public interface IBaseTest
    {
        ILogger Log { get; set; }
        List<string> LoggedExceptionList { get; set; }
        ManagerDictionary ManagerStore { get; }
        IPerfTimerCollection PerfTimerCollection { get; set; }
        ISoftAssert SoftAssert { get; set; }
        TestContext TestContext { get; set; }
        IBaseTestObject TestObject { get; set; }

        void Setup();
        void Teardown();
    }
}