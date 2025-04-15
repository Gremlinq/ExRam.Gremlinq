using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly ref struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private static readonly Traversal IdentityTraversal = IdentityStep.Instance;

        private readonly Span<Traversal> _continuations;
        private readonly FinalContinuationBuilder<TOuterQuery> _finalBuilder;

        public MultiContinuationBuilder(FinalContinuationBuilder<TOuterQuery> finalBuilder, Span<Traversal> continuations)
        {
            _finalBuilder = finalBuilder;
            _continuations = continuations;
        }

        public TResult Build<TResult>(FinalContinuationBuilderTransformation<TOuterQuery, TResult> builderTransformation) => Build(static (builder, continuations, state) => state(builder, continuations), builderTransformation);

        public TResult Build<TResult, TState>(FinalContinuationBuilderTransformation<TOuterQuery, TResult, TState> builderTransformation, TState state) => builderTransformation(
            _finalBuilder,
            _continuations,
            state);


        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery, TState>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TState, TProjectedQuery> continuation, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var traversals = new Traversal[1];
            var finalBuilder = new FinalContinuationBuilder<TOuterQuery>(outer);

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(
                Apply(finalBuilder, continuation, anonymous, flags, out traversals[0], state),
                traversals);
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery>(TOuterQuery outer, TAnonymousQuery anonymous, Func<TAnonymousQuery, TProjectedQuery> continuation, ContinuationFlags flags)
            where TProjectedQuery : IGremlinQueryBase
        {
            var traversals = new Traversal[1];
            var finalBuilder = new FinalContinuationBuilder<TOuterQuery>(outer);

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(
                Apply(finalBuilder, continuation, anonymous, flags, out traversals[0]),
                traversals);
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery>(TOuterQuery outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TProjectedQuery>> continuations, ContinuationFlags flags)
            where TProjectedQuery : IGremlinQueryBase
        {
            var traversals = new Traversal[continuations.Length];
            var finalBuilder = new FinalContinuationBuilder<TOuterQuery>(outer);

            for (var i = 0; i < continuations.Length; i++)
            {
                finalBuilder = Apply(finalBuilder, continuations[i], anonymous, flags, out traversals[i]);
            }

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(finalBuilder, traversals);
        }

        public static MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> Create<TProjectedQuery, TState>(TOuterQuery outer, TAnonymousQuery anonymous, ReadOnlySpan<Func<TAnonymousQuery, TState, TProjectedQuery>> continuations, ContinuationFlags flags, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var traversals = new Traversal[continuations.Length];
            var finalBuilder = new FinalContinuationBuilder<TOuterQuery>(outer);

            for (var i = 0; i < continuations.Length; i++)
            {
                finalBuilder = Apply(finalBuilder, continuations[i], anonymous, flags, out traversals[i], state);
            }

            return new MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>(finalBuilder, traversals);
        }

        private static FinalContinuationBuilder<TOuterQuery> Apply<TProjectedQuery>(FinalContinuationBuilder<TOuterQuery> finalBuilder, Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous, ContinuationFlags flags, out Traversal traversal)
            where TProjectedQuery : IGremlinQueryBase => Apply(finalBuilder, static (anonymous, continuation) => continuation(anonymous), anonymous, flags, out traversal, continuation);

        private static FinalContinuationBuilder<TOuterQuery> Apply<TProjectedQuery, TState>(FinalContinuationBuilder<TOuterQuery> finalBuilder, Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TAnonymousQuery anonymous, ContinuationFlags flags, out Traversal traversal, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuedQuery = continuation(anonymous, state);
            var admin = continuedQuery.AsAdmin();

            continuedQuery = admin.Steps.Count == 0
                ? admin.ConfigureSteps<TProjectedQuery>(static traversal => IdentityTraversal.WithProjection(traversal.Projection))
                : continuedQuery;

            if (continuedQuery is GremlinQueryBase queryBase)
            {
                finalBuilder = finalBuilder.WithNewLabelProjections(
                    static (existingProjections, additionalProjections) => existingProjections.MergeSideEffectLabelProjections(additionalProjections),
                    queryBase.LabelProjections);
            }

            traversal = continuedQuery
                .ToTraversal()
                .Rewrite(flags);

            return finalBuilder;
        }
    }
}
