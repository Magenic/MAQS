//--------------------------------------------------
// <copyright file="EmailDriverFailureTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver failure tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MimeKit;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

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
        /// Get mime mock
        /// </summary>
        /// <returns>A mime mock</returns>
        private MimeMessage GetMocMime()
        {
            Mock<MimeMessage> map = new Mock<MimeMessage>();
            map.Setup(x => x.Attachments).Throws<NotImplementedException>();
            return map.Object;
        }

        /// <summary>
        /// Setup fake email client
        /// </summary>
        /// <returns>Fake email client</returns>
        private Mock<ImapClient> GetMoq()
        {
            Mock<ImapClient> mockForClient = new Mock<ImapClient>();

            var collection = new FolderNamespaceCollection();
            IList<IMailFolder> folders = new List<IMailFolder>
            {
                new MoqMailFolder()
            };

            collection.Add(new FolderNamespace('/', ""));

            mockForClient.Setup(x => x.PersonalNamespaces).Returns(collection);
            mockForClient.Setup(x => x.GetFolders(It.IsAny<FolderNamespace>(), It.IsAny<StatusItems>(), It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(folders);
            mockForClient.Setup(x => x.GetFolder(It.IsNotIn<string>(string.Empty), It.IsAny<CancellationToken>())).Returns(new MoqMailFolder());
            mockForClient.Setup(x => x.GetFolder(It.IsIn<string>(string.Empty), It.IsAny<CancellationToken>())).Throws<NotImplementedException>();
            return mockForClient;
        }

        /// <summary>
        /// Setup a fake email client
        /// </summary>
        [TestInitialize]
        public void SetupMoqDriver()
        {
            this.TestObject.OverrideEmailClient(() => GetMoq().Object);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void MailBoxNamesError()
        {
            EmailDriver.GetMailBoxNames();
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void MailoxError()
        {
            EmailDriver.GetMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void SelectBoxError()
        {
            EmailDriver.SelectMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateMailboxError()
        {
            EmailDriver.CreateMailbox(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void GetMessageError()
        {
            EmailDriver.GetMessage(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void GetAllMessageHeadersError()
        {
            EmailDriver.GetAllMessageHeaders(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void DeleteMessageError()
        {
            EmailDriver.DeleteMessage(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void DeleteMimeMessageError()
        {
            EmailDriver.DeleteMessage(new MimeMessage());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MoveMimeMessageError()
        {
            EmailDriver.MoveMailMessage(new MimeMessage(), string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void MoveMessageError()
        {
            EmailDriver.MoveMailMessage(string.Empty, string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetAttachmentsError()
        {
            EmailDriver.GetAttachments(string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void GetMimeAttachmentsError()
        {
            EmailDriver.GetAttachments(GetMocMime());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DownloadAttachmentsToError()
        {
            EmailDriver.DownloadAttachments(GetMocMime(), string.Empty);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void DownloadAttachmentsError()
        {
            EmailDriver.DownloadAttachments(GetMocMime());
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(TimeoutException))]
        public void SearchMessagesError()
        {
            EmailDriver.SearchMessages(null);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetContentTypesError()
        {
            EmailDriver.GetContentTypes(null);
        }

        /// <summary>
        /// Make sure email driver throws the correct exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetBodyByContentTypesError()
        {
            EmailDriver.GetBodyByContentTypes(null, string.Empty);
        }
    }
}
