using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private readonly ImmutableStack<IDeserializationTransformationFactory> _transformationFactories;

            public GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformationFactory> transformationFactories)
            {
                _transformationFactories = transformationFactories;
            }

            public bool TryDeserialize<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TRequested? value)
            {
                foreach (var transformationFactory in _transformationFactories)
                {
                    if (transformationFactory.TryCreate<TSerialized, TRequested>() is { } transformation)
                    {
                        if (transformation.Transform(serialized, environment, this, out value))
                            return true;
                    }
                }

                value = default;
                return false;
            }

            public IGremlinQueryFragmentDeserializer Override(IDeserializationTransformationFactory deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(_transformationFactories.Push(deserializer));
            }
        }

        private sealed class SingleItemArrayFallbackDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class SingleItemArrayFallbackDeserializationTransformation<TSerialized, TRequestedArray, TRequestedArrayItem> : IDeserializationTransformation<TSerialized, TRequestedArray>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequestedArray? value)
                {
                    if (recurse.TryDeserialize<TSerialized, TRequestedArrayItem>(serialized, environment, out var typedValue))
                    {
                        value = (TRequestedArray)(object) new[] { typedValue };
                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return typeof(TRequested).IsArray
                    ? (IDeserializationTransformation<TSerialized, TRequested>?)Activator.CreateInstance(typeof(SingleItemArrayFallbackDeserializationTransformation<,,>).MakeGenericType(typeof(TSerialized), typeof(TRequested), typeof(TRequested).GetElementType()!))
                    : default;
            }
        }

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformationFactory>.Empty)
            .Override(DeserializationTransformationFactory.Identity);

        public static readonly IGremlinQueryFragmentDeserializer Default = Identity
            .Override(new SingleItemArrayFallbackDeserializationTransformationFactory())
            .AddToStringFallback();

        public static IGremlinQueryFragmentDeserializer AddToStringFallback(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override<object, string>(static (data, env, recurse) => data.ToString());
    }
}
