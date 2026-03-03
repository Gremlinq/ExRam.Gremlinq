using System.Collections.Immutable;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder
    {
        private static readonly Traversal IdentityTraversal = IdentityStep.Instance;

        private readonly Traversal _steps;
        private readonly GremlinQueryBase _outer;
        private readonly IImmutableDictionary<object, object?> _metadata;
        private readonly IImmutableDictionary<StepLabel, LabelProjections> _labelProjections;

        private FinalContinuationBuilder(GremlinQueryBase outerQuery, Traversal steps, IImmutableDictionary<StepLabel, LabelProjections> labelProjections, IImmutableDictionary<object, object?> metadata)
        {
            _steps = steps;
            _outer = outerQuery;
            _metadata = metadata;
            _labelProjections = labelProjections;
        }

        public FinalContinuationBuilder AddStep(Step step) => new (_outer, _steps.Push(step), _labelProjections, _metadata);

        public FinalContinuationBuilder AddSteps(ReadOnlySpan<Step> steps) => new(_outer, _steps.Push(steps), _labelProjections, _metadata);

        public FinalContinuationBuilder WithSteps(Func<Traversal, Traversal> traversalTransformation) => new(_outer, traversalTransformation(_steps), _labelProjections, _metadata);

        public FinalContinuationBuilder WithSteps<TState>(Func<Traversal, TState, Traversal> traversalTransformation, TState state) => new (_outer, traversalTransformation(_steps, state), _labelProjections, _metadata);

        public FinalContinuationBuilder WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => new(_outer, _steps.WithProjection(projectionTransformation(_steps.Projection, state)), _labelProjections, _metadata);

        public FinalContinuationBuilder WithNewLabelProjections<TState>(Func<IImmutableDictionary<StepLabel, LabelProjections>, TState, IImmutableDictionary<StepLabel, LabelProjections>> labelProjectionsTransformation, TState state) => new(_outer, _steps, labelProjectionsTransformation(_labelProjections, state), _metadata);

        public FinalContinuationBuilder WithMetadata(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) => new(_outer, _steps, _labelProjections, metadataTransformation(_metadata));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public GremlinQuery<T1, T2, T3, T4> BuildAuto<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => new (_outer.Environment, _steps, _labelProjections, _metadata);

        public GremlinQuery<T1, T2, T3, IGremlinQueryBase> BuildAuto<T1, T2, T3>() => BuildAuto<T1, T2, T3, IGremlinQueryBase>();

        public GremlinQuery<T1, T2, object, IGremlinQueryBase> BuildAuto<T1, T2>() => BuildAuto<T1, T2, object, IGremlinQueryBase>();

        public GremlinQuery<T1, object, object, IGremlinQueryBase> BuildAuto<T1>() => BuildAuto<T1, object, object, IGremlinQueryBase>();

        public GremlinQuery<object, object, object, IGremlinQueryBase> BuildAuto() => BuildAuto<object, object, object, IGremlinQueryBase>();

        public TNewTargetQuery BuildAs<TNewTargetQuery>() where TNewTargetQuery : IStartGremlinQuery => GremlinQueryFactory.Create<TNewTargetQuery>(_outer.Environment, _steps, _labelProjections, _metadata);

        public FinalContinuationBuilder Apply<TAnonymousQuery, TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous, ContinuationFlags flags, out Traversal traversal)
            where TProjectedQuery : IGremlinQueryBase => Apply(static (anonymous, continuation) => continuation(anonymous), anonymous, flags, out traversal, continuation);

        public FinalContinuationBuilder Apply<TAnonymousQuery, TProjectedQuery, TState>(Func<TAnonymousQuery, TState, TProjectedQuery> continuation, TAnonymousQuery anonymous, ContinuationFlags flags, out Traversal traversal, TState state)
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

        public static FinalContinuationBuilder Create(GremlinQueryBase outerQuery) => new(outerQuery, outerQuery.Steps, outerQuery.LabelProjections, outerQuery.Metadata);
    }
}
