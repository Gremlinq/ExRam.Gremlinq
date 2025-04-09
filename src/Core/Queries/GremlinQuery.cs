#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using CommunityToolkit.HighPerformance;

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
    internal abstract class GremlinQueryBase
    {
        private delegate IGremlinQueryBase QueryContinuation(
            GremlinQueryBase existingQuery,
            Traversal? maybeNewTraversal,
            IImmutableDictionary<StepLabel, LabelProjections>? maybeNewLabelProjections);

        private static readonly ConcurrentDictionary<Type, QueryContinuation> QueryContinuations = new();
        private static readonly Type[] QueryGenericTypeDefinitionArguments = typeof(GremlinQuery<,,,>).GetGenericArguments();
        private static readonly QueryContinuation ObjectQueryContinuation = CreateQueryContinuation<object, object, object, IGremlinQueryBase>();
        private static readonly Type[] ImplementedInterfaces = typeof(GremlinQuery<,,,>).GetInterfaces().Append(typeof(GremlinQuery<,,,>)).ToArray();
        private static readonly MethodInfo TryCreateQueryContinuationMethod = typeof(GremlinQueryBase).GetMethod(nameof(CreateQueryContinuation), BindingFlags.NonPublic | BindingFlags.Static)!;

        protected GremlinQueryBase(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata)
        {
            Steps = steps;
            Metadata = metadata;
            Environment = environment;
            LabelProjections = labelProjections;
        }

        public override string ToString() => $"GremlinQuery(Steps.Count: {Steps.Count})";

        protected internal TTargetQuery CloneAs<TTargetQuery>(Traversal? maybeNewTraversal = null, IImmutableDictionary<StepLabel, LabelProjections>? maybeNewLabelProjections = null)
        {
            if (maybeNewTraversal == null && maybeNewLabelProjections == null && this is TTargetQuery targetQuery)
                return targetQuery;

            var queryFactory = typeof(TTargetQuery).IsGenericType
                ? QueryContinuations.GetOrAdd(
                    typeof(TTargetQuery),
                    static requestedType =>
                    {
                        var requestedTypeDefinition = requestedType.GetGenericTypeDefinition();
                        var queryTypeArguments = new Type?[QueryGenericTypeDefinitionArguments.Length];

                        for (var i = 0; i < ImplementedInterfaces.Length; i++)
                        {
                            if (ImplementedInterfaces[i] is { IsGenericType: true } queryImplementedInterface && queryImplementedInterface.GetGenericTypeDefinition() == requestedTypeDefinition)
                            {
                                var matchingImplementedInterfaceTypeArguments = queryImplementedInterface.GetGenericArguments();

                                for (var j = 0; j < QueryGenericTypeDefinitionArguments.Length; j++)
                                {
                                    for (var k = 0; k < matchingImplementedInterfaceTypeArguments.Length; k++)
                                    {
                                        if (matchingImplementedInterfaceTypeArguments[k] == QueryGenericTypeDefinitionArguments[j])
                                        {
                                            queryTypeArguments[j] = requestedType.GetGenericArguments()[k];

                                            break;
                                        }
                                    }

                                    queryTypeArguments[j] ??= j == 1 && queryTypeArguments[0]!.IsArray
                                        ? queryTypeArguments[0]!.GetElementType()!
                                        : QueryGenericTypeDefinitionArguments[j].GetGenericParameterConstraints().SingleOrDefault() ?? typeof(object);
                                }


                                return (QueryContinuation?)TryCreateQueryContinuationMethod
                                    .MakeGenericMethod(queryTypeArguments!)
                                    .Invoke(null, null)!;
                            }
                        }

                        throw new NotSupportedException();
                    })
                : ObjectQueryContinuation;

            return queryFactory(this, maybeNewTraversal, maybeNewLabelProjections) is TTargetQuery newTargetQuery
                ? newTargetQuery
                : throw new NotSupportedException($"Cannot create a query of type {typeof(TTargetQuery)}.");
        }

        private static QueryContinuation CreateQueryContinuation<T1, T2, T3, T4>() where T4 : IGremlinQueryBase => (existingQuery, maybeNewTraversal, maybeNewLabelProjections) => new GremlinQuery<T1, T2, T3, T4>(
            existingQuery.Environment,
            maybeNewTraversal ?? existingQuery.Steps,
            maybeNewLabelProjections ?? existingQuery.LabelProjections,
            existingQuery.Metadata);

        protected internal Traversal Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<object, object?> Metadata { get; }
        protected internal IImmutableDictionary<StepLabel, LabelProjections> LabelProjections { get; }
    }

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
                        : [];

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
#if NET5_0_OR_GREATER
                            CollectionsMarshal.AsSpan(droppableKeys))
#else
                            droppableKeys.ToArray())
#endif
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

        private GremlinQuery<T1, T2, T3, T4> And(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>> continuations) => And(this
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

        private GremlinQuery<string, object, object, IGremlinQueryBase> AsString() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(AsStringStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<string>());

        private GremlinQuery<int, object, object, IGremlinQueryBase> Length() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(LengthStep.Global)
                .WithNewProjection(Projection.Value)
                .AsAuto<int>());

        private GremlinQuery<T1, T2, T3, T4> ToLower() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ToLowerStep.Global)
                .WithNewProjection(Projection.Value));

        private GremlinQuery<T1, T2, T3, T4> ToUpper() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ToUpperStep.Global)
                .WithNewProjection(Projection.Value));

        private GremlinQuery<T1, T2, T3, T4> Trim() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TrimStep.Global)
                .WithNewProjection(Projection.Value));

        private GremlinQuery<T1, T2, T3, T4> TrimStart() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TrimStartStep.Global)
                .WithNewProjection(Projection.Value));

        private GremlinQuery<T1, T2, T3, T4> TrimEnd() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TrimEndStep.Global)
                .WithNewProjection(Projection.Value));

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

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> BothV<TNewElement>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothVStep.Instance)
                .OfType<GremlinQuery<T1, T2, T3, T4>, T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, false)
                .WithNewProjection(Projection.Vertex)
                .AsAuto<TNewElement>());

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

        private TReturnQuery Coalesce<TTargetQuery, TReturnQuery>(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>> continuations)
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

        private GremlinQuery<T1, T2, T3, T4> Concat(ReadOnlySpan<string> strings) => this
           .Continue()
           .Build(
               static (builder, strings) => builder
                   .AddStep(new ConcatStringsStep(strings.ToImmutableArray())),
               strings);

        private GremlinQuery<T1, T2, T3, T4> Concat(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<T1>>> stringTraversals) => this
            .Continue()
            .With(stringTraversals)
            .Build(static (builder, stringTraversals) => builder
                .AddStep(new ConcatTraversalsStep(stringTraversals
                    .ToImmutableArray())));

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
                    [key])
                .Drop());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> E<TNewElement>(ReadOnlySpan<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new EStep(ids.ToImmutableArray()))
                    .OfType<GremlinQuery<T1, T2, T3, T4>, T1, TNewElement>(builder.OuterQuery.Environment.Model.EdgesModel, true)
                    .WithNewProjection(Projection.Edge)
                    .AsAuto<TNewElement>(),
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

        private GremlinQuery<string, object, object, IGremlinQueryBase> Format(Expression<Func<T1, string>> stringInterpolationExpression)
        {
            if (stringInterpolationExpression is { Parameters: [var singleParameterExpression], Body: MethodCallExpression { Method: { Name: nameof(string.Format) } bodyMethod, Object: null, Arguments: [ConstantExpression { Value: string format }, ..] arguments } } && bodyMethod.DeclaringType == typeof(string))
            {
                return this
                    .Continue()
                    .Build(builder =>
                    {
                        var argumentExpressions = arguments is [_, NewArrayExpression newArrayExpression]
                            ? newArrayExpression.Expressions.ToArray().AsSpan()
                            : arguments.ToArray().AsSpan()[1..];

                        var newArguments = new object?[argumentExpressions.Length];

                        for (var i = 0; i < argumentExpressions.Length; i++)
                        {
                            newArguments[i] = argumentExpressions[i].Strip() switch
                            {
                                ParameterExpression parameterExpression when parameterExpression == singleParameterExpression => "%{_}",
                                MemberExpression { Expression: { } memberExpressionExpression } memberExpression when memberExpressionExpression == singleParameterExpression => "%{_}",
                                var other => other.GetValue()
                            };
                        }

                        builder = builder
                           .AddStep(new FormatStep(format, newArguments.ToImmutableArray()));

                        for (var i = 0; i < argumentExpressions.Length; i++)
                        {
                            if (argumentExpressions[i].Strip() is MemberExpression { Expression: { } memberExpressionExpression } memberExpression && memberExpressionExpression == singleParameterExpression)
                            {
                                if (GetKey(memberExpression) is { RawKey: string rawKey })
                                {
                                    builder = builder
                                        .AddStep(new FormatStep.By(new ValuesStep(ImmutableArray<string>.Empty.Add(rawKey))));
                                }
                                else
                                    throw new ExpressionNotSupportedException(stringInterpolationExpression);
                            }
                            else if (argumentExpressions[i].Strip() is { } parameterExpression && parameterExpression == singleParameterExpression)
                            {
                                builder = builder
                                    .AddStep(new FormatStep.By(IdentityStep.Instance));
                            }
                        }

                        return builder
                            .WithNewProjection(Projection.Value)
                            .AsAuto<string>();
                    });
            }

            throw new ExpressionNotSupportedException(stringInterpolationExpression);
        }

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

        private GremlinQuery<TNewElement, T2, T3, T4> Inject<TNewElement>(ReadOnlySpan<TNewElement> elements) => this
            .Continue()
            .Build(
                static (builder, elements) => builder
                    .AddStep(new InjectStep(
                        elements
                            .ToArray()  //TODO: Optimize
                            .Select(static x => (object)x!)
                            .ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TNewElement, T2, T3, T4>(),
                elements);

        private TNewQuery InOutV<TNewElement, TNewQuery>(Step step)
            where TNewQuery : IStartGremlinQuery => this
                .Continue()
                .Build(
                    static (builder, step) => builder
                        .AddStep(step)
                        .OfType<GremlinQuery<T1, T2, T3, T4>, T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, false)
                        .WithNewProjection(Projection.Vertex)
                        .As<TNewQuery>(),
                    step);

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
            where TTargetQuery : class, IGremlinQueryBase => loopBuilderTransformation(new LoopBuilder<TTargetQuery>(this)).Build();

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
                static (builder, tuple) => builder
                    .OfType<GremlinQuery<T1, T2, T3, T4>, T1, TNewElement>(tuple.model, tuple.force)
                    .As<TTargetQuery>(),
                (model, force));

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

        private GremlinQuery<T1, T2, T3, T4> Or(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>> continuations) => Or(this
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

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> OtherV<TNewElement>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OtherVStep.Instance)
                .OfType<GremlinQuery<T1, T2, T3, T4>, T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, false)
                .WithNewProjection(Projection.Vertex)
                .AsAuto<TNewElement>());

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

        private IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectMapResult<TResult>> continuation) => new ProjectBuilder(this)
            .Map(continuation)
            .Build();

        private IGremlinQuery<dynamic> Project(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectDynamicResult> continuation) => new ProjectBuilder(this)
            .Map(continuation)
            .Build();

        private IMapGremlinQuery<TResult> Project<TResult>(Func<IProjectBuilder<GremlinQuery<T1, T2, T3, T4>, T1>, IProjectTupleResult<TResult>> continuation)
            where TResult : ITuple => new ProjectBuilder(this)
                .Map(continuation)
                .Build();

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, ReadOnlySpan<LambdaExpression> projections) => Properties<TNewElement, TNewPropertyValue, TNewMeta>(projection, GetStringKeys(projections));

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, ReadOnlySpan<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys, projection) => builder
                    .AddStep(new PropertiesStep(keys.ToImmutableArray()))
                    .WithNewProjection(projection)
                    .AsAuto<TNewElement, TNewPropertyValue, TNewMeta>(),
                keys,
                projection);

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

        private GremlinQuery<T1, T2, T3, T4> Replace(string oldValue, string newValue) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new ReplaceStep(tuple.oldValue, tuple.newValue)),
                (oldValue, newValue));

        private GremlinQuery<T1, T2, T3, T4> Reverse() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ReverseStep.Instance));

        private IGremlinQuery<TSelectedElement> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel) => Select<IGremlinQuery<TSelectedElement>>(stepLabel);

        private TNewQuery Select<TNewQuery>(StepLabel stepLabel) where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SelectStepLabelStep(ImmutableArray.Create(tuple.stepLabel)))
                    .WithNewProjection(tuple.stepLabelProjection)
                    .As<TNewQuery>(),
                (stepLabel, stepLabelProjection: GetLabelProjection(stepLabel)));

        private TTargetQuery Select<TTargetQuery>(Expression expression) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, expression) =>
                {
                    var keys = ImmutableArray.Create(expression is LambdaExpression { Parameters: [var singleParameter], Body: { } lambdaBody } && lambdaBody.IsIndexerGet(out var target, out var indexerArgument) && target == singleParameter && indexerArgument.GetValue() is string indexerArgumentValue
                        ? indexerArgumentValue
                        : (Key)expression.AssumePropertyOrFieldMemberExpression().Member.Name);

                    return builder
                        .AddStep(new SelectKeysStep(keys))
                        .WithNewProjection(
                            static (projection, keys) => projection.If<TupleProjection>(tuple => tuple.Select(keys)),
                            keys)
                        .As<TTargetQuery>();
                },
                expression);

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

        private GremlinQuery<T1, T2, T3, T4> Substring(Range range) => this
            .Continue()
            .Build(
                static (builder, range) => builder
                    .AddStep(new SubstringStep(range, Scope.Global)),
                range);

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

        private GremlinQuery<Tree<TRoot>, object, object, IGremlinQueryBase> Tree<TRoot>() where TRoot : notnull => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TreeStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<Tree<TRoot>>());

        private IGremlinQuery<TTree> Tree<TTree>(Func<ITreeBuilder, ITreeBuilderResult<TTree>> continuation)
            where TTree : ITree => new TreeBuilder<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(this)
                .Map(continuation)
                .Build();

        private GremlinQuery<T1, T2, T3, T4> Unfold() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(UnfoldStep.Instance)
                .WithNewProjection(static projection => projection.If<ArrayProjection>(static array => array.Unfold())));

        private TTargetQuery Unfold<TTargetQuery>() => Unfold().CloneAs<TTargetQuery>();

        private TReturnQuery Union<TTargetQuery, TReturnQuery>(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>> unionContinuations)
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

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> V<TNewElement>(ReadOnlySpan<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new VStep(ids.ToImmutableArray()))
                    .OfType<GremlinQuery<T1, T2, T3, T4>, T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, true)
                    .WithNewProjection(Projection.Vertex)
                    .AsAuto<TNewElement>(),
                ids);

        private GremlinQuery<TNewPropertyValue, object, object, IGremlinQueryBase> Value<TNewPropertyValue>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ValueStep.Instance)
                .WithNewProjection(Projection.Value)
                .AsAuto<TNewPropertyValue>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMap<TNewElement>(ReadOnlySpan<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(new ValueMapStep(keys.ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TNewElement>(),
                keys);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMapForExpressions<TNewElement>(ReadOnlySpan<LambdaExpression> projections) => this
            .Continue()
            .Build(
                static (builder, stringKeys) => builder
                    .AddStep(new ValueMapStep(stringKeys.ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TNewElement>(),
                GetStringKeys(projections));

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForStringKeys<TValue>(ReadOnlySpan<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(keys is []
                        ? ValuesStep.All
                        : new ValuesStep(keys.ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .AsAuto<TValue>(),
                keys);

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForProjections<TValue>(ReadOnlySpan<LambdaExpression> projections) => projections is []
            ? ValuesForStringKeys<TValue>([])
            : this
                .Continue()
                .Build(
                    static (builder, projections) =>
                    {
                        var steps = builder.OuterQuery.GetStepsForKeys(projections);

                        if (steps is [])
                            throw new ExpressionNotSupportedException();

                        if (steps is [var singleStep])
                        {
                            builder = builder
                                .AddStep(singleStep);
                        }
                        else
                        {
                            var traversalArray = new Traversal[steps.Length];

                            for (var i = 0; i < steps.Length; i++)
                            {
                                traversalArray[i] = steps[i];
                            }

                            builder = builder
                                .AddStep(new UnionStep(
#if NET8_0_OR_GREATER
                                    ImmutableCollectionsMarshal.AsImmutableArray(traversalArray)));
#else
                                    traversalArray.ToImmutableArray()));
#endif
                        }

                        return builder
                            .WithNewProjection(Projection.Value)
                            .AsAuto<TValue>();
                    },
                    projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta, IGremlinQueryBase> VertexProperties<TNewPropertyValue, TNewMeta>(ReadOnlySpan<LambdaExpression> projections) => Properties<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta>(Projection.VertexProperty, projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object, IGremlinQueryBase> VertexProperties<TNewPropertyValue>(ReadOnlySpan<LambdaExpression> projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(Projection.VertexProperty, projections);

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
                                    if (sourceExpression4 is MemberExpression memberExpression4 && GetKey(memberExpression4).RawKey is string stringKey)
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
                    else if (right.RefersToStepLabel(out var rightStepLabel, out var maybeRightStepLabelValueMemberExpression) && left.RefersToStepLabel(out var leftStepLabel, out var maybeLeftStepLabelValueMemberExpression))
                    {
                        traversal = traversal.Push(new WhereStepLabelAndPredicateStep(leftStepLabel, effectivePredicate));

                        if (maybeLeftStepLabelValueMemberExpression is not null || maybeRightStepLabelValueMemberExpression is not null)
                        {
                            traversal = traversal
                                .Push(new WherePredicateStep.ByMemberStep(maybeLeftStepLabelValueMemberExpression is { } leftStepLabelValueMemberExpression
                                    ? GetKey(leftStepLabelValueMemberExpression)
                                    : default(Key?)))
                                .Push(new WherePredicateStep.ByMemberStep(maybeRightStepLabelValueMemberExpression is { } rightStepLabelValueMemberExpression
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
