using System;
using System.Collections.Generic;
using System.Linq;
using NDuck.Config;

namespace NDuck
{
    class Program
    {
        static void Main(string[] args)
        {
            _Main(args);

            StopWhenDebugging();
        }

        private static void _Main(string[] args)
        {
            var beginTime = DateTime.Now;

            try
            {
                var options = ReadOptionsFromCommandLine(args);
                if (options == null) return;

                var processor = new TypeRepository(options);

                var endTime = DateTime.Now;

                Logger.Success("Processing completed succesfully in {0:hh\\:mm\\:ss\\:fff}.", endTime - beginTime);
                Environment.ExitCode = 0;
            }
            catch (Exception e)
            {
                Logger.Error("There was an error: {0}", e.Message);
                Logger.Debug("At: {0}", e.StackTrace);

                Logger.Error("Execution aborted due to errors.");
                Environment.ExitCode = 1;
            }
        }

        private static void StopWhenDebugging()
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write(@"Press return to close.");
                Console.ReadLine();
            }
#endif
        }

        /// <summary>
        /// Reads the execution options from the
        /// command line arguments.
        /// </summary>
        /// <param name="args">The args array.</param>
        /// <returns>
        /// A ready to use <see cref="NDuck.Config.ExecutionOptions"/> instance,
        /// or null if there was an error or the execution required only
        /// the help message to be printed.
        /// </returns>
        private static ExecutionOptions ReadOptionsFromCommandLine(string[] args)
        {
            var options = new ExecutionOptions(args);

            if (options.HasErrors || options.Command == ExecutionOptions.ExecutionCommand.Help)
            {
                options.WriteHelpMessage();
                if (options.HasErrors) Environment.ExitCode = 1;
                return null;
            }

            return options;
        }

    }
}
