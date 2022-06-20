using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly Traversal? _steps;
        private readonly QueryFlags? _flags;
        private readonly TOuterQuery? _outer;
        private readonly Projection? _projection;
        private readonly IImmutableDictionary<StepLabel, Projection>? _stepLabelProjections;
        private readonly IImmutableDictionary<StepLabel, Projection>? _sideEffectLabelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.Projection, outerQuery.StepLabelProjections, outerQuery.SideEffectLabelProjections, outerQuery.Flags)
        {

        }

        public FinalContinuationBuilder(TOuterQuery outerQuery, Traversal steps, Projection projection, IImmutableDictionary<StepLabel, Projection> stepLabelProjections, IImmutableDictionary<StepLabel, Projection> sideEffectLabelProjections, QueryFlags flags)
        {
            _steps = steps;
            _flags = flags;
            _outer = outerQuery;
            _projection = projection;
            _stepLabelProjections = stepLabelProjections;
            _sideEffectLabelProjections = sideEffectLabelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep<TStep>(TStep step)
             where TStep : Step => With(
                static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, tuple) => outer.Flags.HasFlag(QueryFlags.IsMuted)
                    ? tuple.@this
                    : new(outer, outer.Environment.AddStepHandler.AddStep(steps, tuple.step, outer.Environment), projection, stepLabelProjections, sideEffectLabelProjections, flags),
                (@this: this, step));

        public FinalContinuationBuilder<TOuterQuery> WithSteps(Traversal newSteps) => With(
            static (outer, _, projection, stepLabelProjections, sideEffectLabelProjections, flags, newSteps) => new FinalContinuationBuilder<TOuterQuery>(outer, newSteps, projection, stepLabelProjections, sideEffectLabelProjections, flags),
            newSteps);

        public FinalContinuationBuilder<TOuterQuery> WithSteps<TState>(Func<Traversal, TState, Traversal> stepStackTransformation, TState state) => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, tuple.stepStackTransformation(steps, tuple.state), projection, stepLabelProjections, sideEffectLabelProjections, flags),
            (stepStackTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, tuple.projectionTransformation(projection ?? Projection.Empty, tuple.state), stepLabelProjections, sideEffectLabelProjections, flags),
            (projectionTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewStepLabelProjection<TState>(Func<IImmutableDictionary<StepLabel, Projection>, TState, IImmutableDictionary<StepLabel, Projection>> stepLabelProjectionsTransformation, TState state) => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, projection, tuple.stepLabelProjectionsTransformation(stepLabelProjections, tuple.state), sideEffectLabelProjections, flags),
            (stepLabelProjectionsTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewSideEffectLabelProjection<TState>(Func<IImmutableDictionary<StepLabel, Projection>, TState, IImmutableDictionary<StepLabel, Projection>> sideEffectLabelProjectionsTransformation, TState state) => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, projection, stepLabelProjections, tuple.sideEffectLabelProjectionsTransformation(sideEffectLabelProjections, tuple.state), flags),
            (sideEffectLabelProjectionsTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithFlags(Func<QueryFlags, QueryFlags> flagsProjection) => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, flagsProjection) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flagsProjection(flags)),
            flagsProjection);

        public FinalContinuationBuilder<TOuterQuery> WithFlags(QueryFlags newFlags) => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, _, newFlags) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, newFlags),
            newFlags);

        public TResult With<TState, TResult>(Func<TOuterQuery, Traversal, Projection, IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>, QueryFlags, TState, TResult> continuation, TState state)
        {
            return (_outer is { } outer && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections && _sideEffectLabelProjections is { } sideEffectLabelProjections && _flags is { } flags)
                ? continuation(outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, state)
                : throw new InvalidOperationException();
        }

        public TOuterQuery Build() => Build<TOuterQuery>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AutoBuild<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, object> AutoBuild<TElement, TOutVertex, TInVertex, TScalar, TMeta>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, object>>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, object, object> AutoBuild<TElement, TOutVertex, TInVertex, TScalar>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, object, object>>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, object, object, object> AutoBuild<TElement, TOutVertex, TInVertex>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, object, object, object>>();

        public GremlinQuery<TElement, TOutVertex, object, object, object, object> AutoBuild<TElement, TOutVertex>() => Build<GremlinQuery<TElement, TOutVertex, object, object, object, object>>();

        public GremlinQuery<TElement, object, object, object, object, object> AutoBuild<TElement>() => Build<GremlinQuery<TElement, object, object, object, object, object>>();

        public GremlinQuery<object, object, object, object, object, object> AutoBuild() => Build<GremlinQuery<object, object, object, object, object, object>>();

        public TNewTargetQuery Build<TNewTargetQuery>() where TNewTargetQuery : IGremlinQueryBase => With(
            static (outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, flags, _) => outer.CloneAs<TNewTargetQuery>(
                steps,
                projection,
                stepLabelProjections,
                sideEffectLabelProjections,
                flags),
            0);

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _, _, _, _) => outer,
            0);
    }
}
