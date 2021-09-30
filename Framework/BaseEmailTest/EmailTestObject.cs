//--------------------------------------------------
// <copyright file="EmailTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds email context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using MailKit.Net.Imap;
using System;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Email test context data
    /// </summary>
    public class EmailTestObject : BaseTestObject, IEmailTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTestObject" /> class
        /// </summary>
        /// <param name="emailConnection">The test's email connection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public EmailTestObject(Func<ImapClient> emailConnection, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            this.ManagerStore.Add(typeof(EmailDriverManager).FullName, new EmailDriverManager(emailConnection, this));
        }

        /// <summary>
        /// Gets the email driver manager
        /// </summary>
        public EmailDriverManager EmailManager
        {
            get
            {
                return this.ManagerStore.GetManager<EmailDriverManager>();
            }
        }

        /// <summary>
        /// Gets the email driver
        /// </summary>
        public EmailDriver EmailDriver
        {
            get
            {
                return this.EmailManager.GetEmailDriver();
            }
        }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="emailConnection">Function for getting an email connection</param>
        public void OverrideEmailClient(Func<ImapClient> emailConnection)
        {
            this.EmailManager.OverrideDriver(emailConnection);
        }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="emailDriver">The new email driver</param>
        public void OverrideEmailClient(EmailDriver emailDriver)
        {
            this.EmailManager.OverrideDriver(emailDriver);
        }
    }
}
