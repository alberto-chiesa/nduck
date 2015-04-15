using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Property
    /// </summary>
    public class MethodData
    {
        /// <summary>
        /// The name of the method.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The Accessor specified for this Method.
        /// </summary>
        public AccessorType Accessor { get; set; }

        /// <summary>
        /// The full name of the Return Type for this method.
        /// </summary>
        public String ReturnType { get; set; }

        /// <summary>
        /// Represents a list of the parameters to be passed to this method
        /// </summary>
        public List<ParameterData> Parameters { get; set; }

        /// <summary>
        /// The text of the documentation summary
        /// related to this method.
        /// </summary>
        public String SummaryText { get; set; }

        /// <summary>
        /// The text of the documentation remarks section,
        /// related to this method.
        /// </summary>
        public String RemarksText { get; set; }

        /// <summary>
        /// The text for the Example section.
        /// </summary>
        public String ExampleText { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MethodData()
        {
            Parameters = new List<ParameterData>();
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="method">
        /// The Cecil reflected method definition.
        /// </param>
        public MethodData(MethodDefinition method)
            : this()
        {
            if (method == null) throw new ArgumentNullException("method");
            
            Name = method.Name;
            Accessor = ReadAccessor(method);
            ReturnType = method.ReturnType.FullName;
            
            if (method.HasParameters)
                Parameters.AddRange(method.Parameters.Select(p => new ParameterData(p)));
        }

        /// <summary>
        /// Reads the accessor for the current method.
        /// </summary>
        /// <param name="method">The method definition as read by Cecil.</param>
        /// <returns>A <see cref="NDuck.Data.AccessorType"/> instance.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// when the accessor type is not resolvable.
        /// </exception>
        private AccessorType ReadAccessor(MethodDefinition method)
        {
            if (method.IsPublic) return AccessorType.Public;
            if (method.IsPrivate) return AccessorType.Private;
            if (method.IsFamily) return AccessorType.Protected;
            if (method.IsFamilyOrAssembly) return AccessorType.ProtectedInternal;
            if (method.IsFamilyAndAssembly) return AccessorType.Internal;

            throw new InvalidOperationException("Could not determine the Accessor for method " + method.FullName);
        }

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
}