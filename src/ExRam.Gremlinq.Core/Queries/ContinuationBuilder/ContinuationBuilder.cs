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

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TState state)
            where TProjectedQuery : IGremlinQueryBase =>
                _outer is { } outer && _anonymous is { } anonymous
                    ? new(outer, anonymous, continuation.Apply(anonymous, state))
                    : throw new InvalidOperationException();

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
