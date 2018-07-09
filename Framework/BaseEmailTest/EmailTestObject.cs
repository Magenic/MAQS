//--------------------------------------------------
// <copyright file="EmailTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds email context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using MailKit.Net.Imap;
using System;

namespace Magenic.Maqs.BaseEmailTest
{        
    /// <summary>
    /// Email test context data
    /// </summary>
    public class EmailTestObject : BaseTestObject
    {
        /// <summary>
        /// Gets the email connection driver
        /// </summary>
        private EmailDriver driver;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTestObject" /> class
        /// </summary>
        /// <param name="emailConnection">The test's email connection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public EmailTestObject(Func<ImapClient> emailConnection, Logger logger, SoftAssert softAssert, string fullyQualifiedTestName) : base(logger, softAssert, fullyQualifiedTestName)
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
                return this.ManagerStore[typeof(EmailDriverManager).FullName] as EmailDriverManager;
            }
        }

        /// <summary>
        /// Gets the email driver
        /// </summary>
        public EmailDriver EmailDriver
        {
            get
            {
                if (this.driver != null)
                {
                    return this.driver;
                }

                return this.EmailManager.Get();
            }
        }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="emailConnection">Function for getting an email connection</param>
        public void OverrideDatabaseConnection(Func<ImapClient> emailConnection)
        {
            if (this.driver != null)
            {
                this.driver.Dispose();
                this.driver = null;
            }

            this.OverrideDriverManager(typeof(EmailDriverManager).FullName, new EmailDriverManager(emailConnection, this));
        }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="emailDriver">The new email driver</param>
        public void OverrideDatabaseDriver(EmailDriver emailDriver)
        {
            if (this.driver != null)
            {
                this.driver.Dispose();
            }

            this.driver = emailDriver;
        }
    }
}
