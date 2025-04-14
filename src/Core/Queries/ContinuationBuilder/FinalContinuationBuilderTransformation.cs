namespace ExRam.Gremlinq.Core
{
    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult, in TState>(FinalContinuationBuilder<TOuterQuery> builder, Span<Traversal> traversals, TState state)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult>(FinalContinuationBuilder<TOuterQuery> builder, Span<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;
}
