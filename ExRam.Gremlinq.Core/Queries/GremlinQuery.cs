// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public abstract class GremlinQuery
    {
        private static readonly ConcurrentDictionary<Type, Type> QueryTypes = new ConcurrentDictionary<Type, Type>();

        protected GremlinQuery(IGraphModel model, IGremlinQueryExecutor queryExecutor, IImmutableList<Step> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings, ILogger logger)
        {
            Model = model;
            Steps = steps;
            Logger = logger;
            QueryExecutor = queryExecutor;
            StepLabelMappings = stepLabelBindings;
        }

        protected TTargetQuery ChangeQueryType<TTargetQuery>()
        {
            var type = QueryTypes.GetOrAdd(
                typeof(TTargetQuery),
                closureType =>
                {
                    var metaType = typeof(Unit);
                    var elementType = typeof(Unit);
                    var inVertexType = typeof(Unit);
                    var outVertexType = typeof(Unit);
                    var foldedQueryType = typeof(Unit);
                    var propertyValueType = typeof(Unit);

                    if (closureType != typeof(IGremlinQuery))
                    {
                        if (!closureType.IsGenericType)
                            throw new NotSupportedException();

                        var genericTypeDef = closureType.GetGenericTypeDefinition();

                        if (genericTypeDef != typeof(IArrayGremlinQuery<,>) && genericTypeDef != typeof(IValueGremlinQuery<>) && genericTypeDef != typeof(IGremlinQuery<>) && genericTypeDef != typeof(IVertexGremlinQuery<>) && genericTypeDef != typeof(IEdgeGremlinQuery<>) && genericTypeDef != typeof(IEdgeGremlinQuery<,>) && genericTypeDef != typeof(IEdgeGremlinQuery<,,>))
                            throw new NotSupportedException();

                        elementType = closureType.GetGenericArguments()[0];

                        if (genericTypeDef == typeof(IEdgeGremlinQuery<,>) || genericTypeDef == typeof(IEdgeGremlinQuery<,,>))
                            outVertexType = closureType.GetGenericArguments()[1];

                        if (genericTypeDef == typeof(IEdgeGremlinQuery<,,>))
                            inVertexType = closureType.GetGenericArguments()[2];

                        if (genericTypeDef == typeof(IVertexPropertyGremlinQuery<,>))
                            propertyValueType = closureType.GetGenericArguments()[1];

                        if (genericTypeDef == typeof(IVertexPropertyGremlinQuery<,,>))
                            metaType = closureType.GetGenericArguments()[2];

                        if (genericTypeDef == typeof(IArrayGremlinQuery<,>))
                            foldedQueryType = closureType.GetGenericArguments()[1];
                    }

                    return typeof(GremlinQuery<,,,,,>).MakeGenericType(elementType, outVertexType, inVertexType, propertyValueType, metaType, foldedQueryType);
                });

            return (TTargetQuery)Activator.CreateInstance(type, Model, QueryExecutor, Steps, StepLabelMappings, Logger);
        }

        public static IGremlinQuery<Unit> Anonymous(IGraphModel model = null, ILogger logger = null)
        {
            return Create(model ?? GraphModel.Empty, GremlinQueryExecutor.Invalid, null, logger);
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
                .AddElementProperties(newEdge);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddElementProperties(object element)
        {
            var ret = this;
            var elementType = element.GetType();

            foreach (var (propertyInfo, value) in element.Serialize())
            {
                ret = ret.AddStep(new PropertyStep(propertyInfo.PropertyType, Model.GetIdentifier(elementType, propertyInfo.Name), value));
            }

            return ret;
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep(Step step) => AddStep<TElement>(step);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step) => AddStep<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(step);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Step step) => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Model, QueryExecutor, Steps.Insert(Steps.Count, step), StepLabelMappings, Logger);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStepLabelBinding(StepLabel stepLabel, Expression<Func<TElement, object>> memberExpression)
        {
            var body = memberExpression.Body.StripConvert();

            if (!(body is MemberExpression memberExpressionBody))
                throw new ExpressionNotSupportedException(memberExpression);

            return AddStepLabelBinding(stepLabel, memberExpressionBody.Member.Name);
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
                .AddElementProperties(vertex);
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

        private GremlinQuery<Unit, Unit, Unit, Unit, Unit, Unit> Drop() => AddStep<Unit, Unit, Unit, Unit, Unit, Unit>(DropStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(string lambda) => AddStep(new FilterStep(new Lambda(lambda)));

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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(Expression expression, P predicate)
        {
            return AddStep(new HasStep(Model.GetIdentifier(expression), predicate));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(Expression expression, IGremlinQuery traversal) => AddStep(new HasStep(Model.GetIdentifier(expression), traversal));

        private GremlinQuery<object, Unit, Unit, Unit, Unit, Unit> Id() => AddStep<object, Unit, Unit, Unit, Unit, Unit>(IdStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Identity() => AddStep(IdentityStep.Instance);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().ToArray()));

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit, Unit> InV<TNewElement>() => AddStep<TNewElement, Unit, Unit, Unit, Unit, Unit>(InVStep.Instance);

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

        private GremlinQuery<VertexProperty<TNewPropertyValue>, Unit, Unit, TNewPropertyValue, Unit, Unit> VertexProperties<TNewPropertyValue>(params LambdaExpression[] projections)
        {
            return Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, Unit>(projections);
        }

        private GremlinQuery<Property<TNewPropertyValue>, Unit, Unit, TNewPropertyValue, Unit, Unit> PlainProperties<TNewPropertyValue>(params LambdaExpression[] projections)
        {
            return Properties<Property<TNewPropertyValue>, TNewPropertyValue, Unit>(projections);
        }


        private GremlinQuery<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit> Properties<TNewElement, TNewPropertyValue, TNewMeta>(params LambdaExpression[] projections)
        {
            return AddStep<TNewElement, Unit, Unit, TNewPropertyValue, TNewMeta, Unit>(new PropertiesStep(projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                    {
                        return memberExpression.Member;
                    }

                    throw new ExpressionNotSupportedException(projection);
                })
                .ToArray()));
        }

        private GremlinQuery<Property<TValue>, Unit, Unit, Unit, Unit, Unit> Properties<TValue>(params string[] keys) => AddStep<Property<TValue>, Unit, Unit, Unit, Unit, Unit>(new MetaPropertiesStep(keys));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, [AllowNull] object value)
        {
            if (value == null)
            {
                return SideEffect(_ => _
                    .Properties<Unit, Unit, Unit>(projection)
                    .Drop());
            }

            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new PropertyStep(memberExpression.Type, Model.GetIdentifier(memberExpression), value));

            throw new ExpressionNotSupportedException(projection);
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

        private IValueGremlinQuery<TPropertyValue> Value() => AddStep<TPropertyValue, Unit, Unit, Unit, Unit, Unit>(ValueStep.Instance);

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

        private GremlinQuery<TValue, Unit, Unit, Unit, Unit, Unit> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections)
        {
            return ValuesForKeys<TValue>(GetKeys(projections));
        }

        private GremlinQuery<TValue, Unit, Unit, Unit, Unit, Unit> ValuesForProjections<TValue>(IEnumerable<MemberExpression> projections)
        {
            return ValuesForKeys<TValue>(GetKeys(projections));
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

        private GremlinQuery<TValue, Unit, Unit, Unit, Unit, Unit> ValuesForKeys<TValue>(object[] keys)
        {
            var stepsArray = this
                .GetStepsForKeys(keys)
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

        private IEnumerable<Step> GetStepsForKeys(object[] keys)
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQuery> filterTraversal) => AddStep(new WhereTraversalStep(filterTraversal(Anonymize())));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where<TSource>(Expression<Func<TSource, bool>> predicate)
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
                            if (terminal.Predicate is P.SingleArgumentP singleArgumentP && singleArgumentP.Argument is StepLabel)
                                return Where(_ => _.ValuesForProjections<object>(new[]{ leftMemberExpression }).AddStep(new WherePredicateStep(terminal.Predicate)));

                            if (leftMemberExpression.IsPropertyValue())
                                return AddStep(new HasValueStep(terminal.Predicate));
                        }
                        else if (leftMemberExpression.Expression is MemberExpression leftLeftMemberExpression)
                        {
                            if (leftMemberExpression.IsPropertyValue())
                                return Has(leftLeftMemberExpression, terminal.Predicate);

                            if (!leftLeftMemberExpression.IsVertexPropertyProperties())
                                break;
                        }
                        else
                            break;

                        return Has(leftMemberExpression, terminal.Predicate);
                    }
                    case ParameterExpression leftParameterExpression when terminal.Parameter == leftParameterExpression:
                    {
                        return AddStep(
                            terminal.Predicate is P.SingleArgumentP singleArgumentP && singleArgumentP.Argument is StepLabel
                                ? new WherePredicateStep(terminal.Predicate)
                                : (Step)new IsStep(terminal.Predicate));
                    }
                    case MethodCallExpression methodCallExpression:
                    {
                        if (typeof(IDictionary<string, object>).IsAssignableFrom(methodCallExpression.Object.Type) && methodCallExpression.Method.Name == "get_Item")
                        {
                            return AddStep(new HasStep(methodCallExpression.Arguments[0].GetValue(), terminal.Predicate));
                            //if (typeof(Property).IsAssignableFrom(methodCallExpression.Expression.Type) && leftLeftMemberExpression.Member.Name == nameof(VertexProperty<object>.Properties))
                            //    return Has(leftMemberExpression, predicateArgument);
                        }

                        break;
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }
    }
}
