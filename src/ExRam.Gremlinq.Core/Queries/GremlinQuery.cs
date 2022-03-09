#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.ExpressionParsing;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Models;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Serialization;
using ExRam.Gremlinq.Core.Steps;
using Gremlin.Net.Process.Traversal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExRam.Gremlinq.Core
{
    internal sealed partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> : GremlinQueryBase
    {
        public GremlinQuery(
            IGremlinQueryEnvironment environment,
            StepStack steps,
            Projection projection,
            IImmutableDictionary<StepLabel, Projection> stepLabelProjections,
            QueryFlags flags) : base(steps, projection, environment, stepLabelProjections, flags)
        {

        }

        private GremlinQuery<TEdge, TElement, object, object, object, object> AddE<TEdge>(TEdge newEdge) => this
            .Continue()
            .Build(
                static (builder, newEdge) => builder
                    .AddStep(new AddEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetCache().GetLabel(newEdge!.GetType())))
                    .WithNewProjection(Projection.Edge)
                    .Build<GremlinQuery<TEdge, TElement, object, object, object, object>>(),
                newEdge)
            .AddOrUpdate(newEdge, true);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddOrUpdate(TElement element, bool add)
        {
            var ret = this;
            var props = element.Serialize(
                Environment,
                add
                    ? SerializationBehaviour.IgnoreOnAdd
                    : SerializationBehaviour.IgnoreOnUpdate);

            if (!add)
            {
                ret = ret.SideEffect(_ => _
                    .Properties<object, object, object>(
                        Projection.Empty,
                        props
                            .Select(p => p.key.RawKey)
                            .OfType<string>())
                    .Drop());
            }

            foreach (var (key, value) in props)
            {
                if (!Environment.FeatureSet.Supports(VertexFeatures.UserSuppliedIds) && T.Id.Equals(key.RawKey))
                    Environment.Logger.LogWarning($"User supplied ids are not supported according to the environment's {nameof(Environment.FeatureSet)}.");
                else
                {
                    ret = ret
                        .Continue()
                        .Build(
                            static (builder, tuple) => builder
                                .AddSteps(builder.OuterQuery.GetPropertySteps(tuple.key, tuple.value, builder.OuterQuery.Projection == Projection.Vertex))
                                .Build(),
                            (key, value));
                }
            }

            return ret;
        }

        private IEnumerable<PropertyStep> GetPropertySteps(Key key, object value, bool allowExplicitCardinality)
        {
            if (value is not Traversal && value is IEnumerable enumerable && !Environment.GetCache().FastNativeTypes.ContainsKey(value.GetType()))
            {
                if (!allowExplicitCardinality)
                    throw new NotSupportedException($"A value of type {value.GetType()} is not supported for property '{key}'.");

                foreach (var item in enumerable)
                {
                    if (TryGetPropertyStep(key, item, Cardinality.List) is { } step)
                        yield return step;
                }
            }
            else
            {
                if (TryGetPropertyStep(key, value, allowExplicitCardinality ? Cardinality.Single : default) is { } step)
                    yield return step;
            }
        }

        private PropertyStep? TryGetPropertyStep(Key key, object value, Cardinality? cardinality)
        {
            object? actualValue = value;
            var metaProperties = ImmutableArray<KeyValuePair<string, object>>.Empty;

            if (actualValue is Property property)
            {
                if (property is IVertexProperty vertexProperty)
                {
                    metaProperties = vertexProperty.GetProperties(Environment)
                        .ToImmutableArray();
                }

                actualValue = property.GetValue();
            }

            return actualValue != null
                ? new PropertyStep.ByKeyStep(key, actualValue, metaProperties, cardinality)
                : null;
        }

        private ContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> Continue() => new(
            this,
            new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Environment, StepStack.Empty, Projection, StepLabelProjections, (Flags & ~QueryFlags.SurfaceVisible) | QueryFlags.IsAnonymous));

        private GremlinQuery<TVertex, object, object, object, object, object> AddV<TVertex>(TVertex vertex) => this
            .Continue()
            .Build(
                static (builder, vertex) => builder
                    .AddStep(new AddVStep(builder.OuterQuery.Environment.Model.VerticesModel.GetCache().GetLabel(vertex!.GetType())))
                    .WithNewProjection(Projection.Vertex)
                    .Build<GremlinQuery<TVertex, object, object, object, object, object>>(),
                vertex)
            .AddOrUpdate(vertex, true);

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Scope scope, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            var stepLabel = new TStepLabel();

            return this
                .Aggregate(scope, stepLabel)
                .Apply(__ => continuation(__, stepLabel));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Aggregate<TStepLabel>(Scope scope, TStepLabel stepLabel)
            where TStepLabel : StepLabel => this
                .Continue()
                .Build(
                    static (builder, tuple) => builder
                        .AddStep(new AggregateStep(tuple.scope, tuple.stepLabel))
                        .WithNewStepLabelProjection(_ => _.SetItem(tuple.stepLabel, builder.OuterQuery.Projection.Fold()))
                        .Build(),
                    (scope, stepLabel));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase>[] andContinuations) => this
            .Continue()
            .With(andContinuations)
            .Build(
                static (builder, traversals, andContinuations) =>
                {
                    List<Traversal>? continuedTraversals = default;

                    if (andContinuations.Length == 0)
                    {
                        return builder
                            .AddStep(AndStep.Infix)
                            .Build();
                    }

                    foreach (var traversal in traversals)
                    {
                        if (traversal.IsNone())
                            return builder.OuterQuery.None();

                        if (!traversal.IsIdentity())
                            (continuedTraversals ??= new List<Traversal>()).Add(traversal);
                    }

                    var fusedTraversals = continuedTraversals?
                        .Select(traversal => traversal.RewriteForWhereContext())
                        .Fuse(
                            (p1, p2) => p1.And(p2))
                        .ToArray();

                    return fusedTraversals?.Length switch
                    {
                        null or 0 => builder.OuterQuery,
                        1 => builder.OuterQuery.Where(fusedTraversals[0]),
                        _ => builder
                            .AddStep(new AndStep(fusedTraversals))
                            .Build()
                    };
                },
                andContinuations);

        private TTargetQuery As<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, StepLabel<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>, TTargetQuery> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return As<StepLabel<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>, TTargetQuery>(continuation);
        }

        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
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
                .Apply(__ => continuation(__, stepLabel));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> As(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AsStep(stepLabel))
                    .WithNewStepLabelProjection(_ => _.SetItem(stepLabel, builder.OuterQuery.Projection))
                    .Build(),
                stepLabel);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Barrier() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BarrierStep.Instance)
                .Build());

        private GremlinQuery<object, object, object, object, object, object> Both() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothStep.NoLabels)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> Both<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> BothE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<TEdge, object, object, object, object, object> BothE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new BothEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .Build<GremlinQuery<TEdge, object, object, object, object, object>>());

        private GremlinQuery<TTarget, object, object, object, object, object> BothV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(BothVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .Build<GremlinQuery<TTarget, object, object, object, object, object>>());

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Cast<TNewElement>()
        {
            return typeof(TNewElement) == typeof(TElement)
                ? (GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)(object)this
                : new(Environment, Steps, Projection, StepLabelProjections, Flags);
        }

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTrueQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Choose<TTrueQuery, TFalseQuery, TTargetQuery>(
                    __ => __
                        .Where(predicate),
                    trueChoice,
                    maybeFalseChoice);

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTrueQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(traversalPredicate)
                .Build(
                    static (builder, traversal, choiceTuple) => builder.OuterQuery.Choose<TTrueQuery, TFalseQuery, TTargetQuery>(traversal, choiceTuple.trueChoice, choiceTuple.maybeFalseChoice),
                    (trueChoice, maybeFalseChoice));

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Traversal chooseTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTrueQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(trueChoice)
                .Build(
                    static (builder, trueTraversal, state) =>
                    {
                        var (chooseTraversal, trueChoice, maybeFalseChoice) = state;

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
                                            .AddStep<Step>((chooseTraversal.Count == 1 && chooseTraversal[0] is IsStep isStep)
                                                ? new ChoosePredicateStep(
                                                    isStep.Predicate,
                                                    trueTraversal,
                                                    falseTraversal)
                                                : new ChooseTraversalStep(
                                                    chooseTraversal,
                                                    trueTraversal,
                                                    falseTraversal))
                                            .WithNewProjection(_ => falseTraversal.Projection.Lowest(trueTraversal.Projection))
                                            .Build<TTargetQuery>();
                                    },
                                    (chooseTraversal, trueTraversal));
                        }

                        return builder
                            .AddStep<Step>((chooseTraversal.Count == 1 && chooseTraversal[0] is IsStep isStep)
                                ? new ChoosePredicateStep(
                                    isStep.Predicate,
                                    trueTraversal)
                                : new ChooseTraversalStep(
                                    state.chooseTraversal,
                                    trueTraversal))
                            .WithNewProjection(_ => _.Lowest(trueTraversal.Projection))
                            .Build<TTargetQuery>();
                    },
                    (chooseTraversal, trueChoice, maybeFalseChoice));

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(new ChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, object>(this)).TargetQuery;
        }

        private TReturnQuery Coalesce<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>[] continuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase
        {
            if (continuations.Length == 0)
                throw new ArgumentException("Coalesce must have at least one subquery.");

            return this
                .Continue()
                .With(continuations)
                .Build(static (builder, innerTraversals) =>
                {
                    if (innerTraversals.All(innerTraversal => innerTraversal.Count == 0))
                        return builder.Build<TReturnQuery>();

                    var aggregatedProjection = innerTraversals
                        .Select(x => x.Projection)
                        .Aggregate((x, y) => x.Lowest(y));

                    return builder
                        .AddStep(new CoalesceStep(innerTraversals.ToImmutableArray()))
                        .WithNewProjection(aggregatedProjection)
                        .Build<TReturnQuery>();
                });
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Coin(double probability) => this
            .Continue()
            .Build(
                static (builder, probability) => builder
                    .AddStep(new CoinStep(probability))
                    .Build(),
                probability);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation) => Configure<TElement>(_ => _, transformation);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> ConfigureSteps<TNewElement>(Func<StepStack, StepStack> transformation, Func<Projection, Projection>? projectionTransformation = null) => Configure<TNewElement>(transformation, _ => _, projectionTransformation);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Configure<TNewElement>(
            Func<StepStack, StepStack> stepsTransformation,
            Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation,
            Func<Projection, Projection>? projectionTransformation = null) => new(environmentTransformation(Environment), stepsTransformation(Steps), projectionTransformation?.Invoke(Projection) ?? Projection, StepLabelProjections, Flags);

        private GremlinQuery<TValue, object, object, object, object, object> Constant<TValue>(TValue constant) => this
            .Continue()
            .Build(
                static (builder, constant) => builder
                    .AddStep(new ConstantStep(constant!))
                    .WithNewProjection(Projection.Value)
                    .Build<GremlinQuery<TValue, object, object, object, object, object>>(),
                constant);

        private GremlinQuery<long, object, object, object, object, object> CountGlobal() => Count(Scope.Global);

        private GremlinQuery<long, object, object, object, object, object> CountLocal() => Count(Scope.Local);

        private GremlinQuery<long, object, object, object, object, object> Count(Scope scope) => this
            .Continue()
            .Build(
                static (builder, scope) => builder
                    .AddStep(Scope.Global.Equals(scope)
                        ? CountStep.Global
                        : CountStep.Local)
                    .WithNewProjection(Projection.Value)
                    .Build<GremlinQuery<long, object, object, object, object, object>>(),
                scope);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> CyclicPath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(CyclicPathStep.Instance)
                .Build());

        private string Debug(GroovyFormatting groovyFormatting, bool indented)
        {
            var groovy = Environment.Serializer
                .Serialize(this)
                .ToGroovy(groovyFormatting);

            return JsonConvert.SerializeObject(
                groovy.Bindings.Count > 0
                    ? new { groovy.Script, groovy.Bindings }
                    : new { groovy.Script },
                indented
                    ? Formatting.Indented
                    : Formatting.None);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DedupGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Global)
                .Build());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DedupLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DedupStep.Local)
                .Build());

        private GremlinQuery<object, object, object, object, object, object> Drop() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(DropStep.Instance)
                .WithNewProjection(Projection.Empty)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<string, object, object, object, object, object> Explain() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ExplainStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<string, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> Fail(string? message = null) => this
            .Continue()
            .Build(
                static (builder, message) => builder
                    .AddStep(message is { } actualMessage
                        ? new FailStep(message)
                        : FailStep.NoMessage)
                    .WithNewProjection(Projection.Empty)
                    .Build<GremlinQuery<object, object, object, object, object, object>>(),
                message);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DropProperties(string key) => this
            .SideEffect(_ => _
                .Properties<object, object, object>(
                    Projection.Empty,
                    new[] { key })
                .Drop());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Emit() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(EmitStep.Instance)
                .Build());

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => builder
                .AddStep(new FlatMapStep(innerTraversal))
                .WithNewProjection(innerTraversal.Projection)
                .Build<TTargetQuery>());

        private GremlinQuery<TElement[], object, object, TElement, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(FoldStep.Instance)
                .WithNewProjection(_ => _.Fold())
                .Build<GremlinQuery<TElement[], object, object, TElement, object, TNewFoldedQuery>>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> From<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexContinuation) => this
            .Continue()
            .With(fromVertexContinuation)
            .Build(static (builder, fromVertexTraversal) => builder
                .AddStep(new AddEStep.FromTraversalStep(fromVertexTraversal))
                .Build<GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>>());

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> From<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel<TNewOutVertex> label) => this
           .Continue()
           .Build(
                static (builder, label) => builder
                   .AddStep(new AddEStep.FromLabelStep(label))
                   .Build<GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>>(),
                label);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> To<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IVertexGremlinQueryBase<TNewInVertex>> toVertexContinuation) => this
            .Continue()
            .With(toVertexContinuation)
            .Build(static (builder, toVertexTraversal) => builder
                .AddStep(new AddEStep.ToTraversalStep(toVertexTraversal))
                .Build<GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>>());

        private GremlinQuery<IDictionary<TKey, TValue>, object, object, object, object, object> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IGremlinQueryBase, TKey, TValue>> projection)
        {
            return projection(new GroupBuilder<object, object>(Continue()))
                .Build<GremlinQuery<IDictionary<TKey, TValue>, object, object, object, object, object>>();
        }

        private GremlinQuery<IDictionary<TKey, object>, object, object, object, object, object> Group<TKey>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IGroupBuilderWithKey<IGremlinQueryBase, TKey>> projection)
        {
            return projection(new GroupBuilder<object, object>(Continue()))
                .Build<GremlinQuery<IDictionary<TKey, object>, object, object, object, object, object>>();
        }

        private IEnumerable<string> GetStringKeys(Expression[] projections)
        {
            foreach (var projection in projections)
            {
                if (GetKey(projection).RawKey is string str)
                    yield return str;
            }
        }

        private Key GetKey(Expression projection) => Environment.GetKey(projection);

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IEnumerable<Step> GetStepsForKeys(IEnumerable<Key> keys)
        {
            var hasYielded = false;
            var stringKeys = default(List<string>?);

            foreach (var key in keys)
            {
                switch (key.RawKey)
                {
                    case T t:
                    {
                        if (t.TryToStep() is { } step)
                            yield return step;
                        else
                            throw new ExpressionNotSupportedException($"Can't find an appropriate Gremlin step for {t}.");

                        hasYielded = true;

                        break;
                    }
                    case string str:
                    {
                        (stringKeys ??= new List<string>()).Add(str);

                        break;
                    }
                    default:
                        throw new ExpressionNotSupportedException($"Can't find an appropriate Gremlin step for {key.RawKey}.");
                }
            }

            if (stringKeys?.Count > 0 || !hasYielded)
                yield return new ValuesStep(stringKeys?.ToImmutableArray() ?? ImmutableArray<string>.Empty);
        }

        private GremlinQuery<object, object, object, object, object, object> Id() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(IdStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Identity() => this;

        private GremlinQuery<object, object, object, object, object, object> In() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InStep.NoLabels)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> In<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> InE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<TEdge, object, TElement, object, object, object> InE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new InEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .Build<GremlinQuery<TEdge, object, TElement, object, object, object>>());

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => this
            .Continue()
            .Build(
                static (builder, elements) => builder
                    .AddStep(new InjectStep(elements.Cast<object>().Where(x => x is not null).Select(x => x!).ToImmutableArray()))
                    .WithNewProjection(Projection.Value)
                    .Build<GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>(),
                elements);

        private GremlinQuery<TNewElement, object, object, object, object, object> InV<TNewElement>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(InVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .Build<GremlinQuery<TNewElement, object, object, object, object, object>>());

        private GremlinQuery<string, object, object, object, object, object> Key() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(KeyStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<string, object, object, object, object, object>>());

        private GremlinQuery<string, object, object, object, object, object> Label() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(LabelStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<string, object, object, object, object, object>>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> LimitGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? LimitStep.LimitGlobal1
                        : new LimitStep(count, Scope.Global))
                    .Build(),
                count);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> LimitLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? LimitStep.LimitLocal1
                        : new LimitStep(count, Scope.Local))
                    .Build(),
                count);

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> localTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(localTraversal)
            .Build(static (builder, continuationTraversal) =>
            {
                if (continuationTraversal.Count > 0)
                {
                    builder = builder
                        .AddStep(new LocalStep(continuationTraversal))
                        .WithNewProjection(continuationTraversal.Projection);
                }

                return builder
                    .Build<TTargetQuery>();
            });

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => innerTraversal.Count == 0
                ? builder.OuterQuery
                    .ContinueAs<TTargetQuery>()
                : builder
                    .AddStep(new MapStep(innerTraversal))
                    .WithNewProjection(innerTraversal.Projection)
                    .Build<TTargetQuery>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> None() => this
            .Continue()
            .Build(static builder => builder.OuterQuery.IsIdentity()
                ? builder.OuterQuery
                    .ConfigureSteps<TElement>(_ => StepStack.Empty.Push(NoneStep.Instance))
                : builder
                    .AddStep(NoneStep.Instance)
                    .Build());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> continuation) => this
            .Continue()
            .With(continuation)
            .Build(static (builder, innerTraversal) => innerTraversal.Count == 0
                ? builder.OuterQuery
                    .None()
                : innerTraversal.IsNone()
                    ? builder.OuterQuery
                    : builder
                        .AddStep(new NotStep(innerTraversal))
                        .Build());

        private GremlinQuery<TTarget, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> OfType<TTarget>(IGraphElementModel model)
        {
            if (typeof(TTarget).IsAssignableFrom(typeof(TElement)))
                return Cast<TTarget>();

            var labels = model
                .TryGetFilterLabels(typeof(TTarget), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity)) ?? ImmutableArray.Create(typeof(TTarget).Name);

            return labels.Length > 0
                ? this
                    .Continue()
                    .Build(
                        static (builder, labels) => builder
                            .AddStep(new HasLabelStep(labels))
                            .Build<GremlinQuery<TTarget, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>(),
                        labels)
                : Cast<TTarget>();
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase => this
            .Continue()
            .With(optionalTraversal)
            .Build(static (builder, continuedTraversal) => builder
                .AddStep(new OptionalStep(continuedTraversal))
                .WithNewProjection(_ => _.Lowest(continuedTraversal.Projection))
                .Build<TTargetQuery>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase>[] orTraversalTransformations) => this
            .Continue()
            .With(orTraversalTransformations)
            .Build(static (builder, traversals) =>
            {
                List<Traversal>? continuedTraversals = default;

                if (traversals.Count == 0)
                {
                    return builder
                        .AddStep(OrStep.Infix)
                        .Build();
                }

                foreach (var traversal in traversals)
                {
                    if (traversal.IsIdentity())
                        return builder.OuterQuery;

                    if (!traversal.IsNone())
                        (continuedTraversals ??= new List<Traversal>()).Add(traversal);
                }

                var fusedTraversals = continuedTraversals?
                    .Select(traversal => traversal.RewriteForWhereContext())
                    .Fuse(
                        (p1, p2) => p1.Or(p2))
                    .ToArray();

                return fusedTraversals?.Length switch
                {
                    null or 0 => builder.OuterQuery
                        .None(),
                    1 => builder.OuterQuery
                        .Where(fusedTraversals[0]),
                    _ => builder
                        .AddStep(new OrStep(fusedTraversals))
                        .Build()
                };
            });

        private TTargetQuery OrderGlobal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Global)
                    .Build()
                    .Order(projection),
                projection);

        private TTargetQuery OrderLocal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => this
            .Continue()
            .Build(
                static (builder, projection) => builder
                    .AddStep(OrderStep.Local)
                    .Build()
                    .Order(projection),
                projection);

        private TTargetQuery Order<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => projection(new OrderBuilder(this)).Build();

        private GremlinQuery<object, object, object, object, object, object> Out() => this
           .Continue()
           .Build(static builder => builder
               .AddStep(OutStep.NoLabels)
               .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> Out<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<object, object, object, object, object, object> OutE() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutEStep.NoLabels)
                .WithNewProjection(Projection.Edge)
                .Build<GremlinQuery<object, object, object, object, object, object>>());

        private GremlinQuery<TEdge, TElement, object, object, object, object> OutE<TEdge>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new OutEStep(builder.OuterQuery.Environment.Model.EdgesModel.GetFilterLabelsOrDefault(typeof(TEdge), builder.OuterQuery.Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))))
                .WithNewProjection(Projection.Edge)
                .Build<GremlinQuery<TEdge, TElement, object, object, object, object>>());

        private GremlinQuery<TTarget, object, object, object, object, object> OtherV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OtherVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .Build<GremlinQuery<TTarget, object, object, object, object, object>>());

        private GremlinQuery<TTarget, object, object, object, object, object> OutV<TTarget>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(OutVStep.Instance)
                .WithNewProjection(Projection.Vertex)
                .Build<GremlinQuery<TTarget, object, object, object, object, object>>());

        private GremlinQuery<Path, object, object, object, object, object> Path() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(PathStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<Path, object, object, object, object, object>>());

        private GremlinQuery<string, object, object, object, object, object> Profile() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ProfileStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<string, object, object, object, object, object>>());

        private GremlinQuery<TResult, object, object, object, object, object> Project<TResult>(Func<IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>, IProjectResult> continuation)
        {
            return continuation(new ProjectBuilder<TElement, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(this))
                .Build<GremlinQuery<TResult, object, object, object, object, object>>();
        }

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, params Expression[] projections) => Properties<TNewElement, TNewPropertyValue, TNewMeta>(
            projection,
            projections
                .Select(projection => GetKey(projection).RawKey)
                .OfType<string>());

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, IEnumerable<string> keys) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new PropertiesStep(tuple.keys.ToImmutableArray()))
                    .WithNewProjection(tuple.projection)
                    .Build<GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object>>(),
                (keys, projection));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(LambdaExpression projection, object? value) => Property(GetKey(projection), value);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(Key key, object? value) => this
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

                    foreach (var propertyStep in builder.OuterQuery.GetPropertySteps(tuple.key, tuple.value, builder.OuterQuery.Projection == Projection.Vertex))
                    {
                        builder = builder.AddStep(propertyStep);
                    }

                    return builder
                        .Build();
                },
                (key, value));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(Key key, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (builder, valueTraversal, key) => builder.OuterQuery.Property(key, valueTraversal),
                key);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(LambdaExpression projection, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> valueContinuation) => this
            .Continue()
            .With(valueContinuation)
            .Build(
                static (builder, valueTraversal, projection) => builder.OuterQuery.Property(projection, valueTraversal),
                projection);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> RangeGlobal(long low, long high) => Range(low, high, Scope.Global);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> RangeLocal(long low, long high) => Range(low, high, Scope.Local);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Range(long low, long high, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new RangeStep(tuple.low, tuple.high, tuple.scope))
                    .Build(),
                (low, high, scope));

        private TTargetQuery Repeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatContinuation)
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(repeatContinuation)
                .Build(static (builder, innerTraversal) => builder
                    .AddStep(new RepeatStep(innerTraversal))
                    .WithNewProjection(_ => _.Lowest(innerTraversal.Projection))
                    .Build<TTargetQuery>());

        private TTargetQuery RepeatUntil<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatContinuation, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> untilContinuation)
            where TTargetQuery : IGremlinQueryBase => this
                .Continue()
                .With(repeatContinuation)
                .Build(
                    static (builder, repeatTraversal, untilContinuation) => builder.OuterQuery
                        .Continue()
                        .With(untilContinuation)
                        .Build(
                            static (builder, untilTraversal, repeatTraversal) =>
                            {
                                builder = builder
                                    .AddStep(new RepeatStep(repeatTraversal))
                                    .WithNewProjection(_ => _.Lowest(repeatTraversal.Projection));

                                if (!untilTraversal.IsNone())
                                {
                                    builder = builder
                                        .AddStep(new UntilStep(untilTraversal));
                                }

                                return builder
                                    .Build<TTargetQuery>();
                            },
                            repeatTraversal),
                    untilContinuation);

        private TTargetQuery UntilRepeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatContinuation, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> untilContinuation)
            where TTargetQuery : IGremlinQueryBase => this
               .Continue()
               .With(repeatContinuation)
               .Build(
                    static (builder, repeatTraversal, untilContinuation) => builder.OuterQuery
                       .Continue()
                       .With(untilContinuation)
                       .Build(
                           static (builder, untilTraversal, repeatTraversal) =>
                           {
                               if (!untilTraversal.IsNone())
                               {
                                   builder = builder
                                       .AddStep(new UntilStep(untilTraversal));
                               }

                               builder = builder
                                   .AddStep(new RepeatStep(repeatTraversal))
                                   .WithNewProjection(_ => _.Lowest(repeatTraversal.Projection));

                               return builder
                                   .Build<TTargetQuery>();
                           },
                           repeatTraversal),
                    untilContinuation);

        private GremlinQuery<TSelectedElement, object, object, object, object, object> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel) =>
            StepLabelProjections.TryGetValue(stepLabel, out var stepLabelProjection)
                ? this
                    .Continue()
                    .Build(
                        static (builder, tuple) => builder
                            .AddStep(new SelectStepLabelStep(ImmutableArray.Create<StepLabel>(tuple.stepLabel)))
                            .WithNewProjection(tuple.stepLabelProjection)
                            .Build<GremlinQuery<TSelectedElement, object, object, object, object, object>>(),
                        (stepLabel, stepLabelProjection))
                : throw new InvalidOperationException($"Invalid use of unknown {nameof(StepLabel)} in {nameof(Select)}. Make sure you only pass in a {nameof(StepLabel)} that comes from a previous {nameof(As)}- or {nameof(IGremlinQuerySource.WithSideEffect)}-continuation or has previously been passed to an appropriate overload of {nameof(As)} or {nameof(IGremlinQuerySource.WithSideEffect)}.");

        private TTargetQuery Select<TTargetQuery>(params Expression[] projections) => this
            .Continue()
            .Build(
                static (builder, projections) =>
                {
                    var keys = projections
                        .Select(builder.OuterQuery.GetKey)
                        .ToImmutableArray();

                    return builder
                        .AddStep(new SelectKeysStep(keys))
                        .WithNewProjection(_ => _.If<TupleProjection>(tuple => tuple.Select(keys)))
                        .Build<TTargetQuery>();
                },
                projections);

        private GremlinQuery<TSelectedElement, object, object, TArrayItem, object, TQuery> Cap<TSelectedElement, TArrayItem, TQuery>(StepLabel<IArrayGremlinQuery<TSelectedElement, TArrayItem, TQuery>, TSelectedElement> stepLabel) where TQuery : IGremlinQueryBase => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new CapStep(stepLabel))
                    .WithNewProjection(_ => _.Fold())
                    .Build<GremlinQuery<TSelectedElement, object, object, TArrayItem, object, TQuery>>(),
                stepLabel);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> sideEffectContinuation) => this
            .Continue()
            .With(sideEffectContinuation)
            .Build(static (builder, traversal) => builder
                .AddStep(new SideEffectStep(traversal))
                .Build());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SimplePath() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(SimplePathStep.Instance)
                .Build());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Skip(long count, Scope scope) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new SkipStep(tuple.count, tuple.scope))
                    .Build(),
                (count, scope));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SumGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Global))
                .WithNewProjection(Projection.Value)
                .Build());

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SumLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new SumStep(Scope.Local))
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>());

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MinLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new MinStep(Scope.Local))
                .Build<GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Mute() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(NoneStep.Instance)
                .WithAdditionalFlags(QueryFlags.IsMuted)
                .Build());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MinGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new MinStep(Scope.Global))
                .WithNewProjection(Projection.Value)
                .Build());

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MaxLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new MaxStep(Scope.Local))
                .Build<GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MaxGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new MaxStep(Scope.Global))
                .WithNewProjection(Projection.Value)
                .Build());

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MeanLocal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new MeanStep(Scope.Local))
                .Build<GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MeanGlobal() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(new MeanStep(Scope.Global))
                .WithNewProjection(Projection.Value)
                .Build());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> TailGlobal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(new TailStep(count, Scope.Global))
                    .Build(),
                count);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> TailLocal(long count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(count == 1
                        ? TailStep.TailLocal1
                        : new TailStep(count, Scope.Local))
                    .Build(),
                count);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Times(int count) => this
            .Continue()
            .Build(
                static (builder, count) => builder
                    .AddStep(new TimesStep(count))
                    .Build(),
                count);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => this
            .Continue()
            .Build(
                static (builder, stepLabel) => builder
                    .AddStep(new AddEStep.ToLabelStep(stepLabel))
                    .Build<GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>>(),
                stepLabel);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Unfold() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(UnfoldStep.Instance)
                .WithNewProjection(_ => _.If<ArrayProjection>(array => array.Unfold()))
                .Build());

        private TTargetQuery Unfold<TTargetQuery>() => Unfold().ContinueAs<TTargetQuery>();

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Union(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>[] unionTraversals)
        {
            return Union<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>(unionTraversals);
        }

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals)
            where TTargetQuery : IGremlinQueryBase
        {
            return Union<TTargetQuery, TTargetQuery>(unionTraversals);
        }

        private TReturnQuery Union<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>[] unionContinuations)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase => this
                .Continue()
                .With(unionContinuations)
                .Build(static (builder, unionTraversals) =>
                {
                    var aggregatedProjection = unionTraversals
                        .Select(traversal => traversal.Projection)
                        .Aggregate((x, y) => x.Lowest(y));

                    return builder
                        .AddStep(new UnionStep(unionTraversals.ToImmutableArray()))
                        .WithNewProjection(aggregatedProjection)
                        .Build<TReturnQuery>();
                });

        private GremlinQuery<object, object, object, object, object, object> V(ImmutableArray<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new VStep(ids))
                    .WithNewProjection(Projection.Vertex)
                    .Build<GremlinQuery<object, object, object, object, object, object>>(),
                ids);

        private GremlinQuery<object, object, object, object, object, object> E(ImmutableArray<object> ids) => this
            .Continue()
            .Build(
                static (builder, ids) => builder
                    .AddStep(new EStep(ids))
                    .WithNewProjection(Projection.Edge)
                    .Build<GremlinQuery<object, object, object, object, object, object>>(),
                ids);

        private GremlinQuery<TNewPropertyValue, object, object, object, object, object> Value<TNewPropertyValue>() => this
            .Continue()
            .Build(static builder => builder
                .AddStep(ValueStep.Instance)
                .WithNewProjection(Projection.Value)
                .Build<GremlinQuery<TNewPropertyValue, object, object, object, object, object>>());

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(ImmutableArray<string> keys) => this
            .Continue()
            .Build(
                static (builder, keys) => builder
                    .AddStep(new ValueMapStep(keys))
                    .WithNewProjection(Projection.Value)
                    .Build<GremlinQuery<TNewElement, object, object, object, object, object>>(),
                keys);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
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
                            .Build<GremlinQuery<TNewElement, object, object, object, object, object>>(),
                        stringKeys);
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForKeys<TValue>(IEnumerable<Key> keys)
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
                            .Build<GremlinQuery<TValue, object, object, object, object, object>>(),
                        stepsArray[0]),
                _ => this
                    .Union(stepsArray
                        .Select(step => new Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TValue, object, object, object, object, object>>(__ => __
                            .Continue()
                            .Build(
                                static (builder, step) => builder
                                    .AddStep(step)
                                    .WithNewProjection(Projection.Value)
                                    .Build<GremlinQuery<TValue, object, object, object, object, object>>(),
                                step)))
                        .ToArray())
                    .Continue()
                    .Build(static builder => builder
                        .WithNewProjection(Projection.Value)
                        .Build())
            };
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(projections.Select(projection => GetKey(projection)));

        private GremlinQuery<VertexProperty<TNewPropertyValue, TNewMeta>, object, object, TNewPropertyValue, TNewMeta, object> VertexProperties<TNewPropertyValue, TNewMeta>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta>(Projection.VertexProperty, projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue>, object, object, TNewPropertyValue, object, object> VertexProperties<TNewPropertyValue>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(Projection.VertexProperty, projections);
        
        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(ILambda lambda) => this
            .Continue()
            .Build(
                static (builder, lambda) => builder
                    .AddStep(new FilterStep(lambda))
                    .Build(),
                lambda);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> filterContinuation)
        {
            return this
                .Continue()
                .With(filterContinuation)
                .Build(static (builder, filterTraversal) => filterTraversal.IsIdentity()
                    ? builder.OuterQuery
                    : filterTraversal.IsNone()
                        ? builder.OuterQuery.None()
                        : builder.OuterQuery.Where(filterTraversal));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Traversal traversal) => this
            .Continue()
            .Build(
                static (builder, traversal) =>
                {
                    builder = traversal.Count > 0 && traversal.All(x => x is IIsOptimizableInWhere)
                        ? builder.AddSteps(traversal)
                        : builder.AddStep(new WhereTraversalStep(traversal));

                    return builder.Build();
                },
                traversal.RewriteForWhereContext());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Expression<Func<TElement, bool>> expression) => Where((Expression)expression);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Expression expression)
        {
            try
            {
                switch (expression)
                {
                    case ConstantExpression constantExpression when constantExpression.Value is bool value:
                    {
                        return value
                            ? this
                            : None();
                    }
                    case LambdaExpression lambdaExpression:
                    {
                        return Where(lambdaExpression.Body);
                    }
                    case UnaryExpression { NodeType: ExpressionType.Not } unaryExpression:
                    {
                        return Not(__ => __.Where(unaryExpression.Operand));
                    }
                    case BinaryExpression { NodeType: ExpressionType.OrElse } binary:
                    {
                        return Or(
                            __ => __.Where(binary.Left),
                            __ => __.Where(binary.Right));
                    }
                    case BinaryExpression { NodeType: ExpressionType.AndAlso } binary:
                    {
                        return this
                            .Where(binary.Left)
                            .Where(binary.Right);
                    }
                }

                if (expression.TryToGremlinExpression(Environment.Model) is { } gremlinExpression)
                {
                    return gremlinExpression.Equals(GremlinExpression.True)
                        ? this
                        : gremlinExpression.Equals(GremlinExpression.False)
                            ? None()
                            : this
                                .Continue()
                                .Build(
                                    static (builder, gremlinExpression) => builder
                                        .AddSteps(builder.OuterQuery.Where(gremlinExpression))
                                        .Build(),
                                    gremlinExpression);
                }
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(expression, ex);
            }

            throw new ExpressionNotSupportedException(expression);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where<TProjection>(Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyContinuation) => predicate.TryGetReferredParameter() is not null && predicate.Body is MemberExpression memberExpression
             ? this
                 .Continue()
                 .With(__ => propertyContinuation(__
                     .Cast<TProjection>()))
                 .Build(
                     static (builder, propertyTraversal, key) => builder
                         .AddStep(new HasTraversalStep(key, propertyTraversal))
                         .Build(),
                     GetKey(memberExpression))
             : throw new ExpressionNotSupportedException(predicate);

        private IEnumerable<Step> Where(GremlinExpression gremlinExpression) => Where(
            gremlinExpression.Left,
            gremlinExpression.LeftWellKnownMember,
            gremlinExpression.Semantics,
            gremlinExpression.Right);

        private IEnumerable<Step> Where(ExpressionFragment left, WellKnownMember? leftWellKnownMember, ExpressionSemantics semantics, ExpressionFragment right)
        {
            if (right.Type == ExpressionFragmentType.Constant)
            {
                var maybeEffectivePredicate = Environment.Options
                    .GetValue(PFactory.PFactoryOption)
                    .TryGetP(semantics, right.GetValue(), Environment)
                    ?.WorkaroundLimitations(Environment);

                if (maybeEffectivePredicate is { } effectivePredicate)
                { 
                    if (left.Type == ExpressionFragmentType.Parameter)
                    {
                        switch (left.Expression)
                        {
                            case MemberExpression leftMemberExpression:
                            {
                                var leftMemberExpressionExpression = leftMemberExpression.Expression?.StripConvert();

                                if (leftMemberExpressionExpression is ParameterExpression parameterExpression)
                                {
                                    if (leftWellKnownMember == WellKnownMember.ArrayLength)
                                    {
                                        if (Environment.GetCache().ModelTypes.Contains(parameterExpression.Type))
                                        {
                                            if (GetKey(leftMemberExpression).RawKey is string stringKey)
                                            {
                                                if (!Environment.GetCache().FastNativeTypes.ContainsKey(leftMemberExpression.Type))
                                                {
                                                    yield return new WhereTraversalStep(ImmutableArray.Create<Step>(
                                                        new PropertiesStep(ImmutableArray.Create(stringKey)),
                                                        CountStep.Global,
                                                        new IsStep(effectivePredicate)));

                                                    yield break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            yield return new WhereTraversalStep(ImmutableArray.Create<Step>(
                                                new SelectKeysStep(
                                                    ImmutableArray.Create(GetKey(leftMemberExpression))),
                                                CountStep.Local,
                                                new IsStep(effectivePredicate)));

                                            yield break;
                                        }
                                        
                                        break;
                                    }
                                }
                                else if (leftMemberExpressionExpression is MemberExpression leftLeftMemberExpression)
                                {
                                    // x => x.Name.Value == P.xy(...)
                                    if (leftWellKnownMember == WellKnownMember.PropertyValue)
                                        leftMemberExpression = leftLeftMemberExpression;
                                }
                                else
                                    break;

                                // x => x.Name == P.xy(...)
                                if (right.GetValue() is StepLabel)
                                {
                                    if (right.Expression is MemberExpression memberExpression)
                                    {
                                        yield return new WherePredicateStep(effectivePredicate);
                                        yield return new WherePredicateStep.ByMemberStep(GetKey(leftMemberExpression));

                                        if (memberExpression.Member != leftMemberExpression.Member)
                                            yield return new WherePredicateStep.ByMemberStep(GetKey(memberExpression));

                                        yield break;
                                    }

                                    yield return new HasTraversalStep(
                                        GetKey(leftMemberExpression),
                                        new WherePredicateStep(effectivePredicate));

                                    yield break;
                                }

                                yield return effectivePredicate.EqualsConstant(false)
                                    ? NoneStep.Instance
                                    : new HasPredicateStep(GetKey(leftMemberExpression), effectivePredicate);

                                yield break;
                            }
                            case ParameterExpression parameterExpression:
                            {
                                switch (leftWellKnownMember)
                                {
                                    // x => x.Value == P.xy(...)
                                    case WellKnownMember.PropertyValue when right.GetValue() is not StepLabel:
                                    {
                                        yield return new HasValueStep(effectivePredicate);
                                        yield break;
                                    }
                                    case WellKnownMember.PropertyKey:
                                    {
                                        yield return new WhereTraversalStep(new Traversal(
                                            this
                                                .Where(
                                                    ExpressionFragment.Create(parameterExpression, Environment.Model),
                                                    default,
                                                    semantics,
                                                    right)
                                                .Prepend(KeyStep.Instance),
                                            Projection.Empty));

                                        yield break;
                                    }
                                    case WellKnownMember.VertexPropertyLabel when right.GetValue() is StepLabel:
                                    {
                                        yield return new WhereTraversalStep(new Traversal(
                                            this
                                                .Where(
                                                    ExpressionFragment.Create(parameterExpression, Environment.Model),
                                                    default,
                                                    semantics,
                                                    right)
                                                .Prepend(LabelStep.Instance),
                                            Projection.Empty));

                                        yield break;
                                    }
                                    case WellKnownMember.VertexPropertyLabel:
                                    {
                                        yield return new HasKeyStep(effectivePredicate);
                                        yield break;
                                    }
                                }

                                // x => x == P.xy(...)
                                if (right.GetValue() is StepLabel)
                                {
                                    yield return new WherePredicateStep(effectivePredicate);

                                    if (right.Expression is MemberExpression memberExpression)
                                        yield return new WherePredicateStep.ByMemberStep(GetKey(memberExpression));

                                    yield break;
                                }

                                if (effectivePredicate.EqualsConstant(false))
                                    yield return NoneStep.Instance;
                                else if (!effectivePredicate.EqualsConstant(true))
                                    yield return new IsStep(effectivePredicate);

                                yield break;
                            }
                            case MethodCallExpression methodCallExpression:
                            {
                                var targetExpression = methodCallExpression.Object?.StripConvert();

                                if (targetExpression != null && typeof(IDictionary<string, object>).IsAssignableFrom(targetExpression.Type) && methodCallExpression.Method.Name == "get_Item")
                                {
                                    if (methodCallExpression.Arguments[0].StripConvert()!.GetValue() is string key)
                                    {
                                        yield return new HasPredicateStep(key, effectivePredicate);
                                        yield break;
                                    }
                                }

                                break;
                            }
                        }
                    }
                    else if (left.Type == ExpressionFragmentType.Constant && left.GetValue() is StepLabel leftStepLabel && right.GetValue() is StepLabel)
                    {
                        yield return new WhereStepLabelAndPredicateStep(leftStepLabel, effectivePredicate);

                        if (left.Expression is MemberExpression leftStepValueExpression)
                            yield return new WherePredicateStep.ByMemberStep(GetKey(leftStepValueExpression));

                        if (right.Expression is MemberExpression rightStepValueExpression)
                            yield return new WherePredicateStep.ByMemberStep(GetKey(rightStepValueExpression));

                        yield break;
                    }
                }
            }
            else if (right.Type == ExpressionFragmentType.Parameter)
            {
                if (left.Type == ExpressionFragmentType.Parameter)
                {
                    if (left.Expression is MemberExpression && right.Expression is MemberExpression rightMember)
                    {
                        var newStepLabel = new StepLabel<TElement>();

                        yield return new AsStep(newStepLabel);

                        var subSteps = Where(
                            left,
                            default,
                            semantics,
                            ExpressionFragment.StepLabel(newStepLabel, rightMember));

                        foreach (var step in subSteps)
                        {
                            yield return step;
                        }

                        yield break;
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private TQuery WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
        {
            var stepLabel = new StepLabel<TSideEffect>();

            return continuation(
                ((IGremlinQuerySource)this).WithSideEffect(stepLabel, value),
                stepLabel);
        }

        private GremlinQuery<object, object, object, object, object, object> WithoutStrategies(params Type[] strategyTypes) => this
           .Continue()
           .Build(
                static (builder, strategyTypes) => builder
                   .AddStep(new WithoutStrategiesStep(strategyTypes.ToImmutableArray()))
                   .Build<GremlinQuery<object, object, object, object, object, object>>(),
                strategyTypes);

        private GremlinQuery<object, object, object, object, object, object> WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .AddStep(new WithSideEffectStep(tuple.label, tuple.value!))
                    .WithNewStepLabelProjection(_ => _.SetItem(tuple.label, builder.OuterQuery.Projection))
                    .Build<GremlinQuery<object, object, object, object, object, object>>(),
                (label, value));
    }
}
