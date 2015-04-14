using System;
using System.Collections.Generic;

namespace IronDuck.XmlDoc
{
    /// <summary>
    /// Class representing
    /// the documentation extracted from
    /// an Xml Documentation file generated
    /// by Visual Studio.
    /// </summary>
    public class XmlDocumentation
    {
        /// <summary>
        /// Name of the Assembly
        /// </summary>
        public String AssemblyName { get; set; }

        /// <summary>
        /// Represents the complete list of Members documentation,
        /// extracted from the documentation file.
        /// </summary>
        public IList<XmlMemberDoc> Members { get; set; }
    }
}
