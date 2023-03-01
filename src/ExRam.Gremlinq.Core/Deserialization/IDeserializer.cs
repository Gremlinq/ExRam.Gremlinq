using System.Diagnostics.CodeAnalysis;
using ExRam.Gremlinq.Core.Transformation;

namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializer
    {
        bool TryDeserialize<TSource, TTarget>(TSource source, IGremlinQueryEnvironment environment, [NotNullWhen(true)] out TTarget? value);

        IDeserializer Add(IConverterFactory converterFactory);
    }
}
