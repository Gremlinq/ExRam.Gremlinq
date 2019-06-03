namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySerializer<out TSerializedQuery>
    {
        TSerializedQuery Serialize(IGremlinQuery query);
    }
}
