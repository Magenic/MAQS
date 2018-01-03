//--------------------------------------------------
// <copyright file="TestMethodAttribute.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for TestMethodAttribute</summary>
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
    public sealed class TestMethodAttribute : Attribute
    {
    }
}
