using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQueryFragmentDeserializerClassExtensions
    {
        public readonly struct FluentForClass<TRequested>
            where TRequested : class
        {
            private readonly IGremlinQueryFragmentDeserializer _deserializer;

            public FluentForClass(IGremlinQueryFragmentDeserializer deserializer)
            {
                _deserializer = deserializer;
            }

            public TRequested? From<TSerialized>(TSerialized serialized, IGremlinQueryEnvironment environment) => _deserializer.TryDeserialize<TSerialized, TRequested>(serialized, environment, out var value)
                ? value
                : default;
        }

        public static FluentForClass<TRequested> TryDeserialize<TRequested>(this IGremlinQueryFragmentDeserializer deserializer)
            where TRequested : class
        {
            return new FluentForClass<TRequested>(deserializer);
        }
    }
}
