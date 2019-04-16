// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using ExRam.Gremlinq.Core.Attributes;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public abstract class GremlinQuery
    {
        private static readonly ConcurrentDictionary<Type, Type> QueryTypes = new ConcurrentDictionary<Type, Type>();
        private static readonly Type[] SupportedInterfaceDefinitions = typeof(GremlinQuery<,,,,,>)
            .GetInterfaces()
            .Select(iface => iface.IsGenericType ? iface.GetGenericTypeDefinition() : iface)
            .ToArray();

        protected GremlinQuery(IGraphModel model, IGremlinQueryExecutor queryExecutor, IImmutableList<Step> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings, ILogger logger)
        {
            Model = model;
            Steps = steps;
            Logger = logger;
            QueryExecutor = queryExecutor;
            StepLabelMappings = stepLabelBindings;
        }

        public static IGremlinQuery<Unit> Anonymous(IGraphModel model = null, ILogger logger = null)
        {
            return Create(model ?? GraphModel.Empty, GremlinQueryExecutor.Invalid, null, logger);
        }

        protected TTargetQuery ChangeQueryType<TTargetQuery>()
        {
            var targetQueryType = typeof(TTargetQuery);
            var genericTypeDef = targetQueryType.IsGenericType
                ? targetQueryType.GetGenericTypeDefinition()
                : targetQueryType;

            if (!SupportedInterfaceDefinitions.Contains(genericTypeDef))
                throw new NotSupportedException($"Cannot change the query type to {targetQueryType}.");

            var type = QueryTypes.GetOrAdd(
                targetQueryType,
                closureType => typeof(GremlinQuery<,,,,,>).MakeGenericType(
                    GetMatchingType(closureType, "TElement", "TVertex", "TEdge", "TProperty", "TArray"),
                    GetMatchingType(closureType, "TOutVertex", "TAdjacentVertex"),
                    GetMatchingType(closureType, "TInVertex"),
                    GetMatchingType(closureType, "TValue"),
                    GetMatchingType(closureType, "TMeta"),
                    GetMatchingType(closureType, "TQuery")));

            return (TTargetQuery)Activator.CreateInstance(type, Model, QueryExecutor, Steps, StepLabelMappings, Logger);
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

        internal static IGremlinQuery<Unit> Create(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = null, ILogger logger = null)
        {
            return Create<Unit>(model, queryExecutor, graphName, logger);
        }

        internal static IGremlinQuery<TElement> Create<TElement>(IGraphModel model, IGremlinQueryExecutor queryExecutor, string graphName = null, ILogger logger = null)
        {
            return new GremlinQuery<TElement, Unit, Unit, Unit, Unit, Unit>(
                model,
                queryExecutor,
                graphName != null
                    ? ImmutableList<Step>.Empty.Add(IdentifierStep.Create(graphName))
                    : ImmutableList<Step>.Empty,
                ImmutableDictionary<StepLabel, string>.Empty,
                logger);
        }

        protected ILogger Logger { get; }
        protected IGraphModel Model { get; }
        protected IImmutableList<Step> Steps { get; }
        protected IGremlinQueryExecutor QueryExecutor { get; }
        protected IImmutableDictionary<StepLabel, string> StepLabelMappings { get; }
    }

    internal sealed partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> : GremlinQuery
    {
        public GremlinQuery(IGraphModel model, IGremlinQueryExecutor queryExecutor, IImmutableList<Step> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings, ILogger logger) : base(model, queryExecutor, steps, stepLabelBindings, logger)
        {
        }

        private GremlinQuery<TEdge, TElement, Unit, Unit, Unit, Unit> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, Unit, Unit, Unit, Unit>(new AddEStep(Model, newEdge))
                .AddElementProperties(newEdge, false);
        }

        private GremlinQuery<TEdge, TElement, Unit, Unit, Unit, Unit> UpdateE<TEdge>(TEdge edge)
        {
            return this.AddElementPropertiesForUpdate<TEdge, TElement>(edge, (param) => false, false);
        }

        private GremlinQuery<TEdge, TElement, Unit, Unit, Unit, Unit> UpdateE<TEdge>(TEdge edge, Func<string, bool> excludePropertyFilter)
        {
            return this.AddElementPropertiesForUpdate<TEdge, TElement>(edge, excludePropertyFilter, false);
        }

        private GremlinQuery<TEdge, TElement, Unit, Unit, Unit, Unit> UpdateE<TEdge>(TEdge edge, ImmutableList<string> excludeFromUpdate)
        {
            return this.AddElementPropertiesForUpdate<TEdge, TElement>(edge, (param) => excludeFromUpdate != null && excludeFromUpdate.Contains(param), false);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddElementProperties(object element, bool allowExplicitCardinality)
        {
            var ret = this;

            foreach (var (propertyInfo, value) in element.Serialize())
            {
                foreach (var propertyStep in GetPropertySteps(propertyInfo.PropertyType, Model.GetIdentifier(Expression.Property(Expression.Constant(element), propertyInfo)), value, allowExplicitCardinality))
                {
                    ret = ret.AddStep(propertyStep);
                }
            }

            return ret;
        }

        private IEnumerable<PropertyStep> GetPropertySteps(Type propertyType, object key, object value, bool allowExplicitCardinality)
        {
            if (value != null)
            {
                if (!propertyType.IsArray || propertyType == typeof(byte[]))
                    yield return new PropertyStep(allowExplicitCardinality ? Cardinality.Single : default, key, value);
                else
                {
                    if (!allowExplicitCardinality)
                        throw new InvalidOperationException(/*TODO */);

                    // ReSharper disable once PossibleNullReferenceException
                    if (propertyType.GetElementType().IsInstanceOfType(value))
                    {
                        yield return new PropertyStep(Cardinality.List, key, value);
                    }
                    else
                    {
                        foreach (var item in (IEnumerable)value)
                        {
                            yield return new PropertyStep(Cardinality.List, key, item);
                        }
                    }
                }
            }
        }

        private GremlinQuery<TNewElement, TNewOutVertex, Unit, Unit, Unit, Unit> AddElementPropertiesForUpdate<TNewElement, TNewOutVertex>(object element, Func<string, bool> filter, bool allowExplicitCardinality)
        {
            var elementType = element.GetType();

            // Only pull back the properties without the ReadOnly attribute
            var props = element.Serialize()
                .Where(p => !Attribute.IsDefined(p.Item1, typeof(ReadOnlyAttribute)) && !filter(p.Item1.Name));

            // Drop the properties we found from the existing item
            var drop = Anonymize().Properties<Unit, Unit, Unit>(props.Select(p => p.Item1.Name))
                       .Drop();

            var ret = AddStep<TNewElement, TNewOutVertex, Unit, Unit, Unit, Unit>(new SideEffectStep(drop));

            // Re-add the properties
            foreach (var (propertyInfo, value) in props)
            {
                foreach (var propertyStep in GetPropertySteps(propertyInfo.PropertyType, Model.GetIdentifier(Expression.Property(Expression.Constant(element), propertyInfo)), value, allowExplicitCardinality))
                {
                    ret = ret.AddStep(propertyStep);
                }
            }

            return ret;
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep(Step step) => AddStep<TElement>(step);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step) => AddStep<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(step);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Step step) => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Model, QueryExecutor, Steps.Insert(Steps.Count, step), StepLabelMappings, Logger);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStepLabelBindings(params StepLabel[] stepLabels)
        {
            var ret = this;

            for (var i = 0; i < stepLabels.Length; i++)
            {
                ret = ret.AddStepLabelBinding(stepLabels[i], $"Item{i + 1}");
            }

            return ret;
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStepLabelBinding(StepLabel stepLabel, string name)
        {
            if (StepLabelMappings.TryGetValue(stepLabel, out var existingName) && existingName != name)
                throw new InvalidOperationException($"A StepLabel was already bound to {name} by a previous Select operation. Try changing the position of the StepLabel in the Select operation or introduce a new StepLabel.");

            return new GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(Model, QueryExecutor, Steps, StepLabelMappings.Add(stepLabel, name), Logger);
        }

        private GremlinQuery<TVertex, Unit, Unit, Unit, Unit, Unit> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStep<TVertex, Unit, Unit, Unit, Unit, Unit>(new AddVStep(Model, vertex))
                .AddElementProperties(vertex, true);
        }

        private GremlinQuery<TVertex, Unit, Unit, Unit, Unit, Unit> UpdateV<TVertex>(TVertex vertex)
        {
            return this.AddElementPropertiesForUpdate<TVertex, Unit>(vertex, (param) => false, true);
        }

        private GremlinQuery<TVertex, Unit, Unit, Unit, Unit, Unit> UpdateV<TVertex>(TVertex vertex, Func<string, bool> excludePropertyFilter)
        {
            return this.AddElementPropertiesForUpdate<TVertex, Unit>(vertex, excludePropertyFilter, true);
        }

        private GremlinQuery<TVertex, Unit, Unit, Unit, Unit, Unit> UpdateV<TVertex>(TVertex vertex, ImmutableList<string> excludeFromUpdate)
        {
            return this.AddElementPropertiesForUpdate<TVertex, Unit>(vertex, (param) => excludeFromUpdate != null && excludeFromUpdate.Contains(param), true);
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] andTraversals)
        {
            return AddStep(new AndStep(andTraversals
                .Select(andTraversal => andTraversal(Anonymize()))
                .ToArray()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Anonymize() => Anonymize<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Anonymize<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>() => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Model, GremlinQueryExecutor.Invalid, ImmutableList<Step>.Empty, ImmutableDictionary<StepLabel, string>.Empty, Logger);

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

        private GremlinQuery<TTarget, Unit, Unit, Unit, Unit, Unit> BothV<TTarget>() => AddStep<TTarget, Unit, Unit, Unit, Unit, Unit>(BothVStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> By(Expression<Func<TElement, object>> projection, Order order)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new ByMemberStep(Model.GetIdentifier(memberExpression), order));

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> traversal, Order order) => AddStep(new ByTraversalStep(traversal(Anonymize()), order));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> By(string lambda) => AddStep(new ByLambdaStep(new Lambda(lambda)));

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Cast<TNewElement>() => Cast<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Cast<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>() => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Model, QueryExecutor, Steps, StepLabelMappings, Logger);

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
                    (query, falseQuery) => query.MergeStepLabelMappings(trueQuery, falseQuery),
                    (query, _) => query)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>[] traversals)
            where TTargetQuery : IGremlinQuery
        {
            var coalesceQueries = traversals
                .Select(traversal => (IGremlinQuery)traversal(Anonymize()))
                .ToArray();

            return this
                .AddStep(new CoalesceStep(coalesceQueries))
                .MergeStepLabelMappings(coalesceQueries)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Coin(double probability) => AddStep(new CoinStep(probability));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Dedup() => AddStep(DedupStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Drop() => AddStep(DropStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> DropProperties(string key)
        {
            return SideEffect(_ => _
                .Properties<Unit, Unit, Unit>(new[] { key })
                .Drop());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private Task<TElement> First(CancellationToken ct) => ((IAsyncEnumerable<TElement>)Limit(1)).First(ct);

        private Task<TElement> FirstOrDefault(CancellationToken ct) => ((IAsyncEnumerable<TElement>)Limit(1)).FirstOrDefault(ct);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new FlatMapStep(mappedTraversal))
                .MergeStepLabelMappings(mappedTraversal)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement[], Unit, Unit, Unit, Unit, TNewFoldedQuery> Fold<TNewFoldedQuery>() => AddStep<TElement[], Unit, Unit, Unit, Unit, TNewFoldedQuery>(FoldStep.Instance);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit> From<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> fromVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize())));

        private IAsyncEnumerator<TResult> GetEnumerator<TResult>()
        {
            return QueryExecutor
                .Execute(this.Cast<TResult>())
                .GetEnumerator();
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
                .Select(projection => Model.GetIdentifier(projection))
                .ToArray();
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IEnumerable<Step> GetStepsForKeys(object[] keys)
        {
            var hasYielded = false;

            foreach (var t in keys.OfType<T>())
            {
                if (t == T.Id)
                    yield return IdStep.Instance;
                else if (t == T.Label)
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(Expression expression, P predicate) => AddStep(new HasStep(Model.GetIdentifier(expression), predicate));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(Expression expression, IGremlinQuery traversal) => AddStep(new HasStep(Model.GetIdentifier(expression), traversal));

        private GremlinQuery<object, Unit, Unit, Unit, Unit, Unit> Id() => AddStep<object, Unit, Unit, Unit, Unit, Unit>(IdStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Identity() => AddStep(IdentityStep.Instance);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().ToArray()));

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> InV<TNewElement>() => AddStep<TNewElement, Unit, Unit, Unit, Unit, Unit>(InVStep.Instance);

        private GremlinQuery<string, Unit, Unit, Unit, Unit, Unit> Key() => AddStep<string, Unit, Unit, Unit, Unit, Unit>(KeyStep.Instance);

        private GremlinQuery<string, Unit, Unit, Unit, Unit, Unit> Label() => AddStep<string, Unit, Unit, Unit, Unit, Unit>(LabelStep.Instance);

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
                .MergeStepLabelMappings(localTraversalQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new MapStep(mappedTraversal))
                .MergeStepLabelMappings(mappedTraversal)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Match(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] matchTraversals) => AddStep(new MatchStep(matchTraversals.Select(traversal => traversal(Anonymize()))));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> MergeStepLabelMappings(params IGremlinQuery[] queries)
        {
            var ret = this;

            foreach (var query in queries)
            {
                foreach (var otherMapping in query.AsAdmin().StepLabelMappings)
                {
                    ret = ret.AddStepLabelBinding(otherMapping.Key, otherMapping.Value);
                }
            }

            return ret;
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> None() => AddStep(NoneStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> notTraversal) => AddStep(new NotStep(notTraversal(Anonymize())));

        private GremlinQuery<TTarget, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OfType<TTarget>(IGraphElementModel model, bool disableTypeOptimization = false)
        {
            if (disableTypeOptimization || !typeof(TTarget).IsAssignableFrom(typeof(TElement)))
            {
                var labels = model.GetValidFilterLabels(typeof(TTarget));

                if (labels.Length > 0)
                    return AddStep<TTarget>(new HasLabelStep(labels));
            }

            return Cast<TTarget>();
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery
        {
            var optionalQuery = optionalTraversal(Anonymize());

            return this
                .AddStep(new OptionalStep(optionalQuery))
                .MergeStepLabelMappings(optionalQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] orTraversals)
        {
            return AddStep(new OrStep(orTraversals
                .Select(orTraversal => orTraversal(Anonymize()))
                .ToArray()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OrderBy(Expression<Func<TElement, object>> projection, Order order) => AddStep(OrderStep.Instance).By(projection, order);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OrderBy(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> traversal, Order order) => AddStep(OrderStep.Instance).By(traversal, order);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> OrderBy(string lambda) => AddStep(OrderStep.Instance).By(lambda);

        private GremlinQuery<TTarget, Unit, Unit, Unit, Unit, Unit> OtherV<TTarget>() => AddStep<TTarget, Unit, Unit, Unit, Unit, Unit>(OtherVStep.Instance);

        private GremlinQuery<TTarget, Unit, Unit, Unit, Unit, Unit> OutV<TTarget>() => AddStep<TTarget, Unit, Unit, Unit, Unit, Unit>(OutVStep.Instance);

        private GremlinQuery<TTuple, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Project<TTuple>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery>[] projections)
        {
            var ret = this
                .AddStep<TTuple>(new ProjectStep(Enumerable.Range(1, projections.Length).Select(i => $"Item{i}").ToArray()));

            foreach (var projection in projections)
            {
                ret = ret.AddStep(new ProjectStep.ByTraversalStep(projection(Anonymize())));
            }

            return ret;
        }

        private GremlinQuery<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit> Properties<TNewElement, TNewPropertyValue, TNewMeta>(params LambdaExpression[] projections)
        {
            return Properties<TNewElement, TNewPropertyValue, TNewMeta>(projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                    {
                        return memberExpression.Member.Name;
                    }

                    throw new ExpressionNotSupportedException(projection);
                }));
        }

        private GremlinQuery<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit> Properties<TNewElement, TNewPropertyValue, TNewMeta>(IEnumerable<string> keys) => AddStep<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit>(new PropertiesStep(keys.ToArray()));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, [AllowNull] object value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression && Model.GetIdentifier(memberExpression) is string identifier)
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
                .MergeStepLabelMappings(repeatQuery)
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
                .MergeStepLabelMappings(repeatQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, Unit, Unit, Unit, Unit, Unit> Select<TSelectedElement>(StepLabel stepLabel) => AddStep<TSelectedElement, Unit, Unit, Unit, Unit, Unit>(new SelectStep(stepLabel));

        private GremlinQuery<TSelectedElement, Unit, Unit, Unit, Unit, Unit> Select<TSelectedElement>(params StepLabel[] stepLabels)
        {
            return this
                .AddStep<TSelectedElement, Unit, Unit, Unit, Unit, Unit>(new SelectStep(stepLabels))
                .AddStepLabelBindings(stepLabels);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> sideEffectTraversal) => AddStep(new SideEffectStep(sideEffectTraversal(Anonymize())));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Skip(long count) => AddStep(new SkipStep(count));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SumGlobal() => AddStep(SumStep.Global);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SumLocal() => AddStep(SumStep.Local);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Tail(long count) => AddStep(new TailStep(count, Scope.Global));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> TailLocal(long count) => AddStep(new TailStep(count, Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Times(int count) => AddStep(new TimesStep(count));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit>(new ToLabelStep(stepLabel));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> toVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize())));

        private TTargetQuery Unfold<TTargetQuery>() => AddStep(UnfoldStep.Instance).ChangeQueryType<TTargetQuery>();

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery
        {
            var unionQueries = unionTraversals
                .Select(unionTraversal => (IGremlinQuery)unionTraversal(Anonymize()))
                .ToArray();

            return this
                .AddStep(new UnionStep(unionQueries))
                .MergeStepLabelMappings(unionQueries)
                .ChangeQueryType<TTargetQuery>();
        }

        private IValueGremlinQuery<TNewPropertyValue> Value<TNewPropertyValue>() => AddStep<TNewPropertyValue, Unit, Unit, Unit, Unit, Unit>(ValueStep.Instance);

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> ValueMap<TNewElement>(string[] keys) => AddStep<TNewElement, Unit, Unit, Unit, Unit, Unit>(new ValueMapStep(keys));

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var stringKeys = GetKeys(projections)
                .OfType<string>()
                .ToArray();

            if (stringKeys.Length != projections.Length())
                throw new ExpressionNotSupportedException();

            return AddStep<TNewElement, Unit, Unit, Unit, Unit, Unit>(new ValueMapStep(stringKeys));
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
                    return AddStep<TValue, Unit, Unit, Unit, Unit, Unit>(stepsArray[0]);
                default:
                    return AddStep<TValue, Unit, Unit, Unit, Unit, Unit>(new UnionStep(stepsArray.Select(step => Anonymize().AddStep(step))));
            }
        }

        private GremlinQuery<TValue, Unit, Unit, Unit, Unit, Unit> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(GetKeys(projections));

        private GremlinQuery<VertexProperty<TNewPropertyValue>, Unit, Unit, TNewPropertyValue, Unit, Unit> VertexProperties<TNewPropertyValue>(LambdaExpression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, Unit>(projections);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> VertexProperty(LambdaExpression projection, [AllowNull] object value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
            {
                var identifier = Model.GetIdentifier(memberExpression);

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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(string lambda) => AddStep(new FilterStep(new Lambda(lambda)));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> filterTraversal) => AddStep(new WhereTraversalStep(filterTraversal(Anonymize())));

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
                switch (terminal.Key)
                {
                    case MemberExpression leftMemberExpression:
                    {
                        if (leftMemberExpression.Expression == terminal.Parameter)
                        {
                            // x => x.Value == P.xy(...)
                            if (leftMemberExpression.IsPropertyValue())
                                return AddStep(new HasValueStep(terminal.Predicate));

                            if (leftMemberExpression.IsPropertyKey())
                                return Where(__ => __.Key().Where(terminal.Predicate));

                            if (leftMemberExpression.IsVertexPropertyLabel())
                                return Where(__ => __.Label().Where(terminal.Predicate));
                        }
                        else if (leftMemberExpression.Expression is MemberExpression leftLeftMemberExpression) 
                        {
                            // x => x.Name.Value == P.xy(...)
                            if (leftMemberExpression.IsPropertyValue())
                                leftMemberExpression = leftLeftMemberExpression;    //TODO: What else ?
                        }
                        else
                            break;

                        // x => x.Name == P.xy(...)
                        return Where(leftMemberExpression, terminal.Predicate);
                    }
                    case ParameterExpression leftParameterExpression when terminal.Parameter == leftParameterExpression:
                    {
                        // x => x == P.xy(...)
                        return Where(terminal.Predicate);
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        if (typeof(IDictionary<string, object>).IsAssignableFrom(methodCallExpression.Object.Type) && methodCallExpression.Method.Name == "get_Item")
                        {
                            return AddStep(new HasStep(methodCallExpression.Arguments[0].GetValue(), terminal.Predicate));
                        }

                        break;
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(MemberExpression expression, P predicate)
        {
            if (predicate is P.SingleArgumentP singleArgumentP && singleArgumentP.Argument is StepLabel)
                return Has(expression, Anonymize().Where(predicate));

            return Has(expression, predicate);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(P predicate)
        {
            return predicate is P.SingleArgumentP singleArgumentP && singleArgumentP.Argument is StepLabel
                ? AddStep(new WherePredicateStep(predicate))
                : AddStep(new IsStep(predicate));
        }
    }
}
