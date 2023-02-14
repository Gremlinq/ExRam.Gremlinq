using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerStructExtensions
    {
        public readonly struct FluentForStruct<TRequested>
            where TRequested : struct
        {
            private readonly IGremlinQueryFragmentDeserializer _deserializer;

            public FluentForStruct(IGremlinQueryFragmentDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TRequested? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSerialized, TRequested>(serialized, environment, out var value)
                ? value
                : default(TRequested?);
        }

        public static FluentForStruct<TRequested> TryDeserialize<TRequested>(this IGremlinQueryFragmentDeserializer deserializer)
            where TRequested : struct
        {
            return new FluentForStruct<TRequested>(deserializer);
        }

        public static IGremlinQueryFragmentDeserializer Override<TSerialized, TRequested>(this IGremlinQueryFragmentDeserializer fragmentDeserializer, Func<TSerialized, IGremlinQueryEnvironment, IGremlinQueryFragmentDeserializer, TRequested?> func)
            where TRequested : struct
        {
            return fragmentDeserializer
                .Override<TSerialized>((token, type, env, recurse) => type == typeof(TRequested)
                    ? func(token, env, recurse) is { } value
                        ? value
                        : default(object?)
                    : default);
        }
    }
}
