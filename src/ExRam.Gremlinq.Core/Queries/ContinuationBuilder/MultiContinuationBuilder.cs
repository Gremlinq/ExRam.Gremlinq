﻿using System.Buffers;
using System.Collections.Immutable;

namespace ExRam.Gremlinq.Core
{
    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery, TState>(FinalContinuationBuilder<TOuterQuery> builder, Span<Traversal> traversals, TState state)
         where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal delegate TNewQuery FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery>(FinalContinuationBuilder<TOuterQuery> builder, Span<Traversal> traversals)
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase;

    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly ContinuationFlags _flags;
        private readonly TAnonymousQuery? _anonymous;
        private readonly FastImmutableList<IGremlinQueryBase> _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, FastImmutableList<IGremlinQueryBase> continuations, ContinuationFlags flags)
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
                static (outer, anonymous, continuations, flags, state) => new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, anonymous, continuations.Push(state.continuation.Apply(anonymous, state.state)), flags),
                (continuation, state));
        }

        public TNewQuery Build<TNewQuery, TState>(FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery, TState> builderTransformation, TState state)
            where TNewQuery : IGremlinQueryBase
        {
            return With(
                static (outer, anonymous, continuations, flags, state) =>
                {
                    var (builderTransformation, innerState) = state;

                    if (outer.Flags.HasFlag(QueryFlags.IsMuted))
                        return outer.CloneAs<TNewQuery>();

                    var builder = new FinalContinuationBuilder<TOuterQuery>(outer);

                    using (var owner = MemoryPool<Traversal>.Shared.Rent(continuations.Count))
                    {
                        var traversalSpan = owner
                            .Memory.Span[..continuations.Count];

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

                                traversalSpan[targetIndex++] = continuation
                                    .ToTraversal()
                                    .Rewrite(flags);
                            }
                        }

                        return builderTransformation(
                            builder,
                            traversalSpan,
                            innerState);
                    }
                },
                (builderTransformation, state));
        }

        private TResult With<TState, TResult>(Func<TOuterQuery, TAnonymousQuery, FastImmutableList<IGremlinQueryBase>, ContinuationFlags, TState, TResult> continuation, TState state) => _outer is { } outer && _anonymous is { } anonymous && _continuations is { } continuations
            ? continuation(outer, anonymous, continuations, _flags, state)
            : throw new InvalidOperationException();

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _, _) => outer,
            0);
    }
}
