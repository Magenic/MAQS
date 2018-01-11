//--------------------------------------------------
// <copyright file="UnitTestOutcome.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for UnitTestOutcome</summary>
//--------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// Outcome enumeration holder
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1602:EnumerationItemsMustBeDocumented", Justification = "Reviewed.")]
    public enum UnitTestOutcome
    {
        Failed,
        Inconclusive,
        Passed,
        InProgress,
        Error,
        Timeout,
        Aborted,
        Unknown
    }
}
