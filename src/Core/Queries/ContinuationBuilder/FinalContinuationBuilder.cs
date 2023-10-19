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
        private readonly IImmutableDictionary<StepLabel, LabelProjections>? _labelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.LabelProjections, outerQuery.Flags)
        {

        }

        public FinalContinuationBuilder(TOuterQuery outerQuery, Traversal steps, IImmutableDictionary<StepLabel, LabelProjections> labelProjections, QueryFlags flags)
        {
            _steps = steps;
            _flags = flags;
            _outer = outerQuery;
            _labelProjections = labelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep<TStep>(TStep step)
             where TStep : Step => With(
                static (outer, steps, labelProjections, flags, tuple) => outer.Flags.HasFlag(QueryFlags.IsMuted)
                    ? tuple.@this
                    : new(outer, steps.Push(tuple.step), labelProjections, flags),
                (@this: this, step));

        public FinalContinuationBuilder<TOuterQuery> WithSteps(Traversal newSteps) => With(
            static (outer, _, labelProjections, flags, newSteps) => new FinalContinuationBuilder<TOuterQuery>(outer, newSteps, labelProjections, flags),
            newSteps);

        public FinalContinuationBuilder<TOuterQuery> WithSteps<TState>(Func<Traversal, TState, Traversal> traversalTransformation, TState state) => With(
            static (outer, steps, labelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, tuple.traversalTransformation(steps, tuple.state), labelProjections, flags),
            (traversalTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection<TState>(Func<Projection, TState, Projection> projectionTransformation, TState state) => With(
            static (outer, steps, labelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps.WithProjection(tuple.projectionTransformation(steps.Projection, tuple.state)), labelProjections, flags),
            (projectionTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithNewLabelProjections<TState>(Func<IImmutableDictionary<StepLabel, LabelProjections>, TState, IImmutableDictionary<StepLabel, LabelProjections>> labelProjectionsTransformation, TState state) => With(
            static (outer, steps, labelProjections, flags, tuple) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, tuple.labelProjectionsTransformation(labelProjections, tuple.state), flags),
            (labelProjectionsTransformation, state));

        public FinalContinuationBuilder<TOuterQuery> WithFlags(Func<QueryFlags, QueryFlags> flagsProjection) => With(
            static (outer, steps, labelProjections, flags, flagsProjection) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, labelProjections, flagsProjection(flags)),
            flagsProjection);

        public FinalContinuationBuilder<TOuterQuery> WithFlags(QueryFlags newFlags) => With(
            static (outer, steps, labelProjections, _, newFlags) => new FinalContinuationBuilder<TOuterQuery>(outer, steps, labelProjections, newFlags),
            newFlags);

        public TOuterQuery Build() => Build<TOuterQuery>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AutoBuild<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, object> AutoBuild<TElement, TOutVertex, TInVertex, TScalar, TMeta>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, object>>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, object, object> AutoBuild<TElement, TOutVertex, TInVertex, TScalar>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, object, object>>();

        public GremlinQuery<TElement, TOutVertex, TInVertex, object, object, object> AutoBuild<TElement, TOutVertex, TInVertex>() => Build<GremlinQuery<TElement, TOutVertex, TInVertex, object, object, object>>();

        public GremlinQuery<TElement, TOutVertex, object, object, object, object> AutoBuild<TElement, TOutVertex>() => Build<GremlinQuery<TElement, TOutVertex, object, object, object, object>>();

        public GremlinQuery<TElement, object, object, object, object, object> AutoBuild<TElement>() => Build<GremlinQuery<TElement, object, object, object, object, object>>();

        public GremlinQuery<object, object, object, object, object, object> AutoBuild() => Build<GremlinQuery<object, object, object, object, object, object>>();

        public TNewTargetQuery Build<TNewTargetQuery>() where TNewTargetQuery : IStartGremlinQuery => With(
            static (outer, steps, labelProjections, flags, _) => outer.CloneAs<TNewTargetQuery>(
                steps,
                labelProjections,
                flags),
            0);

        private TResult With<TState, TResult>(Func<TOuterQuery, Traversal, IImmutableDictionary<StepLabel, LabelProjections>, QueryFlags, TState, TResult> continuation, TState state)
        {
            return (_outer is { } outer && _steps is { } steps && _labelProjections is { } labelProjections && _flags is { } flags)
                ? continuation(outer, steps, labelProjections, flags, state)
                : throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => With(
            static (outer, _, _, _, _) => outer,
            0);
    }
}
