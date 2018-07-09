//--------------------------------------------------
// <copyright file="EmailDriverManager.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using MailKit.Net.Imap;
using System;
using System.Data;

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
        /// Cleanup after the email connection
        /// </summary>
        public override void Dispose()
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
        /// Override the email driver
        /// </summary>
        /// <param name="newDriver">The new driver</param>
        public void OverwriteDriver(EmailDriver newDriver)
        {
            this.driver = newDriver;
            this.BaseDriver = newDriver.EmailConnection;
        }

        /// <summary>
        /// Get the email connection driver
        /// </summary>
        /// <returns>The email connection driver</returns>
        public new EmailDriver Get()
        {
            if (this.driver == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting email driver");
                    this.driver = new EmailDriver(() => base.Get() as ImapClient);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing email driver");
                    this.driver = new EventFiringEmailDriver(() => base.Get() as ImapClient);
                    this.MapEvents(this.driver as EventFiringEmailDriver);
                }
            }

            return this.driver;
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
