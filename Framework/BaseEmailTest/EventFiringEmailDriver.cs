//--------------------------------------------------
// <copyright file="EventFiringEmailDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The basic database interactions</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Wraps the basic database interactions
    /// </summary>
    public class EventFiringEmailDriver : EmailDriver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringEmailDriver" /> class
        /// </summary>
        /// <param name="host">The email server host</param>
        /// <param name="username">Email user name</param>
        /// <param name="password">Email user password</param>
        /// <param name="port">Email server port</param>
        /// <param name="serverTimeout">Timeout for the email server</param>
        /// <param name="isSSL">Should SSL be used</param>
        /// <param name="skipSslCheck">Skip the SSL check</param>
        public EventFiringEmailDriver(string host, string username, string password, int port, int serverTimeout = 10000, bool isSSL = true, bool skipSslCheck = false)
            : base(host, username, password, port, serverTimeout, isSSL, skipSslCheck)
        {
            this.OnEvent(StringProcessor.SafeFormatter("Connect to email with user '{0}' on host '{1}', port '{2}'", username, host, port));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventFiringEmailDriver" /> class
        /// </summary>
        /// <param name="setupEmailBaseConnectionOverride">A function that returns the email connection</param>
        public EventFiringEmailDriver(Func<ImapClient> setupEmailBaseConnectionOverride)
            : base(setupEmailBaseConnectionOverride)
        {
            this.OnEvent(StringProcessor.SafeFormatter("Connect to email with function '{0}'", setupEmailBaseConnectionOverride.Method.Name));
        }

        /// <summary>
        /// Email event
        /// </summary>
        public event EventHandler<string> EmailEvent;

        /// <summary>
        /// Email error event
        /// </summary>
        public event EventHandler<string> EmailErrorEvent;

        /// <summary>
        /// Check if the account is accessible
        /// </summary>
        /// <returns>True if the email account is accessible</returns>
        public override bool CanAccessEmailAccount()
        {
            try
            {
                bool canAccess = base.CanAccessEmailAccount();
                this.OnEvent(StringProcessor.SafeFormatter("Access account check returned {0}", canAccess));
                return canAccess;
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        #region Boxes

        /// <summary>
        /// Get the list of mailbox names
        /// </summary>
        /// <returns>A list of mailbox names</returns>
        /// /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="GetMailboxes" lang="C#" />
        /// </example>
        public override List<string> GetMailBoxNames()
        {
            try
            {
                this.OnEvent("Get mailbox names");
                return base.GetMailBoxNames();
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get a mailbox by name
        /// </summary>
        /// <param name="mailbox">The mailbox name</param>
        /// <returns>The mailbox</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="GetMailbox" lang="C#" />
        /// </example> 
        public override IMailFolder GetMailbox(string mailbox)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Get mailbox named '{0}'", mailbox));
                return base.GetMailbox(mailbox);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Select a mailbox by name
        /// </summary>
        /// <param name="mailbox">The name of the mailbox</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="SelectMailbox" lang="C#" />
        /// </example> 
        public override void SelectMailbox(string mailbox)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Select mailbox named '{0}'", mailbox));
                base.SelectMailbox(mailbox);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Create a mailbox
        /// </summary>
        /// <param name="newMailBox">The name of the new mailbox</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="CreateMailbox" lang="C#" />
        /// </example>
        public override void CreateMailbox(string newMailBox)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Create mailbox named '{0}'", newMailBox));
                base.CreateMailbox(newMailBox);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        #endregion

        #region Messages

        /// <summary>
        /// Get an email message from the current mailbox
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <param name="headerOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The message</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="GetMessage" lang="C#" />
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="GetMessage1" lang="C#" />
        /// </example>
        public override MimeMessage GetMessage(string uid, bool headerOnly = false, bool markRead = false)
        {
            try
            {
                this.OnEvent(
                    StringProcessor.SafeFormatter("Get message with uid '{0}',  get header only '{1}' and mark as read '{2}'", uid, headerOnly, markRead));
                return base.GetMessage(uid, headerOnly, markRead);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get a list of email messages from the given mailbox
        /// </summary>
        /// <param name="mailBox">The mailbox in which to find the messages</param>
        /// <returns>A list of email messages</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="EmailHeadersMailbox" lang="C#" />
        /// </example>   
        public override List<MimeMessage> GetAllMessageHeaders(string mailBox)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Get all message headers from '{0}'", mailBox));
                return base.GetAllMessageHeaders(mailBox);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Delete the given email
        /// </summary>
        /// <param name="message">The email with to delete</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithDriver.cs" region="DeleteMessage" lang="C#" />
        /// </example>
        public override void DeleteMessage(MimeMessage message)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Delete message '{0}' from '{1}' recived '{2}'", message.Subject, message.From, message.Date));
                base.DeleteMessage(message);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Delete the email with the given unique identifier
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="DeleteMessageUid" lang="C#" />
        /// </example>
        public override void DeleteMessage(string uid)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Delete message with uid '{0}' from mailbox '{1}'", uid, this.CurrentMailBox));
                base.DeleteMessage(uid);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Move the given email to the destination mailbox
        /// </summary>
        /// <param name="message">The email</param>
        /// <param name="destinationMailbox">The destination mailbox</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="MoveMessage" lang="C#" />
        /// </example>  
        public override void MoveMailMessage(MimeMessage message, string destinationMailbox)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Move message '{0}' from '{1}' recived '{2}' to mailbox '{3}'", message.Subject, message.From.ToString(), message.Date.ToString(), destinationMailbox));
                base.MoveMailMessage(message, destinationMailbox);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Move the email with the given unique identifier to the destination mailbox
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <param name="destinationMailbox">The destination mailbox</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="MoveMessage" lang="C#" />
        /// </example>
        public override void MoveMailMessage(string uid, string destinationMailbox)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Move message with uid '{0}' from mailbox '{1}' to mailbox '{2}'", uid, this.CurrentMailBox, destinationMailbox));
                base.MoveMailMessage(uid, destinationMailbox);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }
        #endregion

        #region Attachments

        /// <summary>
        /// Get the list of attachments for the email with the given unique identifier
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <returns>The list of </returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="AttachmentsByUid" lang="C#" />
        /// </example>
        public override List<MimeEntity> GetAttachments(string uid)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Get list of attachments for message with uid '{0}' in mailbox '{1}'", uid, this.CurrentMailBox));
                return base.GetAttachments(uid);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get the list of attachments for the email with the given message
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The list of attachments</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="GetAttachmentsMessage" lang="C#" />
        /// </example>
        public override List<MimeEntity> GetAttachments(MimeMessage message)
        {
            try
            {
                this.OnEvent(
                    StringProcessor.SafeFormatter("Get list of attachments for message '{0}' from '{1}' recived '{2}' in mailbox '{3}'", message.Subject, message.From, message.Date, this.CurrentMailBox));
                return base.GetAttachments(message);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Download all the attachments for the given message
        /// </summary>
        /// <param name="message">The email</param>
        /// <param name="downloadFolder">The download folder</param>
        /// <returns>List of file paths for the downloaded files</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="DownloadAttachmentsToLocation" lang="C#" />
        /// </example>
        public override List<string> DownloadAttachments(MimeMessage message, string downloadFolder)
        {
            try
            {
                this.OnEvent(
                    StringProcessor.SafeFormatter("Download attachments for message '{0}' from '{1}' recived '{2}' in mailbox '{3}' to '{4}'", message.Subject, message.From, message.Date, this.CurrentMailBox, downloadFolder));
                return base.DownloadAttachments(message, downloadFolder);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        #endregion

        #region Search

        /// <summary>
        /// Get a list of messaged that meet the search criteria
        /// </summary>
        /// <param name="condition">The search condition</param>
        /// <param name="headersOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The list of messages that match the search criteria</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="MessageSince" lang="C#" />
        /// </example>
        public override List<MimeMessage> SearchMessages(SearchQuery condition, bool headersOnly = true, bool markRead = false)
        {
            try
            {
                this.OnEvent(
                    StringProcessor.SafeFormatter("Search for messages in mailbox '{0}' with search condition '{1}', header only '{2}' and mark as read '{3}'", this.CurrentMailBox, condition, headersOnly, markRead));
                return base.SearchMessages(condition, headersOnly, markRead);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        #endregion

        /// <summary>
        /// Get the list of content types for the given message
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>List of content types</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="GetContentTypes" lang="C#" />
        /// </example>
        public override List<string> GetContentTypes(MimeMessage message)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Get list of content types for message '{0}' from '{1}' recived '{2}'", message.Subject, message.From, message.Date));
                return base.GetContentTypes(message);
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get the email body for the given message that matches the content type
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="contentType">The content type</param>
        /// <returns>The message body that matches the content type</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithDriver.cs" region="GetBodyByContent" lang="C#" />
        /// </example>
        public override string GetBodyByContentTypes(MimeMessage message, string contentType)
        {
            try
            {
                this.OnEvent(StringProcessor.SafeFormatter("Get '{0}' content for message '{1}' from '{2}' recived '{3}'", contentType, message.Subject, message.From, message.Date));
                string body = base.GetBodyByContentTypes(message, contentType);
                this.OnEvent(StringProcessor.SafeFormatter("Got message body:\r\n{0}", body));
                return body;
            }
            catch (Exception ex)
            {
                this.RaiseErrorMessage(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Email event
        /// </summary>
        /// <param name="message">The event message</param>
        protected virtual void OnEvent(string message)
        {
            this.EmailEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Email error event
        /// </summary>
        /// <param name="message">The event error message</param>
        protected virtual void OnErrorEvent(string message)
        {
            this.EmailErrorEvent?.Invoke(this, message);
        }

        /// <summary>
        /// Raise an exception message
        /// </summary>
        /// <param name="e">The exception</param>
        private void RaiseErrorMessage(Exception e)
        {
            this.OnErrorEvent(StringProcessor.SafeFormatter("Failed because: {0}{1}{2}", e.Message, Environment.NewLine, e.ToString()));
        }
    }
}
