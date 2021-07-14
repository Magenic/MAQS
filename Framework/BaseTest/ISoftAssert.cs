//--------------------------------------------------
// <copyright file="SoftAssert.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>This is the SoftAssert class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Reflection;

namespace Magenic.Maqs.BaseTest
{
    public interface ISoftAssert
    {
        void AddExpectedAsserts(params string[] expectedAsserts);
     
        bool Assert(Action assertFunction);

        bool Assert(Action assertFunction, string assertName, string failureMessage = "");

        bool AssertFails(Action assertFunction, Type expectedException, string assertName, string failureMessage = "");

        void CaptureTestMethodAttributes(MethodInfo testMethod);
        bool DidSoftAssertsFail();
        bool DidUserCheck();
        void CheckForExpectedAsserts();
        void FailTestIfAssertFailed();
        void FailTestIfAssertFailed(string message);
        void LogFinalAssertData();
        void OverrideLogger(ILogger log);
    }
}