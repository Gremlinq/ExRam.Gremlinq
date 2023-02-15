namespace ExRam.Gremlinq.Core.Deserialization
{
    public interface IDeserializationTransformationFactory
    {
        IDeserializationTransformation<TSerialized, TRequested>? TryCreate<TSerialized, TRequested>();
    }
}
