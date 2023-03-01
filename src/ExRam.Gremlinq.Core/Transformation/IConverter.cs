using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    public interface IConverter<in TSerialized, TRequested>
    {
        bool TryConvert(TSerialized serialized, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }
}
