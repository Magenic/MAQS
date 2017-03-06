//--------------------------------------------------
// <copyright file="EmailConnectionWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>The basic email interactions</summary>
//--------------------------------------------------
using AE.Net.Mail;
using AE.Net.Mail.Imap;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Magenic.MaqsFramework.BaseEmailTest
{
    /// <summary>
    /// Wraps the basic email interactions
    /// </summary>
    public class EmailConnectionWrapper : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConnectionWrapper" /> class
        /// </summary>
        /// <param name="host">The email server host</param>
        /// <param name="username">Email user name</param>
        /// <param name="password">Email user password</param>
        /// <param name="port">Email server port</param>
        /// <param name="serverTimeout">Timeout for the email server</param>
        /// <param name="isSSL">Should SSL be used</param>
        /// <param name="skipSslCheck">Skip the SSL check</param>
        public EmailConnectionWrapper(string host, string username, string password, int port, int serverTimeout = 10000, bool isSSL = true, bool skipSslCheck = false)
        {
            // Get the email connection and make sure it is live
            this.EmailConnection = new ImapClient(host, username, password, AuthMethods.Login, port, isSSL, skipSslCheck);
            this.EmailConnection.ServerTimeout = serverTimeout;

            this.DefaultToInboxIfExists();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailConnectionWrapper" /> class
        /// </summary>
        /// <param name="setupEmailBaseConnectionOverride">A function that returns the email connection</param>
        public EmailConnectionWrapper(Func<ImapClient> setupEmailBaseConnectionOverride)
        {
            this.EmailConnection = setupEmailBaseConnectionOverride();
            this.DefaultToInboxIfExists();
        }

        /// <summary>
        /// Gets the Imap email connection
        /// </summary>
        public ImapClient EmailConnection { get; private set; }

        /// <summary>
        /// Gets the current mailbox
        /// </summary>
        public string CurrentMailBox { get; private set; }

        /// <summary>
        /// Dispose of the database connection
        /// </summary>
        public virtual void Dispose()
        {
            if (this.EmailConnection == null)
            {
                return;
            }

            // Make sure the connection exists and is open before trying to close it
            if (this.EmailConnection.IsConnected)
            {
                this.EmailConnection.Logout();
                this.EmailConnection.Disconnect();
            }

            this.EmailConnection.Dispose();
        }

        /// <summary>
        /// Check if the account is accessible
        /// </summary>
        /// <returns>True if the email account is accessible</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="CanAccessEmail" lang="C#" />
        /// </example>
        public virtual bool CanAccessEmailAccount()
        {
            try
            {
                return this.EmailConnection.IsConnected && this.EmailConnection.GetMessageCount() > -2;
            }
            catch
            {
                // Something went wrong and we cannot access the account
                return false;
            }
        }

        #region Boxes

        /// <summary>
        /// Get the list of mailbox names
        /// </summary>
        /// <returns>A list of mailbox names</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetMailboxes" lang="C#" />
        /// </example>
        public virtual List<string> GetMailBoxNames()
        {
            List<string> mailBoxes = new List<string>();

            // Get all mailboxes
            foreach (Mailbox mailbox in this.EmailConnection.ListMailboxes(string.Empty, "*"))
            {
                mailBoxes.Add(mailbox.Name);
            }

            return mailBoxes;
        }

        /// <summary>
        /// Get a mailbox by name
        /// </summary>
        /// <param name="mailbox">The mailbox name</param>
        /// <returns>The mailbox</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetMailbox" lang="C#" />
        /// </example> 
        public virtual Mailbox GetMailbox(string mailbox)
        {
            this.CurrentMailBox = mailbox;
            return this.EmailConnection.SelectMailbox(mailbox);
        }

        /// <summary>
        /// Select a mailbox by name
        /// </summary>
        /// <param name="mailBox">The name of the mailbox</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="SelectMailbox" lang="C#" />
        /// </example> 
        public virtual void SelectMailbox(string mailBox)
        {
            this.CurrentMailBox = mailBox;
            this.EmailConnection.SelectMailbox(mailBox);
        }

        /// <summary>
        /// Create a mailbox
        /// </summary>
        /// <param name="newMailBox">The name of the new mailbox</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="CreateMailbox" lang="C#" />
        /// </example>
        public virtual void CreateMailbox(string newMailBox)
        {
            this.CurrentMailBox = newMailBox;
            this.EmailConnection.CreateMailbox(newMailBox);
        }

        #endregion

        #region Messages

        /// <summary>
        /// Get an email message
        /// </summary>
        /// <param name="mailBox">The mailbox in which to find the message</param>
        /// <param name="uid">The unique identifier for the email</param>
        /// <param name="headerOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The message</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetMessage" lang="C#" />
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetMessage1" lang="C#" />
        /// </example>
        public virtual MailMessage GetMessage(string mailBox, string uid, bool headerOnly = false, bool markRead = false)
        {
            this.SelectMailbox(mailBox);
            return this.GetMessage(uid, headerOnly, markRead);
        }

        /// <summary>
        /// Get an email message from the current mailbox
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <param name="headerOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The message</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetMessageUid" lang="C#" />
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetMessageUid1" lang="C#" />
        /// </example>
        public virtual MailMessage GetMessage(string uid, bool headerOnly = false, bool markRead = false)
        {
            return this.EmailConnection.GetMessage(uid, headerOnly, markRead);
        }

        /// <summary>
        /// Get a list of email messages from the current mailbox
        /// </summary>
        /// <returns>A list of email messages</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="EmailHeaders" lang="C#" />
        /// </example>        
        public virtual List<MailMessage> GetAllMessageHeaders()
        {
            return this.GetAllMessageHeaders(this.CurrentMailBox);
        }

        /// <summary>
        /// Get a list of email messages from the given mailbox
        /// </summary>
        /// <param name="mailBox">The mailbox in which to find the messages</param>
        /// <returns>A list of email messages</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="EmailHeadersMailbox" lang="C#" />
        /// </example>  
        public virtual List<MailMessage> GetAllMessageHeaders(string mailBox)
        {
            this.SelectMailbox(mailBox);
            return this.SearchMessages(SearchCondition.From("*"));
        }

        /// <summary>
        /// Delete the given email
        /// </summary>
        /// <param name="message">The email with to delete</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="DeleteMessage" lang="C#" />
        /// </example>
        public virtual void DeleteMessage(MailMessage message)
        {
            this.EmailConnection.DeleteMessage(message.Uid);
        }

        /// <summary>
        /// Delete the email with the given unique identifier
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="DeleteMessageUid" lang="C#" />
        /// </example>
        public virtual void DeleteMessage(string uid)
        {
            this.EmailConnection.DeleteMessage(uid);
        }

        /// <summary>
        /// Move the given email to the destination mailbox
        /// </summary>
        /// <param name="message">The email</param>
        /// <param name="destinationMailbox">The destination mailbox</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="MoveMessage" lang="C#" />
        /// </example>  
        public virtual void MoveMailMessage(MailMessage message, string destinationMailbox)
        {
            this.MoveMailMessage(message.Uid, destinationMailbox);
        }

        /// <summary>
        /// Move the email with the given unique identifier to the destination mailbox
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <param name="destinationMailbox">The destination mailbox</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="MoveMessage" lang="C#" />
        /// </example>
        public virtual void MoveMailMessage(string uid, string destinationMailbox)
        {
            try
            {
                this.EmailConnection.MoveMessage(uid, destinationMailbox);
            }
            catch
            {
                // Handle gmail move bug
                Thread.Sleep(1000);
                this.EmailConnection.MoveMessage(uid, destinationMailbox);
                Thread.Sleep(100);
            }
        }

        #endregion

        #region attachments

        /// <summary>
        /// Get the list of attachments for the email with the given unique identifier
        /// </summary>
        /// <param name="uid">The unique identifier for the email</param>
        /// <returns>The list of </returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="AttachmentsByUid" lang="C#" />
        /// </example>
        public virtual List<Attachment> GetAttachments(string uid)
        {
            return this.GetAttachments(this.CurrentMailBox, uid);
        }

        /// <summary>
        /// Get the list of attachments for the email with the given unique identifier in the given mailbox
        /// </summary>
        /// <param name="mailBox">The mailbox</param>
        /// <param name="uid">The unique identifier for the email</param>
        /// <returns>The list of attachments</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="AttachmentsByMailboxAndUid" lang="C#" />
        /// </example>
        public virtual List<Attachment> GetAttachments(string mailBox, string uid)
        {
            this.SelectMailbox(mailBox);
            return this.GetAttachments(this.GetMessage(uid));
        }

        /// <summary>
        /// Get the list of attachments for the email with the given message
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The list of attachments</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetAttachmentsMessage" lang="C#" />
        /// </example>
        public virtual List<Attachment> GetAttachments(MailMessage message)
        {
            return message.Attachments.ToList();
        }

        /// <summary>
        /// Download all the attachments for the given message
        /// </summary>
        /// <param name="message">The email</param>
        /// <returns>List of file paths for the downloaded files</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="DownloadAttachments" lang="C#" />
        /// </example>
        public virtual List<string> DownLoadAttachments(MailMessage message)
        {
            return this.DownLoadAttachments(message, EmailConfig.GetAttachmentDownloadDirectory());
        }

        /// <summary>
        /// Download all the attachments for the given message to a specific folder
        /// </summary>
        /// <param name="message">The email</param>
        /// <param name="downloadFolder">The download folder</param>
        /// <returns>List of file paths for the downloaded files</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="DownloadAttachmentsToLocation" lang="C#" />
        /// </example>
        public virtual List<string> DownLoadAttachments(MailMessage message, string downloadFolder)
        {
            // Create the download folder if id does not already exist
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }

            List<string> paths = new List<string>();

            foreach (Attachment attachment in message.Attachments)
            {
                string destination = Path.Combine(downloadFolder, attachment.Filename);
                attachment.Save(destination);
                paths.Add(destination);
            }

            return paths;
        }

        #endregion

        #region Search

        /// <summary>
        /// Get a list of messages from a specific Mailbox sent since a specific day - Only the date is used in the search, hour and below are ignored
        /// </summary>
        /// <param name="mailBox">The mailbox</param>
        /// <param name="time">The day to search since</param>
        /// <param name="headersOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The list of messages that match the search criteria</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="MessagesSinceByMailbox" lang="C#" />
        /// </example>
        public virtual List<MailMessage> SearchMessagesSince(string mailBox, DateTime time, bool headersOnly = true, bool markRead = false)
        {
            this.SelectMailbox(mailBox);
            return this.SearchMessagesSince(time, headersOnly, markRead);
        }

        /// <summary>
        /// Get a list of messages sent since a specific day - Only the date is used in the search, hour and below are ignored
        /// </summary>
        /// <param name="time">The day to search since</param>
        /// <param name="headersOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The list of messages that match the search criteria</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="MessageSince" lang="C#" />
        /// </example>
        public virtual List<MailMessage> SearchMessagesSince(DateTime time, bool headersOnly = true, bool markRead = false)
        {
            // Add google hack
            if (this.EmailConnection.Host.ToLower().Equals("imap.gmail.com"))
            {
                SearchCondition condition = new SearchCondition();
                condition.Value = string.Format(@"X-GM-RAW ""AFTER:{0:yyyy-MM-dd}""", SearchCondition.SentSince(time).Value);
                return this.SearchMessages(condition, headersOnly, markRead);
            }
            else
            {
                return this.SearchMessages(SearchCondition.SentSince(time), headersOnly, markRead);
            }
        }

        /// <summary>
        /// Get a list of messages that meet the search criteria
        /// </summary>
        /// <param name="mailBox">The mailbox</param>
        /// <param name="condition">The search condition</param>
        /// <param name="headersOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The list of messages that match the search criteria</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="SearchMessages1" lang="C#" />
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="SearchMessages2" lang="C#" />
        /// </example>
        public virtual List<MailMessage> SearchMessages(string mailBox, SearchCondition condition, bool headersOnly = true, bool markRead = false)
        {
            this.SelectMailbox(mailBox);
            return this.SearchMessages(condition, headersOnly, markRead);
        }

        /// <summary>
        /// Get a list of messages that meet the search criteria
        /// </summary>
        /// <param name="condition">The search condition</param>
        /// <param name="headersOnly">Only get header data</param>
        /// <param name="markRead">Mark the email as read</param>
        /// <returns>The list of messages that match the search criteria</returns>
        /// /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="SearchMessages1" lang="C#" />
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="SearchMessages2" lang="C#" />
        /// </example>
        public virtual List<MailMessage> SearchMessages(SearchCondition condition, bool headersOnly = true, bool markRead = false)
        {
            object[] args = { condition, headersOnly, markRead };
            return GenericWait.WaitFor<List<MailMessage>, object[]>(this.GetSearchResults, args);
        }
        #endregion

        /// <summary>
        /// Get the list of content types for the given message
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>List of content types</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetContentTypes" lang="C#" />
        /// </example>
        public virtual List<string> GetContentTypes(MailMessage message)
        {
            List<string> types = new List<string>();

            foreach (Attachment attachment in message.AlternateViews)
            {
                types.Add(attachment.ContentType);
            }

            return types;
        }

        /// <summary>
        /// Get the email body for the given message that matches the content type
        /// </summary>
        /// <param name="message">The message</param>
        /// <param name="contentType">The content type</param>
        /// <returns>The message body that matches the content type</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetBodyByContent" lang="C#" />
        /// </example>
        public virtual string GetBodyByContentTypes(MailMessage message, string contentType)
        {
            foreach (Attachment attachment in message.AlternateViews)
            {
                if (attachment.ContentType.Equals(contentType, StringComparison.CurrentCultureIgnoreCase))
                {
                    return attachment.Body;
                }
            }

            throw new Exception(StringProcessor.SafeFormatter("Failed to find content type '{0}'", contentType));
        }

        /// <summary>
        /// Get the list of emails for a search
        /// </summary>
        /// <param name="args">The search condition followed by the header only and set as seen booleans</param>
        /// <returns>The list of mail message that match the search</returns>
        private List<MailMessage> GetSearchResults(params object[] args)
        {
            List<MailMessage> messageList = new List<MailMessage>();
            foreach (Lazy<MailMessage> message in this.EmailConnection.SearchMessages((SearchCondition)args[0], (bool)args[1], (bool)args[2]))
            {
                messageList.Add(message.Value);
            }

            foreach (MailMessage message in messageList)
            {
                if (message.Subject == null)
                {
                    throw new Exception("Invalid results - found null subject");
                }
            }

            return messageList;
        }

        /// <summary>
        /// Set the default mailbox - Use inbox by default
        /// </summary>
        private void DefaultToInboxIfExists()
        {
            Mailbox[] mailboxes = this.EmailConnection.ListMailboxes(string.Empty, "*");

            foreach (Mailbox mailbox in mailboxes)
            {
                if (mailbox.Name.Equals("Inbox", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.SelectMailbox(mailbox.Name);
                    return;
                }
            }

            this.SelectMailbox(mailboxes[0].Name);
        }
    }
}
