//--------------------------------------------------
// <copyright file="FileLogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to plain text file</summary>
//--------------------------------------------------
namespace Magenic.Maqs.Utilities.Logging
{
    public interface IFileLogger : ILogger
    {
        string FilePath { get; set; }
    }
}