//--------------------------------------------------
// <copyright file="IManagerStore.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Store of driver managers</summary>
//--------------------------------------------------
using System;

namespace Magenic.Maqs.BaseTest
{
    public interface IManagerStore : IDisposable
    {
        void Add(IDriverManager manager);
        void Add(string key, IDriverManager manager);
        void AddOrOverride(IDriverManager manager);
        void AddOrOverride(string key, IDriverManager manager);
        void Clear();
        bool Contains(string key);
        bool Contains(Type type);
        bool Contains<T>() where T : IDriverManager;
        int Count { get; }
        T GetDriver<T, U>() where U : IDriverManager;
        T GetDriver<T>(string key);
        T GetManager<T>() where T : IDriverManager;
        T GetManager<T>(string key) where T : IDriverManager;
        IDriverManager GetManager();
        IDriverManager GetManager(string key);
        bool Remove(string key);
        bool Remove(Type type);
    }
}