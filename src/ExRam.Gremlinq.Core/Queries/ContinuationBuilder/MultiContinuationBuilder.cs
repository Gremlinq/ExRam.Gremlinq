using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IImmutableList<IGremlinQueryBase>? _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IImmutableList<IGremlinQueryBase> continuations, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _anonymous = anonymous;
            _continuations = continuations;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            return With(
                static (outer, anonymous, continuations, flags, state) => new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, continuations.Add(state.continuation.Apply(anonymous, state.state)), flags),
                (continuation, state));
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal[], TState, TNewQuery> builderTransformation, TState state)
        {
            return With(
                static (outer, anonymous, continuations, flags, state) =>
                {
                    var (builderTransformation, innerState) = state;

                    if (outer.Flags.HasFlag(QueryFlags.IsMuted))
                        return outer.CloneAs<TNewQuery>();

                    var builder = new FinalContinuationBuilder<TOuterQuery>(outer);
                    var traversalArray = continuations.Count > 0
                        ? new Traversal[continuations.Count]
                        : Array.Empty<Traversal>();

                    if (continuations.Count > 0)
                    {
                        var targetIndex = 0;

                        foreach (var continuation in continuations)
                        {
                            if (continuation is GremlinQueryBase queryBase)
                            {
                                builder = builder.WithNewLabelProjections(
                                    static (existingProjections, additionalProjections) => existingProjections.MergeSideEffectLabelProjections(additionalProjections),
                                    queryBase.LabelProjections);
                            }

                            traversalArray[targetIndex++] = continuation.ToTraversal();
                        }
                    }

                    return builderTransformation(
                        builder,
                        traversalArray,
                        innerState);
                },
                (builderTransformation, state));
        }

        private TResult With<TState, TResult>(Func<TOuterQuery, TAnonymousQuery, IImmutableList<IGremlinQueryBase>, ContinuationFlags, TState, TResult> continuation, TState state) => _outer is { } outer && _anonymous is { } anonymous && _continuations is { } continuations
            ? continuation(outer, anonymous, continuations, _flags, state)
            : throw new InvalidOperationException();

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _, _) => outer,
            0);
    }
}
