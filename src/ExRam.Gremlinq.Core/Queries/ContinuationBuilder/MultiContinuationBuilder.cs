using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IImmutableList<IGremlinQueryBase>? _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IImmutableList<IGremlinQueryBase> continuations)
        {
            _outer = outer;
            _anonymous = anonymous;
            _continuations = continuations;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With(Func<TAnonymousQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _anonymous is { } anonymous && _continuations is { } continuations
                ? new(outer, anonymous, continuations.Add(continuation.Apply(anonymous)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal[], TState, TNewQuery> builderTransformation, TState state)
        {
            if (_outer is { } outer && _continuations is { } continuations)
            {
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
                            builder = builder.WithNewSideEffectLabelProjection(_ => _.SetItems(queryBase.SideEffectLabelProjections));

                        traversalArray[targetIndex++] = continuation.ToTraversal();
                    }
                }

                return builderTransformation(
                    builder,
                    traversalArray,
                    state);
            }

            throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }
}
