namespace ExRam.Gremlinq.Core
{
    internal delegate FinalContinuationBuilder<TOuterQuery, TNewQuery> SpanStateBuilderTransformation<TOuterQuery, TState, TNewQuery>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> a, ReadOnlySpan<TState> b)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;

    internal delegate FinalContinuationBuilder<TOuterQuery, TNewQuery> SpanStateBuilderTransformation<TOuterQuery, TSpanState, TState, TNewQuery>(FinalContinuationBuilder<TOuterQuery, TOuterQuery> a, ReadOnlySpan<TSpanState> b, TState c)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TNewQuery : IStartGremlinQuery;

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

        public ContinuationBuilder<TNewOuterQuery, TAnonymousQuery> WithOuter<TNewOuterQuery>(TNewOuterQuery query)
            where TNewOuterQuery : GremlinQueryBase, IGremlinQueryBase => new (query, _anonymous, _flags);

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => new (_outer, _anonymous, continuation.Apply(_anonymous, state), _flags);

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti() => new (_outer, _anonymous, FastImmutableList<IGremlinQueryBase>.Empty, _flags);

        public TOuterQuery Build<TState>(Func<FinalContinuationBuilder<TOuterQuery, TOuterQuery>, TState, FinalContinuationBuilder<TOuterQuery, TOuterQuery>> builderTransformation, TState state)
            => Build(static (builder, tuple) => tuple.builderTransformation(builder, tuple.state).Build(), (builderTransformation, state));

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery, TOuterQuery>, TNewQuery> builderTransformation)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery, TOuterQuery>(_outer));

        public TNewQuery Build<TNewQuery, TState>(SpanStateBuilderTransformation<TOuterQuery, TState, TNewQuery> builderTransformation, ReadOnlySpan<TState> state)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery, TOuterQuery>(_outer), state).Build();

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery, TOuterQuery>, TState, TNewQuery> builderTransformation, TState state)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery, TOuterQuery>(_outer), state);

        public TNewQuery Build<TNewQuery, TSpanState, TState>(SpanStateBuilderTransformation<TOuterQuery, TSpanState, TState, TNewQuery> builderTransformation, ReadOnlySpan<TSpanState> spanState, TState state)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery, TOuterQuery>(_outer), spanState, state).Build();
    }
}
