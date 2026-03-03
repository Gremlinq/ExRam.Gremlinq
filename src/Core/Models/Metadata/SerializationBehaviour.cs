namespace ExRam.Gremlinq.Core.Models
{
    /// <summary>
    /// Controls when a graph element property is included during serialization.
    /// </summary>
    [Flags]
    public enum SerializationBehaviour
    {
        /// <summary>
        /// The property is always serialized.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The property is ignored when adding a new element.
        /// </summary>
        IgnoreOnAdd = 1,

        /// <summary>
        /// The property is ignored when updating an existing element.
        /// </summary>
        IgnoreOnUpdate = 2,

        /// <summary>
        /// The property is always ignored during serialization.
        /// </summary>
        IgnoreAlways = 3
    }
}
