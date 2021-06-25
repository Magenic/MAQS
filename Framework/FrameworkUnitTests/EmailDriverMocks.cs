//--------------------------------------------------
// <copyright file="EmailDriverMocks.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver mocks</summary>
//--------------------------------------------------
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Email driver mocks
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal static class EmailDriverMocks 
    {
        /// <summary>
        /// Get mime mock
        /// </summary>
        /// <returns>A mime mock</returns>
        internal static MimeMessage GetMocMime()
        {
            Mock<MimeMessage> map = new Mock<MimeMessage>();
            map.Setup(x => x.Attachments).Throws<NotImplementedException>();
            return map.Object;
        }

        /// <summary>
        /// Setup fake email client
        /// </summary>
        /// <returns>Fake email client</returns>
        internal static Mock<ImapClient> GetMoq()
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
    }
}
