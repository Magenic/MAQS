//--------------------------------------------------
// <copyright file="MessageType.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Message type enumeration</summary>
//--------------------------------------------------

namespace Magenic.Maqs.Utilities.Logging
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
        /// Generic message
        /// </summary>
        GENERIC = 3,

        /// <summary>
        /// Step Message test engineer would insert
        /// </summary>
        STEP = 4,

        /// <summary>
        /// Action Message reflects actions a user would take
        /// </summary>
        ACTION = 5,

        /// <summary>
        /// Informational message - Our default message type
        /// </summary>
        INFORMATION = 6,

        /// <summary>
        /// Verbose message
        /// </summary>
        VERBOSE = 7
    }
}