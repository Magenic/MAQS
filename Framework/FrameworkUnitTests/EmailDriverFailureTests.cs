//--------------------------------------------------
// <copyright file="EmailDriverFailureTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver failure tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using System.Diagnostics.CodeAnalysis;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Test the email driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Email)]
    [ExcludeFromCodeCoverage]
    public class EmailDriverFailureTests : BaseEmailTest
    {
        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void AccessError()
        {
            EmailDriver.CanAccessEmailAccount();
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void MailBoxNamesError()
        {
            EmailDriver.GetMailBoxNames();
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void MailoxError()
        {
            EmailDriver.GetMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void SelectBoxError()
        {
            EmailDriver.SelectMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void CreateMailboxError()
        {
            EmailDriver.CreateMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void GetMessageError()
        {
            EmailDriver.GetMessage(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void GetAllMessageHeadersError()
        {
            EmailDriver.GetAllMessageHeaders(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void DeleteMessageError()
        {
            EmailDriver.DeleteMessage(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void DeleteMimeMessageError()
        {
            EmailDriver.DeleteMessage(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void MoveMimeMessageError()
        {
            EmailDriver.MoveMailMessage(new MimeMessage(), string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void MoveMessageError()
        {
            EmailDriver.MoveMailMessage(string.Empty, string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void GetAttachmentsError()
        {
            EmailDriver.GetAttachments(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void GetMimeAttachmentsError()
        {
            EmailDriver.GetAttachments(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void DownloadAttachmentsToError()
        {
            EmailDriver.DownloadAttachments(new MimeMessage(), string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void DownloadAttachmentsError()
        {
            EmailDriver.DownloadAttachments(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void SearchMessagesError()
        {
            EmailDriver.SearchMessages(null);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void GetContentTypesError()
        {
            EmailDriver.GetContentTypes(null);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(AuthenticationException))]
        public void GetBodyByContentTypesError()
        {
            EmailDriver.GetBodyByContentTypes(null, string.Empty);
        }
    }
}
