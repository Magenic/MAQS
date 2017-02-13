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

namespace UtilitiesUnitTesting
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
        /// <param name="createIfNotExist">Should we create the folder if it does not exist</param>
        /// <returns>Boolean if folder exists</returns>
        public static bool DoesFolderExist(bool createIfNotExist = true)
        {
            bool exists = Directory.Exists(Path.GetDirectoryName(LoggingConfig.GetLogDirectory()));

            if (!exists && createIfNotExist)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LoggingConfig.GetLogDirectory()));
            }

            return exists;
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
                        string unitTestBaseName = "*UtilitiesUnitTesting.*";
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

                        foreach (FileInfo f in new DirectoryInfo(directory).GetFiles(unitTestBaseName + ".xml"))
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
