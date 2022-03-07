using System;
using System.Collections.Immutable;
using System.Linq;

using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class ContinuationExtensions
    {
        public static IGremlinQueryBase Apply<TAnonymousQuery, TProjectedQuery>(this Func<TAnonymousQuery, TProjectedQuery> continuation, TAnonymousQuery anonymous)
            where TAnonymousQuery : IGremlinQueryBase
            where TProjectedQuery : IGremlinQueryBase
        {
            var continuatedQuery = continuation(anonymous);

            if (continuatedQuery is GremlinQueryBase queryBase && (queryBase.Flags & QueryFlags.IsAnonymous) == QueryFlags.None)
                throw new InvalidOperationException("A query continuation must originate from the query that was passed to the continuation function. Did you accidentally use 'g' in the continuation?");

            return continuatedQuery;
        }
    }

    internal readonly struct ContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
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
                ? new(outer, anonymous, continuation.Apply(anonymous))
                : throw new InvalidOperationException();
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery>[] continuations)
            where TProjectedQuery : IGremlinQueryBase
        {
            return _outer is { } outer && _anonymous is { } anonymous
                ? new(
                    outer,
                    anonymous,
                    continuations
                        .Select(contintuation => contintuation.Apply(anonymous))
                        .ToImmutableList())
                : throw new InvalidOperationException();
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti()
        {
            return _outer is { } outer && _anonymous is { } anonymous
                ? new(
                    outer,
                    anonymous,
                    ImmutableList<IGremlinQueryBase>.Empty)
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
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
                ? new (outer, anonymous, ImmutableList.Create(existingContinuation, continuation.Apply(anonymous)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, IGremlinQueryBase, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuation is { } continuation
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuation)
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuation is { } continuation
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuation.ToTraversal())
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
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
                ? new(outer, anonymous, continuations.Add(continuation.Apply(anonymous)))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<IGremlinQueryBase>, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuations is { } continuations
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuations)
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery>(Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<Traversal>, TNewQuery> builderTransformation)
        {
            return _outer is { } outer && _continuations is { } continuations
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), ImmutableList.CreateRange(continuations.Select(x => x.ToTraversal())))
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

        public FinalContinuationBuilder(TOuterQuery outerQuery, StepStack steps, Projection projection, IImmutableDictionary<StepLabel, Projection> stepLabelProjections, QueryFlags additionalFlags)
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
            return _query is { } query && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections
                ? new(query, query.Environment.AddStepHandler.AddStep(_query.Steps, step, query.Environment), projection, stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection(Func<Projection, Projection> projectionTransformation)
        {
            return _query is { } query && _steps is { } steps && _stepLabelProjections is { } stepLabelProjections
                ? new(_query, steps, projectionTransformation(_projection ?? Projection.Empty), _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection(Projection newProjection)
        {
            return _query is { } query && _steps is { } steps && _stepLabelProjections is { } stepLabelProjections
                ? new(_query, steps, newProjection, _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewStepLabelProjection(IImmutableDictionary<StepLabel, Projection> newStepLabelProjections)
        {
            return _query is { } query && _steps is { } steps && _projection is { } projection
                ? new(_query, steps, projection, newStepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithAdditionalFlags(QueryFlags additionalFlags)
        {
            return _query is { } query && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections
                ? new(_query, steps, projection, stepLabelProjections, _additionalFlags | additionalFlags)
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
