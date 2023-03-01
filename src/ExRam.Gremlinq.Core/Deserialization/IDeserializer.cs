using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializer
    {
        bool TryDeserialize<TSource, TRequested>(TSource serialized, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TRequested? value);

        IDeserializer Add(IConverterFactory converterFactory);
    }
}
