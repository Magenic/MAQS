//--------------------------------------------------
// <copyright file="EmailUnitWithoutDriver.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Unit test email driver without base email test</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseEmailTest;
using Magenic.Maqs.Utilities.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            using (EmailDriver test = new EmailDriver("imap.gmail.com", "maqsbaseemailtest@gmail.com", "Magenic3", 993, 10000, true, true))
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
        public void GetUserNameTest()
        {
            #region GetUserName
            string username = EmailConfig.GetUserName();
            #endregion
            Assert.AreEqual(username, "maqsbaseemailtest@gmail.com");
        }
        
        /// <summary>
        /// Gets the password from the config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Email)]
        public void GetPasswordTest()
        {
            #region GetPassword
            string password = EmailConfig.GetPassword();
            #endregion
            Assert.AreEqual(password, "Magenic3");
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
