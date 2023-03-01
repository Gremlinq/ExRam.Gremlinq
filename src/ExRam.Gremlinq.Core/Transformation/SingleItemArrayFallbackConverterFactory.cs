using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
    {
        private sealed class SingleItemArrayFallbackConverter<TSource, TRequestedArray, TRequestedArrayItem> : IConverter<TSource, TRequestedArray>
        {
            public bool TryConvert(TSource serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
            {
                if (recurse.TryDeserialize<TSource, TRequestedArrayItem>(serialized, environment, out var typedValue))
                {
                    value = (TRequestedArray)(object)new[] { typedValue };
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSource, TRequested>? TryCreate<TSource, TRequested>()
        {
            return typeof(TRequested).IsArray
                ? (IConverter<TSource, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSource), typeof(TRequested), typeof(TRequested).GetElementType()!))
                : default;
        }
    }
}
