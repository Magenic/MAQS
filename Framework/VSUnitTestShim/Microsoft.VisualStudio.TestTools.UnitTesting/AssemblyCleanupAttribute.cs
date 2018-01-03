//--------------------------------------------------
// <copyright file="AssemblyCleanupAttribute.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for AssemblyCleanupAttribute</summary>
//--------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// Holder for usage attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    [ExcludeFromCodeCoverage]
    public sealed class AssemblyCleanupAttribute : Attribute
    {
    }
}
