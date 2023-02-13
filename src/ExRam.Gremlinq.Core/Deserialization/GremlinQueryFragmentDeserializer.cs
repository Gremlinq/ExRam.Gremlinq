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

        public static readonly IGremlinQueryFragmentDeserializer Identity = new GremlinQueryFragmentDeserializerImpl(ImmutableStack<IDeserializationTransformationFactory>.Empty)
            .Override(DeserializationTransformationFactory.Identity);

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
                    ? deserializerDelegate.Transform<TSerialized, TNative>(token, env, recurse, out var value)
                        ? value
                        : default(object?)
                    : default(object?));
        }
    }
}
