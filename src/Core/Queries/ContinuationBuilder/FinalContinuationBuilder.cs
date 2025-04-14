using System.Collections.Immutable;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly Traversal _steps;
        private readonly TOuterQuery _outer;
        private readonly IImmutableDictionary<StepLabel, LabelProjections> _labelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.LabelProjections)
        {

        }

        private FinalContinuationBuilder(TOuterQuery outerQuery, Traversal steps, IImmutableDictionary<StepLabel, LabelProjections> labelProjections)
        {
            _steps = steps;
            _outer = outerQuery;
            _labelProjections = labelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep(Step step) => new (_outer, _steps.Push(step), _labelProjections);

        public FinalContinuationBuilder<TOuterQuery> WithSteps<TState>(Func<Traversal, TState, Traversal> traversalTransformation, TState state) => new (_outer, traversalTransformation(_steps, state), _labelProjections);

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => new(_outer, _steps.WithProjection(projectionTransformation(_steps.Projection, state)), _labelProjections);

        public FinalContinuationBuilder<TOuterQuery> WithNewLabelProjections<TState>(Func<IImmutableDictionary<StepLabel, LabelProjections>, TState, IImmutableDictionary<StepLabel, LabelProjections>> labelProjectionsTransformation, TState state) => new(_outer, _steps, labelProjectionsTransformation(_labelProjections, state));

        public TOuterQuery Build() => BuildAs<TOuterQuery>();

        public GremlinQuery<T1, T2, T3, T4> BuildAuto<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => BuildAs<GremlinQuery<T1, T2, T3, T4>>();

        public GremlinQuery<T1, T2, T3, IGremlinQueryBase> BuildAuto<T1, T2, T3>() => BuildAs<GremlinQuery<T1, T2, T3, IGremlinQueryBase>>();

        public GremlinQuery<T1, T2, object, IGremlinQueryBase> BuildAuto<T1, T2>() => BuildAs<GremlinQuery<T1, T2, object, IGremlinQueryBase>>();

        public GremlinQuery<T1, object, object, IGremlinQueryBase> BuildAuto<T1>() => BuildAs<GremlinQuery<T1, object, object, IGremlinQueryBase>>();

        public GremlinQuery<object, object, object, IGremlinQueryBase> BuildAuto() => BuildAs<GremlinQuery<object, object, object, IGremlinQueryBase>>();

        public TNewTargetQuery BuildAs<TNewTargetQuery>() where TNewTargetQuery : IStartGremlinQuery => _outer.CloneAs<TNewTargetQuery>(_steps, _labelProjections);

        public TOuterQuery OuterQuery => _outer;
    }
}
