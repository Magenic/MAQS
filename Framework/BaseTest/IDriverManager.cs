//--------------------------------------------------
// <copyright file="DriverManager.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base driver manager</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;

namespace Magenic.Maqs.BaseTest
{
    public interface IDriverManager
    {
        ILogger Log { get; }

        void Dispose();

        object Get();

        bool IsDriverIntialized();
    }
}