//--------------------------------------------------
// <copyright file="EmailDriverManagerTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver store tests</summary>
//-------------------------------------------------- 
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.WebServiceTester;
using MailKit.Net.Imap;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;

namespace CoreUnitTests
{
    /// <summary>
    /// Test the email driver store
    /// </summary>
    [TestClass]
    [DoNotParallelize]
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

            Assert.AreEqual(this.TestObject.EmailManager.Get().EmailConnection, EmailDriver.EmailConnection);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            EmailDriverManager newDriver = new EmailDriverManager(() => GetClient(), this.TestObject);
            this.TestObject.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.EmailManager, (EmailDriverManager)this.TestObject.ManagerStore["test"]);
            Assert.AreNotEqual(this.TestObject.EmailManager.Get(), ((EmailDriverManager)this.TestObject.ManagerStore["test"]).Get());
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
            // Do something so we initalize the driver
            this.EmailDriver.CanAccessEmailAccount();

            EmailDriverManager driverDriver = this.TestObject.ManagerStore[typeof(EmailDriverManager).FullName] as EmailDriverManager;
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            EmailDriverManager driverDriver = this.TestObject.ManagerStore[typeof(EmailDriverManager).FullName] as EmailDriverManager;
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be intialized until it gets used");
        }

        /// <summary>
        /// Get an email connection
        /// </summary>
        /// <returns>An email connection</returns>
        private static ImapClient GetClient()
        {
            ImapClient client = new ImapClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => true
            };
            client.Connect("imap.gmail.com", 993, true);
            client.Authenticate("maqsbaseemailtest@gmail.com", "Magenic3");
            client.Timeout = 10000;

            return client;
        }
    }
}
