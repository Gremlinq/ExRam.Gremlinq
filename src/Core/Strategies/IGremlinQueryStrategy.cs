#if !NET7_0_OR_GREATER
#pragma warning disable CA2252 // This API requires opting into preview features
#endif

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQueryStrategy
    {

    }

    public interface IGremlinQueryStrategy<TStrategyBuilder, TStrategy> : IGremlinQueryStrategy
        where TStrategyBuilder : IGremlinQueryStrategyBuilder<TStrategy>
        where TStrategy : IGremlinQueryStrategy<TStrategyBuilder, TStrategy>
    {
#if NET6_0_OR_GREATER
        static abstract TStrategyBuilder Build();
#endif
    }
}
