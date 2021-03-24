//--------------------------------------------------
// <copyright file="BaseEmailTest.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base email test class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Data;
using Magenic.Maqs.Utilities.Logging;
using MailKit.Net.Imap;

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
        /// Gets or sets the email driver
        /// </summary>
        public EmailDriver EmailDriver
        {
            get
            {
                return this.TestObject.EmailDriver;
            }

            set
            {
                this.TestObject.OverrideEmailClient(value);
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
            int port = EmailConfig.GetPort();

            if (loggingEnabled)
            {
                this.Log.LogMessage(
                    MessageType.INFORMATION,
                    StringProcessor.SafeFormatter($"Connect to email with user '{username}' on host '{host}', port '{port}'"));
            }

            ImapClient emailConnection = ClientFactory.GetDefaultEmailClient();
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