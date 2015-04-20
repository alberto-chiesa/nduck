using System;
using System.Linq;
using System.Xml.Linq;
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
            Type = GetTypeName(parameterDefinition);
            IsOut = parameterDefinition.IsOut;
            IsRef = parameterDefinition.ParameterType.IsByReference;
        }

        private static string GetTypeName(ParameterDefinition parameterDefinition)
        {
            return TypeData.GetFullName(parameterDefinition.ParameterType);
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
        public XElement ParameterDescription { get; set; }
    }
}