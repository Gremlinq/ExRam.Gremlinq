using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public static class GremlinQueryFragmentDeserializer
    {
        private sealed class GremlinQueryFragmentDeserializerImpl : IGremlinQueryFragmentDeserializer
        {
            private readonly IImmutableList<GremlinQueryFragmentDeserializerDelegate> _delegates;

            public GremlinQueryFragmentDeserializerImpl(IImmutableList<GremlinQueryFragmentDeserializerDelegate> delegates)
            {
                _delegates = delegates;
            }

            public bool TryDeserialize<TSerialized>(TSerialized serialized, Type requestedType, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out object? value)
            {
                foreach (var deserializer in _delegates.Reverse())//TODO: Horror.
                {
                    if (deserializer.Execute(serialized, requestedType, environment, this) is { } ret)
                    {
                        value = ret;
                        return true;
                    }
                }

                value = null;
                return false;
            }

            public IGremlinQueryFragmentDeserializer Override(GremlinQueryFragmentDeserializerDelegate deserializer)
            {
                return new GremlinQueryFragmentDeserializerImpl(_delegates.Add(deserializer));
            }
        }

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableList<GremlinQueryFragmentDeserializerDelegate>.Empty)
            .Override(GremlinQueryFragmentDeserializerDelegate.Identity);

        public static readonly IGremlinQueryFragmentDeserializer Default = Identity
            .Override<object>(static (data, type, env, recurse) =>
            {
                if (type.IsInstanceOfType(data))
                    return data;

                if (type.IsArray)
                {
                    var elementType = type.GetElementType()!;

                    if (recurse.TryDeserialize(data, elementType, env, out var element))
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

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TNative>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, GremlinQueryFragmentDeserializerDelegate deserializerDelegate)
        {
            return fragmentDeserializer
                .Override<TSerialized>((token, type, env, recurse) => type == typeof(TNative)
                    ? deserializerDelegate.Execute(token, type, env, recurse)
                    : default);
        }
    }
}
