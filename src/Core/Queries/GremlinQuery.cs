﻿#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

using Microsoft.Extensions.Logging;

using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Core
{
    internal sealed partial class GremlinQuery<T1, T2, T3, T4> : GremlinQueryBase
    {
        public GremlinQuery(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata) : base(environment, steps, labelProjections, metadata)
        {

        }

        private GremlinQuery<TEdge, T1, object, IGremlinQueryBase> AddE<TEdge>(TEdge newEdge) => this
            .Continue()
            .Build(
                static (builder, newEdge) => builder
                    .AddStep(new AddEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetCache().GetLabel(newEdge!.GetType())))
                    .WithNewProjection(Projection.Edge)
                    .AsAuto<TEdge, T1>(),
                newEdge)
            .AddOrUpdate(newEdge, true);

        private GremlinQuery<T1, T2, T3, T4> AddOrUpdate(T1 element, bool add)
        {
            var ret = this;
            var props = element
                .Serialize(
                    Environment,
                    add
                        ? SerializationBehaviour.IgnoreOnAdd
                        : SerializationBehaviour.IgnoreOnUpdate)
                .ToArray();

            var droppableKeys = new List<string>();
            var propertySteps = new List<PropertyStep>();

            foreach (var (key, maybeValue) in props)
            {
                if (!Environment.FeatureSet.Supports(VertexFeatures.UserSuppliedIds) && T.Id.Equals(key.RawKey))
                    Environment.Logger.LogWarning($"User supplied ids are not supported according to the environment's {nameof(Environment.FeatureSet)}.");
                else
                {
                    var localPropertySteps = maybeValue is { } value
                        ? this
                            .GetPropertySteps(key, value, Steps.Projection == Projection.Vertex)
                            .ToArray()
                        : Array.Empty<PropertyStep>();

                    if (!add && key.RawKey is string rawStringKey && localPropertySteps.All(static propertyStep => Cardinality.List.Equals(propertyStep.Cardinality)))
                        droppableKeys.Add(rawStringKey);

                    propertySteps.AddRange(localPropertySteps);
                }
            }

            if (droppableKeys.Count > 0)
            {
                ret = ret
                    .SideEffect(__ => __
                        .Properties<object, object, object>(
                            Projection.Empty,
                            droppableKeys)
                        .Drop());
            }

            return ret
                .Continue()
                .Build(
                    static (builder, propertySteps) => builder
                        .AddSteps(propertySteps),
                    propertySteps);
        }

        private TTargetQuery AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? maybeProjectionTransformation)
            where TTargetQuery : IStartGremlinQuery => this
                .Continue()
                .Build(
                    static (builder, tuple) =>
                    {
                        var (step, maybeProjectionTransformation) = tuple;

                        builder = builder
                            .AddStep(step);

                        if (maybeProjectionTransformation is { } projectionTransformation)
                            builder = builder.WithNewProjection(projectionTransformation);

                        return builder
                            .As<TTargetQuery>();
                    },
                    (step, maybeProjectionTransformation));

        private GremlinQuery<TVertex, object, object, IGremlinQueryBase> AddV<TVertex>(TVertex vertex) => this
            .Continue()
            .Build(
                static (builder, vertex) => builder
                    .AddStep(new AddVStep(builder.OuterQuery.Environment.Model.VerticesModel.GetCache().GetLabel(vertex!.GetType())))
                    .WithNewProjection(Projection.Vertex)
                    .AsAuto<TVertex>(),
                vertex)
            .AddOrUpdate(vertex, true);

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Scope scope, Func<GremlinQuery<T1, T2, T3, T4>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            var stepLabel = new TStepLabel();

            return this
                .Aggregate(scope, stepLabel)
                .Map(continuation, stepLabel);
        }

        private GremlinQuery<T1, T2, T3, T4> Aggregate<TStepLabel>(Scope scope, TStepLabel stepLabel)
            where TStepLabel : StepLabel => this
                .Continue()
                .Build(
                    static (builder, tuple) => builder
                        .AddStep(new AggregateStep(tuple.scope, tuple.stepLabel))
                        .WithNewLabelProjections(
                            static (existingProjections, tuple) => existingProjections.Set(
                                tuple.stepLabel,
                                tuple.projection,
                                static (projections, projection) => projections.WithSideEffectLabelProjection(projection)),
                            (tuple.stepLabel, projection: builder.OuterQuery.Steps.Projection.Fold())),
                    (scope, stepLabel));

        private GremlinQuery<T1, T2, T3, T4> And<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation1, Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation2, TState state) => And(this
            .Continue(ContinuationFlags.Filter)
            .With(continuation1, state)
            .With(continuation2, state));

        private GremlinQuery<T1, T2, T3, T4> And(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>[] continuations) => And(this
            .Continue(ContinuationFlags.Filter)
            .With(continuations));

        private static GremlinQuery<T1, T2, T3, T4> And(MultiContinuationBuilder<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<T1, T2, T3, T4>> continuationBuilder) => continuationBuilder
            .Build(static (builder, traversals) =>
            {
                if (traversals.Length == 0)
                    throw new ArgumentException("Expected at least 1 sub-query.");

                var count = 0;
                var containsNoneStep = false;
                var containsWriteStep = false;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.IsNone())
                        containsNoneStep = true;

                    if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                        containsWriteStep = true;
                    else if (traversal.IsIdentity())
                        continue;

                    traversals[count++] = traversal;
                }

                if (containsNoneStep && !containsWriteStep)
                    builder = builder.None();
                else
                {
                    var fusedTraversals = traversals[..count]
                        .Fuse(static (p1, p2) => p1.And(p2));

                    if (fusedTraversals is [var single])
                        builder = builder.Where(single);
                    else
                    {
                        if (fusedTraversals.All(static traversal => traversal.Steps.All(static x => x is IFilterStep)))
                        {
                            for (var i = 0; i < fusedTraversals.Length; i++)
                            {
                                builder = builder
                                    .AddSteps(fusedTraversals[i]);
                            }
                        }
                        else
                        {
                            builder = builder
                                .AddStep(new AndStep(LogicalStep<AndStep>.FlattenLogicalTraversals(fusedTraversals)));
                        }
                    }
                }

                return builder;
            });

        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            TStepLabel stepLabel;
            var toContinue = this;

            if (Steps.PeekOrDefault() is AsStep { StepLabel: TStepLabel existingStepLabel })
                stepLabel = existingStepLabel;
            else
            {
                stepLabel = new TStepLabel();
                toContinue = As(stepLabel);
            }

            return toContinue
                .Map(continuation, stepLabel);
        }

        private GremlinQuery<T1, T2, T3, T4> As(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AsStep(stepLabel))
                    .WithNewLabelProjections(
                        static (projection, tuple) => projection.Set(
                            tuple.stepLabel,
                            tuple.otherProjection,
                            static (existingProjections, otherProjection) => existingProjections.WithStepLabelProjection(otherProjection)),
                        (stepLabel, otherProjection: builder.OuterQuery.Steps.Projection)),
                stepLabel);

        private GremlinQuery<T1, T2, T3, T4> Barrier() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BarrierStep.Instance));

        private GremlinQuery<object, object, object, IGremlinQueryBase> Both() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothStep.NoLabels)
                .AsAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Both<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .AsAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> BothE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .AsAuto());

        private GremlinQuery<TEdge, object, object, IGremlinQueryBase> BothE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .AsAuto<TEdge>());

        private GremlinQuery<TTarget, object, object, IGremlinQueryBase> BothV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .AsAuto<TTarget>());

        private GremlinQuery<TSelectedElement, TArrayItem, object, TQuery> Cap<TSelectedElement, TArrayItem, TQuery>(StepLabel<IArrayGremlinQuery<TSelectedElement, TArrayItem, TQuery>, TSelectedElement> stepLabel) where TQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new CapStep(stepLabel))
                    .WithNewProjection(static projection => projection.Fold())
                    .AsAuto<TSelectedElement, TArrayItem, object, TQuery>(),
                stepLabel);

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Choose<TTrueQuery, TFalseQuery, TTargetQuery>(
                    __ => __
                        .Where(predicate),
                    trueChoice,
                    maybeFalseChoice);

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> traversalPredicate, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(traversalPredicate)
                .Build(
                    static (builder, traversal, choiceTuple) => builder.OuterQuery.Choose<TTrueQuery, TFalseQuery, TTargetQuery>(traversal, choiceTuple.trueChoice, choiceTuple.maybeFalseChoice),
                    (trueChoice, maybeFalseChoice));

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Traversal chooseTraversal, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(trueChoice)
                .Build(
                    static (builder, trueTraversal, state) =>
                    {
                        var (chooseTraversal, maybeFalseChoice) = state;

                        if (maybeFalseChoice is { } falseChoice)
                        {
                            return builder.OuterQuery
                                .Continue()
                                .With(falseChoice)
                                .Build(
                                    static (builder, falseTraversal, state) =>
                                    {
                                        var (chooseTraversal, trueTraversal) = state;

                                        return builder
                                            .AddStep(chooseTraversal is [IsStep isStep]
                                                ? new ChoosePredicateStep(
                                                    isStep.Predicate,
                                                    trueTraversal,
                                                    falseTraversal)
                                                : new ChooseTraversalStep(
                                                    chooseTraversal,
                                                    trueTraversal,
                                                    falseTraversal))
                                            .WithNewProjection(
                                                static (_, state) => state.falseTraversal.Projection.Lowest(state.trueTraversal.Projection),
                                                (falseTraversal, trueTraversal))
                                            .As<TTargetQuery>()
                                            .Build();
                                    },
                                    (chooseTraversal, trueTraversal));
                        }

                        return builder
                            .AddStep(chooseTraversal is [IsStep isStep]
                                ? new ChoosePredicateStep(
                                    isStep.Predicate,
                                    trueTraversal)
                                : new ChooseTraversalStep(
                                    state.chooseTraversal,
                                    trueTraversal))
                            .WithNewProjection(
                                static (projection, otherProjection) => projection.Lowest(otherProjection),
                                trueTraversal.Projection)
                            .As<TTargetQuery>()
                            .Build();
                    },
                    (chooseTraversal, maybeFalseChoice));

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<T1, T2, T3, T4>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(new ChooseBuilder<GremlinQuery<T1, T2, T3, T4>, object>(this)).TargetQuery;
        }

        private TReturnQuery Coalesce<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>[] continuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(continuations)
                .Build(static (builder, traversals) =>
                {
                    if (traversals.Length == 0)
                        throw new ArgumentException("Coalesce must have at least one sub-query.");

                    if (!traversals.All(static traversal => traversal.IsIdentity()))
                    {
                        if (traversals is [var singleTraversal])
                        {
                            builder = builder
                                .AddSteps(singleTraversal)
                                .WithNewProjection(singleTraversal.Projection);
                        }
                        else
                        {
                            builder = builder
                                .AddStep(new CoalesceStep(traversals
                                    .ToImmutableArray()))
                                .WithNewProjection(traversals
                                    .LowestProjection());
                        }
                    }

                    return builder
                        .As<TReturnQuery>();
                });

        private GremlinQuery<T1, T2, T3, T4> Coin(double probability) => this
            .Continue()
            .Build(
                static (builder, probability) => builder
                    .AddStep(new CoinStep(probability)),
                probability);

        private TTargetQuery ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> transformation, Func<Projection, Projection>? maybeProjectionTransformation)
            where TTargetQuery : IStartGremlinQuery => this
                .Continue()
                .Build(
                    static (builder, tuple) => builder
                        .WithSteps(
                            static (steps, transformation) => transformation(steps),
                            tuple.transformation)
                        .WithNewProjection(
                            static (projection, maybeProjectionTransformation) => maybeProjectionTransformation is { } projectionTransformation
                                ? projectionTransformation(projection)
                                : projection,
                            tuple.maybeProjectionTransformation)
                        .As<TTargetQuery>(),
                    (transformation, maybeProjectionTransformation));

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> Constant<TValue>(TValue constant) => this
            .Continue()
            .Build(
                static (builder, constant) => builder
                    .AddStep(new ConstantStep(constant!))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TValue>(),
                constant);

        private GremlinQuery<long, object, object, IGremlinQueryBase> Count(Scope scope) => this
            .Continue()
            .Build(
                static (builder, scope) => builder
                    .AddStep(Scope.Global.Equals(scope)
                        ? CountStep.Global
                        : CountStep.Local)
                    .WithNewProjection(Projection.Value)
                    .AsAuto<long>(),
                scope);

        private GremlinQuery<long, object, object, IGremlinQueryBase> CountGlobal() => Count(Scope.Global);

        private GremlinQuery<long, object, object, IGremlinQueryBase> CountLocal() => Count(Scope.Local);

        private GremlinQuery<T1, T2, T3, T4> CyclicPath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(CyclicPathStep.Instance));

        private string Debug()
        {
            var serialized = Environment.Serializer
                .TransformTo<Bytecode>()
                .From(this, Environment);

            return Environment.Debugger.Debug(serialized, Environment);
        }

        private GremlinQuery<T1, T2, T3, T4> DedupGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Global));

        private GremlinQuery<T1, T2, T3, T4> DedupLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Local));

        private GremlinQuery<object, object, object, IGremlinQueryBase> Drop() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DropStep.Instance)
                .WithNewProjection(Projection.Empty)
                .AsAuto());

        private GremlinQuery<T1, T2, T3, T4> DropProperties(string key) => this
            .SideEffect(_ => _
                .Properties<object, object, object>(
                    Projection.Empty,
                    new[] { key })
                .Drop());

        private GremlinQuery<object, object, object, IGremlinQueryBase> E(ImmutableArray<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new EStep(ids))
                    .WithNewProjection(Projection.Edge)
                    .AsAuto(),
                ids);

        private GremlinQuery<string, object, object, IGremlinQueryBase> Explain() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ExplainStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<string>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Fail(string? message = null) => this
            .Continue()
            .Build(
                static (builder, message) => builder
                    .AddStep(message is { } actualMessage
                        ? new FailStep(actualMessage)
                        : FailStep.NoMessage)
                    .WithNewProjection(Projection.Empty)
                    .AsAuto(),
                message);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => builder
                .AddStep(new FlatMapStep(innerTraversal))
                .WithNewProjection(innerTraversal.Projection)
                .As<TTargetQuery>());

        private GremlinQuery<T1[], T1, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() where TNewFoldedQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(FoldStep.Instance)
                .WithNewProjection(static projection => projection.Fold())
                .AsAuto<T1[], T1, object, TNewFoldedQuery>());

        private GremlinQuery<T1, object, object, IGremlinQueryBase> ForceElement() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(static _ => _.Highest(Projection.Element))
                .AsAuto<T1>());

        private GremlinQuery<T1, TNewOutVertex, TInVertex, IGremlinQueryBase> From<TNewOutVertex, TInVertex>(Func<GremlinQuery<TInVertex, T2, T3, T4>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexContinuation) => this
            .Continue<TInVertex, T2, T3, T4>()
            .With(fromVertexContinuation)
            .Build(static (builder, fromVertexTraversal) => builder
                .AddStep(new AddEStep.FromTraversalStep(fromVertexTraversal))
                .AsAuto<T1, TNewOutVertex, TInVertex>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, IGremlinQueryBase> From<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel<TNewOutVertex> label) => this
           .Continue()
           .Build(
                static (builder, label) => builder
                    .AddStep(new AddEStep.FromLabelStep(label))
                    .AsAuto<TNewElement, TNewOutVertex, TNewInVertex>(),
                label);

        private IMapGremlinQuery<IDictionary<TKey, TValue>> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>, IGroupBuilderWithKeyAndValue<TKey, TValue>> projection) =>
            projection(new GroupBuilder<object, object>(Continue())).Build();

        private IMapGremlinQuery<IDictionary<TKey, T1[]>> Group<TKey>(Func<IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>, IGroupBuilderWithKey<IGremlinQueryBase<T1>, TKey>> projection) => new GroupBuilder<object, object>(Continue())
            .Map(projection)
            .ByValue(__ => __
                .Cast<T1>()
                .Fold())
            .Build();

        private GremlinQuery<object, object, object, IGremlinQueryBase> Id() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(IdStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto());

        private GremlinQuery<T1, T2, T3, T4> Identity() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(IdentityStep.Instance));

        private GremlinQuery<object, object, object, IGremlinQueryBase> In() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InStep.NoLabels)
                .AsAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> In<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .AsAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> InE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .AsAuto());

        private GremlinQuery<TEdge, object, T1, IGremlinQueryBase> InE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .AsAuto<TEdge, object, T1>());

        private GremlinQuery<TNewElement, T2, T3, T4> Inject<TNewElement>(IEnumerable<TNewElement> elements) => this
            .Continue()
            .Build(
                static (builder, elements) => builder
                    .AddStep(new InjectStep(
                        elements
                            .Where(static x => x is not null)
                            .Select(static x => (object)x!)
                            .ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TNewElement, T2, T3, T4>(),
                elements);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> InOutV<TNewElement>(Step step) => this
            .Continue()
            .Build(
                static (builder, step) => builder
                    .AddStep(step)
                    .WithNewProjection(Projection.Vertex)
                    .AsAuto<TNewElement>(),
                step);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> InV<TNewElement>() => InOutV<TNewElement>(InVStep.Instance);

        private GremlinQuery<string, object, object, IGremlinQueryBase> Key() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(KeyStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<string>());

        private GremlinQuery<string, object, object, IGremlinQueryBase> Label() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(LabelStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<string>());

        private GremlinQuery<T1, T2, T3, T4> LimitGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? LimitStep.LimitGlobal1
                        : new LimitStep(count, Scope.Global)),
                count);

        private GremlinQuery<T1, T2, T3, T4> LimitLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                            ? LimitStep.LimitLocal1Workaround
                            : LimitStep.LimitLocal1
                        : new LimitStep(count, Scope.Local)),
                count);

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(localTraversal)
            .Build(static (builder, continuationTraversal) =>
            {
                if (!continuationTraversal.IsIdentity())
                {
                    builder = builder
                        .AddStep(new LocalStep(continuationTraversal))
                        .WithNewProjection(continuationTraversal.Projection);
                }

                return builder
                    .As<TTargetQuery>();
            });

        private TTargetQuery Loop<TTargetQuery>(Func<IStartLoopBuilder<TTargetQuery>, IFinalLoopBuilder<TTargetQuery>> loopBuilderTransformation)
            where TTargetQuery : IGremlinQueryBase => loopBuilderTransformation(new LoopBuilder<TTargetQuery>(this)).Build();

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => innerTraversal.IsIdentity()
                ? builder
                    .As<TTargetQuery>()
                : builder
                    .AddStep(new MapStep(innerTraversal))
                    .WithNewProjection(innerTraversal.Projection)
                    .As<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> MaxGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MaxStep.Global)
                .WithNewProjection(Projection.Value));

        private TNewQuery MaxLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MaxStep.Local)
                .As<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> MeanGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MeanStep.Global)
                .WithNewProjection(Projection.Value));

        private TNewQuery MeanLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MeanStep.Local)
                .As<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> MinGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MinStep.Global)
                .WithNewProjection(Projection.Value));

        private TNewQuery MinLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MinStep.Local)
                .As<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> None() => this
            .Continue()
            .Build(static builder => builder
                .None());

        private GremlinQuery<T1, T2, T3, T4> Not<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation, TState state) => this
            .Continue()
            .With(continuation, state)
            .Build(static (builder, innerTraversal) => innerTraversal.IsIdentity()
                ? builder
                    .None()
                : innerTraversal.IsNone()
                    ? builder
                    : builder
                        .AddStep(new NotStep(innerTraversal)));

        private TTargetQuery OfType<TNewElement, TTargetQuery>(IGraphElementModel model, bool force = false) where TTargetQuery : IStartGremlinQuery => this
            .Continue()
            .Build(
                static (builder, tuple) =>
                {
                    var (env, model, force) = tuple;

                    if (force || !typeof(TNewElement).IsAssignableFrom(typeof(T1)))
                    {
                        var labels = model.TryGetFilterLabels(typeof(TNewElement), env.Options.GetValue(GremlinqOption.FilterLabelsVerbosity)) ?? ImmutableArray.Create(typeof(TNewElement).Name);

                        if (labels.Length > 0)
                            builder = builder.AddStep(new HasLabelStep(labels));
                    }

                    return builder
                        .As<TTargetQuery>();
                },
                (Environment, model, force));

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(optionalTraversal)
            .Build(static (builder, continuedTraversal) => builder
                .AddStep(new OptionalStep(continuedTraversal))
                .WithNewProjection(
                    static (projection, otherProjection) => projection.Lowest(otherProjection),
                    continuedTraversal.Projection)
                .As<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> Or<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation1, Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation2, TState state) => Or(this
            .Continue(ContinuationFlags.Filter)
            .With(continuation1, state)
            .With(continuation2, state));

        private GremlinQuery<T1, T2, T3, T4> Or(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>[] continuations) => Or(this
            .Continue(ContinuationFlags.Filter)
            .With(continuations));

        private static GremlinQuery<T1, T2, T3, T4> Or(MultiContinuationBuilder<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<T1, T2, T3, T4>> continuationBuilder) => continuationBuilder
            .Build(static (builder, traversals) =>
            {
                if (traversals.Length == 0)
                    throw new ArgumentException("Expected at least 1 sub-query.");

                var count = 0;
                var containsWriteStep = false;
                var containsIdentityStep = false;

                for (var i = 0; i < traversals.Length; i++)
                {
                    var traversal = traversals[i];

                    if (traversal.IsIdentity())
                        containsIdentityStep = true;
                    else if (traversal.SideEffectSemantics == SideEffectSemantics.Write)
                        containsWriteStep = true;
                    else if (traversal.IsNone())
                        continue;

                    traversals[count++] = traversal;
                }

                if (!containsIdentityStep || containsWriteStep)
                {
                    var fusedTraversals = traversals[..count]
                        .Fuse(static (p1, p2) => p1.Or(p2));

                    builder = fusedTraversals switch
                    {
                        [] => builder
                            .None(),
                        [var singleTraversal] => builder
                            .Where(singleTraversal),
                        _ => builder
                            .AddStep(new OrStep(LogicalStep<OrStep>.FlattenLogicalTraversals(fusedTraversals)))
                    };
                }

                return builder;
            });

        private TTargetQuery Order<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => projection(new OrderBuilder(this)).Build();

        private TTargetQuery OrderGlobal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Global)
                    .Build()
                    .Order(projection),
                projection);

        private TTargetQuery OrderLocal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Local)
                    .Build()
                    .Order(projection),
                projection);

        private GremlinQuery<TTarget, object, object, IGremlinQueryBase> OtherV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OtherVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .AsAuto<TTarget>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Out() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutStep.NoLabels)
                .AsAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Out<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .AsAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> OutE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .AsAuto());

        private GremlinQuery<TEdge, T1, object, IGremlinQueryBase> OutE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .AsAuto<TEdge, T1>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> OutV<TNewElement>() => InOutV<TNewElement>(OutVStep.Instance);

        private GremlinQuery<Path, object, object, IGremlinQueryBase> Path() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(PathStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<Path>());

        private GremlinQuery<string, object, object, IGremlinQueryBase> Profile() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ProfileStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<string>());

        private IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectMapResult<TResult>> continuation)
        {
            return new ProjectBuilder(this)
                .Map(continuation)
                .Build();
        }

        private IGremlinQuery<dynamic> Project(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectDynamicResult> continuation)
        {
            return new ProjectBuilder(this)
                .Map(continuation)
                .Build();
        }

        private IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectTupleResult<TResult>> continuation)
            where TResult : ITuple
        {
            return new ProjectBuilder(this)
                .Map(continuation)
                .Build();
        }

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, params Expression[] projections) => Properties<TNewElement, TNewPropertyValue, TNewMeta>(
            projection,
            projections
                .Select(projection => GetKey(projection).RawKey)
                .OfType<string>());

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, IEnumerable<string> keys) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new PropertiesStep(tuple.keys.ToImmutableArray()))
                    .WithNewProjection(tuple.projection)
                    .AsAuto<TNewElement, TNewPropertyValue, TNewMeta>(),
                (keys, projection));

        private GremlinQuery<T1, T2, T3, T4> Property(LambdaExpression projection, object? value) => Property(GetKey(projection), value);

        private GremlinQuery<T1, T2, T3, T4> Property(Key key, object? value) => this
            .Continue()
            .Build(
                static (builder, tuple) =>
                {
                    if (tuple.value == null)
                    {
                        if (tuple.key.RawKey is string stringKey)
                            return builder.OuterQuery.DropProperties(stringKey);

                        throw new InvalidOperationException("Can't set a special property to null.");
                    }

                    foreach (var propertyStep in builder.OuterQuery.GetPropertySteps(tuple.key, tuple.value, builder.OuterQuery.Steps.Projection == Projection.Vertex))
                    {
                        builder = builder.AddStep(propertyStep);
                    }

                    return builder
                        .Build();
                },
                (key, value));

        private GremlinQuery<T1, T2, T3, T4> Property(Key key, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (builder, valueTraversal, key) => builder.OuterQuery.Property(key, valueTraversal),
                key);

        private GremlinQuery<T1, T2, T3, T4> Property(LambdaExpression projection, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (builder, valueTraversal, projection) => builder.OuterQuery.Property(projection, valueTraversal),
                projection);

        private GremlinQuery<T1, T2, T3, T4> Range(long low, long high, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(Scope.Local.Equals(tuple.scope) && tuple.high - tuple.low == 1 && builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                        ? new MapStep(Traversal.Empty.Push(
                            UnfoldStep.Instance,
                            new RangeStep(tuple.low, tuple.high, Scope.Global),
                            FoldStep.Instance))
                        : new RangeStep(tuple.low, tuple.high, tuple.scope)),
                (low, high, scope));

        private GremlinQuery<T1, T2, T3, T4> RangeGlobal(long low, long high) => Range(low, high, Scope.Global);

        private GremlinQuery<T1, T2, T3, T4> RangeLocal(long low, long high) => Range(low, high, Scope.Local);

        private IGremlinQuery<TSelectedElement> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel) => Select<IGremlinQuery<TSelectedElement>>(stepLabel);

        private TNewQuery Select<TNewQuery>(StepLabel stepLabel) where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SelectStepLabelStep(ImmutableArray.Create(tuple.stepLabel)))
                    .WithNewProjection(tuple.stepLabelProjection)
                    .As<TNewQuery>(),
                (stepLabel, stepLabelProjection: GetLabelProjection(stepLabel)));

        private TTargetQuery Select<TTargetQuery>(params Expression[] projections) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, projections) =>
                {
                    var keys = projections
                        .Select(static expression =>
                        {
                            if (expression is LambdaExpression { Parameters: [var singleParameter], Body: { } lambdaBody } && lambdaBody.IsIndexerGet(out var target, out var indexerArgument) && target == singleParameter)
                            {
                                if (indexerArgument.GetValue() is string indexerArgumentValue)
                                    return indexerArgumentValue;
                            }

                            return (Key)expression.AssumePropertyOrFieldMemberExpression().Member.Name;
                        })
                        .ToImmutableArray();

                    return builder
                        .AddStep(new SelectKeysStep(keys))
                        .WithNewProjection(
                            static (projection, keys) => projection.If<TupleProjection>(tuple => tuple.Select(keys)),
                            keys)
                        .As<TTargetQuery>();
                },
                projections);

        private GremlinQuery<T1, T2, T3, T4> SideEffect(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> sideEffectContinuation) => this
            .Continue()
            .With(sideEffectContinuation)
            .Build(static (builder, traversal) => builder
                .AddStep(new SideEffectStep(traversal)));

        private GremlinQuery<T1, T2, T3, T4> SimplePath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(SimplePathStep.Instance));

        private GremlinQuery<T1, T2, T3, T4> Skip(long count, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SkipStep(tuple.count, tuple.scope)),
                (count, scope));

        private GremlinQuery<T1, T2, T3, T4> SumGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Global))
                .WithNewProjection(Projection.Value));

        private TNewQuery SumLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Local))
                .WithNewProjection(Projection.Value)
                .As<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> TailGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(new TailStep(count, Scope.Global)),
                count);

        private GremlinQuery<T1, T2, T3, T4> TailLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                            ? TailStep.TailLocal1Workaround
                            : TailStep.TailLocal1
                        : new TailStep(count, Scope.Local)),
                count);

        private GremlinQuery<T1, TOutVertex, TNewInVertex, IGremlinQueryBase> To<TOutVertex, TNewInVertex>(Func<GremlinQuery<TOutVertex, T2, T3, T4>, IVertexGremlinQueryBase<TNewInVertex>> toVertexContinuation) => this
            .Continue<TOutVertex, T2, T3, T4>()
            .With(toVertexContinuation)
            .Build(static (builder, toVertexTraversal) => builder
                .AddStep(new AddEStep.ToTraversalStep(toVertexTraversal))
                .AsAuto<T1, TOutVertex, TNewInVertex>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, IGremlinQueryBase> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AddEStep.ToLabelStep(stepLabel))
                    .AsAuto<TNewElement, TNewOutVertex, TNewInVertex>(),
                stepLabel);

        private GremlinQuery<T1, T2, T3, T4> Unfold() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(UnfoldStep.Instance)
                .WithNewProjection(static projection => projection.If<ArrayProjection>(static array => array.Unfold())));

        private TTargetQuery Unfold<TTargetQuery>() => Unfold().CloneAs<TTargetQuery>();

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>[] unionTraversals)
            where TTargetQuery : IGremlinQueryBase =>
            Union<TTargetQuery, TTargetQuery>(unionTraversals);

        private TReturnQuery Union<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>[] unionContinuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(unionContinuations)
                .Build(static (builder, unionTraversals) => builder
                    .AddStep(new UnionStep(unionTraversals
                        .ToImmutableArray()))
                    .WithNewProjection(unionTraversals
                        .LowestProjection())
                    .As<TReturnQuery>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> V(ImmutableArray<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new VStep(ids))
                    .WithNewProjection(Projection.Vertex)
                    .AsAuto(),
                ids);

        private GremlinQuery<TNewPropertyValue, object, object, IGremlinQueryBase> Value<TNewPropertyValue>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ValueStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<TNewPropertyValue>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMap<TNewElement>(ImmutableArray<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(new ValueMapStep(keys))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TNewElement>(),
                keys);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var projectionsArray = projections
                .ToArray<Expression>();

            var stringKeys = GetStringKeys(projectionsArray)
                .ToImmutableArray();

            return stringKeys.Length != projectionsArray.Length
                ? throw new ExpressionNotSupportedException($"One of the expressions in {nameof(ValueMap)} maps to a {nameof(T)}-token. Can't have special tokens in {nameof(ValueMap)}.")
                : this
                    .Continue()
                    .Build(
                        static (builder, stringKeys) => builder
                            .AddStep(new ValueMapStep(stringKeys))
                            .WithNewProjection(Projection.Value)
                            .AsAuto<TNewElement>(),
                        stringKeys);
        }

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForKeys<TValue>(IEnumerable<Key> keys)
        {
            var stepsArray = GetStepsForKeys(keys)
                .ToArray();

            return stepsArray.Length switch
            {
                0 => throw new ExpressionNotSupportedException(),
                1 => this
                    .Continue()
                    .Build(
                        static (builder, step) => builder
                            .AddStep(step)
                            .WithNewProjection(Projection.Value)
                            .AsAuto<TValue>(),
                        stepsArray[0]),
                _ => this
                    .Union(stepsArray
                        .Select(static step => new Func<GremlinQuery<T1, T2, T3, T4>, GremlinQuery<TValue, object, object, IGremlinQueryBase>>(__ => __
                            .Continue()
                            .Build(
                                static (builder, step) => builder
                                    .AddStep(step)
                                    .WithNewProjection(Projection.Value)
                                    .AsAuto<TValue>(),
                                step)))
                        .ToArray())
                    .Continue()
                    .Build(static builder => builder
                        .WithNewProjection(Projection.Value))
            };
        }

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(projections.Select(GetKey));

        private GremlinQuery<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta, IGremlinQueryBase> VertexProperties<TNewPropertyValue, TNewMeta>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta>(Projection.VertexProperty, projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object, IGremlinQueryBase> VertexProperties<TNewPropertyValue>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(Projection.VertexProperty, projections);

        private GremlinQuery<T1, T2, T3, T4> Where(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> filterContinuation) => this
            .Continue(ContinuationFlags.Filter)
            .With(filterContinuation)
            .Build(static (builder, filterTraversal) => filterTraversal.IsIdentity()
                ? builder
                : filterTraversal.IsNone() && filterTraversal.SideEffectSemantics == SideEffectSemantics.Read
                    ? builder.None()
                    : builder.Where(filterTraversal));

        private GremlinQuery<T1, T2, T3, T4> Where(Expression expression)
        {
            expression = expression.Strip();

            try
            {
                return expression switch
                {
                    ConstantExpression { Value: bool value } => value
                        ? this
                        : None(),

                    LambdaExpression lambdaExpression => Where(lambdaExpression.Body),

                    UnaryExpression { NodeType: ExpressionType.Not } unaryExpression => Not(
                        static (__, unaryExpression) => __.Where(unaryExpression.Operand),
                        unaryExpression),

                    BinaryExpression { NodeType: ExpressionType.OrElse } binaryExpression => Or(
                        static (__, binaryExpression) => __.Where(binaryExpression.Left),
                        static (__, binaryExpression) => __.Where(binaryExpression.Right),
                        binaryExpression),

                    BinaryExpression { NodeType: ExpressionType.AndAlso } binaryExpression => And(
                        static (__, binaryExpression) => __.Where(binaryExpression.Left),
                        static (__, binaryExpression) => __.Where(binaryExpression.Right),
                        binaryExpression),

                    _ when expression.TryParseWhereExpression() is { } whereExpression => whereExpression.Equals(WhereExpression.True)
                        ? this
                        : whereExpression.Equals(WhereExpression.False)
                            ? None()
                            : this
                                .Continue()
                                .Build(
                                    static (builder, whereExpression) => builder
                                        .WithSteps(
                                            static (steps, state) =>
                                            {
                                                var (outerQuery, whereExpression) = state;

                                                return outerQuery
                                                    .Where(steps, whereExpression.Left, whereExpression.Semantics, whereExpression.Right);
                                            },
                                            (builder.OuterQuery, whereExpression)),
                                    whereExpression),

                    _ => throw new ExpressionNotSupportedException()
                };
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(expression, ex);
            }
        }

        private GremlinQuery<T1, T2, T3, T4> Where<TProjection>(Expression<Func<T1, TProjection>> predicate, Func<IGremlinQuery<TProjection>, IGremlinQueryBase> propertyContinuation) => predicate.RefersToParameter(out _) && predicate.Body is MemberExpression memberExpression
             ? this
                 .Continue()
                 .With(
                     static (__, propertyContinuation) => propertyContinuation(__.CloneAs<IGremlinQuery<TProjection>>()),
                     propertyContinuation)
                 .Build(
                     static (builder, propertyTraversal, key) => builder
                         .AddStep(new HasTraversalStep(key, propertyTraversal)),
                     GetKey(memberExpression))
             : throw new ExpressionNotSupportedException(predicate);

        private Traversal Where(Traversal traversal, Expression left, ExpressionSemantics semantics, Expression right)
        {
            if (right.RefersToParameter(out _))
            {
                if (left.RefersToParameter(out _))
                {
                    if (left is MemberExpression && right is MemberExpression rightMember)
                    {
                        var newStepLabel = new StepLabel<T1>();
                        var newRightExpression = Expression.MakeMemberAccess(Expression.MakeMemberAccess(Expression.Constant(newStepLabel), typeof(StepLabel<T1>).GetProperty(nameof(StepLabel<T1>.Value))!), rightMember.Member);

                        return Where(
                            traversal
                                .Push(new AsStep(newStepLabel)),
                            left,
                            semantics,
                            newRightExpression);
                    }
                }
                else
                    return Where(traversal, right, semantics.Flip(), left);
            }
            else
            {
                var rightValue = right.GetValue() switch
                {
                    IEnumerable enumerable when enumerable is not ICollection && !Environment.SupportsType(enumerable.GetType()) => enumerable.Cast<object>().ToArray(),
                    var otherwise => otherwise
                };

                var maybeEffectivePredicate = Environment.Options
                    .GetValue(PFactory.PFactoryOption)
                    .TryGetP(semantics, rightValue, Environment);

                if (maybeEffectivePredicate?.WorkaroundLimitations(Environment) is { } effectivePredicate)
                {
                    if (effectivePredicate.EqualsConstant(false))
                        return traversal.Push(NoneStep.Instance);

                    if (left.RefersToParameter(out _))
                    {
                        switch (left)
                        {
                            case MemberExpression leftMemberExpression:
                            {
                                if (left.IsPropertyKey(out var sourceExpression1) && sourceExpression1 is ParameterExpression parameterExpression1)
                                {
                                    return traversal
                                        .Push(new FilterStep.ByTraversalStep(this
                                            .Where(
                                                KeyStep.Instance,
                                                parameterExpression1,
                                                semantics,
                                                right)));
                                }

                                if (left.IsPropertyValue(out var sourceExpression2) && sourceExpression2 is ParameterExpression && rightValue is not null and not StepLabel)
                                    return traversal.Push(new HasValueStep(effectivePredicate));

                                if (left.IsVertexPropertyLabel(out var sourceExpression3) && sourceExpression3 is ParameterExpression parameterExpression3)
                                {
                                    if (rightValue is StepLabel)
                                    {
                                        return traversal
                                            .Push(new FilterStep.ByTraversalStep(this
                                                .Where(
                                                    LabelStep.Instance,
                                                    parameterExpression3,
                                                    semantics,
                                                    right)));
                                    }

                                    return traversal.Push(new HasKeyStep(effectivePredicate));
                                }

                                if (left.IsVertexPropertyId(out var sourceExpression4))
                                {
                                    if (sourceExpression4 is MemberExpression memberExpression4 && GetKey(sourceExpression4).RawKey is string stringKey)
                                    {
                                        return traversal
                                            .Push(new FilterStep.ByTraversalStep(Traversal.Empty
                                                .Push(new PropertiesStep(ImmutableArray<string>.Empty.Add(stringKey)))
                                                .Push(new HasPredicateStep(T.Id, effectivePredicate))));
                                    }
                                }

                                var leftMemberExpressionKey = GetKey(leftMemberExpression);

                                // x => x.Name == P.xy(...)
                                if (rightValue is StepLabel)
                                {
                                    if (right is MemberExpression { Expression: { } rightMemberExpressionExpression } memberExpression && rightMemberExpressionExpression.IsStepLabelValue(out _))
                                    {
                                        traversal = traversal
                                            .Push(new WherePredicateStep(effectivePredicate))
                                            .Push(new WherePredicateStep.ByMemberStep(leftMemberExpressionKey));

                                        if (memberExpression.Member != leftMemberExpression.Member)
                                            traversal = traversal.Push(new WherePredicateStep.ByMemberStep(GetKey(memberExpression)));

                                        return traversal;
                                    }

                                    return traversal
                                        .Push(new HasTraversalStep(
                                            leftMemberExpressionKey,
                                            new WherePredicateStep(effectivePredicate)));
                                }

                                return traversal
                                    .Push(effectivePredicate
                                        .GetFilterStep(leftMemberExpressionKey));
                            }
                            case ParameterExpression:
                            {
                                if (rightValue is StepLabel)
                                {
                                    traversal = traversal.Push(new WherePredicateStep(effectivePredicate));

                                    if (right is MemberExpression { Expression: { } rightMemberExpressionExpression } memberExpression && rightMemberExpressionExpression.IsStepLabelValue(out _))
                                        traversal = traversal.Push(new WherePredicateStep.ByMemberStep(GetKey(memberExpression)));
                                }
                                else if (!effectivePredicate.EqualsConstant(true))
                                    traversal = traversal.Push(new IsStep(effectivePredicate));

                                return traversal;
                            }
                            case MethodCallExpression { Arguments: [ { } firstArgument] } methodCallExpression:
                            {
                                var targetExpression = methodCallExpression.Object?.Strip();

                                if (targetExpression != null && typeof(IDictionary<string, object>).IsAssignableFrom(targetExpression.Type) && methodCallExpression.Method.Name == "get_Item")
                                {
                                    if (firstArgument.Strip().GetValue() is string key)
                                        return traversal.Push(new HasPredicateStep(key, effectivePredicate));
                                }

                                break;
                            }
                            case UnaryExpression { NodeType: ExpressionType.ArrayLength, Operand: { } operandExpression }:
                            {
                                operandExpression = operandExpression.Strip();

                                if (operandExpression is MemberExpression { Expression: ParameterExpression parameterExpression })
                                {
                                    var operandExpressionKey = GetKey(operandExpression);

                                    if (Environment.GetCache().ModelTypes.Contains(parameterExpression.Type))
                                    {
                                        if (operandExpressionKey.RawKey is string stringKey)
                                        {
                                            if (!Environment.SupportsType(operandExpression.Type))
                                            {
                                                return traversal
                                                    .Push(new FilterStep.ByTraversalStep(Traversal
                                                        .Create(
                                                            3,
                                                            (stringKey, effectivePredicate),
                                                            static (steps, state) =>
                                                            {
                                                                var (stringKey, effectivePredicate) = state;

                                                                steps[0] = new PropertiesStep(ImmutableArray.Create(stringKey));
                                                                steps[1] = CountStep.Global;
                                                                steps[2] = new IsStep(effectivePredicate);
                                                            })));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        return traversal
                                            .Push(new FilterStep.ByTraversalStep(Traversal
                                                .Create(
                                                    3,
                                                    (operandExpressionKey, effectivePredicate),
                                                    static (steps, state) =>
                                                    {
                                                        var (leftMemberExpressionKey, effectivePredicate) = state;

                                                        steps[0] = new SelectKeysStep(ImmutableArray.Create(leftMemberExpressionKey));
                                                        steps[1] = CountStep.Local;
                                                        steps[2] = new IsStep(effectivePredicate);
                                                    })));
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (right.RefersToStepLabel(out var rightStepLabel, out var maybyRightStepLabelValueMemberExpression) && left.RefersToStepLabel(out var leftStepLabel, out var maybeLeftStepLabelValueMemberExpression))
                    {
                        traversal = traversal.Push(new WhereStepLabelAndPredicateStep(leftStepLabel, effectivePredicate));

                        if (maybeLeftStepLabelValueMemberExpression is not null || maybyRightStepLabelValueMemberExpression is not null)
                        {
                            traversal = traversal
                                .Push(new WherePredicateStep.ByMemberStep(maybeLeftStepLabelValueMemberExpression is { } leftStepLabelValueMemberExpression
                                    ? GetKey(leftStepLabelValueMemberExpression)
                                    : default(Key?)))
                                .Push(new WherePredicateStep.ByMemberStep(maybyRightStepLabelValueMemberExpression is { } rightStepLabelValueMemberExpression
                                    ? GetKey(rightStepLabelValueMemberExpression)
                                    : default(Key?)));
                        }

                        return traversal;
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
        {
            var stepLabel = new StepLabel<TSideEffect>();

            return continuation(
                WithSideEffect(stepLabel, value),
                stepLabel);
        }

        private GremlinQuery<object, object, object, IGremlinQueryBase> WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .WithSteps(
                        static (traversal, newSideEffectStep) =>
                        {
                            if (traversal.PeekOrDefault() is WithSideEffectStep { Label: { } existingLabel } && existingLabel == newSideEffectStep.Label)
                                traversal = traversal.Pop();

                            return traversal.Push(newSideEffectStep);
                        },
                        new WithSideEffectStep(tuple.label, tuple.value!))
                    .WithNewLabelProjections(
                        static (projections, tuple) => projections.Set(
                            tuple.label,
                            tuple.projection,
                            static (projections, projection) => projections.WithSideEffectLabelProjection(projection)),
                        (tuple.label, projection: builder.OuterQuery.Steps.Projection))
                    .AsAuto(),
                (label, value));
    }
}
