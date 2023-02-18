using Newtonsoft.Json.Linq;

using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    internal abstract class FixedTypeConverterFactory<TStaticRequested> : IConverterFactory
        where TStaticRequested : struct
    {
        private sealed class FixedTypeConverter : IConverter<JValue, TStaticRequested>
        {
            private readonly FixedTypeConverterFactory<TStaticRequested> _factory;

            public FixedTypeConverter(FixedTypeConverterFactory<TStaticRequested> factory)
            {
                _factory = factory;
            }

            public bool Transform(JValue serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TStaticRequested value)
            {
                if (_factory.Convert(serialized, environment, recurse) is { } requested)
                {
                    value = requested;

                    return true;
                }

                value = default;

                return false;
            }
        }


        public IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
        {
            return typeof(TRequested) == typeof(TStaticRequested) && typeof(TSerialized) == typeof(JValue)
                ? (IConverter<TSerialized, TRequested>)(object)new FixedTypeConverter(this)
                : null;
        }

        protected abstract TStaticRequested? Convert(JValue jValue, IGremlinQueryEnvironment environment, IDeserializer recurse);
    }
}
