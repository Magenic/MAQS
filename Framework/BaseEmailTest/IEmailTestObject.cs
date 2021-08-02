//--------------------------------------------------
// <copyright file="IEmailTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds email test object interface</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using MailKit.Net.Imap;
using System;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Email test object interface
    /// </summary>
    public interface IEmailTestObject : ITestObject
    {
        /// <summary>
        /// Gets the email driver
        /// </summary>
        EmailDriver EmailDriver { get; }

        /// <summary>
        /// Gets the email driver manager
        /// </summary>
        EmailDriverManager EmailManager { get; }

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="emailDriver">The new email driver</param>
        void OverrideEmailClient(EmailDriver emailDriver);

        /// <summary>
        /// Override the email driver
        /// </summary>
        /// <param name="emailConnection">Function for getting an email connection</param>
        void OverrideEmailClient(Func<ImapClient> emailConnection);
    }
}