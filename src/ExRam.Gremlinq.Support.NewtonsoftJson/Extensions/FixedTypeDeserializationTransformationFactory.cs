using Newtonsoft.Json.Linq;

using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    internal abstract class FixedTypeDeserializationTransformationFactory<TStaticRequested> : IDeserializationTransformationFactory
        where TStaticRequested : struct
    {
        private sealed class FixedTypeDeserializationTransformation : IDeserializationTransformation<JValue, TStaticRequested>
        {
            private readonly FixedTypeDeserializationTransformationFactory<TStaticRequested> _factory;

            public FixedTypeDeserializationTransformation(FixedTypeDeserializationTransformationFactory<TStaticRequested> factory)
            {
                _factory = factory;
            }

            public bool Transform(JValue serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TStaticRequested value)
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


        public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
        {
            return typeof(TRequested) == typeof(TStaticRequested) && typeof(TSerialized) == typeof(JValue)
                ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new FixedTypeDeserializationTransformation(this)
                : null;
        }

        protected abstract TStaticRequested? Convert(JValue jValue, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse);
    }
}
