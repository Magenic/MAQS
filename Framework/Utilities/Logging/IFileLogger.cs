//--------------------------------------------------
// <copyright file="IFileLogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>File logger interface</summary>
//--------------------------------------------------
namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    /// Interface for file logger
    /// </summary>
    public interface IFileLogger : ILogger
    {
        /// <summary>
        /// Gets or sets path to the log file
        /// </summary>
        string FilePath { get; set; }
    }
}