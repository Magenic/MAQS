//--------------------------------------------------
// <copyright file="EmailUnitWithWrapper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test email wrapper with base email test</summary>
//--------------------------------------------------
using AE.Net.Mail;
using AE.Net.Mail.Imap;
using Magenic.MaqsFramework.BaseEmailTest;
using Magenic.MaqsFramework.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using SystemEmail = System.Net.Mail;

namespace EmailUnitTests
{
    /// <summary>
    /// Test basic email testing with the base email test
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EmailUnitWithWrapper : BaseEmailTest
    {
        /// <summary>
        /// Did the logging folder exist at the start of the test run
        /// </summary>
        private static bool loggingFolderExistsBeforeRun = false;

        /// <summary>
        /// Setup before we start running selenium tests
        /// </summary>
        /// <param name="context">The upcoming test context</param>
        [ClassInitialize]
        public static void CheckBeforeClass(TestContext context)
        {
            loggingFolderExistsBeforeRun = TestHelper.DoesFolderExist();
        }

        /// <summary>
        /// Cleanup after the test run
        /// </summary>
        [ClassCleanup]
        public static void Cleanup()
        {
            try
            {
                string host = EmailConfig.GetHost();
                string username = EmailConfig.GetUserName();
                string password = EmailConfig.GetPassword();
                int port = EmailConfig.GetPort();
                bool isSsl = EmailConfig.GetEmailViaSSL();
                bool checkSsl = EmailConfig.GetEmailSkipSslValidation();

                using (EmailConnectionWrapper wrapper = new EmailConnectionWrapper(host, username, password, port, 10000, true, true))
                {
                    wrapper.SelectMailbox("Inbox");
                    foreach (MailMessage messageHeader in wrapper.GetAllMessageHeaders())
                    {
                        wrapper.DeleteMessage(messageHeader);
                        Thread.Sleep(100);
                    }
                }
            }
            finally
            {
                TestHelper.Cleanup(loggingFolderExistsBeforeRun);
            }
        }

        /// <summary>
        /// Verify the user can access account 
        /// </summary>
        #region CanAccessEmail
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void CanAccessEmail()
        {
            Assert.IsTrue(this.EmailWrapper.CanAccessEmailAccount(), "Email account was not accessable");
        }
        #endregion

        /// <summary>
        /// Switch between different mailboxes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void SwitchBetweenMailBoxes()
        {
            List<string> mailBoxes = this.EmailWrapper.GetMailBoxNames();

            // The Gmail inbox is not needed
            mailBoxes.Remove("[Gmail]");

            foreach (string mailBox in mailBoxes)
            {
                this.EmailWrapper.SelectMailbox(mailBox);
                Assert.AreEqual(mailBox, this.EmailWrapper.CurrentMailBox);
            }
        }

        /// <summary>
        /// Loop over all mailboxes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEachMailBox()
        {
            List<string> mailBoxes = this.EmailWrapper.GetMailBoxNames();

            // The Gmail inbox is not needed
            mailBoxes.Remove("[Gmail]");

            foreach (string mailBox in mailBoxes)
            {
                Mailbox box = this.EmailWrapper.GetMailbox(mailBox);
                Assert.AreEqual(box.Name, mailBox);
                Assert.AreEqual(mailBox, this.EmailWrapper.CurrentMailBox);
            }
        }

        /// <summary>
        /// Test to get the mailboxes
        /// </summary>
        #region GetMailboxes
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMailboxes()
        {
            List<string> mailBoxes = this.EmailWrapper.GetMailBoxNames();
            Assert.IsTrue(mailBoxes.Count > 0);
        }
        #endregion

        /// <summary>
        /// Test to get a mailbox
        /// </summary>
        #region GetMailbox
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMailbox()
        {
            string mailBox = "[Gmail]/All Mail";
            Mailbox box = this.EmailWrapper.GetMailbox(mailBox);
            Assert.AreEqual(box.Name, mailBox);
            Assert.AreEqual(mailBox, this.EmailWrapper.CurrentMailBox);
        }
        #endregion

        /// <summary>
        /// Test to select a specific mailbox
        /// </summary>
        #region SelectMailbox
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void SelectMailbox()
        {
            string mailBox = "[Gmail]/All Mail";
            this.EmailWrapper.SelectMailbox(mailBox);
            Assert.AreEqual(mailBox, this.EmailWrapper.CurrentMailBox);
        }
        #endregion

        /// <summary>
        /// Create a mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void CreateMailBox()
        {
            string newMailBox = Guid.NewGuid().ToString();
            try
            {
                this.EmailWrapper.CreateMailbox(newMailBox);
                Assert.AreEqual(newMailBox, this.EmailWrapper.CurrentMailBox);
                Mailbox box = this.EmailWrapper.GetMailbox(newMailBox);
                Assert.AreEqual(newMailBox, this.EmailWrapper.CurrentMailBox);
            }
            finally
            {
                // Cleanup after the test
                Thread.Sleep(100);
                try
                {
                    this.EmailWrapper.EmailConnection.DeleteMailbox(newMailBox);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cleanup problem: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Create a mailbox
        /// </summary>
        #region CreateMailbox
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void CreateMailBoxTest()
        {
            string newMailBox = Guid.NewGuid().ToString();
            this.EmailWrapper.CreateMailbox(newMailBox);
            Assert.AreEqual(newMailBox, this.EmailWrapper.CurrentMailBox);
            Mailbox box = this.EmailWrapper.GetMailbox(newMailBox);
            Assert.AreEqual(newMailBox, this.EmailWrapper.CurrentMailBox);
            this.EmailWrapper.EmailConnection.DeleteMailbox(newMailBox);
        }
        #endregion

        /// <summary>
        /// Get a specific email
        /// </summary>
        #region GetMessageUid
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessage()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");
            MailMessage singleMessage = this.EmailWrapper.GetMessage("3");
            Assert.AreEqual("Plain Text", singleMessage.Subject);
            Assert.IsFalse(string.IsNullOrEmpty(singleMessage.Body), "Expected to go the message body");
        }
        #endregion

        /// <summary>
        /// Get the header info for a specific email
        /// </summary>
        #region GetMessageUid1
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessageHeader()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");
            MailMessage singleMessage = this.EmailWrapper.GetMessage("3", true);
            Assert.AreEqual("Plain Text", singleMessage.Subject);
            Assert.IsTrue(string.IsNullOrEmpty(singleMessage.Body), "Expected not to go the message body");
        }
        #endregion

        /// <summary>
        /// Get a specific email in a specific mailbox
        /// </summary>
        #region GetMessage
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessagePassingInMailbox()
        {
            MailMessage singleMessage = this.EmailWrapper.GetMessage("Test/SubTest", "2");
            Assert.AreEqual("RTF Text", singleMessage.Subject);
            Assert.IsFalse(string.IsNullOrEmpty(singleMessage.Body), "Expected to go the message body");
        }
        #endregion

        /// <summary>
        /// Get just the header for a specific email in a specific mailbox
        /// </summary>
        #region GetMessage1
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessageHeaderPassingInMailbox()
        {
            MailMessage singleMessage = this.EmailWrapper.GetMessage("Test/SubTest", "2", true);

            Assert.AreEqual("RTF Text", singleMessage.Subject);
            Assert.IsTrue(string.IsNullOrEmpty(singleMessage.Body), "Expected not to go the message body");
        }
        #endregion

        /// <summary>
        /// Get all email headers
        /// </summary>
        #region EmailHeaders
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailHeaders()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");
            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();
            Assert.AreEqual(messageHeaders.Count, 4, "Expected 4 messages in 'Test/SubTest' but found " + messageHeaders.Count);
            foreach (MailMessage message in messageHeaders)
            {
                Assert.IsTrue(string.IsNullOrEmpty(message.Body), "Got body data but only expected header data");
            }
        }
        #endregion

        /// <summary>
        /// Test to get the mail headers for a selected mailbox
        /// </summary>
        #region EmailHeadersMailbox
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailHeadersMailbox()
        {
            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders("Test/SubTest");
            Assert.AreEqual(messageHeaders.Count, 4, "Expected 4 messages in 'Test/SubTest' but found " + messageHeaders.Count);
            foreach (MailMessage message in messageHeaders)
            {
                Assert.IsTrue(string.IsNullOrEmpty(message.Body), "Got body data but only expected header data");
            }
        }
        #endregion

        /// <summary>
        /// Check the flags for a group of emails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetFlagsFromEmailHeaders()
        {
            int seen = 0;
            int notSeen = 0;

            this.EmailWrapper.SelectMailbox("Test/SubTest");

            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Flags.HasFlag(Flags.Seen))
                {
                    seen++;
                }
                else if (message.Flags.HasFlag(Flags.None))
                {
                    notSeen++;
                }
                else
                {
                    Assert.Fail("Found message with unexpected flag of " + message.Flags.ToString());
                }
            }

            Assert.AreEqual(1, seen, "Wrong number of read message but found");
            Assert.AreEqual(3, notSeen, "Wrong number of unread message but found");
        }

        /// <summary>
        /// Delete a email
        /// </summary>
        #region DeleteMessage
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DeleteMessage()
        {
            string uniqueSubject = Guid.NewGuid().ToString();
            this.SendTestEmail(uniqueSubject);

            if (!GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject))
            {
                Assert.Fail("Failed to get message " + uniqueSubject);
            }

            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            // find the email and delete it
            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    this.EmailWrapper.DeleteMessage(message);
                    break;
                }
            }

            messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            // Make sure it actually was deleted
            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    Assert.Fail("Message " + uniqueSubject + " was not deleted");
                }
            }
        }
        #endregion

        /// <summary>
        /// Mark and email as seen
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void MarkAsSeen()
        {
            string uniqueSubject = Guid.NewGuid().ToString();
            this.SendTestEmail(uniqueSubject);

            if (!GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject))
            {
                Assert.Fail("Failed to get message " + uniqueSubject);
            }

            Thread.Sleep(1000);

            SearchCondition condition = SearchCondition.Subject(uniqueSubject);

            // Get the email and mark it as seen
            MailMessage message = this.EmailWrapper.SearchMessages(SearchCondition.Subject(uniqueSubject), false)[0];

            message = this.EmailWrapper.GetMessage(message.Uid, true, true);

            if (!GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject))
            {
                Assert.Fail("Refresh of " + uniqueSubject + " failed");
            }

            message = this.EmailWrapper.SearchMessages(SearchCondition.Subject(uniqueSubject), false)[0];

            Assert.AreEqual(Flags.Seen, message.Flags, "Message not marked as read");

            this.EmailWrapper.DeleteMessage(message);
        }

        /// <summary>
        /// Delete a message with a specific unique identifier
        /// </summary>
        #region DeleteMessageUid
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DeleteMessageUsingUid()
        {
            string uniqueSubject = Guid.NewGuid().ToString();
            this.SendTestEmail(uniqueSubject);

            GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject);

            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    this.EmailWrapper.DeleteMessage(message.Uid);
                    break;
                }
            }

            messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    Assert.Fail("Message " + uniqueSubject + " was not deleted");
                }
            }
        }
        #endregion

        /// <summary>
        /// Verify we can move messages from one folder to another
        /// </summary>
        #region MoveMessage
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [Ignore]
        public void MoveMessage()
        {
            // Test is ignored for CI test run
            string uniqueSubject = Guid.NewGuid().ToString();
            this.SendTestEmail(uniqueSubject);

            GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject);
            Thread.Sleep(1000);

            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                Thread.Sleep(1000);
                if (message.Subject.Equals(uniqueSubject))
                {
                    // Move by message
                    this.EmailWrapper.MoveMailMessage(message, "Test");

                    break;
                }
            }

            if (!GenericWait.WaitUntil<string>(this.HasBeenRemoved, uniqueSubject))
            {
                Assert.Fail("Message " + uniqueSubject + " was not removed by message");
            }

            Thread.Sleep(1000);
            this.EmailWrapper.SelectMailbox("Test");
            messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    // Move by unique identifier
                    this.EmailWrapper.MoveMailMessage(message.Uid, "AA");
                    break;
                }
            }

            if (!GenericWait.WaitUntil<string>(this.HasBeenRemoved, uniqueSubject))
            {
                Assert.Fail("Message " + uniqueSubject + " was not removed by uid");
            }

            Thread.Sleep(100);
            this.EmailWrapper.SelectMailbox("AA");
            messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    this.EmailWrapper.DeleteMessage(message.Uid);
                    return;
                }
            }

            Assert.Fail("Message " + uniqueSubject + " was moved to new folder");
        }
        #endregion

        /// <summary>
        /// Verify we can get all attachments for a given unique identifier
        /// </summary>
        #region AttachmentsByUid
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetAttachmentsByUid()
        {
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            MailMessage singleMessage = this.EmailWrapper.GetMessage("Test/SubTest", "4");
            List<Attachment> attchments = this.EmailWrapper.GetAttachments(singleMessage.Uid);

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (Attachment attachment in attchments)
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.Filename)), "Found extra file '" + attachment.Filename + "'");
            }
        }
        #endregion

        /// <summary>
        /// Verify we can get all attachments for a given mailbox and unique identifier
        /// </summary>
        #region AttachmentsByMailboxAndUid
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetAttachmentsByMailBoxAndUid()
        {
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            List<Attachment> attchments = this.EmailWrapper.GetAttachments("Test/SubTest", "4");

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (Attachment attachment in attchments)
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.Filename)), "Found extra file '" + attachment.Filename + "'");
            }
        }
        #endregion

        /// <summary>
        /// Verify we can get the list of attachments
        /// </summary>
        #region GetAttachmentsMessage
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetAttachments()
        {
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            MailMessage singleMessage = this.EmailWrapper.GetMessage("Test/SubTest", "4");
            List<Attachment> attchments = this.EmailWrapper.GetAttachments(singleMessage);

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (Attachment attachment in attchments)
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.Filename)), "Found extra file '" + attachment.Filename + "'");
            }
        }
        #endregion

        /// <summary>
        /// Download all attachments for a specific email - Verify it has the expected attachments and that they can be downloaded to the configured download location
        /// </summary>
        #region DownloadAttachments
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DownloadAttachmentsToConfigLocation()
        {
            MailMessage singleMessage = this.EmailWrapper.GetMessage("Test/SubTest", "4");
            List<string> attchments = this.EmailWrapper.DownLoadAttachments(singleMessage);

            try
            {
                Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

                foreach (string file in attchments)
                {
                    string downloadFileHash = this.GetFileHash(file);
                    string testFileHash = this.GetFileHash(Path.Combine(EmailConfig.GetAttachmentDownloadDirectory(), Path.GetFileName(file)));

                    Assert.AreEqual(testFileHash, downloadFileHash, Path.GetFileName(file) + " test file and download file do not match");
                }
            }
            finally
            {
                foreach (string file in attchments)
                {
                    File.Delete(Path.Combine(EmailConfig.GetAttachmentDownloadDirectory(), Path.GetFileName(file)));
                }
            }
        }
        #endregion

        /// <summary>
        /// Download all attachments for a specific email - Verify it has the expected attachments and that they can be downloaded to a specified location
        /// </summary>
        #region DownloadAttachmentsToLocation
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DownloadAttachmentsToTestDefinedLocation()
        {
            // Setup a test download location
            string downloadLocation = Path.Combine(EmailConfig.GetAttachmentDownloadDirectory(), Guid.NewGuid().ToString());
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            try
            {
                MailMessage singleMessage = this.EmailWrapper.GetMessage("Test/SubTest", "4");
                List<string> attchments = this.EmailWrapper.DownLoadAttachments(singleMessage, downloadLocation);

                Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

                foreach (string file in attchments)
                {
                    string tempDownload = Path.Combine(downloadLocation, Guid.NewGuid().ToString());

                    // Fix weird Git related CRLF issue
                    if (file.EndsWith(".cs"))
                    {
                        string value = File.ReadAllText(Path.Combine(testFilePath, Path.GetFileName(file))).Replace("\r\n", "\n").Replace("\n", "\r\n");
                        File.WriteAllText(tempDownload, value);

                        value = File.ReadAllText(file).Replace("\r\n", "\n").Replace("\n", "\r\n");
                        File.WriteAllText(file, value);
                    }
                    else
                    {
                        File.Copy(Path.Combine(testFilePath, Path.GetFileName(file)), tempDownload);
                    }

                    string downloadFileHash = this.GetFileHash(file);
                    string testFileHash = this.GetFileHash(tempDownload);

                    Assert.AreEqual(testFileHash, downloadFileHash, Path.GetFileName(file) + " test file and download file do not match");
                }
            }
            finally
            {
                Directory.Delete(downloadLocation, true);
            }
        }
        #endregion

        /// <summary>
        /// Do since search for headers only
        /// </summary>
        #region MessageSince
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessagesSince()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");

            List<MailMessage> messages = this.EmailWrapper.SearchMessagesSince(new DateTime(2016, 3, 11), false);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' after the given date but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body), "Expected the full message, not just the header");
        }
        #endregion

        /// <summary>
        /// Do since search for headers only in a specific mailbox
        /// </summary>
        #region MessagesSinceByMailbox
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessagesSinceForMailbox()
        {
            List<MailMessage> messages = this.EmailWrapper.SearchMessagesSince("Test/SubTest", new DateTime(2016, 3, 11), false);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' after the given date but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body), "Expected the full message, not just the header");
        }
        #endregion

        /// <summary>
        /// Run a compound search that should find one result, but only the header
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageHeaderWithCompoundCondition()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");

            SearchCondition condition = SearchCondition.Unseen().And(SearchCondition.Subject("Plain"));

            List<MailMessage> messages = this.EmailWrapper.SearchMessages(condition);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' between the given dates but found " + messages.Count);
            Assert.IsTrue(string.IsNullOrEmpty(messages[0].Body), "Expected the message header only, not the entire message");
        }

        /// <summary>
        /// Run a compound search that should find one result
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageWithCompoundCondition()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");

            SearchCondition condition = SearchCondition.Unseen().And(SearchCondition.Subject("Plain"));

            List<MailMessage> messages = this.EmailWrapper.SearchMessages(condition, false);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' between the given dates but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body), "Expected the entire message, but only got the header");
        }

        /// <summary>
        /// Run a compound search that should find one result in a given mailbox
        /// </summary>
        #region SearchMessages
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageWithMailboxAndCompoundCondition()
        {
            SearchCondition condition = SearchCondition.Unseen().And(SearchCondition.From("walsh"));

            List<MailMessage> messages = this.EmailWrapper.SearchMessages("Test/SubTest", condition, false);
            Assert.AreEqual(messages.Count, 3, "Expected 3 message in 'Test/SubTest' between the given dates but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body), "Expected the entire message, but only got the header");
        }
        #endregion

        /// <summary>
        /// Run a compound search that should find no results
        /// </summary>
        #region SearchMessages1
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetNoMessagesWithCompoundCondition()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");

            SearchCondition condition = SearchCondition.Unseen().And(SearchCondition.Subject("RTF"));

            List<MailMessage> messages = this.EmailWrapper.SearchMessages(condition);
            Assert.AreEqual(messages.Count, 0, "Expected 0 message in 'Test/SubTest' between the given dates but found " + messages.Count);
        }
        #endregion

        /// <summary>
        /// Search for the one expected email that matches the message since search
        /// </summary>
        #region SearchMessages2
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessagesHeadersSince()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");

            List<MailMessage> messages = this.EmailWrapper.SearchMessagesSince(new DateTime(2016, 3, 11));
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' after the given date but found " + messages.Count);
            Assert.IsTrue(string.IsNullOrEmpty(messages[0].Body), "Expected the message header only, not the entire message");
        }
        #endregion

        /// <summary>
        /// Check that we get all the content types we expect for a specific email
        /// </summary>
        #region GetContentTypes
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetTypes()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");

            List<MailMessage> messages = this.EmailWrapper.SearchMessagesSince(new DateTime(2016, 3, 11), false);

            List<string> types = this.EmailWrapper.GetContentTypes(messages[0]);

            Assert.IsTrue(types.Count == 2, "Expected 2 content types");
            Assert.IsTrue(types.Contains("text/plain"), "Expected 'text/plain' content types");
            Assert.IsTrue(types.Contains("text/html"), "Expected 'text/html' content types");
        }
        #endregion

        /// <summary>
        /// Get the body of an email and verify it is of the correct type 
        /// </summary>
        #region GetBodyByContent
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetBodyByContentType()
        {
            this.EmailWrapper.SelectMailbox("Test/SubTest");
            List<MailMessage> messages = this.EmailWrapper.SearchMessagesSince(new DateTime(2016, 3, 11), false);
            string content = this.EmailWrapper.GetBodyByContentTypes(messages[0], "text/html");

            // Make sure we got the html content back
            Assert.IsTrue(content.StartsWith("<html "), "Expected html content");
        }
        #endregion

        /// <summary>
        /// Make sure the test objects map properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [TestCategory(TestCategories.Utilities)]
        public void EmailTestObjectMapCorrectly()
        {
            Assert.AreEqual(this.TestObject.Log, this.Log, "Logs don't match");
            Assert.AreEqual(this.TestObject.SoftAssert, this.SoftAssert, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.PerfTimerCollection, this.PerfTimerCollection, "Soft asserts don't match");
            Assert.AreEqual(this.TestObject.EmailWrapper, this.EmailWrapper, "Web service wrapper don't match");
        }

        /// <summary>
        /// Make sure test object values are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [TestCategory(TestCategories.Utilities)]
        public void EmailTestObjectValuesCanBeUsed()
        {
            this.TestObject.SetValue("1", "one");

            Assert.AreEqual(this.TestObject.Values["1"], "one");
            string outValue;
            Assert.IsFalse(this.TestObject.Values.TryGetValue("2", out outValue), "Didn't expect to get value for key '2', but got " + outValue);
        }

        /// <summary>
        /// Make sure the test object objects are saved as expected
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [TestCategory(TestCategories.Utilities)]
        public void EmailTestObjectObjectssCanBeUsed()
        {
            StringBuilder builder = new StringBuilder();
            this.TestObject.SetObject("1", builder);

            Assert.AreEqual(this.TestObject.Objects["1"], builder);

            object outObject;
            Assert.IsFalse(this.TestObject.Objects.TryGetValue("2", out outObject), "Didn't expect to get value for key '2'");

            builder.Append("123");

            Assert.AreEqual(((StringBuilder)this.TestObject.Objects["1"]).ToString(), builder.ToString());
        }

        /// <summary>
        /// Check that there are no emails with the given subject in the current mailbox
        /// </summary>
        /// <param name="uniqueSubject">The subject</param>
        /// <returns>True if no emails are found with the given subject</returns>
        private bool HasBeenRemoved(string uniqueSubject)
        {
            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Send a test email
        /// </summary>
        /// <param name="subject">Subject of the test email</param>
        private void SendTestEmail(string subject)
        {
            SystemEmail.SmtpClient client = new SystemEmail.SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 60000;
            client.DeliveryMethod = SystemEmail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(EmailConfig.GetUserName(), EmailConfig.GetPassword());

            SystemEmail.MailMessage message = new SystemEmail.MailMessage(EmailConfig.GetUserName(), EmailConfig.GetUserName(), subject, "test");
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.DeliveryNotificationOptions = SystemEmail.DeliveryNotificationOptions.OnFailure;

            client.Send(message);

            Thread.Sleep(5000);
        }

        /// <summary>
        /// Is an email with the given subject in the current mailbox
        /// </summary>
        /// <param name="subject">The subject</param>
        /// <returns>True if the email with the given subject is present</returns>
        private bool IsEmailThere(string subject)
        {
            List<MailMessage> messageHeaders = this.EmailWrapper.GetAllMessageHeaders();

            foreach (MailMessage message in messageHeaders)
            {
                if (message.Subject.Equals(subject))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create a hash string for a file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>The hash value</returns>
        private string GetFileHash(string filePath)
        {
            using (FileStream stream = File.OpenRead(filePath))
            {
                using (HashAlgorithm hashAlgorithm = new SHA1CryptoServiceProvider())
                {
                    byte[] hashArray = hashAlgorithm.ComputeHash(stream);
                    return BitConverter.ToString(hashArray);
                }
            }
        }
    }
}
