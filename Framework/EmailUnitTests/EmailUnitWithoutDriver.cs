//--------------------------------------------------
// <copyright file="EmailUnitWithoutDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
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
            #region GetHost
            string host = EmailConfig.GetHost();
            #endregion
            Assert.AreEqual(host, "imap.gmail.com");
        }
        
        /// <summary>
        /// Gets the username from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        [DoNotParallelize]
        public void GetUserNameTest()
        {
            // Replace the password so it doesn't need to be hardcoded in our test
            string saveName = EmailConfig.GetUserName();
            string tempName = "TEMP";

            Config.AddTestSettingValues(new Dictionary<string, string> { { "EmailUserName", tempName } }, "EmailMaqs", true);

            try
            {
                #region GetUserName
                string username = EmailConfig.GetUserName();
                #endregion
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
            // Replace the password so it doesn't need to be hardcoded in our test
            string savePass = EmailConfig.GetPassword();
            string tempPass = "TEMP";

            Config.AddTestSettingValues(new Dictionary<string, string> { { "EmailPassword", tempPass } }, "EmailMaqs", true);

            try
            {
                #region GetPassword
                string password = EmailConfig.GetPassword();
                #endregion
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
            #region GetPort
            int port = EmailConfig.GetPort();
            #endregion
            Assert.AreEqual(port, 993);
        }

        /// <summary>
        /// Gets the boolean to get emails via SSL from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailViaSSLTest()
        {
            #region GetEmailViaSSL
            bool ssl = EmailConfig.GetEmailViaSSL();
            #endregion
            Assert.AreEqual(ssl, true);
        }

        /// <summary>
        /// Checks to see if we should skip SSL validation from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetEmailSkipSSLValidationTest()
        {
            #region SkipSSL
            bool skipSsl = EmailConfig.GetEmailSkipSslValidation();
            #endregion
            Assert.AreEqual(skipSsl, true);
        }

        /// <summary>
        /// Gets the download directory from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetDownloadDirectoryTest()
        {
            #region DownloadDirectory
            string downloadDirectory = EmailConfig.GetAttachmentDownloadDirectory();
            #endregion
            Assert.AreEqual(downloadDirectory, @"C:\Frameworks\downloads");
        }
    }
}
