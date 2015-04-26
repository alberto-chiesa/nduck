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
        static Logger()
        {
            Verbosity = OutputLevel.Warning;
        }

        /// <summary>
        /// OutputLevel defines the possible verbosity
        /// levels associated to the logger.
        /// </summary>
        /// <seealso cref="NDuck.Logger"/>
        public enum OutputLevel
        {
            /// <summary>
            /// Output level for debug messages.
            /// If the level is set to debug, every
            /// message type will be written.
            /// </summary>
            Debug = 0,
            /// <summary>
            /// Output Level for Information messages.
            /// It will exclude Debug messages from the output.
            /// </summary>
            Info = 1,
            /// <summary>
            /// Prints onl Warning and Error messages.
            /// </summary>
            Warning = 2,
            /// <summary>
            /// Prints only error messages.
            /// </summary>
            Error = 3,
            /// <summary>
            /// Silent mode.
            /// </summary>
            None = 4
        }

        /// <summary>
        /// If true, Debug messages will be enabled.
        /// </summary>
        public static OutputLevel Verbosity { get; set; }

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
            Log(OutputLevel.Debug, format, values);
        }

        /// <summary>
        /// Writes a debug message to the Console.
        /// </summary>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Debug(ConsoleColor color, String format, params Object[] values)
        {
            Log(OutputLevel.Debug, color, format, values);
        }

        /// <summary>
        /// Writes an information message to the Console.
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Info(String format, params Object[] values)
        {
            Log(OutputLevel.Info, format, values);
        }

        /// <summary>
        /// Writes an information message to the Console.
        /// </summary>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Info(ConsoleColor color, String format, params Object[] values)
        {
            Log(OutputLevel.Info, color, format, values);
        }

        /// <summary>
        /// Writes to the console (a yellow warning).
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Warn(String format, params Object[] values)
        {
            Log(OutputLevel.Warning, ConsoleColor.Yellow, format, values);
        }

        /// <summary>
        /// Writes to the console (in red!).
        /// </summary>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        public static void Error(String format, params Object[] values)
        {
            Log(OutputLevel.Error, ConsoleColor.Red, format, values);
        }

        /// <summary>
        /// Writes a debug message to the Console.
        /// </summary>
        /// <param name="level">The output level associated with the message.</param>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        private static void Log(OutputLevel level, String format, params Object[] values)
        {
            if (level < Verbosity) return;
            
            if (values == null || values.Length == 0)
                WriteLine(format);
            else
                WriteLine(format, values);
        }

        /// <summary>
        /// Writes a debug message to the Console.
        /// </summary>
        /// <param name="level">The output level associated with the message.</param>
        /// <param name="color">The color to apply to the message.</param>
        /// <param name="format">The format string to be used.</param>
        /// <param name="values">A params array of values to be formatted.</param>
        private static void Log(OutputLevel level, ConsoleColor color, String format, params Object[] values)
        {
            if (level < Verbosity) return;
            
            if (values == null || values.Length == 0)
                WriteLine(color, format);
            else
                WriteLine(color, format, values);
        }
    }
}
