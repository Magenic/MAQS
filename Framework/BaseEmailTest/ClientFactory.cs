//--------------------------------------------------
// <copyright file="ClientFactory.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Factory for getting email client connections</summary>
//--------------------------------------------------
using MailKit.Net.Imap;

namespace Magenic.Maqs.BaseEmailTest
{
    /// <summary>
    /// Email client factory
    /// </summary>
    public static class ClientFactory
    {
        /// <summary>
        /// Get the email client using connection information from the test run configuration 
        /// </summary>
        /// <returns>The email connection</returns>
        public static ImapClient GetDefaultEmailClient()
        {
            string host = EmailConfig.GetHost();
            string username = EmailConfig.GetUserName();
            string password = EmailConfig.GetPassword();
            int port = EmailConfig.GetPort();
            bool isSSL = EmailConfig.GetEmailViaSSL();
            bool skipSslCheck = EmailConfig.GetEmailSkipSslValidation();
            int serverTimeout = EmailConfig.GetTimeout();

            return GetEmailClient(host, username, password, port, serverTimeout, isSSL, skipSslCheck);
        }

        /// <summary>
        /// Get the email client for the given connection values
        /// </summary>
        /// <param name="host">The email server host</param>
        /// <param name="username">Email user name</param>
        /// <param name="password">Email user password</param>
        /// <param name="port">Email server port</param>
        /// <param name="serverTimeout">Timeout for the email server</param>
        /// <param name="isSSL">Should SSL be used</param>
        /// <param name="skipSslCheck">Skip the SSL check</param>
        /// <returns>The email connection</returns>
        public static ImapClient GetEmailClient(string host, string username, string password, int port, int serverTimeout = 10000, bool isSSL = true, bool skipSslCheck = false)
        {
            // Get the email connection and make sure it is live
            var client = new ImapClient
            {
                ServerCertificateValidationCallback = (s, c, h, e) => skipSslCheck
            };
            client.Connect(host, port, isSSL);
            client.Authenticate(username, password);
            client.Timeout = serverTimeout;

            return client;
        }
    }
}
