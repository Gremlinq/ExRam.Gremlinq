namespace ExRam.Gremlinq.Core
{
    internal delegate FinalContinuationBuilder<TOuterQuery, TNewQuery> FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery, in TState>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Span<Traversal> traversals, TState state)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;

    internal delegate FinalContinuationBuilder<TOuterQuery, TNewQuery> FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> builder, Span<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;
}
