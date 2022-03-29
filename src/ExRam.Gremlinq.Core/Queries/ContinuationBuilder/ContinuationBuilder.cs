using System;
using System.Collections.Immutable;
using System.Linq;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct ContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;

        public ContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous)
        {
            _outer = outer;
            _anonymous = anonymous;
        }

        public ContinuationBuilder<TNewOuterQuery, TAnonymousQuery> WithOuter<TNewOuterQuery>(TNewOuterQuery query)
            where TNewOuterQuery : GremlinQueryBase, IGremlinQueryBase =>
                _anonymous is { } anonymous
                    ? new(query, anonymous)
                    : throw new InvalidOperationException();

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase =>
                _outer is { } outer && _anonymous is { } anonymous
                    ? new(outer, anonymous, continuation.Apply(anonymous))
                    : throw new InvalidOperationException();

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery>[] continuations)
            where TProjectedQuery : IGremlinQueryBase =>
                _outer is { } outer && _anonymous is { } anonymous
                    ? new(
                        outer,
                        anonymous,
                        continuations
                            .Select(contintuation => (IGremlinQueryBase)contintuation.Apply(anonymous))
                            .ToImmutableList())
                    : throw new InvalidOperationException();

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti() =>
            _outer is { } outer && _anonymous is { } anonymous
                ? new(
                    outer,
                    anonymous,
                    ImmutableList<IGremlinQueryBase>.Empty)
                : throw new InvalidOperationException();

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, TNewQuery> builderTransformation, TState state) =>
            _outer is { } outer
                ? (outer.Flags & QueryFlags.IsMuted) == QueryFlags.IsMuted
                    ? outer.CloneAs<TNewQuery>()
                    : builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), state)
                : throw new InvalidOperationException();

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }
}
