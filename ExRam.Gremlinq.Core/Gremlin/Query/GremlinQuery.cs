// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using Microsoft.Extensions.Logging;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    public abstract class GremlinQuery
    {
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
            var metaType = typeof(Unit);
            var elementType = typeof(Unit);
            var inVertexType = typeof(Unit);
            var outVertexType = typeof(Unit);
            var foldedQueryType = typeof(Unit);

            if (typeof(TTargetQuery) != typeof(IGremlinQuery))
            {
                if (!typeof(TTargetQuery).IsGenericType)
                    throw new NotSupportedException();

                var genericTypeDef = typeof(TTargetQuery).GetGenericTypeDefinition();

                if (genericTypeDef != typeof(IArrayGremlinQuery<,>) && genericTypeDef != typeof(IGremlinQuery<>) && genericTypeDef != typeof(IVertexGremlinQuery<>) && genericTypeDef != typeof(IEdgeGremlinQuery<>) && genericTypeDef != typeof(IEdgeGremlinQuery<,>) && genericTypeDef != typeof(IEdgeGremlinQuery<,,>))
                    throw new NotSupportedException();

                elementType = typeof(TTargetQuery).GetGenericArguments()[0];

                if (genericTypeDef == typeof(IEdgeGremlinQuery<,>) || genericTypeDef == typeof(IEdgeGremlinQuery<,,>))
                    outVertexType = typeof(TTargetQuery).GetGenericArguments()[1];

                if (genericTypeDef == typeof(IEdgeGremlinQuery<,,>))
                    inVertexType = typeof(TTargetQuery).GetGenericArguments()[2];

                if (genericTypeDef == typeof(IVertexPropertyGremlinQuery<,>))
                    metaType = typeof(TTargetQuery).GetGenericArguments()[1];

                if (genericTypeDef == typeof(IArrayGremlinQuery<,>))
                    foldedQueryType = typeof(TTargetQuery).GetGenericArguments()[1];
            }

            var type = typeof(GremlinQuery<,,,,>).MakeGenericType(elementType, outVertexType, inVertexType, metaType, foldedQueryType);
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
            return new GremlinQuery<TElement, Unit, Unit, Unit, Unit>(
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

    internal sealed partial class GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> : GremlinQuery
    {
        public GremlinQuery(IGraphModel model, IGremlinQueryExecutor queryExecutor, IImmutableList<Step> steps, IImmutableDictionary<StepLabel, string> stepLabelBindings, ILogger logger) : base(model, queryExecutor, steps, stepLabelBindings, logger)
        {
        }

        private GremlinQuery<TEdge, TElement, Unit, Unit, Unit> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, Unit, Unit, Unit>(new AddEStep(Model, newEdge))
                .AddElementProperties(newEdge);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> AddElementProperties(object element)
        {
            var ret = this;
            var elementType = element.GetType();

            foreach (var (propertyInfo, value) in element.Serialize())
            {
                ret = ret.AddStep(new PropertyStep(propertyInfo.PropertyType, Model.GetIdentifier(elementType, propertyInfo.Name), value));
            }

            return ret;
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> AddStep(Step step) => AddStep<TElement>(step);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step) => AddStep<TNewElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>(step);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery>(Step step) => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery>(Model, QueryExecutor, Steps.Insert(Steps.Count, step), StepLabelMappings, Logger);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> AddStepLabelBinding(StepLabel stepLabel, Expression<Func<TElement, object>> memberExpression)
        {
            var body = memberExpression.Body.StripConvert();

            if (!(body is MemberExpression memberExpressionBody))
                throw new ExpressionNotSupportedException(memberExpression);

            return AddStepLabelBinding(stepLabel, memberExpressionBody.Member.Name);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> AddStepLabelBinding(StepLabel stepLabel, string name)
        {
            if (StepLabelMappings.TryGetValue(stepLabel, out var existingName) && existingName != name)
                throw new InvalidOperationException($"A StepLabel was already bound to {name} by a previous Select operation. Try changing the position of the StepLabel in the Select operation or introduce a new StepLabel.");

            return new GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>(Model, QueryExecutor, Steps, StepLabelMappings.Add(stepLabel, name), Logger);
        }

        private GremlinQuery<TVertex, Unit, Unit, Unit, Unit> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStep<TVertex, Unit, Unit, Unit, Unit>(new AddVStep(Model, vertex))
                .AddElementProperties(vertex);
        }

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                AddStep(new AggregateStep(stepLabel)),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery>[] andTraversals)
        {
            return AddStep(new AndStep(andTraversals
                .Select(andTraversal => andTraversal(Anonymize()))
                .ToArray()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Anonymize() => Anonymize<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery> Anonymize<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery>() => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery>(Model, GremlinQueryExecutor.Invalid, ImmutableList<Step>.Empty, ImmutableDictionary<StepLabel, string>.Empty, Logger);

        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQuery
        {
            var stepLabel = new TStepLabel();

            return continuation(
                As(stepLabel),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> As(StepLabel stepLabel) => AddStep(new AsStep(stepLabel));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Barrier() => AddStep(BarrierStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> By(Expression<Func<TElement, object>> projection, Order order)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new ByMemberStep(Model.GetIdentifier(memberExpression), order));

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> traversal, Order order)
        {
            return this
                .AddStep(new ByTraversalStep(traversal(Anonymize()), order));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> By(string lambda)
        {
            return this
                .AddStep(new ByLambdaStep(new Lambda(lambda)));
        }

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Cast<TNewElement>() => Cast<TNewElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery> Cast<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery>() => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewMeta, TNewFoldedQuery>(Model, QueryExecutor, Steps, StepLabelMappings, Logger);

        private TTargetQuery Choose<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> falseChoice) where TTargetQuery : IGremlinQuery
        { 
            var anonymous = Anonymize();
            var trueQuery = trueChoice(anonymous);
            var falseQuery = falseChoice(anonymous);
            
            return this
                .AddStep(new ChooseStep(traversalPredicate(anonymous), trueQuery, Option<IGremlinQuery>.Some(falseQuery)))
                .MergeStepLabelMappings(trueQuery, falseQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Choose<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> trueChoice) where TTargetQuery : IGremlinQuery
        {
            var anonymous = Anonymize();
            var trueQuery = trueChoice(anonymous);

            return this
                .AddStep(new ChooseStep(traversalPredicate(anonymous), trueQuery))
                .MergeStepLabelMappings(trueQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery>[] traversals)
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Dedup() => AddStep(DedupStep.Instance);

        private GremlinQuery<Unit, Unit, Unit, Unit, Unit> Drop() => AddStep<Unit, Unit, Unit, Unit, Unit>(DropStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Filter(string lambda) => AddStep(new FilterStep(new Lambda(lambda)));

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new FlatMapStep(mappedTraversal))
                .MergeStepLabelMappings(mappedTraversal)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement[], Unit, Unit, Unit, TNewFoldedQuery> Fold<TNewFoldedQuery>() => AddStep<TElement[], Unit, Unit, Unit, TNewFoldedQuery>(FoldStep.Instance);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit> From<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> fromVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize())));

        private IAsyncEnumerator<TResult> GetEnumerator<TResult>()
        {
            return QueryExecutor
                .Execute<TResult>(this.Cast<TResult>())
                .GetEnumerator();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Has(Expression expression, P predicate) => AddStep(new HasStep(Model.GetIdentifier(expression), predicate));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Has(Expression expression, IGremlinQuery traversal) => AddStep(new HasStep(Model.GetIdentifier(expression), traversal));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> HasNot(Expression expression) => AddStep(new HasNotStep(Model.GetIdentifier(expression)));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> HasWithin(Expression expression, Expression enumerableExpression)
        {
            if (enumerableExpression.GetValue() is IEnumerable enumerable)
            {
                return HasWithin(expression, enumerable);
            }

            throw new ExpressionNotSupportedException(enumerableExpression);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> HasWithin(Expression expression, IEnumerable enumerable)
        {
            var objectArray = enumerable as object[] ?? enumerable.Cast<object>().ToArray();

            return Has(expression,
                new P.Within(objectArray));
        }

        private GremlinQuery<object, Unit, Unit, Unit, Unit> Id() => AddStep<object, Unit, Unit, Unit, Unit>(IdStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Identity() => AddStep(IdentityStep.Instance);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().ToArray()));

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit> InV<TNewElement>() => AddStep<TNewElement, Unit, Unit, Unit, Unit>(InVStep.Instance);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Limit(long count)
        {
            return AddStep(count == 1
                ? LimitStep.Limit1
                : new LimitStep(count));
        }

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQuery
        {
            var localTraversalQuery = localTraversal(Anonymize());

            return this
                .AddStep(new LocalStep(localTraversalQuery))
                .MergeStepLabelMappings(localTraversalQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQuery
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new MapStep(mappedTraversal))
                .MergeStepLabelMappings(mappedTraversal)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Match(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery>[] matchTraversals) => AddStep(new MatchStep(matchTraversals.Select(traversal => traversal(Anonymize()))));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> MergeStepLabelMappings(params IGremlinQuery[] queries)
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> notTraversal) => AddStep(new NotStep(notTraversal(Anonymize())));

        private GremlinQuery<TTarget, TOutVertex, TInVertex, TMeta, TFoldedQuery> OfType<TTarget>(IGraphElementModel model, bool disableTypeOptimization = false)
        {
            if (disableTypeOptimization || !typeof(TTarget).IsAssignableFrom(typeof(TElement)))
            {
                var labels = model.GetValidFilterLabels(typeof(TTarget));

                if (labels.Length > 0)
                    return AddStep<TTarget>(new HasLabelStep(labels));
            }

            return Cast<TTarget>();
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQuery
        {
            var optionalQuery = optionalTraversal(Anonymize());

            return this
                .AddStep(new OptionalStep(optionalQuery))
                .MergeStepLabelMappings(optionalQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery>[] orTraversals)
        {
            return AddStep(new OrStep(orTraversals
                .Select(orTraversal => orTraversal(Anonymize()))
                .ToArray()));
        }


        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> OrderBy(Expression<Func<TElement, object>> projection, Order order)
        {
            return this
                .AddStep(OrderStep.Instance)
                .By(projection, order);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> OrderBy(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> traversal, Order order)
        {
            return this
                .AddStep(OrderStep.Instance)
                .By(traversal, order);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> OrderBy(string lambda)
        {
            return this
                .AddStep(OrderStep.Instance)
                .By(lambda);
        }

        private GremlinQuery<TNewElement, Unit, Unit, TNewMeta, Unit> Properties<TSource, TTarget, TNewElement, TNewMeta>(params Expression<Func<TSource, TTarget>>[] projections)
        {
            return AddStep<TNewElement, Unit, Unit, TNewMeta, Unit>(new PropertiesStep(projections
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

        private GremlinQuery<Property<TMetaValue>, Unit, Unit, Unit, Unit> Properties<TMetaValue>(params string[] keys) => AddStep<Property<TMetaValue>, Unit, Unit, Unit, Unit>(new MetaPropertiesStep(keys));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, [AllowNull] object value)
        {
            if (value == null)
            {
                return SideEffect(_ => _
                    .Properties<TSource, TValue, Unit, Unit>(projection)
                    .Drop());
            }

            if (projection.Body.StripConvert() is MemberExpression memberExpression)
                return AddStep(new PropertyStep(memberExpression.Type, Model.GetIdentifier(memberExpression), value));

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Range(long low, long high) => AddStep(new RangeStep(low, high));

        private TTargetQuery Repeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal)
            where TTargetQuery : IGremlinQuery
        {
            var repeatQuery = repeatTraversal(Anonymize());

            return this
                .AddStep(new RepeatStep(repeatQuery))
                .MergeStepLabelMappings(repeatQuery)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery RepeatUntil<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> untilTraversal)
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

        private GremlinQuery<TSelectedElement, TSelectedOutVertex, TSelectedInVertex, Unit, Unit> Select<TSelectedElement, TSelectedOutVertex, TSelectedInVertex>(StepLabel stepLabel) => AddStep<TSelectedElement, TSelectedOutVertex, TSelectedInVertex, Unit, Unit>(new SelectStep(stepLabel));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> sideEffectTraversal) => AddStep(new SideEffectStep(sideEffectTraversal(Anonymize())));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Skip(long count) => AddStep(new SkipStep(count));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> SumGlobal() => AddStep(SumStep.Global);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> SumLocal() => AddStep(SumStep.Local);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Tail(long count) => AddStep(new TailStep(count));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Times(int count) => AddStep(new TimesStep(count));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit>(new ToLabelStep(stepLabel));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit> To<TNewElement, TNewOutVertex, TNewInVertex>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> toVertexTraversal) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, Unit, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize())));

        private TTagetQuery Unfold<TTagetQuery>() => AddStep(UnfoldStep.Instance).ChangeQueryType<TTagetQuery>();

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQuery
        {
            var unionQueries = unionTraversals
                .Select(unionTraversal => (IGremlinQuery)unionTraversal(Anonymize()))
                .ToArray();

            return this
                .AddStep(new UnionStep(unionQueries))
                .MergeStepLabelMappings(unionQueries)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit> ValueMap<TNewElement>() => AddStep<TNewElement, Unit, Unit, Unit, Unit>(new ValueMapStep());

        private GremlinQuery<TNewElement, Unit, Unit, Unit, Unit> Values<TSource, TTarget, TNewElement>(IEnumerable<Expression<Func<TSource, TTarget>>> projections)
        {
            var keys = projections
                .Select(projection =>
                {
                    if (projection.Body.StripConvert() is MemberExpression memberExpression)
                        return Model.GetIdentifier(memberExpression);

                    throw new ExpressionNotSupportedException(projection);
                })
                .ToArray();

            return AddStep<TNewElement, Unit, Unit, Unit, Unit>(new ValuesStep(keys));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery>, IGremlinQuery> filterTraversal) => AddStep(new WhereTraversalStep(filterTraversal(Anonymize())));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Where<TSource>(Expression<Func<TSource, bool>> predicate)
        {
            var body = predicate.Body;

            try
            {
                switch (body)
                {
                    case UnaryExpression unaryExpression:
                    {
                        if (unaryExpression.NodeType == ExpressionType.Not)
                            return Not(_ => _.Where(Expression.Lambda<Func<TElement, bool>>(unaryExpression.Operand, predicate.Parameters)));

                        break;
                    }
                    case MemberExpression memberExpression:
                    {
                        if (memberExpression.Member is PropertyInfo property && property.PropertyType == typeof(bool))
                            return Where(predicate.Parameters[0], memberExpression, Expression.Constant(true), ExpressionType.Equal);

                        break;
                    }
                    case BinaryExpression binaryExpression:
                        return Where(predicate.Parameters[0], binaryExpression.Left.StripConvert(), binaryExpression.Right.StripConvert(), binaryExpression.NodeType);
                    case MethodCallExpression methodCallExpression:
                    {
                        var methodInfo = methodCallExpression.Method;

                        if (methodInfo.IsEnumerableAny())
                        {
                            if (methodCallExpression.Arguments[0] is MethodCallExpression previousExpression && previousExpression.Method.IsEnumerableIntersect())
                            {
                                if (previousExpression.Arguments[0] is MemberExpression sourceMember)
                                    return HasWithin(sourceMember, previousExpression.Arguments[1]);

                                if (previousExpression.Arguments[1] is MemberExpression argument && argument.Expression == predicate.Parameters[0])
                                    return HasWithin(argument, previousExpression.Arguments[0]);
                            }
                            else
                                return Where(predicate.Parameters[0], methodCallExpression.Arguments[0], Expression.Constant(null, methodCallExpression.Arguments[0].Type), ExpressionType.NotEqual);
                        }
                        else if (methodInfo.IsEnumerableContains())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression sourceMember && sourceMember.Expression == predicate.Parameters[0])
                                return Has(sourceMember, new P.Eq(methodCallExpression.Arguments[1].GetValue()));

                            if (methodCallExpression.Arguments[1] is MemberExpression argument && argument.Expression == predicate.Parameters[0])
                                return HasWithin(argument, methodCallExpression.Arguments[0]);
                        }
                        else if (methodInfo.IsStringStartsWith())
                        {
                            if (methodCallExpression.Arguments[0] is MemberExpression argumentExpression && argumentExpression.Expression == predicate.Parameters[0])
                            {
                                if (methodCallExpression.Object.GetValue() is string stringValue)
                                {
                                    return HasWithin(argumentExpression,
                                        Enumerable
                                            .Range(0, stringValue.Length + 1)
                                            .Select(i => stringValue.Substring(0, i)));
                                }
                            }
                            else if (methodCallExpression.Object is MemberExpression memberExpression && memberExpression.Expression == predicate.Parameters[0])
                            {
                                if (methodCallExpression.Arguments[0].GetValue() is string lowerBound)
                                {
                                    string upperBound;

                                    if (lowerBound.Length == 0)
                                        return Has(memberExpression, P.True);

                                    if (lowerBound[lowerBound.Length - 1] == char.MaxValue)
                                        upperBound = lowerBound + char.MinValue;
                                    else
                                    {
                                        var upperBoundChars = lowerBound.ToCharArray();

                                        upperBoundChars[upperBoundChars.Length - 1]++;
                                        upperBound = new string(upperBoundChars);
                                    }

                                    return Has(memberExpression, new P.Between(lowerBound, upperBound));
                                }
                            }
                        }

                        break;
                    }
                }
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(predicate, ex);
            }

            throw new ExpressionNotSupportedException(predicate);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Where(ParameterExpression parameter, Expression left, Expression right, ExpressionType nodeType)
        {
            if (nodeType == ExpressionType.OrElse || nodeType == ExpressionType.AndAlso)
            {
                var leftLambda = Expression.Lambda<Func<TElement, bool>>(left, parameter);
                var rightLambda = Expression.Lambda<Func<TElement, bool>>(right, parameter);

                return nodeType == ExpressionType.OrElse
                    ? Or(
                        _ => _.Where(leftLambda),
                        _ => _.Where(rightLambda))
                    : And(
                        _ => _.Where(leftLambda),
                        _ => _.Where(rightLambda));
            }

            return right.HasExpressionInMemberChain(parameter)
                ? Where(parameter, right, left.GetValue(), nodeType.Switch())
                : Where(parameter, left, right.GetValue(), nodeType);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Where(ParameterExpression parameter, Expression left, object rightConstant, ExpressionType nodeType)
        {
            if (rightConstant == null)
            {
                // ReSharper disable once SwitchStatementMissingSomeCases
                switch (nodeType)
                {
                    case ExpressionType.Equal:
                        return HasNot(left);
                    case ExpressionType.NotEqual:
                        return Has(left, P.True);
                }
            }
            else
            {
                var predicateArgument = nodeType.ToP(rightConstant);

                switch (left)
                {
                    case MemberExpression leftMemberExpression:
                    {
                        if (leftMemberExpression.Expression == parameter)
                        {
                            if (typeof(Property).IsAssignableFrom(leftMemberExpression.Expression.Type) && leftMemberExpression.Member.Name == nameof(Property<object>.Value))
                                return AddStep(new HasValueStep(predicateArgument));

                            return rightConstant is StepLabel
                                ? Has(leftMemberExpression, Anonymize().AddStep(new WherePredicateStep(predicateArgument)))
                                : Has(leftMemberExpression, predicateArgument);
                        }

                        if (leftMemberExpression.Expression is MemberExpression leftLeftMemberExpression)
                        {
                            if (typeof(Property).IsAssignableFrom(leftLeftMemberExpression.Expression.Type) && leftLeftMemberExpression.Member.Name == nameof(VertexProperty<object>.Properties))
                                return Has(leftMemberExpression, predicateArgument);
                        }

                        break;
                    }
                    case ParameterExpression leftParameterExpression when parameter == leftParameterExpression:
                    {
                        return AddStep(
                            rightConstant is StepLabel
                                ? new WherePredicateStep(predicateArgument)
                                : (Step)new IsStep(predicateArgument));
                    }
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TMeta, TFoldedQuery> Where<TProjection>(Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Has(predicate.Body, propertyTraversal(Anonymize<TProjection, Unit, Unit, Unit, Unit>()));
    }
}
