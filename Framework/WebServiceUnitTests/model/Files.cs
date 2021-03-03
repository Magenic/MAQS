﻿//--------------------------------------------------
// <copyright file="Files.cs" company="Magenic">
//  Copyright 2021 Magenic, All rights Reserved
// </copyright>
// <summary>Employee model</summary>
//--------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebServiceTesterUnitTesting.Model
{
    /// <summary>
    /// Employee Model
    /// </summary>
    [ExcludeFromCodeCoverage]
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