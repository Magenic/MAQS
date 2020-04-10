﻿//--------------------------------------------------
// <copyright file="Base.cs" company="Magenic">
//  Copyright 2020 Magenic, All rights Reserved
// </copyright>
// <summary>Core Base unit tests</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FrameworkUnitTests
{
    /// <summary>
    /// Simple base test
    /// </summary>
    [TestClass]
    [TestCategory(TestCategories.Framework)]
    [ExcludeFromCodeCoverage]
    public class Base : BaseTest
    {
        /// <summary>
        /// Can a basic test run
        /// </summary>
        [TestMethod]
        public void CanRunTest()
        {
            Assert.IsNotNull(this.TestObject);
        }

        /// <summary>
        /// Can we add to a specific section
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void TestRunSettingAddition()
        {
            Assert.AreEqual("SAMPLE", Config.GetValueForSection("SeleniumMaqs", "Adding"));
        }

        /// <summary>
        /// Can we add to the general config
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void TestRunSettingOverrideGeneral()
        {
            Assert.AreEqual("YetAnother", Config.GetGeneralValue("Grog"));
        }

        /// <summary>
        /// Can we override JSON values
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategories.UtilitiesCore)]
        public void TestRunSettingOverrideSection()
        {
            Assert.AreEqual("ANOTHERSAMPLE", Config.GetValueForSection("Magenicmaqs", "SectionOverrideCore"));
        }

        /// <summary>
        /// Test that paths that don't have underlying files don't get written to the log
        /// </summary>
        [TestMethod]
        public void TeardownIgnoresNonExistantFiles()
        {
            // get log path
            string logFilePath = ((FileLogger)this.Log).FilePath;

            // add non existent associated files
            this.TestObject.AddAssociatedFile(@"TeardownTest/FakeFileToAttach1.txt");
            this.TestObject.AddAssociatedFile(@"TeardownTest/FakeFileToAttach2.txt");

            // call the teardown to add associated files
            this.Teardown();

            // test that no associated files are written to the log
            using (StreamReader sr = File.OpenText(logFilePath))
            {
                string[] lines = File.ReadAllLines(logFilePath);
                for (int x = 0; x < lines.Length - 1; x++)
                {
                    if (lines[x] == "GENERIC:	List of Associated Files: ")
                    {
                        Assert.Fail("Associated files logged despite the files not existing");
                    }
                }
            }
        }

        /// <summary>
        /// Test that performance counters files get added to associated files in teardown
        /// </summary>
        [TestMethod]
        public void PerfTimerCollectionFilesAreAddedToAssociatedFiles()
        {
            // get log path
            string logFilePath = ((FileLogger)this.Log).FilePath;

            this.PerfTimerCollection.StartTimer("testTimer");
            this.PerfTimerCollection.EndTimer("testTimer");
            this.PerfTimerCollection.Write(this.Log);

            string perfTimerLogPath = LoggingConfig.GetLogDirectory() + "\\" + this.PerfTimerCollection.FileName;

            this.Teardown();

            // test that performance timer file path is written to the log
            using (StreamReader sr = File.OpenText(logFilePath))
            {
                string[] lines = File.ReadAllLines(logFilePath);
                for (int x = 0; x < lines.Length - 1; x++)
                {
                    if (lines[x] == "GENERIC:	List of Associated Files: ")
                    {
                        Assert.AreEqual(perfTimerLogPath, lines[x + 1]);
                    }
                }
            }
        }
    }
}
