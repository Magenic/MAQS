//--------------------------------------------------
// <copyright file="EmailConnectionWrapper.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>The basic email interactions</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            var client = new ImapClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => skipSslCheck
            };
            client.Connect(host, port, isSSL);
            client.Authenticate(username, password);
            client.Timeout = serverTimeout;
            this.EmailConnection = client;

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
        /// Gets the current folder
        /// </summary>
        public IMailFolder CurrentFolder { get; private set; }

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
                this.EmailConnection.Disconnect(true);
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
                return this.EmailConnection.IsConnected && this.GetCurrentFolder().Count > -2;
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
            foreach (IMailFolder mailbox in this.EmailConnection.GetFolders(this.BaseNamespace()))
            {
                mailBoxes.Add(mailbox.FullName);
            }

            return mailBoxes;
        }

        /// <summary>
        /// Get the list of mailbox names in a specific namespace
        /// </summary>
        /// /// <param name="folderNamespace">The folderNamespace</param>
        /// <returns>A list of mailbox names in a specific namespace</returns>
        public virtual List<string> GetMailBoxNamesInNamespace(FolderNamespace folderNamespace)
        {
            List<string> mailBoxes = new List<string>();

            // Get all mailboxes in folderNamespace
            foreach (IMailFolder mailbox in this.EmailConnection.GetFolders(folderNamespace))
            {
                mailBoxes.Add(mailbox.FullName);
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
        public virtual IMailFolder GetMailbox(string mailbox)
        {
            return GenericWait.WaitFor<IMailFolder>(() =>
            {
                this.CurrentMailBox = mailbox;
                this.CurrentFolder = this.EmailConnection.GetFolder(mailbox);
                this.CurrentFolder.Open(FolderAccess.ReadWrite);
                return this.CurrentFolder;
            });
        }

        /// <summary>
        /// Select a mailbox by name
        /// </summary>
        /// <param name="mailbox">The name of the mailbox</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="SelectMailbox" lang="C#" />
        /// </example> 
        public virtual void SelectMailbox(string mailbox)
        {
            GenericWait.WaitFor<bool>(() =>
            {
                this.CurrentMailBox = mailbox;
                this.CurrentFolder = this.EmailConnection.GetFolder(mailbox);
                this.CurrentFolder.Open(FolderAccess.ReadWrite);
                return true;
            });
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
            var topFolder = this.EmailConnection.GetFolder(this.BaseNamespace());
            topFolder.Create(newMailBox, true);
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
        public virtual MimeMessage GetMessage(string mailBox, string uid, bool headerOnly = false, bool markRead = false)
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
        public virtual MimeMessage GetMessage(string uid, bool headerOnly = false, bool markRead = false)
        {
            UniqueId uniqueID = new UniqueId(uint.Parse(uid));
            if (markRead)
            {
                this.GetCurrentFolder().AddFlags(uniqueID, MessageFlags.Seen, true);
            }

            MimeMessage message = this.GetCurrentFolder().GetMessage(uniqueID);
            if (headerOnly)
            {
                message.Body = null;
            }

            return message;
        }

        /// <summary>
        /// Get a list of email messages from the current mailbox
        /// </summary>
        /// <returns>A list of email messages</returns>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="EmailHeaders" lang="C#" />
        /// </example>        
        public virtual List<MimeMessage> GetAllMessageHeaders()
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
        public virtual List<MimeMessage> GetAllMessageHeaders(string mailBox)
        {
            this.SelectMailbox(mailBox);
            return this.SearchMessages(SearchQuery.All);
        }

        /// <summary>
        /// Delete the given email
        /// </summary>
        /// <param name="message">The email with to delete</param>
        /// <example>
        /// <code source="../EmailUnitTests/EmailUnitWithWrapper.cs" region="DeleteMessage" lang="C#" />
        /// </example>
        public virtual void DeleteMessage(MimeMessage message)
        {
            this.DeleteMessage(this.GetUniqueIDString(message));
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
            var folder = this.GetCurrentFolder();
            var items = folder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Flags);
            foreach (var item in items)
            {
                if (uid.Equals(item.UniqueId.Id.ToString()))
                {
                    folder.AddFlags(item.UniqueId, MessageFlags.Deleted, true);
                }

                folder.Expunge();
            }
        }

        /// <summary>
        /// Move the given email to the destination mailbox
        /// </summary>
        /// <param name="message">The email</param>
        /// <param name="destinationMailbox">The destination mailbox</param>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="MoveMessage" lang="C#" />
        /// </example>  
        public virtual void MoveMailMessage(MimeMessage message, string destinationMailbox)
        {
            this.MoveMailMessage(this.GetUniqueIDString(message), destinationMailbox);
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
            IMailFolder folder = this.GetCurrentFolder();
            folder.MoveTo(new UniqueId(uint.Parse(uid)), this.EmailConnection.GetFolder(destinationMailbox));
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
        public virtual List<MimeEntity> GetAttachments(string uid)
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
        public virtual List<MimeEntity> GetAttachments(string mailBox, string uid)
        {
            this.SelectMailbox(mailBox);
            var folder = this.GetCurrentFolder();
            return this.GetAttachments(folder.GetMessage(new UniqueId(uint.Parse(uid))));
        }

        /// <summary>
        /// Get the list of attachments for the email with the given message
        /// </summary>
        /// <param name="message">The message</param>
        /// <returns>The list of attachments</returns>
        /// <example>
        /// <code source = "../EmailUnitTests/EmailUnitWithWrapper.cs" region="GetAttachmentsMessage" lang="C#" />
        /// </example>
        public virtual List<MimeEntity> GetAttachments(MimeMessage message)
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
        public virtual List<string> DownloadAttachments(MimeMessage message)
        {
            return this.DownloadAttachments(message, EmailConfig.GetAttachmentDownloadDirectory());
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
        public virtual List<string> DownloadAttachments(MimeMessage message, string downloadFolder)
        {
            // Create the download folder if id does not already exist
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }

            List<string> paths = new List<string>();
            foreach (MimePart attachment in message.Attachments)
            {
                string destination = Path.Combine(downloadFolder, attachment.FileName);
                using (var stream = File.Create(destination))
                {
                    attachment.Content.DecodeTo(stream);
                }

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
        public virtual List<MimeMessage> SearchMessagesSince(string mailBox, DateTime time, bool headersOnly = true, bool markRead = false)
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
        public virtual List<MimeMessage> SearchMessagesSince(DateTime time, bool headersOnly = true, bool markRead = false)
        {
            return this.SearchMessages(SearchQuery.SentAfter(time), headersOnly, markRead);
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
        public virtual List<MimeMessage> SearchMessages(string mailBox, SearchQuery condition, bool headersOnly = true, bool markRead = false)
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
        public virtual List<MimeMessage> SearchMessages(SearchQuery condition, bool headersOnly = true, bool markRead = false)
        {
            object[] args = { condition, headersOnly, markRead };
            return GenericWait.WaitFor<List<MimeMessage>, object[]>(this.GetSearchResults, args);
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
        public virtual List<string> GetContentTypes(MimeMessage message)
        {
            List<string> types = new List<string>();

            foreach (MimeEntity bodyPart in message.BodyParts)
            {
                if (!bodyPart.IsAttachment)
                {
                    types.Add(bodyPart.ContentType.MimeType);
                }
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
        public virtual string GetBodyByContentTypes(MimeMessage message, string contentType)
        {
            foreach (MimeEntity bodyPart in message.BodyParts)
            {
                if (bodyPart.ContentType.MimeType.Equals(contentType, StringComparison.CurrentCultureIgnoreCase))
                {
                    MemoryStream stream = new MemoryStream();
                    bodyPart.WriteTo(stream, true);
                    return Encoding.ASCII.GetString(stream.ToArray());
                }
            }

            throw new Exception(StringProcessor.SafeFormatter("Failed to find content type '{0}'", contentType));
        }

        /// <summary>
        /// Get a list of flags for the email with the given uniqueID
        /// </summary>
        /// <param name="uid">The uniqueID for the message</param>
        /// <returns>The list of flags</returns>
        public List<IMessageSummary> GetEmailFlags(string uid)
        {
            var folder = this.GetCurrentFolder();
            return folder.Fetch(new[] { new UniqueId(uint.Parse(uid)) }, MessageSummaryItems.Flags | MessageSummaryItems.GMailLabels).ToList();
        }

        /// <summary>
        /// Get the UniqueID for the inputted MimeMessage
        /// </summary>
        /// <param name="message">MimeMessage to get the Unique ID for</param>
        /// <returns>The UniqueID in string form</returns>
        public string GetUniqueIDString(MimeMessage message)
        {
            var folder = this.GetCurrentFolder();
            var items = folder.Fetch(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Flags);
            foreach (var item in items)
            {
                if (message.MessageId.Equals(folder.GetMessage(item.UniqueId).MessageId))
                {
                    return item.UniqueId.Id.ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Get the list of emails for a search
        /// </summary>
        /// <param name="args">The search condition followed by the header only and set as seen booleans</param>
        /// <returns>The list of mail message that match the search</returns>
        private List<MimeMessage> GetSearchResults(params object[] args)
        {
            List<MimeMessage> messageList = new List<MimeMessage>();
            IMailFolder folder = this.GetCurrentFolder();
            foreach (UniqueId uid in folder.Search((SearchQuery)args[0], default(CancellationToken)))
            {
                if ((bool)args[2])
                {
                    folder.AddFlags(uid, MessageFlags.Seen, true);
                }

                MimeMessage message = null;

                if ((bool)args[1])
                {
                    HeaderList headers = folder.GetHeaders(uid);
                    message = new MimeMessage(headers);
                }
                else
                {
                    message = folder.GetMessage(uid);
                }

                messageList.Add(message);
            }

            foreach (MimeMessage message in messageList)
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
            List<IMailFolder> mailboxes = this.EmailConnection.GetFolders(this.BaseNamespace()).ToList();

            foreach (IMailFolder mailbox in mailboxes)
            {
                if (mailbox.Name.Equals("Inbox", StringComparison.CurrentCultureIgnoreCase))
                {
                    this.SelectMailbox(mailbox.Name);
                    return;
                }
            }

            this.SelectMailbox(mailboxes[0].Name);
        }

        /// <summary>
        /// Get the default folder namespace
        /// </summary>
        /// <returns>The default folder namespace</returns>
        private FolderNamespace BaseNamespace()
        {
            return this.EmailConnection.PersonalNamespaces[0];
        }

        /// <summary>
        /// Gets and update the Folder representing the CurrentMailBox
        /// </summary>
        /// <returns>The folder representing the CurrentMailBox</returns>
        private IMailFolder GetCurrentFolder()
        {
            this.CurrentFolder.Check();
            return this.CurrentFolder;
        }
    }
}
