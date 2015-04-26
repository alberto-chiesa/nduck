using System;
using System.Collections.Generic;
using System.Linq;
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