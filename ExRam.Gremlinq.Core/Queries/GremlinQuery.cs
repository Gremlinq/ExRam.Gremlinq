// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;
using LanguageExt;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQuery
    {
        internal static readonly IImmutableStack<Step> AnonymousNoneSteps = ImmutableStack<Step>.Empty.Push(NoneStep.Instance);

        public static IGremlinQuery<object> Anonymous(IGremlinQueryEnvironment environment)
        {
            return Create<object>(
                ImmutableStack<Step>.Empty,
                environment,
                false);
        }

        public static IGremlinQuery<TElement> Create<TElement>(IGremlinQueryEnvironment environment)
        {
            return Create<TElement>(
                ImmutableStack<Step>.Empty,
                environment,
                true);
        }

        public static IGremlinQuery<TElement> Create<TElement>(IImmutableStack<Step> steps, IGremlinQueryEnvironment environment, bool surfaceVisible)
        {
            return new GremlinQuery<TElement, object, object, object, object, object>(
                steps,
                environment,
                QuerySemantics.None,
                ImmutableDictionary<StepLabel, QuerySemantics>.Empty,
                surfaceVisible);
        }
    }

    internal sealed partial class GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> : GremlinQueryBase where TMeta : class
    {
        private sealed class OrderBuilder : IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> _query;

            public OrderBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> query)
            {
                _query = query;
            }

            GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.Build() => _query;

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.By(Expression<Func<TElement, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Incr);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Incr);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.By(ILambda lambda) => By(lambda);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Incr);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.By(ILambda lambda) => By(lambda);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.ByDescending(Expression<Func<TElement, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Decr);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.ByDescending(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Decr);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.ByDescending(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Decr);

            private OrderBuilder By(Expression<Func<TElement, object?>> projection, Order order)
            {
                if (projection.Body.StripConvert() is MemberExpression memberExpression)
                    return new OrderBuilder(_query.AddStep(new OrderStep.ByMemberStep(_query.Environment.Model.PropertiesModel.GetIdentifier(memberExpression), order)));

                throw new ExpressionNotSupportedException(projection);
            }

            private OrderBuilder By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal, Order order) => new OrderBuilder(_query.AddStep(new OrderStep.ByTraversalStep(traversal(_query.Anonymize()), order)));

            private OrderBuilder By(ILambda lambda) => new OrderBuilder(_query.AddStep(new OrderStep.ByLambdaStep(lambda)));
        }

        private sealed class ChooseBuilder<TTargetQuery, TPickElement> :
            IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>,
            IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TPickElement>,
            IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TPickElement, TTargetQuery>
            where TTargetQuery : IGremlinQueryBase
        {
            private readonly IGremlinQueryBase _targetQuery;
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> _sourceQuery;

            public ChooseBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> sourceQuery, IGremlinQueryBase targetQuery)
            {
                _sourceQuery = sourceQuery;
                _targetQuery = targetQuery;
            }

            public IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TNewPickElement> On<TNewPickElement>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewPickElement>> chooseTraversal)
            {
                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep(new ChooseOptionTraversalStep(chooseTraversal(_sourceQuery))));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep(new OptionTraversalStep(element, continuation(_sourceQuery))));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep(new OptionTraversalStep(default, continuation(_sourceQuery))));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery
            {
                get
                {
                    if (_targetQuery == null)
                        throw new InvalidOperationException();

                    return _targetQuery
                        .AsAdmin()
                        .ChangeQueryType<TTargetQuery>();
                }
            }
        }

        private sealed class GroupBuilder<TKey, TValue> :
            IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>,
            IGroupBuilderWithKeyAndValue<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TKey, TValue>
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> _sourceQuery;

            public GroupBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> sourceQuery, IGremlinQueryBase<TKey>? keyQuery = default, IGremlinQueryBase<TValue>? valueQuery = default)
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                KeyQuery = keyQuery;
                ValueQuery = valueQuery;
#pragma warning restore CS8601 // Possible null reference assignment.

                _sourceQuery = sourceQuery;
            }

            IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TNewKey> IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>.ByKey<TNewKey>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewKey>> keySelector)
            {
                return new GroupBuilder<TNewKey, object>(
                    _sourceQuery,
                    keySelector(_sourceQuery));
            }

            IGroupBuilderWithKeyAndValue<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TKey, TNewValue> IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TKey>.ByValue<TNewValue>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewValue>> valueSelector)
            {
                return new GroupBuilder<TKey, TNewValue>(
                    _sourceQuery,
                    KeyQuery,
                    valueSelector(_sourceQuery));
            }

            public IGremlinQueryBase<TKey> KeyQuery { get; }

            public IGremlinQueryBase<TValue> ValueQuery { get; }
        }

        private sealed partial class ProjectBuilderImpl<TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>,
            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>
        {
            private readonly GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> _sourceQuery;

            public ProjectBuilderImpl(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> sourceQuery) : this(sourceQuery, ImmutableDictionary<string, IGremlinQueryBase>.Empty)
            {
            }

            private ProjectBuilderImpl(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> sourceQuery, IImmutableDictionary<string, IGremlinQueryBase> projections)
            {
                _sourceQuery = sourceQuery;
                Projections = projections;
            }

            private ProjectBuilderImpl<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>($"Item{Projections.Count + 1}", projection);
            }

            private ProjectBuilderImpl<TProjectElement, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object> By(string name, Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return By<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(name, projection);
            }

            private ProjectBuilderImpl<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(string name, Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return new ProjectBuilderImpl<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(
                    _sourceQuery,
                    Projections.SetItem(name, projection(_sourceQuery)));
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement> IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>.ToTuple()
            {
                return this;
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement> IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>.ToDynamic()
            {
                return this;
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TNewItem16> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15>.By<TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem16>> projection)
            {
                return By<TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TNewItem16>(projection);
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement, TNewItem1> IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>.By<TNewItem1>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewItem1>> projection)
            {
                return By<TNewItem1, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>.By(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return By($"Item{Projections.Count + 1}", projection);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TProjectElement>.By(string name, Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return By(name, projection);
            }

            public IImmutableDictionary<string, IGremlinQueryBase> Projections { get; }
        }

        public GremlinQuery(
            IImmutableStack<Step> steps,
            IGremlinQueryEnvironment environment,
            QuerySemantics semantics,
            IImmutableDictionary<StepLabel, QuerySemantics> stepLabelSemantics,
            bool surfaceVisible) : base(steps, environment, semantics, stepLabelSemantics, surfaceVisible)
        {

        }

        private GremlinQuery<TEdge, TElement, object, object, object, object> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, object, object, object, object>(new AddEStep(Environment.Model, newEdge), QuerySemantics.Edge)
                .AddOrUpdate(newEdge, true, false);
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
                    .Properties<object, object, object>(
                        props
                            .Select(p => p.identifier)
                            .OfType<string>(),
                        Semantics)
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

        private PropertyStep GetPropertyStep(object key, object value, Cardinality? cardinality)
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep(Step step, QuerySemantics? querySemantics = null, IImmutableDictionary<StepLabel, QuerySemantics>? stepLabelSemantics = null) => AddStep<TElement>(step, querySemantics, stepLabelSemantics);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step, QuerySemantics? querySemantics = null, IImmutableDictionary<StepLabel, QuerySemantics>? stepLabelSemantics = null) => AddStep<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(step, querySemantics, stepLabelSemantics);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Step step, QuerySemantics? querySemantics = null, IImmutableDictionary<StepLabel, QuerySemantics>? stepLabelSemantics = null) where TNewMeta : class => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Steps.Push(step), Environment, querySemantics ?? Semantics, stepLabelSemantics ?? StepLabelSemantics, SurfaceVisible);

        private GremlinQuery<TNewElement, object, object, object, object, object> AddStepWithObjectTypes<TNewElement>(Step step, QuerySemantics? querySemantics = null) => AddStep<TNewElement, object, object, object, object, object>(step, querySemantics);

        private GremlinQuery<TVertex, object, object, object, object, object> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStepWithObjectTypes<TVertex>(new AddVStep(Environment.Model, vertex), QuerySemantics.Vertex)
                .AddOrUpdate(vertex, true, true);
        }

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(TStepLabel stepLabel, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(
                AddStep(new AggregateStep(stepLabel), QuerySemantics.None, StepLabelSemantics.Add(stepLabel, Semantics)),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase>[] andTraversalTransformations)
        {
            if (andTraversalTransformations.Length == 0)
                return AddStep(AndStep.Infix);

            var anonymous = Anonymize();
            var subQueries = default(List<IGremlinQueryBase>);

            foreach (var transformation in andTraversalTransformations)
            {
                var transformed = transformation(anonymous);

                if (transformed.IsNone())
                    return None();

                if (transformed != anonymous)
                    (subQueries ??= new List<IGremlinQueryBase>()).Add(transformed);
            }

            return (subQueries?.Count).GetValueOrDefault() == 0
                ? this
                : AddStep(new AndStep(subQueries!.ToArray()));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Anonymize(bool surfaceVisible = false) => Anonymize<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(surfaceVisible);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Anonymize<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(bool surfaceVisible = false) where TNewMeta : class => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(ImmutableStack<Step>.Empty, Environment, Semantics, StepLabelSemantics, surfaceVisible);

        private TTargetQuery As<TStepLabel, TTargetQuery>(TStepLabel stepLabel, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel
            where TTargetQuery : IGremlinQueryBase
        {
            if (!Steps.IsEmpty && Steps.Peek() is AsStep asStep && asStep.StepLabels.FirstOrDefault() is TStepLabel existingStepLabel1)
                return continuation(this, existingStepLabel1);

            return continuation(
                As(stepLabel),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> As(params StepLabel[] stepLabels)
        {
            return AddStep(
                new AsStep(stepLabels),
                default,
                StepLabelSemantics.AddRange(stepLabels.Select(stepLabel => new KeyValuePair<StepLabel, QuerySemantics>(stepLabel, Semantics))));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Barrier() => AddStep(BarrierStep.Instance);

        private GremlinQuery<TTarget, object, object, object, object, object> BothV<TTarget>() => AddStepWithObjectTypes<TTarget>(BothVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Cast<TNewElement>() => Cast<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Cast<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>() where TNewMeta : class => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Steps, Environment, Semantics, StepLabelSemantics, SurfaceVisible);

        private TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Option<Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>> maybeFalseChoice = default) where TTargetQuery : IGremlinQueryBase
        {
            if (predicate.TryToGremlinExpression() is { } gremlinExpression)
            {
                if (gremlinExpression.Key is ParameterExpression)
                {
                    var anonymous = Anonymize();
                    var trueQuery = trueChoice(anonymous);
                    var maybeFalseQuery = maybeFalseChoice.Map(falseChoice => (IGremlinQueryBase)falseChoice(anonymous));

                    return this
                        .AddStep(new ChoosePredicateStep(gremlinExpression.Predicate, trueQuery, maybeFalseQuery), QuerySemantics.None)
                        .ChangeQueryType<TTargetQuery>();
                }
            }

            throw new ExpressionNotSupportedException(predicate);
        }

        private TTargetQuery Choose<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Option<Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>> maybeFalseChoice = default) where TTargetQuery : IGremlinQueryBase
        {
            var anonymous = Anonymize();
            var trueQuery = trueChoice(anonymous);
            var maybeFalseQuery = maybeFalseChoice.Map(falseChoice => (IGremlinQueryBase)falseChoice(anonymous));

            return maybeFalseQuery
                .BiFold(
                    AddStep(new ChooseTraversalStep(traversalPredicate(anonymous), trueQuery, maybeFalseQuery), QuerySemantics.None),
                    (query, falseQuery) => query,
                    (query, _) => query)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(new ChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, object>(Anonymize(), this)).TargetQuery;
        }

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>[] traversals)
            where TTargetQuery : IGremlinQueryBase
        {
            if (traversals.Length == 0)
                throw new ArgumentException("Coalesce must have at least one subquery.");

            var coalesceQueries = traversals
                .Select(traversal => (IGremlinQueryBase)traversal(Anonymize()))
                .ToArray();

            return (coalesceQueries.All(x => x.IsIdentity())
                ? this
                : AddStep(new CoalesceStep(coalesceQueries), QuerySemantics.None)).ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Coin(double probability) => AddStep(new CoinStep(probability));

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> ConfigureSteps<TNewElement>(Func<IImmutableStack<Step>, IImmutableStack<Step>> configurator) => new GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(configurator(Steps), Environment, Semantics, StepLabelSemantics, SurfaceVisible);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Dedup() => AddStep(DedupStep.Instance);

        private GremlinQuery<object, object, object, object, object, object> Drop() => AddStepWithObjectTypes<object>(DropStep.Instance, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> DropProperties(string key)
        {
            return SideEffect(_ => _
                .Properties<object, object, object>(new[] { key }, QuerySemantics.Property)
                .Drop());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase
        {
            var mappedTraversal = mapping(Anonymize());

            return this
                .AddStep(new FlatMapStep(mappedTraversal), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement[], object, object, object, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() => AddStep<TElement[], object, object, object, object, TNewFoldedQuery>(FoldStep.Instance, QuerySemantics.None);

        private GremlinQuery<IDictionary<TKey, TValue>, object, object, object, object, object> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IGremlinQueryBase, TKey, TValue>> projection)
        {
            var group = projection(new GroupBuilder<object, object>(Anonymize()));

            return this
                .AddStep<IDictionary<TKey, TValue>, object, object, object, object, object>(GroupStep.Instance, QuerySemantics.None)
                .AddStep(new GroupStep.ByTraversalStep(group.KeyQuery), QuerySemantics.None)
                .AddStep(new GroupStep.ByTraversalStep(group.ValueQuery), QuerySemantics.None);
        }

        private GremlinQuery<IDictionary<TKey, object>, object, object, object, object, object> Group<TKey>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IGroupBuilderWithKey<IGremlinQueryBase, TKey>> projection)
        {
            var group = projection(new GroupBuilder<object, object>(Anonymize()));

            return this
                .AddStep<IDictionary<TKey, object>, object, object, object, object, object>(GroupStep.Instance, QuerySemantics.None)
                .AddStep(new GroupStep.ByTraversalStep(group.KeyQuery), QuerySemantics.None);
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
                .Select(projection => Environment.Model.PropertiesModel.GetIdentifier(projection))
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

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(MemberExpression expression, P predicate)
        {
            return predicate.EqualsConstant(false)
                ? None()
                : AddStep(new HasStep(Environment.Model.PropertiesModel.GetIdentifier(expression), predicate));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Has(MemberExpression expression, IGremlinQueryBase traversal)
        {
            return AddStep(new HasStep(Environment.Model.PropertiesModel.GetIdentifier(expression), traversal));
        }

        private GremlinQuery<object, object, object, object, object, object> Id() => AddStepWithObjectTypes<object>(IdStep.Instance, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Identity() => this;

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().ToArray()), QuerySemantics.None);

        private GremlinQuery<TNewElement, object, object, object, object, object> InV<TNewElement>() => AddStepWithObjectTypes<TNewElement>(InVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<string, object, object, object, object, object> Key() => AddStepWithObjectTypes<string>(KeyStep.Instance, QuerySemantics.None);

        private GremlinQuery<string, object, object, object, object, object> Label() => AddStepWithObjectTypes<string>(LabelStep.Instance, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Limit(long count)
        {
            return AddStep(
                count == 1
                    ? LimitStep.LimitGlobal1
                    : new LimitStep(count, Scope.Global));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> LimitLocal(long count)
        {
            return AddStep(
                count == 1
                    ? LimitStep.LimitLocal1
                    : new LimitStep(count, Scope.Local));
        }

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var localTraversalQuery = localTraversal(Anonymize());

            return (localTraversalQuery.IsIdentity()
                ? this
                : AddStep(new LocalStep(localTraversalQuery), QuerySemantics.None)).ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase
        {
            var mappedTraversal = mapping(Anonymize());

            return (mappedTraversal.IsIdentity()
                ? this
                : AddStep(new MapStep(mappedTraversal), QuerySemantics.None)).ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Match(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase>[] matchTraversals) => AddStep(new MatchStep(matchTraversals.Select(traversal => traversal(Anonymize()))));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> None()
        {
            return this.IsIdentity()
                ? new GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(GremlinQuery.AnonymousNoneSteps, Environment, Semantics, StepLabelSemantics, SurfaceVisible)
                : AddStep(NoneStep.Instance);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> notTraversal)
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

            var labels = model
                .TryGetFilterLabels(typeof(TTarget), Environment.Options.GetValue(GremlinqOption.FilterLabelsVerbosity))
                .IfNone(new[] { typeof(TTarget).Name });

            return labels.Length > 0
                ? !Steps.IsEmpty && Steps.Peek() is HasLabelStep hasLabelStep
                    ? ConfigureSteps<TTarget>(steps => steps.Pop().Push(new HasLabelStep(labels.Intersect(hasLabelStep.Labels).ToArray())))
                    : AddStep<TTarget>(new HasLabelStep(labels), Semantics)
                : Cast<TTarget>();
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase
        {
            var optionalQuery = optionalTraversal(Anonymize());

            return this
                .AddStep(new OptionalStep(optionalQuery))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase>[] orTraversalTransformations)
        {
            if (orTraversalTransformations.Length == 0)
                return AddStep(OrStep.Infix);

            var anonymous = Anonymize();
            var subQueries = default(List<IGremlinQueryBase>);

            foreach (var transformation in orTraversalTransformations)
            {
                var transformed = transformation(anonymous);

                if (transformed == anonymous)
                    return this;

                if (!transformed.IsNone())
                    (subQueries ??= new List<IGremlinQueryBase>()).Add(transformed);
            }

            return (subQueries?.Count).GetValueOrDefault() == 0
                ? None()
                : AddStep(new OrStep(subQueries!.ToArray()));
        }

        private TTargetQuery Order<TTargetQuery>(Func<IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>>, IOrderBuilderWithBy<TElement, TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => projection(new OrderBuilder(AddStep(OrderStep.Instance))).Build();

        private GremlinQuery<TTarget, object, object, object, object, object> OtherV<TTarget>() => AddStepWithObjectTypes<TTarget>(OtherVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TTarget, object, object, object, object, object> OutV<TTarget>() => AddStepWithObjectTypes<TTarget>(OutVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TResult, object, object, object, object, object> Project<TActualElement, TResult>(Func<IProjectBuilder<GremlinQuery<TActualElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TActualElement>, IProjectResult> continuation)
        {
            var projections = continuation(new ProjectBuilderImpl<TActualElement, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(Anonymize<TActualElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>(true)))
                .Projections
                .OrderBy(x => x.Key)
                .ToArray();

            var ret = this
                .AddStepWithObjectTypes<TResult>(new ProjectStep(projections.Select(x => x.Key).ToArray()), QuerySemantics.None);

            foreach (var projection in projections)
            {
                ret = ret.AddStep(new ProjectStep.ByTraversalStep(projection.Value), QuerySemantics.None);
            }

            return ret;
        }

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(QuerySemantics querySemantics, params LambdaExpression[] projections) where TNewMeta : class
        {
            return Properties<TNewElement, TNewPropertyValue, TNewMeta>(
                projections
                    .Select(projection => projection.GetMemberInfo().Name),
                querySemantics);
        }

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(IEnumerable<string> keys, QuerySemantics querySemantics) where TNewMeta : class => AddStep<TNewElement, object, object, TNewPropertyValue, TNewMeta, object>(new PropertiesStep(keys.ToArray()), querySemantics);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, [AllowNull] object? value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression && Environment.Model.PropertiesModel.GetIdentifier(memberExpression) is string identifier)
                return Property(identifier, value);

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Property(string key, [AllowNull] object? value)
        {
            return value == null
                ? DropProperties(key)
                : AddStep(new PropertyStep(key, value));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Range(long low, long high) => AddStep(new RangeStep(low, high));

        private TTargetQuery Repeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatQuery = repeatTraversal(Anonymize());

            return this
                .AddStep(new RepeatStep(repeatQuery), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery RepeatUntil<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> untilTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var anonymous = Anonymize();
            var repeatQuery = repeatTraversal(anonymous);

            return this
                .AddStep(new RepeatStep(repeatQuery), QuerySemantics.None)
                .AddStep(new UntilStep(untilTraversal(anonymous)), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery UntilRepeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> untilTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var anonymous = Anonymize();
            var repeatQuery = repeatTraversal(anonymous);

            return this
                .AddStep(new UntilStep(untilTraversal(anonymous)), QuerySemantics.None)
                .AddStep(new RepeatStep(repeatQuery), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, object, object, object, object, object> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel)
        {
            var stepLabelSemantics = StepLabelSemantics
                .TryGetValue(stepLabel)
                .IfNone(() => throw new InvalidOperationException(/* TODO */ ));

            return AddStepWithObjectTypes<TSelectedElement>(new SelectStep(stepLabel), stepLabelSemantics);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> sideEffectTraversal) => AddStep(new SideEffectStep(sideEffectTraversal(Anonymize())));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Skip(long count) => AddStep(new SkipStep(count));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SumGlobal() => AddStep(SumStep.Global, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> SumLocal() => AddStep(SumStep.Local, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Tail(long count) => AddStep(new TailStep(count, Scope.Global));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> TailLocal(long count) => AddStep(new TailStep(count, Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Times(int count) => AddStep(new TimesStep(count));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>(new AddEStep.ToLabelStep(stepLabel));

        private TTargetQuery Unfold<TTargetQuery>() => AddStep(UnfoldStep.Instance).ChangeQueryType<TTargetQuery>();

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQueryBase
        {
            var unionQueries = unionTraversals
                .Select(unionTraversal => (IGremlinQueryBase)unionTraversal(Anonymize()))
                .ToArray();

            return this
                .AddStep(new UnionStep(unionQueries))
                .ChangeQueryType<TTargetQuery>();
        }

        private IValueGremlinQuery<TNewPropertyValue> Value<TNewPropertyValue>() => AddStepWithObjectTypes<TNewPropertyValue>(ValueStep.Instance, QuerySemantics.None);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(string[] keys) => AddStepWithObjectTypes<TNewElement>(new ValueMapStep(keys), QuerySemantics.None);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var projectionsArray = projections
                .ToArray();

            var stringKeys = GetKeys(projectionsArray)
                .OfType<string>()
                .ToArray();

            if (stringKeys.Length != projectionsArray.Length)
                throw new ExpressionNotSupportedException();

            return AddStepWithObjectTypes<TNewElement>(new ValueMapStep(stringKeys), QuerySemantics.None);
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForKeys<TValue>(object[] keys)
        {
            var stepsArray = GetStepsForKeys(keys)
                .ToArray();

            return stepsArray.Length switch
            {
                0 => throw new ExpressionNotSupportedException(),
                1 => AddStepWithObjectTypes<TValue>(stepsArray[0], QuerySemantics.None),
                _ => AddStepWithObjectTypes<TValue>(new UnionStep(stepsArray.Select(step => Anonymize().AddStep(step, QuerySemantics.None))), QuerySemantics.None)
            };
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(GetKeys(projections));

        private GremlinQuery<VertexProperty<TNewPropertyValue>, object, object, TNewPropertyValue, object, object> VertexProperties<TNewPropertyValue>(LambdaExpression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(QuerySemantics.VertexProperty, projections);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> VertexProperty(LambdaExpression projection, [AllowNull] object value)
        {
            if (projection.Body.StripConvert() is MemberExpression memberExpression)
            {
                var identifier = Environment.Model.PropertiesModel.GetIdentifier(memberExpression);

                if (value == null)
                {
                    if (identifier is string stringKey)
                        return DropProperties(stringKey);
                }
                else
                {
                    var ret = this;

                    foreach (var propertyStep in GetPropertySteps(memberExpression.Type, identifier, value, true))
                    {
                        ret = ret.AddStep(propertyStep);
                    }

                    return ret;
                }
            }

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(ILambda lambda) => AddStep(new FilterStep(lambda));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery>, IGremlinQueryBase> filterTraversal)
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
                return Where(predicate.Body, predicate.Parameters[0]);
            }
            catch (ExpressionNotSupportedException ex)
            {
                throw new ExpressionNotSupportedException(predicate, ex);
            }
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(Expression expression, ParameterExpression parameter)
        {
            if (expression is UnaryExpression unaryExpression)
            {
                if (unaryExpression.NodeType == ExpressionType.Not)
                    return Not(_ => _.Where(unaryExpression.Operand, parameter));
            }
            else
            {
                if (expression.TryToGremlinExpression(parameter) is { } gremlinExpression)
                {
                    return Where(gremlinExpression);
                }

                if (expression is BinaryExpression binary)
                {
                    var left = binary.Left.StripConvert();
                    var right = binary.Right.StripConvert();

                    if (binary.NodeType == ExpressionType.OrElse)
                    {
                        return Or(
                            _ => _.Where(binary.Left, parameter),
                            _ => _.Where(binary.Right, parameter));
                    }

                    if (binary.NodeType == ExpressionType.AndAlso)
                    {
                        return this
                            .Where(binary.Left, parameter)
                            .Where(binary.Right, parameter);
                    }

                    var leftHasParameter = left.HasExpressionInMemberChain(parameter);
                    var rightHasParameter = right.HasExpressionInMemberChain(parameter);

                    if (leftHasParameter && rightHasParameter)
                    {
                        if (left is MemberExpression leftMember && right is MemberExpression rightMember)
                        {
                            return As(
                                new StepLabel<TElement>(),
                                (_, label) => _
                                    .Where(binary.NodeType.ToP(label))
                                    .AddStep(new WherePredicateStep.ByMemberStep(Environment.Model.PropertiesModel.GetIdentifier(leftMember)))
                                    .AddStep(new WherePredicateStep.ByMemberStep(Environment.Model.PropertiesModel.GetIdentifier(rightMember))));
                        }
                    }
                    else if (!leftHasParameter && !rightHasParameter)
                    {
                        if (left.GetValue() is StepLabel leftStepLabel && right.GetValue() is StepLabel rightStepLabel)
                            return Where(leftStepLabel, binary.NodeType.ToP(rightStepLabel));
                    }
                }
            }

            throw new ExpressionNotSupportedException(expression);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where<TProjection>(Expression<Func<TElement, TProjection>> predicate, Func<IGremlinQueryBase<TProjection>, IGremlinQueryBase> propertyTraversal)
        {
            return predicate.Body is MemberExpression memberExpression
                ? Has(memberExpression, propertyTraversal(Anonymize<TProjection, object, object, object, object, object>()))
                : throw new ExpressionNotSupportedException(predicate);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(GremlinExpression gremlinExpression)
        {
            var effectivePredicate = gremlinExpression.Predicate is TextP textP
                ? textP.WorkaroundLimitations(Environment.Options)
                : gremlinExpression.Predicate;

            switch (gremlinExpression.Key)
            {
                case MemberExpression leftMemberExpression:
                {
                    var leftMemberExpressionExpression = leftMemberExpression.Expression.StripConvert();

                    if (leftMemberExpressionExpression is ParameterExpression)
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
                    return effectivePredicate.ContainsOnlyStepLabels()
                        ? Has(leftMemberExpression, Anonymize().Where(effectivePredicate))
                        : Has(leftMemberExpression, effectivePredicate);
                }
                case ParameterExpression _:
                {
                    // x => x == P.xy(...)
                    return Where(effectivePredicate);
                }
                case MethodCallExpression methodCallExpression:
                {
                    var targetExpression = methodCallExpression.Object.StripConvert();

                    if (targetExpression != null && typeof(IDictionary<string, object>).IsAssignableFrom(targetExpression.Type) && methodCallExpression.Method.Name == "get_Item")
                    {
                        return AddStep(new HasStep(methodCallExpression.Arguments[0].StripConvert().GetValue(), effectivePredicate));
                    }

                    break;
                }
            }

            throw new ExpressionNotSupportedException();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(P predicate)
        {
            return predicate.ContainsOnlyStepLabels()
                ? AddStep(new WherePredicateStep(predicate))
                : AddStep(new IsStep(predicate));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TPropertyValue, TMeta, TFoldedQuery> Where(StepLabel stepLabel, P predicate)
        {
            return AddStep(new WhereStepLabelAndPredicateStep(stepLabel, predicate));
        }
    }
}
