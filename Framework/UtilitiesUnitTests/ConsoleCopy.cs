//--------------------------------------------------
// <copyright file="ConsoleCopy.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Copy console output to a file for testing. Copied off the internet.</summary>
//--------------------------------------------------
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace UtilitiesUnitTesting
{
    /// <summary>
    /// Class to copy console output to a file
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConsoleCopy : IDisposable
    {
        /// <summary>
        /// used to write to both the console and the log
        /// </summary>
        private readonly TextWriter doubleWriter;
        
        /// <summary>
        /// stores the original Console output
        /// </summary>
        private readonly TextWriter oldOut;

        /// <summary>
        /// used to write to the log file
        /// </summary>
        private StreamWriter fileWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCopy"/> class 
        /// </summary>
        /// <param name="path">path of the log file</param>
        public ConsoleCopy(string path)
        {
            this.oldOut = Console.Out;

            try
            {
                this.fileWriter = new StreamWriter(File.Open(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                {
                    AutoFlush = true
                };

                this.doubleWriter = new DoubleWriter(this.fileWriter, this.oldOut);
            }
            catch (Exception e)
            {
                Console.WriteLine("Cannot open file for writing");
                Console.WriteLine(e.Message);
                return;
            }

            Console.SetOut(this.doubleWriter);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ConsoleCopy" /> class
        /// </summary>
        ~ConsoleCopy()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Cleans up the writers and reverts the console
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up the writers and reverts the console
        /// </summary>
        /// <param name="disposing">True if you want to release managed resources</param>
        [SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "doubleWriter")]
        protected virtual void Dispose(bool disposing)
        {
            Console.SetOut(this.oldOut);
            if (this.fileWriter != null)
            {
                this.fileWriter.Flush();
                this.fileWriter.Close();
                this.fileWriter = null;
            }
        }

        /// <summary>
        /// Custom TextWriter that writes to both a log file and the console
        /// </summary>
        private class DoubleWriter : TextWriter
        {
            /// <summary>
            /// used to write to the log file
            /// </summary>
            private readonly TextWriter fileOutput;

            /// <summary>
            /// used to write to the console
            /// </summary>
            private readonly TextWriter consoleOutput;

            /// <summary>
            /// Initializes a new instance of the <see cref="DoubleWriter"/> class
            /// </summary>
            /// <param name="fileOutput">used to write to the log file</param>
            /// <param name="consoleOutput">used to write to the console</param>
            public DoubleWriter(TextWriter fileOutput, TextWriter consoleOutput)
            {
                this.fileOutput = fileOutput;
                this.consoleOutput = consoleOutput;
            }

            /// <summary>
            /// Set the encoding field
            /// </summary>
            public override Encoding Encoding
            {
                get
                {
                    return this.fileOutput.Encoding;
                }
            }

            /// <summary>
            /// Flush both outputs
            /// </summary>
            public override void Flush()
            {
                this.fileOutput.Flush();
                this.consoleOutput.Flush();
            }

            /// <summary>
            /// Write to both outputs
            /// </summary>
            /// <param name="value">Value to write to the outputs</param>
            public override void Write(char value)
            {
                this.fileOutput.Write(value);
                this.consoleOutput.Write(value);
            }
        }
    }
}
