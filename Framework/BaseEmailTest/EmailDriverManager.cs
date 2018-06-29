//--------------------------------------------------
// <copyright file="EmailDriverManager.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Email driver</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using MailKit.Net.Imap;
using System;
using System.Data;

namespace Magenic.MaqsFramework.BaseEmailTest
{
    /// <summary>
    /// Email driver
    /// </summary>
    public class EmailDriverManager : DriverManager
    {
        /// <summary>
        /// Cached instance of the email connection wrapper
        /// </summary>
        private EmailDriver wrapper;

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
            if (this.wrapper != null)
            {
                (this.BaseDriver as ImapClient).Dispose();
                this.wrapper.Dispose(); 
            }

            this.wrapper = null;
            this.BaseDriver = null;
        }

        /// <summary>
        /// Override the email wrapper
        /// </summary>
        /// <param name="newWrapper">The new wrapper</param>
        public void OverwriteWrapper(EmailDriver newWrapper)
        {
            this.wrapper = newWrapper;
            this.BaseDriver = newWrapper.EmailConnection;
        }

        /// <summary>
        /// Get the email connection wrapper
        /// </summary>
        /// <returns>The email connection wrapper</returns>
        public new EmailDriver Get()
        {
            if (this.wrapper == null)
            {
                if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting email wrapper");
                    this.wrapper = new EmailDriver(() => base.Get() as ImapClient);
                }
                else
                {
                    this.Log.LogMessage(MessageType.INFORMATION, "Getting event firing email wrapper");
                    this.wrapper = new EventFiringEmailDriver(() => base.Get() as ImapClient);
                    this.MapEvents(this.wrapper as EventFiringEmailDriver);
                }
            }

            return this.wrapper;
        }

        /// <summary>
        /// Map email events to log events
        /// </summary>
        /// <param name="eventFiringConnectionWrapper">The event firing email wrapper that we want mapped</param>
        private void MapEvents(EventFiringEmailDriver eventFiringConnectionWrapper)
        {
            eventFiringConnectionWrapper.EmailEvent += this.Email_Event;
            eventFiringConnectionWrapper.EmailErrorEvent += this.Email_Error;
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
