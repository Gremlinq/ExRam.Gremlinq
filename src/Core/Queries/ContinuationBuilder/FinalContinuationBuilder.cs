using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly Traversal? _steps;
        private readonly TOuterQuery? _outer;
        private readonly IImmutableDictionary<StepLabel, LabelProjections>? _labelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.LabelProjections)
        {

        }

        public FinalContinuationBuilder(TOuterQuery outerQuery, Traversal steps, IImmutableDictionary<StepLabel, LabelProjections> labelProjections)
        {
            _steps = steps;
            _outer = outerQuery;
            _labelProjections = labelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep(Step step) => With(
            static (outer, steps, labelProjections, step) => new FinalContinuationBuilder<TOuterQuery>(outer, steps.Push(step), labelProjections),
            step);

        public FinalContinuationBuilder<TOuterQuery> WithSteps(Traversal newSteps) => With(
            static (outer, _, labelProjections, newSteps) => new FinalContinuationBuilder<TOuterQuery>(outer, newSteps, labelProjections),
            newSteps);

        public FinalContinuationBuilder<TOuterQuery> WithSteps<TState>(Func<Traversal, TState, Traversal> traversalTransformation, TState state) => With(
            static (outer, steps, labelProjections, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, tuple.traversalTransformation(steps, tuple.state), labelProjections),
            (traversalTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => With(
            static (outer, steps, labelProjections, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps.WithProjection(tuple.projectionTransformation(steps.Projection, tuple.state)), labelProjections),
            (projectionTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewLabelProjections<TState>(Func<IImmutableDictionary<StepLabel, LabelProjections>, TState, IImmutableDictionary<StepLabel, LabelProjections>> labelProjectionsTransformation, TState state) => With(
            static (outer, steps, labelProjections, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, tuple.labelProjectionsTransformation(labelProjections, tuple.state)),
            (labelProjectionsTransformation, state));

        public TOuterQuery Build() => Build<TOuterQuery>();

        public GremlinQuery<T1, T2, T3, T4> AutoBuild<T1, T2, T3, T4>() => Build<GremlinQuery<T1, T2, T3, T4>>();

        public GremlinQuery<T1, T2, T3, object> AutoBuild<T1, T2, T3>() => Build<GremlinQuery<T1, T2, T3, object>>();

        public GremlinQuery<T1, T2, object, object> AutoBuild<T1, T2>() => Build<GremlinQuery<T1, T2, object, object>>();

        public GremlinQuery<T1, object, object, object> AutoBuild<T1>() => Build<GremlinQuery<T1, object, object, object>>();

        public GremlinQuery<object, object, object, object> AutoBuild() => Build<GremlinQuery<object, object, object, object>>();

        public TNewTargetQuery Build<TNewTargetQuery>() where TNewTargetQuery : IStartGremlinQuery => With(
            static (outer, steps, labelProjections, _) => outer.CloneAs<TNewTargetQuery>(
                steps,
                labelProjections),
            0);

        private TResult With<TState, TResult>(Func<TOuterQuery, Traversal, IImmutableDictionary<StepLabel, LabelProjections>, TState, TResult> continuation, TState state) => (_outer is { } outer && _steps is { } steps && _labelProjections is { } labelProjections)
            ? continuation(outer, steps, labelProjections, state)
            : throw new InvalidOperationException();

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _) => outer,
            0);
    }
}
