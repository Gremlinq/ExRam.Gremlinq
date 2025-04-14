using System.Buffers;

namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery _outer;
        private readonly ContinuationFlags _flags;
        private readonly Span<IGremlinQueryBase> _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, Span<IGremlinQueryBase> continuations, ContinuationFlags flags)
        {
            _outer = outer;
            _flags = flags;
            _continuations = continuations;
        }

        public TResult Build<TResult>(FinalContinuationBuilderTransformation<TOuterQuery, TResult> builderTransformation) => Build(static (builder, continuations, state) => state(builder, continuations), builderTransformation);

        public TResult Build<TResult, TState>(FinalContinuationBuilderTransformation<TOuterQuery, TResult, TState> builderTransformation, TState state)
        {
            var builder = new FinalContinuationBuilder<TOuterQuery>(_outer);

            using (var owner = MemoryPool<Traversal>.Shared.Rent(_continuations.Length))
            {
                var traversalsSpan = owner.Memory.Span;

                if (_continuations.Length > 0)
                {
                    for (var i = 0; i < _continuations.Length; i++)
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
                    traversalsSpan[.._continuations.Length],
                    state);
            }
        }
    }
}
