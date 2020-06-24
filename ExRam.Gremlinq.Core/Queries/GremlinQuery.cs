// ReSharper disable ArrangeThisQualifier
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using Gremlin.Net.Process.Traversal;
using Microsoft.Extensions.Logging;

namespace ExRam.Gremlinq.Core
{
    internal static class GremlinQuery
    {
        internal static readonly IImmutableStack<Step> AnonymousNoneSteps = ImmutableStack<Step>.Empty.Push(NoneStep.Instance);

        public static GremlinQuery<object, object, object, object, object, object> Anonymous(IGremlinQueryEnvironment environment)
        {
            return Create<object>(
                ImmutableStack<Step>.Empty,
                environment,
                QueryFlags.IsAnonymous);
        }

        public static GremlinQuery<TElement, object, object, object, object, object> Create<TElement>(IGremlinQueryEnvironment environment)
        {
            return Create<TElement>(
                ImmutableStack<Step>.Empty,
                environment,
                QueryFlags.SurfaceVisible);
        }

        public static GremlinQuery<TElement, object, object, object, object, object> Create<TElement>(IImmutableStack<Step> steps, IGremlinQueryEnvironment environment, QueryFlags flags)
        {
            return new GremlinQuery<TElement, object, object, object, object, object>(
                steps,
                environment,
                QuerySemantics.None,
                ImmutableDictionary<StepLabel, QuerySemantics>.Empty,
                flags);
        }
    }

    internal sealed partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> : GremlinQueryBase where TMeta : class
    {
        private sealed class OrderBuilder : IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _query;

            public OrderBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> query)
            {
                _query = query;
            }

            GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.Build() => _query;

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(Expression<Func<TElement, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Incr);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Incr);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(ILambda lambda) => By(lambda);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Incr);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.By(ILambda lambda) => By(lambda);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByDescending(Expression<Func<TElement, object?>> projection) => By(projection, Gremlin.Net.Process.Traversal.Order.Decr);

            IOrderBuilderWithBy<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<TElement, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByDescending(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Decr);

            IOrderBuilderWithBy<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>> IOrderBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByDescending(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal) => By(traversal, Gremlin.Net.Process.Traversal.Order.Decr);

            private OrderBuilder By(Expression<Func<TElement, object?>> projection, Order order) => new OrderBuilder(_query.AddStep(new OrderStep.ByMemberStep(_query.GetKey(projection), order)));

            private OrderBuilder By(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversal, Order order) => new OrderBuilder(_query.AddStep(new OrderStep.ByTraversalStep(_query.Continue(traversal).ToTraversal(), order)));

            private OrderBuilder By(ILambda lambda) => new OrderBuilder(_query.AddStep(new OrderStep.ByLambdaStep(lambda)));
        }

        private sealed class ChooseBuilder<TTargetQuery, TPickElement> :
            IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>,
            IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement>,
            IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TTargetQuery>
            where TTargetQuery : IGremlinQueryBase
        {
            private readonly IGremlinQueryBase _targetQuery;
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _sourceQuery;

            public ChooseBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery, IGremlinQueryBase targetQuery)
            {
                _sourceQuery = sourceQuery;
                _targetQuery = targetQuery;
            }

            public IChooseBuilderWithCondition<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewPickElement> On<TNewPickElement>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewPickElement>> chooseTraversal)
            {
                return new ChooseBuilder<TTargetQuery, TNewPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep<IGremlinQueryBase>(new ChooseOptionTraversalStep(_sourceQuery.Continue(chooseTraversal).ToTraversal())));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TNewTargetQuery> Case<TNewTargetQuery>(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep<IGremlinQueryBase>(new OptionTraversalStep(element, _sourceQuery.Continue(continuation).ToTraversal())));
            }

            public IChooseBuilderWithCaseOrDefault<TNewTargetQuery> Default<TNewTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewTargetQuery> continuation) where TNewTargetQuery : IGremlinQueryBase
            {
                return new ChooseBuilder<TNewTargetQuery, TPickElement>(
                    _sourceQuery,
                    _targetQuery.AsAdmin().AddStep<IGremlinQueryBase>(new OptionTraversalStep(default, _sourceQuery.Continue(continuation).ToTraversal())));
            }

            public IChooseBuilderWithCase<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TPickElement, TTargetQuery> Case(TPickElement element, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Case<TTargetQuery>(element, continuation);

            public IChooseBuilderWithCaseOrDefault<TTargetQuery> Default(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> continuation) => Default<TTargetQuery>(continuation);

            public TTargetQuery TargetQuery
            {
                get => _targetQuery
                    .AsAdmin()
                    .ChangeQueryType<TTargetQuery>();
            }
        }

        private sealed class GroupBuilder<TKey, TValue> :
            IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>,
            IGroupBuilderWithKeyAndValue<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey, TValue>
        {
            private readonly GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _sourceQuery;

            public GroupBuilder(GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery, IGremlinQueryBase<TKey>? keyQuery = default, IGremlinQueryBase<TValue>? valueQuery = default)
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                KeyQuery = keyQuery;
                ValueQuery = valueQuery;
#pragma warning restore CS8601 // Possible null reference assignment.

                _sourceQuery = sourceQuery;
            }

            IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TNewKey> IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>.ByKey<TNewKey>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewKey>> keySelector)
            {
                return new GroupBuilder<TNewKey, object>(
                    _sourceQuery,
                    _sourceQuery.Continue(keySelector));
            }

            IGroupBuilderWithKeyAndValue<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey, TNewValue> IGroupBuilderWithKey<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TKey>.ByValue<TNewValue>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase<TNewValue>> valueSelector)
            {
                return new GroupBuilder<TKey, TNewValue>(
                    _sourceQuery,
                    KeyQuery,
                    _sourceQuery.Continue(valueSelector));
            }

            public IGremlinQueryBase<TKey> KeyQuery { get; }

            public IGremlinQueryBase<TValue> ValueQuery { get; }
        }

        private sealed partial class ProjectBuilder<TProjectElement, TItem1, TItem2, TItem3, TItem4, TItem5, TItem6, TItem7, TItem8, TItem9, TItem10, TItem11, TItem12, TItem13, TItem14, TItem15, TItem16> :
            IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>,
            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>
        {
            private readonly GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> _sourceQuery;

            public ProjectBuilder(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery) : this(sourceQuery, ImmutableDictionary<string, ProjectStep.ByStep>.Empty)
            {
            }

            private ProjectBuilder(GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> sourceQuery, IImmutableDictionary<string, ProjectStep.ByStep> projections)
            {
                _sourceQuery = sourceQuery;
                Projections = projections;
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection, string? name = default)
            {
                //TODO: Unwrap single local(...) in by(...).
                return By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(new ProjectStep.ByTraversalStep(_sourceQuery.Continue(projection, true).ToTraversal()), name);
            }
           
            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(Expression projection, string? name = default)
            {
                return projection is LambdaExpression lambdaExpression && lambdaExpression.IsIdentityExpression()
                    ? By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(__ => __.Identity(), name)
                    : By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(new ProjectStep.ByKeyStep(_sourceQuery.GetKey(projection)), name);
            }

            private ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16> By<TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(ProjectStep.ByStep step, string? name = default)
            {
                return new ProjectBuilder<TProjectElement, TNewItem1, TNewItem2, TNewItem3, TNewItem4, TNewItem5, TNewItem6, TNewItem7, TNewItem8, TNewItem9, TNewItem10, TNewItem11, TNewItem12, TNewItem13, TNewItem14, TNewItem15, TNewItem16>(
                    _sourceQuery,
                    Projections.SetItem(name ?? $"Item{Projections.Count + 1}", step));
            }

            IProjectTupleBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.ToTuple()
            {
                return this;
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.ToDynamic()
            {
                return this;
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return By<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(string name, Func<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> projection)
            {
                return By<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(string name, Expression<Func<TProjectElement, object>> projection)
            {
                return By<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(projection, name);
            }

            IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement> IProjectDynamicBuilder<GremlinQuery<TProjectElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TProjectElement>.By(Expression<Func<TProjectElement, object>> projection)
            {
                return projection.IsIdentityExpression()
                    ? By<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(__ => __.Identity())
                    : projection.Body.Strip() is MemberExpression memberExpression
                        ? By<object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(memberExpression, memberExpression.Member.Name)
                        : throw new ExpressionNotSupportedException(projection);
            }

            public IImmutableDictionary<string, ProjectStep.ByStep> Projections { get; }
        }

        public GremlinQuery(
            IImmutableStack<Step> steps,
            IGremlinQueryEnvironment environment,
            QuerySemantics semantics,
            IImmutableDictionary<StepLabel, QuerySemantics> stepLabelSemantics,
            QueryFlags flags) : base(steps, environment, semantics, stepLabelSemantics, flags)
        {

        }

        private GremlinQuery<TEdge, TElement, object, object, object, object> AddE<TEdge>(TEdge newEdge)
        {
            return this
                .AddStep<TEdge, TElement, object, object, object, object>(new AddEStep(Environment.Model.EdgesModel.GetLabel(newEdge!.GetType())), QuerySemantics.Edge)
                .AddOrUpdate(newEdge, true, false, Environment.FeatureSet.Supports(EdgeFeatures.UserSuppliedIds));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddOrUpdate(TElement element, bool add, bool allowExplicitCardinality, bool allowUserSuppliedId)
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
                        props
                            .Select(p => p.key.RawKey)
                            .OfType<string>(),
                        Semantics)
                    .Drop());
            }

            foreach (var (key, value) in props)
            {
                if (!allowUserSuppliedId && T.Id.Equals(key.RawKey))
                    Environment.Logger.LogWarning("User supplied ids are not supported according to the envrionment's FeatureSet.");
                else
                {
                    foreach (var propertyStep in GetPropertySteps(key, value, allowExplicitCardinality))
                    {
                        ret = ret.AddStep(propertyStep);
                    }
                }
            }

            return ret;
        }

        private IEnumerable<PropertyStep> GetPropertySteps(Key key, object value, bool allowExplicitCardinality)
        {
            if (value is IEnumerable enumerable && !Environment.Model.NativeTypes.Contains(value.GetType()))
            {
                if (!allowExplicitCardinality)
                    throw new NotSupportedException($"A value of type {value.GetType()} is not supported for property '{key}'.");

                foreach (var item in enumerable)
                {
                    yield return GetPropertyStep(key, item, Cardinality.List);
                }
            }
            else
                yield return GetPropertyStep(key, value, allowExplicitCardinality ? Cardinality.Single : default);
        }

        private PropertyStep GetPropertyStep(Key key, object value, Cardinality? cardinality)
        {
            var metaProperties = ImmutableArray<object>.Empty;

            if (value is Property property)
            {
                if (value is IVertexProperty)
                {
                    if (property.GetMetaProperties(Environment) is { } dict)
                    {
                        metaProperties = dict
                            .SelectMany(kvp => new[] { kvp.Key, kvp.Value })
                            .ToImmutableArray();
                    }
                }

                value = property.GetValue();
            }

            return new PropertyStep(key, value, metaProperties, cardinality);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddStep(Step step, QuerySemantics? querySemantics = null, IImmutableDictionary<StepLabel, QuerySemantics>? stepLabelSemantics = null, QueryFlags additionalFlags = QueryFlags.None) => AddStep<TElement>(step, querySemantics, stepLabelSemantics, additionalFlags);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> AddStep<TNewElement>(Step step, QuerySemantics? querySemantics = null, IImmutableDictionary<StepLabel, QuerySemantics>? stepLabelSemantics = null, QueryFlags additionalFlags = QueryFlags.None) => AddStep<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(step, querySemantics, stepLabelSemantics, additionalFlags);

        private GremlinQuery<TNewElement, object, object, object, object, object> AddStepWithObjectTypes<TNewElement>(Step step, QuerySemantics? querySemantics = null) => AddStep<TNewElement, object, object, object, object, object>(step, querySemantics);

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> AddStep<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Step step, QuerySemantics? querySemantics = null, IImmutableDictionary<StepLabel, QuerySemantics>? stepLabelSemantics = null, QueryFlags additionalFlags = QueryFlags.None) where TNewMeta : class
        {
            var newSteps = Steps;

            if ((Flags & QueryFlags.IsMuted) == 0)
                newSteps = Environment.AddStepHandler.AddStep(newSteps, step, Environment);

            return new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(newSteps, Environment, querySemantics ?? Semantics, stepLabelSemantics ?? StepLabelSemantics, Flags | additionalFlags);
        }
        
        private GremlinQuery<TVertex, object, object, object, object, object> AddV<TVertex>(TVertex vertex)
        {
            return this
                .AddStepWithObjectTypes<TVertex>(new AddVStep(Environment.Model.VerticesModel.GetLabel(vertex!.GetType())), QuerySemantics.Vertex)
                .AddOrUpdate(vertex, true, true, Environment.FeatureSet.Supports(VertexFeatures.UserSuppliedIds));
        }

        private TTargetQuery Aggregate<TStepLabel, TTargetQuery>(Scope scope, TStepLabel stepLabel, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(
                AddStep(new AggregateStep(scope, stepLabel)),
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> And(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase>[] andTraversalTransformations)
        {
            if (andTraversalTransformations.Length == 0)
                return AddStep(AndStep.Infix);

            var subQueries = default(List<IGremlinQueryBase>);

            foreach (var transformation in andTraversalTransformations)
            {
                var transformed = Continue(transformation);

                if (transformed.IsNone())
                    return None();

                if (!transformed.IsIdentity())
                    (subQueries ??= new List<IGremlinQueryBase>()).Add(transformed);
            }

            return (subQueries?.Count).GetValueOrDefault() == 0
                ? this
                : (subQueries!.Count == 1)
                    ? Where(subQueries[0])
                    : AddStep(new AndStep(subQueries.Select(x => x.ToTraversal())));
        }

        private TTargetQuery Continue<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> transformation, bool surfaceVisible = false)
        {
            var targetQuery = transformation(new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(ImmutableStack<Step>.Empty, Environment, Semantics, StepLabelSemantics, (surfaceVisible ? Flags | QueryFlags.SurfaceVisible : Flags & ~QueryFlags.SurfaceVisible) | QueryFlags.IsAnonymous));

            if (targetQuery is GremlinQueryBase queryBase && (queryBase.Flags & QueryFlags.IsAnonymous) == QueryFlags.None)
                throw new InvalidOperationException("A query continuation must originate from query that was passed to the continuation function. Did you accidentally use 'g' in the continuation?");

            return targetQuery;
        }

        private TTargetQuery As<TStepLabel, TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TStepLabel, TTargetQuery> continuation)
            where TStepLabel : StepLabel, new()
            where TTargetQuery : IGremlinQueryBase
        {
            var toContinue = this;
            var stepLabel = default(TStepLabel);

            if (Steps.PeekOrDefault() is AsStep asStep && asStep.StepLabel is TStepLabel existingStepLabel)
                stepLabel = existingStepLabel;
            else
            {
                stepLabel = new TStepLabel();
                toContinue = As(stepLabel);
            }

            return continuation(
                toContinue,
                stepLabel);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> As(StepLabel stepLabel)
        {
            return AddStep(
                new AsStep(stepLabel),
                default,
                StepLabelSemantics.SetItem(stepLabel, Semantics));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Barrier() => AddStep(BarrierStep.Instance);

        private GremlinQuery<TTarget, object, object, object, object, object> BothV<TTarget>() => AddStepWithObjectTypes<TTarget>(BothVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Cast<TNewElement>() => Cast<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>();

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery> Cast<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>() where TNewMeta : class => new GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, TNewPropertyValue, TNewMeta, TNewFoldedQuery>(Steps, Environment, Semantics, StepLabelSemantics, Flags);

        private TTargetQuery Choose<TTargetQuery>(Expression<Func<TElement, bool>> predicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>? maybeFalseChoice = default) where TTargetQuery : IGremlinQueryBase
        {
            var trueQuery = Continue(trueChoice);
            var maybeFalseQuery = maybeFalseChoice is { } falseChoice
                ? Continue(falseChoice)
                : default;

            var query = Continue(__ => __.Where(predicate));

            if (query.Steps.TryGetSingleStep() is IsStep isStep)
            {
                return this
                    .AddStep(
                        new ChoosePredicateStep(
                            isStep.Predicate,
                            trueQuery.ToTraversal(),
                            maybeFalseQuery?.ToTraversal()),
                        QuerySemantics.None)
                    .ChangeQueryType<TTargetQuery>();
            }

            return this
                .AddStep(
                    new ChooseTraversalStep(
                        query.ToTraversal(),
                        trueQuery.ToTraversal(),
                        maybeFalseQuery?.ToTraversal()),
                    QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Choose<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> traversalPredicate, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> trueChoice, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>? maybeFalseChoice = default) where TTargetQuery : IGremlinQueryBase
        {
            var trueQuery = Continue(trueChoice);
            var maybeFalseQuery = maybeFalseChoice is { } falseChoice
                ? Continue(falseChoice)
                : default;

            return AddStep(new ChooseTraversalStep(Continue(traversalPredicate).ToTraversal(), trueQuery.ToTraversal(), maybeFalseQuery?.ToTraversal()), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Choose<TTargetQuery>(Func<IChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IChooseBuilderWithCaseOrDefault<TTargetQuery>> continuation)
            where TTargetQuery : IGremlinQueryBase
        {
            return continuation(new ChooseBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, object>(this, this)).TargetQuery;
        }

        private TTargetQuery Coalesce<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>[] traversals)
            where TTargetQuery : IGremlinQueryBase
        {
            if (traversals.Length == 0)
                throw new ArgumentException("Coalesce must have at least one subquery.");

            var coalesceQueries = traversals
                .Select(traversal => Continue(traversal))
                .ToArray();

            return (coalesceQueries.All(x => x.IsIdentity())
                ? this
                : AddStep(new CoalesceStep(coalesceQueries.Select(x => x.ToTraversal()).ToImmutableArray()), QuerySemantics.None)).ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Coin(double probability) => AddStep(new CoinStep(probability));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation) => Configure<TElement>(_ => _, transformation);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> ConfigureSteps<TNewElement>(Func<IImmutableStack<Step>, IImmutableStack<Step>> transformation) => Configure<TNewElement>(transformation, _ => _);

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Configure<TNewElement>(
            Func<IImmutableStack<Step>, IImmutableStack<Step>> stepsTransformation,
            Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> environmentTransformation) => new GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(stepsTransformation(Steps), environmentTransformation(Environment), Semantics, StepLabelSemantics, Flags);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DedupGlobal() => AddStep(DedupStep.Global);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DedupLocal() => AddStep(DedupStep.Local);

        private GremlinQuery<object, object, object, object, object, object> Drop() => AddStepWithObjectTypes<object>(DropStep.Instance, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> DropProperties(string key)
        {
            return SideEffect(_ => _
                .Properties<object, object, object>(new[] { key }, QuerySemantics.Property)
                .Drop());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Emit() => AddStep(EmitStep.Instance);

        private TTargetQuery FlatMap<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase
        {
            var mappedTraversal = Continue(mapping);

            return this
                .AddStep(new FlatMapStep(mappedTraversal.ToTraversal()), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement[], object, object, TElement, object, TNewFoldedQuery> Fold<TNewFoldedQuery>() => AddStep<TElement[], object, object, TElement, object, TNewFoldedQuery>(FoldStep.Instance, QuerySemantics.None);

        private GremlinQuery<IDictionary<TKey, TValue>, object, object, object, object, object> Group<TKey, TValue>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IGroupBuilderWithKeyAndValue<IGremlinQueryBase, TKey, TValue>> projection)
        {
            var group = projection(new GroupBuilder<object, object>(this));

            return this
                .AddStep<IDictionary<TKey, TValue>, object, object, object, object, object>(GroupStep.Instance, QuerySemantics.None)
                .AddStep(new GroupStep.ByTraversalStep(group.KeyQuery.ToTraversal()), QuerySemantics.None)
                .AddStep(new GroupStep.ByTraversalStep(group.ValueQuery.ToTraversal()), QuerySemantics.None);
        }

        private GremlinQuery<IDictionary<TKey, object>, object, object, object, object, object> Group<TKey>(Func<IGroupBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>, IGroupBuilderWithKey<IGremlinQueryBase, TKey>> projection)
        {
            var group = projection(new GroupBuilder<object, object>(this));

            return this
                .AddStep<IDictionary<TKey, object>, object, object, object, object, object>(GroupStep.Instance, QuerySemantics.None)
                .AddStep(new GroupStep.ByTraversalStep(group.KeyQuery.ToTraversal()), QuerySemantics.None);
        }

        private Key[] GetKeys(IEnumerable<Expression> projections)
        {
            return projections
                .Select(projection => GetKey(projection))
                .ToArray();
        }

        private Key GetKey(Expression projection)
        {
            return Environment.Model.PropertiesModel.GetKey(projection);
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private static IEnumerable<Step> GetStepsForKeys(Key[] keys)
        {
            var hasYielded = false;

            foreach (var t in keys.Select(x => x.RawKey).OfType<T>())
            {
                if (T.Id.Equals(t))
                    yield return IdStep.Instance;
                else if (T.Label.Equals(t))
                    yield return LabelStep.Instance;
                else
                    throw new ExpressionNotSupportedException($"Can't find an appropriate Gremlin step for {t}.");

                hasYielded = true;
            }

            var stringKeys = keys
                .Select(x => x.RawKey)
                .OfType<string>()
                .ToImmutableArray();

            if (stringKeys.Length > 0 || !hasYielded)
                yield return new ValuesStep(stringKeys);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Has(MemberExpression expression, P predicate)
        {
            return predicate.EqualsConstant(false)
                ? None()
                : Has(
                    GetKey(expression),
                    predicate.EqualsConstant(true)
                        ? default
                        : predicate);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Has(Key key, P? predicate)
        {
            return AddStep(new HasPredicateStep(key, predicate));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Has(MemberExpression expression, IGremlinQueryBase traversal)
        {
            return AddStep(new HasTraversalStep(GetKey(expression), traversal.ToTraversal()));
        }

        private GremlinQuery<object, object, object, object, object, object> Id() => AddStepWithObjectTypes<object>(IdStep.Instance, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Identity() => this;

        private GremlinQuery<TNewElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Inject<TNewElement>(IEnumerable<TNewElement> elements) => AddStep<TNewElement>(new InjectStep(elements.Cast<object>().ToImmutableArray()), QuerySemantics.None);

        private GremlinQuery<TNewElement, object, object, object, object, object> InV<TNewElement>() => AddStepWithObjectTypes<TNewElement>(InVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Is(P predicate)
        {
            if (predicate.Value == null)
                throw new ExpressionNotSupportedException();

            return AddStep(new IsStep(predicate));
        }

        private GremlinQuery<string, object, object, object, object, object> Key() => AddStepWithObjectTypes<string>(KeyStep.Instance, QuerySemantics.None);

        private GremlinQuery<string, object, object, object, object, object> Label() => AddStepWithObjectTypes<string>(LabelStep.Instance, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Limit(long count)
        {
            return AddStep(
                count == 1
                    ? LimitStep.LimitGlobal1
                    : new LimitStep(count, Scope.Global));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> LimitLocal(long count)
        {
            return AddStep(
                count == 1
                    ? LimitStep.LimitLocal1
                    : new LimitStep(count, Scope.Local));
        }

        private TTargetQuery Local<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> localTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var localTraversalQuery = Continue(localTraversal);

            return (localTraversalQuery.IsIdentity()
                ? this
                : AddStep(new LocalStep(localTraversalQuery.ToTraversal()), QuerySemantics.None)).ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery Map<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> mapping) where TTargetQuery : IGremlinQueryBase
        {
            var mappedTraversal = Continue(mapping);

            return (mappedTraversal.IsIdentity()
                ? this
                : AddStep(new MapStep(mappedTraversal.ToTraversal()), QuerySemantics.None)).ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> None()
        {
            return this.IsIdentity()
                ? ConfigureSteps<TElement>(_ => GremlinQuery.AnonymousNoneSteps)
                : AddStep(NoneStep.Instance);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Not(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> notTraversal)
        {
            var transformed = Continue(notTraversal);
            
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
                ? AddStep<TTarget>(new HasLabelStep(labels), Semantics)
                : Cast<TTarget>();
        }

        private TTargetQuery Optional<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> optionalTraversal) where TTargetQuery : IGremlinQueryBase
        {
            var optionalQuery = Continue(optionalTraversal);

            return this
                .AddStep(new OptionalStep(optionalQuery.ToTraversal()))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Or(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase>[] orTraversalTransformations)
        {
            return Or(orTraversalTransformations.Select(transformation => Continue(transformation)).ToArray());
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Or(
            GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> left,
            GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> right)
        {
            if (left.Steps.TryGetSingleStep() is { } leftStep && right.Steps.TryGetSingleStep() is { } rightStep)
            {
                switch (leftStep)
                {
                    case HasPredicateStep leftHasPredicate when rightStep is HasPredicateStep rightHasPredicateStep && leftHasPredicate.Key == rightHasPredicateStep.Key:
                    {
                        return Has(
                            leftHasPredicate.Key,
                            leftHasPredicate.Predicate is { } leftP && rightHasPredicateStep.Predicate is { } rightP
                                ? leftP.Or(rightP)
                                : null);
                    }
                    case IsStep leftIsStep when rightStep is IsStep rightIsStep:
                    {
                        return Is(leftIsStep.Predicate.Or(rightIsStep.Predicate));
                    }
                }
            }

            return Or(new IGremlinQueryBase[] { left, right });
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Or(params IGremlinQueryBase[] orTraversals)
        {
            if (orTraversals.Length == 0)
                return AddStep(OrStep.Infix);

            var subQueries = default(List<IGremlinQueryBase>);

            foreach (var transformed in orTraversals)
            {
                if (transformed.IsIdentity())
                    return this;

                if (!transformed.IsNone())
                    (subQueries ??= new List<IGremlinQueryBase>()).Add(transformed);
            }

            return (subQueries?.Count).GetValueOrDefault() == 0
                ? None()
                : subQueries!.Count == 1
                    ? Where(subQueries[0])
                    : AddStep(new OrStep(subQueries.Select(x => x.ToTraversal())));
        }

        private TTargetQuery OrderGlobal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => Order(projection, OrderStep.Global);

        private TTargetQuery OrderLocal<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection) where TTargetQuery : IGremlinQueryBase<TElement> => Order(projection, OrderStep.Local);

        private TTargetQuery Order<TTargetQuery>(Func<OrderBuilder, IOrderBuilderWithBy<TTargetQuery>> projection, OrderStep orderStep) where TTargetQuery : IGremlinQueryBase<TElement> => projection(new OrderBuilder(AddStep(orderStep))).Build();

        private GremlinQuery<TTarget, object, object, object, object, object> OtherV<TTarget>() => AddStepWithObjectTypes<TTarget>(OtherVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TTarget, object, object, object, object, object> OutV<TTarget>() => AddStepWithObjectTypes<TTarget>(OutVStep.Instance, QuerySemantics.Vertex);

        private GremlinQuery<TResult, object, object, object, object, object> Project<TResult>(Func<IProjectBuilder<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TElement>, IProjectResult> continuation)
        {
            var projections = continuation(new ProjectBuilder<TElement, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object, object>(this))
                .Projections
                .OrderBy(x => x.Key)
                .ToArray();

            var ret = this
                .AddStepWithObjectTypes<TResult>(new ProjectStep(projections.Select(x => x.Key).ToImmutableArray()), QuerySemantics.None);

            foreach (var projection in projections)
            {
                ret = ret.AddStep(projection.Value, QuerySemantics.None);
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

        private GremlinQuery<TNewElement, object, object, TNewPropertyValue, TNewMeta, object> Properties<TNewElement, TNewPropertyValue, TNewMeta>(IEnumerable<string> keys, QuerySemantics querySemantics) where TNewMeta : class => AddStep<TNewElement, object, object, TNewPropertyValue, TNewMeta, object>(new PropertiesStep(keys.ToImmutableArray()), querySemantics);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property<TSource, TValue>(Expression<Func<TSource, TValue>> projection, object? value)
        {
            return Property(GetKey(projection), value);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Property(Key key, object? value)
        {
            return value == null
                ? key.RawKey is string name
                    ? DropProperties(name)
                    : throw new InvalidOperationException("Can't set a special property to null.")
                : AddStep(new PropertyStep(key, value));
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Range(long low, long high, Scope scope) => AddStep(new RangeStep(low, high, scope));

        private TTargetQuery Repeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatQuery = Continue(repeatTraversal);

            return this
                .AddStep(new RepeatStep(repeatQuery.ToTraversal()), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery RepeatUntil<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> untilTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatQuery = Continue(repeatTraversal);

            return this
                .AddStep(new RepeatStep(repeatQuery.ToTraversal()), QuerySemantics.None)
                .AddStep(new UntilStep(Continue(untilTraversal).ToTraversal()), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private TTargetQuery UntilRepeat<TTargetQuery>(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery> repeatTraversal, Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> untilTraversal)
            where TTargetQuery : IGremlinQueryBase
        {
            var repeatQuery = Continue(repeatTraversal);

            return this
                .AddStep(new UntilStep(Continue(untilTraversal).ToTraversal()), QuerySemantics.None)
                .AddStep(new RepeatStep(repeatQuery.ToTraversal()), QuerySemantics.None)
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, object, object, object, object, object> Select<TSelectedElement>(StepLabel<TSelectedElement> stepLabel)
        {
            if (StepLabelSemantics.TryGetValue(stepLabel, out var stepLabelSemantics))
                return AddStepWithObjectTypes<TSelectedElement>(new SelectStep(ImmutableArray.Create<StepLabel>(stepLabel)), stepLabelSemantics);

            throw new InvalidOperationException($"Invalid use of unknown {nameof(StepLabel)} in {nameof(Select)}. Make sure you only pass in a {nameof(StepLabel)} that comes from a previous {nameof(As)}-continuation or has previously been passed to an appropriate overload of {nameof(As)}.");
        }

        private TTargetQuery Select<TTargetQuery>(params Expression[] projections)
        {
            var keys = projections
                .Select(projection =>
                {
                    if (projection is LambdaExpression lambdaExpression && lambdaExpression.Body is MemberExpression memberExpression && memberExpression.Expression == lambdaExpression.Parameters[0])
                        return (Key)memberExpression.Member.Name;

                    throw new ExpressionNotSupportedException(projection);
                })
                .ToImmutableArray();

            return AddStep(new SelectKeysStep(keys))
                .ChangeQueryType<TTargetQuery>();
        }

        private GremlinQuery<TSelectedElement, object, object, TArrayItem, object, TQuery> Cap<TSelectedElement, TArrayItem, TQuery>(StepLabel<IArrayGremlinQuery<TSelectedElement, TArrayItem, TQuery>, TSelectedElement> stepLabel) where TQuery : IGremlinQueryBase => AddStep<TSelectedElement, object, object, TArrayItem, object, TQuery>(new CapStep(stepLabel), QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SideEffect(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> sideEffectTraversal) => AddStep(new SideEffectStep(Continue(sideEffectTraversal).ToTraversal()));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Skip(long count, Scope scope) => AddStep(new SkipStep(count, scope));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SumGlobal() => AddStep(SumStep.Global, QuerySemantics.None);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> SumLocal() => AddStep<object>(SumStep.Local, QuerySemantics.None);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MinLocal() => AddStep<object>(MinStep.Local, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Mute() => AddStep(NoneStep.Instance, additionalFlags: QueryFlags.IsMuted);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MinGlobal() => AddStep(MinStep.Global, QuerySemantics.None);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MaxLocal() => AddStep<object>(MaxStep.Local, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MaxGlobal() => AddStep(MaxStep.Global, QuerySemantics.None);

        private GremlinQuery<object, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MeanLocal() => AddStep<object>(MeanStep.Local, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> MeanGlobal() => AddStep(MeanStep.Global, QuerySemantics.None);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Tail(long count) => AddStep(new TailStep(count, Scope.Global));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> TailLocal(long count) => AddStep(new TailStep(count, Scope.Local));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Times(int count) => AddStep(new TimesStep(count));

        private GremlinQuery<TNewElement, TNewOutVertex, TNewInVertex, object, object, object> To<TNewElement, TNewOutVertex, TNewInVertex>(StepLabel stepLabel) => AddStep<TNewElement, TNewOutVertex, TNewInVertex, object, object, object>(new AddEStep.ToLabelStep(stepLabel));

        private TTargetQuery Unfold<TTargetQuery>() => AddStep(UnfoldStep.Instance).ChangeQueryType<TTargetQuery>();

        private TTargetQuery Union<TTargetQuery>(params Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, TTargetQuery>[] unionTraversals) where TTargetQuery : IGremlinQueryBase
        {
            var unionQueries = unionTraversals
                .Select(unionTraversal => ((IGremlinQueryBase)Continue(unionTraversal)).ToTraversal())
                .ToImmutableArray();

            return this
                .AddStep(new UnionStep(unionQueries))
                .ChangeQueryType<TTargetQuery>();
        }

        private IValueGremlinQuery<TNewPropertyValue> Value<TNewPropertyValue>() => AddStepWithObjectTypes<TNewPropertyValue>(ValueStep.Instance, QuerySemantics.None);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(ImmutableArray<string> keys) => AddStepWithObjectTypes<TNewElement>(new ValueMapStep(keys), QuerySemantics.None);

        private GremlinQuery<TNewElement, object, object, object, object, object> ValueMap<TNewElement>(IEnumerable<LambdaExpression> projections)
        {
            var projectionsArray = projections
                .ToArray();

            var stringKeys = GetKeys(projectionsArray)
                .Select(x => x.RawKey)
                .OfType<string>()
                .ToImmutableArray();

            if (stringKeys.Length != projectionsArray.Length)
                throw new ExpressionNotSupportedException($"One of the expressions in {nameof(ValueMap)} maps to a {nameof(T)}-token. Can't have special tokens in {nameof(ValueMap)}.");

            return AddStepWithObjectTypes<TNewElement>(new ValueMapStep(stringKeys), QuerySemantics.None);
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForKeys<TValue>(Key[] keys)
        {
            var stepsArray = GetStepsForKeys(keys)
                .ToArray();

            return stepsArray.Length switch
            {
                0 => throw new ExpressionNotSupportedException(),
                1 => AddStepWithObjectTypes<TValue>(stepsArray[0], QuerySemantics.None),
                _ => AddStepWithObjectTypes<TValue>(new UnionStep(stepsArray.Select(step => Continue(__ => __.AddStep(step, QuerySemantics.None).ToTraversal())).ToImmutableArray()), QuerySemantics.None)
            };
        }

        private GremlinQuery<TValue, object, object, object, object, object> ValuesForProjections<TValue>(IEnumerable<LambdaExpression> projections) => ValuesForKeys<TValue>(GetKeys(projections));

        private GremlinQuery<VertexProperty<TNewPropertyValue>, object, object, TNewPropertyValue, object, object> VertexProperties<TNewPropertyValue>(LambdaExpression[] projections) => Properties<VertexProperty<TNewPropertyValue>, TNewPropertyValue, object>(QuerySemantics.VertexProperty, projections);

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> VertexProperty(LambdaExpression projection, object? value)
        {
            var key = GetKey(projection);

            if (value == null)
            {
                if (key.RawKey is string stringKey)
                    return DropProperties(stringKey);
            }
            else
            {
                var ret = this;

                foreach (var propertyStep in GetPropertySteps(key, value, true))
                {
                    ret = ret.AddStep(propertyStep);
                }

                return ret;
            }

            throw new ExpressionNotSupportedException(projection);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(ILambda lambda) => AddStep(new FilterStep(lambda));

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Func<GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>, IGremlinQueryBase> filterTraversal)
        {
            var filtered = Continue(filterTraversal);

            return filtered.IsIdentity()
                ? this
                : filtered.IsNone()
                    ? None()
                    : Where(filtered);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(IGremlinQueryBase query)
        {
            return AddStep(
                query.AsAdmin().Steps.TryGetSingleStep() switch
                {
                    HasPredicateStep hasPredicateStep => hasPredicateStep,
                    WhereTraversalStep whereTraversalStep => new WhereTraversalStep(whereTraversalStep.Traversal),
                    _ => new WhereTraversalStep(query.ToTraversal())
                });
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(Expression expression)
        {
            try
            {
                switch (expression)
                {
                    case ConstantExpression constantExpression when constantExpression.GetValue(Environment.Model) is bool value:
                    {
                        return value
                            ? this
                            : None();
                    }
                    case LambdaExpression lambdaExpression:
                    {
                        return Where(lambdaExpression.Body);
                    }
                    case UnaryExpression unaryExpression when unaryExpression.NodeType == ExpressionType.Not:
                    {
                        return Not(_ => _.Where(unaryExpression.Operand));
                    }
                    case BinaryExpression binary when binary.NodeType == ExpressionType.OrElse:
                    {
                        return Or(
                            Continue(__ => __.Where(binary.Left)),
                            Continue(__ => __.Where(binary.Right)));
                    }
                    case BinaryExpression binary when binary.NodeType == ExpressionType.AndAlso:
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
                            : Where(gremlinExpression);
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
            return predicate.RefersToParameter() && predicate.Body is MemberExpression memberExpression
                ? Has(memberExpression, Cast<TProjection>().Continue(propertyTraversal))
                : throw new ExpressionNotSupportedException(predicate);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(GremlinExpression gremlinExpression)
        {
            return Where(
                gremlinExpression.Left,
                gremlinExpression.Semantics,
                gremlinExpression.Right);
        }

        private GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> Where(ExpressionFragment left, ExpressionSemantics semantics, ExpressionFragment right)
        {
            switch (right)
            {
                case ConstantExpressionFragment rightConstantFragment:
                {
                    var effectivePredicate = semantics
                        .ToP(rightConstantFragment.Value)
                        .WorkaroundLimitations(Environment.Options);

                    switch (left)
                    {
                        case ParameterExpressionFragment leftParameterFragment:
                        {
                            switch (leftParameterFragment.Expression)
                            {
                                case MemberExpression leftMemberExpression:
                                {
                                    var memberSemantics = leftMemberExpression.TryGetWellKnownMember();
                                    var leftMemberExpressionExpression = leftMemberExpression.Expression.Strip();

                                    if (leftMemberExpressionExpression is ParameterExpression)
                                    {
                                        switch (memberSemantics)
                                        {
                                            // x => x.Value == P.xy(...)
                                            case WellKnownMember.PropertyValue when !(rightConstantFragment.Value is StepLabel):
                                                return AddStep(new HasValueStep(effectivePredicate));
                                            case WellKnownMember.PropertyKey:
                                                return Where(__ => __
                                                    .Key()
                                                    .Where(
                                                        ExpressionFragment.Create(leftMemberExpression.Expression, Environment.Model),
                                                        semantics,
                                                        right));
                                            case WellKnownMember.VertexPropertyLabel when rightConstantFragment.Value is StepLabel:
                                                return Where(__ => __
                                                    .Label()
                                                    .Where(
                                                        ExpressionFragment.Create(leftMemberExpression.Expression, Environment.Model),
                                                        semantics,
                                                        right));
                                            case WellKnownMember.VertexPropertyLabel:
                                                return AddStep(new HasKeyStep(effectivePredicate));
                                        }
                                    }
                                    else if (leftMemberExpressionExpression is MemberExpression leftLeftMemberExpression)
                                    {
                                        // x => x.Name.Value == P.xy(...)
                                        if (memberSemantics == WellKnownMember.PropertyValue)
                                            leftMemberExpression = leftLeftMemberExpression;    //TODO: What else ?
                                    }
                                    else
                                        break;

                                    // x => x.Name == P.xy(...)
                                    if (rightConstantFragment.Value is StepLabel)
                                    {
                                        if (rightConstantFragment.Expression is MemberExpression memberExpression)
                                        {
                                            var ret = this
                                                .AddStep(new WherePredicateStep(effectivePredicate))
                                                .AddStep(new WherePredicateStep.ByMemberStep(GetKey(leftMemberExpression)));

                                            if (memberExpression.Member != leftMemberExpression.Member)
                                                ret = ret.AddStep(new WherePredicateStep.ByMemberStep(GetKey(memberExpression)));

                                            return ret;
                                        }

                                        return Has(
                                            leftMemberExpression,
                                            Continue(__ => __
                                                .AddStep(new WherePredicateStep(effectivePredicate))));
                                    }

                                    return Has(leftMemberExpression, effectivePredicate);
                                }
                                case ParameterExpression _:
                                {
                                    // x => x == P.xy(...)
                                    if (rightConstantFragment is StepLabelExpressionFragment stepLabelExpressionFragment)
                                    {
                                        var ret = AddStep(new WherePredicateStep(effectivePredicate));

                                        if (stepLabelExpressionFragment.Expression is MemberExpression memberExpression)
                                            ret = ret.AddStep(new WherePredicateStep.ByMemberStep(GetKey(memberExpression)));

                                        return ret;
                                    }

                                    return Is(effectivePredicate);
                                }
                                case MethodCallExpression methodCallExpression:
                                {
                                    var targetExpression = methodCallExpression.Object.Strip();

                                    if (targetExpression != null && typeof(IDictionary<string, object>).IsAssignableFrom(targetExpression.Type) && methodCallExpression.Method.Name == "get_Item")
                                    {
                                        return Has((string)methodCallExpression.Arguments[0].Strip().GetValue(Environment.Model), effectivePredicate);
                                    }

                                    break;
                                }
                            }

                            break;
                        }
                        case ConstantExpressionFragment leftConstantFragment when leftConstantFragment.Value is StepLabel leftStepLabel && rightConstantFragment.Value is StepLabel:
                        {
                            var ret = AddStep(new WhereStepLabelAndPredicateStep(leftStepLabel, effectivePredicate));

                            //TODO: What if x < x.Value.Prop ? i.e only one by operator?
                            if (leftConstantFragment.Expression is MemberExpression leftStepValueExpression)
                                ret = ret.AddStep(new WherePredicateStep.ByMemberStep(GetKey(leftStepValueExpression)));

                            if (rightConstantFragment.Expression is MemberExpression rightStepValueExpression)
                                ret = ret.AddStep(new WherePredicateStep.ByMemberStep(GetKey(rightStepValueExpression)));

                            return ret;
                        }
                    }

                    break;
                }
                case ParameterExpressionFragment rightParameterFragment:
                {
                    if (left is ParameterExpressionFragment leftParameterFragment)
                    {
                        if (leftParameterFragment.Expression is MemberExpression && rightParameterFragment.Expression is MemberExpression rightMember)
                        {
                            return As<StepLabel<TElement>, GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>>(
                                (_, label) => _
                                    .Where(
                                        leftParameterFragment,
                                        semantics,
                                        new StepLabelExpressionFragment(label, rightMember)));
                        }
                    }

                    break;
                }
            }

            throw new ExpressionNotSupportedException();
        }
    }
}
