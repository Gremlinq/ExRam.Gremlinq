namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct TwoContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly Traversal _continuation1;
        private readonly Traversal _continuation2;
        private readonly FinalContinuationBuilder<TOuterQuery> _finalBuilder;

        private TwoContinuationBuilder(FinalContinuationBuilder<TOuterQuery> finalBuilder, Traversal continuation1, Traversal continuation2)
        {
            _finalBuilder = finalBuilder;
            _continuation1 = continuation1;
            _continuation2 = continuation2;
        }

        public TResult Build<TResult>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, Traversal, TResult> builderTransformation)
            => builderTransformation(_finalBuilder, _continuation1, _continuation2);

        public static TwoContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery, TState>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TState, TProjectedQuery> continuation1, Func<TAnonymousQuery, TState, TProjectedQuery> continuation2, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase => new(
                FinalContinuationBuilder<TOuterQuery>
                    .Create(outer)
                    .Apply(continuation1, anonymous, flags, out var continuationTraversal1, state)
                    .Apply(continuation2, anonymous, flags, out var continuationTraversal2, state),
                continuationTraversal1,
                continuationTraversal2);
    }
}
