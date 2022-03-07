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
        private static readonly Step[] EmptyProjectionProtectionDecoratorSteps = new []
        {
            new MapStep(new Step[]
            {
                UnfoldStep.Instance,
                GroupStep.Instance,
                new GroupStep.ByTraversalStep(new SelectColumnStep(Column.Keys)),
                new GroupStep.ByTraversalStep(new Step[]
                {
                    new SelectColumnStep(Column.Values),
                    UnfoldStep.Instance
                })
            })
        };

        public GremlinQuery(
            StepStack steps,
            Projection projection,
            IGremlinQueryEnvironment environment,
            IImmutableDictionary<StepLabel, Projection> stepLabelProjections,
            QueryFlags flags) : base(steps, projection, environment, stepLabelProjections, flags)
        {

        }

        private GremlinQuery<TEdge, TElement, object, object, object, object> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, object, object, object, object>(
                    new AddEStep(Environment.Model.EdgesModel.GetCache().GetLabel(newEdge!.GetType())),
                    _ => Projection.Edge)
                .AddOrUpdate(newEdge, true);
        }

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
                    ret = ret.AddSteps(GetPropertySteps(key, value, Projection == Projection.Vertex));
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddSteps(IEnumerable<Step> steps, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None) => AddSteps<TElement>(steps, projectionTransformation, stepLabelProjections, additionalFlags);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddStep(Step step, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None) => AddStep<TElement>(step, projectionTransformation, stepLabelProjections, additionalFlags);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddSteps<TNewElement>(IEnumerable<Step> steps, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None) => AddSteps<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(steps, projectionTransformation, stepLabelProjections, additionalFlags);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None) => AddStep<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(step, projectionTransformation, stepLabelProjections, additionalFlags);

        private GremlinQuery<TNewElement, object, object, object, object, object> AddStepWithObjectTypes<TNewElement>(Step step, Func<Projection, Projection>? projectionTransformation = null) => AddStep<TNewElement, object, object, object, object, object>(step, projectionTransformation);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Step step, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None)
        {
            var newSteps = Steps;

            if ((Flags & QueryFlags.IsMuted) == 0)
                newSteps = Environment.AddStepHandler.AddStep(newSteps, step, Environment);

            return Continue<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(newSteps, projectionTransformation, stepLabelProjections, additionalFlags);
        }

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddSteps<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(IEnumerable<Step> steps, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None)
        {
            var newSteps = Steps;

            if ((Flags & QueryFlags.IsMuted) == 0)
            {
                foreach (var step in steps)
                {
                    newSteps = Environment.AddStepHandler.AddStep(newSteps, step, Environment);
                }
            }

            return Continue<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(newSteps, projectionTransformation, stepLabelProjections, additionalFlags);
        }

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Continue<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(StepStack newSteps, Func<Projection, Projection>? projectionTransformation = null, IImmutableDictionary<StepLabel, Projection>? stepLabelProjections = null, QueryFlags additionalFlags = QueryFlags.None)
        {
            return new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(
                newSteps,
                projectionTransformation is { } transformation
                    ? transformation(Projection)
                    : Projection,
                Environment,
                stepLabelProjections ?? StepLabelProjections,
                Flags | additionalFlags);
        }

        private ContinuationBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> Continue()
        {
            return new(
                this,
                new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(StepStack.Empty, Projection, Environment, StepLabelProjections, (Flags & ~QueryFlags.SurfaceVisible) | QueryFlags.IsAnonymous));
        }

        private GremlinQuery<TVertex, object, object, object, object, object> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStepWithObjectTypes<TVertex>(
                    new AddVStep(Environment.Model.VerticesModel.GetCache().GetLabel(vertex!.GetType())),
                    _ => Projection.Vertex)
                .AddOrUpdate(vertex, true);
        }

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Scope scope, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            var stepLabel = new TStepLabel();

            return this
                .Aggregate(scope, stepLabel)
                .ContinueOuter(__ => continuation(__, stepLabel));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Aggregate<TStepLabel>(Scope scope, TStepLabel stepLabel)
            where TStepLabel : StepLabel
        {
            return AddStep(
                new AggregateStep(scope, stepLabel),
                _ => _,
                StepLabelProjections.SetItem(stepLabel, Projection.Fold()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase>[] andTraversalTransformations)
        {
            if (andTraversalTransformations.Length == 0)
                return AddStep(AndStep.Infix);

            List<IGremlinQueryBase>? subQueries = default;

            foreach (var transformation in andTraversalTransformations)
            {
                var transformed = ContinueInner(transformation);

                if (transformed.IsNone())
                    return None();

                if (!transformed.IsIdentity())
                    (subQueries ??= new List<IGremlinQueryBase>()).Add(transformed);
            }

            var fusedTraversals = subQueries?
                .Select(x => x.ToTraversal().RewriteForWhereContext())
                .Fuse(
                    (p1, p2) => p1.And(p2))
                .ToArray();

            return fusedTraversals?.Length switch
            {
                null or 0 => this,
                1 => Where(fusedTraversals[0]),
                _ => AddStep(new AndStep(fusedTraversals!))
            };
        }

        private TTargetQuery ContinueInner<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> transformation, bool surfaceVisible = false)
        {
            var targetQuery = transformation(new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(StepStack.Empty, Projection, Environment, StepLabelProjections, (surfaceVisible ? Flags | QueryFlags.SurfaceVisible : Flags & ~QueryFlags.SurfaceVisible) | QueryFlags.IsAnonymous));

            if (targetQuery is GremlinQueryBase queryBase && (queryBase.Flags & QueryFlags.IsAnonymous) == QueryFlags.None)
                throw new InvalidOperationException("A query continuation must originate from the query that was passed to the continuation function. Did you accidentally use 'g' in the continuation?");

            return targetQuery;
        }

        private TTargetQuery ContinueOuter<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> transformation)
        {
            return transformation(this);
        }

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
                .ContinueOuter(__ => continuation(__, stepLabel));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> As(StepLabel stepLabel)
        {
            return AddStep(
                new AsStep(stepLabel),
                stepLabelProjections: StepLabelProjections.SetItem(stepLabel, Projection));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Barrier() => AddStep(BarrierStep.Instance);

        private GremlinQuery<TTarget, object, object, object, object, object> BothV<TTarget>() => AddStepWithObjectTypes<TTarget>(BothVStep.Instance, _ => Projection.Vertex);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Cast<TNewElement>()
        {
            if (typeof(TNewElement) == typeof(TElement))
                return (GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>)(object)this;

             return new(Steps, Projection, Environment, StepLabelProjections, Flags);
        }

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTrueQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TFalseQuery>? maybeFalseChoice = default)
            where TTrueQuery : IGremlinQueryBase
            where TFalseQuery : IGremlinQueryBase
            where TTargetQuery : IGremlinQueryBase
        {
            return Choose<TTrueQuery, TFalseQuery, TTargetQuery>(
                this
                    .ContinueInner(__ => __
                        .Where(predicate))
                    .ToTraversal(),
                trueChoice,
                maybeFalseChoice);
        }

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTrueQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TFalseQuery>? maybeFalseChoice = default)
           where TTrueQuery : IGremlinQueryBase
           where TFalseQuery : IGremlinQueryBase
           where TTargetQuery : IGremlinQueryBase
        {
            return Choose<TTrueQuery, TFalseQuery, TTargetQuery>(
                this
                    .ContinueInner(traversalPredicate)
                    .ToTraversal(),
                trueChoice,
                maybeFalseChoice);
        }

        private TTargetQuery Choose<TTrueQuery, TFalseQuery, TTargetQuery>(Traversal chooseTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTrueQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TFalseQuery>? maybeFalseChoice = default)
           where TTrueQuery : IGremlinQueryBase
           where TFalseQuery : IGremlinQueryBase
           where TTargetQuery : IGremlinQueryBase
        {
            var trueTraversal = this
                .ContinueInner(trueChoice)
                .ToTraversal();

            var maybeFalseTraversal = maybeFalseChoice is { } falseChoice
                ? ContinueInner(falseChoice).ToTraversal()
                : default(Traversal?);

            Step chooseStep = (chooseTraversal.Count == 1 && chooseTraversal[0] is IsStep isStep)
               ? new ChoosePredicateStep(
                   isStep.Predicate,
                   trueTraversal,
                   maybeFalseTraversal)
               : new ChooseTraversalStep(
                   chooseTraversal,
                   trueTraversal,
                   maybeFalseTraversal);

            var projection = (maybeFalseTraversal?.Projection ?? Projection)
                .Lowest(trueTraversal.Projection);

            return this
                .AddStep(
                   chooseStep,
                   _ => projection)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(new ChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, object>(this, this)).TargetQuery;
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
                .Build((builder, innerTraversals) =>
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Coin(double probability) => AddStep(new CoinStep(probability));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation) => Configure<TElement>(_ => _, transformation);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> ConfigureSteps<TNewElement>(Func<StepStack, StepStack> transformation, Func<Projection, Projection>? projectionTransformation = null) => Configure<TNewElement>(transformation, _ => _, projectionTransformation);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Configure<TNewElement>(
            Func<StepStack, StepStack> stepsTransformation,
            Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation,
            Func<Projection, Projection>? projectionTransformation = null) => new(stepsTransformation(Steps), projectionTransformation?.Invoke(Projection) ?? Projection, environmentTransformation(Environment), StepLabelProjections, Flags);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> CyclicPath() => AddStep(CyclicPathStep.Instance);

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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DedupGlobal() => AddStep(DedupStep.Global);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DedupLocal() => AddStep(DedupStep.Local);

        private GremlinQuery<object, object, object, object, object, object> Drop() => AddStepWithObjectTypes<object>(DropStep.Instance, _ => Projection.Empty);

        private GremlinQuery<object, object, object, object, object, object> Fail(string? message = null) => AddStepWithObjectTypes<object>(
            message is { } actualMessage
                ? new FailStep(message)
                : FailStep.NoMessage,
            _ => Projection.Empty);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DropProperties(string key)
        {
            return SideEffect(_ => _
                .Properties<object, object, object>(
                    Projection.Empty,
                    new[] { key })
                .Drop());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase
        {
            var mappedTraversal = ContinueInner(mapping)
                .ToTraversal();

            return this
                .AddStep(new FlatMapStep(mappedTraversal), _ => mappedTraversal.Projection)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement[], object, object, TElement, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() => AddStep<TElement[], object, object, TElement, object, TNewFoldedQuery>(FoldStep.Instance, _ => _.Fold());

        private GremlinQuery<IDictionary<TKey, TValue>, object, object, object, object, object> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IGremlinQueryBase, TKey, TValue>> projection)
        {
            var group = projection(new GroupBuilder<object, object>(this));

            return Group<TKey, TValue>(
                group.KeyQuery.ToTraversal(),
                group.ValueQuery.ToTraversal());
        }

        private GremlinQuery<IDictionary<TKey, object>, object, object, object, object, object> Group<TKey>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IGroupBuilderWithKey<IGremlinQueryBase, TKey>> projection)
        {
            var group = projection(new GroupBuilder<object, object>(this));

            return Group<TKey, object>(
               group.KeyQuery.ToTraversal(),
               null);
        }

        private GremlinQuery<IDictionary<TKey, TValue>, object, object, object, object, object> Group<TKey, TValue>(Traversal keyTraversal, Traversal? maybeValueTraversal)
        {
            var ret = this
                .AddStep<IDictionary<TKey, TValue>, object, object, object, object, object>(
                    GroupStep.Instance,
                    _ => _.Group(
                        keyTraversal.Projection,
                        maybeValueTraversal?.Projection ?? Projection))
                .AddStep(new GroupStep.ByTraversalStep(keyTraversal));

            return (maybeValueTraversal is { } valueTraversal)
                ? ret.AddStep(new GroupStep.ByTraversalStep(valueTraversal))
                : ret;
        }

        private IEnumerable<string> GetStringKeys(Expression[] projections)
        {
            foreach (var projection in projections)
            {
                if (GetKey(projection).RawKey is string str)
                    yield return str;
            }
        }

        private Key GetKey(Expression projection)
        {
            return Environment.GetKey(projection);
        }

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

        private GremlinQuery<object, object, object, object, object, object> Id() => AddStepWithObjectTypes<object>(IdStep.Instance, _ => Projection.Value);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Identity() => this;

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().Where(x => x is not null).Select(x => x!).ToImmutableArray()), _ => Projection.Value);

        private GremlinQuery<TNewElement, object, object, object, object, object> InV<TNewElement>() => AddStepWithObjectTypes<TNewElement>(InVStep.Instance, _ => Projection.Vertex);

        private GremlinQuery<string, object, object, object, object, object> Key() => AddStepWithObjectTypes<string>(KeyStep.Instance, _ => Projection.Value);

        private GremlinQuery<string, object, object, object, object, object> Label() => AddStepWithObjectTypes<string>(LabelStep.Instance, _ => Projection.Value);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> LimitGlobal(long count)
        {
            return AddStep(
                count == 1
                    ? LimitStep.LimitGlobal1
                    : new LimitStep(count, Scope.Global),
                _ => _);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> LimitLocal(long count)
        {
            return count == 1
                ? AddStep(LimitStep.LimitLocal1)
                : AddStep(new LimitStep(count, Scope.Local));
        }

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            return this
                .Continue()
                .With(localTraversal)
                .Build((builder, continuationTraversal) =>
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
        }

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase
        {
            var mappedTraversal = ContinueInner(mapping)
                .ToTraversal();

            return (mappedTraversal.Count == 0
                ? this
                : AddStep(new MapStep(mappedTraversal), _ => mappedTraversal.Projection)).ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> None()
        {
            return this.IsIdentity()
                ? ConfigureSteps<TElement>(_ => StepStack.Empty.Push(NoneStep.Instance))
                : AddStep(NoneStep.Instance);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> notTraversal)
        {
            var transformed = ContinueInner(notTraversal);
            
            return transformed.IsIdentity()
                ? None()
                : transformed.IsNone()
                    ? this
                    : AddStep(new NotStep(transformed.ToTraversal()));
        }

        private GremlinQuery<TTarget, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> OfType<TTarget>(IGraphElementModel model)
        {
            if (typeof(TTarget).IsAssignableFrom(typeof(TElement)))
                return Cast<TTarget>();

            var labels = model
                .TryGetFilterLabels(typeof(TTarget), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity)) ?? ImmutableArray.Create(typeof(TTarget).Name);

            return labels.Length > 0
                ? AddStep<TTarget>(new HasLabelStep(labels))
                : Cast<TTarget>();
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase
        {
            var optionalQuery = ContinueInner(optionalTraversal)
                .ToTraversal();

            return this
                .AddStep(new OptionalStep(optionalQuery), _ => _.Lowest(optionalQuery.Projection))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase>[] orTraversalTransformations)
        {
            return Or(orTraversalTransformations.Select(transformation => ContinueInner(transformation)).ToArray());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Or(params IGremlinQueryBase[] orTraversals)
        {
            if (orTraversals.Length == 0)
                return AddStep(OrStep.Infix);

            List<IGremlinQueryBase>? subQueries = default;

            foreach (var transformed in orTraversals)
            {
                if (transformed.IsIdentity())
                    return this;

                if (!transformed.IsNone())
                    (subQueries ??= new List<IGremlinQueryBase>()).Add(transformed);
            }

            var fusedTraversals = subQueries?
                .Select(x => x.ToTraversal().RewriteForWhereContext())
                .Fuse(
                    (p1, p2) => p1.Or(p2))
                .ToArray();

            return fusedTraversals?.Length switch
            {
                null or 0 => None(),
                1 => Where(fusedTraversals[0]),
                _ => AddStep(new OrStep(fusedTraversals))
            };
        }

        private TTargetQuery OrderGlobal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => Order(projection, OrderStep.Global);

        private TTargetQuery OrderLocal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => Order(projection, OrderStep.Local);

        private TTargetQuery Order<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection, OrderStep orderStep) where TTargetQuery : IGremlinQueryBase<TElement> => projection(new OrderBuilder(AddStep(orderStep))).Build();

        private GremlinQuery<TTarget, object, object, object, object, object> OtherV<TTarget>() => AddStepWithObjectTypes<TTarget>(OtherVStep.Instance, _ => Projection.Vertex);

        private GremlinQuery<TTarget, object, object, object, object, object> OutV<TTarget>() => AddStepWithObjectTypes<TTarget>(OutVStep.Instance, _ => Projection.Vertex);

        private GremlinQuery<TResult, object, object, object, object, object> Project<TResult>(Func<IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>, IProjectResult> continuation)
        {
            var enableEmptyProjectionValueProtection = Environment.Options
                .GetValue(GremlinqOption.EnableEmptyProjectionValueProtection);

            var projectionsPairs = continuation(new ProjectBuilder<TElement, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(this))
                .Projections
                .OrderBy(x => x.Key)
                .ToArray();

            var projectStep = new ProjectStep(projectionsPairs
                .Select(x => x.Key)
                .ToImmutableArray());

            var ret = this
                .AddStepWithObjectTypes<TResult>(
                    projectStep,
                    _ => _.Project(
                            projectStep,
                            projectionsPairs
                                .Select(x => x.Value)
                                .ToArray()));

            foreach (var projectionsPair in projectionsPairs)
            {
                var byStep = projectionsPair.Value;

                if (enableEmptyProjectionValueProtection)
                {
                    var byTraversalStep = byStep
                        .ToByTraversalStep();

                    byStep = new ProjectStep.ByTraversalStep(new Traversal(byTraversalStep.Traversal.Append(LimitStep.LimitGlobal1).Append(FoldStep.Instance), byTraversalStep.Traversal.Projection));
                }

                ret = ret.AddStep(byStep);
            }

            if (enableEmptyProjectionValueProtection)
            {
                ret = ret
                    .AddSteps(EmptyProjectionProtectionDecoratorSteps, _ => _);
            }

            return ret;
        }

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, params Expression[] projections)
        {
            return Properties<TNewElement, TNewPropertyValue, TNewMeta>(
                projection,
                projections
                    .Select(projection => GetKey(projection).RawKey)
                    .OfType<string>());
        }

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(Projection projection, IEnumerable<string> keys)
            => AddStep<TNewElement, object, object, TNewPropertyValue, TNewMeta, object>(
                new PropertiesStep(keys.ToImmutableArray()),
                _ => projection);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(LambdaExpression projection, object? value) => Property(GetKey(projection), value);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(Key key, object? value)
        {
            if (value == null)
            {
                if (key.RawKey is string stringKey)
                    return DropProperties(stringKey);

                throw new InvalidOperationException("Can't set a special property to null.");
            }

            var ret = this;

            foreach (var propertyStep in GetPropertySteps(key, value, Projection == Projection.Vertex))
            {
                ret = ret.AddStep(propertyStep);
            }

            return ret;
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(Key key, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> valueTraversal) => Property(key, ContinueInner(valueTraversal).ToTraversal());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(LambdaExpression projection, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> valueTraversal) => Property(projection, ContinueInner(valueTraversal).ToTraversal());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> RangeGlobal(long low, long high) => AddStep(new RangeStep(low, high, Scope.Global));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> RangeLocal(long low, long high)
        {
            return AddStep(new RangeStep(low, high, Scope.Local));
        }

        private TTargetQuery Repeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatContinuation)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatTraversal = ContinueInner(repeatContinuation)
                .ToTraversal();

            return this
                .AddStep(new RepeatStep(repeatTraversal), _ => _.Lowest(repeatTraversal.Projection))
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery RepeatUntil<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatContinuation, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> untilTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatTraversal = ContinueInner(repeatContinuation)
                .ToTraversal();

            var untilQuery = ContinueInner(untilTraversal);

            var ret = this
                .AddStep(new RepeatStep(repeatTraversal), _ => _.Lowest(repeatTraversal.Projection));

            if (!untilQuery.IsNone())
                ret = ret.AddStep(new UntilStep(untilQuery.ToTraversal()));

            return ret
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery UntilRepeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatContinuation, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> untilTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatTraversal = ContinueInner(repeatContinuation)
                .ToTraversal();

            var untilQuery = ContinueInner(untilTraversal);

            var ret = this;

            if (!untilQuery.IsNone())
                ret = ret.AddStep(new UntilStep(ContinueInner(untilTraversal).ToTraversal()));

            return ret
                .AddStep(new RepeatStep(repeatTraversal), _ => _.Lowest(repeatTraversal.Projection))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, object, object, object, object, object> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel)
        {
            if (StepLabelProjections.TryGetValue(stepLabel, out var stepLabelProjection))
                return AddStepWithObjectTypes<TSelectedElement>(new SelectStepLabelStep(ImmutableArray.Create<StepLabel>(stepLabel)), _ => stepLabelProjection);

            throw new InvalidOperationException($"Invalid use of unknown {nameof(StepLabel)} in {nameof(Select)}. Make sure you only pass in a {nameof(StepLabel)} that comes from a previous {nameof(As)}- or {nameof(IGremlinQuerySource.WithSideEffect)}-continuation or has previously been passed to an appropriate overload of {nameof(As)} or {nameof(IGremlinQuerySource.WithSideEffect)}.");
        }

        private TTargetQuery Select<TTargetQuery>(params Expression[] projections)
        {
            var keys = projections
                .Select(GetKey)
                .ToImmutableArray();

            return this
                .AddStep(
                    new SelectKeysStep(keys),
                    _ => _.If<TupleProjection>(tuple => tuple.Select(keys)))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, object, object, TArrayItem, object, TQuery> Cap<TSelectedElement, TArrayItem, TQuery>(StepLabel<IArrayGremlinQuery<TSelectedElement, TArrayItem, TQuery>, TSelectedElement> stepLabel) where TQuery : IGremlinQueryBase => AddStep<TSelectedElement, object, object, TArrayItem, object, TQuery>(new CapStep(stepLabel), _ => _.Fold());

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> sideEffectTraversal) => AddStep(new SideEffectStep(ContinueInner(sideEffectTraversal).ToTraversal()));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SimplePath() => AddStep(SimplePathStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Skip(long count, Scope scope) => AddStep(new SkipStep(count, scope));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SumGlobal() => AddStep(new SumStep(Scope.Global), _ => Projection.Value);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SumLocal() => AddStep<object>(new SumStep(Scope.Local));

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MinLocal() => AddStep<object>(new MinStep(Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Mute() => AddStep(NoneStep.Instance, additionalFlags: QueryFlags.IsMuted);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MinGlobal() => AddStep(new MinStep(Scope.Global), _ => Projection.Value);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MaxLocal() => AddStep<object>(new MaxStep(Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MaxGlobal() => AddStep(new MaxStep(Scope.Global), _ => Projection.Value);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MeanLocal() => AddStep<object>(new MeanStep(Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MeanGlobal() => AddStep(new MeanStep(Scope.Global), _ => Projection.Value);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> TailGlobal(long count) => AddStep(new TailStep(count, Scope.Global));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> TailLocal(long count)
        {
            return count == 1
                ? AddStep(TailStep.TailLocal1)
                : AddStep(new TailStep(count, Scope.Local));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Times(int count) => AddStep(new TimesStep(count));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>(new AddEStep.ToLabelStep(stepLabel));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Unfold() => AddStep(
            UnfoldStep.Instance,
            _ => _.If<ArrayProjection>(array => array.Unfold()));

        private TTargetQuery Unfold<TTargetQuery>() => Unfold().ChangeQueryType<TTargetQuery>();

        private TReturnQuery Union<TTargetQuery, TReturnQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals)
            where TTargetQuery : IGremlinQueryBase
            where TReturnQuery : IGremlinQueryBase
        {
            var unionQueries = unionTraversals
                .Select(unionTraversal => ContinueInner(unionTraversal))
                .ToArray();

            var aggregatedSemantics = unionQueries
                .Select(x => x.AsAdmin().Projection)
                .Aggregate((x, y) => x.Lowest(y));

            return this
                .AddStep(new UnionStep(unionQueries.Select(x => x.ToTraversal()).ToImmutableArray()), _ => aggregatedSemantics)
                .ChangeQueryType<TReturnQuery>();
        }

        private GremlinQuery<object, object, object, object, object, object> V(ImmutableArray<object> ids) => AddStepWithObjectTypes<object>(new VStep(ids), _ => Projection.Vertex);

        private GremlinQuery<object, object, object, object, object, object> E(ImmutableArray<object> ids) => AddStepWithObjectTypes<object>(new EStep(ids), _ => Projection.Edge);

        private IValueGremlinQuery<TNewPropertyValue> Value<TNewPropertyValue>() => AddStepWithObjectTypes<TNewPropertyValue>(ValueStep.Instance, _ => Projection.Value);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(ImmutableArray<string> keys) => AddStepWithObjectTypes<TNewElement>(new ValueMapStep(keys), _ => Projection.Value);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var projectionsArray = projections
                .ToArray<Expression>();

            var stringKeys = GetStringKeys(projectionsArray)
                .ToImmutableArray();

            return stringKeys.Length != projectionsArray.Length
                ? throw new ExpressionNotSupportedException($"One of the expressions in {nameof(ValueMap)} maps to a {nameof(T)}-token. Can't have special tokens in {nameof(ValueMap)}.")
                : AddStepWithObjectTypes<TNewElement>(new ValueMapStep(stringKeys), _ => Projection.Value);
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForKeys<TValue>(IEnumerable<Key> keys)
        {
            var stepsArray = GetStepsForKeys(keys)
                .ToArray();

            return stepsArray.Length switch
            {
                0 => throw new ExpressionNotSupportedException(),
                1 => AddStepWithObjectTypes<TValue>(stepsArray[0], _ => Projection.Value),
                _ => AddStepWithObjectTypes<TValue>(new UnionStep(stepsArray.Select(step => ContinueInner(__ => __.AddStep(step, _ => Projection.Value).ToTraversal())).ToImmutableArray()), _ => Projection.Value)
            };
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(projections.Select(projection => GetKey(projection)));

        private GremlinQuery<VertexProperty<TNewPropertyValue, TNewMeta>, object, object, TNewPropertyValue, TNewMeta, object> VertexProperties<TNewPropertyValue, TNewMeta>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue, TNewMeta>, TNewPropertyValue, TNewMeta>(Projection.VertexProperty, projections);

        private GremlinQuery<VertexProperty<TNewPropertyValue>, object, object, TNewPropertyValue, object, object> VertexProperties<TNewPropertyValue>(Expression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(Projection.VertexProperty, projections);
        
        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(ILambda lambda) => AddStep(new FilterStep(lambda));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> filterTraversal)
        {
            var filtered = ContinueInner(filterTraversal);

            return filtered.IsIdentity()
                ? this
                : filtered.IsNone()
                    ? None()
                    : Where(filtered.ToTraversal());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Traversal traversal)
        {
            traversal = traversal.RewriteForWhereContext();

            return traversal.Count > 0 && traversal.All(x => x is IIsOptimizableInWhere)
                ? AddSteps(traversal)
                : AddStep(new WhereTraversalStep(traversal));
        }

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
                            ContinueInner(__ => __.Where(binary.Left)),
                            ContinueInner(__ => __.Where(binary.Right)));
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
                            : AddSteps(Where(gremlinExpression));
                }
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(expression, ex);
            }

            throw new ExpressionNotSupportedException(expression);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where<TProjection>(Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal)
        {
            return predicate.TryGetReferredParameter() is not null && predicate.Body is MemberExpression memberExpression
                ? AddStep(
                    new HasTraversalStep(GetKey(memberExpression), Cast<TProjection>().ContinueInner(propertyTraversal).ToTraversal()),
                    _ => _)
                : throw new ExpressionNotSupportedException(predicate);
        }

        private IEnumerable<Step> Where(GremlinExpression gremlinExpression)
        {
            return Where(
                gremlinExpression.Left,
                gremlinExpression.LeftWellKnownMember,
                gremlinExpression.Semantics,
                gremlinExpression.Right);
        }

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
                                        this
                                            .ContinueInner(__ => __
                                                .AddStep(new WherePredicateStep(effectivePredicate)))
                                            .ToTraversal());

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
    }
}
