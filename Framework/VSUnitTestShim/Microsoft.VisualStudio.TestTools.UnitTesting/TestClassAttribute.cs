//--------------------------------------------------
// <copyright file="TestClassAttribute.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for TestClassAttribute</summary>
//--------------------------------------------------
using System;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// Holder for usage attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TestClassAttribute : Attribute
    {
    }
}
