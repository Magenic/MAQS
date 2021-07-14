using Magenic.Maqs.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magenic.Maqs.Utilities.Logging
{
    public static class LoggerFactory
    {
        public static ILogger GetConsoleLogger()
        {
            return new ConsoleLogger(LoggingConfig.GetLoggingLevelSetting());
        }

        public static ILogger GetLogger(string logName)
        {
            if (LoggingConfig.GetLoggingEnabledSetting() == LoggingEnabled.NO)
            {
                return GetLogger(logName, "CONSOLE", MessageType.SUSPENDED);
            }

            return GetLogger(logName, Config.GetGeneralValue("LogType", "CONSOLE"), LoggingConfig.GetLoggingLevelSetting());
        }

        public static ILogger GetLogger(string logName, string logType, MessageType loggingLevel)
        {
            string logDirectory = LoggingConfig.GetLogDirectory();

            switch (logType.ToUpper())
            {
                case "CONSOLE":
                    return new ConsoleLogger(loggingLevel);
                case "TXT":
                    return new FileLogger(logDirectory, logName, loggingLevel);
                case "HTML":
                case "HTM":
                    return new HtmlFileLogger(logDirectory, logName, loggingLevel);
                default:
                    throw new MaqsLoggingConfigException($"Log type '{logType}' is not a valid option");
            }
        }
    }
}
