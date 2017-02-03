//--------------------------------------------------
// <copyright file="MessageType.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Message type enumeration</summary>
//--------------------------------------------------

namespace Magenic.MaqsFramework.Utilities.Logging
{
    /// <summary>
    /// The type of message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Error message
        /// </summary>
        SUSPENDED = -1,

        /// <summary>
        /// Error message
        /// </summary>
        ERROR = 0,

        /// <summary>
        /// Warning message
        /// </summary>
        WARNING = 1,

        /// <summary>
        /// Success message
        /// </summary>
        SUCCESS = 2,

        /// <summary>
        /// Generic message - Our default message type
        /// </summary>
        GENERIC = 3,

        /// <summary>
        /// Informational message
        /// </summary>
        INFORMATION = 4,

        /// <summary>
        /// Verbose message
        /// </summary>
        VERBOSE = 5
    }
}