using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializationTransformation
    {
        bool Execute<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }

    public static class DeserializationTransformation
    {
        private sealed class IdentityDeserializationTransformation : IDeserializationTransformation
        {
            public bool Execute<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
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

            public bool Execute<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
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
