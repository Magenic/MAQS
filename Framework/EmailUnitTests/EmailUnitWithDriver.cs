//--------------------------------------------------
// <copyright file="EmailUnitWithDriver.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
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
using System.Linq;
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
    [DoNotParallelize]
    [ExcludeFromCodeCoverage]
    public class EmailUnitWithDriver : BaseEmailTest
    {
        private const string PlainTextMessageUID = "1";
        private const string RTFMessageUID = "2";
        private const string MultipartMessageUID = "3";
        private const string MimeMessageUID = "4";

        /// <summary>
        /// Verify the user can access account 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void CanAccessEmail()
        {
            Assert.IsTrue(this.EmailDriver.CanAccessEmailAccount(), "Email account was not accessible");
        }

        /// <summary>
        /// Switch between different mailboxes
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void SwitchBetweenMailBoxes()
        {
            List<string> mailBoxes = this.EmailDriver.GetMailBoxNames();

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
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMailboxes()
        {
            List<string> mailBoxes = this.EmailDriver.GetMailBoxNames();
            Assert.IsTrue(mailBoxes.Count > 0);
        }

        /// <summary>
        /// Test to get a mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMailbox()
        {
            string mailBox = "TestInbox";
            IMailFolder box = this.EmailDriver.GetMailbox(mailBox);
            Assert.AreEqual(box.FullName, mailBox);
            Assert.AreEqual(mailBox, this.EmailDriver.CurrentMailBox);
        }

        /// <summary>
        /// Test to select a specific mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void SelectMailbox()
        {
            string mailBox = "TestInbox";
            this.EmailDriver.SelectMailbox(mailBox);
            Assert.AreEqual(mailBox, this.EmailDriver.CurrentMailBox);
        }

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
                this.EmailDriver.GetMailbox(newMailBox);
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
        /// Create a mailbox to test deleting a Mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DeleteMailBox()
        {
            string newMailBox = Guid.NewGuid().ToString();

            this.EmailDriver.CreateMailbox(newMailBox);
            Assert.AreEqual(newMailBox, this.EmailDriver.CurrentMailBox);
            var mailbox = this.EmailDriver.GetMailbox(newMailBox);
            // In dovecot, if you delete a mailbox without closing it, it terminates the IMAP connection.
            this.EmailDriver.CurrentFolder.Close();
            mailbox.Delete();

            Assert.IsTrue(this.EmailDriver.GetMailBoxNames().Any(mailbox => mailbox != newMailBox), $"Unable to delete mailbox {newMailBox}");
        }

        /// <summary>
        /// Get a specific email
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessage()
        {
            this.EmailDriver.SelectMailbox("TestInbox");
            MimeMessage singleMessage = this.EmailDriver.GetMessage(PlainTextMessageUID);
            Assert.AreEqual("Plain Text", singleMessage.Subject);
            Assert.IsFalse(string.IsNullOrEmpty(singleMessage.Body.ToString()), "Expected to go the message body");
        }

        /// <summary>
        /// Get the header info for a specific email
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessageHeader()
        {
            this.EmailDriver.SelectMailbox("TestInbox");
            MimeMessage singleMessage = this.EmailDriver.GetMessage(PlainTextMessageUID, true);
            Assert.AreEqual("Plain Text", singleMessage.Subject);
            Assert.IsNull(singleMessage.Body, "Expected not to go the message body");
        }

        /// <summary>
        /// Get a specific email in a specific mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessagePassingInMailbox()
        {
            MimeMessage singleMessage = this.EmailDriver.GetMessage("TestInbox", RTFMessageUID);
            Assert.AreEqual("RTF Text", singleMessage.Subject);
            Assert.IsFalse(string.IsNullOrEmpty(singleMessage.Body.ToString()), "Expected to go the message body");
        }

        /// <summary>
        /// Get just the header for a specific email in a specific mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetSpecificMessageHeaderPassingInMailbox()
        {
            MimeMessage singleMessage = this.EmailDriver.GetMessage("TestInbox", RTFMessageUID, true);

            Assert.AreEqual("RTF Text", singleMessage.Subject);
            Assert.IsNull(singleMessage.Body, "Expected not to go the message body");
        }

        /// <summary>
        /// Get all email headers
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailHeaders()
        {
            this.EmailDriver.SelectMailbox("TestInbox");
            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();
            Assert.AreEqual(4, messageHeaders.Count, "Expected 4 messages in 'TestInbox' but found " + messageHeaders.Count);
            foreach (MimeMessage message in messageHeaders)
            {
                Assert.IsNull(message.Body, "Got body data but only expected header data");
            }
        }

        /// <summary>
        /// Test to get the mail headers for a selected mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailHeadersMailbox()
        {
            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders("TestInbox");
            Assert.AreEqual(4, messageHeaders.Count, "Expected 4 messages in 'TestInbox' but found " + messageHeaders.Count);
            foreach (MimeMessage message in messageHeaders)
            {
                Assert.IsNull(message.Body, "Got body data but only expected header data");
            }
        }

        /// <summary>
        /// Check the flags for a group of emails
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetFlagsFromEmailHeaders()
        {
            int seen = 0;
            int notSeen = 0;

            this.EmailDriver.SelectMailbox("TestInbox");

            List<MimeMessage> messageHeaders = this.EmailDriver.GetAllMessageHeaders();

            foreach (MimeMessage message in messageHeaders)
            {
                List<IMessageSummary> flags = this.EmailDriver.GetEmailFlags(this.EmailDriver.GetUniqueIDString(message));
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

            // Get the email and mark it as seen
            MimeMessage message = this.EmailDriver.SearchMessages(SearchQuery.SubjectContains(uniqueSubject), false)[0];

            message = this.EmailDriver.GetMessage(this.EmailDriver.GetUniqueIDString(message), true, true);
            Assert.IsNotNull(message, "Unexpected null message");

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

        /// <summary>
        /// Verify we can move messages from one folder to another
        /// </summary>
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

        /// <summary>
        /// Verify we can get all attachments for a given unique identifier
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetAttachmentsByUid()
        {
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            MimeMessage singleMessage = this.EmailDriver.GetMessage("TestInbox", MimeMessageUID);
            List<MimeEntity> attchments = this.EmailDriver.GetAttachments(this.EmailDriver.GetUniqueIDString(singleMessage));

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (MimePart attachment in attchments.OfType<MimePart>())
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.FileName)), "Found extra file '" + attachment.FileName + "'");
            }
        }

        /// <summary>
        /// Verify we can get all attachments for a given mailbox and unique identifier
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetAttachmentsByMailBoxAndUid()
        {
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            List<MimeEntity> attchments = this.EmailDriver.GetAttachments("TestInbox", MimeMessageUID);

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (MimePart attachment in attchments.OfType<MimePart>())
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.FileName)), "Found extra file '" + attachment.FileName + "'");
            }
        }

        /// <summary>
        /// Verify we can get the list of attachments
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetAttachments()
        {
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            MimeMessage singleMessage = this.EmailDriver.GetMessage("TestInbox", MimeMessageUID);
            List<MimeEntity> attchments = this.EmailDriver.GetAttachments(singleMessage);

            // Make sure we have the correct number of attachments
            Assert.AreEqual(3, attchments.Count, "Expected 3 attachments");

            // Make sure the expected files are included
            foreach (MimePart attachment in attchments.OfType<MimePart>())
            {
                Assert.IsTrue(File.Exists(Path.Combine(testFilePath, attachment.FileName)), "Found extra file '" + attachment.FileName + "'");
            }
        }

        /// <summary>
        /// Download all attachments for a specific email - Verify it has the expected attachments and that they can be downloaded to the configured download location
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DownloadAttachmentsToConfigLocation()
        {
            MimeMessage singleMessage = this.EmailDriver.GetMessage("TestInbox", MimeMessageUID);
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

        /// <summary>
        /// Download all attachments for a specific email - Verify it has the expected attachments and that they can be downloaded to a specified location
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void DownloadAttachmentsToTestDefinedLocation()
        {
            // Setup a test download location
            string downloadLocation = Path.Combine(EmailConfig.GetAttachmentDownloadDirectory(), Guid.NewGuid().ToString());
            string testFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TestFiles");

            try
            {
                MimeMessage singleMessage = this.EmailDriver.GetMessage("TestInbox", MimeMessageUID);
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

        /// <summary>
        /// Do since search for headers only
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessagesSince()
        {
            this.EmailDriver.SelectMailbox("TestInbox");

            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2020, 5, 12), false);
            Assert.AreEqual(1, messages.Count, "Expected 1 message in 'TestInbox' after the given date but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the full message, not just the header");
        }

        /// <summary>
        /// Do since search for headers only in a specific mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessagesSinceForMailbox()
        {
            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince("TestInbox", new DateTime(2020, 5, 12), false);
            Assert.AreEqual(1, messages.Count, "Expected 1 message in 'TestInbox' after the given date but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the full message, not just the header");
        }

        /// <summary>
        /// Run a compound search that should find one result, but only the header
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageHeaderWithCompoundCondition()
        {
            this.EmailDriver.SelectMailbox("TestInbox");

            SearchQuery condition = SearchQuery.Seen.And(SearchQuery.SubjectContains("Plain"));

            List<MimeMessage> messages = this.EmailDriver.SearchMessages(condition);
            Assert.AreEqual(1, messages.Count, "Expected 1 message in 'TestInbox' between the given dates but found " + messages.Count);
            Assert.IsNull(messages[0].Body, "Expected the message header only, not the entire message");
        }

        /// <summary>
        /// Run a compound search that should find one result
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageWithCompoundCondition()
        {
            this.EmailDriver.SelectMailbox("TestInbox");

            SearchQuery condition = SearchQuery.Seen.And(SearchQuery.SubjectContains("Plain"));

            List<MimeMessage> messages = this.EmailDriver.SearchMessages(condition, false);
            Assert.AreEqual(1, messages.Count, "Expected 1 message in 'TestInbox' between the given dates but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the entire message, but only got the header");
        }

        /// <summary>
        /// Run a compound search that should find one result in a given mailbox
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessageWithMailboxAndCompoundCondition()
        {
            SearchQuery condition = SearchQuery.NotSeen.And(SearchQuery.SubjectContains("Message"));
            List<MimeMessage> messages = this.EmailDriver.SearchMessages("TestInbox", condition, false);
            Assert.AreEqual(2, messages.Count, "Expected 2 message in 'TestInbox' between the given dates but found " + messages.Count);
            Assert.IsFalse(string.IsNullOrEmpty(messages[0].Body.ToString()), "Expected the entire message, but only got the header");
        }

        /// <summary>
        /// Run a compound search that should find no results
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetNoMessagesWithCompoundCondition()
        {
            this.EmailDriver.SelectMailbox("TestInbox");

            SearchQuery condition = SearchQuery.Seen.And(SearchQuery.SubjectContains("Production"));

            List<MimeMessage> messages = this.EmailDriver.SearchMessages(condition);
            Assert.AreEqual(0, messages.Count, "Expected 0 message in 'TestInbox' between the given dates but found " + messages.Count);
        }

        /// <summary>
        /// Search for the one expected email that matches the message since search
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetMessagesHeadersSince()
        {
            this.EmailDriver.SelectMailbox("TestInbox");

            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2020, 5, 12));
            Assert.AreEqual(1, messages.Count, "Expected 1 message in 'TestInbox' after the given date but found " + messages.Count);
            Assert.IsNull(messages[0].Body, "Expected the message header only, not the entire message");
        }

        /// <summary>
        /// Check that we get all the content types we expect for a specific email
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetTypes()
        {
            this.EmailDriver.SelectMailbox("TestInbox");

            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2020, 5, 12), false);

            List<string> types = this.EmailDriver.GetContentTypes(messages[0]);

            Assert.IsTrue(types.Count == 2, "Expected 2 content types");
            Assert.IsTrue(types.Contains("text/plain"), "Expected 'text/plain' content types");
            Assert.IsTrue(types.Contains("text/html"), "Expected 'text/html' content types");
        }

        /// <summary>
        /// Get the body of an email and verify it is of the correct type 
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetBodyByContentType()
        {
            this.EmailDriver.SelectMailbox("TestInbox");
            List<MimeMessage> messages = this.EmailDriver.SearchMessagesSince(new DateTime(2020, 5, 12), false);
            string content = this.EmailDriver.GetBodyByContentTypes(messages[0], "text/html");

            // Make sure we got the html content back
            Assert.IsTrue(content.StartsWith("<html>"), "Expected html content");
        }

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

            Assert.AreEqual("one", this.TestObject.Values["1"]);
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
            client.Port = 25;
            client.Host = "localhost";
            client.EnableSsl = false;
            client.Timeout = 60000;
            client.DeliveryMethod = SystemEmail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(EmailConfig.GetUserName(), EmailConfig.GetPassword());

            SystemEmail.MailMessage message = new SystemEmail.MailMessage(EmailConfig.GetUserName(), EmailConfig.GetUserName(), subject, "test");
            message.BodyEncoding = UTF8Encoding.UTF8;
            message.Headers.Add("Message-Id", Guid.NewGuid().ToString());
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
