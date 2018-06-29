//--------------------------------------------------
// <copyright file="EmailDriverStoreTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver store tests</summary>
//-------------------------------------------------- 
using Magenic.MaqsFramework.BaseEmailTest;
using Magenic.MaqsFramework.WebServiceTester;
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
    public class EmailDriverStoreTests : BaseEmailTest
    {
        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideEmailDriver()
        {
            EmailDriver temp = new EmailDriver(() => GetClient());
            this.TestObject.EmailDriver.OverwriteWrapper(temp);

            Assert.AreEqual(this.TestObject.EmailDriver.Get().EmailConnection, EmailWrapper.EmailConnection);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            EmailDriverStore newDriver = new EmailDriverStore(() => GetClient(), this.TestObject);
            this.TestObject.DriversStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.EmailDriver, (EmailDriverStore)this.TestObject.DriversStore["test"]);
            Assert.AreNotEqual(this.TestObject.EmailDriver.Get(), ((EmailDriverStore)this.TestObject.DriversStore["test"]).Get());
        }

        /// <summary>
        /// Make sure the test object wrapper is the same as the one in the driver store
        /// </summary>
        [TestMethod]
        public void EmailWrapperInDriverStore()
        {
            Assert.AreEqual(this.TestObject.EmailWrapper, this.TestObject.GetDriver<EmailDriverStore>().Get());
        }

        /// <summary>
        /// Make sure we can add different driver types
        /// </summary>
        [TestMethod]
        public void MixedStoreTypes()
        {
            this.TestObject.AddDriver(new WebServiceDriverStore(() => new HttpClient(), this.TestObject));

            Assert.IsNotNull(this.TestObject.GetDriver<EmailDriverStore>(), "Expected a Email driver store");
            Assert.IsNotNull(this.TestObject.GetDriver<WebServiceDriverStore>(), "Expected a web service driver store");
        }

        /// <summary>
        /// Make sure the driver is  initialized if we use it
        /// </summary>
        [TestMethod]
        public void Intialized()
        {
            // Do something so we initalize the driver
            this.EmailWrapper.CanAccessEmailAccount();

            EmailDriverStore driverWrapper = this.TestObject.DriversStore[typeof(EmailDriverStore).FullName] as EmailDriverStore;
            Assert.IsTrue(driverWrapper.IsDriverIntialized(), "The driver should have been intialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            EmailDriverStore driverWrapper = this.TestObject.DriversStore[typeof(EmailDriverStore).FullName] as EmailDriverStore;
            Assert.IsFalse(driverWrapper.IsDriverIntialized(), "The driver should not be intialized until it gets used");
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
