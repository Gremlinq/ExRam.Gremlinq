using static ExRam.Gremlinq.Core.ExceptionHelper;

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
            where TNewOuterQuery : GremlinQueryBase, IGremlinQueryBase => _anonymous is { } anonymous
                ? new ContinuationBuilder<TNewOuterQuery, TAnonymousQuery>(query, anonymous, _flags)
                : throw UninitializedStruct();

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase => _outer is { } outer && _anonymous is { } anonymous
                ? new SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, continuation.Apply(anonymous, state), _flags)
                : throw UninitializedStruct();

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti() => _outer is { } outer && _anonymous is { } anonymous
            ? new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, FastImmutableList<IGremlinQueryBase>.Empty, _flags)
            : throw UninitializedStruct();

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery, TOuterQuery>, TState, FinalContinuationBuilder<TOuterQuery, TNewQuery>> builderTransformation, TState state)
            where TNewQuery : IStartGremlinQuery => Build(static (builder, tuple) => tuple.builderTransformation(builder, tuple.state).Build(), (builderTransformation, state));

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery, TOuterQuery>, TState, TNewQuery> builderTransformation, TState state)
            where TNewQuery : IStartGremlinQuery => _outer is { } outer
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery, TOuterQuery>(outer), state)
                : throw UninitializedStruct();
    }
}
