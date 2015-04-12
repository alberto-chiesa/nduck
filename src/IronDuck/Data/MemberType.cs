namespace IronDuck.Data
{
    /// <summary>
    /// Available Types for an Xml Documentation
    /// &lt;member&gt; tag.
    /// </summary>
    public enum MemberType
    {
        /// <summary>
        /// Represents an Event
        /// </summary>
        Event,
        /// <summary>
        /// Represents a Field
        /// </summary>
        Field,
        /// <summary>
        /// Represents a Method
        /// </summary>
        Method,
        /// <summary>
        /// Represents a Property
        /// </summary>
        Property,
        /// <summary>
        /// Represents a Type or Interface
        /// </summary>
        Type
    }
}