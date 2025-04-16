namespace ExRam.Gremlinq.Core
{
    internal delegate TResult FinalContinuationBuilderTransformation<TOuterQuery, TResult>(FinalContinuationBuilder<TOuterQuery> builder, Memory<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;
}
