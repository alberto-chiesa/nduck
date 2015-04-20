using System;
using System.Xml.Linq;
using Mono.Cecil;
using NDuck.Data.Enum;
using NDuck.XmlDoc;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Property
    /// </summary>
    public class PropertyData : IDocumentable
    {
        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The full name of the property.
        /// </summary>
        public string FullName { get; set; }

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
        /// Contains the Example documentation
        /// for this property.
        /// </summary>
        public XElement Example { get; set; }

        /// <summary>
        /// Contains the Remarks documentation
        /// for this Property
        /// </summary>
        public XElement Remarks { get; set; }

        /// <summary>
        /// Contains the Summary documentation
        /// for this property
        /// </summary>
        public XElement Summary { get; set; }

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
            if (property == null) throw new ArgumentNullException("property");

            Name = property.Name;
            FullName = GetFullName(property);
            Type = property.PropertyType.FullName;
            ReadAccessors(property);
            HasGetter = property.GetMethod != null;
            HasSetter = property.SetMethod != null;
        }

        /// <summary>
        /// Format the property's full name.
        /// </summary>
        /// <param name="property">A property definition reflected by Cecil.</param>
        /// <returns>The property formatted name.</returns>
        public static string GetFullName(PropertyDefinition property)
        {
            return String.Concat(TypeData.GetFullName(property.DeclaringType), ".", property.Name);
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

        public void LoadDocumentation(XmlMemberDoc doc)
        {
            Summary = doc.SummaryXml;
            Remarks = doc.RemarksXml;
            Example = doc.ExampleXml;
        }
    }
}