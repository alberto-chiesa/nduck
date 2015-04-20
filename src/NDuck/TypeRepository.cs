using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mono.Cecil;
using NDuck.Data;
using NDuck.XmlDoc;

namespace NDuck
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
        public Dictionary<string, NamespaceData> Namespaces { get; private set; }

        /// <summary>
        /// Stores an index of every type in the repository.
        /// </summary>
        private Dictionary<string, TypeData> TypesIndex { get; set; }

        /// <summary>
        /// Stores an index of every method in the repository.
        /// </summary>
        public Dictionary<string, MethodData> MethodIndex { get; set; }

        /// <summary>
        /// Stores an index of every Field in the repository.
        /// </summary>
        public Dictionary<string, FieldData> FieldIndex { get; set; }

        /// <summary>
        /// Stores an index of every Property in the repository.
        /// </summary>
        public Dictionary<string, PropertyData> PropertyIndex { get; set; }

        /// <summary>
        /// Stores an index of every Event in the repository.
        /// </summary>
        public Dictionary<string, EventData> EventIndex { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TypeRepository()
        {
            Namespaces = new Dictionary<string, NamespaceData>();
            TypesIndex = new Dictionary<string, TypeData>();
            MethodIndex = new Dictionary<string, MethodData>();
            FieldIndex = new Dictionary<string, FieldData>();
            PropertyIndex = new Dictionary<string, PropertyData>();
            EventIndex = new Dictionary<string, EventData>();
        }

        /// <summary>
        /// Loads the types defined into an assembly into the repository.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="xmlDocumentationFilePath"></param>
        public void LoadAssemblyTypes(String fullPath, String xmlDocumentationFilePath = null)
        {
            if (String.IsNullOrEmpty(fullPath)) throw new ArgumentNullException("fullPath");

            try
            {
                var module = ModuleDefinition.ReadModule(fullPath);

                foreach (var t in module.Types) LoadType(t);

                Reindex(Namespaces.Values);

                if (!String.IsNullOrEmpty(xmlDocumentationFilePath))
                    LoadXmlDoc(xmlDocumentationFilePath);
            }
            catch (Exception e)
            {
                Logger.Error("There was an error reading the requested module \"{0}\": {1}", fullPath, e.Message);
                Logger.Debug(ConsoleColor.DarkRed, "At: {0}", e.StackTrace);
                throw;
            }
        }

        private void Reindex(IEnumerable<NamespaceData> toList)
        {
            foreach (var ns in toList)
            {
                foreach (var type in ns.Types.Values) ReindexType(type);
    
                Reindex(ns.SubNamespaces.Values);
            }
        }

        private void ReindexType(TypeData type)
        {
            TypesIndex[type.FullName] = type;
            
            foreach (var m in type.Methods) MethodIndex[m.FullName] = m;

            foreach (var f in type.Fields) FieldIndex[f.FullName] = f;
            
            foreach (var p in type.Properties) PropertyIndex[p.FullName] = p;
            
            foreach (var e in type.Events) EventIndex[e.FullName] = e;
        }

        private void LoadType(TypeDefinition typeDefinition)
        {
            if (String.IsNullOrEmpty(typeDefinition.Namespace)) return;

            var ns = GetNamespace(typeDefinition.Namespace);

            var type = new TypeData(typeDefinition);
            ns.Types.Add(typeDefinition.Name, type);
        }

        /// <summary>
        /// Returns the requested type information,
        /// or throws in case of errors.
        /// </summary>
        /// <param name="fullName">
        /// The name of the type, including the namespace.
        /// </param>
        /// <returns></returns>
        public TypeData GetTypeData(String fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                throw new ArgumentNullException("fullName");

            if (!TypesIndex.ContainsKey(fullName))
                throw new ArgumentOutOfRangeException("fullName", "The requested type " + fullName + " was not found");

            return TypesIndex[fullName];
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


        /// <summary>
        /// Loads an xml documentation file into the repository,
        /// updating all the corresponding repository items.
        /// </summary>
        public void LoadXmlDoc(string xmlDocumentationFilePath)
        {
            var xmlDocumentation = XmlProcessor.ReadXmlDocumentation(xmlDocumentationFilePath);

            LoadXmlDoc(xmlDocumentation);
        }

        /// <summary>
        /// Loads an xml documentation file into the repository,
        /// updating all the corresponding repository items.
        /// </summary>
        /// <param name="xmlDocumentation">
        /// An <see cref="NDuck.XmlDoc.XmlDocumentation"/> instance
        /// produced by the elaboration of an Xml Documentation file.
        /// </param>
        public void LoadXmlDoc(XmlDocumentation xmlDocumentation)
        {
            if (xmlDocumentation == null) throw new ArgumentNullException("xmlDocumentation");

            foreach (var member in xmlDocumentation.Members)
            {
                if (member == null) continue;

                var documentable = GetIDocumentable(member);
                documentable.LoadDocumentation(member);
            }
        }

        private IDocumentable GetIDocumentable(XmlMemberDoc member)
        {
            switch (member.Type)
            {
                case MemberType.Type:
                    return GetIDocumentableFrom(TypesIndex, member.Name);
                case MemberType.Event:
                    return GetIDocumentableFrom(EventIndex, member.Name);
                case MemberType.Field:
                    return GetIDocumentableFrom(FieldIndex, member.Name);
                case MemberType.Method:
                    return GetIDocumentableFrom(MethodIndex, member.Name);
                case MemberType.Property:
                    return GetIDocumentableFrom(PropertyIndex, member.Name);
            }

            throw new InvalidOperationException("Unexpected member type: " + member.Type);
        }

        private static IDocumentable GetIDocumentableFrom<T>(Dictionary<String, T> dic, String name)
            where T : IDocumentable
        {
            name = name.Replace('{', '<').Replace('}', '>');

            if (!dic.ContainsKey(name))
                throw new InvalidOperationException("No " + name + " was found in the repository");

            return dic[name];
        }
    }
}
