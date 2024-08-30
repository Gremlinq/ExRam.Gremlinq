namespace ExRam.Gremlinq.Core
{
    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, out TNewQuery, in TState>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Span<Traversal> traversals, TState state)
         where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, out TNewQuery>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Span<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;
}
