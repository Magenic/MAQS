//--------------------------------------------------
// <copyright file="BaseEmailTest.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base email test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using MailKit.Net.Imap;
using System;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Generic base email test class
    /// </summary>
    public class BaseEmailTest : BaseExtendableTest<EmailTestObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEmailTest"/> class.
        /// Setup the email client for each test class
        /// </summary>
        public BaseEmailTest()
        {
        }

        /// <summary>
        /// Gets or sets the email wrapper
        /// </summary>
        public EmailDriver EmailWrapper
        {
            get
            {
                return this.TestObject.EmailWrapper;
            }

            set
            {
                this.TestObject.OverrideDatabaseWrapper(value);
            }
        }

        /// <summary>
        /// Get the email connection
        /// </summary>
        /// <returns>The email connection</returns>
        protected virtual ImapClient GetEmailConnection()
        {
            bool loggingEnabled = this.LoggingEnabledSetting != LoggingEnabled.NO;

            string host = EmailConfig.GetHost();
            string username = EmailConfig.GetUserName();
            string password = EmailConfig.GetPassword();
            int port = EmailConfig.GetPort();
            bool isSSL = EmailConfig.GetEmailViaSSL();
            bool skipSslCheck = EmailConfig.GetEmailSkipSslValidation();

            if (loggingEnabled)
            {
                this.Log.LogMessage(
                    MessageType.INFORMATION,
                    StringProcessor.SafeFormatter("Connect to email with user '{0}' on host '{1}', port '{2}'", username, host, port));
            }

            ImapClient emailConnection = new ImapClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => skipSslCheck
            };
            emailConnection.Connect(host, port, isSSL);
            emailConnection.Authenticate(username, password);
            emailConnection.Timeout = EmailConfig.GetTimeout();

            emailConnection.NoOp();

            if (loggingEnabled)
            {
                this.Log.LogMessage(MessageType.INFORMATION, "Connected to email account");
            }

            return emailConnection;
        }

        /// <summary>
        /// Create an email test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            Logger newLogger = this.CreateLogger();
            this.TestObject = new EmailTestObject(() => this.GetEmailConnection(), newLogger, new SoftAssert(newLogger), this.GetFullyQualifiedTestClassName());
        }
    }
}