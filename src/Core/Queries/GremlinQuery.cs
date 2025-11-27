#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

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

        protected internal Traversal Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
        protected internal IImmutableDictionary<object, object?> Metadata { get; }
        protected internal IImmutableDictionary<StepLabel, LabelProjections> LabelProjections { get; }
    }

    internal sealed partial class GremlinQuery<T1, T2, T3, T4> : GremlinQueryBase
    {
#if NET8_0_OR_GREATER
        [InlineArray(8)]
        private struct Buffer8
        {
            public object _element0;
        }
#endif

        public GremlinQuery(
            IGremlinQueryEnvironment environment,
            Traversal steps,
            IImmutableDictionary<StepLabel, LabelProjections> labelProjections,
            IImmutableDictionary<object, object?> metadata) : base(environment, steps, labelProjections, metadata)
        {

        }

        private TTargetQuery CloneAs<TTargetQuery>() where TTargetQuery : IStartGremlinQuery
            => this is TTargetQuery targetQuery
                ? targetQuery
                : this
                    .Continue()
                    .Build(static builder => builder
                        .BuildAs<TTargetQuery>());

        private GremlinQuery<TEdge, T1, object, IGremlinQueryBase> AddE<TEdge>(TEdge newEdge) => this
            .Continue()
            .Build(
                static (builder, newEdge) => builder
                    .AddStep(new AddEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetCache().GetLabel(newEdge!.GetType())))
                    .WithNewProjection(Projection.Edge)
                    .BuildAuto<TEdge, T1>(),
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
                if (T.Id.Equals(key.RawKey) && !Environment.FeatureSet.Supports(VertexFeatures.UserSuppliedIds))
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
                            droppableKeys.AsSpan())
                        .Drop());
            }

            return ret
                .Continue()
                .Build(
                    static (builder, propertySteps) => builder
                        .AddSteps(propertySteps
                            .AsSpan()
                            .Cast()
                            .To<Step>()),
                    propertySteps)
                .BuildAuto<T1, T2, T3, T4>();
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
                            .BuildAs<TTargetQuery>();
                    },
                    (step, maybeProjectionTransformation));

        private GremlinQuery<TVertex, object, object, IGremlinQueryBase> AddV<TVertex>(TVertex vertex) => this
            .Continue()
            .Build(
                static (builder, vertex) => builder
                    .AddStep(new AddVStep(builder.OuterQuery.Environment.Model.VerticesModel.GetCache().GetLabel(vertex!.GetType())))
                    .WithNewProjection(Projection.Vertex)
                    .BuildAuto<TVertex>(),
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
                            (tuple.stepLabel, projection: builder.OuterQuery.Steps.Projection.Fold()))
                        .BuildAuto<T1, T2, T3, T4>(),
                    (scope, stepLabel));

        private GremlinQuery<T1, T2, T3, T4> And<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation1, Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation2, TState state) => this
            .Continue(ContinuationFlags.Filter)
            .With(continuation1, continuation2, state)
            .Build(static (builder, continuation1, continuation2) => builder
                .And([continuation1, continuation2])
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> And(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>> continuations) => this
            .Continue(ContinuationFlags.Filter)
            .With(continuations)
            .Build((builder, traversals) => builder
                .And(traversals.Span)
                .BuildAuto<T1, T2, T3, T4>());

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
                        (stepLabel, otherProjection: builder.OuterQuery.Steps.Projection))
                    .BuildAuto<T1, T2, T3, T4>(),
                stepLabel);

        private GremlinQuery<string, object, object, IGremlinQueryBase> AsString() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(AsStringStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<string>());

        private GremlinQuery<int, object, object, IGremlinQueryBase> Length() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(LengthStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<int>());

        private GremlinQuery<T1, T2, T3, T4> ToLower() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ToLowerStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> ToUpper() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ToUpperStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> Trim() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TrimStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> TrimStart() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TrimStartStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> TrimEnd() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TrimEndStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> Barrier() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BarrierStep.Instance)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Both() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothStep.NoLabels)
                .BuildAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Both<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .BuildAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> BothE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .BuildAuto());

        private GremlinQuery<TEdge, object, object, IGremlinQueryBase> BothE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .BuildAuto<TEdge>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> BothV<TNewElement>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothVStep.Instance)
                .OfType<T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, false)
                .WithNewProjection(Projection.Vertex)
                .BuildAuto<TNewElement>());

        private GremlinQuery<TSelectedElement, TArrayItem, object, TQuery> Cap<TSelectedElement, TArrayItem, TQuery>(StepLabel<IArrayGremlinQuery<TSelectedElement, TArrayItem, TQuery>, TSelectedElement> stepLabel) where TQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new CapStep(stepLabel))
                    .WithNewProjection(static projection => projection.Fold())
                    .BuildAuto<TSelectedElement, TArrayItem, object, TQuery>(),
                stepLabel);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> Cast<TNewElement>() => this as GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ?? this
            .Continue()
            .Build(static builder => builder
                .BuildAuto<TNewElement>());

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Expression<Func<T1, bool>> predicate, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueChoice, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseChoice = null)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Choose<TTrueQuery, TFalseQuery, TTargetQuery>(
                    __ => __
                        .Where(predicate),
                    trueChoice,
                    maybeFalseChoice);

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> predicateContinuation, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueContinuation, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseContinuation = null)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(predicateContinuation)
                .Build(
                    static (_, traversal, tuple) => tuple.@this
                        .Choose<TTrueQuery, TFalseQuery, TTargetQuery>(traversal, tuple.trueContinuation, tuple.maybeFalseContinuation),
                    (trueContinuation, maybeFalseContinuation, @this: this));

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Traversal predicateTraversal, Func<GremlinQuery<T1, T2, T3, T4>, TTrueQuery> trueContinuation, Func<GremlinQuery<T1, T2, T3, T4>, TFalseQuery>? maybeFalseContinuation = null)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(trueContinuation)
                .Build(
                    static (builder, trueTraversal, state) =>
                    {
                        var (predicateTraversal, maybeFalseContinuation, @this) = state;

                        return maybeFalseContinuation is { } falseContinuation
                            ? @this
                                .Continue()
                                .With(falseContinuation)
                                .Build(
                                    static (builder, falseTraversal, state) =>
                                    {
                                        var (predicateTraversal, trueTraversal) = state;

                                        return builder
                                            .AddStep(predicateTraversal is [IsStep isStep]
                                                ? new ChoosePredicateStep(
                                                    isStep.Predicate,
                                                    trueTraversal,
                                                    falseTraversal)
                                                : new ChooseTraversalStep(
                                                    predicateTraversal,
                                                    trueTraversal,
                                                    falseTraversal))
                                            .WithNewProjection(
                                                static (_, state) => state.falseTraversal.Projection
                                                    .Lowest(state.trueTraversal.Projection),
                                                (falseTraversal, trueTraversal))
                                            .BuildAs<TTargetQuery>();
                                    },
                                    (predicateTraversal, trueTraversal))
                            : builder
                                .AddStep(predicateTraversal is [IsStep isStep]
                                    ? new ChoosePredicateStep(
                                        isStep.Predicate,
                                        trueTraversal)
                                    : new ChooseTraversalStep(
                                        state.predicateTraversal,
                                        trueTraversal))
                                .WithNewProjection(
                                    static (projection, otherProjection) => projection
                                        .Lowest(otherProjection),
                                    trueTraversal.Projection)
                                .BuildAs<TTargetQuery>();
                    },
                    (predicateTraversal, maybeFalseContinuation, @this: this));

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<T1, T2, T3, T4>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase => continuation
                .Invoke(new ChooseBuilder<GremlinQuery<T1, T2, T3, T4>, object>(this))
                .Build();

        private TReturnQuery Coalesce<TTargetQuery, TReturnQuery>(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>> continuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(continuations)
                .Build(static (builder, traversalsMemory) =>
                {
                    var traversals = traversalsMemory.Span;

                    if (traversals.Length == 0)
                        throw new ArgumentException("Coalesce must have at least one sub-query.");

                    if (!traversals.All(static traversal => traversal.IsIdentity()))
                    {
                        if (traversals is [var singleTraversal])
                        {
                            builder = builder
                                .AddSteps(singleTraversal.Steps)
                                .WithNewProjection(singleTraversal.Projection);
                        }
                        else
                        {
                            builder = builder
                                .AddStep(new CoalesceStep(traversalsMemory
                                    .UnsafeToImmutableArray()))
                                .WithNewProjection(traversals
                                    .LowestProjection());
                        }
                    }

                    return builder
                        .BuildAs<TReturnQuery>();
                });

        private GremlinQuery<T1, T2, T3, T4> Coin(double probability) => this
            .Continue()
            .Build(
                static (builder, probability) => builder
                    .AddStep(new CoinStep(probability))
                    .BuildAuto<T1, T2, T3, T4>(),
                probability);

        private TTargetQuery ConfigureMetadata<TTargetQuery>(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation)
            where TTargetQuery : IStartGremlinQuery => this
                .Continue()
                .Build(builder => builder
                    .WithMetadata(metadataTransformation)
                    .BuildAs<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> Concat(ReadOnlySpan<string> strings) => this
            .Continue()
            .Build(
                static (builder, strings) => builder
                    .AddStep(new ConcatStringsStep(strings.ToImmutableArray()))
                    .BuildAuto<T1, T2, T3, T4>(),
                strings);

        private GremlinQuery<T1, T2, T3, T4> Concat(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<T1>>> stringTraversals) => this
            .Continue()
            .With(stringTraversals)
            .Build(static (builder, traversals) => builder
                .AddStep(new ConcatTraversalsStep(traversals
                    .UnsafeToImmutableArray()))
                .BuildAuto<T1, T2, T3, T4>());

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
                        .BuildAs<TTargetQuery>(),
                    (transformation, maybeProjectionTransformation));

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> Constant<TValue>(TValue constant) => this
            .Continue()
            .Build(
                static (builder, constant) => builder
                    .AddStep(new ConstantStep(constant!))
                    .WithNewProjection(Projection.Value)
                    .BuildAuto<TValue>(),
                constant);

        private GremlinQuery<long, object, object, IGremlinQueryBase> Count(Scope scope) => this
            .Continue()
            .Build(
                static (builder, scope) => builder
                    .AddStep(Scope.Global.Equals(scope)
                        ? CountStep.Global
                        : CountStep.Local)
                    .WithNewProjection(Projection.Value)
                    .BuildAuto<long>(),
                scope);

        private GremlinQuery<long, object, object, IGremlinQueryBase> CountGlobal() => Count(Scope.Global);

        private GremlinQuery<long, object, object, IGremlinQueryBase> CountLocal() => Count(Scope.Local);

        private GremlinQuery<T1, T2, T3, T4> CyclicPath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(CyclicPathStep.Instance)
                .BuildAuto<T1, T2, T3, T4>());

        private string Debug() => Environment.Debugger
            .Debug(
                Environment.Serializer
                    .TransformTo<Bytecode>()
                    .From(this, Environment),
                Environment);

        private GremlinQuery<T1, T2, T3, T4> DedupGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Global)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> DedupLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Local)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Drop() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DropStep.Instance)
                .WithNewProjection(Projection.Empty)
                .BuildAuto());

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
                    .OfType<T1, TNewElement>(builder.OuterQuery.Environment.Model.EdgesModel, true)
                    .WithNewProjection(Projection.Edge)
                    .BuildAuto<TNewElement>(),
                ids);

        private GremlinQuery<string, object, object, IGremlinQueryBase> Explain() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ExplainStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<string>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Fail(string? message = null) => this
            .Continue()
            .Build(
                static (builder, message) => builder
                    .AddStep(message is { } actualMessage
                        ? new FailStep(actualMessage)
                        : FailStep.NoMessage)
                    .WithNewProjection(Projection.Empty)
                    .BuildAuto(),
                message);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => builder
                .AddStep(new FlatMapStep(innerTraversal))
                .WithNewProjection(innerTraversal.Projection)
                .BuildAs<TTargetQuery>());

        private GremlinQuery<T1[], T1, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() where TNewFoldedQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(FoldStep.Instance)
                .WithNewProjection(static projection => projection.Fold())
                .BuildAuto<T1[], T1, object, TNewFoldedQuery>());

        private GremlinQuery<T1, object, object, IGremlinQueryBase> ForceElement() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(static _ => _.Highest(Projection.Element))
                .BuildAuto<T1>());

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
                           .AddStep(new FormatStep(format, newArguments.UnsafeToImmutableArray()));

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
                            .BuildAuto<string>();
                    });
            }

            throw new ExpressionNotSupportedException(stringInterpolationExpression);
        }

        private GremlinQuery<T1, TNewOutVertex, TInVertex, IGremlinQueryBase> From<TNewOutVertex, TInVertex>(Func<GremlinQuery<TInVertex, T2, T3, T4>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexContinuation) => this
            .Continue<TInVertex, T2, T3, T4>()
            .With(fromVertexContinuation)
            .Build(static (builder, fromVertexTraversal) => builder
                .AddStep(new AddEStep.FromTraversalStep(fromVertexTraversal))
                .BuildAuto<T1, TNewOutVertex, TInVertex>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, IGremlinQueryBase> From<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel<TNewOutVertex> label) => this
            .Continue()
            .Build(
                static (builder, label) => builder
                    .AddStep(new AddEStep.FromLabelStep(label))
                    .BuildAuto<TNewElement, TNewOutVertex, TNewInVertex>(),
                label);

        private IMapGremlinQuery<IDictionary<TKey, TValue>> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>, IGroupBuilderWithKeyAndValue<TKey, TValue>> projection) => projection
            .Invoke(new GroupBuilder<object, object>(this))
            .Build();

        private IMapGremlinQuery<IDictionary<TKey, T1[]>> Group<TKey>(Func<IGroupBuilder<GremlinQuery<T1, T2, T3, T4>>, IGroupBuilderWithKey<IGremlinQueryBase<T1>, TKey>> projection) => new GroupBuilder<object, object>(this)
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
                .BuildAuto());

        private GremlinQuery<T1, T2, T3, T4> Identity() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(IdentityStep.Instance)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> In() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InStep.NoLabels)
                .BuildAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> In<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .BuildAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> InE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .BuildAuto());

        private GremlinQuery<TEdge, object, T1, IGremlinQueryBase> InE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .BuildAuto<TEdge, object, T1>());

        private GremlinQuery<TNewElement, T2, T3, T4> Inject<TNewElement>(ReadOnlySpan<TNewElement> elements) => this
            .Continue()
            .Build(
                static (builder, elements) =>
                {
                    var injects = new object[elements.Length];

                    for (var i = 0; i < elements.Length; i++)
                    {
                        injects[i] = elements[i]!;
                    }

                    return builder
                        .AddStep(new InjectStep(injects.UnsafeToImmutableArray()))
                        .WithNewProjection(Projection.Value)
                        .BuildAuto<TNewElement, T2, T3, T4>();
                },
                elements);

        private TNewQuery InOutV<TNewElement, TNewQuery>(Step step)
            where TNewQuery : IStartGremlinQuery => this
                .Continue()
                .Build(
                    static (builder, step) => builder
                        .AddStep(step)
                        .OfType<T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, false)
                        .WithNewProjection(Projection.Vertex)
                        .BuildAs<TNewQuery>(),
                    step);

        private GremlinQuery<string, object, object, IGremlinQueryBase> Key() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(KeyStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<string>());

        private GremlinQuery<string, object, object, IGremlinQueryBase> Label() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(LabelStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<string>());

        private GremlinQuery<T1, T2, T3, T4> LimitGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? LimitStep.LimitGlobal1
                        : new LimitStep(count, Scope.Global))
                    .BuildAuto<T1, T2, T3, T4>(),
                count);

        private GremlinQuery<T1, T2, T3, T4> LimitLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                            ? LimitStep.LimitLocal1Workaround
                            : LimitStep.LimitLocal1
                        : new LimitStep(count, Scope.Local))
                    .BuildAuto<T1, T2, T3, T4>(),
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
                    .BuildAs<TTargetQuery>();
            });

        private TTargetQuery Loop<TTargetQuery>(Func<IStartLoopBuilder<TTargetQuery>, IFinalLoopBuilder<TTargetQuery>> loopBuilderTransformation)
            where TTargetQuery : class, IGremlinQueryBase => loopBuilderTransformation(new LoopBuilder<TTargetQuery>(this)).Build();

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => innerTraversal.IsIdentity()
                ? builder
                    .BuildAs<TTargetQuery>()
                : builder
                    .AddStep(new MapStep(innerTraversal))
                    .WithNewProjection(innerTraversal.Projection)
                    .BuildAs<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> MaxGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MaxStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private TNewQuery MaxLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MaxStep.Local)
                .BuildAs<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> MeanGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MeanStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private TNewQuery MeanLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MeanStep.Local)
                .BuildAs<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> MinGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MinStep.Global)
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private TNewQuery MinLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(MinStep.Local)
                .BuildAs<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> None() => this
            .Continue()
            .Build(static builder => builder
                .None()
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> Not<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation, TState state) => this
            .Continue()
            .With(continuation, state)
            .Build(static (builder, innerTraversal) => innerTraversal.IsIdentity()
                ? builder
                    .None()
                : innerTraversal.IsNone()
                    ? builder
                    : builder
                        .AddStep(new NotStep(innerTraversal)))
            .BuildAuto<T1, T2, T3, T4>();

        private TTargetQuery OfType<TNewElement, TTargetQuery>(IGraphElementModel model, bool force = false) where TTargetQuery : IStartGremlinQuery => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .OfType<T1, TNewElement>(tuple.model, tuple.force)
                    .BuildAs<TTargetQuery>(),
                (model, force));

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(optionalTraversal)
            .Build(static (builder, continuedTraversal) => builder
                .AddStep(new OptionalStep(continuedTraversal))
                .WithNewProjection(
                    static (projection, otherProjection) => projection.Lowest(otherProjection),
                    continuedTraversal.Projection)
                .BuildAs<TTargetQuery>());

        private GremlinQuery<T1, T2, T3, T4> Or<TState>(Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation1, Func<GremlinQuery<T1, T2, T3, T4>, TState, IGremlinQueryBase> continuation2, TState state) => this
            .Continue(ContinuationFlags.Filter)
            .With(continuation1, continuation2, state)
            .Build(static (builder, continuation1, continuation2) => builder
                .Or([continuation1, continuation2])
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> Or(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase>> continuations) => this
            .Continue(ContinuationFlags.Filter)
            .With(continuations)
            .Build(static (builder, traversals) => builder
                .Or(traversals.Span)
                .BuildAuto<T1, T2, T3, T4>());

        private TTargetQuery Order<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => projection(new OrderBuilder(this)).Build();

        private TTargetQuery OrderGlobal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Global)
                    .BuildAuto<T1, T2, T3, T4>()
                    .Order(projection),
                projection);

        private TTargetQuery OrderLocal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<T1> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Local)
                    .BuildAuto<T1, T2, T3, T4>()
                    .Order(projection),
                projection);

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> OtherV<TNewElement>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OtherVStep.Instance)
                .OfType<T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, false)
                .WithNewProjection(Projection.Vertex)
                .BuildAuto<TNewElement>());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Out() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutStep.NoLabels)
                .BuildAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> Out<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .BuildAuto());

        private GremlinQuery<object, object, object, IGremlinQueryBase> OutE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .BuildAuto());

        private GremlinQuery<TEdge, T1, object, IGremlinQueryBase> OutE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .BuildAuto<TEdge, T1>());

        private GremlinQuery<Path, object, object, IGremlinQueryBase> Path() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(PathStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<Path>());

        private GremlinQuery<string, object, object, IGremlinQueryBase> Profile() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ProfileStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<string>());

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

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, ReadOnlySpan<LambdaExpression> projections) => PropertiesImpl<TNewElement, TNewPropertyValue, TNewMeta>(projection, GetStringKeyArray(projections));

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, ReadOnlySpan<string> keys) => PropertiesImpl<TNewElement, TNewPropertyValue, TNewMeta>(projection, keys.ToImmutableArray());

        private GremlinQuery<TNewElement, TNewPropertyValue, TNewMeta, IGremlinQueryBase> PropertiesImpl<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, ImmutableArray<string> keys) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(tuple.keys.IsEmpty
                        ? PropertiesStep.All
                        : new PropertiesStep(tuple.keys))
                    .WithNewProjection(tuple.projection)
                    .BuildAuto<TNewElement, TNewPropertyValue, TNewMeta>(),
                (keys, projection));

        private GremlinQuery<T1, T2, T3, T4> Property(LambdaExpression projection, object? value) => Property(GetKey(projection), value);

        private GremlinQuery<T1, T2, T3, T4> Property(Key key, object? value) => this
            .Continue()
            .Build(
                static (builder, tuple) =>
                {
                    var (key, value, @this) = tuple;

                    if (value == null)
                    {
                        return key.RawKey is string stringKey
                            ? @this.DropProperties(stringKey)
                            : throw new InvalidOperationException("Can't set a special property to null.");
                    }

                    foreach (var propertyStep in @this.GetPropertySteps(key, value, builder.OuterQuery.Steps.Projection == Projection.Vertex))
                    {
                        builder = builder.AddStep(propertyStep);
                    }

                    return builder
                        .BuildAuto<T1, T2, T3, T4>();
                },
                (key, value, @this: this));

        private GremlinQuery<T1, T2, T3, T4> Property(Key key, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (_, valueTraversal, tuple) => tuple.@this.Property(tuple.key, valueTraversal),
                (key, @this: this));

        private GremlinQuery<T1, T2, T3, T4> Property(LambdaExpression projection, Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (_, valueTraversal, tuple) => tuple.@this.Property(tuple.projection, valueTraversal),
                (projection, @this: this));

        private GremlinQuery<T1, T2, T3, T4> Range(long low, long high, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(Scope.Local.Equals(tuple.scope) && tuple.high - tuple.low == 1 && builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                        ? new MapStep(Traversal.Empty.Push(
                            UnfoldStep.Instance,
                            new RangeStep(tuple.low, tuple.high, Scope.Global),
                            FoldStep.Instance))
                        : new RangeStep(tuple.low, tuple.high, tuple.scope))
                    .BuildAuto<T1, T2, T3, T4>(),
                (low, high, scope));

        private GremlinQuery<T1, T2, T3, T4> RangeGlobal(long low, long high) => Range(low, high, Scope.Global);

        private GremlinQuery<T1, T2, T3, T4> RangeLocal(long low, long high) => Range(low, high, Scope.Local);

        private GremlinQuery<T1, T2, T3, T4> Replace(string oldValue, string newValue) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new ReplaceStep(tuple.oldValue, tuple.newValue))
                    .BuildAuto<T1, T2, T3, T4>(),
                (oldValue, newValue));

        private GremlinQuery<T1, T2, T3, T4> Reverse() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ReverseStep.Instance)
                .BuildAuto<T1, T2, T3, T4>());

        private IGremlinQuery<TSelectedElement> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel) => Select<IGremlinQuery<TSelectedElement>>(stepLabel);

        private TNewQuery Select<TNewQuery>(StepLabel stepLabel) where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SelectStepLabelStep(ImmutableArray.Create(tuple.stepLabel)))
                    .WithNewProjection(tuple.stepLabelProjection)
                    .BuildAs<TNewQuery>(),
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
                        .BuildAs<TTargetQuery>();
                },
                expression);

        private GremlinQuery<T1, T2, T3, T4> SideEffect(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> sideEffectContinuation) => this
            .Continue()
            .With(sideEffectContinuation)
            .Build(static (builder, traversal) => builder
                .AddStep(new SideEffectStep(traversal))
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> SimplePath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(SimplePathStep.Instance)
                .BuildAuto<T1, T2, T3, T4>());

        private GremlinQuery<T1, T2, T3, T4> Skip(long count, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SkipStep(tuple.count, tuple.scope))
                    .BuildAuto<T1, T2, T3, T4>(),
                (count, scope));

        private GremlinQuery<T1, T2, T3, T4> Substring(Range range) => this
            .Continue()
            .Build(
                static (builder, range) => builder
                    .AddStep(new SubstringStep(range, Scope.Global))
                    .BuildAuto<T1, T2, T3, T4>(),
                range);

        private GremlinQuery<T1, T2, T3, T4> SumGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Global))
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1, T2, T3, T4>());

        private TNewQuery SumLocal<TNewQuery>() where TNewQuery : IGremlinQueryBase => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Local))
                .WithNewProjection(Projection.Value)
                .BuildAs<TNewQuery>());

        private GremlinQuery<T1, T2, T3, T4> TailGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(new TailStep(count, Scope.Global))
                    .BuildAuto<T1, T2, T3, T4>(),
                count);

        private GremlinQuery<T1, T2, T3, T4> TailLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.WorkaroundRangeInconsistencies)
                            ? TailStep.TailLocal1Workaround
                            : TailStep.TailLocal1
                        : new TailStep(count, Scope.Local))
                    .BuildAuto<T1, T2, T3, T4>(),
                count);

        private GremlinQuery<T1, TOutVertex, TNewInVertex, IGremlinQueryBase> To<TOutVertex, TNewInVertex>(Func<GremlinQuery<TOutVertex, T2, T3, T4>, IVertexGremlinQueryBase<TNewInVertex>> toVertexContinuation) => this
            .Continue<TOutVertex, T2, T3, T4>()
            .With(toVertexContinuation)
            .Build(static (builder, toVertexTraversal) => builder
                .AddStep(new AddEStep.ToTraversalStep(toVertexTraversal))
                .BuildAuto<T1, TOutVertex, TNewInVertex>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, IGremlinQueryBase> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AddEStep.ToLabelStep(stepLabel))
                    .BuildAuto<TNewElement, TNewOutVertex, TNewInVertex>(),
                stepLabel);

        private GremlinQuery<Tree<TRoot>, object, object, IGremlinQueryBase> Tree<TRoot>() where TRoot : notnull => this
            .Continue()
            .Build(static builder => builder
                .AddStep(TreeStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<Tree<TRoot>>());

        private IGremlinQuery<TTree> Tree<TTree>(Func<ITreeBuilder, ITreeBuilderResult<TTree>> continuation)
            where TTree : ITree => new TreeBuilder<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(this)
                .Map(continuation)
                .Build();

        private TTargetQuery Unfold<TTargetQuery>()
            where TTargetQuery : IStartGremlinQuery => this
                .Continue()
                .Build(static builder => builder
                    .AddStep(UnfoldStep.Instance)
                    .WithNewProjection(static projection => projection.If<ArrayProjection>(static array => array.Unfold()))
                    .BuildAs<TTargetQuery>());

        private TReturnQuery Union<TTargetQuery, TReturnQuery>(ReadOnlySpan<Func<GremlinQuery<T1, T2, T3, T4>, TTargetQuery>> unionContinuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(unionContinuations)
                .Build(static (builder, traversals) => builder
                    .AddStep(new UnionStep(traversals
                        .UnsafeToImmutableArray()))
                    .WithNewProjection(traversals.Span
                        .LowestProjection())
                    .BuildAs<TReturnQuery>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> V<TNewElement>(ReadOnlySpan<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new VStep(ids.ToImmutableArray()))
                    .OfType<T1, TNewElement>(builder.OuterQuery.Environment.Model.VerticesModel, true)
                    .WithNewProjection(Projection.Vertex)
                    .BuildAuto<TNewElement>(),
                ids);

        private GremlinQuery<TNewPropertyValue, object, object, IGremlinQueryBase> Value<TNewPropertyValue>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ValueStep.Instance)
                .WithNewProjection(Projection.Value)
                .BuildAuto<TNewPropertyValue>());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMap<TNewElement>(ReadOnlySpan<string> keys) => ValueMapImpl<TNewElement>(keys.ToImmutableArray());

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMapForExpressions<TNewElement>(ReadOnlySpan<LambdaExpression> projections) => ValueMapImpl<TNewElement>(GetStringKeyArray(projections));

        private GremlinQuery<TNewElement, object, object, IGremlinQueryBase> ValueMapImpl<TNewElement>(ImmutableArray<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(keys.IsEmpty
                        ? ValueMapStep.All
                        : new ValueMapStep(keys))
                    .WithNewProjection(Projection.Value)
                    .BuildAuto<TNewElement>(),
                keys);

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForStringKeys<TValue>(ReadOnlySpan<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(keys is []
                        ? ValuesStep.All
                        : new ValuesStep(keys.ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .BuildAuto<TValue>(),
                keys);

        private GremlinQuery<TValue, object, object, IGremlinQueryBase> ValuesForProjections<TValue>(ReadOnlySpan<LambdaExpression> projections) => projections is []
            ? ValuesForStringKeys<TValue>([])
            : this
                .Continue()
                .Build(
                    static (builder, projections, @this) =>
                    {
                        var tStepCount = 0;
                        var stringKeyCount = 0;

#if NET8_0_OR_GREATER
                        var buffer = default(Buffer8);

                        var objects = projections.Length <= 8
                            ? buffer[..]
                            : new object[projections.Length];
#else
                        var objects = new object[projections.Length].AsSpan();
#endif

                        foreach (var projection in projections)
                        {
                            _ = @this.GetKey(projection).RawKey switch
                            {
                                T t when t.TryToStep() is { } step => objects[tStepCount++] = step,
                                string str => objects[^(++stringKeyCount)] = str,
                                _ => throw new ExpressionNotSupportedException(projection)
                            };
                        }

                        if (stringKeyCount > 0)
                            objects[tStepCount++] = new ValuesStep(objects[^stringKeyCount..].Cast().To<string>().ToImmutableArray());

                        var steps = objects[..tStepCount].Cast().To<Step>();

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
                                .AddStep(new UnionStep(traversalArray.UnsafeToImmutableArray()));
                        }

                        return builder
                            .WithNewProjection(Projection.Value)
                            .BuildAuto<TValue>();
                    },
                    projections,
                    this);

        private GremlinQuery<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta, IGremlinQueryBase> VertexProperties<TNewPropertyValue, TNewMeta>(ReadOnlySpan<LambdaExpression> projections) => Properties<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta>(Projection.VertexProperty, projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object, IGremlinQueryBase> VertexProperties<TNewPropertyValue>(ReadOnlySpan<LambdaExpression> projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(Projection.VertexProperty, projections);

        private GremlinQuery<T1, T2, T3, T4> Where(Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase> filterContinuation) => this
            .Continue(ContinuationFlags.Filter)
            .With(filterContinuation)
            .Build(static (builder, filterTraversal) => filterTraversal.IsIdentity()
                ? builder
                : filterTraversal.IsNone() && filterTraversal.SideEffectSemantics == SideEffectSemantics.Read
                    ? builder.None()
                    : builder.Where(filterTraversal))
            .BuildAuto<T1, T2, T3, T4>();

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

                    MemberExpression { Member: FieldInfo } fieldExpression when fieldExpression.Type == typeof(bool) => (bool)fieldExpression.GetValue()!
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
                                    static (builder, tuple) => builder
                                        .WithSteps(
                                            static (steps, state) =>
                                            {
                                                var (outerQuery, whereExpression, @this) = state;

                                                return @this
                                                    .Where(steps, whereExpression.Left, whereExpression.Semantics, whereExpression.Right);
                                            },
                                            (builder.OuterQuery, tuple.whereExpression, tuple.@this))
                                        .BuildAuto<T1, T2, T3, T4>(),
                                    (whereExpression, @this: this)),

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
                .Cast<TProjection>()
                .Continue()
                .With(propertyContinuation)
                .Build(
                    static (builder, propertyTraversal, key) => builder
                        .AddStep(new HasTraversalStep(key, propertyTraversal))
                        .BuildAuto<T1, T2, T3, T4>(),
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
                        var newRightExpression = Expression.MakeMemberAccess(Expression.MakeMemberAccess(Expression.Constant(newStepLabel), typeof(StepLabel<T1>).GetProperty(nameof(StepLabel<>.Value))!), rightMember.Member);

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
                    .BuildAuto(),
                (label, value));
    }
}
