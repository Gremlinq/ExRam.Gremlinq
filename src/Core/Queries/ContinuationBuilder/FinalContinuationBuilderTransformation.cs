namespace ExRam.Gremlinq.Core
{
    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery, in TState>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Span<Traversal> traversals, TState state)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;

    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Span<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;
}
