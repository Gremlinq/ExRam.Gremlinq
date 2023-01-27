using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IGremlinQueryFragmentDeserializer
    {
        bool TryDeserialize<TSerializedData>(TSerializedData serializedData, Type fragmentType, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out object? value);

        IGremlinQueryFragmentDeserializer Override<TSerialized>(GremlinQueryFragmentDeserializerDelegate<TSerialized> deserializer);
    }
}
