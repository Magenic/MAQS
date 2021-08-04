//--------------------------------------------------
// <copyright file="EmailDriverManagerTests.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver store tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.BaseWebServiceTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;

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
        /// Setup a fake email client
        /// </summary>
        [TestInitialize]
        public void SetupMoqDriver()
        {
            this.TestObject.OverrideEmailClient(() => EmailDriverMocks.GetMoq().Object);
        }

        /// <summary>
        /// Make sure we can override the driver
        /// </summary>
        [TestMethod]
        public void CanOverrideEmailDriver()
        {
            this.TestObject.EmailManager.OverrideDriver(() => EmailDriverMocks.GetMoq().Object);

            Assert.AreEqual(this.TestObject.EmailManager.GetEmailDriver().EmailConnection, EmailDriver.EmailConnection);
        }

        /// <summary>
        /// Make sure we can override the driver in a lazy fashion
        /// </summary>
        [TestMethod]
        public void CanLazyOverrideEmailDriver()
        {
            this.TestObject.EmailManager.OverrideDriver(() => EmailDriverMocks.GetMoq().Object);
            Assert.AreEqual(false, this.TestObject.EmailManager.IsDriverIntialized(), "Did not expect the driver to be initialized");
            Assert.AreEqual(this.TestObject.EmailManager.GetEmailDriver().EmailConnection, EmailDriver.EmailConnection);
        }

        /// <summary>
        /// Check that we can add multiples of the same driver type, provided we use a key
        /// </summary>
        [TestMethod]
        public void CanUseMultiple()
        {
            EmailDriverManager newDriver = new EmailDriverManager(() => EmailDriverMocks.GetMoq().Object, this.TestObject);
            this.ManagerStore.Add("test", newDriver);

            Assert.AreNotEqual(this.TestObject.EmailManager, this.ManagerStore.GetManager<EmailDriverManager>("test"));
            Assert.AreNotEqual(this.TestObject.EmailManager.Get(), (this.ManagerStore.GetManager<EmailDriverManager>("test")).Get());
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

            EmailDriverManager driverDriver = this.ManagerStore.GetManager<EmailDriverManager>();
            Assert.IsTrue(driverDriver.IsDriverIntialized(), "The driver should have been initialized");
        }

        /// <summary>
        /// Make sure the driver is not initialized if we don't use it
        /// </summary>
        [TestMethod]
        public void NotIntialized()
        {
            EmailDriverManager driverDriver = this.ManagerStore.GetManager<EmailDriverManager>();
            Assert.IsFalse(driverDriver.IsDriverIntialized(), "The driver should not be initialized until it gets used");
        }
    }
}
