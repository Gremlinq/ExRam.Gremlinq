using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class Deserializer
    {
        public static readonly ITransformer Default = Transformer.Identity
            .Add(new SingleItemArrayFallbackConverterFactory())
            .AddToStringFallback();

        public static ITransformer AddToStringFallback(this ITransformer deserializer) => deserializer
            .Add(new ToStringFallbackConverterFactory());
    }
}
