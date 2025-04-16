namespace ExRam.Gremlinq.Core
{
    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult>(FinalContinuationBuilder<TOuterQuery> builder, Memory<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult, TState>(FinalContinuationBuilder<TOuterQuery> builder, ReadOnlySpan<TState> state)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TResult : IStartGremlinQuery;
}
