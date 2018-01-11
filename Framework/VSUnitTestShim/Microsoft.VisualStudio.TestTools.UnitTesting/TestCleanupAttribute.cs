//--------------------------------------------------
// <copyright file="TestCleanupAttribute.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for TestCleanupAttribute</summary>
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
    public sealed class TestCleanupAttribute : Attribute
    {
    }
}
