//--------------------------------------------------
// <copyright file="EmailUnitWithoutDriver.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test email driver without base email test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace EmailUnitTests
{
    /// <summary>
    /// Test basic email testing without a base test
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    [ExcludeFromCodeCoverage]
    public class EmailUnitWithoutDriver
    {
        /// <summary>
        /// Make sure we can connect without extending the base email test
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void BasicConnectionTest()
        {
            string host = EmailConfig.GetHost();
            string username = EmailConfig.GetUserName();
            string password = EmailConfig.GetPassword();
            int port = EmailConfig.GetPort();

            using (EmailDriver test = new EmailDriver(host, username, password, port, 10000, true, true))
            {
                test.EmailConnection.NoOp();
            }
        }

        /// <summary>
        /// Gets the host from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetHostTest()
        {
            string host = EmailConfig.GetHost();
            Assert.AreEqual("imap.gmail.com", host);
        }
        
        /// <summary>
        /// Gets the username from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [DoNotParallelize]
        public void GetUserNameTest()
        {
            // Replace the password so it doesn't need to be hard-coded in our test
            string saveName = EmailConfig.GetUserName();
            string tempName = "TEMP";

            Config.AddTestSettingValues(new Dictionary<string, string> { { "EmailUserName", tempName } }, "EmailMaqs", true);

            try
            {
                string username = EmailConfig.GetUserName();
                Assert.AreEqual(username, tempName);
            }
            finally
            {
                Config.AddTestSettingValues(new Dictionary<string, string> { { "EmailUserName", saveName } }, "EmailMaqs", true);
            }
        }
        
        /// <summary>
        /// Gets the password from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [DoNotParallelize]
        public void GetPasswordTest()
        {
            // Replace the password so it doesn't need to be hard-coded in our test
            string savePass = EmailConfig.GetPassword();
            string tempPass = "TEMP";

            Config.AddTestSettingValues(new Dictionary<string, string> { { "EmailPassword", tempPass } }, "EmailMaqs", true);

            try
            {
                string password = EmailConfig.GetPassword();
                Assert.AreEqual(password, tempPass);
            }
            finally
            {
                Config.AddTestSettingValues(new Dictionary<string, string> { { "EmailPassword", savePass } }, "EmailMaqs", true);
            }
        }
        
        /// <summary>
        /// Gets the port from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetPortTest()
        {
            int port = EmailConfig.GetPort();
            Assert.AreEqual(993, port);
        }

        /// <summary>
        /// Gets the boolean to get emails via SSL from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailViaSSLTest()
        {
            bool ssl = EmailConfig.GetEmailViaSSL();
            Assert.AreEqual(true, ssl);
        }

        /// <summary>
        /// Checks to see if we should skip SSL validation from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailSkipSSLValidationTest()
        {
            bool skipSsl = EmailConfig.GetEmailSkipSslValidation();
            Assert.AreEqual(true, skipSsl);
        }

        /// <summary>
        /// Gets the download directory from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetDownloadDirectoryTest()
        {
            string downloadDirectory = EmailConfig.GetAttachmentDownloadDirectory();
            Assert.AreEqual(@"C:\Frameworks\downloads", downloadDirectory);
        }
    }
}
