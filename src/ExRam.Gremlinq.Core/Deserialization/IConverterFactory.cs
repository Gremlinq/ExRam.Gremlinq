namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IConverterFactory
    {
        IConverter<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>();
    }
}
