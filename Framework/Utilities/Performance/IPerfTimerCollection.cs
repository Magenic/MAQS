//--------------------------------------------------
// <copyright file="IPerfTimerCollection.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Interface for performance timer collection</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using System.Collections.Generic;

namespace Magenic.Maqs.Utilities.Performance
{
    /// <summary>
    /// Performance timer collection interface
    /// </summary>
    public interface IPerfTimerCollection
    {
        /// <summary>
        /// Gets or sets the File name
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets the generic payload string
        /// </summary>
        string PerfPayloadString { get; set; }

        /// <summary>
        /// Gets or sets the test name
        /// </summary>
        string TestName { get; set; }

        /// <summary>
        /// Gets or sets the test name
        /// </summary>
        List<PerfTimer> Timerlist { get; }

        /// <summary>
        /// Method to Read in the Performance Timer Collection from disk
        /// </summary>
        /// <param name="filepath">The file from which to initialize</param>
        /// <returns> <see cref="PerfTimerCollection"/> initialized from file path</returns>
        IPerfTimerCollection LoadPerfTimerCollection(string filepath);

        /// <summary>
        /// Method to stop an existing timer with a specified name for a test
        /// </summary>
        /// <param name="timerName">Name of the timer</param>
        void StartTimer(string timerName);

        /// <summary>
        /// Method to start a timer with a specified name and for a specific context
        /// </summary>
        /// <param name="contextName">Name of the context</param>
        /// <param name="timerName">Name of the timer</param>
        void StartTimer(string contextName, string timerName);

        /// <summary>
        /// Method to stop an existing timer with a specified name for a test
        /// </summary>
        /// <param name="timerName">Name of the timer</param>
        void StopTimer(string timerName);

        /// <summary>
        /// Method to Write the Performance Timer Collection to disk
        /// </summary>
        /// <param name="log">The current test Logger</param>
        void Write(ILogger log);
    }
}