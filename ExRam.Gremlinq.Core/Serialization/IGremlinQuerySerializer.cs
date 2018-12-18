namespace ExRam.Gremlinq.Core.Serialization
{
    public interface IGremlinQuerySerializer<out TSerialized>
    {
        TSerialized Serialize(IGremlinQuery query);
    }
}
