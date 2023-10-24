namespace ExRam.Gremlinq.Core
{
    internal readonly struct ContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery? _anonymous;

        public ContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
        }

        public ContinuationBuilder<TNewOuterQuery, TAnonymousQuery> WithOuter<TNewOuterQuery>(TNewOuterQuery query)
            where TNewOuterQuery : GremlinQueryBase, IGremlinQueryBase => With(
                static (_, anonymous, flags, query) => new ContinuationBuilder<TNewOuterQuery, TAnonymousQuery>(query, anonymous, flags),
                query);

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => With(
                static (outer, anonymous, flags, state) => new SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, state.continuation.Apply(anonymous, state.state), flags),
                (continuation, state));

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery>[] continuations, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var multi = ToMulti();

            for (var i = 0; i < continuations.Length; i++)
            {
                multi = multi
                    .With(continuations[i], state);
            }

            return multi;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti() => With(
            static (outer, anonymous, flags, _) => new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, FastImmutableList<IGremlinQueryBase>.Empty, flags),
            0);

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, TNewQuery> builderTransformation, TState state) => With(
            static (outer, _, _, state) => state.builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), state.state),
            (builderTransformation, state));

        private TResult With<TState, TResult>(Func<TOuterQuery, TAnonymousQuery, ContinuationFlags, TState, TResult> continuation, TState state) => (_outer is { } outer && _anonymous is { } anonymous)
            ? continuation(outer, anonymous, _flags, state)
            : throw new InvalidOperationException();

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _) => outer,
            0);
    }
}
