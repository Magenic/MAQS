//--------------------------------------------------
// <copyright file="FileUpload.cs" company="Magenic">
//  Copyright 2015 Magenic, All rights Reserved
// </copyright>
// <summary>Employee model</summary>
//--------------------------------------------------

using System;
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

    /// <summary>
    /// Employee Model
    /// </summary>
    public class Files
    {
        /// <summary>
        /// Gets or sets the file uploaded name
        /// </summary>
        public virtual string ContentName { get; set; }

        /// <summary>
        /// Gets or sets the file uploaded name
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Gets or sets the uploaded date
        /// </summary>
        public virtual DateTime DateUploaded { get; set; }
    }
}