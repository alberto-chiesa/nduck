using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private const string HEADER_TEXT = @"NDuck: A documentation generation tool for .NET, (vaguely) inspired by JsDuck

Usage:
  nduck init [options] [projectFile]          Initializes a new NDuck project.
  nduck build [options] [projectFile]         Builds the current project.
  nduck help                                  Prints this help message.

";

        private static readonly Dictionary<string, ExecutionCommand> ExecutionCommands = new Dictionary<string, ExecutionCommand>
        {
            {ExecutionCommand.Build.ToString().ToLowerInvariant(), ExecutionCommand.Build},
            {ExecutionCommand.Init.ToString().ToLowerInvariant(), ExecutionCommand.Init},
            {ExecutionCommand.Help.ToString().ToLowerInvariant(), ExecutionCommand.Help}
        };

        /// <summary>
        /// Helper class used for command line parsing.
        /// </summary>
        private readonly OptionSet _cmdOptions;

        /// <summary>
        /// Command to be executed.
        /// </summary>
        public ExecutionCommand Command { get; set; }

        /// <summary>
        /// The output path to be passed to
        /// the emitters (see <see cref="NDuck.Output.IOutputEmitter" />)
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
        public Logger.OutputLevel? Verbosity { get; set; }

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
            get { return Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName); }
        }

        /// <summary>
        /// True if there where validation errors.
        /// </summary>
        public bool HasErrors { get; set; }

        /// <summary>
        /// Returns an <see cref="ExecutionOptions" />
        /// instance with default values.
        /// </summary>
        public static ExecutionOptions Default
        {
            get
            {
                return new ExecutionOptions
                {
                    ConfigurationFile = "nduck.json",
                    OutputPath = "./nduck",
                    Verbosity = Logger.OutputLevel.Warning
                };
            }
        }

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
                .Add("c|content=", "The {CONTENTDIR} holding additional content files.", v => ContentDirectory = v)
                .Add("p|prjfile=", "The json {FILE} holding the build configuration. Defaults to './nduck.json.'", v => ConfigurationFile = v);

            Assemblies = new List<string>();
            Verbosity = null;
        }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ExecutionOptions(string[] args) : this()
        {
            try
            {
                Parse(args);
                ApplyDefaults(Default);
            }
            catch (Exception e)
            {
                HasErrors = true;
                Logger.Error("There was an error parsing command line arguments: {0}.", e.Message);
                Logger.WriteLine(@"Try `{0} --help` for more information.", typeof(ExecutionOptions).Assembly.FullName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="defaults"></param>
        private void ApplyDefaults(ExecutionOptions defaults)
        {
            if (defaults == null) throw new ArgumentNullException("defaults");

            if (String.IsNullOrEmpty(ContentDirectory))
                ContentDirectory = defaults.ContentDirectory;

            if (Emitters == null || Emitters.Count == 0)
                Emitters = defaults.Emitters;

            if (String.IsNullOrEmpty(OutputPath))
                OutputPath = defaults.OutputPath;

            if (Verbosity == null)
                Verbosity = defaults.Verbosity;
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

                ReadCommandFromPrompt(additionalOptions);
                //if (additionalInFiles != null && additionalInFiles.Count > 0)
                //    InFiles.AddRange(additionalInFiles);
            }
            catch (OptionException e)
            {
                HasErrors = true;
                Logger.Error("There was an error parsing command line arguments: {0}.", e.Message);
                Logger.WriteLine(@"Try `{0} --help` for more information.", typeof(ExecutionOptions).Assembly.FullName);
            }
        }

        /// <summary>
        /// Reads the command line options to extract the command
        /// specified.
        /// </summary>
        /// <param name="additionalOptions"></param>
        private void ReadCommandFromPrompt(List<string> additionalOptions)
        {
            if (additionalOptions == null || additionalOptions.Count < 1)
            {
                HasErrors = true;
                Message = String.Format("No command was specified. Available commands are:\n{0}",
                    String.Join("\n", ExecutionCommands.Keys.Select(s => "   " + s)));
                return;
            }

            var cmd = additionalOptions[0].ToLowerInvariant();
            if (!ExecutionCommands.ContainsKey(cmd))
            {
                HasErrors = true;
                Message = String.Format("The requested command '{0}' could not be found. Available commands are:\n{1}",
                    cmd,
                    String.Join("\n", ExecutionCommands.Keys.Select(s => "   " + s)));

                return;
            }

            Command = ExecutionCommands[cmd];
        }

        /// <summary>
        /// Reads an <see cref="ExecutionOptions" /> instance
        /// from a file (containing json data).
        /// </summary>
        /// <param name="filePath">
        /// A valid path.
        /// </param>
        /// <returns>
        /// An options instance.
        /// </returns>
        public static ExecutionOptions ReadFromFile(string filePath)
        {
            if (filePath == null) throw new ArgumentNullException("filePath");
            if (!File.Exists(filePath)) throw new InvalidOperationException("The file " + filePath + " was not found.");

            string json;

            try
            {
                json = File.ReadAllText(filePath);
            }
            catch (Exception e)
            {
                Logger.Error("There was an error reading configuration file : " + e.Message);

                throw new InvalidOperationException("There was an error reading configuration file .", e);
            }

            return ReadFromJson(json);
        }

        /// <summary>
        /// Reads an instance from a Json string.
        /// </summary>
        /// <param name="json">
        /// A valid json representation
        /// of an <see cref="ExecutionOptions" /> instance.
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

        /// <summary>
        /// Writes the Command Line help
        /// to the provided <see cref="System.IO.TextWriter" /> or <see cref="System.Console.Out" />
        /// </summary>
        /// <param name="textWriter">
        /// A textWriter to be used for output. When null or unspecified, defaults
        /// to <see cref="System.Console.Out" />
        /// </param>
        /// <returns></returns>
        public void WriteHelpMessage(TextWriter textWriter = null)
        {
            if (textWriter == null) textWriter = Console.Out;

            _cmdOptions.WriteOptionDescriptions(textWriter);
            if (!String.IsNullOrEmpty(Message)) textWriter.WriteLine(Message);
        }
    }
}