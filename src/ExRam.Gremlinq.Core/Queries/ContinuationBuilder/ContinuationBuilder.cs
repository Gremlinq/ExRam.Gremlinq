﻿using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal readonly struct ContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;

        public ContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous)
        {
            _outer = outer;
            _anonymous = anonymous;
        }

        public ContinuationBuilder<TNewSourceQuery, TAnonymousQuery> FromSource<TNewSourceQuery>(TNewSourceQuery query)
            where TNewSourceQuery : GremlinQueryBase
        {
            return _anonymous is { } anonymous
                ? new(query, _anonymous)
                : throw new InvalidOperationException();
        }

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase
        {
            return _outer is { } outer && _anonymous is { } anonymous
                ? new(outer, anonymous, continuation(anonymous))
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IGremlinQueryBase? _continuation;

        public SingleContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IGremlinQueryBase continuation)
        {
            _outer = outer;
            _anonymous = anonymous;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With(Func<TAnonymousQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _anonymous is { } anonymous && _continuation is { } existingContinuation
                ? new (outer, anonymous, ImmutableList.Create(existingContinuation, continuation(anonymous)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, IGremlinQueryBase, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuation is { } continuation
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuation)
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IImmutableList<IGremlinQueryBase>? _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IImmutableList<IGremlinQueryBase> continuations)
        {
            _outer = outer;
            _anonymous = anonymous;
            _continuations = continuations;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With(Func<TAnonymousQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _anonymous is { } anonymous && _continuations is { } continuations
                ? new(outer, anonymous, continuations.Add(continuation(anonymous)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<IGremlinQueryBase>, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuations is { } continuations
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuations)
                : throw new InvalidOperationException();
        }
    }
   
    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase
    {
        private readonly StepStack? _steps;
        private readonly TOuterQuery? _query;
        private readonly Projection? _projection;
        private readonly QueryFlags _additionalFlags = QueryFlags.None;
        private readonly IImmutableDictionary<StepLabel, Projection>? _stepLabelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.Projection, outerQuery.StepLabelProjections, outerQuery.Flags)
        {

        }

        public FinalContinuationBuilder(TOuterQuery outerQuery, StepStack? steps = null, Projection? projection = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None)
        {
            _steps = steps;
            _query = outerQuery;
            _projection = projection;
            _additionalFlags = additionalFlags;
            _stepLabelProjections = stepLabelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep<TStep>(TStep step)
             where TStep : Step
        {
            return _query is { } query
                ? new(query, query.Environment.AddStepHandler.AddStep(_query.Steps, step, query.Environment))
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection(Func<Projection, Projection> projectionTransformation)
        {
            return (_query is { } query)
                ? new(_query, _steps, projectionTransformation(_projection ?? Projection.Empty), _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection(Projection newProjection)
        {
            return (_query is { } query)
                ? new(_query, _steps, newProjection, _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewStepLabelProjection(IImmutableDictionary<StepLabel, Projection> newStepLabelProjections)
        {
            return (_query is { } query)
                ? new(_query, _steps, _projection, newStepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithAdditionalFlags(QueryFlags additionalFlags)
        {
            return (_query is { } query)
                ? new(_query, _steps, _projection, _stepLabelProjections, _additionalFlags | additionalFlags)
                : throw new InvalidOperationException();
        }

        public TOuterQuery Build()
        {
            return Build<TOuterQuery>();
        }

        public TNewTargetQuery Build<TNewTargetQuery>()
        {
            return _query is { } query
                ? query
                    .Continue(_steps, _projection, _stepLabelProjections, _additionalFlags)
                    .ChangeQueryType<TNewTargetQuery>()
                : throw new InvalidOperationException();
        }
    }
}
