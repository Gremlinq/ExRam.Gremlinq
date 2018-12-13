namespace ExRam.Gremlinq
{
    public interface IGremlinQueryStrategy
    {
        IGremlinQuery<TElement> Apply<TElement>(IGremlinQuery<TElement> query);
    }
}
