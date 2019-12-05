// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;
using LanguageExt;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public static class GremlinQuery
    {
        internal static readonly IImmutableList<Step> AnonymousNoneSteps = ImmutableList<Step>.Empty.Add(NoneStep.Instance);

        public static IGremlinQuery<Unit> Anonymous(IGremlinQueryEnvironment environment)
        {
            return Create<Unit>(
                ImmutableList<Step>.Empty,
                environment);
        }

        public static IGremlinQuery<TElement> Create<TElement>(IGremlinQueryEnvironment environment)
        {
            return Create<TElement>(
                ImmutableList<Step>.Empty,
                environment);
        }

        public static IGremlinQuery<TElement> Create<TElement>(IImmutableList<Step> steps, IGremlinQueryEnvironment environment)
        {
            return new GremlinQuery<TElement, Unit, Unit, Unit, Unit, Unit>(
                steps,
                environment);
        }
    }

    internal abstract class GremlinQueryBase
    {
        private static readonly ConcurrentDictionary<Type, Func<IImmutableList<Step>, IGremlinQueryEnvironment, IGremlinQuery>> QueryTypes = new ConcurrentDictionary<Type, Func<IImmutableList<Step>, IGremlinQueryEnvironment, IGremlinQuery>>();

        private static readonly Type[] SupportedInterfaceDefinitions = typeof(GremlinQuery<,,,,,>)
            .GetInterfaces()
            .Select(iface => iface.IsGenericType ? iface.GetGenericTypeDefinition() : iface)
            .ToArray();

        protected GremlinQueryBase(IImmutableList<Step> steps, IGremlinQueryEnvironment environment)
        {
            Steps = steps;
            Environment = environment;
        }

        protected TTargetQuery ChangeQueryType<TTargetQuery>()
        {
            var targetQueryType = typeof(TTargetQuery);
            var genericTypeDef = targetQueryType.IsGenericType
                ? targetQueryType.GetGenericTypeDefinition()
                : targetQueryType;

            if (!SupportedInterfaceDefinitions.Contains(genericTypeDef))
                throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");

            var constructor = QueryTypes.GetOrAdd(
                targetQueryType,
                closureType =>
                {
                    var genericType = typeof(GremlinQuery<,,,,,>).MakeGenericType(
                        GetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray"),
                        GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex"),
                        GetMatchingType(closureType, "TInVertex"),
                        GetMatchingType(closureType, "TValue"),
                        GetMatchingType(closureType, "TMeta"),
                        GetMatchingType(closureType, "TQuery"));

                    var stepsParameter = Expression.Parameter(typeof(IImmutableList<Step>));
                    var environmentParameter = Expression.Parameter(typeof(IGremlinQueryEnvironment));

                    return Expression
                        .Lambda<Func<IImmutableList<Step>, IGremlinQueryEnvironment, IGremlinQuery>>(
                            Expression.New(
                                genericType.GetConstructor(new[]
                                {
                                    stepsParameter.Type,
                                    environmentParameter.Type
                                }),
                                stepsParameter,
                                environmentParameter),
                            stepsParameter,
                            environmentParameter)
                        .Compile();
                });

            return (TTargetQuery)constructor(Steps, Environment);
        }

        private static Type GetMatchingType(Type interfaceType, params string[] argumentNames)
        {
            if (interfaceType.IsGenericType)
            {
                var genericArguments = interfaceType.GetGenericArguments();
                var genericTypeDefinitionArguments = interfaceType.GetGenericTypeDefinition().GetGenericArguments();

                foreach (var argumentName in argumentNames)
                {
                    for (var i = 0; i < genericTypeDefinitionArguments.Length; i++)
                    {
                        if (genericTypeDefinitionArguments[i].ToString() == argumentName)
                            return genericArguments[i];
                    }
                }
            }

            return typeof(Unit);
        }

        protected internal IImmutableList<Step> Steps { get; }
        protected internal IGremlinQueryEnvironment Environment { get; }
    }

    internal sealed partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> : GremlinQueryBase
    {
        public GremlinQuery(IImmutableList<Step> steps, IGremlinQueryEnvironment environment) : base(steps, environment)
        {

        }

        private GremlinQuery<TEdge, TElement, Unit, Unit, Unit, Unit> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, Unit, Unit, Unit, Unit>(new AddEStep(Environment.Model, newEdge))
                .AddOrUpdate(newEdge, true,false);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddOrUpdate(TElement element, bool add, bool allowExplicitCardinality)
        {
            var ret = this;
            var props = element.Serialize(
                Environment.Model.PropertiesModel,
                add
                    ? SerializationBehaviour.IgnoreOnAdd
                    : SerializationBehaviour.IgnoreOnUpdate);

            if (!add)
            {
                ret = ret.SideEffect(_ => _
                    .Properties<Unit, Unit, Unit>(props
                        .Select(p => p.identifier)
                        .OfType<string>())
                    .Drop());
            }

            foreach (var (property, identifier, value) in props)
            {
                foreach (var propertyStep in GetPropertySteps(property.PropertyType, identifier, value, allowExplicitCardinality))
                {
                    ret = ret.AddStep(propertyStep);
                }
            }

            return ret;
        }

        private IEnumerable<PropertyStep> GetPropertySteps(Type propertyType, object key, object value, bool allowExplicitCardinality)
        {
            if (!propertyType.IsArray || propertyType == typeof(byte[]))
                yield return GetPropertyStep(key, value, allowExplicitCardinality ? Cardinality.Single : default);
            else
            {
                if (!allowExplicitCardinality)
                    throw new InvalidOperationException(/*TODO */);

                // ReSharper disable once PossibleNullReferenceException
                if (propertyType.GetElementType().IsInstanceOfType(value))
                {
                    yield return GetPropertyStep(key, value, Cardinality.List);
                }
                else
                {
                    foreach (var item in (IEnumerable)value)
                    {
                        yield return GetPropertyStep(key, item, Cardinality.List);
                    }
                }
            }
        }

        private PropertyStep GetPropertyStep(object key, object value, Cardinality cardinality)
        {
            var metaProperties = Array.Empty<object>();

            if (value is IVertexProperty && value is Property property)
            {
                metaProperties = property
                    .GetMetaProperties(Environment.Model.PropertiesModel)
                    .SelectMany(kvp => new[] { kvp.Key, kvp.Value })
                    .ToArray();

                value = property.GetValue();
            }

            return new PropertyStep(key, value, metaProperties, cardinality);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep(Step step) => AddStep<TElement>(step);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step) => AddStep<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(step);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Step step) => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Steps.Insert(Steps.Count, step), Environment);

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> AddStepWithUnitTypes<TNewElement>(Step step) => AddStep<TNewElement, Unit, Unit, Unit, Unit, Unit>(step);

        private GremlinQuery<TVertex, Unit, Unit, Unit, Unit, Unit> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStepWithUnitTypes<TVertex>(new AddVStep(Environment.Model, vertex))
                .AddOrUpdate(vertex, true, true);
        }

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                AddStep(new AggregateStep(stepLabel)),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] andTraversalTransformations)
        {
            if (andTraversalTransformations.Length == 0)
                return AddStep(AndStep.Infix);

            var anonymous = Anonymize();
            var subQueries = default(List<IGremlinQuery>);

            foreach (var transformation in andTraversalTransformations)
            {
                var transformed = transformation(anonymous);

                if (transformed.IsNone())
                    return None();

                if (transformed != anonymous)
                    (subQueries ??= new List<IGremlinQuery>()).Add(transformed);
            }

            return (subQueries?.Count).GetValueOrDefault() == 0
                ? this
                : AddStep(new AndStep(subQueries.ToArray()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Anonymize() => Anonymize<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Anonymize<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>() => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(ImmutableList<Step>.Empty, Environment);

        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                As(stepLabel),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> As(params StepLabel[] stepLabels) => AddStep(new AsStep(stepLabels));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Barrier() => AddStep(BarrierStep.Instance);

        private GremlinQuery<TTarget, Unit, Unit, Unit, Unit, Unit> BothV<TTarget>() => AddStepWithUnitTypes<TTarget>(BothVStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> By(Expression<Func<TElement, object>> projection, Order order)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new OrderStep.ByMemberStep(Environment.Model.PropertiesModel.GetIdentifier(memberExpression.Member), order));

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> traversal, Order order) => AddStep(new OrderStep.ByTraversalStep(traversal(Anonymize()), order));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> By(ILambda lambda) => AddStep(new OrderStep.ByLambdaStep(lambda));

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Cast<TNewElement>() => Cast<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Cast<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>() => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Steps, Environment);

        private TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Option<Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>> maybeFalseChoice = default) where TTargetQuery : IGremlinQuery
        {
            var gremlinExpression = predicate.ToGremlinExpression();

            if (gremlinExpression is TerminalGremlinExpression terminal)
            {
                if (terminal.Key == terminal.Parameter)
                {
                    var anonymous = Anonymize();
                    var trueQuery = trueChoice(anonymous);
                    var maybeFalseQuery = maybeFalseChoice.Map(falseChoice => (IGremlinQuery)falseChoice(anonymous));

                    return this
                        .AddStep(new ChoosePredicateStep(terminal.Predicate, trueQuery, maybeFalseQuery))
                        .ChangeQueryType<TTargetQuery>();
                }
            }

            throw new ExpressionNotSupportedException(predicate);
        }

        private TTargetQuery Choose<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Option<Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>> maybeFalseChoice = default) where TTargetQuery : IGremlinQuery
        {
            var anonymous = Anonymize();
            var trueQuery = trueChoice(anonymous);
            var maybeFalseQuery = maybeFalseChoice.Map(falseChoice => (IGremlinQuery)falseChoice(anonymous));

            return maybeFalseQuery
                .BiFold(
                    AddStep(new ChooseTraversalStep(traversalPredicate(anonymous), trueQuery, maybeFalseQuery)),
                    (query, falseQuery) => query,
                    (query, _) => query)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQuery
        {
            return continuation(ChooseBuilder.Create(Anonymize(), this)).TargetQuery;
        }

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>[] traversals)
            where TTargetQuery : IGremlinQuery
        {
            var coalesceQueries = traversals
                .Select(traversal => (IGremlinQuery)traversal(Anonymize()))
                .ToArray();

            return this
                .AddStep(new CoalesceStep(coalesceQueries))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Coin(double probability) => AddStep(new CoinStep(probability));

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> ConfigureSteps<TNewElement>(Func<IImmutableList<Step>, IImmutableList<Step>> configurator) => new GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(configurator(Steps), Environment);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Dedup() => AddStep(DedupStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Drop() => AddStep(DropStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> DropProperties(string key)
        {
            return SideEffect(_ => _
                .Properties<Unit, Unit, Unit>(new[] { key })
                .Drop());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new FlatMapStep(mappedTraversal))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement[], Unit, Unit, Unit, Unit, TNewFoldedQuery> Fold<TNewFoldedQuery>() => AddStep<TElement[], Unit, Unit, Unit, Unit, TNewFoldedQuery>(FoldStep.Instance);

        private GremlinQuery<IDictionary<TKey, TValue>, Unit, Unit, Unit, Unit, Unit> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IGremlinQuery, TKey, TValue>> projection)
        {
            var group = projection(GroupBuilder.Create(Anonymize()));

            return this
                .AddStep<IDictionary<TKey, TValue>, Unit, Unit, Unit, Unit, Unit>(GroupStep.Instance)
                .AddStep(new GroupStep.ByTraversalStep(group.KeyQuery))
                .AddStep(new GroupStep.ByTraversalStep(group.ValueQuery));
        }

        private GremlinQuery<IDictionary<TKey, object>, Unit, Unit, Unit, Unit, Unit> Group<TKey>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IGroupBuilderWithKey<IGremlinQuery, TKey>> projection)
        {
            var group = projection(GroupBuilder.Create(this.Anonymize()));

            return this
                .AddStep<IDictionary<TKey, object>, Unit, Unit, Unit, Unit, Unit>(GroupStep.Instance)
                .AddStep(new GroupStep.ByTraversalStep(group.KeyQuery));
        }

        private IAsyncEnumerator<TResult> GetAsyncEnumerator<TResult>(CancellationToken ct = default)
        {
            return Environment.Pipeline
                .Execute(this.Cast<TResult>())
                .GetAsyncEnumerator(ct);
        }

        private object[] GetKeys(IEnumerable<LambdaExpression> projections)
        {
            return GetKeys(projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                        return memberExpression;

                    throw new ExpressionNotSupportedException(projection);
                }));
        }

        private object[] GetKeys(IEnumerable<MemberExpression> projections)
        {
            return projections
                .Select(projection => Environment.Model.PropertiesModel.GetIdentifier(projection.Member))
                .ToArray();
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IEnumerable<Step> GetStepsForKeys(object[] keys)
        {
            var hasYielded = false;

            foreach (var t in keys.OfType<T>())
            {
                if (T.Id.Equals(t))
                    yield return IdStep.Instance;
                else if (T.Label.Equals(t))
                    yield return LabelStep.Instance;
                else
                    throw new ExpressionNotSupportedException();

                hasYielded = true;
            }

            var stringKeys = keys
                .OfType<string>()
                .ToArray();

            if (stringKeys.Length > 0 || !hasYielded)
                yield return new ValuesStep(stringKeys);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(Expression expression, P predicate)
        {
            if (predicate.EqualsConstant(false))
                return None();

            if (expression is MemberExpression memberExpression)
                return AddStep(new HasStep(Environment.Model.PropertiesModel.GetIdentifier(memberExpression.Member), predicate));

            throw new ExpressionNotSupportedException(expression);//TODO: Lift?
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(Expression expression, IGremlinQuery traversal)
        {
            if (expression is MemberExpression memberExpression)
                return AddStep(new HasStep(Environment.Model.PropertiesModel.GetIdentifier(memberExpression.Member), traversal));

            throw new ExpressionNotSupportedException(expression);//TODO: Lift?
        }

        private GremlinQuery<object, Unit, Unit, Unit, Unit, Unit> Id() => AddStepWithUnitTypes<object>(IdStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Identity() => this;

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStepWithUnitTypes<TNewElement>(new InjectStep(elements.Cast<object>().ToArray()));

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> InV<TNewElement>() => AddStepWithUnitTypes<TNewElement>(InVStep.Instance);

        private GremlinQuery<string, Unit, Unit, Unit, Unit, Unit> Key() => AddStepWithUnitTypes<string>(KeyStep.Instance);

        private GremlinQuery<string, Unit, Unit, Unit, Unit, Unit> Label() => AddStepWithUnitTypes<string>(LabelStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Limit(long count)
        {
            return AddStep(count == 1
                ? LimitStep.LimitGlobal1
                : new LimitStep(count, Scope.Global));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> LimitLocal(long count)
        {
            return AddStep(count == 1
                ? LimitStep.LimitLocal1
                : new LimitStep(count, Scope.Local));
        }

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQuery
        {
            var localTraversalQuery = localTraversal(Anonymize());

            return this
                .AddStep(new LocalStep(localTraversalQuery))
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new MapStep(mappedTraversal))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Match(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] matchTraversals) => AddStep(new MatchStep(matchTraversals.Select(traversal => traversal(Anonymize()))));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> None()
        {
            return this.IsIdentity()
                ? new GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(GremlinQuery.AnonymousNoneSteps, Environment)
                : AddStep(NoneStep.Instance);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> notTraversal)
        {
            var transformed = notTraversal(Anonymize());

            return transformed.IsIdentity()
                ? None()
                : transformed.IsNone()
                    ? this
                    : AddStep(new NotStep(transformed));
        }

        private GremlinQuery<TTarget, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OfType<TTarget>(IGraphElementModel model)
        {
            if (typeof(TTarget).IsAssignableFrom(typeof(TElement)))
                return Cast<TTarget>();

            return model
                .TryGetFilterLabels(typeof(TTarget), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))
                .Match(
                    labels => labels.Length > 0
                        ? Steps.Count > 0 && Steps[Steps.Count - 1] is HasLabelStep
                            ? ConfigureSteps<TTarget>(steps => steps.SetItem(steps.Count - 1, new HasLabelStep(labels)))
                            : AddStep<TTarget>(new HasLabelStep(labels))
                        : Cast<TTarget>(),
                    () => AddStep<TTarget>(NoneStep.Instance));
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery
        {
            var optionalQuery = optionalTraversal(Anonymize());

            return this
                .AddStep(new OptionalStep(optionalQuery))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] orTraversalTransformations)
        {
            if (orTraversalTransformations.Length == 0)
                return AddStep(OrStep.Infix);

            var anonymous = Anonymize();
            var subQueries = default(List<IGremlinQuery>);

            foreach (var transformation in orTraversalTransformations)
            {
                var transformed = transformation(anonymous);

                if (transformed == anonymous)
                    return this;

                if (!transformed.IsNone())
                    (subQueries ??= new List<IGremlinQuery>()).Add(transformed);
            }

            return (subQueries?.Count).GetValueOrDefault() == 0
                ? None()
                : AddStep(new OrStep(subQueries.ToArray()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OrderBy(Expression<Func<TElement, object>> projection, Order order) => AddStep(OrderStep.Instance).By(projection, order);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OrderBy(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> traversal, Order order) => AddStep(OrderStep.Instance).By(traversal, order);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OrderBy(ILambda lambda) => AddStep(OrderStep.Instance).By(lambda);

        private GremlinQuery<TTarget, Unit, Unit, Unit, Unit, Unit> OtherV<TTarget>() => AddStepWithUnitTypes<TTarget>(OtherVStep.Instance);

        private GremlinQuery<TTarget, Unit, Unit, Unit, Unit, Unit> OutV<TTarget>() => AddStepWithUnitTypes<TTarget>(OutVStep.Instance);

        private GremlinQuery<TResult, Unit, Unit, Unit, Unit, Unit> Project<TActualElement, TResult>(Func<IProjectBuilder<GremlinQuery<TActualElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TActualElement>, IProjectResult> continuation)
        {
            var projections = continuation(ProjectBuilder.Create<GremlinQuery<TActualElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TActualElement>(Anonymize().Cast<TActualElement>()))
                .Projections
                .OrderBy(x => x.Key)
                .ToArray();

            var ret = this
                .AddStepWithUnitTypes<TResult>(new ProjectStep(projections.Select(x => x.Key).ToArray()));

            foreach (var projection in projections)
            {
                ret = ret.AddStep(new ProjectStep.ByTraversalStep(projection.Value));
            }

            return ret;
        }

        private GremlinQuery<TTuple, Unit, Unit, Unit, Unit, Unit> Project<TTuple>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] projections)
        {
            var ret = this
                .AddStepWithUnitTypes<TTuple>(new ProjectStep(Enumerable.Range(1, projections.Length).Select(i => $"Item{i}").ToArray()));

            foreach (var projection in projections)
            {
                ret = ret.AddStep(new ProjectStep.ByTraversalStep(projection(Anonymize())));
            }

            return ret;
        }

        private GremlinQuery<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit> Properties<TNewElement, TNewPropertyValue, TNewMeta>(params LambdaExpression[] projections)
        {
            return Properties<TNewElement, TNewPropertyValue, TNewMeta>(projections
                .Select(projection => projection.GetMemberInfo().Name));
        }

        private GremlinQuery<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit> Properties<TNewElement, TNewPropertyValue, TNewMeta>(IEnumerable<string> keys) => AddStep<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit>(new PropertiesStep(keys.ToArray()));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, [AllowNull] object value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression && Environment.Model.PropertiesModel.GetIdentifier(memberExpression.Member) is string identifier)
                return Property(identifier, value);

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Property(string key, [AllowNull] object value)
        {
            return value == null
                ? DropProperties(key)
                : AddStep(new PropertyStep(key, value));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Range(long low, long high) => AddStep(new RangeStep(low, high));

        private TTargetQuery Repeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal)
            where TTargetQuery : IGremlinQuery
        {
            var repeatQuery = repeatTraversal(Anonymize());

            return this
                .AddStep(new RepeatStep(repeatQuery))
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery RepeatUntil<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> untilTraversal)
            where TTargetQuery : IGremlinQuery
        {
            var anonymous = Anonymize();
            var repeatQuery = repeatTraversal(anonymous);

            return this
                .AddStep(new RepeatStep(repeatQuery))
                .AddStep(new UntilStep(untilTraversal(anonymous)))
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery UntilRepeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> untilTraversal)
            where TTargetQuery : IGremlinQuery
        {
            var anonymous = Anonymize();
            var repeatQuery = repeatTraversal(anonymous);

            return this
                .AddStep(new UntilStep(untilTraversal(anonymous)))
                .AddStep(new RepeatStep(repeatQuery))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, Unit, Unit, Unit, Unit, Unit> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel) => AddStepWithUnitTypes<TSelectedElement>(new SelectStep(stepLabel));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> sideEffectTraversal) => AddStep(new SideEffectStep(sideEffectTraversal(Anonymize())));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Skip(long count) => AddStep(new SkipStep(count));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SumGlobal() => AddStep(SumStep.Global);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SumLocal() => AddStep(SumStep.Local);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Tail(long count) => AddStep(new TailStep(count, Scope.Global));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> TailLocal(long count) => AddStep(new TailStep(count, Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Times(int count) => AddStep(new TimesStep(count));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit>(new AddEStep.ToLabelStep(stepLabel));

        private TTargetQuery Unfold<TTargetQuery>() => AddStep(UnfoldStep.Instance).ChangeQueryType<TTargetQuery>();

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery
        {
            var unionQueries = unionTraversals
                .Select(unionTraversal => (IGremlinQuery)unionTraversal(Anonymize()))
                .ToArray();

            return this
                .AddStep(new UnionStep(unionQueries))
                .ChangeQueryType<TTargetQuery>();
        }

        private IValueGremlinQuery<TNewPropertyValue> Value<TNewPropertyValue>() => AddStepWithUnitTypes<TNewPropertyValue>(ValueStep.Instance);

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> ValueMap<TNewElement>(string[] keys) => AddStepWithUnitTypes<TNewElement>(new ValueMapStep(keys));

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var projectionsArray = projections
                .ToArray();

            var stringKeys = GetKeys(projectionsArray)
                .OfType<string>()
                .ToArray();

            if (stringKeys.Length != projectionsArray.Length)
                throw new ExpressionNotSupportedException();

            return AddStepWithUnitTypes<TNewElement>(new ValueMapStep(stringKeys));
        }

        private GremlinQuery<TValue, Unit, Unit, Unit, Unit, Unit> ValuesForKeys<TValue>(object[] keys)
        {
            var stepsArray = GetStepsForKeys(keys)
                .ToArray();

            switch (stepsArray.Length)
            {
                case 0:
                    throw new ExpressionNotSupportedException();
                case 1:
                    return AddStepWithUnitTypes<TValue>(stepsArray[0]);
                default:
                    return AddStepWithUnitTypes<TValue>(new UnionStep(stepsArray.Select(step => Anonymize().AddStep(step))));
            }
        }

        private GremlinQuery<TValue, Unit, Unit, Unit, Unit, Unit> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(GetKeys(projections));

        private GremlinQuery<VertexProperty<TNewPropertyValue>, Unit, Unit, TNewPropertyValue, Unit, Unit> VertexProperties<TNewPropertyValue>(LambdaExpression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, Unit>(projections);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> VertexProperty(LambdaExpression projection, [AllowNull] object value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
            {
                var identifier = Environment.Model.PropertiesModel.GetIdentifier(memberExpression.Member);

                if (value == null)
                {
                    if (identifier is string stringKey)
                        return DropProperties(stringKey);
                }
                else
                {
                    var ret = this;

                    foreach(var propertyStep in GetPropertySteps(memberExpression.Type, identifier, value, true))
                    {
                        ret = ret.AddStep(propertyStep);
                    }

                    return ret;
                }
            }

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(ILambda lambda) => AddStep(new FilterStep(lambda));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> filterTraversal)
        {
            var anonymous = Anonymize();
            var filtered = filterTraversal(anonymous);

            return filtered.IsIdentity()
                ? this
                : filtered.IsNone()
                    ? None()
                    : AddStep(new WhereTraversalStep(filtered));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(LambdaExpression predicate)
        {
            try
            {
                return Where(predicate.ToGremlinExpression());
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(ex);
            }
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where<TProjection>(Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Has(predicate.Body, propertyTraversal(Anonymize<TProjection, Unit, Unit, Unit, Unit, Unit>()));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(GremlinExpression gremlinExpression)
        {
            if (gremlinExpression is OrGremlinExpression or)
            {
                return Or(
                    _ => _.Where(or.Operand1),
                    _ => _.Where(or.Operand2));
            }

            if (gremlinExpression is AndGremlinExpression and)
            {
                return this
                    .Where(and.Operand1)
                    .Where(and.Operand2);
            }

            if (gremlinExpression is NotGremlinExpression not)
                return Not(_ => _.Where(not.Negate()));

            if (gremlinExpression is TerminalGremlinExpression terminal)
            {
                var effectivePredicate = terminal.Predicate is TextP textP
                    ? textP.WorkaroundLimitations(Environment.Options)
                    : terminal.Predicate;

                switch (terminal.Key)
                {
                    case MemberExpression leftMemberExpression:
                    {
                        var leftMemberExpressionExpression = leftMemberExpression.Expression.StripConvert();

                        if (leftMemberExpressionExpression == terminal.Parameter)
                        {
                            // x => x.Value == P.xy(...)
                            if (leftMemberExpression.IsPropertyValue())
                                return AddStep(new HasValueStep(effectivePredicate));

                            if (leftMemberExpression.IsPropertyKey())
                                return Where(__ => __.Key().Where(effectivePredicate));

                            if (leftMemberExpression.IsVertexPropertyLabel())
                                return Where(__ => __.Label().Where(effectivePredicate));
                        }
                        else if (leftMemberExpressionExpression is MemberExpression leftLeftMemberExpression) 
                        {
                            // x => x.Name.Value == P.xy(...)
                            if (leftMemberExpression.IsPropertyValue())
                                leftMemberExpression = leftLeftMemberExpression;    //TODO: What else ?
                        }
                        else
                            break;

                        // x => x.Name == P.xy(...)
                        return Where(leftMemberExpression, effectivePredicate);
                    }
                    case ParameterExpression leftParameterExpression when terminal.Parameter == leftParameterExpression:
                    {
                        // x => x == P.xy(...)
                        return Where(effectivePredicate);
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        var targetExpression = methodCallExpression.Object;

                        if (targetExpression != null && typeof(IDictionary<string, object>).IsAssignableFrom(targetExpression.Type) && methodCallExpression.Method.Name == "get_Item")
                        {
                            return AddStep(new HasStep(methodCallExpression.Arguments[0].GetValue(), effectivePredicate));
                        }

                        break;
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(MemberExpression expression, P predicate)
        {
            return predicate.ContainsOnlyStepLabels()
                ? Has(expression, Anonymize().Where(predicate))
                : Has(expression, predicate);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(P predicate)
        {
            return predicate.ContainsOnlyStepLabels()
                ? AddStep(new WherePredicateStep(predicate))
                : AddStep(new IsStep(predicate));
        }
    }
}
