//--------------------------------------------------
// <copyright file="Calculator.cs" company="Magenic">
//  Copyright 2018 Magenic, All rights Reserved
// </copyright>
// <summary>Generic class for testing the extension</summary>
//--------------------------------------------------

namespace SpecFlowExtensionNUnitTests.Models
{
    /// <summary>
    /// Helper calculator class
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// Gets or sets the first number
        /// </summary>
        public int FirstNumber { get; set; }

        /// <summary>
        /// Gets or sets the second number
        /// </summary>
        public int SecondNumber { get; set; }

        /// <summary>
        /// Adds the numbers
        /// </summary>
        /// <returns>The result</returns>
        public int Add()
        {
            return this.FirstNumber + this.SecondNumber;
        }

        /// <summary>
        /// Subtracts the number
        /// </summary>
        /// <returns>The result</returns>
        public int Subtract()
        {
            return this.FirstNumber - this.SecondNumber;
        }

        /// <summary>
        /// Multiplies the numbers
        /// </summary>
        /// <returns>The result</returns>
        public int Multiply()
        {
            return this.FirstNumber * this.SecondNumber;
        }

        /// <summary>
        /// Divides the numbers
        /// </summary>
        /// <returns>The result</returns>
        public int Divide()
        {
            return this.FirstNumber / this.SecondNumber;
        }
    }
}
