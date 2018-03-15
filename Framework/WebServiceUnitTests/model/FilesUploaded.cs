//--------------------------------------------------
// <copyright file="FilesUploaded.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>FilesUploaded model</summary>
//--------------------------------------------------
using System.Collections.Generic;

namespace WebServiceTesterUnitTesting.Model
{
    /// <summary>
    /// Employee Model
    /// </summary>
    public class FilesUploaded
    {
        /// <summary>
        /// Gets or sets the list of files uploaded
        /// </summary>
        public virtual List<Files> Files { get; set; }
    }
}