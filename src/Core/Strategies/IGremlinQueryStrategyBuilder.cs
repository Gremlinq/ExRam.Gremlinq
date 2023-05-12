namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryStrategyBuilder<TStrategy>
        where TStrategy : IGremlinQueryStrategy
    {
        Func<IGremlinQuerySource, TStrategy> Create();
    }
}
