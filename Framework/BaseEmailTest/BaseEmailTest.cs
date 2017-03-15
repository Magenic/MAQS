//--------------------------------------------------
// <copyright file="BaseEmailTest.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>This is the base email test class</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Helper;
using Magenic.MaqsFramework.Utilities.Logging;
using MailKit.Net.Imap;
using System;

namespace Magenic.MaqsFramework.BaseEmailTest
{
    /// <summary>
    /// Generic base email test class
    /// </summary>
    public class BaseEmailTest : BaseExtendableTest<EmailConnectionWrapper, EmailTestObject>
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
        public EmailConnectionWrapper EmailWrapper
        {
            get
            {
                return (EmailConnectionWrapper)this.ObjectUnderTest;
            }

            set
            {
                this.ObjectUnderTest = value;
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

            ImapClient emailConnection = new ImapClient();
            emailConnection.ServerCertificateValidationCallback = (s, c, h, e) => skipSslCheck;
            emailConnection.Connect(host, port, isSSL);
            emailConnection.Authenticate(username, password);
            emailConnection.Timeout = Convert.ToInt32(Config.GetValue("Timeout", "10000"));

            emailConnection.NoOp();

            if (loggingEnabled)
            {
                this.Log.LogMessage(MessageType.INFORMATION, "Connected to email account");
            }

            return emailConnection;
        }

        /// <summary>
        /// Close the email connection
        /// </summary>
        /// <param name="resultType">The test result</param>
        protected override void BeforeLoggingTeardown(TestResultType resultType)
        {
            if (this.IsObjectUnderTestStored())
            {
                this.EmailWrapper.Dispose();
            }
        }

        /// <summary>
        /// Setup the event firing email connection
        /// </summary>
        protected override void SetupEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting event logging email wrapper");
            this.EmailWrapper = new EventFiringEmailConnectionWrapper(this.GetEmailConnection);
            this.MapEvents((EventFiringEmailConnectionWrapper)this.EmailWrapper);
        }

        /// <summary>
        /// Setup the normal email connection - the none event firing implementation
        /// </summary>
        protected override void SetupNoneEventFiringTester()
        {
            this.Log.LogMessage(MessageType.INFORMATION, "Getting email wrapper");
            this.EmailWrapper = new EmailConnectionWrapper(this.GetEmailConnection);
        }

        /// <summary>
        /// Create an email test object
        /// </summary>
        protected override void CreateNewTestObject()
        {
            this.TestObject = new EmailTestObject(this.EmailWrapper, this.Log, this.SoftAssert, this.PerfTimerCollection);
        }

        /// <summary>
        /// Map email events to log events
        /// </summary>
        /// <param name="eventFiringConnectionWrapper">The event firing email wrapper that we want mapped</param>
        private void MapEvents(EventFiringEmailConnectionWrapper eventFiringConnectionWrapper)
        {
            if (this.LoggingEnabledSetting == LoggingEnabled.YES || this.LoggingEnabledSetting == LoggingEnabled.ONFAIL)
            {
                eventFiringConnectionWrapper.EmailEvent += this.Email_Event;
                eventFiringConnectionWrapper.EmailErrorEvent += this.Email_Error;
            }
        }

        /// <summary>
        /// Email event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void Email_Event(object sender, string message)
        {
            this.Log.LogMessage(MessageType.INFORMATION, message);
        }

        /// <summary>
        /// Email error event
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="message">The logging message</param>
        private void Email_Error(object sender, string message)
        {
            this.Log.LogMessage(MessageType.ERROR, message);
        }
    }
}