using Magenic.Maqs.Utilities.Helper;
using Magenic.Maqs.Utilities.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UtilityLogger = Magenic.Maqs.Utilities.Logging.Logger;

namespace UtilitiesUnitTests
{

    [TestClass]
    public class FileLoggerThreadSafetyTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void FileLoggerMultipleTaskTest()
        {
            var fileLogger = new FileLogger(logFolder: "Log", name: $"{this.TestContext.TestName}.txt");

            this.ExecuteLoggingTasks(fileLogger);
        }

        [TestMethod]
        [TestCategory(TestCategories.Utilities)]
        public void HtmlFileLoggerMultipleTaskTest()
        {
            var fileLogger = new HtmlFileLogger(logFolder: "Log", name: $"{this.TestContext.TestName}.html");

            this.ExecuteLoggingTasks(fileLogger);
        }

        /// <summary>
        /// logs a message using 5 tasks to verify the log file is locked when 
        /// being written to
        /// </summary>
        /// <param name="logger"> the logger that is writing to the file</param>
        private void ExecuteLoggingTasks(UtilityLogger logger)
        {
            var loggingTasks = new List<Task>();

            for (int i = 0; i < 4; i++)
            { 
                loggingTasks.Add(Task.Run(() => logger.LogMessage($"parallel task running..")));
            }
            
            Task.WaitAll(loggingTasks.ToArray());
        }
    }
}
