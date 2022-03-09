using System;
using System.Collections.Immutable;
using System.Linq;

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

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<Traversal>, TState, TNewQuery> builderTransformation, TState state)
        {
            if (_outer is { } outer && _continuations is { } continuations)
            {
                if (continuations.Count > 0)
                {
                    outer = outer.CloneAs<TOuterQuery>(
                        maybeSideEffectLabelProjectionsTransformation: projections =>
                        {
                            foreach (var continuation in continuations)
                            {
                                if (continuation is GremlinQueryBase queryBase)
                                    projections = projections.SetItems(queryBase.SideEffectLabelProjections);
                            }

                            return projections;
                        });
                }

                return builderTransformation(
                    new FinalContinuationBuilder<TOuterQuery>(outer),
                    continuations
                        .Select(x => x.ToTraversal())
                        .ToImmutableList(),
                    state);
            }

            throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }
}
