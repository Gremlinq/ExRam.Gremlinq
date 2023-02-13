using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IGremlinQueryFragmentDeserializer
    {
        bool TryDeserialize<TSerialized>(TSerialized serialized, Type requestedType, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out object? value);

        IGremlinQueryFragmentDeserializer Override(GremlinQueryFragmentDeserializerDelegate deserializer);
    }
}
