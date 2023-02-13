using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IGremlinQueryFragmentDeserializer
    {
        bool TryDeserialize<TSerialized, TRequested>(TSerialized serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TRequested? value);

        IGremlinQueryFragmentDeserializer Override(IDeserializationTransformation deserializer);
    }
}
