//--------------------------------------------------
// <copyright file="EmailTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds email context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;
using MailKit.Net.Imap;
using System;

namespace Magenic.MaqsFramework.BaseEmailTest
{        
    /// <summary>
    /// Email test context data
    /// </summary>
    public class EmailTestObject : BaseTestObject
    {
        /// <summary>
        /// Gets the email connection wrapper
        /// </summary>
        private EmailDriver wrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTestObject" /> class
        /// </summary>
        /// <param name="emailConnection">The test's email connection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="fullyQualifiedTestName">The test's fully qualified test name</param>
        public EmailTestObject(Func<ImapClient> emailConnection, Logger logger, SoftAssert softAssert, string fullyQualifiedTestName) : base(logger, softAssert, fullyQualifiedTestName)
        {
            this.DriversStore.Add(typeof(EmailDriverStore).FullName, new EmailDriverStore(emailConnection, this));
        }

        /// <summary>
        /// Gets the email driver
        /// </summary>
        public EmailDriverStore EmailDriver
        {
            get
            {
                return this.DriversStore[typeof(EmailDriverStore).FullName] as EmailDriverStore;
            }
        }

        /// <summary>
        /// Gets the email wrapper
        /// </summary>
        public EmailDriver EmailWrapper
        {
            get
            {
                if (this.wrapper != null)
                {
                    return this.wrapper;
                }

                return (this.DriversStore[typeof(EmailDriverStore).FullName] as EmailDriverStore).Get();
            }
        }

        /// <summary>
        /// Override the email wrapper
        /// </summary>
        /// <param name="emailConnection">Function for getting an email connection</param>
        public void OverrideDatabaseConnection(Func<ImapClient> emailConnection)
        {
            if (this.wrapper != null)
            {
                this.wrapper.Dispose();
                this.wrapper = null;
            }

            this.OverrideDriver(typeof(EmailDriverStore).FullName, new EmailDriverStore(emailConnection, this));
        }

        /// <summary>
        /// Override the email wrapper
        /// </summary>
        /// <param name="emailWrapper">The new email wrapper</param>
        public void OverrideDatabaseWrapper(EmailDriver emailWrapper)
        {
            if (this.wrapper != null)
            {
                this.wrapper.Dispose();
            }

            this.wrapper = emailWrapper;
        }
    }
}
