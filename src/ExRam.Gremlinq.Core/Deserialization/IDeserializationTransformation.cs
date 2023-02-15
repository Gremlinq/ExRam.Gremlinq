using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializationTransformation<TSerialized, TRequested>
    {
        bool Transform(TSerialized serialized, IGremlinQueryEnvironment environment, IGremlinQueryFragmentDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }
}
