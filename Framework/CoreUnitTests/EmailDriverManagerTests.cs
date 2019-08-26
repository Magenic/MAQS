//--------------------------------------------------
// <copyright file="EmailDriverManagerTests.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver store tests</summary>
//-------------------------------------------------- 
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using MailKit.Net.Imap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the email driver store
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    [TestCategory(TestCategories.Email)]
    [ExcludeFromCodeCoverage]
    public class EmailDriverManagerTests : BaseEmailTest
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideEmailDriver()
        {
            EmailDriver temp = new EmailDriver(() => GetClient());
            this.TestObject.EmailManager.OverwriteDriver(temp);

            Assert.AreEqual(this.TestObject.EmailManager.GetEmailDriver().EmailConnection, EmailDriver.EmailConnection);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            EmailDriverManager newDriver = new EmailDriverManager(() => GetClient(), this.TestObject);
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

        /// <summary>
        /// Get an email connection
        /// </summary>
        /// <returns>An email connection</returns>
        private static ImapClient GetClient()
        {
            string host = EmailConfig.GetHost();
            string username = EmailConfig.GetUserName();
            string password = EmailConfig.GetPassword();

            return ClientFactory.GetEmailClient(host, username, password, 993, 10000, true, true);
        }
    }
}
