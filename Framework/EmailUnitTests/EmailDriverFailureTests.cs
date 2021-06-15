//--------------------------------------------------
// <copyright file="EmailDriverFailureTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver failure tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit.Net.Imap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EmailUnitTests
{
    /// <summary>
    /// Test the email driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Email)]
    [ExcludeFromCodeCoverage]
    public class EmailDriverFailureTests : BaseEmailTest
    {
        protected override ImapClient GetEmailConnection()
        {
            return ClientFactory.GetEmailClient(string.Empty, string.Empty, string.Empty, 1);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AccessError()
        {
            EmailDriver.CanAccessEmailAccount();
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailBoxNamesError()
        {
            EmailDriver.GetMailBoxNames();
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MailoxError()
        {
            EmailDriver.GetMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectBoxError()
        {
            EmailDriver.SelectMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateMailboxError()
        {
            EmailDriver.CreateMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMessageError()
        {
            EmailDriver.GetMessage(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAllMessageHeadersError()
        {
            EmailDriver.GetAllMessageHeaders(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteMessageError()
        {
            EmailDriver.DeleteMessage(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteMimeMessageError()
        {
            EmailDriver.DeleteMessage(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveMimeMessageError()
        {
            EmailDriver.MoveMailMessage(new MimeMessage(), string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void MoveMessageError()
        {
            EmailDriver.MoveMailMessage(string.Empty, string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetAttachmentsError()
        {
            EmailDriver.GetAttachments(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetMimeAttachmentsError()
        {
            EmailDriver.GetAttachments(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DownloadAttachmentsToError()
        {
            EmailDriver.DownloadAttachments(new MimeMessage(), string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DownloadAttachmentsError()
        {
            EmailDriver.DownloadAttachments(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SearchMessagesError()
        {
            EmailDriver.SearchMessages(null);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetContentTypesError()
        {
            EmailDriver.GetContentTypes(null);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetBodyByContentTypesError()
        {
            EmailDriver.GetBodyByContentTypes(null, string.Empty);
        }
    }
}
