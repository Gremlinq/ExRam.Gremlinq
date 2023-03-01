using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Deserialization;

namespace ExRam.Gremlinq.Core.Transformation
{
    public interface IConverter<in TSource, TRequested>
    {
        bool TryConvert(TSource source, IGremlinQueryEnvironment environment, IDeserializer recurse, [NotNullWhen(true)] out TRequested? value);
    }
}
