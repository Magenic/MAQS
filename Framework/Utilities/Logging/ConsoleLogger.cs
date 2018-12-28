//--------------------------------------------------
// <copyright file="ConsoleLogger.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>Helper class for console logging</summary>
//--------------------------------------------------
using Magenic.Maqs.Utilities.Data;
using System;

namespace Magenic.Maqs.Utilities.Logging
{
    /// <summary>
    /// Helper class for logging to the console
    /// </summary>
    public class ConsoleLogger : Logger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleLogger" /> class.
        /// </summary>
        /// <param name="level">The logging level</param>
        public ConsoleLogger(MessageType level = MessageType.INFORMATION)
            : base(level)
        {
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as a generic message
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public override void LogMessage(string message, params object[] args)
        {
            this.WriteLine(message, args);
        }

        /// <summary>
        /// Write the formatted message (one line) to the console as the specified type
        /// </summary>
        /// <param name="messageType">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public override void LogMessage(MessageType messageType, string message, params object[] args)
        {
            this.WriteLine(messageType, message, args);
        }

        /// <summary>
        /// Write the formatted message to the console as a generic message
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public void Write(string message, params object[] args)
        {
            this.SetColorWriteAndRestore(MessageType.INFORMATION, false, message, args);
        }

        /// <summary>
        /// Write the formatted message followed by a line break to the console as a generic message
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="args">String format arguments</param>
        public void WriteLine(string message, params object[] args)
        {
            this.SetColorWriteAndRestore(MessageType.INFORMATION, true, message, args);
        }

        /// <summary>
        /// Write the formatted message to the console as the given message type
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">Message string format arguments</param>
        public void Write(MessageType type, string message, params object[] args)
        {
            this.SetColorWriteAndRestore(type, false, message, args);
        }

        /// <summary>
        /// Write the formatted message followed by a line break to the console as the given message type
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="message">The message text</param>
        /// <param name="args">Message string format arguments</param>
        public void WriteLine(MessageType type, string message, params object[] args)
        {
            this.SetColorWriteAndRestore(type, true, message, args);
        }

        /// <summary>
        /// Set the console colors
        /// </summary>
        /// <param name="fore">The foreground color</param>
        /// <param name="back">The background color</param>
        private static void SetConsoleColor(ConsoleColor fore, ConsoleColor back = ConsoleColor.Black)
        {
            Console.ForegroundColor = fore;
            Console.BackgroundColor = back;
        }

        /// <summary>
        /// Change the console color to match the message type, write the message and restore the previous console colors
        /// </summary>
        /// <param name="type">The type of message</param>
        /// <param name="line">Is this a write-line command, else it is just a write</param>
        /// <param name="message">The log message</param>
        /// <param name="args">Message string format arguments</param>
        private void SetColorWriteAndRestore(MessageType type, bool line, string message, params object[] args)
        {
            // Just return if there is no message or this type of message should not be logged
            if (string.IsNullOrEmpty(message) || !this.ShouldMessageBeLogged(type))
            {
                return;
            }

            // Save the original console colors
            ConsoleColor originalBack = Console.BackgroundColor;
            ConsoleColor originalFore = Console.ForegroundColor;

            // Update console colors
            SetConsoleColor(type);
            string result = StringProcessor.SafeFormatter(message, args);
            try
            {
                // If this a write-line command
                if (line)
                {
                    Console.WriteLine(result);
                }
                else
                {
                    Console.Write(result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(StringProcessor.SafeFormatter("Failed to write to the console because: {0}", e.Message));
            }

            // Cleanup after yourself
            SetConsoleColor(originalFore, originalBack);
        }

        /// <summary>
        /// Set the console color based on the message type
        /// </summary>
        /// <param name="type">The type of message that will be written</param>
        private void SetConsoleColor(MessageType type)
        {
            switch (type)
            {
                case MessageType.SUSPENDED:
                    // Suspended so we do nothing
                    break;
                case MessageType.VERBOSE:
                    SetConsoleColor(ConsoleColor.Black, ConsoleColor.White);
                    break;
                case MessageType.INFORMATION:
                    SetConsoleColor(ConsoleColor.Blue, ConsoleColor.White);
                    break;
                case MessageType.GENERIC:
                    SetConsoleColor(ConsoleColor.White);
                    break;
                case MessageType.SUCCESS:
                    SetConsoleColor(ConsoleColor.Green);
                    break;
                case MessageType.WARNING:
                    SetConsoleColor(ConsoleColor.Yellow);
                    break;
                case MessageType.ERROR:
                    SetConsoleColor(ConsoleColor.Red);
                    break;
                default:
                    SetConsoleColor(ConsoleColor.Yellow);
                    Console.WriteLine(this.UnknownMessageTypeMessage(type));
                    SetConsoleColor(ConsoleColor.White);
                    break;
            }
        }
    }
}
