//--------------------------------------------------
// <copyright file="BaseTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds base context data</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseTest
{
    public interface IBaseTestObject : IDisposable
    {
        ILogger Log { get; set; }
        ManagerDictionary ManagerStore { get; }
        Dictionary<string, object> Objects { get; }
        IPerfTimerCollection PerfTimerCollection { get; set; }
        ISoftAssert SoftAssert { get; set; }
        Dictionary<string, string> Values { get; }

        HashSet<string> AssociatedFiles { get; }

        bool AddAssociatedFile(string path);
        void AddDriverManager(string key, IDriverManager driver);
        void AddDriverManager<T>(T driver, bool overrideIfExists = false) where T : IDriverManager;
        bool ContainsAssociatedFile(string path);
        void Dispose();
        string[] GetArrayOfAssociatedFiles();
        T GetDriverManager<T>() where T : IDriverManager;
        void OverrideDriverManager(string key, IDriverManager driver);
        bool RemoveAssociatedFile(string path);
        void SetObject(string key, object value);
        void SetValue(string key, string value);
    }
}