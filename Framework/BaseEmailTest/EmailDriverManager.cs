//--------------------------------------------------
// <copyright file="EmailDriverManager.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using MailKit.Net.Imap;
using System;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Email driver
    /// </summary>
    public class EmailDriverManager : DriverManager
    {
        /// <summary>
        /// Cached instance of the email connection driver
        /// </summary>
        private EmailDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailDriverManager"/> class
        /// </summary>
        /// <param name="getEmailClient">Function for getting an email connection</param>
        /// <param name="testObject">The associated test object</param>
        public EmailDriverManager(Func<ImapClient> getEmailClient, BaseTestObject testObject) : base(getEmailClient, testObject)
        {
        }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="newDriver">The new driver</param>
        [Obsolete("Change to OverrideDriver for consistency")]
        public void OverwriteDriver(EmailDriver newDriver)
        {
            this.OverrideDriver(newDriver);
        }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="newDriver">The new driver</param>
        public void OverrideDriver(EmailDriver newDriver)
        {
            this.driver = newDriver;
        }

        /// <summary>
        /// Override the email driver - respects lazy loading
        /// </summary>
        /// <param name="overrideDriver">The new email driver</param>
        public void OverrideDriver(Func<ImapClient> overrideDriver)
        {
            this.driver = null;
            this.OverrideDriverGet(overrideDriver);
        }

        /// <summary>
        /// Get the email connection driver
        /// </summary>
        /// <returns>The email connection driver</returns>
        public EmailDriver GetEmailDriver()
        {
            if (this.driver == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting email driver");
                    this.driver = new EmailDriver(() => GetBase() as ImapClient);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing email driver");
                    this.driver = new EventFiringEmailDriver(() => GetBase() as ImapClient);
                    this.MapEvents(this.driver as EventFiringEmailDriver);
                }
            }

            return this.driver;
        }

        /// <summary>
        /// Get the email driver
        /// </summary>
        /// <returns>The email driver</returns>
        public override object Get()
        {
            return this.GetEmailDriver();
        }

        /// <summary>
        /// Cleanup after the email connection
        /// </summary>
        protected override void DriverDispose()
        {
            if (this.driver != null)
            {
                (this.BaseDriver as ImapClient).Dispose();
                this.driver.Dispose();
            }

            this.driver = null;
            this.BaseDriver = null;
        }

        /// <summary>
        /// Map email events to log events
        /// </summary>
        /// <param name="eventFiringConnectionDriver">The event firing email driver that we want mapped</param>
        private void MapEvents(EventFiringEmailDriver eventFiringConnectionDriver)
        {
            eventFiringConnectionDriver.EmailEvent += this.Email_Event;
            eventFiringConnectionDriver.EmailErrorEvent += this.Email_Error;
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
