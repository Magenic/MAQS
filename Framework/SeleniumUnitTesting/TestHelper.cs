//--------------------------------------------------
// <copyright file="TestHelper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class to deal with cleanup</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Helper class for tests
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        /// <summary>
        /// Checks to see if the folder exists
        /// </summary>
        /// <returns>Boolean if folder exists</returns>
        public static bool DoesFolderExist()
        {
            return Directory.Exists(Path.GetDirectoryName(LoggingConfig.GetLogDirectory()));
        }

        /// <summary>
        /// Cleanup the files generated
        /// </summary>
        /// <param name="loggingFolderExistsBeforeRun">If the folder existed before the test, we keep it</param>
        public static void Cleanup(bool loggingFolderExistsBeforeRun)
        {
            string directory = Path.GetDirectoryName(LoggingConfig.GetLogDirectory());

            try
            {
                if (!loggingFolderExistsBeforeRun)
                {
                    Directory.Delete(directory, true);
                }
                else
                {
                    if (Directory.Exists(directory))
                    {
                        string unitTestBaseName = "SeleniumUnitTests.*";
                        foreach (FileInfo f in new DirectoryInfo(directory).GetFiles(unitTestBaseName + ".txt"))
                        {
                            f.Delete();
                        }

                        foreach (FileInfo f in new DirectoryInfo(directory).GetFiles(unitTestBaseName + ".rtf"))
                        {
                            f.Delete();
                        }

                        foreach (FileInfo f in new DirectoryInfo(directory).GetFiles(unitTestBaseName + ".png"))
                        {
                            f.Delete();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(StringProcessor.SafeFormatter("Problems occured while cleaning up after tests:\r\n{0}\r\n\r\n{1}", e.Message, e.StackTrace));
            }
        }
    }
}
