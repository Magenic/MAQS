//--------------------------------------------------
// <copyright file="EmailDriverManagerTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver store tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit;
using MailKit.Net.Imap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Test the email driver store
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Email)]
    [ExcludeFromCodeCoverage]
    public class EmailDriverManagerTests : BaseEmailTest
    {
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
            mockForClient.Setup(x => x.GetFolder(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(new MoqMailFolder());

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
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideEmailDriver()
        {
            this.TestObject.EmailManager.OverrideDriver(() => GetMoq().Object);

            Assert.AreEqual(this.TestObject.EmailManager.GetEmailDriver().EmailConnection, EmailDriver.EmailConnection);
        }

        /// <summary>
        /// Make sure we can override the driver in a lazy fashion
        /// </summary>
        [TestMethod]
        public void CanLazyOverrideEmailDriver()
        {
            this.TestObject.EmailManager.OverrideDriver(() => GetMoq().Object);
            Assert.AreEqual(false, this.TestObject.EmailManager.IsDriverIntialized(), "Did not expect the driver to be initialized");
            Assert.AreEqual(this.TestObject.EmailManager.GetEmailDriver().EmailConnection, EmailDriver.EmailConnection);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            EmailDriverManager newDriver = new EmailDriverManager(() => GetMoq().Object, this.TestObject);
            this.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.EmailManager, (EmailDriverManager)this.ManagerStore["test"]);
            Assert.AreNotEqual(this.TestObject.EmailManager.Get(), ((EmailDriverManager)this.ManagerStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object driver is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void EmailDriverInDriverStore()
        {
            Assert.AreEqual(this.TestObject.EmailDriver, this.TestObject.GetDriverManager<EmailDriverManager>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriverManager(new WebServiceDriverManager(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriverManager<EmailDriverManager>(), "Expected a Email driver store");
            Assert.IsNotNull(this.TestObject.GetDriverManager<WebServiceDriverManager>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we initialize the driver
            this.EmailDriver.CanAccessEmailAccount();

            EmailDriverManager driverDriver = this.ManagerStore[typeof(EmailDriverManager).FullName] as EmailDriverManager;
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been initialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            EmailDriverManager driverDriver = this.ManagerStore[typeof(EmailDriverManager).FullName] as EmailDriverManager;
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be initialized until it gets used");
        }
    }
}
