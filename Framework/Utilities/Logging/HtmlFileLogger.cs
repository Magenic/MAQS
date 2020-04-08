//--------------------------------------------------
// <copyright file="HtmlFileLogger.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to HTML file</summary>
//--------------------------------------------------
using System;
using System.Globalization;
using System.IO;
using Magenic.Maqs.Utilities.Data;
using System.Web;

namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    /// Helper class for adding logs to an HTML file. Allows configurable file path.
    /// </summary>
    public class HtmlFileLogger : FileLogger, IDisposable
    {
        /// <summary>
        /// The default log name
        /// </summary>
        private const string DEFAULTLOGNAME = "FileLog.html";

        /// <summary>
        /// Default header for the HTML file, this gives us our colored text
        /// </summary>
        private const string DEFAULTHTMLHEADER =
            "<!DOCTYPE html><html><header><title>Test Log</title></header><body>";

        private const string DEFUALTCDNTAGS = "<!DOCTYPE html><html lang='en'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'><title>{0}</title><link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css'> <script src='https://code.jquery.com/jquery-3.4.1.slim.min.js' integrity='sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n' crossorigin='anonymous'></script> <script src='https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js' integrity='sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo' crossorigin='anonymous'></script> <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js' integrity='sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6' crossorigin='anonymous'></script> <script src='https://use.fontawesome.com/releases/v5.0.8/js/all.js'></script> </head><body>";

        private const string SCRIPTANDCSSTAGS = "<style>.modal-dialog{max-width: fit-content}</style><script>$(function (){$('.pop').on('click', function (e){$('.imagepreview').attr('src', $(this).find('img').attr('src'));$('#imagemodal').modal('show');});});</script><script>$(function (){$('.pop2').on('click', function (){$('.imagepreview').attr('src', $(this).attr('src'));$('#imagemodal').modal('show');});});</script><script>$(function (){$('.dropdown-item').on('click', function (e){$(this).attr('class', function (i, old){return old=='dropdown-item' ? 'dropdown-item bg-secondary' :'dropdown-item';});var temp=$(this).data('name');$('[data-logtype=\\\'' + temp + '\\\']').toggleClass('show');e.stopPropagation();});});</script><script>$(function (){$(document).ready(function(){$('#AssertsRan').text($('#AssertCount').data('assertsran'));$('#AssertsPassed').text($('#AssertCount').data('assertspassed'));$('#AssertsFailed').text($('#AssertCount').data('assertsfailed'));$('.progress-bar.bg-success').width(parseInt($('#AssertCount').data('assertspassed'))/parseInt($('#AssertCount').data('assertsran'))*100 + '%');$('.progress-bar.bg-danger').width(parseInt($('#AssertCount').data('assertsfailed'))/parseInt($('#AssertCount').data('assertsran'))*100 + '%');});});</script>";

        private const string FILTERDROPDOWN = "<div class='dropdown'><button class='btn btn-secondary dropdown-toggle' type='button' id='FilterByDropdown' data-toggle='dropdown'aria-haspopup='true' aria-expanded='false'>Filter By</button><div class='dropdown-menu' aria-labelledby='FilterByDropdown'><button class='dropdown-item bg-secondary' data-name='ERROR'>Filter Error</button><button class='dropdown-item bg-secondary' data-name='WARNING'>Filter Warning</button><button class='dropdown-item bg-secondary' data-name='SUCCESS'>Filter Success</button><button class='dropdown-item' data-name='GENERIC'>Filter Generic</a><button class='dropdown-item' data-name='STEP'>Filter Step</button><button class='dropdown-item' data-name='ACTION'>Filter Action</button><button class='dropdown-item' data-name='INFORMATION'>Filter Information</button><button class='dropdown-item' data-name='VERBOSE'>Filter Verbose</button><button class='dropdown-item bg-secondary' data-name='IMAGE'>Filter Images</button></div></div></div>";

        private const string CARDSTART = "<div class='containter-fluid'><div class='row'>";
        /// <summary>
        /// Initializes a new instance of the HtmlFileLogger class
        /// </summary>
        /// <param name="logFolder">Where log files should be saved</param>
        /// <param name="name">File Name</param>
        /// <param name="messageLevel">Messaging level</param>
        /// <param name="append">True to append to an existing log file or false to overwrite it - If the file does not exist this, flag will have no affect</param>
        public HtmlFileLogger(string logFolder = "", string name = DEFAULTLOGNAME, MessageType messageLevel = MessageType.INFORMATION, bool append = false)
            : base(logFolder, name, messageLevel, append)
        {
            StreamWriter writer = new StreamWriter(this.FilePath, true);
            writer.Write(String.Format(DEFUALTCDNTAGS, Path.GetFileNameWithoutExtension(this.FilePath)));
            writer.Write(SCRIPTANDCSSTAGS);
            writer.Write(FILTERDROPDOWN);
            writer.Write(CARDSTART);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Gets the file extension
        /// </summary>
        protected override string Extension
        {
            get { return ".html"; }
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as a generic message
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public override void LogMessage(MessageType messageType, string message, params object[] args)
        {
            // If the message level is greater that the current log level then do not log it.
            if (this.ShouldMessageBeLogged(messageType))
            {
                // Log the message
                lock (this.FileLock)
                {
                    string date = DateTime.UtcNow.ToString(Logger.DEFAULTDATEFORMAT, CultureInfo.InvariantCulture);

                    try
                    {
                        using (StreamWriter writer = new StreamWriter(this.FilePath, true))
                        {
                            writer.Write(StringProcessor.SafeFormatter(
                                "<div class='collapse col-12' data-logtype='{0}'><div class='card'><div class='card-body {1}'><h5 class='card-title mb-1'>{0}</h5><h6 class='card-subtitle mb-1'>{2}</h6><p class='card-text'>{3}</p></div></div></div>", 
                                messageType.ToString(),
                                GetTextWithColorFlag(messageType),
                                date,
                                HttpUtility.HtmlEncode(StringProcessor.SafeFormatter(message, args))));
                        }   
                    }
                    catch (Exception e)
                    {
                        // Failed to write to the event log, write error to the console instead
                        ConsoleLogger console = new ConsoleLogger();
                        console.LogMessage(MessageType.ERROR, StringProcessor.SafeFormatter("Failed to write to event log because: {0}", e.Message));
                        console.LogMessage(messageType, message, args);
                    }
                }
            }
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        /// <param name="disposing">True if you want to release managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && File.Exists(this.FilePath))
            {
                var writer = new StreamWriter(this.FilePath, true);

                writer.WriteLine("</div></body></html>");
                writer.Flush();
                writer.Close();
            }
        }

        /// <summary>
        /// Get the HTML style key for the given message type
        /// </summary>
        /// <param name="type">The message type</param>
        /// <returns>string - The HTML style key for the given message type</returns>
        private string GetTextWithColorFlag(MessageType type)
        {
            switch (type)
            {
                case MessageType.VERBOSE:
                    return "text-secondary";
                case MessageType.ACTION:
                    return "text-primary";
                case MessageType.STEP:
                    return "bg-secondary";
                case MessageType.ERROR:
                    return "text-danger";
                case MessageType.GENERIC:
                    return string.Empty;
                case MessageType.INFORMATION:
                    return "text-info";
                case MessageType.SUCCESS:
                    return "text-success";
                case MessageType.WARNING:
                    return "text-warning";
                default:
                    Console.WriteLine(this.UnknownMessageTypeMessage(type));
                    return "text-white bg-dark";
            }
        }
    }
}