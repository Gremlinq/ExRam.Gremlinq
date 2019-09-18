namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySerializer
    {
        object Serialize(IGremlinQuery query);
    }
}
