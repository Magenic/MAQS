//--------------------------------------------------
// <copyright file="EmailUnitWithDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test email driver with base email test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit;
using MailKit.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
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
    public class EmailUnitWithDriver : BaseEmailTest
    {
        /// <summary>
        /// Cleanup after the test run
        /// </summary>
        [ClassCleanup]
        public static void Cleanup()
        {
                string host = EmailConfig.GetHost();
                string username = EmailConfig.GetUserName();
                string password = EmailConfig.GetPassword();
                int port = EmailConfig.GetPort();
                bool isSsl = EmailConfig.GetEmailViaSSL();
                bool checkSsl = EmailConfig.GetEmailSkipSslValidation();

                using (EmailDriver driver = new EmailDriver(host, username, password, port, 10000, isSsl, checkSsl))
                {
                    driver.SelectMailbox("Inbox");
                    foreach (MimeMessage messageHeader in driver.GetAllMessageHeaders())
                    {
                        driver.DeleteMessage(messageHeader);
                        Thread.Sleep(100);
                    }
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
            Assert.IsTrue(this.EmailDriver.CanAccessEmailAccount(), "Email account was not accessible");
        }
        #endregion

        /// <summary>
        /// Switch between different mailboxes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void SwitchBetweenMailBoxes()
        {
            List<string> mailBoxes = this.EmailDriver.GetMailBoxNames();

            // The Gmail inbox is not needed
            mailBoxes.Remove("[Gmail]");

            foreach (string mailBox in mailBoxes)
            {
                this.EmailDriver.SelectMailbox(mailBox);
                Assert.AreEqual(mailBox, this.EmailDriver.CurrentMailBox);
            }
        }

        /// <summary>
        /// Loop over all mailboxes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEachMailBox()
        {
            List<string> mailBoxes = this.EmailDriver.GetMailBoxNames();

            // The Gmail inbox is not needed
            mailBoxes.Remove("[Gmail]");

            foreach (string mailBox in mailBoxes)
            {
                IMailFolder box = this.EmailDriver.GetMailbox(mailBox);
                Assert.AreEqual(box.FullName, mailBox);
                Assert.AreEqual(mailBox, this.EmailDriver.CurrentMailBox);
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
            List<string> mailBoxes = this.EmailDriver.GetMailBoxNames();
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
            IMailFolder box = this.EmailDriver.GetMailbox(mailBox);
            Assert.AreEqual(box.FullName, mailBox);
            Assert.AreEqual(mailBox, this.EmailDriver.CurrentMailBox);
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
            this.EmailDriver.SelectMailbox(mailBox);
            Assert.AreEqual(mailBox, this.EmailDriver.CurrentMailBox);
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
                this.EmailDriver.CreateMailbox(newMailBox);
                Assert.AreEqual(newMailBox, this.EmailDriver.CurrentMailBox);
                IMailFolder box = this.EmailDriver.GetMailbox(newMailBox);
                Assert.AreEqual(newMailBox, this.EmailDriver.CurrentMailBox);
            }
            finally
            {
                // Cleanup after the test
                Thread.Sleep(100);
                try
                {
                    this.EmailDriver.GetMailbox(newMailBox).Delete();
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
            this.EmailDriver.CreateMailbox(newMailBox);
            Assert.AreEqual(newMailBox, this.EmailDriver.CurrentMailBox);
            IMailFolder box = this.EmailDriver.GetMailbox(newMailBox);
            Assert.AreEqual(newMailBox, this.EmailDriver.CurrentMailBox);
            this.EmailDriver.GetMailbox(newMailBox).Delete();
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
            this.EmailDriver.SelectMailbox("Test/SubTest");
            MimeMessage singleMessage = this.EmailDriver.GetMessage("3");
            Assert.AreEqual("Plain Text", singleMessage.Subject);
            Assert.IsFalse(string.IsNullOrEmpty(singleMessage.Body.ToString()), "Expected to go the message body");
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
            this.EmailDriver.SelectMailbox("Test/SubTest");
            MimeMessage singleMessage = this.EmailDriver.GetMessage("3", true);
            Assert.AreEqual("Plain Text", singleMessage.Subject);
            Assert.IsNull(singleMessage.Body, "Expected not to go the message body");
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
            MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "2");
            Assert.AreEqual("RTF Text", singleMessage.Subject);
            Assert.IsFalse(string.IsNullOrEmpty(singleMessage.Body.ToString()), "Expected to go the message body");
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
            MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "2", true);

            Assert.AreEqual("RTF Text", singleMessage.Subject);
            Assert.IsNull(singleMessage.Body, "Expected not to go the message body");
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
            this.EmailDriver.SelectMailbox("Test/SubTest");
            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();
            Assert.AreEqual(messageHeaders.Count, 4, "Expected 4 messages in 'Test/SubTest' but found " + messageHeaders.Count);
            foreach (MimeMessage message in messageHeaders)
            {
                Assert.IsNull(message.Body, "Got body data but only expected header data");
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
            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders("Test/SubTest");
            Assert.AreEqual(messageHeaders.Count, 4, "Expected 4 messages in 'Test/SubTest' but found " + messageHeaders.Count);
            foreach (MimeMessage message in messageHeaders)
            {
                Assert.IsNull(message.Body, "Got body data but only expected header data");
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

            this.EmailDriver.SelectMailbox("Test/SubTest");

            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
            {
                List<IMessageSummary> flags  = this.EmailDriver.GetEmailFlags(this.EmailDriver.GetUniqueIDString(message));
                if (flags[0].Flags.Value.HasFlag(MessageFlags.Seen))
                {
                    seen++;
                }
                else if (flags[0].Flags.Value.HasFlag(MessageFlags.None))
                {
                    notSeen++;
                }
                else
                {
                    Assert.Fail("Found message with unexpected flag of " + flags[0].Flags.Value.ToString());
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
            if (!GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 1, 0), uniqueSubject))
            {
                Assert.Fail("Failed to get message " + uniqueSubject);
            }

            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            // find the email and delete it
            foreach (MimeMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    this.EmailDriver.DeleteMessage(message);
                    break;
                }
            }

            messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            // Make sure it actually was deleted
            foreach (MimeMessage message in messageHeaders)
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

            SearchQuery condition = SearchQuery.SubjectContains(uniqueSubject);

            // Get the email and mark it as seen
            MimeMessage message = this.EmailDriver.SearchMessages(SearchQuery.SubjectContains(uniqueSubject), false)[0];

            message = this.EmailDriver.GetMessage(this.EmailDriver.GetUniqueIDString(message), true, true);

            if (!GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject))
            {
                Assert.Fail("Refresh of " + uniqueSubject + " failed");
            }

            message = this.EmailDriver.SearchMessages(SearchQuery.SubjectContains(uniqueSubject), false)[0];

            List<IMessageSummary> flags = this.EmailDriver.GetEmailFlags(this.EmailDriver.GetUniqueIDString(message));
            Assert.IsTrue(flags[0].Flags.Value.HasFlag(MessageFlags.Seen), "Message not marked as read");

            this.EmailDriver.DeleteMessage(message);
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

            GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 1, 0), uniqueSubject);

            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    this.EmailDriver.DeleteMessage(this.EmailDriver.GetUniqueIDString(message));
                    break;
                }
            }

            messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
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
        public void MoveMessage()
        {
            // Test is ignored for CI test run
            string uniqueSubject = Guid.NewGuid().ToString();
            this.SendTestEmail(uniqueSubject);

            GenericWait.Wait<bool, string>(this.IsEmailThere, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 30), uniqueSubject);
            Thread.Sleep(1000);

            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
            {
                Thread.Sleep(1000);
                if (message.Subject.Equals(uniqueSubject))
                {
                    // Move by message
                    this.EmailDriver.MoveMailMessage(message, "Test");

                    break;
                }
            }

            if (!GenericWait.WaitUntil<string>(this.HasBeenRemoved, uniqueSubject))
            {
                Assert.Fail("Message " + uniqueSubject + " was not removed by message");
            }

            Thread.Sleep(1000);
            this.EmailDriver.SelectMailbox("Test");
            messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    // Move by unique identifier
                    this.EmailDriver.MoveMailMessage(message, "AA");
                    break;
                }
            }

            if (!GenericWait.WaitUntil<string>(this.HasBeenRemoved, uniqueSubject))
            {
                Assert.Fail("Message " + uniqueSubject + " was not removed by uid");
            }

            Thread.Sleep(100);
            this.EmailDriver.SelectMailbox("AA");
            messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
            {
                if (message.Subject.Equals(uniqueSubject))
                {
                    this.EmailDriver.DeleteMessage(message);
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

            MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "4");
            List<MimeEntity> attchments = this.EmailDriver.GetAttachments(this.EmailDriver.GetUniqueIDString(singleMessage));

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (MimePart attachment in attchments)
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.FileName)), "Found extra file '" + attachment.FileName + "'");
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

            List<MimeEntity> attchments = this.EmailDriver.GetAttachments("Test/SubTest", "4");

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (MimePart attachment in attchments)
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.FileName)), "Found extra file '" + attachment.FileName + "'");
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

            MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "4");
            List<MimeEntity> attchments = this.EmailDriver.GetAttachments(singleMessage);

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (MimePart attachment in attchments)
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.FileName)), "Found extra file '" + attachment.FileName + "'");
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
            MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "4");
            List<string> attchments = this.EmailDriver.DownloadAttachments(singleMessage);

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
                MimeMessage singleMessage = this.EmailDriver.GetMessage("Test/SubTest", "4");
                List<string> attchments = this.EmailDriver.DownloadAttachments(singleMessage, downloadLocation);

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
            this.EmailDriver.SelectMailbox("Test/SubTest");
            
            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2016, 3, 11), false);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' after the given date but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the full message, not just the header");
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
            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince("Test/SubTest", new DateTime(2016, 3, 11), false);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' after the given date but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the full message, not just the header");
        }
        #endregion

        /// <summary>
        /// Run a compound search that should find one result, but only the header
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageHeaderWithCompoundCondition()
        {
            this.EmailDriver.SelectMailbox("Test/SubTest");

            SearchQuery condition = SearchQuery.NotSeen.And(SearchQuery.SubjectContains("Plain"));

            List<MimeMessage> messages = this.EmailDriver.SearchMessages(condition);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' between the given dates but found " + messages.Count);
            Assert.IsNull(messages[0].Body, "Expected the message header only, not the entire message");
        }

        /// <summary>
        /// Run a compound search that should find one result
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageWithCompoundCondition()
        {
            this.EmailDriver.SelectMailbox("Test/SubTest");

            SearchQuery condition = SearchQuery.NotSeen.And(SearchQuery.SubjectContains("Plain"));

            List<MimeMessage> messages = this.EmailDriver.SearchMessages(condition, false);
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' between the given dates but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the entire message, but only got the header");
        }

        /// <summary>
        /// Run a compound search that should find one result in a given mailbox
        /// </summary>
        #region SearchMessages
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageWithMailboxAndCompoundCondition()
        {
            SearchQuery condition = SearchQuery.NotSeen.And(SearchQuery.FromContains("walsh"));
            List<MimeMessage> messages = this.EmailDriver.SearchMessages("Test/SubTest", condition, false);
            Assert.AreEqual(messages.Count, 3, "Expected 3 message in 'Test/SubTest' between the given dates but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the entire message, but only got the header");
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
            this.EmailDriver.SelectMailbox("Test/SubTest");

            SearchQuery condition = SearchQuery.NotSeen.And(SearchQuery.SubjectContains("RTF"));

            List<MimeMessage> messages = this.EmailDriver.SearchMessages(condition);
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
            this.EmailDriver.SelectMailbox("Test/SubTest");

            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2016, 3, 11));
            Assert.AreEqual(messages.Count, 1, "Expected 1 message in 'Test/SubTest' after the given date but found " + messages.Count);
            Assert.IsNull(messages[0].Body, "Expected the message header only, not the entire message");
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
            this.EmailDriver.SelectMailbox("Test/SubTest");

            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2016, 3, 11), false);

            List<string> types = this.EmailDriver.GetContentTypes(messages[0]);

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
            this.EmailDriver.SelectMailbox("Test/SubTest");
            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2016, 3, 11), false);
            string content = this.EmailDriver.GetBodyByContentTypes(messages[0], "text/html");

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
            Assert.AreEqual(this.TestObject.EmailDriver, this.EmailDriver, "Web service driver don't match");
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
            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
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
            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
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
