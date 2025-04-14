using System.Buffers;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery _outer;
        private readonly ContinuationFlags _flags;
        private readonly FastImmutableList<IGremlinQueryBase> _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, FastImmutableList<IGremlinQueryBase> continuations, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _continuations = continuations;
        }

        public TNewQuery Build<TNewQuery>(FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery> builderTransformation)
            where TNewQuery : IGremlinQueryBase => Build(static (builder, continuations, state) => state(builder, continuations), builderTransformation);

        public TNewQuery Build<TNewQuery, TState>(FinalContinuationBuilderTransformation<TOuterQuery, TNewQuery, TState> builderTransformation, TState state) where TNewQuery : IGremlinQueryBase
        {
            var builder = new FinalContinuationBuilder<TOuterQuery>(_outer);

            using (var owner = MemoryPool<Traversal>.Shared.Rent(_continuations.Count))
            {
                var traversalsSpan = owner.Memory.Span;

                if (_continuations.Count > 0)
                {
                    for (var i = 0; i < _continuations.Count; i++)
                    {
                        var continuation = _continuations[i];

                        if (continuation is GremlinQueryBase queryBase)
                        {
                            builder = builder.WithNewLabelProjections(
                                static (existingProjections, additionalProjections) => existingProjections.MergeSideEffectLabelProjections(additionalProjections),
                                queryBase.LabelProjections);
                        }

                        traversalsSpan[i] = continuation
                            .ToTraversal()
                            .Rewrite(_flags);
                    }
                }

                return builderTransformation(
                    builder,
                    traversalsSpan[.._continuations.Count],
                    state);
            }
        }
    }
}
