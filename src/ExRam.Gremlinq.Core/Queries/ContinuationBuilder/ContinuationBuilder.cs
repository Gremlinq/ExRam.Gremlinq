using System;
using System.Collections.Immutable;
using System.Linq;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core
{
    internal static class ContinuationBuilderExtensions
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

        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this ContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, state) => state(builder), builderTransformation);
        }


        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);
        }

        public static TNewQuery Build<TOuterQuery, TAnonymousQuery, TNewQuery>(this MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> continuationBuilder, Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<Traversal>, TNewQuery> builderTransformation)
            where TOuterQuery : GremlinQueryBase
            where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
        {
            return continuationBuilder.Build(static (builder, continuation, state) => state(builder, continuation), builderTransformation);
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

        public ContinuationBuilder<TNewOuterQuery, TAnonymousQuery> WithOuter<TNewOuterQuery>(TNewOuterQuery query)
            where TNewOuterQuery : GremlinQueryBase
        {
            return _anonymous is { } anonymous
                ? new(query, _anonymous)
                : throw new InvalidOperationException();
        }

        public SingleContinuationBuilder<TOuterQuery, TAnonymousQuery> With<TProjectedQuery>(Func<TAnonymousQuery, TProjectedQuery> continuation)
            where TProjectedQuery : IGremlinQueryBase
        {
            return _outer is { } outer && _anonymous is { } anonymous
                ? new(outer, anonymous, continuation.Apply(anonymous).ToTraversal())
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
                        .Select(contintuation => contintuation.Apply(anonymous).ToTraversal())
                        .ToImmutableList())
                : throw new InvalidOperationException();
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> ToMulti()
        {
            return _outer is { } outer && _anonymous is { } anonymous
                ? new(
                    outer,
                    anonymous,
                    ImmutableList<Traversal>.Empty)
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, TState, TNewQuery> builderTransformation, TState state)
        {
            return _outer is { } outer
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), state)
                : throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }

    internal readonly struct SingleContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly Traversal? _continuation;
        private readonly TAnonymousQuery? _anonymous;

        public SingleContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, Traversal continuation)
        {
            _outer = outer;
            _anonymous = anonymous;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With(Func<TAnonymousQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _anonymous is { } anonymous && _continuation is { } existingContinuation
                ? new (outer, anonymous, ImmutableList.Create(existingContinuation, continuation.Apply(anonymous).ToTraversal()))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, Traversal, TState, TNewQuery> builderTransformation, TState state)
        {
            return _outer is { } outer && _continuation is { } continuation
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuation, state)
                : throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }

    internal readonly struct MultiContinuationBuilder<TOuterQuery, TAnonymousQuery>
        where TOuterQuery : GremlinQueryBase
        where TAnonymousQuery : GremlinQueryBase, IGremlinQueryBase
    {
        private readonly TOuterQuery? _outer;
        private readonly TAnonymousQuery? _anonymous;
        private readonly IImmutableList<Traversal>? _continuations;

        public MultiContinuationBuilder(TOuterQuery outer, TAnonymousQuery anonymous, IImmutableList<Traversal> continuations)
        {
            _outer = outer;
            _anonymous = anonymous;
            _continuations = continuations;
        }

        public MultiContinuationBuilder<TOuterQuery, TAnonymousQuery> With(Func<TAnonymousQuery, IGremlinQueryBase> continuation)
        {
            return _outer is { } outer && _anonymous is { } anonymous && _continuations is { } continuations
                ? new(outer, anonymous, continuations.Add(continuation.Apply(anonymous).ToTraversal()))
                : throw new InvalidOperationException();
        }

        public TNewQuery Build<TNewQuery, TState>(Func<FinalContinuationBuilder<TOuterQuery>, IImmutableList<Traversal>, TState, TNewQuery> builderTransformation, TState state)
        {
            return _outer is { } outer && _continuations is { } continuations
                ? builderTransformation(new FinalContinuationBuilder<TOuterQuery>(outer), continuations, state)
                : throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }

    internal readonly struct FinalContinuationBuilder<TOuterQuery>
        where TOuterQuery : GremlinQueryBase
    {
        private readonly StepStack? _steps;
        private readonly TOuterQuery? _outer;
        private readonly Projection? _projection;
        private readonly QueryFlags _additionalFlags = QueryFlags.None;
        private readonly IImmutableDictionary<StepLabel, Projection>? _stepLabelProjections;

        public FinalContinuationBuilder(TOuterQuery outerQuery) : this(outerQuery, outerQuery.Steps, outerQuery.Projection, outerQuery.StepLabelProjections, outerQuery.Flags)
        {

        }

        public FinalContinuationBuilder(TOuterQuery outerQuery, StepStack steps, Projection projection, IImmutableDictionary<StepLabel, Projection> stepLabelProjections, QueryFlags additionalFlags)
        {
            _steps = steps;
            _outer = outerQuery;
            _projection = projection;
            _additionalFlags = additionalFlags;
            _stepLabelProjections = stepLabelProjections;
        }

        public FinalContinuationBuilder<TOuterQuery> AddStep<TStep>(TStep step)
             where TStep : Step
        {
            return _outer is { } query && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections
                ? new(query, query.Environment.AddStepHandler.AddStep(steps, step, query.Environment), projection, stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection(Func<Projection, Projection> projectionTransformation)
        {
            return _outer is { } query && _steps is { } steps && _stepLabelProjections is { } stepLabelProjections
                ? new(_outer, steps, projectionTransformation(_projection ?? Projection.Empty), _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewProjection(Projection newProjection)
        {
            return _outer is { } query && _steps is { } steps && _stepLabelProjections is { } stepLabelProjections
                ? new(_outer, steps, newProjection, _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithNewStepLabelProjection(IImmutableDictionary<StepLabel, Projection> newStepLabelProjections)
        {
            return _outer is { } query && _steps is { } steps && _projection is { } projection
                ? new(_outer, steps, projection, newStepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TOuterQuery> WithAdditionalFlags(QueryFlags additionalFlags)
        {
            return _outer is { } query && _steps is { } steps && _projection is { } projection && _stepLabelProjections is { } stepLabelProjections
                ? new(_outer, steps, projection, stepLabelProjections, _additionalFlags | additionalFlags)
                : throw new InvalidOperationException();
        }

        public TOuterQuery Build()
        {
            return Build<TOuterQuery>();
        }

        public TNewTargetQuery Build<TNewTargetQuery>()
        {
            return _outer is { } query
                ? query
                    .Continue(_steps, _projection, _stepLabelProjections, _additionalFlags)
                    .ChangeQueryType<TNewTargetQuery>()
                : throw new InvalidOperationException();
        }

        public TOuterQuery OuterQuery => _outer ?? throw new InvalidOperationException();
    }
}
