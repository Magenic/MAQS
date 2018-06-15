//--------------------------------------------------
// <copyright file="DriverStore.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Base driver storage</summary>
//--------------------------------------------------
using Magenic.MaqsFramework.Utilities.Logging;
using System;

namespace Magenic.MaqsFramework.BaseTest
{
    /// <summary>
    /// Base driver storage object
    /// </summary>
    public abstract class DriverStore : IDisposable
    {
        /// <summary>
        /// The test object associated with the driver
        /// </summary>
        private readonly BaseTestObject testObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriverStore"/> class
        /// </summary>
        /// <param name="funcToRun">How to get the underlying driver</param>
        /// <param name="testObject">The associate test object</param>
        public DriverStore(Func<object> funcToRun, BaseTestObject testObject)
        {
            this.GetDriver = funcToRun;
            this.testObject = testObject;
        }

        /// <summary>
        /// Gets the testing object
        /// </summary>
        public Logger Log
        {
            get
            {
                return this.testObject.Log;
            }
        }

        /// <summary>
        /// Gets or sets the underlying driver; like the web driver or database connection wrapper
        /// </summary>
        protected object BaseDriver { get; set; }

        /// <summary>
        /// Gets or sets the function for getting the underlying driver
        /// </summary>
        protected Func<object> GetDriver { get; set; }

        /// <summary>
        /// Check if the underlying driver has been initialized
        /// </summary>
        /// <returns>True if the underlying driver has already been initialized</returns>
        public bool IsDriverIntialized() => this.BaseDriver != null;

        /// <summary>
        /// Get the underlying driver
        /// </summary>
        /// <returns>The underlying driver</returns>
        public object Get()
        {
            // Initialize the driver if we haven't already
            if (this.BaseDriver == null)
            {
                this.BaseDriver = this.GetDriver();
            }

            return this.BaseDriver;
        }

        /// <summary>
        /// Cleanup the underlying driver
        /// </summary>
        public abstract void Dispose();
    }
}
