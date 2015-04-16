using System;
using Mono.Cecil;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Property
    /// </summary>
    public class PropertyData
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Accessor defined for the property.
        /// </summary>
        public AccessorType Accessor { get; set; }

        /// <summary>
        /// The Accessor defined for the property.
        /// </summary>
        public AccessorType GetAccessor { get; set; }

        /// <summary>
        /// The Accessor defined for the property.
        /// </summary>
        public AccessorType SetAccessor { get; set; }

        /// <summary>
        /// The full name of the type of the property
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// True if the property specifies a get method.
        /// </summary>
        public Boolean HasGetter { get; set; }

        /// <summary>
        /// True if the property specifies a set method.
        /// </summary>
        public Boolean HasSetter { get; set; }

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public PropertyData()
        { }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="property">
        /// Property definition as reflected by Cecil.
        /// </param>
        public PropertyData(PropertyDefinition property)
        {
            Name = property.Name;
            Type = property.PropertyType.FullName;
            ReadAccessors(property);
            HasGetter = property.GetMethod != null;
            HasSetter = property.SetMethod != null;
        }

        /// <summary>
        /// Reads the Accessors for the property.
        /// </summary>
        /// <param name="property">
        /// Property definition as reflected by Cecil.
        /// </param>
        private void ReadAccessors(PropertyDefinition property)
        {
            GetAccessor = property.GetMethod != null ? MethodData.ReadAccessor(property.GetMethod) : AccessorType.Invalid;
            SetAccessor = property.SetMethod != null ? MethodData.ReadAccessor(property.SetMethod) : AccessorType.Invalid;
            Accessor = GetAccessor >= SetAccessor ? GetAccessor : SetAccessor;
        }
    }
}