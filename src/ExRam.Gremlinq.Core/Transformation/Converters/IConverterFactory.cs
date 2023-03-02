namespace ExRam.Gremlinq.Core.Transformation
{
    public interface IConverterFactory
    {
        IConverter<TSource, TTarget>? TryCreate<TSource, TTarget>();
    }
}
