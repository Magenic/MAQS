//--------------------------------------------------
// <copyright file="TestContext.cs" company="Magenic">
//  Copyright 2017 Magenic, All rights Reserved
// </copyright>
// <summary>Shim for TestContext</summary>
//--------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// TestContext shim
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class TestContext
    {
        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestRunDirectory
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string DeploymentDirectory
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string ResultsDirectory
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestRunResultsDirectory
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestResultsDirectory
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestDir
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestDeploymentDir
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestLogsDir
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string FullyQualifiedTestClassName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual string TestName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual UnitTestOutcome CurrentTestOutcome
        {
            get
            {
                return UnitTestOutcome.Unknown;
            }
        }

        /// <summary>
        /// Gets shim value
        /// </summary>
        public virtual IDictionary Properties
        {
            get
            {
                return new Dictionary<string, string>();
            }
        }
    }
}
