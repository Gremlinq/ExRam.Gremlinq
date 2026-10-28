namespace ExRam.Gremlinq.Core
{
    // The anonymous query is created in With only: most continuations go straight to Build and never need it.
    internal readonly struct ContinuationBuilder<TAnonymousQuery>
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly GremlinQueryBase _outer;
        private readonly ContinuationFlags _flags;
        private readonly Func<GremlinQueryBase, TAnonymousQuery> _createAnonymous;

        public ContinuationBuilder(GremlinQueryBase outer, Func<GremlinQueryBase, TAnonymousQuery> createAnonymous, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _createAnonymous = createAnonymous;
        }

        public SingleContinuationBuilder<TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase => SingleContinuationBuilder<TAnonymousQuery>.Create(_outer, _createAnonymous(_outer), continuation, _flags);

        public SingleContinuationBuilder<TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => SingleContinuationBuilder<TAnonymousQuery>.Create(_outer, _createAnonymous(_outer), continuation, _flags, state);

        public TwoContinuationBuilder<TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation1, Func<TAnonymousQuery, TState, TProjectedQuery> continuation2, TState state)
            where TProjectedQuery : IGremlinQueryBase => TwoContinuationBuilder<TAnonymousQuery>.Create(_outer, _createAnonymous(_outer), continuation1, continuation2, _flags, state);

        public MultiContinuationBuilder<TAnonymousQuery, TProjectedQuery> With<TProjectedQuery>(ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations)
            where TProjectedQuery : IGremlinQueryBase => MultiContinuationBuilder<TAnonymousQuery, TProjectedQuery>.Create(_outer, _createAnonymous(_outer), continuations, _flags);

        public TResult Build<TResult>(Func<FinalContinuationBuilder, TResult> builderTransformation)
            => builderTransformation(FinalContinuationBuilder.Create(_outer));

        public TResult Build<TResult, TState>(Func<FinalContinuationBuilder, TState, TResult> builderTransformation, TState state)
            => builderTransformation(FinalContinuationBuilder.Create(_outer), state);

        public TResult Build<TResult, TState>(FinalContinuationBuilderTransformation<TResult, TState> builderTransformation, ReadOnlySpan<TState> state)
            => builderTransformation(FinalContinuationBuilder.Create(_outer), state);

        public TResult Build<TResult, TSpan, TState>(FinalContinuationBuilderTransformation<TResult, TSpan, TState> builderTransformation, ReadOnlySpan<TSpan> span, TState state)
            => builderTransformation(FinalContinuationBuilder.Create(_outer), span, state);
    }
}
