//--------------------------------------------------
// <copyright file="PerformanceUnitTests.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>PerformanceTests class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Response time test class
    /// </summary>
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PerformanceUnitTests : BaseTest
    {
        /// <summary>
        /// An example class to save in the payload
        /// </summary>
        private Tconfig tc;

        /// <summary>
        /// Test method to test Performance Timers
        /// </summary>
        #region PerfTimers
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PerfStartStop2Timers()
        {
            PerfTimerCollection p = this.PerfTimerCollection;

            // build an object to store in the payloadstring of the PerfTimerCollection
            this.tc = new Tconfig
            {
                LogPath = Config.GetGeneralValue("FileLoggerPath"),
                Logtype = Config.GetGeneralValue("LogType"),
                WebURI = Config.GetGeneralValue("WebServiceUri")
            };

            // store it (as a JSON string)
            p.PerfPayloadString = JsonConvert.SerializeObject(this.tc);
            string json_string = p.PerfPayloadString;

            p.StartTimer("Outer", "test1");
            System.Threading.Thread.Sleep(1000);
            p.StartTimer("Inner", "test2");
            System.Threading.Thread.Sleep(1000);
            p.EndTimer("test1");
            p.EndTimer("test2");

            // Write the log and validate the resulting file contents
            p.Write(this.Log);
            string filepath = LoggingConfig.GetLogDirectory() + p.FileName;

            // If the file doesnt exist, just bail
            Assert.IsTrue(File.Exists(filepath), "File Check : Expected File does not exist:" + filepath);

            // Otherwise record the assertion as true and continue...
            SoftAssert.IsTrue(true, "File Check : Expected File exists.");

            PerfTimerCollection r = PerfTimerCollection.LoadPerfTimerCollection(filepath);

            // Payload check
            SoftAssert.AreEqual(json_string, r.PerfPayloadString, "Payload", "Validated Payload (json)");

            // There should be 2 timers
            SoftAssert.AreEqual(2.ToString(), r.Timerlist.Count.ToString(), "Expected number of timers");

            // Check the timers
            int badnamecount = 0;
            foreach (PerfTimer pt in r.Timerlist)
            {
                switch (pt.TimerName)
                {
                    // Timer = test1 should have a context of Outer
                    case "test1":
                        SoftAssert.AreEqual("Outer", pt.TimerContext, "test1", "Test1 Context");
                        break;

                    // Timer = test2 should have an empty contex
                    case "test2":
                        SoftAssert.AreEqual("Inner", pt.TimerContext, "test2", "Test2 Context");
                        break;

                    // Catch any extra timers
                    default:
                        badnamecount++;
                        SoftAssert.IsTrue(false, "ExtraTimer", "Extra timer present: " + pt.TimerName);
                        break;
                }
            }

            if (badnamecount != 0)
            {
                // We would have logged any extra timers, so pass the ExtraTimer assert
                SoftAssert.IsTrue(true, "ExtraTimer");
            }

            SoftAssert.FailTestIfAssertFailed();
        }
        #endregion

        /// <summary>
        /// Performance timer test to validate error
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PerfDontStopTimer()
        {
            PerfTimerCollection r;
            PerfTimerCollection p = this.PerfTimerCollection;
            string filepath;

            p.StartTimer("StoppedOuter", "test1");
            p.StartTimer("test2");
            System.Threading.Thread.Sleep(1000);
            p.EndTimer("test1");
            p.EndTimer("test2");
            p.StartTimer("NotStopped", "test3");

            // Write the log and validate the resulting file contents
            p.Write(this.Log);
            filepath = LoggingConfig.GetLogDirectory() + p.FileName;

            // If the file doesnt exist, just bail
            Assert.IsTrue(File.Exists(filepath), "File Check : Expected File does not exist:" + filepath);

            // Otherwise record the assertion as true and continue...
            SoftAssert.IsTrue(true, "File Check : Expected File exists.");

            r = PerfTimerCollection.LoadPerfTimerCollection(filepath);

            // Payload should be empty
            SoftAssert.IsTrue(string.IsNullOrEmpty(r.PerfPayloadString), "EmptyPayload", "Payload was not Empty! Contained: " + r.PerfPayloadString);

            // There should be 2 timers
            SoftAssert.AreEqual(2.ToString(), r.Timerlist.Count.ToString(), "Expected number of timers");

            // Check the timers
            int badnamecount = 0;
            foreach (PerfTimer pt in r.Timerlist)
            {
                switch (pt.TimerName)
                {
                    // Timer = test1 should have a context of StoppedOuter
                    case "test1":
                        SoftAssert.AreEqual("StoppedOuter", pt.TimerContext, "test1", "Test1 Context");
                        break;

                    // Timer = test2 should have an empty contex
                    case "test2":
                        SoftAssert.IsTrue(string.IsNullOrEmpty(pt.TimerContext), "Timer2Context", "Context for " + pt.TimerName + " was not Empty! Contained: " + pt.TimerContext);
                        break;

                    // Catch any extra timers
                    default:
                        badnamecount++;
                        SoftAssert.IsTrue(false, "ExtraTimer", "Extra timer present: " + pt.TimerName);
                        break;
                }
            }

            if (badnamecount != 0)
            {
                // We would have logged any extra timers, so pass the ExtraTimer assert
                SoftAssert.IsTrue(true, "ExtraTimer");
            }

            SoftAssert.FailTestIfAssertFailed();
        }

        /// <summary>
        /// Verify that the performance timer throws expected error when attempting to start an already started timer
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(ArgumentException))]
        public void PerfStartTimerThrowException()
        {
            PerfTimerCollection p = this.PerfTimerCollection;
            p.StartTimer("alreadyStarted");
            p.StartTimer("alreadyStarted");
        }

        /// <summary>
        /// Verify that the performance timer throws expected error when attempting to end a timer a stopped timer
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        [ExpectedException(typeof(ArgumentException))]
        public void PerfEndTimerThrowException()
        {
            PerfTimerCollection p = this.PerfTimerCollection;
            p.EndTimer("notStarted");
        }

        /// <summary>
        /// Verify that the performance timer constructor creates the expected timer
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PerfTimerConstructorTest()
        {
            PerfTimerCollection p = new PerfTimerCollection("testTimer");
            Assert.AreEqual(p.TestName, "testTimer");
        }

        /// <summary>
        /// Verify that the performance timer handles invalid testNames properly
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void PerfTimerWriteException()
        {
            // Invalid testName
            PerfTimerCollection p = new PerfTimerCollection("<>");
            p.StartTimer("testTimer");
            p.EndTimer("testTimer");
            FileLogger log = new FileLogger("PerfTimerWriteException", "PerfTimerWriteException", MessageType.GENERIC, true);
            p.Write(log);
            Assert.IsTrue(File.ReadAllText(log.FilePath).Contains("Could not save response time file.  Error was:"));
        }

        /// <summary>
        /// Example class to save in the payload element of the performance timer collection
        /// </summary>
        private class Tconfig
        {
            /// <summary>
            /// Gets or sets the Log Path
            /// </summary>
            public string LogPath { get; set; }

            /// <summary>
            /// Gets or sets the Log type
            /// </summary>
            public string Logtype { get; set; }

            /// <summary>
            /// Gets or sets the Web URI
            /// </summary>
            public string WebURI { get; set; }
        }
    }
}
