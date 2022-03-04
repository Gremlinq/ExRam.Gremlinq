using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct ContinuationBuilder<TOuterQuery, TInnerQuery>
        where TOuterQuery : GremlinQueryBase
        where TInnerQuery : GremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TInnerQuery? _inner;

        public ContinuationBuilder(TOuterQuery outer, TInnerQuery inner)
        {
            _outer = outer;
            _inner = inner;
        }

        public ContinuationBuilder<TNewSourceQuery, TInnerQuery> FromSource<TNewSourceQuery>(TNewSourceQuery query)
            where TNewSourceQuery : GremlinQueryBase
        {
            return _inner is { } inner
                ? new(query, _inner)
                : throw new InvalidOperationException();
        }

        public SingleContinuationBuilder<TOuterQuery, TInnerQuery> With<TProjectedQuery>(Func<TInnerQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase
        {
            return _outer is { } outer && _inner is { } inner
                ? new(outer, inner, continuation(inner))
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct SingleContinuationBuilder<TOuterQuery, TInnerQuery>
        where TOuterQuery : GremlinQueryBase
        where TInnerQuery : GremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TInnerQuery? _inner;
        private readonly IGremlinQueryBase? _continuation;

        public SingleContinuationBuilder(TOuterQuery outer, TInnerQuery inner, IGremlinQueryBase continuation)
        {
            _outer = outer;
            _inner = inner;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TOuterQuery, TInnerQuery> With(Func<TInnerQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _inner is { } inner && _continuation is { } existingContinuation
                ? new (outer, inner, ImmutableList.Create(existingContinuation, continuation(inner)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder, IGremlinQueryBase, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuation is { } continuation
                ? builderTransformation(new FinalContinuationBuilder(outer), continuation)
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct MultiContinuationBuilder<TOuterQuery, TInnerQuery>
        where TOuterQuery : GremlinQueryBase
        where TInnerQuery : GremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TInnerQuery? _inner;
        private readonly IImmutableList<IGremlinQueryBase>? _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, TInnerQuery inner, IImmutableList<IGremlinQueryBase> continuations)
        {
            _outer = outer;
            _inner = inner;
            _continuations = continuations;
        }

        public MultiContinuationBuilder<TOuterQuery, TInnerQuery> With(Func<TInnerQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _inner is { } inner && _continuations is { } continuations
                ? new(outer, inner, continuations.Add(continuation(inner)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder, IImmutableList<IGremlinQueryBase>, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuations is { } continuations
                ? builderTransformation(new FinalContinuationBuilder(outer), continuations)
                : throw new InvalidOperationException();
        }
    }
   
    internal readonly struct FinalContinuationBuilder
    {
        private readonly StepStack? _steps;
        private readonly Projection? _projection;
        private readonly GremlinQueryBase? _query;
        private readonly QueryFlags _additionalFlags = QueryFlags.None;
        private readonly IImmutableDictionary<StepLabel, Projection>? _stepLabelProjections;

        public FinalContinuationBuilder(GremlinQueryBase outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.Projection, outerQuery.StepLabelProjections, outerQuery.Flags)
        {

        }

        public FinalContinuationBuilder(GremlinQueryBase outerQuery, StepStack? steps = null, Projection? projection = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None)
        {
            _steps = steps;
            _query = outerQuery;
            _projection = projection;
            _additionalFlags = additionalFlags;
            _stepLabelProjections = stepLabelProjections;
        }

        public FinalContinuationBuilder AddStep<TStep>(TStep step)
             where TStep : Step
        {
            return _query is { } query
                ? new(query, query.Environment.AddStepHandler.AddStep(_query.Steps, step, query.Environment))
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder WithNewProjection(Func<Projection, Projection> projectionTransformation)
        {
            return (_query is { } query)
                ? new(_query, _steps, projectionTransformation(_projection ?? Projection.Empty), _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder WithNewProjection(Projection newProjection)
        {
            return (_query is { } query)
                ? new(_query, _steps, newProjection, _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder WithNewStepLabelProjection(IImmutableDictionary<StepLabel, Projection> newStepLabelProjections)
        {
            return (_query is { } query)
                ? new(_query, _steps, _projection, newStepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder WithAdditionalFlags(QueryFlags additionalFlags)
        {
            return (_query is { } query)
                ? new(_query, _steps, _projection, _stepLabelProjections, _additionalFlags | additionalFlags)
                : throw new InvalidOperationException();
        }

        public TNewTargetQuery Build<TNewTargetQuery>() where TNewTargetQuery : IGremlinQueryBase
        {
            return _query is { } query
                ? query
                    .Continue(_steps, _projection, _stepLabelProjections, _additionalFlags)
                    .ChangeQueryType<TNewTargetQuery>()
                : throw new InvalidOperationException();
        }
    }
}
