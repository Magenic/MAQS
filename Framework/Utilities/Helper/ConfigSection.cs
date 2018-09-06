//--------------------------------------------------
// <copyright file="ConfigSection.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Configuration sections</summary>
//--------------------------------------------------

namespace Magenic.Maqs.Utilities.Helper
{
    /// <summary>
    /// The configuration sections
    /// </summary>
    public enum ConfigSection
    {
        /// <summary>
        /// General MAQS section
        /// </summary>
        MagenicMaqs,

        /// <summary>
        /// Appium section
        /// </summary>
        AppiumMaqs,

        /// <summary>
        /// Appium capabilities section
        /// </summary>
        AppiumCapsMaqs,

        /// <summary>
        /// Database section
        /// </summary>
        DatabaseMaqs,

        /// <summary>
        /// Email section
        /// </summary>
        EmailMaqs,

        /// <summary>
        /// Selenium section
        /// </summary>
        SeleniumMaqs,

        /// <summary>
        /// Selenium remote capabilities section
        /// </summary>
        RemoteSeleniumCapsMaqs,

        /// <summary>
        /// Web service section
        /// </summary>
        WebServiceMaqs
    }
}