namespace ExRam.Gremlinq.Core
{
    internal delegate TResult FinalContinuationBuilderTransformation<TResult>(FinalContinuationBuilder builder, Memory<Traversal> traversals);

    internal delegate TResult SpanFinalContinuationBuilderTransformation<TResult>(FinalContinuationBuilder builder, Span<Traversal> traversals);

    internal delegate TResult FinalContinuationBuilderTransformation<TResult, TSpan>(FinalContinuationBuilder builder, ReadOnlySpan<TSpan> span);

    internal delegate TResult FinalContinuationBuilderTransformation<TResult, TSpan, TState>(FinalContinuationBuilder builder, ReadOnlySpan<TSpan> span, TState state);
}
