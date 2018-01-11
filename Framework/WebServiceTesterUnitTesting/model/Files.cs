//--------------------------------------------------
// <copyright file="Files.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Employee model</summary>
//--------------------------------------------------
using System;

namespace WebServiceTesterUnitTesting.Model
{
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