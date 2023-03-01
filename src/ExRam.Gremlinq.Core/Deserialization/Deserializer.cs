using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class Deserializer
    {
        public static readonly IDeserializer Identity = new Transformer(ImmutableStack<IConverterFactory>.Empty)
            .Add(new IdentityConverterFactory());

        public static readonly IDeserializer Default = Identity
            .Add(new SingleItemArrayFallbackConverterFactory())
            .AddToStringFallback();

        public static IDeserializer AddToStringFallback(this IDeserializer deserializer) => deserializer
            .Add(new ToStringFallbackConverterFactory());
    }
}
