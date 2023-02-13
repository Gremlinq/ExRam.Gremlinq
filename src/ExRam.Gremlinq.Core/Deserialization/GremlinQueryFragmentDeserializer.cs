using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private readonly ImmutableStack<IDeserializationTransformation> _delegates;

            public GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformation> delegates)
            {
                _delegates = delegates;
            }

            public bool TryDeserialize<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TRequested? value)
            {
                foreach (var deserializer in _delegates)
                {
                    if (deserializer.Execute(serialized, environment, this, out value))
                        return true;
                }

                value = default;
                return false;
            }

            public IGremlinQueryFragmentDeserializer Override(IDeserializationTransformation deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(_delegates.Push(deserializer));
            }
        }

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformation>.Empty)
            .Override(DeserializationTransformation.Identity);

        public static readonly IGremlinQueryFragmentDeserializer Default = Identity
            .Override<object>(static (data, type, env, recurse) =>
            {
                if (type.IsInstanceOfType(data))
                    return data;

                if (type.IsArray)
                {
                    var elementType = type.GetElementType()!;

                    if (recurse.TryDeserialize(elementType).From(data, env) is { } element)
                    {
                        var ret = Array.CreateInstance(elementType, 1);

                        ret
                            .SetValue(element, 0);

                        return ret;
                    }
                }

                return default;
            })
            .AddToStringFallback();

        public static IGremlinQueryFragmentDeserializer AddToStringFallback(this IGremlinQueryFragmentDeserializer deserializer) => deserializer
            .Override<object>(static (data, type, env, recurse) => type == typeof(string)
                ? data.ToString()
                : default(object?));

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TNative>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, IDeserializationTransformation deserializerDelegate)
        {
            //TODO: Dedicated!
            return fragmentDeserializer
                .Override<TSerialized>((token, type, env, recurse) => type == typeof(TNative)
                    ? deserializerDelegate.Execute<TSerialized, TNative>(token, env, recurse, out var value)
                        ? value
                        : default(object?)
                    : default(object?));
        }
    }
}
