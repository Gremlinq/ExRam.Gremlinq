using System.Diagnostics.CodeAnalysis;

namespace ExRam.Gremlinq.Core.Transformation
{
    public interface IConverter<in TSource, TTarget>
    {
        bool TryConvert(TSource source, ITransformer defer, ITransformer recurse, [NotNullWhen(true)] out TTarget? value);
    }
}
