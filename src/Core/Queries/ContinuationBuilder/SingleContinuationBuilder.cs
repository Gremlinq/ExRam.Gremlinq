namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly Traversal _continuation;
        private readonly FinalContinuationBuilder<TOuterQuery> _finalBuilder;

        private SingleContinuationBuilder(FinalContinuationBuilder<TOuterQuery> finalBuilder, Traversal continuation)
        {
            _continuation = continuation;
            _finalBuilder = finalBuilder;
        }

        public TOuterQuery Build<TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, FinalContinuationBuilder<TOuterQuery>> builderTransformation, TState state)
            => Build(static (builder, traversal, tuple) => tuple.builderTransformation(builder, traversal, tuple.state).Build(), (builderTransformation, state));

        public TOuterQuery Build(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, FinalContinuationBuilder<TOuterQuery>> builderTransformation)
            => Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);

        public TResult Build<TResult>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TResult> builderTransformation)
            => Build(static (builder, traversal, builderTransformation) => builderTransformation(builder, traversal), builderTransformation);

        public TResult Build<TResult, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, TResult> builderTransformation, TState state)
            => builderTransformation(_finalBuilder, _continuation, state);


        public static SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery, TState>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TState, TProjectedQuery> continuation, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase => new (
                FinalContinuationBuilder<TOuterQuery>
                    .Create(outer)
                    .Apply(continuation, anonymous, flags, out var traversal, state),
                traversal);

        public static SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TProjectedQuery> continuation, ContinuationFlags flags)
            where TProjectedQuery : IGremlinQueryBase => new (
                FinalContinuationBuilder<TOuterQuery>
                    .Create(outer)
                    .Apply(continuation, anonymous, flags, out var traversal),
                traversal);
    }
}
