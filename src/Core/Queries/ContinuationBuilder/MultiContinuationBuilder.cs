using System.Buffers;

using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private static readonly Traversal IdentityTraversal = IdentityStep.Instance;

        private readonly TOuterQuery _outer;
        private readonly ContinuationFlags _flags;
        private readonly Span<IGremlinQueryBase> _continuations;

        private MultiContinuationBuilder(TOuterQuery outer, Span<IGremlinQueryBase> continuations, ContinuationFlags flags)
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


        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery, TState>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TState, TProjectedQuery> continuation, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuationList = new IGremlinQueryBase[] { Apply(continuation, anonymous, state) };

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, continuationList, flags);
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TProjectedQuery> continuation, ContinuationFlags flags)
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuationList = new IGremlinQueryBase[] { Apply(continuation, anonymous) };

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, continuationList, flags);
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery, TState>(TOuterQuery outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TState, TProjectedQuery>> continuations, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuationList = new IGremlinQueryBase[continuations.Length];

            for (var i = 0; i < continuations.Length; i++)
            {
                continuationList[i] = Apply(continuations[i], anonymous, state);
            }

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, continuationList, flags);
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery>(TOuterQuery outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags)
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuationList = new IGremlinQueryBase[continuations.Length];

            for (var i = 0; i < continuations.Length; i++)
            {
                continuationList[i] = Apply(continuations[i], anonymous);
            }

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(outer, continuationList, flags);
        }



        private static TProjectedQuery Apply<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous)
            where TProjectedQuery : IGremlinQueryBase => Apply(static (anonymous, continuation) => continuation(anonymous), anonymous, continuation);

        private static TProjectedQuery Apply<TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TAnonymousQuery anonymous, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuedQuery = continuation(anonymous, state);
            var admin = continuedQuery.AsAdmin();

            return admin.Steps.Count == 0
                ? admin.ConfigureSteps<TProjectedQuery>(static traversal => IdentityTraversal.WithProjection(traversal.Projection))
                : continuedQuery;
        }
    }
}
