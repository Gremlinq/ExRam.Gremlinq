namespace ExRam.Gremlinq.Core
{
    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult>(FinalContinuationBuilder builder, Memory<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult, TSpan>(FinalContinuationBuilder builder, ReadOnlySpan<TSpan> span)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult, TSpan, TState>(FinalContinuationBuilder builder, ReadOnlySpan<TSpan> span, TState state)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;
}
