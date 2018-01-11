//--------------------------------------------------
// <copyright file="DatabaseConfig.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for getting database specific configuration values</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Helper;

namespace Magenic.MaqsFramework.BaseDatabaseTest
{
    /// <summary>
    /// Config class
    /// </summary>
    public static class DatabaseConfig
    {
        /// <summary>
        /// Get the database connection string
        /// </summary>
        /// <returns>The browser type</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseConfigUnitTests.cs" region="GetConnection" lang="C#" />
        /// </example>
        public static string GetConnectionString()
        {
            return Config.GetValue("DataBaseConnectionString");
        }

        /// <summary>
        /// Get the database timeout in seconds
        /// </summary>
        /// <returns>The timeout in seconds from the config file or default of 30 seconds when no app.config key is found</returns>
        /// <example>
        /// <code source="../DatabaseUnitTests/DatabaseConfigUnitTests.cs" region="GetQueryTimeout" lang="C#" />
        /// </example>
        public static int GetQueryTimeout()
        {
            return int.Parse(Config.GetValue("DatabaseTimeout", "30"));
        }
    }
}
