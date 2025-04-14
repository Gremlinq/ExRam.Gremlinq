namespace ExRam.Gremlinq.Core
{
    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery _anonymous;
        private readonly IGremlinQueryBase _continuation;

        public SingleContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IGremlinQueryBase continuation, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TState>(Func<TAnonymousQuery, TState, IGremlinQueryBase> continuation, TState state) => new (_outer, _anonymous, FastImmutableList<IGremlinQueryBase>.Empty.Push(_continuation).Push(continuation.Apply(_anonymous, state)), _flags);

        public TOuterQuery Build<TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, FinalContinuationBuilder<TOuterQuery>> builderTransformation, TState state)
            => Build(static (builder, traversal, tuple) => tuple.builderTransformation(builder, traversal, tuple.state).Build(), (builderTransformation, state));

        public TOuterQuery Build(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, FinalContinuationBuilder<TOuterQuery>> builderTransformation)
            => Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);

        public TResult Build<TResult>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TResult> builderTransformation)
            => Build(static (builder, traversal, builderTransformation) => builderTransformation(builder, traversal), builderTransformation);

        public TResult Build<TResult, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, TResult> builderTransformation, TState state)
        {
            var builder = new FinalContinuationBuilder<TOuterQuery>(_outer);

            if (_continuation is GremlinQueryBase queryBase)
            {
                builder = builder.WithNewLabelProjections(
                    static (existingProjections, additionalProjections) => existingProjections.MergeSideEffectLabelProjections(additionalProjections),
                    queryBase.LabelProjections);
            }

            return builderTransformation(
                builder,
                _continuation
                    .ToTraversal()
                    .Rewrite(_flags),
                state);
        }
    }
}
