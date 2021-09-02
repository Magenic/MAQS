//--------------------------------------------------
// <copyright file="PerfTimerCollection.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Performance Timer Collection Class</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Magenic.Maqs.Utilities.Performance
{
    /// <summary>
    /// Response timer collection class -  Object to be owned by Test Class (Object), and passed to page Constructors to insert Performance Timers 
    /// /// </summary>
    public class PerfTimerCollection : IPerfTimerCollection
    {
        /// <summary>
        /// Locker object so the Performance Timer Document save doesn't save at the same time
        /// </summary>
        private static readonly object writerLocker = new object();

        /// <summary>
        /// List object to store Timers 
        /// </summary>
        private readonly Dictionary<string, PerfTimer> openTimerList = new Dictionary<string, PerfTimer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfTimerCollection"/> class
        /// </summary>
        /// <param name="logger">Logger to use</param>
        /// <param name="fullyQualifiedTestName">Test name</param>
        public PerfTimerCollection(ILogger logger, string fullyQualifiedTestName)
        {
            this.Log = logger;
            this.TestName = fullyQualifiedTestName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfTimerCollection"/> class
        /// </summary>
        /// <param name="fullyQualifiedTestName">Test name</param>
        public PerfTimerCollection(string fullyQualifiedTestName)
        {
            this.Log = new ConsoleLogger();
            this.TestName = fullyQualifiedTestName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerfTimerCollection"/> class
        /// </summary>
        public PerfTimerCollection()
        {
            this.Log = new ConsoleLogger();
        }

        /// <summary>
        /// Gets or sets the test name
        /// </summary>
        public List<PerfTimer> Timerlist { get; } = new List<PerfTimer>();

        /// <summary>
        /// Gets or sets the File name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the test name
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Gets or sets the generic payload string
        /// </summary>
        public string PerfPayloadString { get; set; } = string.Empty;

        /// <summary>
        /// Gets the logger
        /// </summary>
        protected ILogger Log { get; private set; }

        /// <summary>
        /// Method to start a timer with a specified name
        /// </summary>
        /// <param name="timerName">Name of the timer</param>
        public void StartTimer(string timerName)
        {
            this.StartTimer(string.Empty, timerName);
        }

        /// <summary>
        /// Method to start a timer with a specified name and for a specific context
        /// </summary>
        /// <param name="contextName">Name of the context</param>
        /// <param name="timerName">Name of the timer</param>
        public void StartTimer(string contextName, string timerName)
        {
            if (this.openTimerList.ContainsKey(timerName))
            {
                throw new ArgumentException($"Timer already Started: {timerName}");
            }
            else
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Starting response timer: {timerName}");

                PerfTimer timer = new PerfTimer
                {
                    TimerName = timerName,
                    TimerContext = contextName,
                    StartTime = DateTime.UtcNow
                };

                this.openTimerList.Add(timerName, timer);
            }
        }

        /// <summary>
        /// Method to stop an existing timer with a specified name for a test
        /// </summary>
        /// <param name="timerName">Name of the timer</param>
        public void StopTimer(string timerName)
        {
            DateTime et = DateTime.UtcNow;
            if (!this.openTimerList.ContainsKey(timerName))
            {
                throw new ArgumentException("Response time test does not exist");
            }
            else
            {
                this.Log.LogMessage(MessageType.INFORMATION, $"Stopping response time test: {timerName}");
                this.openTimerList[timerName].EndTime = et;
                this.openTimerList[timerName].Duration = this.openTimerList[timerName].EndTime - this.openTimerList[timerName].StartTime;
                this.Timerlist.Add(this.openTimerList[timerName]);
                this.openTimerList.Remove(timerName);
            }
        }

        /// <summary>
        /// Method to Write the Performance Timer Collection to disk
        /// </summary>
        /// <param name="log">The current test Logger</param>
        public void Write(ILogger log)
        {
            // Only run if the response times is greater than 0
            if (this.Timerlist.Count > 0)
            {
                // Locks the writer if other tests are using it
                lock (writerLocker)
                {
                    try
                    {
                        // If filename doesn't exist, we haven't created the file yet
                        if (this.FileName == null)
                        {
                            this.FileName = $"PerformanceTimerResults-{this.TestName}-{DateTime.UtcNow.ToString("O").Replace(':', '-')}.xml";
                        }

                        log.LogMessage(MessageType.INFORMATION, $"filename: {LoggingConfig.GetLogDirectory()}{Path.DirectorySeparatorChar}{this.FileName}");

                        XmlWriterSettings settings = new XmlWriterSettings
                        {
                            WriteEndDocumentOnClose = true,
                            Indent = true
                        };

                        XmlWriter writer = XmlWriter.Create($"{LoggingConfig.GetLogDirectory()}{Path.DirectorySeparatorChar}{this.FileName}", settings);

                        XmlSerializer x = new XmlSerializer(this.GetType());
                        x.Serialize(writer, this);

                        writer.Flush();
                        writer.Close();
                    }
                    catch (Exception e)
                    {
                        log.LogMessage(MessageType.ERROR, $"Could not save response time file.  Error was: {e.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Method to Read in the Performance Timer Collection from disk
        /// </summary>
        /// <param name="filepath">The file from which to initialize</param>
        /// <returns> <see cref="PerfTimerCollection"/> initialized from file path</returns>
        public IPerfTimerCollection LoadPerfTimerCollection(string filepath)
        {
            PerfTimerCollection perfTimerCollection;
            XmlSerializer serializer = new XmlSerializer(typeof(PerfTimerCollection));

            StreamReader reader = new StreamReader(filepath);
            perfTimerCollection = (PerfTimerCollection)serializer.Deserialize(reader);
            reader.Close();
            return perfTimerCollection;
        }
    }
}
