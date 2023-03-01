namespace ExRam.Gremlinq.Core.Transformation
{
    public interface IConverterFactory
    {
        IConverter<TSource, TRequested>? TryCreate<TSource, TRequested>();
    }
}
