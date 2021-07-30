//--------------------------------------------------
// <copyright file="HtmlFileLogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to HTML file</summary>
//--------------------------------------------------
namespace Magenic.Maqs.Utilities.Logging
{
    public interface IHtmlFileLogger : IFileLogger
    {
        void EmbedImage(string base64String);
    }
}