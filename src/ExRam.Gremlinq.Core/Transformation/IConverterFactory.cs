namespace ExRam.Gremlinq.Core.Transformation
{
    public interface IConverterFactory
    {
        IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>();
    }
}
