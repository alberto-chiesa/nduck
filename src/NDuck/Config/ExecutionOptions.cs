using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Options;
using Newtonsoft.Json;

namespace NDuck.Config
{
    /// <summary>
    /// Contains the complete set
    /// of options used during processing.
    /// The options will be read from both
    /// command line and the nduck.json file.
    /// </summary>
    public class ExecutionOptions
    {
        /// <summary>
        /// Helper class used for command line parsing.
        /// </summary>
        private readonly OptionSet _cmdOptions;

        /// <summary>
        /// Command to be executed.
        /// </summary>
        public ExecutionCommand Command { get; set; }

        /// <summary>
        /// List of the Commands available from the command
        /// line.
        /// </summary>
        public enum ExecutionCommand
        {
            /// <summary>
            /// Command used to Initialize the
            /// nduck.json file.
            /// </summary>
            Init,
            /// <summary>
            /// Builds the documentation.
            /// </summary>
            Build,
            /// <summary>
            /// Prints the help message.
            /// </summary>
            Help,
            /// <summary>
            /// Default unknown command.
            /// </summary>
            Unknown
        }

        /// <summary>
        /// The output path to be passed to
        /// the emitters (see <see cref="NDuck.Output.IOutputEmitter"/>)
        /// triggered by the build.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        /// List of the assemblies to be documented.
        /// </summary>
        /// <remarks>
        /// The list should contain a list of valid paths,
        /// including the .dll or .exe extension.
        /// </remarks>
        public List<string> Assemblies { get; set; }

        /// <summary>
        /// Defines the ouput level of the logger.
        /// </summary>
        public Logger.OutputLevel Verbosity { get; set; }

        /// <summary>
        /// List of the emitters enabled on the current build.
        /// </summary>
        public List<String> Emitters { get; set; }

        /// <summary>
        /// Path to the configuration file.
        /// If omitted, defaults to "nduck.json".
        /// </summary>
        public String ConfigurationFile { get; set; }

        /// <summary>
        /// Path to the Content directory, which contains
        /// extra resources like help topics, guides, pictures
        /// and so on. Defaults to "./content".
        /// </summary>
        public String ContentDirectory { get; set; }

        /// <summary>
        /// Represents a message about options validation.
        /// </summary>
        private string Message { get; set; }

        /// <summary>
        /// Returns the currently running exe file name.
        /// </summary>
        public string ExeFileName
        {
            get { return Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName); }
        }

        /// <summary>
        /// True if there where validation errors.
        /// </summary>
        public bool HasErrors { get; set; }

        private const string HEADER_TEXT = @"NDuck: A documentation generation tool for .NET, (vaguely) inspired by JsDuck

Usage:
  nduck init [options] [projectFile]          Initializes a new NDuck project.
  nduck build [options] [projectFile]         Builds the current project.
  nduck help                                  Prints this help message.

";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExecutionOptions()
        {
            _cmdOptions = new OptionSet()
                .Add(HEADER_TEXT)
                .Add("h|?|help", "Prints this help message", v => Command = ExecutionCommand.Help)
                .Add("v|verbose", "Prints debug messages during execution.", v => Verbosity = Logger.OutputLevel.Debug)
                .Add("s|silent", "Reduces console output to a minimum.", v => Verbosity = Logger.OutputLevel.Error)
                .Add("in=", "The {INPUT} assemblies to be documented.", v => Assemblies.Add(v))
                .Add("o|out=", "The {OUTDIR} base folder for the generated files.", v => OutputPath = v)
                ;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ExecutionOptions(string[] args) : this()
        {
            Parse(args);
        }

        /// <summary>
        /// Parses the command line arguments array and loads the
        /// option instance. Usually this method is called
        /// by the constructor.
        /// </summary>
        /// <param name="args">
        /// A valid command line args array.
        /// </param>
        public void Parse(string[] args)
        {
            if (args == null) throw new ArgumentNullException("args");

            Command = ExecutionCommand.Unknown;

            try
            {
                var additionalOptions = _cmdOptions.Parse(args);

                //if (additionalInFiles != null && additionalInFiles.Count > 0)
                //    InFiles.AddRange(additionalInFiles);
            }
            catch (OptionException e)
            {
                Logger.Error("There was an error parsing command line arguments: {0}.", e.Message);

                Logger.WriteLine(@"Try `{0} --help` for more information.", typeof(ExecutionOptions).Assembly.FullName);
            }
        }

        /// <summary>
        /// Writes the Command Line help
        /// to the provided <see cref="System.IO.TextWriter"/> or <see cref="System.Console.Out"/>
        /// </summary>
        /// <param name="textWriter">
        /// A textWriter to be used for output. When null or unspecified, defaults
        /// to <see cref="System.Console.Out"/>
        /// </param>
        /// <returns></returns>
        public void WriteHelpMessage(TextWriter textWriter = null)
        {
            if (textWriter == null) textWriter = Console.Out;

            _cmdOptions.WriteOptionDescriptions(textWriter);
            if (!String.IsNullOrEmpty(Message)) textWriter.WriteLine(Message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="defaults"></param>
        public void ApplyDefaults(ExecutionOptions defaults)
        {
            
        }

        /// <summary>
        /// Reads an instance from a Json string.
        /// </summary>
        /// <param name="json">
        /// A valid json representation
        /// of an <see cref="ExecutionOptions"/> instance.
        /// </param>
        /// <returns>
        /// An options instance.
        /// </returns>
        public static ExecutionOptions ReadFromJson(string json)
        {
            try
            {
                var settings = new JsonSerializerSettings();
                return JsonConvert.DeserializeObject<ExecutionOptions>(json, settings);
            }
            catch (Exception e)
            {
                Logger.Error("There was an error deserializing options json: " + e.Message);
                
                throw new InvalidOperationException("There was an error deserializing options.", e);
            }
        }
    }
}