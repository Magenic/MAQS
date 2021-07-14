//--------------------------------------------------
// <copyright file="PerfTimerCollection.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Performance Timer Collection Class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using System.Collections.Generic;

namespace Magenic.Maqs.Utilities.Performance
{
    public interface IPerfTimerCollection
    {
        string FileName { get; set; }
        string PerfPayloadString { get; set; }
        string TestName { get; set; }
        List<PerfTimer> Timerlist { get; }

        void EndTimer(string timerName);
        IPerfTimerCollection LoadPerfTimerCollection(string filepath);
        void StartTimer(string timerName);
        void StartTimer(string contextName, string timerName);
        void StopTimer(string timerName);
        void Write(ILogger log);
    }
}