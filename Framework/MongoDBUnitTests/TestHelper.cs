//--------------------------------------------------
// <copyright file="TestHelper.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class to deal with cleanup</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Data;
using Magenic.MaqsFramework.Utilities.Logging;
using System;
using System.IO;
using System.Linq;

namespace MongoDBUnitTests
{
    /// <summary>
    /// Helper class for tests
    /// </summary>
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
            try
            {
                string directory = Path.GetDirectoryName(LoggingConfig.GetLogDirectory());

                // Exit if the directory doesn't exist
                if (!Directory.Exists(directory))
                {
                    return;
                }

                if (!loggingFolderExistsBeforeRun)
                {
                    Directory.Delete(directory, true);
                }
                else
                {
                    if (Directory.Exists(directory))
                    {
                        string unitTestBaseName = "DatabaseUnitTests.*";
                        string[] extensions = new[] { ".txt", ".rtf", ".png" };
                        FileInfo[] files = new DirectoryInfo(directory)
                            .GetFiles(unitTestBaseName)
                            .Where(f => extensions.Contains(f.Extension.ToLower()))
                            .ToArray();
                        foreach (FileInfo f in files)
                        {
                            f.Delete();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(StringProcessor.SafeFormatter("Problems occured while cleaning up after tests:\r\n{0}{1}{2}", e.Message, "\r\n\r\n", e.StackTrace));
            }
        }
    }
}
