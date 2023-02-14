using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializationTransformationFactory
    {
        IDeserializationTransformation? TryCreate<TSerialized, TRequested>();
    }

    public interface IDeserializationTransformation
    {
        bool Transform<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }

    public static class DeserializationTransformationFactory
    {
        private sealed class DeserializationTransformationFactoryImpl<TStaticSerialized> : IDeserializationTransformationFactory
        {
            private sealed class DeserializationTransformationImpl : IDeserializationTransformation
            {
                private readonly Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> _func;

                public DeserializationTransformationImpl(Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
                {
                    _func = func;
                }

                public bool Transform<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
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

            public IDeserializationTransformation? TryCreate<TSerialized, TRequested>()
            {
                if (typeof(TStaticSerialized).IsAssignableFrom(typeof(TSerialized)) || (typeof(TSerialized).IsAssignableFrom(typeof(TStaticSerialized))))
                    return new DeserializationTransformationImpl(_func);

                return null;
            }
        }

        private sealed class IdentityDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            private sealed class IdentityDeserializationTransformation : IDeserializationTransformation
            {
                public bool Transform<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
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

            public IDeserializationTransformation? TryCreate<TSerialized, TRequested>() => new IdentityDeserializationTransformation();
        }

        public static readonly IDeserializationTransformationFactory Identity = new IdentityDeserializationTransformationFactory();

        public static IDeserializationTransformationFactory From<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new DeserializationTransformationFactoryImpl<TSerialized>(func);
        }
    }
}
