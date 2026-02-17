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
        IEdgeGremlinQuery<T1, T2, T3>,

        IInOrOutEdgeGremlinQuery<T1, T2>,
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

        IEdgeGremlinQuery<T1, TNewOutVertex, T3> IInEdgeGremlinQueryBase<T1, T3>.From<TNewOutVertex>(Func<IVertexGremlinQuery<T3>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal) => From(fromVertexTraversal);

        IVertexGremlinQuery<T3> IInEdgeGremlinQueryBase<T1, T3>.InV() => InOutV<object, GremlinQuery<T3, object, object, IGremlinQueryBase>>(InVStep.Instance);

        IEdgeGremlinQuery<T1> IInEdgeGremlinQueryBase<T1, T3>.Lower() => this;

        IEdgeGremlinQuery<T1> IOutEdgeGremlinQueryBase<T1, T2>.Lower() => this;

        IVertexGremlinQuery<T2> IOutEdgeGremlinQueryBase<T1, T2>.OutV() => InOutV<object, GremlinQuery<T2, object, object, IGremlinQueryBase>>(OutVStep.Instance);

        IEdgeGremlinQuery<T1, T2, TNewInVertex> IOutEdgeGremlinQueryBase<T1, T2>.To<TNewInVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal) => To(toVertexTraversal);

        IEdgeGremlinQuery<object> IInEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IEdgeGremlinQuery<object> IOutEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1, T2, T3>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeOrVertexGremlinQuery<object>>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.BothV() => BothV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.BothV<TVertex>() => BothV<TVertex>();

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IEdgeGremlinQueryBase<T1>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => From<T1, TNewOutVertex, T3>(stepLabel);

        IEdgeOrVertexGremlinQuery<T1> IEdgeGremlinQueryBase<T1>.Lower() => this;

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IEdgeGremlinQueryBase<T1>.From<TNewOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal) => From<TNewOutVertex, object>(fromVertexTraversal);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.InV() => InOutV<object, GremlinQuery<object, object, object, IGremlinQueryBase>>(InVStep.Instance);

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.InV<TVertex>() => InOutV<TVertex, GremlinQuery<TVertex, object, object, IGremlinQueryBase>>(InVStep.Instance);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OtherV() => OtherV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OtherV<TVertex>() => OtherV<TVertex>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OutV() => InOutV<object, GremlinQuery<object, object, object, IGremlinQueryBase>>(OutVStep.Instance);

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OutV<TVertex>() => InOutV<TVertex, GremlinQuery<TVertex, object, object, IGremlinQueryBase>>(OutVStep.Instance);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, TValue>>> projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, Property<TValue>>>> projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<T1>.Properties(params Expression<Func<T1, Property<object>>>[] projections) => Properties<Property<object>, object, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<T1>.Properties(params ReadOnlySpan<Expression<Func<T1, Property<object>>>> projections) => Properties<Property<object>, object, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase.Properties() => Properties<Property<object>, object, object>(Projection.Property, Array.Empty<string>());

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase.Properties<TValue>() => Properties<Property<TValue>, object, object>(Projection.Property, Array.Empty<string>());

        IInEdgeGremlinQuery<T1, TNewInVertex> IEdgeGremlinQueryBase<T1>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<T1, object, TNewInVertex>(stepLabel);

        IInEdgeGremlinQuery<T1, TNewInVertex> IEdgeGremlinQueryBase<T1>.To<TNewInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal) => To<object, TNewInVertex>(toVertexTraversal);

        IGremlinQuery<TValue> IEdgeGremlinQueryBase<T1>.Values<TValue>(params Expression<Func<T1, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TValue> IEdgeGremlinQueryBase<T1>.Values<TValue>(params ReadOnlySpan<Expression<Func<T1, Property<TValue>>>> projections) => ValuesForProjections<TValue>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<object> IEdgeGremlinQueryBase<T1>.Values(params Expression<Func<T1, Property<object>>>[] projections) => ValuesForProjections<object>(projections);

        IGremlinQuery<object> IEdgeGremlinQueryBase<T1>.Values(params ReadOnlySpan<Expression<Func<T1, Property<object>>>> projections) => ValuesForProjections<object>(projections.Cast().To<LambdaExpression>());

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IEdgeGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

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

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase<T1>.ValueMap<TTarget>(params Expression<Func<T1, TTarget>>[] keys) => ValueMapForExpressions<IDictionary<string, TTarget>>(keys);

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

        IGremlinQuery<TTree> IGremlinQueryBase.Tree<TTree>(Func<ITreeBuilder, ITreeBuilderResult<TTree>> continuation) => Tree(continuation);

        IGremlinQuery<string> IGremlinQueryBase.Profile() => Profile();

        TQuery IGremlinQueryBase.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label) => Select<TQuery>(label);

        IArrayGremlinQuery<TNewElement, TNewScalar, TQuery> IGremlinQueryBase.Cap<TNewElement, TNewScalar, TQuery>(StepLabel<IArrayGremlinQuery<TNewElement, TNewScalar, TQuery>, TNewElement> label) => Cap(label);

        IGremlinQuery<TLabelledElement> IGremlinQueryBase.Select<TLabelledElement>(StepLabel<TLabelledElement> label) => Select(label);

        IGremlinQuery<T1> IGremlinQueryBase<T1>.Lower() => this;

        IGremlinQuery<object> IGremlinQueryBase.Lower() => CloneAs<IGremlinQuery<object>>();

        IGremlinQuery<object> IGremlinQueryBase.Drop() => Drop();

        IGremlinQuery<object> IGremlinQueryBase.Fail(string? message) => Fail(message);

        TTargetQuery IGremlinQueryAdmin.ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> transformation, Func<Projection, Projection>? maybeProjectionTransformation) => ConfigureSteps<TTargetQuery>(transformation, maybeProjectionTransformation);

        TTargetQuery IGremlinQueryAdmin.AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? maybeProjectionTransformation) => AddStep<TTargetQuery>(step, maybeProjectionTransformation);

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => CloneAs<TTargetQuery>();

        TTargetQuery IGremlinQueryAdmin.ConfigureMetadata<TTargetQuery>(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) => ConfigureMetadata<TTargetQuery>(metadataTransformation);

        IGremlinQuerySource IGremlinQueryAdmin.GetSource() => this
            .Continue()
            .Build(static builder => builder
                .WithSteps(static _ => Traversal.Empty)
                .BuildAs<IGremlinQuerySource>());

        Traversal IGremlinQueryAdmin.Steps => Steps;

        IGremlinQueryEnvironment IGremlinQueryAdmin.Environment => Environment;

        IImmutableDictionary<object, object?> IGremlinQueryAdmin.Metadata => Metadata;

        IGremlinQueryAdmin IStartGremlinQuery.AsAdmin() => this;

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge) => AddE(edge);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQuery<TNewElement> IStartGremlinQuery.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TNewElement> IStartGremlinQuery.Inject<TNewElement>(params ReadOnlySpan<TNewElement> elements) => Inject(elements);

        IVertexGremlinQuery<TNewVertex> IStartGremlinQuery.ReplaceV<TNewVertex>(TNewVertex vertex) => ((IStartGremlinQuery)this).V<TNewVertex>(vertex!.GetId(Environment)).Update(vertex);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>() => AddV(new TVertex());

        IVertexGremlinQuery<object> IStartGremlinQuery.V(object id) => V<object>([id]);

        IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids) => V<object>(ids);

        IVertexGremlinQuery<object> IStartGremlinQuery.V(params ReadOnlySpan<object> ids) => V<object>(ids);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(object id) => V<TVertex>([id]);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params object[] ids) => V<TVertex>(ids);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params ReadOnlySpan<object> ids) => V<TVertex>(ids);

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(object id) => E<object>([id]);

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(params object[] ids) => E<object>(ids);

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(params ReadOnlySpan<object> ids) => E<object>(ids);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(object id) => E<TEdge>([id]);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(params object[] ids) => E<TEdge>(ids);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(params ReadOnlySpan<object> ids) => E<TEdge>(ids);

        IEdgeGremlinQuery<TNewEdge> IStartGremlinQuery.ReplaceE<TNewEdge>(TNewEdge edge) => ((IGremlinQuerySource)this).E<TNewEdge>(edge!.GetId(Environment)).Update(edge);

        IGremlinQuerySource IGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation) => new GremlinQuery<T1, T2, T3, T4>(transformation(Environment), Steps, LabelProjections, Metadata);

        IGremlinQuerySource IGremlinQuerySource.WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => WithSideEffect(label, value);

        TQuery IGremlinQuerySource.WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation) => WithSideEffect(value, continuation);

        IEdgeGremlinQuery<T1> IInOrOutEdgeGremlinQueryBase<T1, T2>.Lower() => this;

        IEdgeGremlinQuery<T1, TTargetVertex, T2> IInOrOutEdgeGremlinQueryBase<T1, T2>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => From<T1, TTargetVertex, T2>(stepLabel);

        IEdgeGremlinQuery<T1, TNewOutVertex, T2> IInOrOutEdgeGremlinQueryBase<T1, T2>.From<TNewOutVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal) => From(fromVertexTraversal);

        IEdgeGremlinQuery<T1, T2, TTargetVertex> IInOrOutEdgeGremlinQueryBase<T1, T2>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<T1, T2, TTargetVertex>(stepLabel);

        IEdgeGremlinQuery<T1, T2, TTargetVertex> IInOrOutEdgeGremlinQueryBase<T1, T2>.To<TTargetVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal) => To(toVertexTraversal);

        IEdgeGremlinQuery<object> IInOrOutEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

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

        IGremlinQuery<TTargetValue> IMapGremlinQueryBase<T1>.Select<TTargetValue>(Expression<Func<T1, TTargetValue>> projection) => Select<IGremlinQuery<TTargetValue>>(projection);

        IEdgeOrVertexGremlinQuery<T1> IVertexGremlinQueryBase<T1>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IVertexGremlinQueryBase.Lower() => CloneAs<IEdgeOrVertexGremlinQuery<object>>();

        IInOrOutEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IInOrOutEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both() => Both(null);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge>() => Both(TypeArrayCache<TEdge>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2>() => Both(TypeArrayCache<TEdge1, TEdge2>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => Both(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE() => BothE<object>(null);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.BothE<TEdge>() => BothE<TEdge>(TypeArrayCache<TEdge>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => BothE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In() => In(null);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge>() => In(TypeArrayCache<TEdge>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2>() => In(TypeArrayCache<TEdge1, TEdge2>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => In(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE() => InE<object>(null);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.InE<TEdge>() => InE<TEdge>(TypeArrayCache<TEdge>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2>() => InE<object>(TypeArrayCache<TEdge1, TEdge2>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE() => InE<object>(null);

        IInEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.InE<TEdge>() => InE<TEdge>(TypeArrayCache<TEdge>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2>() => InE<object>(TypeArrayCache<TEdge1, TEdge2>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IInEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => InE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out() => Out(null);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge>() => Out(TypeArrayCache<TEdge>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2>() => Out(TypeArrayCache<TEdge1, TEdge2>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => Out(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);


        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE() => OutE<object>(null);

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.OutE<TEdge>() => OutE<TEdge>(TypeArrayCache<TEdge>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE() => OutE<object>(null);

        IOutEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge>() => OutE<TEdge>(TypeArrayCache<TEdge>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>.Types);

        IOutEdgeGremlinQuery<object, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>() => OutE<object>(TypeArrayCache<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>.Types);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties() => VertexProperties<object>([]);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, TValue>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, TValue[]>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue>>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>[] projections) => VertexProperties<TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>> projections) => VertexProperties<TValue, TNewMeta>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue>[]>>> projections) => VertexProperties<TValue>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>[]>>[] projections) => VertexProperties<TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue, TNewMeta>[]>>> projections) => VertexProperties<TValue, TNewMeta>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>() => VertexProperties<TValue>([]);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties(params Expression<Func<T1, VertexProperty<object>>>[] projections) => VertexProperties<object>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties(params ReadOnlySpan<Expression<Func<T1, VertexProperty<object>>>> projections) => VertexProperties<object>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue, TNewMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>> projections) => ValuesForProjections<TValue>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue>(params Expression<Func<T1, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TValue>>>> projections) => ValuesForProjections<TValue>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TTarget>[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget, TTargetMeta>(params Expression<Func<T1, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget, TTargetMeta>(params ReadOnlySpan<Expression<Func<T1, VertexProperty<TTarget, TTargetMeta>[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<object> IVertexGremlinQueryBase<T1>.Values(params Expression<Func<T1, VertexProperty<object>>>[] projections) => ValuesForProjections<object>(projections);

        IGremlinQuery<object> IVertexGremlinQueryBase<T1>.Values(params ReadOnlySpan<Expression<Func<T1, VertexProperty<object>>>> projections) => ValuesForProjections<object>(projections.Cast().To<LambdaExpression>());

        IVertexGremlinQuery<T1> IVertexGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IVertexGremlinQuery<T1> IVertexGremlinQuery<T1>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue[]>> projection, TProjectedValue value) => Property(projection, value != null ? new[] { value } : null);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T1, TTarget[]>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IElementGremlinQuery<T1> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Lower() => this;

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Properties<TValue>(params Expression<Func<T3, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Properties<TValue>(params ReadOnlySpan<Expression<Func<T3, TValue>>> projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<T1, T2, T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Property<TValue>(Expression<Func<T3, TValue>> projection, TValue value) => Property(projection, value);

        IGremlinQuery<T2> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Value() => Value<T2>();

        IGremlinQuery<T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.ValueMap() => ValueMap<T3>([]);

        IGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Values<TTarget>(params Expression<Func<T3, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Values<TTarget>(params ReadOnlySpan<Expression<Func<T3, TTarget>>> projections) => ValuesForProjections<TTarget>(projections.Cast().To<LambdaExpression>());

        IVertexPropertyGremlinQuery<T1, T2, T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Where(Expression<Func<VertexProperty<T2, T3>, bool>> predicate) => Where(predicate);

        IElementGremlinQuery<T1> IVertexPropertyGremlinQueryBase<T1, T2>.Lower() => this;

        IElementGremlinQuery<object> IVertexPropertyGremlinQueryBase.Lower() => CloneAs<IElementGremlinQuery<object>>();

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>([]);

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params ReadOnlySpan<string> keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IMapGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params string[] keys) => ValueMap<IDictionary<string, object>>(keys);

        IMapGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params ReadOnlySpan<string> keys) => ValueMap<IDictionary<string, object>>(keys);

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>() => ValuesForStringKeys<TValue>([]);

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params string[] keys) => ValuesForStringKeys<TValue>(keys);

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params ReadOnlySpan<string> keys) => ValuesForStringKeys<TValue>(keys);

        IGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params string[] keys) => ValuesForStringKeys<object>(keys);

        IGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params ReadOnlySpan<string> keys) => ValuesForStringKeys<object>(keys);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params string[] keys) => Properties<Property<object>, object, object>(Projection.Property, keys);

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params ReadOnlySpan<string> keys) => Properties<Property<object>, object, object>(Projection.Property, keys);

        IVertexPropertyGremlinQuery<VertexProperty<T2, TNewMeta>, T2, TNewMeta> IVertexPropertyGremlinQueryBase<T1, T2>.Meta<TNewMeta>() => CloneAs<IVertexPropertyGremlinQuery<VertexProperty<T2, TNewMeta>, T2, TNewMeta>>();

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, object, object>(Projection.Property, keys);

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2>.Properties<TValue>(params ReadOnlySpan<string> keys) => Properties<Property<TValue>, object, object>(Projection.Property, keys);

        IGremlinQuery<T2> IVertexPropertyGremlinQueryBase<T1, T2>.Value() => Value<T2>();

        IInEdgeGremlinQuery<T1, T3> IEdgeGremlinQuery<T1, T2, T3>.AsInEdge() => this;

        IOutEdgeGremlinQuery<T1, T2> IEdgeGremlinQuery<T1, T2, T3>.AsOutEdge() => this;

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.LimitLocal(long count) => LimitLocal(count);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.RangeLocal(long low, long high) => RangeLocal(low, high);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.SkipLocal(long count) => Skip(count, Scope.Local);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.TailLocal(long count) => TailLocal(count);

        IStringGremlinQuery<string> IGremlinQueryBase.AsString() => AsString();

        IDateGremlinQuery<DateTimeOffset> IGremlinQueryBase.AsDate() => AsDate();

        IDateGremlinQuery<T1> IDateGremlinQuery<T1>.Add(TimeSpan duration) => DateAdd(duration);

        IGremlinQuery<long> IDateGremlinQuery<T1>.Diff(DateTimeOffset other) => DateDiff(other);

        IGremlinQuery<long> IDateGremlinQuery<T1>.Diff(Func<IDateGremlinQuery<T1>, IGremlinQueryBase<DateTimeOffset>> other) => DateDiff(other);

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params string[] strings) => Concat(strings);

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params ReadOnlySpan<string> strings) => Concat(strings);

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params Func<IStringGremlinQuery<T1>, IGremlinQueryBase<T1>>[] stringTraversals) => Concat(stringTraversals);

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Concat(params ReadOnlySpan<Func<IStringGremlinQuery<T1>, IGremlinQueryBase<T1>>> stringTraversals) => Concat(stringTraversals.Cast().To<Func<GremlinQuery<T1, T2, T3, T4>, IGremlinQueryBase<T1>>>());

        IStringGremlinQuery<T1> IStringGremlinQuery<T1>.Replace(string oldValue, string newValue) => Replace(oldValue, newValue);

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

        IStringGremlinQuery<string> IGremlinQueryBase<T1>.Format(Expression<Func<T1, string>> stringInterpolationExpression) => Format(stringInterpolationExpression);
    }
}
