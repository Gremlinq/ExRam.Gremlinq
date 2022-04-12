using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly StepStack? _steps;
        private readonly TOuterQuery? _outer;
        private readonly Projection? _projection;
        private readonly QueryFlags _additionalFlags = QueryFlags.None;
        private readonly IImmutableDictionary<StepLabel, Projection>? _stepLabelProjections;
        private readonly IImmutableDictionary<StepLabel, Projection>? _sideEffectLabelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.Projection, outerQuery.StepLabelProjections, outerQuery.SideEffectLabelProjections, outerQuery.Flags)
        {

        }

        public FinalContinuationBuilder(TOuterQuery outerQuery, StepStack steps, Projection projection, IImmutableDictionary<StepLabel, Projection> stepLabelProjections, IImmutableDictionary<StepLabel, Projection> sideEffectLabelProjections, QueryFlags additionalFlags)
        {
            _steps = steps;
            _outer = outerQuery;
            _projection = projection;
            _additionalFlags = additionalFlags;
            _stepLabelProjections = stepLabelProjections;
            _sideEffectLabelProjections = sideEffectLabelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep<TStep>(TStep step)
             where TStep : Step
        {
            return _outer is { } outer && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections && _sideEffectLabelProjections is { } sideEffectLabelProjections
                ? outer.Flags.HasFlag(QueryFlags.IsMuted)
                    ? this
                    : new(outer, outer.Environment.AddStepHandler.AddStep(steps, step, outer.Environment), projection, stepLabelProjections, sideEffectLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state)
        {
            return _outer is { } outer && _steps is { } steps && _stepLabelProjections is { } stepLabelProjections && _sideEffectLabelProjections is { } sideEffectLabelProjections
                ? new(outer, steps, projectionTransformation(_projection ?? Projection.Empty, state), stepLabelProjections, sideEffectLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }
        
        public FinalContinuationBuilder<TOuterQuery> WithNewStepLabelProjection<TState>(Func<IImmutableDictionary<StepLabel, Projection>, TState, IImmutableDictionary<StepLabel, Projection>> stepLabelProjectionsTransformation, TState state)
        {
            return _outer is { } outer && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections && _sideEffectLabelProjections is { } sideEffectLabelProjections
                ? new(outer, steps, projection, stepLabelProjectionsTransformation(stepLabelProjections, state), sideEffectLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewSideEffectLabelProjection(Func<IImmutableDictionary<StepLabel, Projection>, IImmutableDictionary<StepLabel, Projection>> sideEffectLabelProjectionsTransformation)
        {
            return _outer is { } outer && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections && _sideEffectLabelProjections is { } sideEffectLabelProjections
                ? new(outer, steps, projection, stepLabelProjections, sideEffectLabelProjectionsTransformation(sideEffectLabelProjections), _additionalFlags)
                : throw new InvalidOperationException();
        }
        
        public FinalContinuationBuilder<TOuterQuery> WithAdditionalFlags(QueryFlags additionalFlags)
        {
            return _outer is { } outer && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections && _sideEffectLabelProjections is { } sideEffectLabelProjections
                ? new(outer, steps, projection, stepLabelProjections, sideEffectLabelProjections, _additionalFlags | additionalFlags)
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

        public TNewTargetQuery Build<TNewTargetQuery>() where TNewTargetQuery : IGremlinQueryBase
        {
            return _outer is { } query
                ? query.CloneAs<TNewTargetQuery>(
                    maybeStepStackTransformation: _steps is { } newSteps ? _ => newSteps : null,
                    maybeProjectionTransformation: _projection is { } newProjection ? _ => newProjection : null,
                    maybeStepLabelProjectionsTransformation: _stepLabelProjections is { } newStepLabelProjections ? _ => newStepLabelProjections : null,
                    maybeSideEffectLabelProjectionsTransformation: _sideEffectLabelProjections  is { } sideEffectLabelProjections ? _ => sideEffectLabelProjections : null,
                    maybeQueryFlagsTransformation: (_additionalFlags is var additionalFlags && additionalFlags != QueryFlags.None) ? flags => flags | additionalFlags : null)
                : throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }
}
