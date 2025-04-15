namespace ExRam.Gremlinq.Core
{
    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery _outer;
        private readonly ContinuationFlags _flags;
        private readonly IGremlinQueryBase _continuation;

        public SingleContinuationBuilder(TOuterQuery outer, IGremlinQueryBase continuation, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _continuation = continuation;
        }

        public TOuterQuery Build<TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, FinalContinuationBuilder<TOuterQuery>> builderTransformation, TState state)
            => Build(static (builder, traversal, tuple) => tuple.builderTransformation(builder, traversal, tuple.state).Build(), (builderTransformation, state));

        public TOuterQuery Build(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, FinalContinuationBuilder<TOuterQuery>> builderTransformation)
            => Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);

        public TResult Build<TResult>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TResult> builderTransformation)
            => Build(static (builder, traversal, builderTransformation) => builderTransformation(builder, traversal), builderTransformation);

        public TResult Build<TResult, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, TResult> builderTransformation, TState state)
            => MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
                .Create(_outer, [_continuation], _flags)
                .Build((builder, traversals, innerState) => builderTransformation(builder, traversals[0], innerState), state);
    }
}
