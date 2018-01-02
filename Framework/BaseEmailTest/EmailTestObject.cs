//--------------------------------------------------
// <copyright file="EmailTestObject.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Holds email context data</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.BaseTest;
using Magenic.MaqsFramework.Utilities.Logging;
using Magenic.MaqsFramework.Utilities.Performance;

namespace Magenic.MaqsFramework.BaseEmailTest
{
    /// <summary>
    /// Email test context data
    /// </summary>
    public class EmailTestObject : BaseTestObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailTestObject" /> class
        /// </summary>
        /// <param name="emailConnection">The test's email connection</param>
        /// <param name="logger">The test's logger</param>
        /// <param name="softAssert">The test's soft assert</param>
        /// <param name="perfTimerCollection">The test's performance timer collection</param>
        public EmailTestObject(EmailConnectionWrapper emailConnection, Logger logger, SoftAssert softAssert, PerfTimerCollection perfTimerCollection) : base(logger, softAssert, perfTimerCollection)
        {
            this.EmailWrapper = emailConnection;
        }

        /// <summary>
        /// Gets the email connection wrapper
        /// </summary>
        public EmailConnectionWrapper EmailWrapper { get; private set; }
    }
}
