using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace NDuck.Data
{
    /// <summary>
    /// Class responsible for containing
    /// all the information to be outputted
    /// to documentation, organized in namespaces,
    /// types, etc.
    /// </summary>
    public class TypeRepository
    {
        /// <summary>
        /// Dictionary of the available namespaces
        /// </summary>
        public Dictionary<String, NamespaceData> Namespaces { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TypeRepository()
        {
            Namespaces = new Dictionary<string, NamespaceData>();
        }

        /// <summary>
        /// Loads the types defined into an assembly into the repository.
        /// </summary>
        /// <param name="fullPath"></param>
        public void LoadAssemblyTypes(String fullPath)
        {
            if (String.IsNullOrEmpty(fullPath)) throw new ArgumentNullException("fullPath");

            try
            {
                var module = ModuleDefinition.ReadModule(fullPath);
                
                foreach (var t in module.Types)
                {
                    LoadType(t);
                }
            }
            catch (Exception e)
            {
                Logger.Error("There was an error reading the requested module \"{0}\": {1}", fullPath, e.Message);
                Logger.Debug(ConsoleColor.DarkRed, "At: {0}", e.StackTrace);
            }
        }

        private void LoadType(TypeDefinition typeDefinition)
        {
            if (String.IsNullOrEmpty(typeDefinition.Namespace)) return;

            var ns = GetNamespace(typeDefinition.Namespace);

            ns.Types.Add(typeDefinition.Name, new TypeData(typeDefinition));
        }

        /// <summary>
        /// Gets the requested namespace from the hierarchy
        /// of namespaces and initializes as necessary.
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        private NamespaceData GetNamespace(string namespaceName)
        {
            var namespaces = (!namespaceName.Contains(".")) ? new[] {namespaceName} : namespaceName.Split('.');

            NamespaceData ns = null;
            var subNamespaces = Namespaces;
            foreach (var nstoken in namespaces)
            {
                if (!subNamespaces.ContainsKey(nstoken)) subNamespaces[nstoken] = new NamespaceData(nstoken, ns);

                ns = subNamespaces[nstoken];
                subNamespaces = ns.SubNamespaces;
            }

            return ns;
        }

    }

    /// <summary>
    /// Represents every data linked
    /// to types in a namespace.
    /// </summary>
    public class NamespaceData
    {
        /// <summary>
        /// The name of this namespace.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// The full name of this namespace.
        /// </summary>
        public String Fullname { get; set; }

        /// <summary>
        /// A Dictionary containing the sub namespaces for this
        /// namespace. The index string is simply a namespace element
        /// (an id between two dots).
        /// </summary>
        public Dictionary<String, NamespaceData> SubNamespaces { get; set; }

        /// <summary>
        /// List of the Types contained in this namespace.
        /// </summary>
        public Dictionary<String, TypeData> Types { get; set; }

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
