using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;
using NDuck.Data.Enum;
using NDuck.XmlDoc;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Type
    /// </summary>
    public class TypeData : DocumentableBase
    {
        /// <summary>
        /// The name of the Assembly this type is defined in
        /// </summary>
        public String AssemblyName { get; set; }

        /// <summary>
        /// The name of the containing namespace.
        /// </summary>
        public String Namespace { get; set; }

        /// <summary>
        /// The name of this type.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Stores the full name of this type.
        /// </summary>
        public String FullName { get; set; }

        /// <summary>
        /// The Accessor specified for this Type.
        /// </summary>
        public AccessorType Accessor { get; set; }

        /// <summary>
        /// The full name of the base type of this type.
        /// </summary>
        public String BaseClass { get; set; }

        /// <summary>
        /// True if type is static
        /// </summary>
        public ClassType ClassType { get; set; }

        /// <summary>
        /// True if type is static
        /// </summary>
        public Boolean IsStatic { get; set; }

        /// <summary>
        /// True if type is sealed
        /// </summary>
        public Boolean IsSealed { get; set; }

        /// <summary>
        /// The list of the interfaces this type directly implements.
        /// </summary>
        public List<String> InterfacesImplemented { get; set; }

        /// <summary>
        /// List of Events registered on this type.
        /// </summary>
        public List<EventData> Events { get; set; }

        /// <summary>
        /// List of the Fields registered on this type.
        /// </summary>
        public List<FieldData> Fields { get; set; }

        /// <summary>
        /// List of the Properties registered on this type.
        /// </summary>
        public List<PropertyData> Properties { get; set; }

        /// <summary>
        /// List of the Methods registered on this type.
        /// </summary>
        public List<MethodData> Methods { get; set; }

        /// <summary>
        /// List of the type parameters of this Type.
        /// </summary>
        public List<GenericParameterData> GenericParameters { get; set; }

        /// <summary>
        /// Contains a list of all the nested
        /// types for the current type.
        /// </summary>
        public List<TypeData> NestedTypes { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public TypeData()
        {
            Events = new List<EventData>();
            Fields = new List<FieldData>();
            Properties = new List<PropertyData>();
            Methods = new List<MethodData>();
            InterfacesImplemented = new List<string>();
            GenericParameters = new List<GenericParameterData>();
            NestedTypes = new List<TypeData>();
        }

        /// <summary>
        /// Constructor used for type mapping
        /// </summary>
        /// <param name="typeDefinition"></param>
        public TypeData(TypeDefinition typeDefinition) : this()
        {
            Logger.Debug("Reading type {0}...", typeDefinition.Name);

            Name = typeDefinition.Name;
            FullName = GetFullName(typeDefinition);
            Namespace = typeDefinition.IsNested ?
                GetFullName(typeDefinition.DeclaringType) :
                typeDefinition.Namespace;
            AssemblyName = typeDefinition.Module.Assembly.Name.Name;
            IsSealed = typeDefinition.IsSealed;
            IsStatic = typeDefinition.IsAbstract && typeDefinition.IsSealed;

            Accessor = ReadAccessor(typeDefinition);
            ClassType = ReadClassType(typeDefinition);

            BaseClass = typeDefinition.BaseType != null ? typeDefinition.BaseType.FullName : null;
            InterfacesImplemented.AddRange(typeDefinition.Interfaces.Select(i => i.FullName));

            if (typeDefinition.HasGenericParameters)
                GenericParameters.AddRange(typeDefinition.GenericParameters.Select(gp => new GenericParameterData(gp)));

            if (typeDefinition.HasEvents)
                Events.AddRange(typeDefinition.Events.Select(e => new EventData(e)));

            if (typeDefinition.HasMethods)
                Methods.AddRange(typeDefinition.Methods.Select(m => new MethodData(m)));

            if (typeDefinition.HasFields)
                Fields.AddRange(typeDefinition.Fields.Select(f => new FieldData(f)));

            if (typeDefinition.HasProperties)
                Properties.AddRange(typeDefinition.Properties.Select(p => new PropertyData(p)));

            if (typeDefinition.HasNestedTypes)
                NestedTypes.AddRange(typeDefinition.NestedTypes.Select(t => new TypeData(t)));
        }

        /// <summary>
        /// Reads the ClassType from the definition.
        /// The class type can be Class, Enum, Interface or Struct.
        /// </summary>
        /// <param name="type">
        /// A Cecil reflected type definition instance.
        /// </param>
        /// <returns>
        /// The reflected <see cref="ClassType"/>
        /// </returns>
        public static ClassType ReadClassType(TypeDefinition type)
        {
            if (type.IsInterface) return ClassType.Interface;
            if (type.IsEnum) return ClassType.Enum;
            if (type.IsClass) return type.IsValueType ? ClassType.Struct : ClassType.Class;

            throw new InvalidOperationException("The type " + type.Name + " seems not to be a class, interface, struct or enum.");
        }

        /// <summary>
        /// Reads the Accessor from Type definition.
        /// </summary>
        /// <param name="type">
        /// A Cecil reflected type definition instance.
        /// </param>
        /// <returns>The reflected <see cref="AccessorType"/>.</returns>
        public static AccessorType ReadAccessor(TypeDefinition type)
        {
            if (type.IsPublic || type.IsNestedPublic) return AccessorType.Public;
            
            if (type.IsNestedPrivate) return AccessorType.Private;

            return AccessorType.Internal;
        }

        /// <summary>
        /// Returns a method, specified by name.
        /// </summary>
        /// <param name="name">
        /// The name of the method to be retrieved.
        /// Can be either the simple name, or the full name
        /// including namespace and param types.
        /// </param>
        /// <returns>A <see cref="NDuck.Data.MethodData"/> instance.</returns>
        public MethodData GetMethod(String name)
        {
            return Methods.First(m => m.Name == name || m.FullName == name);
        }

        /// <summary>
        /// Returns a formatted full name of the type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <remarks>
        /// A type name is composed by 4 segments:
        ///   * the namespace (or the name of the enclosing type)
        ///   * the type name, stripped of any generic reference
        ///   * in case of generic types, a '`' followed by the number of parameters
        ///   * in case of generic instance types, a &lt;> enclosed list of type arguments
        /// </remarks>
        public static string GetFullName(TypeReference type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var typeNs = type.IsNested ? GetFullName(type.DeclaringType) : type.Namespace;

            var genericInstanceType = type as GenericInstanceType;

            var boundGenericTypes = (genericInstanceType != null) ?
                genericInstanceType.GenericArguments.Select(GetFullName) :
                null;

            var genericParametersCount = GetGenericParametersCount(type);

            return String.Concat(typeNs, ".",
                new String(type.Name.TakeWhile(c => c != '`').ToArray()),
                (genericParametersCount > 0 ? "`" + genericParametersCount : ""),
                boundGenericTypes != null ? "<" + String.Join(",", boundGenericTypes) + ">" : "");
        }

        private static int GetGenericParametersCount(TypeReference type)
        {
            var inheritedParams = type.IsNested ? GetGenericParametersCount(type.DeclaringType) : 0;
            return type.GenericParameters.Count - inheritedParams;
        }

        /// <summary>
        /// Returns a string representation of the type.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullName;
        }

    }
}