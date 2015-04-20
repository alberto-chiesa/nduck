namespace NDuck.Data.Enum
{
    /// <summary>
    /// Accessors defined for types, properties, methods, etc.
    /// </summary>
    public enum AccessorType
    {
        /// <summary>
        /// Identified elements that are not existing or
        /// invalid. Usually identifies non existing getters or setters
        /// methods of analyzed properties.
        /// </summary>
        Invalid,
        /// <summary>
        /// Private Accessor
        /// </summary>
        Private,
        /// <summary>
        /// Protected Accessor
        /// </summary>
        Protected,
        /// <summary>
        /// Internal accessor
        /// </summary>
        Internal,
        /// <summary>
        /// Protected Internal accessor
        /// </summary>
        ProtectedInternal,
        /// <summary>
        /// Public Accessor
        /// </summary>
        Public
    }
}