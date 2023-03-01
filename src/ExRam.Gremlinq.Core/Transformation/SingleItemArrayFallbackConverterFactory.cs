using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    internal sealed class SingleItemArrayFallbackConverterFactory : IConverterFactory
    {
        private sealed class SingleItemArrayFallbackConverter<TSerialized, TRequestedArray, TRequestedArrayItem> : IConverter<TSerialized, TRequestedArray>
        {
            public bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
            {
                if (recurse.TryDeserialize<TSerialized, TRequestedArrayItem>(serialized, environment, out var typedValue))
                {
                    value = (TRequestedArray)(object)new[] { typedValue };
                    return true;
                }

                value = default;
                return false;
            }
        }

        public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
        {
            return typeof(TRequested).IsArray
                ? (IConverter<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackConverter<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                : default;
        }
    }
}
