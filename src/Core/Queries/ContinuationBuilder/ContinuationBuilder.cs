namespace ExRam.Gremlinq.Core
{
    internal readonly struct ContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery _anonymous;

        public ContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
        }

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase => SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>.Create(_outer, _anonymous, continuation, _flags);

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>.Create(_outer, _anonymous, continuation, _flags, state);


        public TwoContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation1, Func<TAnonymousQuery, TState, TProjectedQuery> continuation2, TState state)
            where TProjectedQuery : IGremlinQueryBase => TwoContinuationBuilder<TOuterQuery, TAnonymousQuery>.Create(_outer, _anonymous, continuation1, continuation2, _flags, state);


        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations)
            where TProjectedQuery : IGremlinQueryBase => MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>.Create(_outer, _anonymous, continuations, _flags);

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(ReadOnlySpan<Func<TAnonymousQuery, TState, TProjectedQuery>> continuations, TState state)
            where TProjectedQuery : IGremlinQueryBase => MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>.Create(_outer, _anonymous, continuations, _flags, state);


        public TOuterQuery Build(Func<FinalContinuationBuilder<TOuterQuery>, FinalContinuationBuilder<TOuterQuery>> builderTransformation)
            => Build(static (builder, state) => state(builder).BuildAs<TOuterQuery>(), builderTransformation);

        public TOuterQuery Build<TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, FinalContinuationBuilder<TOuterQuery>> builderTransformation, TState state)
            => Build(static (builder, tuple) => tuple.builderTransformation(builder, tuple.state).BuildAs<TOuterQuery>(), (builderTransformation, state));

        public TResult Build<TResult>(Func<FinalContinuationBuilder<TOuterQuery>, TResult> builderTransformation)
            => builderTransformation(FinalContinuationBuilder<TOuterQuery>.Create(_outer));

        public TResult Build<TResult, TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, TResult> builderTransformation, TState state)
            => builderTransformation(FinalContinuationBuilder<TOuterQuery>.Create(_outer), state);

        public TResult Build<TResult, TState>(FinalContinuationBuilderTransformation<TOuterQuery, TResult, TState> builderTransformation, ReadOnlySpan<TState> state)
            => builderTransformation(FinalContinuationBuilder<TOuterQuery>.Create(_outer), state);

        public TResult Build<TResult, TSpan, TState>(FinalContinuationBuilderTransformation<TOuterQuery, TResult, TSpan, TState> builderTransformation, ReadOnlySpan<TSpan> span, TState state)
            => builderTransformation(FinalContinuationBuilder<TOuterQuery>.Create(_outer), span, state);
    }
}
