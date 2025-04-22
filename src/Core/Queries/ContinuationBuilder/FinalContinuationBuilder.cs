using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private static readonly Traversal IdentityTraversal = IdentityStep.Instance;

        private readonly Traversal _steps;
        private readonly TOuterQuery _outer;
        private readonly IImmutableDictionary<object, object?> _metadata;
        private readonly IImmutableDictionary<StepLabel, LabelProjections> _labelProjections;

        private FinalContinuationBuilder(TOuterQuery outerQuery, Traversal steps, IImmutableDictionary<StepLabel, LabelProjections> labelProjections, IImmutableDictionary<object, object?> metadata)
        {
            _steps = steps;
            _outer = outerQuery;
            _metadata = metadata;
            _labelProjections = labelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep(Step step) => new (_outer, _steps.Push(step), _labelProjections, _metadata);

        public FinalContinuationBuilder<TOuterQuery> AddSteps(ReadOnlySpan<Step> steps) => new(_outer, _steps.Push(steps), _labelProjections, _metadata);

        public FinalContinuationBuilder<TOuterQuery> WithSteps(Func<Traversal, Traversal> traversalTransformation) => new(_outer, traversalTransformation(_steps), _labelProjections, _metadata);

        public FinalContinuationBuilder<TOuterQuery> WithSteps<TState>(Func<Traversal, TState, Traversal> traversalTransformation, TState state) => new (_outer, traversalTransformation(_steps, state), _labelProjections, _metadata);

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => new(_outer, _steps.WithProjection(projectionTransformation(_steps.Projection, state)), _labelProjections, _metadata);

        public FinalContinuationBuilder<TOuterQuery> WithNewLabelProjections<TState>(Func<IImmutableDictionary<StepLabel, LabelProjections>, TState, IImmutableDictionary<StepLabel, LabelProjections>> labelProjectionsTransformation, TState state) => new(_outer, _steps, labelProjectionsTransformation(_labelProjections, state), _metadata);

        public FinalContinuationBuilder<TOuterQuery> WithMetadata(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) => new(_outer, _steps, _labelProjections, metadataTransformation(_metadata));

        public TOuterQuery Build() => BuildAs<TOuterQuery>();

        public GremlinQuery<T1, T2, T3, T4> BuildAuto<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => BuildAs<GremlinQuery<T1, T2, T3, T4>>();

        public GremlinQuery<T1, T2, T3, IGremlinQueryBase> BuildAuto<T1, T2, T3>() => BuildAs<GremlinQuery<T1, T2, T3, IGremlinQueryBase>>();

        public GremlinQuery<T1, T2, object, IGremlinQueryBase> BuildAuto<T1, T2>() => BuildAs<GremlinQuery<T1, T2, object, IGremlinQueryBase>>();

        public GremlinQuery<T1, object, object, IGremlinQueryBase> BuildAuto<T1>() => BuildAs<GremlinQuery<T1, object, object, IGremlinQueryBase>>();

        public GremlinQuery<object, object, object, IGremlinQueryBase> BuildAuto() => BuildAs<GremlinQuery<object, object, object, IGremlinQueryBase>>();

        public TNewTargetQuery BuildAs<TNewTargetQuery>() where TNewTargetQuery : IStartGremlinQuery => GremlinQueryFactory.CloneAs<TNewTargetQuery>(_outer.Environment, _steps, _labelProjections, _metadata);

        public FinalContinuationBuilder<TOuterQuery> Apply<TAnonymousQuery, TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous, ContinuationFlags flags, out Traversal traversal)
            where TProjectedQuery : IGremlinQueryBase => Apply(static (anonymous, continuation) => continuation(anonymous), anonymous, flags, out traversal, continuation);

        public FinalContinuationBuilder<TOuterQuery> Apply<TAnonymousQuery, TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TAnonymousQuery anonymous, ContinuationFlags flags, out Traversal traversal, TState state)
            where TProjectedQuery : IGremlinQueryBase
        {
            var ret = this;
            var continuedQuery = continuation(anonymous, state);
            var admin = continuedQuery.AsAdmin();

            continuedQuery = admin.Steps.Count == 0
                ? admin.ConfigureSteps<TProjectedQuery>(static traversal => IdentityTraversal.WithProjection(traversal.Projection))
                : continuedQuery;

            if (continuedQuery is GremlinQueryBase queryBase)
            {
                ret = ret.WithNewLabelProjections(
                    static (existingProjections, additionalProjections) => existingProjections.MergeSideEffectLabelProjections(additionalProjections),
                    queryBase.LabelProjections);
            }

            traversal = continuedQuery
                .ToTraversal()
                .Rewrite(flags);

            return ret;
        }

        public GremlinQueryBase OuterQuery => _outer;

        public static FinalContinuationBuilder<TOuterQuery> Create(TOuterQuery outerQuery) => new(outerQuery, outerQuery.Steps, outerQuery.LabelProjections, outerQuery.Metadata);
    }
}
