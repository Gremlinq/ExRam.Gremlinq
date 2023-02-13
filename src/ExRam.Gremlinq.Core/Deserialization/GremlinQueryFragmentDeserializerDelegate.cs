using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public abstract class GremlinQueryFragmentDeserializerDelegate
    {
        private sealed class IdentityGremlinQueryFragmentDeserializerDelegate : GremlinQueryFragmentDeserializerDelegate
        {
            public override bool Execute<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value) where TRequested : default
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

        private sealed class GremlinQueryFragmentDeserializerDelegateImpl<TStaticSerialized> : GremlinQueryFragmentDeserializerDelegate
        {
            private readonly Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> _func;

            public GremlinQueryFragmentDeserializerDelegateImpl(Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
            {
                _func = func;
            }

            public override bool Execute<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value)
                where TRequested : default
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

        public static GremlinQueryFragmentDeserializerDelegate Identity = new IdentityGremlinQueryFragmentDeserializerDelegate();

        public static GremlinQueryFragmentDeserializerDelegate From<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new GremlinQueryFragmentDeserializerDelegateImpl<TSerialized>(func);
        }

        public abstract bool Execute<TSerialized, TRequested>(TSerialized serializedData, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }
}
