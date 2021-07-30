//--------------------------------------------------
// <copyright file="ITestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Test object interface</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseTest
{
    public interface ITestObject : IDisposable
    {
        ILogger Log { get; set; }
        IManagerStore ManagerStore { get; }
        Dictionary<string, object> Objects { get; }
        IPerfTimerCollection PerfTimerCollection { get; set; }
        ISoftAssert SoftAssert { get; set; }
        Dictionary<string, string> Values { get; }

        HashSet<string> AssociatedFiles { get; }

        bool AddAssociatedFile(string path);
        void AddDriverManager(string key, IDriverManager manager);
        void AddDriverManager<T>(T manager, bool overrideIfExists = false) where T : IDriverManager;
        bool ContainsAssociatedFile(string path);
        void Dispose();
        string[] GetArrayOfAssociatedFiles();
        T GetDriverManager<T>() where T : IDriverManager;
        void OverrideDriverManager(string key, IDriverManager manager);
        bool RemoveAssociatedFile(string path);
        void SetObject(string key, object value);
        void SetValue(string key, string value);
    }
}