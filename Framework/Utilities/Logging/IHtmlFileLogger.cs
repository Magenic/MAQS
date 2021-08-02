//--------------------------------------------------
// <copyright file="IHtmlFileLogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>HTML file logger interface</summary>
//--------------------------------------------------
namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    /// Inteface for HTML file logger
    /// </summary>
    public interface IHtmlFileLogger : IFileLogger
    {
        /// <summary>
        /// Embedd a base 64 image
        /// </summary>
        /// <param name="base64String">Base 64 image string</param>
        void EmbedImage(string base64String);
    }
}