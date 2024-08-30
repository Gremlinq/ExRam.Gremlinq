using System.Collections.Immutable;
using System.Diagnostics;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;
using static ExRam.Gremlinq.Core.ExceptionHelper;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder<TOuterQuery, TTargetQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
        where TTargetQuery : IStartGremlinQuery
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

        public FinalContinuationBuilder<TOuterQuery, TTargetQuery> AddStep(Step step) => With(
            static (outer, steps, labelProjections, step) => new FinalContinuationBuilder<TOuterQuery, TTargetQuery>(outer, steps.Push(step), labelProjections),
            step);

        public FinalContinuationBuilder<TOuterQuery, TTargetQuery> WithSteps(Traversal newSteps) => With(
            static (outer, _, labelProjections, newSteps) => new FinalContinuationBuilder<TOuterQuery, TTargetQuery>(outer, newSteps, labelProjections),
            newSteps);

        public FinalContinuationBuilder<TOuterQuery, TTargetQuery> WithSteps<TState>(Func<Traversal, TState, Traversal> traversalTransformation, TState state) => With(
            static (outer, steps, labelProjections, tuple) => new FinalContinuationBuilder<TOuterQuery, TTargetQuery>(outer, tuple.traversalTransformation(steps, tuple.state), labelProjections),
            (traversalTransformation, state));

        public FinalContinuationBuilder<TOuterQuery, TTargetQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => With(
            static (outer, steps, labelProjections, tuple) => new FinalContinuationBuilder<TOuterQuery, TTargetQuery>(outer, steps.WithProjection(tuple.projectionTransformation(steps.Projection, tuple.state)), labelProjections),
            (projectionTransformation, state));

        public FinalContinuationBuilder<TOuterQuery, TTargetQuery> WithNewLabelProjections<TState>(Func<IImmutableDictionary<StepLabel, LabelProjections>, TState, IImmutableDictionary<StepLabel, LabelProjections>> labelProjectionsTransformation, TState state) => With(
            static (outer, steps, labelProjections, tuple) => new FinalContinuationBuilder<TOuterQuery, TTargetQuery>(outer, steps, tuple.labelProjectionsTransformation(labelProjections, tuple.state)),
            (labelProjectionsTransformation, state));

        public TTargetQuery Build() => With(
            static (outer, steps, labelProjections, _) => outer.CloneAs<TTargetQuery>(steps, labelProjections),
            0);

        public GremlinQuery<T1, T2, T3, T4> AutoBuild<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => As<GremlinQuery<T1, T2, T3, T4>>().Build();

        public GremlinQuery<T1, T2, T3, IGremlinQueryBase> AutoBuild<T1, T2, T3>() => As<GremlinQuery<T1, T2, T3, IGremlinQueryBase>>().Build();

        public GremlinQuery<T1, T2, object, IGremlinQueryBase> AutoBuild<T1, T2>() => As<GremlinQuery<T1, T2, object, IGremlinQueryBase>>().Build();

        public GremlinQuery<T1, object, object, IGremlinQueryBase> AutoBuild<T1>() => As<GremlinQuery<T1, object, object, IGremlinQueryBase>>().Build();

        public GremlinQuery<object, object, object, IGremlinQueryBase> AutoBuild() => As<GremlinQuery<object, object, object, IGremlinQueryBase>>().Build();

        public FinalContinuationBuilder<TOuterQuery, TNewTargetQuery> As<TNewTargetQuery>() where TNewTargetQuery : IStartGremlinQuery => With(
            static (outer, steps, labelProjections, _) => new FinalContinuationBuilder<TOuterQuery, TNewTargetQuery>(outer, steps, labelProjections),
            0);

        private TResult With<TState, TResult>(Func<TOuterQuery, Traversal, IImmutableDictionary<StepLabel, LabelProjections>, TState, TResult> continuation, TState state) => (_outer is { } outer && _steps is { } steps && _labelProjections is { } labelProjections)
            ? continuation(outer, steps, labelProjections, state)
            : throw UninitializedStruct();

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _) => outer,
            0);
    }
}
