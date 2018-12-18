namespace ExRam.Gremlinq.Serialization
{
    public interface IGremlinQuerySerializer<out TSerialized>
    {
        TSerialized Serialize(IGremlinQuery query);
    }
}