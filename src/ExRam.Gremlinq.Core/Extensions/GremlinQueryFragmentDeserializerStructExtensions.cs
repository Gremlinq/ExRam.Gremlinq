using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerStructExtensions
    {
        private sealed class FixedTypeDeserializationTransformationFactory<TStaticSerialized, TStaticRequested> : IDeserializationTransformationFactory
            where TStaticRequested : struct
        {
            private sealed class FixedTypeDeserializationTransformation<TSerialized> : IDeserializationTransformation<TSerialized, TStaticRequested>
            {
                private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TStaticRequested?> _func;

                public FixedTypeDeserializationTransformation(Func<TStaticSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TStaticRequested?> func)
                {
                    _func = func;
                }

                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TStaticRequested value)
                {
                    if (serialized is TStaticSerialized staticSerialized && _func(staticSerialized, environment, recurse) is { } requested)
                    {
                        value = requested;

                        return true;
                    }

                    value = default;

                    return false;
                }
            }

            private readonly Func<TStaticSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TStaticRequested?> _func;

            public FixedTypeDeserializationTransformationFactory(Func<TStaticSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TStaticRequested?> func)
            {
                _func = func;
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                return ((typeof(TSerialized).IsAssignableFrom(typeof(TStaticSerialized)) || typeof(TStaticSerialized).IsAssignableFrom(typeof(TSerialized))) && (typeof(TRequested) == typeof(TStaticRequested)))
                    ? (IDeserializationTransformation<TSerialized, TRequested>)(object)new FixedTypeDeserializationTransformation<TSerialized>(_func)
                    : null;
            }
        }

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TRequested>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, Func<TSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TRequested?> func)
            where TRequested : struct
        {
            return fragmentDeserializer
                .Override(new FixedTypeDeserializationTransformationFactory<TSerialized, TRequested>(func));
        }
    }
}
