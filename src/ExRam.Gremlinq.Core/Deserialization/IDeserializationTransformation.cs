using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializationTransformationFactory
    {
        IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>();
    }

    public interface IDeserializationTransformation<TSerialized, TRequested>
    {
        bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }

    public static class DeserializationTransformationFactory
    {
        private sealed class DeserializationTransformationFactoryImpl<TStaticSerialized> : IDeserializationTransformationFactory
        {
            private sealed class DeserializationTransformationImpl<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
            {
                private readonly Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> _func;

                public DeserializationTransformationImpl(Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
                {
                    _func = func;
                }

                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (serialized is TStaticSerialized staticSerialized)
                    {
                        if (_func(staticSerialized, typeof(TRequested), environment, recurse) is TRequested value2)
                        {
                            value = value2;
                            return true;
                        }
                    }

                    value = default;
                    return false;
                }
            }

            private readonly Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> _func;

            public DeserializationTransformationFactoryImpl(Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
            {
                _func = func;
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>()
            {
                if (typeof(TStaticSerialized).IsAssignableFrom(typeof(TSerialized)) || (typeof(TSerialized).IsAssignableFrom(typeof(TStaticSerialized))))
                    return new DeserializationTransformationImpl<TSerialized, TRequested>(_func);

                return null;
            }
        }

        private sealed class IdentityDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class IdentityDeserializationTransformation<TSerialized, TRequested> : IDeserializationTransformation<TSerialized, TRequested>
            {
                public bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                {
                    if (typeof(TRequested).IsInstanceOfType(serialized))
                    {
                        value = (TRequested)(object)serialized!;

                        return true;
                    }

                    value = default;
                    return false;
                }
            }

            public IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>() => new IdentityDeserializationTransformation<TSerialized, TRequested>();
        }

        public static readonly IDeserializationTransformationFactory Identity = new IdentityDeserializationTransformationFactory();

        public static IDeserializationTransformationFactory From<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new DeserializationTransformationFactoryImpl<TSerialized>(func);
        }
    }
}
