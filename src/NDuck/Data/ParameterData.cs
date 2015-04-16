using System;
using System.Linq;
using Mono.Cecil;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing informations regarding a
    /// method parameter.
    /// </summary>
    public class ParameterData
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public ParameterData()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="parameterDefinition">
        /// The Cecil parameter definition data.
        /// </param>
        public ParameterData(ParameterDefinition parameterDefinition)
        {
            Name = parameterDefinition.Name;
            Type = parameterDefinition.ParameterType.FullName;
            IsOut = parameterDefinition.IsOut;
            IsRef = parameterDefinition.ParameterType.IsByReference;
        }

        /// <summary>
        /// The name of the parameter.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The full name of the Type of the parameter.
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// True if the parameter is configured as Out.
        /// </summary>
        public Boolean IsOut { get; set; }

        /// <summary>
        /// True if the parameter is configured as Out.
        /// </summary>
        public Boolean IsRef { get; set; }

        /// <summary>
        /// Description of the parameter, coming from
        /// Xml Doc file.
        /// </summary>
        public String ParameterDescription { get; set; }
    }
}