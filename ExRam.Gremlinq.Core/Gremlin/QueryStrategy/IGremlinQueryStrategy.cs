namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryStrategy
    {
        IGremlinQuery<TElement> Apply<TElement>(IGremlinQuery<TElement> query);
    }
}
