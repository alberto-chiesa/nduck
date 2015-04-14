using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace IronDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Type
    /// </summary>
    public class TypeData
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
        /// The text of the documentation summary
        /// related to this type.
        /// </summary>
        public String SummaryText { get; set; }

        /// <summary>
        /// The text of the documentation remarks section,
        /// related to this type.
        /// </summary>
        public String RemarksText { get; set; }

        /// <summary>
        /// The text for the Example section.
        /// </summary>
        public String ExampleText { get; set; }

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
        /// Default constructor
        /// </summary>
        public TypeData()
        {
            Events = new List<EventData>();
            Fields = new List<FieldData>();
            Properties = new List<PropertyData>();
            Methods = new List<MethodData>();
            InterfacesImplemented = new List<string>();
        }

        /// <summary>
        /// Constructor used for type mapping
        /// </summary>
        /// <param name="typeDefinition"></param>
        public TypeData(TypeDefinition typeDefinition) : this()
        {
            Name = typeDefinition.Name;
            Namespace = typeDefinition.Namespace;
            AssemblyName = typeDefinition.Module.Assembly.Name.Name;
            IsSealed = typeDefinition.IsSealed;
            IsStatic = typeDefinition.IsAbstract && typeDefinition.IsSealed;

            Accessor = ReadAccessor(typeDefinition);
            ClassType = ReadClassType(typeDefinition);

            BaseClass = typeDefinition.BaseType.FullName;
            InterfacesImplemented.AddRange(typeDefinition.Interfaces.Select(i => i.FullName));
            if (typeDefinition.HasEvents)
                Events.AddRange(typeDefinition.Events.Select(e => new EventData(e)));

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
            if (type.IsClass) return type.IsValueType ? ClassType.Struct : ClassType.Class;
            if (type.IsInterface) return ClassType.Interface;
            if (type.IsEnum) return ClassType.Enum;

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
    }
}