using System;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IGremlinQueryBase? _continuation;

        public SingleContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IGremlinQueryBase continuation)
        {
            _outer = outer;
            _anonymous = anonymous;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With(Func<TAnonymousQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _anonymous is { } anonymous && _continuation is { } existingContinuation
                ? new (outer, anonymous, ImmutableList.Create(existingContinuation, continuation.Apply(anonymous)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, TNewQuery> builderTransformation, TState state)
        {
            if (_outer is { } outer && _continuation is { } continuation)
            {
                var builder = new FinalContinuationBuilder<TOuterQuery>(outer);

                if (continuation is GremlinQueryBase queryBase)
                    builder = builder.WithNewSideEffectLabelProjection(_ => _.SetItems(queryBase.SideEffectLabelProjections));

                return builderTransformation(
                    builder,
                    continuation.ToTraversal(),
                    state);
            }

            throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }
}
