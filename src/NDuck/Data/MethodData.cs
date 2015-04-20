﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;
using NDuck.Data.Enum;
using NDuck.XmlDoc;

namespace NDuck.Data
{

    
    /// <summary>
    /// Class containing every information related to a Property
    /// </summary>
    public class MethodData : IDocumentable
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
        /// Contains the Example documentation
        /// for this method.
        /// </summary>
        public XElement Example { get; set; }

        /// <summary>
        /// Contains the Remarks documentation
        /// for this method.
        /// </summary>
        public XElement Remarks { get; set; }

        /// <summary>
        /// Contains the Summary documentation
        /// for this method.
        /// </summary>
        public XElement Summary { get; set; }

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
            FullName = GetFullName(method);
            Accessor = ReadAccessor(method);
            ReturnType = method.ReturnType.FullName;
            IsConstructor = method.IsConstructor;

            if (method.HasParameters)
                Parameters.AddRange(method.Parameters.Select(p => new ParameterData(p)));
        }

        /// <summary>
        /// Builds the full name of the method, according
        /// to the format used in the Xml Documentation files.
        /// </summary>
        /// <param name="method">
        /// A method definition as built by Cecil.
        /// </param>
        /// <returns>
        /// The full name of the method.
        /// </returns>
        public static string GetFullName(MethodDefinition method)
        {
            var stringType = TypeData.GetFullName(method.DeclaringType);

            var methodName = method.IsConstructor ? "#ctor" : method.Name;

            var parameterTypes = method.HasParameters ?
                method.Parameters.Select(p => TypeData.GetFullName(p.ParameterType)) :
                null;

            var parameters = parameterTypes != null ?
                "(" + String.Join(",", parameterTypes) + ")" :
                String.Empty;

            return String.Concat(stringType, ".", methodName, parameters);
        }

        /// <summary>
        /// Reads the accessor for the current method.
        /// </summary>
        /// <param name="method">The method definition as read by Cecil.</param>
        /// <returns>A <see cref="AccessorType"/> instance.</returns>
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

        /// <summary>
        /// Loads the documentation data into this
        /// method data instance.
        /// </summary>
        /// <param name="doc"></param>
        public void LoadDocumentation(XmlMemberDoc doc)
        {
            Summary = doc.SummaryXml;
            Remarks = doc.RemarksXml;
            Example = doc.ExampleXml;

            ReadParametersDocs(doc);
        }

        private void ReadParametersDocs(XmlMemberDoc doc)
        {
            var paramDic = Parameters.ToDictionary(p => p.Name, p => p);

            foreach (var param in doc.ParamXmlList.Where(p => p != null && p.Attribute("name") != null))
            {
                var paramName = param.Attribute("name").Value;
                if (!String.IsNullOrEmpty(paramName) && paramDic.ContainsKey(paramName))
                    paramDic[paramName].ParameterDescription = param;
            }
        }
    }


}
