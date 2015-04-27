using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using NDuck.Data.Enum;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Type
    /// </summary>
    public class TypeData : DocumentableBase
    {
        /// <summary>
        /// The name of the Assembly this type is defined in.
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
        /// Defines the class type (class, enum, interface or
        /// struct) for the present type.
        /// </summary>
        public ClassType ClassType { get; set; }

        /// <summary>
        /// True if type is static.
        /// </summary>
        public Boolean IsStatic { get; set; }

        /// <summary>
        /// True if type is sealed.
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
        /// List of the Methods defined by this type.
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
        /// Default constructor.
        /// </summary>
        public TypeData()
        {
            Events = new List<EventData>();
            Fields = new List<FieldData>();
            Methods = new List<MethodData>();
            Properties = new List<PropertyData>();
            InterfacesImplemented = new List<string>();
            GenericParameters = new List<GenericParameterData>();
            NestedTypes = new List<TypeData>();
        }

        /// <summary>
        /// Constructor used for type mapping.
        /// </summary>
        /// <param name="type">
        /// A type definition extracted from a module by Cecil.
        /// </param>
        public TypeData(TypeDefinition type)
        {
            Logger.Debug("Reading type {0}...", type.Name);

            Name = type.Name;
            FullName = GetFullName(type);
            Namespace = type.IsNested ? GetFullName(type.DeclaringType) : type.Namespace;
            AssemblyName = type.Module.Assembly.Name.Name;
            IsSealed = type.IsSealed;
            IsStatic = type.IsAbstract && type.IsSealed;
            Accessor = ReadAccessor(type);
            ClassType = ReadClassType(type);
            BaseClass = type.BaseType != null ? type.BaseType.FullName : null;

            InterfacesImplemented = type.Interfaces != null ?
                type.Interfaces.Select(i => i.FullName).ToList() :
                new List<String>();

            Events = type.HasEvents ?
                type.Events.Select(e => new EventData(e)).ToList() :
                new List<EventData>();

            Fields = type.HasFields ?
                type.Fields.Select(f => new FieldData(f)).ToList() :
                new List<FieldData>();
            
            Methods = type.HasMethods ?
                type.Methods.Select(m => new MethodData(m)).ToList() :
                new List<MethodData>();
            
            Properties = type.HasProperties ?
                type.Properties.Select(p => new PropertyData(p)).ToList() :
                new List<PropertyData>();

            GenericParameters = type.HasGenericParameters ?
                type.GenericParameters.Select(gp => new GenericParameterData(gp)).ToList() :
                new List<GenericParameterData>();

            NestedTypes = type.HasNestedTypes ?
                type.NestedTypes.Select(t => new TypeData(t)).ToList() :
                new List<TypeData>();
        }

        /// <summary>
        /// Reads the ClassType from the definition.
        /// The class type can be Class, Enum, Interface or Struct.
        /// </summary>
        /// <param name="type">
        /// A Cecil reflected type definition instance.
        /// </param>
        /// <returns>
        /// The reflected <see cref="ClassType"/>.
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
        /// <param name="type">
        /// A type reference to be analyzed.
        /// </param>
        /// <returns>
        /// The type name, as will be defined in the xml
        /// documentation files.
        /// </returns>
        /// <remarks>
        /// I was not able to find a proper documentation for the
        /// inner details of the naming strategy, so this method has
        /// been completely reverse engineered from the widest set
        /// of samples I could come up with.
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

        /// <summary>
        /// Helper method used to compute the number of
        /// generic parameters.
        /// </summary>
        /// <param name="type">
        /// A type reference to be analyzed.
        /// </param>
        /// <returns>
        /// The number of generic parameters defined in the
        /// provided type reference.
        /// </returns>
        /// <remarks>
        /// This method is necessary because of nested types: at the IL
        /// level, a nested type will list also every generic parameter
        /// in the enclosing types, but the name of the type will report
        /// only the number of generic parameters declared for the
        /// current type; so, in order to produce the correct naming
        /// (in order to match the tags generated for the Xml documentation
        /// file), we have to make the difference between the parameters
        /// we see at the IL level, and the parameters which where defined
        /// in the enclosing types.
        /// </remarks>
        private static int GetGenericParametersCount(TypeReference type)
        {
            var inheritedParams = 0;
            var declaringType = type.DeclaringType;

            while (declaringType != null)
            {
                inheritedParams += GetGenericParametersCount(declaringType);
                declaringType = declaringType.DeclaringType;
            }
            
            return type.GenericParameters.Count - inheritedParams;
        }

        /// <summary>
        /// Returns a string representation of the type.
        /// </summary>
        /// <returns>
        /// The object string representation. Ha-ha!
        /// </returns>
        public override string ToString()
        {
            return FullName;
        }

    }
}