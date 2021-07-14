//--------------------------------------------------
// <copyright file="Logger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Abstract logging interface</summary>
//--------------------------------------------------
using System;

namespace Magenic.Maqs.Utilities.Logging
{
    public interface ILogger : IDisposable
    {
        void ContinueLogging();
        MessageType GetLoggingLevel();
        void LogMessage(MessageType messageType, string message, params object[] args);
        void LogMessage(string message, params object[] args);
        void SetLoggingLevel(MessageType level);
        void SuspendLogging();
    }
}