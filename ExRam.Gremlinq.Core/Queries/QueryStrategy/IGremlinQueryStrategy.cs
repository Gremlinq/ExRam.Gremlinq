namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryStrategy
    {
        IGremlinQuery Apply(IGremlinQuery query);
    }
}
