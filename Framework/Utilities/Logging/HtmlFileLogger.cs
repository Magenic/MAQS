﻿//--------------------------------------------------
// <copyright file="HtmlFileLogger.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Writes event logs to HTML file</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using System;
using System.IO;
using System.Web;

namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    /// Helper class for adding logs to an HTML file. Allows configurable file path.
    /// </summary>
    public class HtmlFileLogger : FileLogger, IHtmlFileLogger
    {
        /// <summary>
        /// The default log name
        /// </summary>
        private const string DEFAULTLOGNAME = "FileLog.html";

        /// <summary>
        /// Document Start and contains the references to the CDN's
        /// </summary>
        private const string DEFUALTCDNTAGS = "<!DOCTYPE html><html lang='en'><head><meta charset='utf-8'><meta name='viewport' content='width=device-width, initial-scale=1'><title>{0}</title><link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.1.1/css/bootstrap.min.css'> <script src='https://code.jquery.com/jquery-3.4.1.slim.min.js' integrity='sha384-J6qa4849blE2+poT4WnyKhv5vZF5SrPo0iEjwBvKU7imGFAV0wwj1yYfoRSJoZ+n' crossorigin='anonymous'></script> <script src='https://cdn.jsdelivr.net/npm/popper.js@1.16.0/dist/umd/popper.min.js' integrity='sha384-Q6E9RHvbIyZFJoft+2mJbHaEWldlvI9IOYy5n3zV9zzTtmI3UksdQRVvoxMfooAo' crossorigin='anonymous'></script> <script src='https://stackpath.bootstrapcdn.com/bootstrap/4.4.1/js/bootstrap.min.js' integrity='sha384-wfSDF2E50Y2D1uUdj0O3uMBJnjuUD4Ih7YwaYd1iqfktj0Uod8GCExl3Og8ifwB6' crossorigin='anonymous'></script> <script src='https://use.fontawesome.com/releases/v5.0.8/js/all.js'></script> </head><body>";

        /// <summary>
        /// Contains the CSS tags, and the JQuery Scripts
        /// </summary>
        private const string SCRIPTANDCSSTAGS = "<style>.modal-dialog{max-width: fit-content} p{white-space: pre-wrap} .pop{ cursor: pointer; }</style><script>$(function (){$('.pop').on('click', function (e){$('.imagepreview').attr('src', $(this).find('img').attr('src'));$('#imagemodal').modal('show');});});</script><script>$(function (){$('.pop2').on('click', function (){$('.imagepreview').attr('src', $(this).attr('src'));$('#imagemodal').modal('show');});});</script><script>$(function (){$('.dropdown-item').on('click', function (e){$(this).attr('class', function (i, old){return old=='dropdown-item' ? 'dropdown-item bg-secondary' :'dropdown-item';});var temp=$(this).data('name');$('[data-logtype=\\\'' + temp + '\\\']').toggleClass('show');e.stopPropagation();});});</script><script>$(function (){$(document).ready(function(){$('#Header').append(' ' + $('title').text())})});</script> <script>$(function(){$(document).ready(function(){$('.card-text:contains(\\\'Test\\\')').each(function (index, element){switch($(this).text()){case 'Test passed': $('#TestResult:not([class])').addClass('text-success'); case 'Test failed': $('#TestResult:not([class])').addClass('text-danger'); case 'Test was inconclusive': $('#TestResult:not([class])').addClass('text-danger'); $('#TestResult').text($(this).text()); $('#TestResult').addClass('font-weight-bold'); break;}})})}) </script>";

        /// <summary>
        /// Contains the FIlter By dropdown
        /// </summary>
        private const string FILTERDROPDOWN = "<div id='Header' class='dropdown'><button class='btn btn-secondary dropdown-toggle' type='button' id='FilterByDropdown' data-toggle='dropdown'aria-haspopup='true' aria-expanded='false'>Filter By</button><div class='dropdown-menu' aria-labelledby='FilterByDropdown'><button class='dropdown-item' data-name='ERROR'>Filter Error</button><button class='dropdown-item' data-name='WARNING'>Filter Warning</button><button class='dropdown-item' data-name='SUCCESS'>Filter Success</button><button class='dropdown-item' data-name='GENERIC'>Filter Generic</button><button class='dropdown-item' data-name='STEP'>Filter Step</button><button class='dropdown-item' data-name='ACTION'>Filter Action</button><button class='dropdown-item' data-name='INFORMATION'>Filter Information</button><button class='dropdown-item' data-name='VERBOSE'>Filter Verbose</button><button class='dropdown-item' data-name='IMAGE'>Filter Images</button></div><span id='TestResult'></span></div>";

        /// <summary>
        /// Contains the Modal Div that is needed to make a larger image
        /// </summary>
        private const string MODALDIV = "<div class='modal fade' id='imagemodal' tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'> <div class='modal-content'> <div class='modal-body'><button type='button' class='close' data-dismiss='modal'><span aria-hidden='true'>&times;</span><span class='sr-only'>Close</span></button> <img src='' class='imagepreview' style='width: 100%;' ></div></div></div></div>";

        /// <summary>
        /// The begininning to the cards section
        /// </summary>
        private const string CARDSTART = "<div class='container-fluid'><div class='row'>";

        /// <summary>
        /// Image DIV
        /// </summary>
        private const string IMAGEDIV = "<div class='collapse col-12 show' data-logtype='IMAGE'><div class='card'><div class='card-body'><h5 class='card-title mb-1'>IMAGE</h5><h6 class='card-subtitle mb-1'>{0}</h6></div><a class='pop'><img class='card-img-top rounded' src='data:image/png;base64, {1}'style='width: 200px;'></a></div></div>";

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
                InsertHtml($"<div class='collapse col-12 show' data-logtype='{messageType}'><div class='card'><div class='card-body {GetTextWithColorFlag(messageType)}'><h5 class='card-title mb-1'>{messageType}</h5><h6 class='card-subtitle mb-1'>{CurrentDateTime()}</h6><p class='card-text'>{HttpUtility.HtmlEncode(StringProcessor.SafeFormatter(message, args))}</p></div></div></div>");

            }
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as a generic message
        /// </summary>
        /// <param name="html">Html content</param>
        private void InsertHtml(string html)
        {
            // Log the message
            lock (this.FileLock)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(this.FilePath, true))
                    {
                        writer.Write(html);
                    }
                }
                catch (Exception e)
                {
                    // Failed to write to the event log, write error to the console instead
                    ConsoleLogger console = new ConsoleLogger();
                    console.LogMessage(MessageType.ERROR, $"Failed to write to event log because: {e}{Environment.NewLine}Content: {html}");
                }
            }
        }

        /// <summary>
        /// Embedd a base 64 image
        /// </summary>
        /// <param name="base64String">Base 64 image string</param>
        public void EmbedImage(string base64String)
        {
            InsertHtml(StringProcessor.SafeFormatter(IMAGEDIV, CurrentDateTime(), base64String));
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        /// <param name="disposing">True if you want to release managed resources</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && File.Exists(this.FilePath))
            {
                var writer = new StreamWriter(this.FilePath, true);

                writer.Write(MODALDIV);
                writer.Write("</div></body></html>");
                writer.Flush();
                writer.Close();
            }

            base.Dispose(disposing);
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
                    return "bg-secondary";
                case MessageType.ACTION:
                    return "text-info";
                case MessageType.STEP:
                    return "text-primary";
                case MessageType.ERROR:
                    return "text-danger";
                case MessageType.GENERIC:
                    return string.Empty;
                case MessageType.INFORMATION:
                    return "text-secondary";
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