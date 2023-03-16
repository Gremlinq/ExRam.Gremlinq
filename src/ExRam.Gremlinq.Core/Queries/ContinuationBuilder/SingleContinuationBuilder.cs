namespace ExRam.Gremlinq.Core
{
    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IGremlinQueryBase? _continuation;

        public SingleContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IGremlinQueryBase continuation, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TState>(Func<TAnonymousQuery, TState, IGremlinQueryBase> continuation, TState state)
        {
            return With(
                static (outer, anonymous, existingContinuation, flags, state) => new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, FastImmutableList<IGremlinQueryBase>.Empty.Push(existingContinuation).Push(state.continuation.Apply(anonymous, state.state)), flags),
                (continuation, state));
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, TNewQuery> builderTransformation, TState state)
        {
            return With(
                static (outer, _, continuation, flags, state) =>
                {
                    var (builderTransformation, innerState) = state;

                    if (outer.Flags.HasFlag(QueryFlags.IsMuted))
                        return outer.CloneAs<TNewQuery>();

                    var builder = new FinalContinuationBuilder<TOuterQuery>(outer);

                    if (continuation is GremlinQueryBase queryBase)
                    {
                        builder = builder.WithNewLabelProjections(
                            static (existingProjections, additionalProjections) => existingProjections.MergeSideEffectLabelProjections(additionalProjections),
                            queryBase.LabelProjections);
                    }

                    return builderTransformation(
                        builder,
                        continuation
                            .ToTraversal()
                            .Rewrite(flags),
                        innerState);
                },
                (builderTransformation, state));
        }

        private TResult With<TState, TResult>(Func<TOuterQuery, TAnonymousQuery, IGremlinQueryBase, ContinuationFlags, TState, TResult> continuation, TState state) => _outer is { } outer && _anonymous is { } anonymous && _continuation is { } existingContinuation
            ? continuation(outer, anonymous, existingContinuation, _flags, state)
            : throw new InvalidOperationException();

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _, _) => outer,
            0);
    }
}
