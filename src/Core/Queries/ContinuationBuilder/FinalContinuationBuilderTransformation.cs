namespace ExRam.Gremlinq.Core
{
    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult>(FinalContinuationBuilder<TOuterQuery> builder, Memory<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TNewQuery SpanStateBuilderTransformation<TOuterQuery, TState, TNewQuery>(FinalContinuationBuilder<TOuterQuery> a, ReadOnlySpan<TState> b)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;
}
