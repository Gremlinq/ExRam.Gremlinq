using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

namespace ExRam.Gremlinq.Core.Queries
{
    //targetQuery
    //    .Continue()                     //ContinuationBuilder
    //    .FromSource(sourceQuery)        //ContinuationBuilder mit anderer Source
    //    .With(continuation1)            //SingleConrinuationBuilder
    //    .With(continuation2)            //MultiContinuationBuilder
    //    .WithSteps(targetQuery / targetQueries => Step / Step[]) // 2 Overloads für Single, 2 für Multi
    //    .As<TTargetQuery>()             //-> TargetQuery
    //    .AsSourceQuery

    internal static class AddStepHandlerExtensions
    {
        public static StepStack AddSteps<TStep>(this IAddStepHandler addStepHandler, StepStack stepStack, IEnumerable<TStep> steps, IGremlinQueryEnvironment env)
            where TStep : Step
        {
            foreach (var step in steps)
            {
                stepStack = addStepHandler.AddStep(stepStack, step, env);
            }

            return stepStack;
        }

    }
    
    internal readonly struct ContinuationBuilder<TSourceQuery, TTargetQuery>
        where TSourceQuery : GremlinQueryBase
        where TTargetQuery : GremlinQueryBase
    {
        private readonly TSourceQuery? _source;
        private readonly TTargetQuery? _target;

        public ContinuationBuilder(TSourceQuery source, TTargetQuery target)
        {
            _source = source;
            _target = target;
        }

        public static ContinuationBuilder<TNewSourceQuery, TNewSourceQuery> Create<TNewSourceQuery>(TNewSourceQuery query)
            where TNewSourceQuery : GremlinQueryBase
        {
            return new(query, query);
        }

        public ContinuationBuilder<TNewSourceQuery, TTargetQuery> FromSource<TNewSourceQuery>(TNewSourceQuery query)
            where TNewSourceQuery : GremlinQueryBase
        {
            return _target is { } target
                ? new(query, _target)
                : throw new InvalidOperationException();
        }

        public SingleContinuationBuilder<TSourceQuery, TTargetQuery> With(Func<TSourceQuery, IGremlinQueryBase> continuation)
        {
            return _source is { } source && _target is { } target
                ? new(source, target, continuation(source))
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct SingleContinuationBuilder<TSourceQuery, TTargetQuery>
        where TSourceQuery : GremlinQueryBase
        where TTargetQuery : GremlinQueryBase
    {
        private readonly TSourceQuery? _source;
        private readonly TTargetQuery? _target;
        private readonly IGremlinQueryBase? _continuation;

        public SingleContinuationBuilder(TSourceQuery source, TTargetQuery target, IGremlinQueryBase continuation)
        {
            _source = source;
            _target = target;
            _continuation = continuation;
        }

        public MultiContinuationBuilder<TSourceQuery, TTargetQuery> With(Func<TSourceQuery, IGremlinQueryBase> continuation)
        {
            return _source is { } source && _target is { } target && _continuation is { } existingContinuation
                ? new (source, target, ImmutableList.Create(existingContinuation,  continuation(source)))
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TTargetQuery> AddStep<TStep>(Func<IGremlinQueryBase, TStep> stepFactory)
             where TStep : Step
        {
            return _target is { } target && _continuation is { } continuation
                ? new(target, target.Environment.AddStepHandler.AddStep(_target.Steps, stepFactory(continuation), target.Environment))
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TTargetQuery> AddSteps<TStep>(Func<IGremlinQueryBase, IEnumerable<TStep>> stepsFactory)
            where TStep : Step
        {
            return _target is { } target && _continuation is { } continuation
                ? new(target, target.Environment.AddStepHandler.AddSteps(_target.Steps, stepsFactory(continuation), target.Environment))
                : throw new InvalidOperationException();
        }
    }

    internal readonly struct MultiContinuationBuilder<TSourceQuery, TTargetQuery>
        where TSourceQuery : GremlinQueryBase
        where TTargetQuery : GremlinQueryBase
    {
        private readonly TSourceQuery? _source;
        private readonly TTargetQuery? _target;
        private readonly IImmutableList<IGremlinQueryBase>? _continuations;

        public MultiContinuationBuilder(TSourceQuery source, TTargetQuery target, IImmutableList<IGremlinQueryBase> continuations)
        {
            _source = source;
            _target = target;
            _continuations = continuations;
        }

        public MultiContinuationBuilder<TSourceQuery, TTargetQuery> With(Func<TSourceQuery, IGremlinQueryBase> continuation)
        {
            return _source is { } source && _target is { } target && _continuations is { } continuations
                ? new(source, target, continuations.Add(continuation(source)))
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TTargetQuery> AddStep<TStep>(Func<IImmutableList<IGremlinQueryBase>, TStep> stepFactory)
            where TStep : Step
        {
            return _target is { } target && _continuations is { } continuations
                ? new(target, target.Environment.AddStepHandler.AddStep(_target.Steps, stepFactory(continuations), target.Environment))
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TTargetQuery> AddSteps<TStep>(Func<IImmutableList<IGremlinQueryBase>, IEnumerable<TStep>> stepsFactory)
            where TStep : Step
        {
            return _target is { } target && _continuations is { } continuations
                ? new(target, target.Environment.AddStepHandler.AddSteps(_target.Steps, stepsFactory(continuations), target.Environment))
                : throw new InvalidOperationException();
        }
    }
   
    internal readonly struct FinalContinuationBuilder<TTargetQuery>
        where TTargetQuery : GremlinQueryBase
    {
        private readonly StepStack? _steps;
        private readonly TTargetQuery? _target;
        private readonly Projection? _projection;
        private readonly QueryFlags _additionalFlags = QueryFlags.None;
        private readonly IImmutableDictionary<StepLabel, Projection>? _stepLabelProjections;

        public FinalContinuationBuilder(TTargetQuery targetQuery, StepStack? steps = null, Projection? projection = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None)
        {
            _steps = steps;
            _projection = projection;
            _target = targetQuery;
            _additionalFlags = additionalFlags;
            _stepLabelProjections = stepLabelProjections;
        }

        public FinalContinuationBuilder<TTargetQuery> WithNewProjection(Projection newProjection)
        {
            return (_target is { } target)
                ? new(_target, _steps, newProjection, _stepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TTargetQuery> WithNewStepLabelProjection(IImmutableDictionary<StepLabel, Projection> newStepLabelProjections)
        {
            return (_target is { } target)
                ? new(_target, _steps, _projection, newStepLabelProjections, _additionalFlags)
                : throw new InvalidOperationException();
        }

        public FinalContinuationBuilder<TTargetQuery> WithAdditionalFlags(QueryFlags additionalFlags)
        {
            return (_target is { } target)
                ? new(_target, _steps, _projection, _stepLabelProjections, _additionalFlags | additionalFlags)
                : throw new InvalidOperationException();
        }

        public TNewTargetQuery As<TNewTargetQuery>() where TNewTargetQuery : IGremlinQueryBase
        {
            return _target is { } target
                ? target
                    .Continue(_steps, _projection, _stepLabelProjections, _additionalFlags)
                    .ChangeQueryType<TNewTargetQuery>()
                : throw new InvalidOperationException();
        }
    }
}
