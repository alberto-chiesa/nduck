using System;
using System.Linq;

namespace NDuck
{
    /// <summary>
    /// Static helper class used to log messages to a common output
    /// (currently the console).
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// If true, Debugg messages will be enabled.
        /// </summary>
        public static Boolean Verbose { get; set; }

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="value">The value to write</param>
        public static void Write(String value)
        {
            Console.Write(value);
        }

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="value">The value to write.</param>
        public static void Write(ConsoleColor color, String value)
        {
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(value);
            Console.ForegroundColor = colorBackup;
        }

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="value">The value to write</param>
        public static void WriteLine(String value)
        {
            Console.WriteLine(value);
        }

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void WriteLine(String format, params Object[] values)
        {
            Console.WriteLine(format, values);
        }

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="value">The value to write.</param>
        public static void WriteLine(ConsoleColor color, String value)
        {
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = colorBackup;
        }

        /// <summary>
        /// Writes to the console.
        /// </summary>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void WriteLine(ConsoleColor color, String format, params Object[] values)
        {
            var colorBackup = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(format, values);
            Console.ForegroundColor = colorBackup;
        }

        /// <summary>
        /// Writes to the console (in red!).
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Error(String format, params Object[] values)
        {
            WriteLine(ConsoleColor.Red, format, values);
        }

        /// <summary>
        /// Writes to the console (in green!).
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Success(String format, params Object[] values)
        {
            WriteLine(ConsoleColor.DarkGreen, format, values);
        }

        /// <summary>
        /// Writes a debug message to the Console.
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Debug(String format, params Object[] values)
        {
            if (Verbose) WriteLine(format, values);
        }

        /// <summary>
        /// Writes a debug message to the Console.
        /// </summary>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Debug(ConsoleColor color, String format, params Object[] values)
        {
            if (Verbose) WriteLine(color, format, values);
        }
    }
}
