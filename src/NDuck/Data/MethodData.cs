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
        /// The na
        /// </summary>
        public String FullName { get; set; }

        /// <summary>
        /// The Accessor specified for this Method.
        /// </summary>
        public AccessorType Accessor { get; set; }

        /// <summary>
        /// The full name of the Return Type for this method.
        /// </summary>
        public String ReturnType { get; set; }

        /// <summary>
        /// Represents a list of the parameters to be passed to this method.
        /// </summary>
        public List<ParameterData> Parameters { get; set; }

        /// <summary>
        /// True if the method has parameters.
        /// </summary>
        public Boolean HasParameters
        {
            get { return Parameters.Count > 0; }
        }

        /// <summary>
        /// True if this method is a constructor.
        /// </summary>
        public Boolean IsConstructor { get; set; }

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
        /// Default Constructor.
        /// </summary>
        /// <param name="method">
        /// The Cecil reflected method definition.
        /// </param>
        public MethodData(MethodDefinition method)
            : this()
        {
            if (method == null) throw new ArgumentNullException("method");
            
            Name = method.Name;
            FullName = method.FullName;
            Accessor = ReadAccessor(method);
            ReturnType = method.ReturnType.FullName;
            IsConstructor = method.IsConstructor;

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
        public static AccessorType ReadAccessor(MethodDefinition method)
        {
            if (method.IsPublic) return AccessorType.Public;
            if (method.IsPrivate) return AccessorType.Private;
            if (method.IsFamily) return AccessorType.Protected;
            if (method.IsFamilyOrAssembly) return AccessorType.ProtectedInternal;
            if (method.IsAssembly) return AccessorType.Internal;

            return AccessorType.Invalid;
            //throw new InvalidOperationException("Could not determine the Accessor for method " + method.FullName);
        }

        /// <summary>
        /// Returns a string representation of the Method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullName;
        }

    }


}
