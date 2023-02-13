﻿namespace ExRam.Gremlinq.Core.Deserialization
{
    public abstract class GremlinQueryFragmentDeserializerDelegate
    {
        private sealed class IdentityGremlinQueryFragmentDeserializerDelegate : GremlinQueryFragmentDeserializerDelegate
        {
            public override object? Execute<TAvailableSerialized>(TAvailableSerialized serializedData, Type requestedType, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
            {
                if (requestedType.IsInstanceOfType(serializedData))
                    return serializedData;

                return null;
            }
        }

        private sealed class GremlinQueryFragmentDeserializerDelegateImpl<TStaticSerialized> : GremlinQueryFragmentDeserializerDelegate
        {
            private readonly Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> _func;

            public GremlinQueryFragmentDeserializerDelegateImpl(Func<TStaticSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
            {
                _func = func;
            }

            public override object? Execute<TSerialized>(TSerialized serializedData, Type requestedType, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse)
            {
                if (serializedData is TStaticSerialized staticSerialized)
                    return _func(staticSerialized, requestedType, environment, recurse);

                return null;
            }
        }

        public static GremlinQueryFragmentDeserializerDelegate Identity = new IdentityGremlinQueryFragmentDeserializerDelegate();

        public static GremlinQueryFragmentDeserializerDelegate From<TSerialized>(Func<TSerialized, Type, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, object?> func)
        {
            return new GremlinQueryFragmentDeserializerDelegateImpl<TSerialized>(func);
        }

        public abstract object? Execute<TSerialized>(TSerialized serializedData, Type requestedType, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse);
    }
}
