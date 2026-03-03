using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
{
    /// <summary>
    /// Provides the default <see cref="ITransformer"/> instance for deserializing query results.
    /// </summary>
    public static class Deserializer
    {
        /// <summary>
        /// The default deserializer transformer.
        /// </summary>
        public static readonly ITransformer Default = Transformer.DeserializerEmpty
            .AsIncomplete();
    }
}
