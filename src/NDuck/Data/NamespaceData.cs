using System;
using System.Collections.Generic;
using System.Linq;

namespace NDuck.Data
{
    /// <summary>
    /// Represents every data linked
    /// to types in a namespace.
    /// </summary>
    public class NamespaceData
    {
        /// <summary>
        /// The name of this namespace.
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// The full name of this namespace.
        /// </summary>
        public String Fullname { get; private set; }

        /// <summary>
        /// A Dictionary containing the sub namespaces for this
        /// namespace. The index string is simply a namespace element
        /// (an id between two dots).
        /// </summary>
        public Dictionary<String, NamespaceData> SubNamespaces { get; private set; }

        /// <summary>
        /// List of the Types contained in this namespace.
        /// </summary>
        public Dictionary<String, TypeData> Types { get; private set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public NamespaceData()
        {
            SubNamespaces = new Dictionary<string, NamespaceData>();
            Types = new Dictionary<string, TypeData>();
        }

        /// <summary>
        /// Constructor initializing Namespace hierarchy.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parentNs"></param>
        public NamespaceData(string name, NamespaceData parentNs) : this()
        {
            Name = name;
            Fullname = (parentNs != null ? parentNs.Fullname + "." : "") + Name;
        }
    }
}