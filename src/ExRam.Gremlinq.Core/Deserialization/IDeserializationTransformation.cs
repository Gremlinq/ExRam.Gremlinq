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
            private readonly Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> _func;

            public DeserializationTransformationFactoryImpl(Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
            {
                _func = func;
            }

            public IDeserializationTransformation? TryCreate<TSerialized, TRequested>()
            {
                if (typeof(TStaticSerialized).IsAssignableFrom(typeof(TSerialized)) || (typeof(TSerialized).IsAssignableFrom(typeof(TStaticSerialized))))
                    return DeserializationTransformation.From(_func);

                return null;
            }
        }

        private sealed class IdentityDeserializationTransformationFactory : IDeserializationTransformationFactory
        {
            public IDeserializationTransformation? TryCreate<TSerialized, TRequested>() => DeserializationTransformation.Identity;
        }

        public static IDeserializationTransformationFactory Identity = new IdentityDeserializationTransformationFactory();

        public static IDeserializationTransformationFactory From<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new DeserializationTransformationFactoryImpl<TSerialized>(func);
        }
    }

    public static class DeserializationTransformation
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

        private sealed class DeserializationTransformationImpl<TStaticSerialized> : IDeserializationTransformation
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

        public static IDeserializationTransformation Identity = new IdentityDeserializationTransformation();

        public static IDeserializationTransformation From<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new DeserializationTransformationImpl<TSerialized>(func);
        }
    }
}
