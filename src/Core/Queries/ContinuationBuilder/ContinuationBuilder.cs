namespace ExRam.Gremlinq.Core
{
    internal delegate FinalContinuationBuilder<TOuterQuery> SpanStateBuilderTransformation<TOuterQuery, TState>(FinalContinuationBuilder<TOuterQuery> a, ReadOnlySpan<TState> b)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TNewQuery SpanStateBuilderTransformation<TOuterQuery, TState, TNewQuery>(FinalContinuationBuilder<TOuterQuery> a, ReadOnlySpan<TState> b)
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


        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase => With(
                static (anonymous, continuation) => continuation(anonymous),
                continuation);

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => new (_outer, _anonymous, continuation.Apply(_anonymous, state), _flags);

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations)
            where TProjectedQuery : IGremlinQueryBase
        {
            var multi = ToMulti();

            for (var i = 0; i < continuations.Length; i++)
            {
                multi = multi
                    .With(continuations[i]);
            }

            return multi;
        }


        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti() => new (_outer, _anonymous, FastImmutableList<IGremlinQueryBase>.Empty, _flags);


        public TOuterQuery Build(Func<FinalContinuationBuilder<TOuterQuery>, FinalContinuationBuilder<TOuterQuery>> builderTransformation)
            => Build(static (builder, state) => state(builder).Build(), builderTransformation);

        public TOuterQuery Build<TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, FinalContinuationBuilder<TOuterQuery>> builderTransformation, TState state)
            => Build(static (builder, tuple) => tuple.builderTransformation(builder, tuple.state).Build(), (builderTransformation, state));


        public TOuterQuery Build<TState>(SpanStateBuilderTransformation<TOuterQuery, TState> builderTransformation, ReadOnlySpan<TState> state)
            => builderTransformation(new FinalContinuationBuilder<TOuterQuery>(_outer), state).Build();


        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, TNewQuery> builderTransformation)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery>(_outer));

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, TNewQuery> builderTransformation, TState state)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery>(_outer), state);


        public TNewQuery Build<TNewQuery, TState>(SpanStateBuilderTransformation<TOuterQuery, TState, TNewQuery> builderTransformation, ReadOnlySpan<TState> state)
            where TNewQuery : IStartGremlinQuery => builderTransformation(new FinalContinuationBuilder<TOuterQuery>(_outer), state);
    }
}
