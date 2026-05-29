// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion
using System.Collections.Immutable;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ExRam.Gremlinq.Core.Execution;
using ExRam.Gremlinq.Core.GraphElements;
using ExRam.Gremlinq.Core.Projections;
using ExRam.Gremlinq.Core.Steps;

using Gremlin.Net.Process.Traversal;

using Path = ExRam.Gremlinq.Core.GraphElements.Path;

namespace ExRam.Gremlinq.Core
{
    internal partial class GremlinQuery<T1, T2, T3, T4> :
        IGremlinQueryAdmin,
        IGremlinQuerySource,

        IAsyncEnumerable<T1>,
        IGremlinQuery<T1>,

        IElementGremlinQuery<T1>,

        IVertexGremlinQuery<T1>,

        IEdgeOrVertexGremlinQuery<T1>,
        IEdgeGremlinQuery<T1>,
        IEdgeGremlinQuery<T1, T2>,
        IEdgeGremlinQuery<T1, T2, T3>,

        IAddEdgeGremlinQuery<T1, T2>,
        IInEdgeGremlinQuery<T1, T3>,
        IOutEdgeGremlinQuery<T1, T2>,

        IPropertyGremlinQuery<T1>,

        IStringGremlinQuery<T1>,
        IDateGremlinQuery<T1>,

        IVertexPropertyGremlinQuery<T1, T2>,
        IVertexPropertyGremlinQuery<T1, T2, T3>,

        IMapGremlinQuery<T1>,

        IArrayGremlinQuery<T1, T2, T4> where T4 : IGremlinQueryBase
    {
        T4 IArrayGremlinQueryBase<T1, T2, T4>.Unfold() => Unfold<T4>();

        IGremlinQuery<T1> IArrayGremlinQueryBase<T1, T2>.Lower() => this;

        IGremlinQuery<object> IArrayGremlinQueryBase.Unfold() => Unfold<IGremlinQuery<object>>();

        IGremlinQuery<T2[]> IArrayGremlinQueryBase<T2>.Lower() => CloneAs<IGremlinQuery<T2[]>>();

        IGremlinQuery<object[]> IArrayGremlinQueryBase.Lower() => CloneAs<IGremlinQuery<object[]>>();

        IGremlinQuery<T2> IArrayGremlinQueryBase<T2>.Unfold() => Unfold<IGremlinQuery<T2>>();

        IEdgeGremlinQuery<T1> IEdgeGremlinQuery<T1, T2, T3>.Lower() => this;

        IEdgeGremlinQuery<T1, TNewOutVertex, T3> IInEdgeGremlinQueryBase<T1, T3>.From<TNewOutVertex>(Func<IVertexGremlinQuery<T3>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal)
        {
            ArgumentNullException.ThrowIfNull(fromVertexTraversal);

            return From(fromVertexTraversal);
        }

        IVertexGremlinQuery<T3> IInEdgeGremlinQueryBase<T1, T3>.InV() => InOutV<object, GremlinQuery<T3, object, object, IGremlinQueryBase>>(InVStep.Instance);

        IEdgeGremlinQuery<T1> IInEdgeGremlinQueryBase<T1, T3>.Lower() => this;

        IEdgeGremlinQuery<T1> IOutEdgeGremlinQueryBase<T1, T2>.Lower() => this;

        IVertexGremlinQuery<T2> IOutEdgeGremlinQueryBase<T1, T2>.OutV() => InOutV<object, GremlinQuery<T2, object, object, IGremlinQueryBase>>(OutVStep.Instance);

        IEdgeGremlinQuery<T1, T2, TNewInVertex> IOutEdgeGremlinQueryBase<T1, T2>.To<TNewInVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal)
        {
            ArgumentNullException.ThrowIfNull(toVertexTraversal);

            return To(toVertexTraversal);
        }

        IEdgeGremlinQuery<object> IInEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IEdgeGremlinQuery<object> IOutEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1, T2, T3>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeOrVertexGremlinQuery<object>>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.BothV() => BothV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.BothV<TVertex>() => BothV<TVertex>();

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IEdgeGremlinQueryBase<T1>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            return From<T1, TNewOutVertex, T3>(stepLabel);
        }

        IEdgeOrVertexGremlinQuery<T1> IEdgeGremlinQueryBase<T1>.Lower() => this;

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IEdgeGremlinQueryBase<T1>.From<TNewOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal)
        {
            ArgumentNullException.ThrowIfNull(fromVertexTraversal);

            return From<TNewOutVertex, object>(fromVertexTraversal);
        }

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.InV() => InOutV<object, GremlinQuery<object, object, object, IGremlinQueryBase>>(InVStep.Instance);

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.InV<TVertex>() => InOutV<TVertex, GremlinQuery<TVertex, object, object, IGremlinQueryBase>>(InVStep.Instance);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OtherV() => OtherV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OtherV<TVertex>() => OtherV<TVertex>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OutV() => InOutV<object, GremlinQuery<object, object, object, IGremlinQueryBase>>(OutVStep.Instance);

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OutV<TVertex>() => InOutV<TVertex, GremlinQuery<TVertex, object, object, IGremlinQueryBase>>(OutVStep.Instance);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return Properties<Property<TValue>, TValue, object>(Projection.Property, projections);
        }

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, TValue>>> projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, Property<TValue>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return Properties<Property<TValue>, TValue, object>(Projection.Property, projections);
        }

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, Property<TValue>>>> projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<T1>.Properties(params Expression<Func<T1, Property<object>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return Properties<Property<object>, object, object>(Projection.Property, projections);
        }

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<T1>.Properties(params ReadOnlySpan<Expression<Func<T1, Property<object>>>> projections) => Properties<Property<object>, object, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase.Properties() => Properties<Property<object>, object, object>(Projection.Property, Array.Empty<string>());

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase.Properties<TValue>() => Properties<Property<TValue>, object, object>(Projection.Property, Array.Empty<string>());

        IInEdgeGremlinQuery<T1, TNewInVertex> IEdgeGremlinQueryBase<T1>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            return To<T1, object, TNewInVertex>(stepLabel);
        }

        IInEdgeGremlinQuery<T1, TNewInVertex> IEdgeGremlinQueryBase<T1>.To<TNewInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal)
        {
            ArgumentNullException.ThrowIfNull(toVertexTraversal);

            return To<object, TNewInVertex>(toVertexTraversal);
        }

        IGremlinQuery<TValue> IEdgeGremlinQueryBase<T1>.Values<TValue>(params Expression<Func<T1, Property<TValue>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TValue>(projections);
        }

        IGremlinQuery<TValue> IEdgeGremlinQueryBase<T1>.Values<TValue>(params ReadOnlySpan<Expression<Func<T1, Property<TValue>>>> projections) => ValuesForProjections<TValue>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<object> IEdgeGremlinQueryBase<T1>.Values(params Expression<Func<T1, Property<object>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<object>(projections);
        }

        IGremlinQuery<object> IEdgeGremlinQueryBase<T1>.Values(params ReadOnlySpan<Expression<Func<T1, Property<object>>>> projections) => ValuesForProjections<object>(projections.Cast().To<LambdaExpression>());

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IEdgeGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IEdgeGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IElementGremlinQuery<T1> IEdgeOrVertexGremlinQueryBase<T1>.Lower() => this;

        IElementGremlinQuery<object> IEdgeOrVertexGremlinQueryBase.Lower() => CloneAs<IElementGremlinQuery<object>>();

        IGremlinQuery<object> IElementGremlinQueryBase.Id() => Id();

        IGremlinQuery<string> IElementGremlinQueryBase.Label() => Label();

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>([]);

        IMapGremlinQuery<IDictionary<string, object>> IElementGremlinQueryBase.ValueMap() => ValueMap<IDictionary<string, object>>([]);

        IGremlinQuery<TValue> IElementGremlinQueryBase.Values<TValue>() => ValuesForStringKeys<TValue>([]);

        IGremlinQuery<object> IElementGremlinQueryBase.Values() => ValuesForStringKeys<object>([]);

        IElementGremlinQuery<T1> IElementGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase<T1>.ValueMap<TTarget>(params Expression<Func<T1, TTarget>>[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return ValueMapForExpressions<IDictionary<string, TTarget>>(keys);
        }

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase<T1>.ValueMap<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget>>> keys) => ValueMapForExpressions<IDictionary<string, TTarget>>(keys.Cast().To<LambdaExpression>());

        IMapGremlinQuery<T1> IGremlinQueryBase<T1>.ForceValueTuple() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1>());

        IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>> IGremlinQueryBase<T1>.ForceArray() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Value.Fold())
                .BuildAs<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>>());

        IGremlinQuery<TValue> IGremlinQueryBase.Constant<TValue>(TValue constant) => Constant(constant);

        string IGremlinQueryBase.Debug() => Debug();

        IGremlinQuery<long> IGremlinQueryBase.Count() => CountGlobal();

        IGremlinQuery<long> IGremlinQueryBase.CountLocal() => CountLocal();

        IGremlinQuery<string> IGremlinQueryBase.Explain() => Explain();

        IMapGremlinQuery<IDictionary<T1, T1[]>> IGremlinQueryBase<T1>.Group() => Group(static _ => _.ByKey(static __ => __));

        TaskAwaiter IGremlinQueryBase.GetAwaiter() => ((Task)this
            .LastOrDefaultAsync()
            .AsTask()).GetAwaiter();

        IGremlinQuery<T1> IGremlinQueryBase<T1>.ForceBase() => CloneAs<IGremlinQuery<T1>>();

        IElementGremlinQuery<T1> IGremlinQueryBase<T1>.ForceElement() => ForceElement();

        IVertexGremlinQuery<T1> IGremlinQueryBase<T1>.ForceVertex() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Vertex)
                .BuildAuto<T1>());

        IVertexPropertyGremlinQuery<T1, TNewValue> IGremlinQueryBase<T1>.ForceVertexProperty<TNewValue>() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Element)
                .BuildAs<IVertexPropertyGremlinQuery<T1, TNewValue>>());

        IVertexPropertyGremlinQuery<T1, TNewValue, TNewMeta> IGremlinQueryBase<T1>.ForceVertexProperty<TNewValue, TNewMeta>() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Element)
                .BuildAs<IVertexPropertyGremlinQuery<T1, TNewValue, TNewMeta>>());

        IPropertyGremlinQuery<T1> IGremlinQueryBase<T1>.ForceProperty() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1>());

        IEdgeGremlinQuery<T1> IGremlinQueryBase<T1>.ForceEdge() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Edge)
                .BuildAuto<T1>());

        IInEdgeGremlinQuery<T1, TNewInVertex> IGremlinQueryBase<T1>.ForceInEdge<TNewInVertex>() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Edge)
                .BuildAs<IInEdgeGremlinQuery<T1, TNewInVertex>>());

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IGremlinQueryBase<T1>.ForceOutEdge<TNewOutVertex>() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Edge)
                .BuildAs<IOutEdgeGremlinQuery<T1, TNewOutVertex>>());

        IEdgeGremlinQuery<T1, TNewOutVertex, TNewInVertex> IGremlinQueryBase<T1>.ForceEdge<TNewOutVertex, TNewInVertex>() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Edge)
                .BuildAs<IEdgeGremlinQuery<T1, TNewOutVertex, TNewInVertex>>());

        IGremlinQuery<T1> IGremlinQueryBase<T1>.ForceValue() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(Projection.Value)
                .BuildAuto<T1>());

        TaskAwaiter<T1[]> IGremlinQueryBase<T1>.GetAwaiter() => (this as IAsyncEnumerable<T1>)
            .ToArrayAsync()
            .AsTask()
            .GetAwaiter();

        IAsyncEnumerator<T1> IAsyncEnumerable<T1>.GetAsyncEnumerator(CancellationToken ct) => Environment.Executor
            .Execute<T1>(GremlinQueryExecutionContext.Create(this))
            .GetAsyncEnumerator(ct);

        IAsyncEnumerable<T1> IGremlinQueryBase<T1>.ToAsyncEnumerable() => this;

        IGremlinQuery<Path> IGremlinQueryBase.Path() => Path();

        IGremlinQuery<Tree<object>> IGremlinQueryBase.Tree() => Tree<object>();

        IGremlinQuery<Tree<TRoot>> IGremlinQueryBase.Tree<TRoot>() => Tree<TRoot>();

        IGremlinQuery<TTree> IGremlinQueryBase.Tree<TTree>(Func<ITreeBuilder, ITreeBuilderResult<TTree>> continuation)
        {
            ArgumentNullException.ThrowIfNull(continuation);

            return Tree(continuation);
        }

        IGremlinQuery<string> IGremlinQueryBase.Profile() => Profile();

        TQuery IGremlinQueryBase.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label)
        {
            ArgumentNullException.ThrowIfNull(label);

            return Select<TQuery>(label);
        }

        IArrayGremlinQuery<TNewElement, TNewScalar, TQuery> IGremlinQueryBase.Cap<TNewElement, TNewScalar, TQuery>(StepLabel<IArrayGremlinQuery<TNewElement, TNewScalar, TQuery>, TNewElement> label)
        {
            ArgumentNullException.ThrowIfNull(label);

            return Cap(label);
        }

        IGremlinQuery<TLabelledElement> IGremlinQueryBase.Select<TLabelledElement>(StepLabel<TLabelledElement> label)
        {
            ArgumentNullException.ThrowIfNull(label);

            return Select(label);
        }

        IGremlinQuery<T1> IGremlinQueryBase<T1>.Lower() => this;

        IGremlinQuery<object> IGremlinQueryBase.Lower() => CloneAs<IGremlinQuery<object>>();

        IGremlinQuery<object> IGremlinQueryBase.Drop() => Drop();

        IGremlinQuery<object> IGremlinQueryBase.Fail(string? message) => Fail(message);

        TTargetQuery IGremlinQueryAdmin.ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> transformation, Func<Projection, Projection>? maybeProjectionTransformation)
        {
            ArgumentNullException.ThrowIfNull(transformation);

            return ConfigureSteps<TTargetQuery>(transformation, maybeProjectionTransformation);
        }

        TTargetQuery IGremlinQueryAdmin.AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? maybeProjectionTransformation)
        {
            ArgumentNullException.ThrowIfNull(step);

            return AddStep<TTargetQuery>(step, maybeProjectionTransformation);
        }

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => CloneAs<TTargetQuery>();

        TTargetQuery IGremlinQueryAdmin.ConfigureMetadata<TTargetQuery>(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation)
        {
            ArgumentNullException.ThrowIfNull(metadataTransformation);

            return ConfigureMetadata<TTargetQuery>(metadataTransformation);
        }

        IGremlinQuerySource IGremlinQueryAdmin.GetSource() => this
            .Continue()
            .Build(static builder => builder
                .WithSteps(static _ => Traversal.Empty)
                .BuildAs<IGremlinQuerySource>());

        Traversal IGremlinQueryAdmin.Steps => Steps;

        IGremlinQueryEnvironment IGremlinQueryAdmin.Environment => Environment.InnerEnvironment;

        IImmutableDictionary<object, object?> IGremlinQueryAdmin.Metadata => Metadata;

        IGremlinQueryAdmin IStartGremlinQuery.AsAdmin() => this;

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge) => AddE(edge);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQuery<TNewElement> IStartGremlinQuery.Inject<TNewElement>(params TNewElement[] elements)
        {
            ArgumentNullException.ThrowIfNull(elements);

            return Inject(elements);
        }

        IGremlinQuery<TNewElement> IStartGremlinQuery.Inject<TNewElement>(params ReadOnlySpan<TNewElement> elements) => Inject(elements);

        IVertexGremlinQuery<TNewVertex> IStartGremlinQuery.ReplaceV<TNewVertex>(TNewVertex vertex) => ((IStartGremlinQuery)this).V<TNewVertex>(vertex!.GetId(Environment)).Update(vertex);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>() => AddV(new TVertex());

        IVertexGremlinQuery<object> IStartGremlinQuery.V(object id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return V<object>([id]);
        }

        IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids)
        {
            ArgumentNullException.ThrowIfNull(ids);

            return V<object>(ids);
        }

        IVertexGremlinQuery<object> IStartGremlinQuery.V(params ReadOnlySpan<object> ids) => V<object>(ids);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(object id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return V<TVertex>([id]);
        }

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params object[] ids)
        {
            ArgumentNullException.ThrowIfNull(ids);

            return V<TVertex>(ids);
        }

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params ReadOnlySpan<object> ids) => V<TVertex>(ids);

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(object id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return E<object>([id]);
        }

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(params object[] ids)
        {
            ArgumentNullException.ThrowIfNull(ids);

            return E<object>(ids);
        }

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(params ReadOnlySpan<object> ids) => E<object>(ids);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(object id)
        {
            ArgumentNullException.ThrowIfNull(id);

            return E<TEdge>([id]);
        }

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(params object[] ids)
        {
            ArgumentNullException.ThrowIfNull(ids);

            return E<TEdge>(ids);
        }

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(params ReadOnlySpan<object> ids) => E<TEdge>(ids);

        IEdgeGremlinQuery<TNewEdge> IStartGremlinQuery.ReplaceE<TNewEdge>(TNewEdge edge) => ((IGremlinQuerySource)this).E<TNewEdge>(edge!.GetId(Environment)).Update(edge);

        IGremlinQuerySource IGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation)
        {
            ArgumentNullException.ThrowIfNull(transformation);

            return new GremlinQuery<T1, T2, T3, T4>(transformation(Environment.InnerEnvironment), Steps, LabelProjections, Metadata);
        }

        IGremlinQuerySource IGremlinQuerySource.WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value)
        {
            ArgumentNullException.ThrowIfNull(label);

            return WithSideEffect(label, value);
        }

        TQuery IGremlinQuerySource.WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation)
        {
            ArgumentNullException.ThrowIfNull(continuation);

            return WithSideEffect(value, continuation);
        }

        IEdgeGremlinQuery<T1> IAddEdgeGremlinQueryBase<T1, T2>.Lower() => this;

        IEdgeGremlinQuery<T1, TTargetVertex, T2> IAddEdgeGremlinQueryBase<T1, T2>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            return From<T1, TTargetVertex, T2>(stepLabel);
        }

        IEdgeGremlinQuery<T1, TNewOutVertex, T2> IAddEdgeGremlinQueryBase<T1, T2>.From<TNewOutVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal)
        {
            ArgumentNullException.ThrowIfNull(fromVertexTraversal);

            return From(fromVertexTraversal);
        }

        IEdgeGremlinQuery<T1, T2, TTargetVertex> IAddEdgeGremlinQueryBase<T1, T2>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel)
        {
            ArgumentNullException.ThrowIfNull(stepLabel);

            return To<T1, T2, TTargetVertex>(stepLabel);
        }

        IEdgeGremlinQuery<T1, T2, TTargetVertex> IAddEdgeGremlinQueryBase<T1, T2>.To<TTargetVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal)
        {
            ArgumentNullException.ThrowIfNull(toVertexTraversal);

            return To(toVertexTraversal);
        }

        IEdgeGremlinQuery<object> IAddEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IGremlinQuery<string> IPropertyGremlinQueryBase<T1>.Key() => Key();

        IGremlinQuery<TValue> IPropertyGremlinQueryBase<T1>.Value<TValue>() => Value<TValue>();

        IGremlinQuery<object> IPropertyGremlinQueryBase<T1>.Value() => Value<object>();

        IGremlinQuery<T2> IArrayGremlinQueryBase<T2>.SumLocal() => SumLocal<IGremlinQuery<T2>>();

        IGremlinQuery<object> IArrayGremlinQueryBase.SumLocal() => SumLocal<IGremlinQuery<object>>();

        T4 IArrayGremlinQueryBase<T1, T2, T4>.SumLocal() => SumLocal<T4>();

        IGremlinQuery<T2> IArrayGremlinQueryBase<T2>.MinLocal() => MinLocal<IGremlinQuery<T2>>();

        IGremlinQuery<object> IArrayGremlinQueryBase.MinLocal() => MinLocal<IGremlinQuery<object>>();

        T4 IArrayGremlinQueryBase<T1, T2, T4>.MinLocal() => MinLocal<T4>();

        IGremlinQuery<T2> IArrayGremlinQueryBase<T2>.MaxLocal() => MaxLocal<IGremlinQuery<T2>>();

        IGremlinQuery<object> IArrayGremlinQueryBase.MaxLocal() => MaxLocal<IGremlinQuery<object>>();

        T4 IArrayGremlinQueryBase<T1, T2, T4>.MaxLocal() => MaxLocal<T4>();

        IGremlinQuery<T2> IArrayGremlinQueryBase<T2>.MeanLocal() => MeanLocal<IGremlinQuery<T2>>();

        IGremlinQuery<object> IArrayGremlinQueryBase.MeanLocal() => MeanLocal<IGremlinQuery<object>>();

        T4 IArrayGremlinQueryBase<T1, T2, T4>.MeanLocal() => MeanLocal<T4>();

        IGremlinQuery<TTargetValue> IMapGremlinQueryBase<T1>.Select<TTargetValue>(Expression<Func<T1, TTargetValue>> projection)
        {
            ArgumentNullException.ThrowIfNull(projection);

            return Select<IGremlinQuery<TTargetValue>>(projection);
        }

        IEdgeOrVertexGremlinQuery<T1> IVertexGremlinQueryBase<T1>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IVertexGremlinQueryBase.Lower() => CloneAs<IEdgeOrVertexGremlinQuery<object>>();

        IAddEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IAddEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.AddE<TEdge>() => AddE(new TEdge());

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties() => VertexProperties<object>([]);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<TValue>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, TValue>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<TValue>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, TValue[]>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, VertexProperty<TValue>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<TValue>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue>>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<TValue, TNewMeta>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>> projections) => VertexProperties<TValue, TNewMeta>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, VertexProperty<TValue>[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<TValue>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue>[]>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<TValue, TNewMeta>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue, TNewMeta>[]>>> projections) => VertexProperties<TValue, TNewMeta>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>() => VertexProperties<TValue>([]);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties(params Expression<Func<T1, VertexProperty<object>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return VertexProperties<object>(projections);
        }

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties(params ReadOnlySpan<Expression<Func<T1, VertexProperty<object>>>> projections) => VertexProperties<object>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TValue>(projections);
        }

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue, TNewMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>> projections) => ValuesForProjections<TValue>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue>(params Expression<Func<T1, VertexProperty<TValue>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TValue>(projections);
        }

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue>>>> projections) => ValuesForProjections<TValue>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, VertexProperty<TTarget>[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TTarget>[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget, TTargetMeta>(params Expression<Func<T1, VertexProperty<TTarget, TTargetMeta>[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget, TTargetMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TTarget, TTargetMeta>[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<object> IVertexGremlinQueryBase<T1>.Values(params Expression<Func<T1, VertexProperty<object>>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<object>(projections);
        }

        IGremlinQuery<object> IVertexGremlinQueryBase<T1>.Values(params ReadOnlySpan<Expression<Func<T1, VertexProperty<object>>>> projections) => ValuesForProjections<object>(projections.Cast().To<LambdaExpression>());

        IVertexGremlinQuery<T1> IVertexGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IVertexGremlinQuery<T1> IVertexGremlinQuery<T1>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue[]>> projection, TProjectedValue value)
        {
            ArgumentNullException.ThrowIfNull(projection);

            return Property(projection, value != null ? new[] { value } : null);
        }

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget[]>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IElementGremlinQuery<T1> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Lower() => this;

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Properties<TValue>(params Expression<Func<T3, TValue>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return Properties<Property<TValue>, TValue, object>(Projection.Property, projections);
        }

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T3, TValue>>> projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<T1, T2, T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Property<TValue>(Expression<Func<T3, TValue>> projection, TValue value)
        {
            ArgumentNullException.ThrowIfNull(projection);

            return Property(projection, value);
        }

        IGremlinQuery<T2> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Value() => Value<T2>();

        IGremlinQuery<T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.ValueMap() => ValueMap<T3>([]);

        IGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Values<TTarget>(params Expression<Func<T3, TTarget>>[] projections)
        {
            ArgumentNullException.ThrowIfNull(projections);

            return ValuesForProjections<TTarget>(projections);
        }

        IGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T3, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<T1, T2, T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Where(Expression<Func<VertexProperty<T2, T3>, bool>> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);

            return Where(predicate);
        }

        IElementGremlinQuery<T1> IVertexPropertyGremlinQueryBase<T1, T2>.Lower() => this;

        IElementGremlinQuery<object> IVertexPropertyGremlinQueryBase.Lower() => CloneAs<IElementGremlinQuery<object>>();

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>([]);

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return ValueMap<IDictionary<string, TTarget>>(keys);
        }

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params ReadOnlySpan<string> keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IMapGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return ValueMap<IDictionary<string, object>>(keys);
        }

        IMapGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params ReadOnlySpan<string> keys) => ValueMap<IDictionary<string, object>>(keys);

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>() => ValuesForStringKeys<TValue>([]);

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return ValuesForStringKeys<TValue>(keys);
        }

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params ReadOnlySpan<string> keys) => ValuesForStringKeys<TValue>(keys);

        IGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return ValuesForStringKeys<object>(keys);
        }

        IGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params ReadOnlySpan<string> keys) => ValuesForStringKeys<object>(keys);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return Properties<Property<object>, object, object>(Projection.Property, keys);
        }

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params ReadOnlySpan<string> keys) => Properties<Property<object>, object, object>(Projection.Property, keys);

        IVertexPropertyGremlinQuery<VertexProperty<T2, TNewMeta>, T2, TNewMeta> IVertexPropertyGremlinQueryBase<T1, T2>.Meta<TNewMeta>() => CloneAs<IVertexPropertyGremlinQuery<VertexProperty<T2, TNewMeta>, T2, TNewMeta>>();

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2>.Properties<TValue>(params string[] keys)
        {
            ArgumentNullException.ThrowIfNull(keys);

            return Properties<Property<TValue>, object, object>(Projection.Property, keys);
        }

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2>.Properties<TValue>(params ReadOnlySpan<string> keys) => Properties<Property<TValue>, object, object>(Projection.Property, keys);

        IGremlinQuery<T2> IVertexPropertyGremlinQueryBase<T1, T2>.Value() => Value<T2>();

        IInEdgeGremlinQuery<T1, T3> IEdgeGremlinQuery<T1, T2, T3>.AsInEdge() => this;

        IOutEdgeGremlinQuery<T1, T2> IEdgeGremlinQuery<T1, T2, T3>.AsOutEdge() => this;

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1, T2>.Lower() => this;

        IEdgeGremlinQuery<T1> IEdgeGremlinQuery<T1, T2>.Lower() => this;

        IInEdgeGremlinQuery<T1, T2> IEdgeGremlinQuery<T1, T2>.AsInEdge() => CloneAs<IInEdgeGremlinQuery<T1, T2>>();

        IOutEdgeGremlinQuery<T1, T2> IEdgeGremlinQuery<T1, T2>.AsOutEdge() => this;

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.LimitLocal(long count) => LimitLocal(count);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.RangeLocal(long low, long high) => RangeLocal(low, high);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.SkipLocal(long count) => Skip(count, Scope.Local);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.TailLocal(long count) => TailLocal(count);

        IStringGremlinQuery<string> IGremlinQueryBase.AsString() => AsString();

        IDateGremlinQuery<DateTimeOffset> IGremlinQueryBase.AsDate() => AsDate();

        IDateGremlinQuery<T1> IDateGremlinQuery<T1>.Add(TimeSpan duration) => DateAdd(duration);

        IGremlinQuery<long> IDateGremlinQuery<T1>.Diff(DateTimeOffset other) => DateDiff(other);

        IGremlinQuery<long> IDateGremlinQuery<T1>.Diff(Func<IDateGremlinQuery<T1>, IGremlinQueryBase<DateTimeOffset>> other)
        {
            ArgumentNullException.ThrowIfNull(other);

            return DateDiff(other);
        }

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params string[] strings)
        {
            ArgumentNullException.ThrowIfNull(strings);

            return Concat(strings);
        }

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params ReadOnlySpan<string> strings) => Concat(strings);

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params Func<IStringGremlinQuery<T1>, IGremlinQueryBase<T1>>[] stringTraversals)
        {
            ArgumentNullException.ThrowIfNull(stringTraversals);

            return Concat(stringTraversals);
        }

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params ReadOnlySpan<Func<IStringGremlinQuery<T1>, IGremlinQueryBase<T1>>> stringTraversals) => Concat(stringTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<T1>>>());

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Replace(string oldValue, string newValue)
        {
            ArgumentNullException.ThrowIfNull(oldValue);
            ArgumentNullException.ThrowIfNull(newValue);

            return Replace(oldValue, newValue);
        }

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Reverse() => Reverse();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Substring(int startIndex) => startIndex >= 0
            ? Substring(System.Range.StartAt(startIndex))
            : throw new ArgumentOutOfRangeException();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Substring(int startIndex, int length) => startIndex >= 0 && length >= 0
            ? Substring(new Range(startIndex, Index.FromStart(startIndex + length)))
            : throw new ArgumentOutOfRangeException();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Substring(Range range) => Substring(range);

        IGremlinQuery<int> IStringGremlinQuery<T1>.Length() => Length();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.ToLower() => ToLower();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.ToUpper() => ToUpper();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Trim() => Trim();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.TrimStart() => TrimStart();

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.TrimEnd() => TrimEnd();

        IStringGremlinQuery<string> IGremlinQueryBase<T1>.Format(Expression<Func<T1, string>> stringInterpolationExpression)
        {
            ArgumentNullException.ThrowIfNull(stringInterpolationExpression);

            return Format(stringInterpolationExpression);
        }
    }
}
