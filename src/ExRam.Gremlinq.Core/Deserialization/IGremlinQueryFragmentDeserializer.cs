using System;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IGremlinQueryFragmentDeserializer
    {
        object? TryDeserialize<TSerializedData>(TSerializedData serializedData, Type fragmentType, IGremlinQueryEnvironment environment);

        IGremlinQueryFragmentDeserializer Override<TSerialized>(GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer);
    }
}
