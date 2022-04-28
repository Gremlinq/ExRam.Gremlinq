using System;

namespace ExRam.Gremlinq.Core.Serialization
{
    public static class SerializedGremlinQueryExtensions
    {
        public static GroovyGremlinQuery ToGroovy(this ISerializedGremlinQuery serializedGremlinQuery, GroovyFormatting formatting = GroovyFormatting.WithBindings)
        {
            return serializedGremlinQuery.TryToGroovy(formatting) ?? throw new NotSupportedException($"Can't convert serialized query of type {serializedGremlinQuery.GetType()} to {nameof(GroovyGremlinQuery)}.");
        }

        internal static GroovyGremlinQuery? TryToGroovy(this ISerializedGremlinQuery serializedGremlinQuery, GroovyFormatting formatting = GroovyFormatting.WithBindings)
        {
            return serializedGremlinQuery switch
            {
                GroovyGremlinQuery serializedQuery => formatting == GroovyFormatting.Inline
                    ? serializedQuery.Inline()
                    : serializedQuery,
                BytecodeGremlinQuery byteCodeQuery => byteCodeQuery.ToGroovy(formatting),
                _ => default
            };
        }
    }
}
