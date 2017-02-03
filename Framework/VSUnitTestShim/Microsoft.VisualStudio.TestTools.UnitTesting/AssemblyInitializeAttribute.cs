//--------------------------------------------------
// <copyright file="AssemblyInitializeAttribute.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for AssemblyInitializeAttribute</summary>
//--------------------------------------------------
using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// Holder for usage attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class AssemblyInitializeAttribute : Attribute
    {
    }
}
