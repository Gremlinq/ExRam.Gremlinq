using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class ToStringFallbackConverterFactory : IConverterFactory
    {
        private sealed class ToStringFallbackConverter<TSource> : IConverter<TSource, string>
        {
            public bool TryConvert(TSource source, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out string? value)
            {
                if (source?.ToString() is { } requested)
                {
                    value = requested;
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>()
        {
            return typeof(TTarget) == typeof(string)
                ? (IConverter<TSource, TTarget>)(object)new ToStringFallbackConverter<TSource>()
                : default;
        }
    }
}
