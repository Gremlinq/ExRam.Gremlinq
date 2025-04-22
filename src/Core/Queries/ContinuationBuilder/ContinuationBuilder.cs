namespace ExRam.Gremlinq.Core
{
    internal readonly struct ContinuationBuilder<TAnonymousQuery>
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly GremlinQueryBase _query;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery _anonymous;

        public ContinuationBuilder(GremlinQueryBase query, TAnonymousQuery anonymous, ContinuationFlags flags)
        {
            _query = query;
            _flags = flags;
            _anonymous = anonymous;
        }

        public SingleContinuationBuilder<TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase => SingleContinuationBuilder<TAnonymousQuery>.Create(_query, _anonymous, continuation, _flags);

        public SingleContinuationBuilder<TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => SingleContinuationBuilder<TAnonymousQuery>.Create(_query, _anonymous, continuation, _flags, state);


        public TwoContinuationBuilder<TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation1, Func<TAnonymousQuery, TState, TProjectedQuery> continuation2, TState state)
            where TProjectedQuery : IGremlinQueryBase => TwoContinuationBuilder<TAnonymousQuery>.Create(_query, _anonymous, continuation1, continuation2, _flags, state);


        public MultiContinuationBuilder<TAnonymousQuery> With<TProjectedQuery>(ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations)
            where TProjectedQuery : IGremlinQueryBase => MultiContinuationBuilder<TAnonymousQuery>.Create(_query, _anonymous, continuations, _flags);

        public MultiContinuationBuilder<TAnonymousQuery> With<TProjectedQuery, TState>(ReadOnlySpan<Func<TAnonymousQuery, TState, TProjectedQuery>> continuations, TState state)
            where TProjectedQuery : IGremlinQueryBase => MultiContinuationBuilder<TAnonymousQuery>.Create(_query, _anonymous, continuations, _flags, state);


        public TResult Build<TResult>(Func<FinalContinuationBuilder, TResult> builderTransformation)
            => builderTransformation(FinalContinuationBuilder.Create(_query));

        public TResult Build<TResult, TState>(Func<FinalContinuationBuilder, TState, TResult> builderTransformation, TState state)
            => builderTransformation(FinalContinuationBuilder.Create(_query), state);

        public TResult Build<TResult, TState>(FinalContinuationBuilderTransformation<TResult, TState> builderTransformation, ReadOnlySpan<TState> state)
            => builderTransformation(FinalContinuationBuilder.Create(_query), state);

        public TResult Build<TResult, TSpan, TState>(FinalContinuationBuilderTransformation<TResult, TSpan, TState> builderTransformation, ReadOnlySpan<TSpan> span, TState state)
            => builderTransformation(FinalContinuationBuilder.Create(_query), span, state);
    }
}
