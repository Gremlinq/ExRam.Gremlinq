using System;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class SerializedGremlinQueryExtensions
    {
        public static GroovyGremlinQuery ToGroovy(this ISerializedGremlinQuery serializedGremlinQuery)
        {
            return serializedGremlinQuery.TryToGroovy() ?? throw new NotSupportedException($"Can't convert serialized query of type {serializedGremlinQuery.GetType()} to {nameof(GroovyGremlinQuery)}.");
        }

        internal static GroovyGremlinQuery? TryToGroovy(this ISerializedGremlinQuery serializedGremlinQuery)
        {
            return serializedGremlinQuery switch
            {
                GroovyGremlinQuery serializedQuery => serializedQuery,
                BytecodeGremlinQuery byteCodeQuery => byteCodeQuery.ToGroovy(),
                _ => default
            };
        }
    }
}
