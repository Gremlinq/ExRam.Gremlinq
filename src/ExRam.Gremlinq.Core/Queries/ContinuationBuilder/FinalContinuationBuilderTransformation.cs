namespace ExRam.Gremlinq.Core
{
    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery, TState>(FinalContinuationBuilder<TOuterQuery> builder, Span<Traversal> traversals, TState state)
         where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery>(FinalContinuationBuilder<TOuterQuery> builder, Span<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;
}
