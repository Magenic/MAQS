//--------------------------------------------------
// <copyright file="ILogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Abstract logging interface</summary>
//--------------------------------------------------
using System;
using System.IO;

namespace Magenic.Maqs.Utilities.Logging
{
    public interface ILogger : IDisposable
    {
        void ContinueLogging();
        MessageType GetLoggingLevel();
        void LogMessage(MessageType messageType, string message, params object[] args);
        void LogMessage(string message, params object[] args);
        void SetLoggingLevel(MessageType level);
        string CurrentDateTime();
        void SuspendLogging();
    }
}