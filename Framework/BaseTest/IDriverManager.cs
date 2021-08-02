//--------------------------------------------------
// <copyright file="IDriverManager.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Base driver manager interface</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;

namespace Magenic.Maqs.BaseTest
{
    /// <summary>
    /// Interface for base driver manager
    /// </summary>
    public interface IDriverManager
    {
        /// <summary>
        /// Gets the testing object
        /// </summary>
        ILogger Log { get; }

        /// <summary>
        /// Cleanup the driver
        /// </summary>
        void Dispose();

        /// <summary>
        /// Get the driver
        /// </summary>
        /// <returns>The driver</returns>
        object Get();

        /// <summary>
        /// Check if the underlying driver has been initialized
        /// </summary>
        /// <returns>True if the underlying driver has already been initialized</returns>
        bool IsDriverIntialized();
    }
}