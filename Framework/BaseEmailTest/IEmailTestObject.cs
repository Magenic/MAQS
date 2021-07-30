//--------------------------------------------------
// <copyright file="EmailTestObject.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Holds email context data</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using MailKit.Net.Imap;
using System;

namespace Magenic.Maqs.BaseEmailTest
{
    public interface IEmailTestObject : ITestObject
    {
        EmailDriver EmailDriver { get; }
        EmailDriverManager EmailManager { get; }

        void OverrideEmailClient(EmailDriver emailDriver);
        void OverrideEmailClient(Func<ImapClient> emailConnection);
    }
}