using System;
using System.Linq;
using Mono.Cecil;

namespace NDuck.Data
{
    /// <summary>
    /// Class containing every information related to a Field
    /// </summary>
    public class FieldData
    {
        /// <summary>
        /// Contains the name of the property
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contains the full name of the type
        /// of this field
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Contains the Accessor type for this field.
        /// </summary>
        public AccessorType Accessor { get; set; }

        /// <summary>
        /// True if the field is const.
        /// </summary>
        public Boolean IsConst { get; set; }

        /// <summary>
        /// True if the field is static.
        /// </summary>
        public Boolean IsStatic { get; set; }

        /// <summary>
        /// True if the field is readonly.
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public FieldData()
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="field">
        /// Field definition, provided by Cecil.
        /// </param>
        public FieldData(FieldDefinition field)
        {
            Name = field.Name;
            Type = field.FieldType.FullName;
            Accessor = GetAccessor(field);
            IsStatic = field.IsStatic;
            IsReadOnly = field.IsInitOnly;
            IsConst = field.HasConstant;
        }

        /// <summary>
        /// Reads the accessor for the current method.
        /// </summary>
        /// <param name="field">The field definition as read by Cecil.</param>
        /// <returns>A <see cref="NDuck.Data.AccessorType"/> instance.</returns>
        /// <exception cref="System.InvalidOperationException">
        /// when the accessor type is not resolvable.
        /// </exception>
        private AccessorType GetAccessor(FieldDefinition field)
        {
            if (field.IsPublic) return AccessorType.Public;
            if (field.IsPrivate) return AccessorType.Private;
            if (field.IsFamily) return AccessorType.Protected;
            if (field.IsFamilyOrAssembly) return AccessorType.ProtectedInternal;
            if (field.IsAssembly) return AccessorType.Internal;

            throw new InvalidOperationException("Could not determine the Accessor for field " + field.FullName);
        }
    }
}