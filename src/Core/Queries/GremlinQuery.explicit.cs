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

        IVertexGremlinQuery<T3> IInEdgeGremlinQueryBase<T1, T3>.InV() => InV<T3>();

        IEdgeGremlinQuery<T1> IInEdgeGremlinQueryBase<T1, T3>.Lower() => this;

        IEdgeGremlinQuery<T1> IOutEdgeGremlinQueryBase<T1, T2>.Lower() => this;

        IVertexGremlinQuery<T2> IOutEdgeGremlinQueryBase<T1, T2>.OutV() => OutV<T2>();

        IEdgeGremlinQuery<T1, T2, TNewInVertex> IOutEdgeGremlinQueryBase<T1, T2>.To<TNewInVertex>(Func<IVertexGremlinQuery<T2>, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal) => To(toVertexTraversal);

        IEdgeGremlinQuery<object> IInEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IEdgeGremlinQuery<object> IOutEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeGremlinQuery<object>>();

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1, T2, T3>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IEdgeGremlinQueryBase.Lower() => CloneAs<IEdgeOrVertexGremlinQuery<object>>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.BothV() => BothV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.BothV<TVertex>() => ((IEdgeGremlinQueryBase)this)
            .BothV()
            .OfType<TVertex>();

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IEdgeGremlinQueryBase<T1>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => From<T1, TNewOutVertex, T3>(stepLabel);

        IEdgeOrVertexGremlinQuery<T1> IEdgeGremlinQueryBase<T1>.Lower() => this;

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IEdgeGremlinQueryBase<T1>.From<TNewOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal) => From<TNewOutVertex, object>(fromVertexTraversal);

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.InV() => InV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.InV<TVertex>() => ((IEdgeGremlinQueryBase)this)
            .InV()
            .OfType<TVertex>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OtherV() => OtherV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OtherV<TVertex>() => ((IEdgeGremlinQueryBase)this)
            .OtherV()
            .OfType<TVertex>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.OutV() => OutV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.OutV<TVertex>() => ((IEdgeGremlinQueryBase)this)
            .OutV()
            .OfType<TVertex>();

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<T1>.Properties(params Expression<Func<T1, Property<object>>>[] projections) => Properties<Property<object>, object, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase.Properties() => Properties<Property<object>, object, object>(Projection.Property, Array.Empty<string>());

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase.Properties<TValue>() => Properties<Property<TValue>, object, object>(Projection.Property, Array.Empty<string>());

        IInEdgeGremlinQuery<T1, TNewInVertex> IEdgeGremlinQueryBase<T1>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<T1, object, TNewInVertex>(stepLabel);

        IInEdgeGremlinQuery<T1, TNewInVertex> IEdgeGremlinQueryBase<T1>.To<TNewInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal) => To<object, TNewInVertex>(toVertexTraversal);

        IGremlinQuery<TValue> IEdgeGremlinQueryBase<T1>.Values<TValue>(params Expression<Func<T1, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<object> IEdgeGremlinQueryBase<T1>.Values(params Expression<Func<T1, Property<object>>>[] projections) => ValuesForProjections<object>(projections);

        IEdgeGremlinQuery<T1> IEdgeGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IEdgeGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IElementGremlinQuery<T1> IEdgeOrVertexGremlinQueryBase<T1>.Lower() => this;

        IElementGremlinQuery<object> IEdgeOrVertexGremlinQueryBase.Lower() => CloneAs<IElementGremlinQuery<object>>();

        IGremlinQuery<object> IElementGremlinQueryBase.Id() => Id();

        IGremlinQuery<string> IElementGremlinQueryBase.Label() => Label();

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>(ImmutableArray<string>.Empty);

        IMapGremlinQuery<IDictionary<string, object>> IElementGremlinQueryBase.ValueMap() => ValueMap<IDictionary<string, object>>(ImmutableArray<string>.Empty);

        IGremlinQuery<TValue> IElementGremlinQueryBase.Values<TValue>() => ValuesForKeys<TValue>(Array.Empty<Key>());

        IGremlinQuery<object> IElementGremlinQueryBase.Values() => ValuesForKeys<object>(Array.Empty<Key>());

        IElementGremlinQuery<T1> IElementGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase<T1>.ValueMap<TTarget>(params Expression<Func<T1, TTarget>>[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IMapGremlinQuery<T1> IGremlinQueryBase<T1>.ForceValueTuple() => CloneAs<IMapGremlinQuery<T1>>(maybeNewTraversal: Steps.WithProjection(Projection.Value));

        IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>> IGremlinQueryBase<T1>.ForceArray() => CloneAs<IArrayGremlinQuery<T1[], T1, IGremlinQuery<T1>>>(maybeNewTraversal: Steps.WithProjection(Projection.Value.Fold()));

        IGremlinQuery<TValue> IGremlinQueryBase.Constant<TValue>(TValue constant) => Constant(constant);

        string IGremlinQueryBase.Debug() => Debug();

        IGremlinQuery<long> IGremlinQueryBase.Count() => CountGlobal();

        IGremlinQuery<long> IGremlinQueryBase.CountLocal() => CountLocal();

        IGremlinQuery<string> IGremlinQueryBase.Explain() => Explain();

        IMapGremlinQuery<IDictionary<T1, T1[]>> IGremlinQueryBase<T1>.Group() => Group(static _ => _.ByKey(static __ => __));

        TaskAwaiter IGremlinQueryBase.GetAwaiter() => ((Task)((IGremlinQuery<T1>)this).ToAsyncEnumerable().LastOrDefaultAsync().AsTask()).GetAwaiter();

        IGremlinQuery<T1> IGremlinQueryBase<T1>.ForceBase() => CloneAs<IGremlinQuery<T1>>();

        IElementGremlinQuery<T1> IGremlinQueryBase<T1>.ForceElement() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(static _ => _.Highest(Projection.Element))
                .Build<IElementGremlinQuery<T1>>());

        IVertexGremlinQuery<T1> IGremlinQueryBase<T1>.ForceVertex() => CloneAs<IVertexGremlinQuery<T1>>(maybeNewTraversal: Steps.WithProjection(Projection.Vertex));

        IVertexPropertyGremlinQuery<T1, TNewValue> IGremlinQueryBase<T1>.ForceVertexProperty<TNewValue>() => CloneAs<IVertexPropertyGremlinQuery<T1, TNewValue>>(maybeNewTraversal: Steps.WithProjection(Projection.Element));

        IVertexPropertyGremlinQuery<T1, TNewValue, TNewMeta> IGremlinQueryBase<T1>.ForceVertexProperty<TNewValue, TNewMeta>() => CloneAs<IVertexPropertyGremlinQuery<T1, TNewValue, TNewMeta>>(maybeNewTraversal: Steps.WithProjection(Projection.Element));

        IPropertyGremlinQuery<T1> IGremlinQueryBase<T1>.ForceProperty() => CloneAs<IPropertyGremlinQuery<T1>>(maybeNewTraversal: Steps.WithProjection(Projection.Value));

        IEdgeGremlinQuery<T1> IGremlinQueryBase<T1>.ForceEdge() => CloneAs<IEdgeGremlinQuery<T1>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IInEdgeGremlinQuery<T1, TNewInVertex> IGremlinQueryBase<T1>.ForceInEdge<TNewInVertex>() => CloneAs<IInEdgeGremlinQuery<T1, TNewInVertex>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IOutEdgeGremlinQuery<T1, TNewOutVertex> IGremlinQueryBase<T1>.ForceOutEdge<TNewOutVertex>() => CloneAs<IOutEdgeGremlinQuery<T1, TNewOutVertex>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IEdgeGremlinQuery<T1, TNewOutVertex, TNewInVertex> IGremlinQueryBase<T1>.ForceEdge<TNewOutVertex, TNewInVertex>() => CloneAs<IEdgeGremlinQuery<T1, TNewOutVertex, TNewInVertex>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IGremlinQuery<T1> IGremlinQueryBase<T1>.ForceValue() => CloneAs<IGremlinQuery<T1>>(maybeNewTraversal: Steps.WithProjection(Projection.Value));

        GremlinQueryAwaiter<T1> IGremlinQueryBase<T1>.GetAwaiter() => new((this).ToArrayAsync().AsTask().GetAwaiter());

        IAsyncEnumerable<T1> IGremlinQueryBase<T1>.ToAsyncEnumerable() => Environment.Executor
            .Execute<T1>(GremlinQueryExecutionContext.Create(this));

        IGremlinQuery<Path> IGremlinQueryBase.Path() => Path();

        IGremlinQuery<string> IGremlinQueryBase.Profile() => Profile();

        TQuery IGremlinQueryBase.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label) => Select<TQuery>(label);

        IArrayGremlinQuery<TNewElement, TNewScalar, TQuery> IGremlinQueryBase.Cap<TNewElement, TNewScalar, TQuery>(StepLabel<IArrayGremlinQuery<TNewElement, TNewScalar, TQuery>, TNewElement> label) => Cap(label);

        IGremlinQuery<TLabelledElement> IGremlinQueryBase.Select<TLabelledElement>(StepLabel<TLabelledElement> label) => Select(label);

        IGremlinQuery<T1> IGremlinQueryBase<T1>.Lower() => this;

        IGremlinQuery<object> IGremlinQueryBase.Lower() => CloneAs<IGremlinQuery<object>>();

        IGremlinQuery<object> IGremlinQueryBase.Drop() => Drop();

        IGremlinQuery<object> IGremlinQueryBase.Fail(string? message) => Fail(message);

        TTargetQuery IGremlinQueryAdmin.ConfigureSteps<TTargetQuery>(Func<Traversal, Traversal> transformation, Func<Projection, Projection>? maybeProjectionTransformation) => this
            .Continue()
            .Build(
                static (builder, tuple) => builder
                    .WithSteps(
                        static (steps, transformation) => transformation(steps),
                        tuple.transformation)
                    .WithNewProjection(
                        static (projection, maybeProjectionTransformation) => maybeProjectionTransformation is { } projectionTransformation
                            ? projectionTransformation(projection)
                            : projection,
                        tuple.maybeProjectionTransformation)
                    .Build<TTargetQuery>(),
                (transformation, maybeProjectionTransformation));

        TTargetQuery IGremlinQueryAdmin.AddStep<TTargetQuery>(Step step, Func<Projection, Projection>? maybeProjectionTransformation) => this
            .Continue()
            .Build(
                static (builder, tuple) =>
                {
                    var (step, maybeProjectionTransformation) = tuple;

                    builder = builder
                        .AddStep(step);

                    if (maybeProjectionTransformation is { } projectionTransformation)
                        builder = builder.WithNewProjection(projectionTransformation);

                    return builder
                        .Build<TTargetQuery>();
                },
                (step, maybeProjectionTransformation));

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => CloneAs<TTargetQuery>();

        IGremlinQuerySource IGremlinQueryAdmin.GetSource() => CloneAs<IGremlinQuerySource>(maybeNewTraversal: Traversal.Empty);

        Traversal IGremlinQueryAdmin.Steps => Steps;

        IGremlinQueryEnvironment IGremlinQueryAdmin.Environment => Environment;

        IImmutableDictionary<object, object?> IGremlinQueryAdmin.Metadata => Metadata;

        IGremlinQueryAdmin IStartGremlinQuery.AsAdmin() => this;

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>(TEdge edge) => AddE(edge);

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        IGremlinQuery<TNewElement> IStartGremlinQuery.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IVertexGremlinQuery<TNewVertex> IStartGremlinQuery.ReplaceV<TNewVertex>(TNewVertex vertex) => ((IStartGremlinQuery)this).V<TNewVertex>(vertex!.GetId(Environment)).Update(vertex);

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.AddV<TVertex>() => AddV(new TVertex());

        IVertexGremlinQuery<object> IStartGremlinQuery.V(object id) => V(ImmutableArray.Create(id));

        IVertexGremlinQuery<object> IStartGremlinQuery.V(params object[] ids) => V(ids.ToImmutableArray());

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(object id) => ((IStartGremlinQuery)this).V(id).OfType<TVertex>();

        IVertexGremlinQuery<TVertex> IStartGremlinQuery.V<TVertex>(params object[] ids) => ((IStartGremlinQuery)this).V(ids).OfType<TVertex>();

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(object id) => E(ImmutableArray.Create(id));

        IEdgeGremlinQuery<object> IStartGremlinQuery.E(params object[] ids) => E(ids.ToImmutableArray());

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(object id) => ((IGremlinQuerySource)this).E(id).OfType<TEdge>();

        IEdgeGremlinQuery<TEdge> IStartGremlinQuery.E<TEdge>(params object[] ids) => ((IGremlinQuerySource)this).E(ids).OfType<TEdge>();

        IEdgeGremlinQuery<TNewEdge> IStartGremlinQuery.ReplaceE<TNewEdge>(TNewEdge edge) => ((IGremlinQuerySource)this).E<TNewEdge>(edge!.GetId(Environment)).Update(edge);

        IGremlinQuerySource IGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation) => new GremlinQuery<T1, T2, T3, T4>(transformation(Environment), Steps, LabelProjections, Metadata);

        IGremlinQuerySource IGremlinQuerySource.ConfigureMetadata(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) => new GremlinQuery<T1, T2, T3, T4>(Environment, Steps, LabelProjections, metadataTransformation(Metadata));

        IGremlinQuerySource IGremlinQuerySource.WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => WithSideEffect(label, value);

        TQuery IGremlinQuerySource.WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation) => WithSideEffect(value, continuation);

        IGremlinQuerySource IGremlinQuerySource.WithPartitionStrategy(string partitionKey) => WithPartitionStrategy(partitionKey);

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

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both() => Both();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge>() => Both<TEdge>();

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE() => BothE();

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.BothE<TEdge>() => BothE<TEdge>();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In() => In();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge>() => In<TEdge>();

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE() => InE();

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.InE<TEdge>() => InE<TEdge>();

        IInEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.InE<TEdge>() => InE<TEdge>();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out() => Out();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge>() => Out<TEdge>();

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.OutE<TEdge>() => OutE<TEdge>();

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE() => OutE();

        IOutEdgeGremlinQuery<TEdge, T1> IVertexGremlinQueryBase<T1>.OutE<TEdge>() => OutE<TEdge>();

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties() => VertexProperties<object>(Array.Empty<Expression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>[] projections) => VertexProperties<TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>(params Expression<Func<T1, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<T1>.Properties<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>[]>>[] projections) => VertexProperties<TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<T1>.Properties<TValue>() => VertexProperties<TValue>(Array.Empty<Expression>());

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<T1>.Properties(params Expression<Func<T1, VertexProperty<object>>>[] projections) => VertexProperties<object>(projections);

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue, TNewMeta>(params Expression<Func<T1, VertexProperty<TValue, TNewMeta>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TValue> IVertexGremlinQueryBase<T1>.Values<TValue>(params Expression<Func<T1, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget, TTargetMeta>(params Expression<Func<T1, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<object> IVertexGremlinQueryBase<T1>.Values(params Expression<Func<T1, VertexProperty<object>>>[] projections) => ValuesForProjections<object>(projections);

        IVertexGremlinQuery<T1> IVertexGremlinQueryBase<T1>.Update(T1 element) => AddOrUpdate(element, false);

        IVertexGremlinQuery<T1> IVertexGremlinQuery<T1>.Property<TProjectedValue>(Expression<Func<T1, TProjectedValue[]>> projection, TProjectedValue value) => Property(projection, value != null ? new[] { value } : null);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<T1>.Values<TTarget>(params Expression<Func<T1, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IElementGremlinQuery<T1> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Lower() => this;

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Properties<TValue>(params Expression<Func<T3, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IVertexPropertyGremlinQuery<T1, T2, T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Property<TValue>(Expression<Func<T3, TValue>> projection, TValue value) => Property(projection, value);

        IGremlinQuery<T2> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Value() => Value<T2>();

        IGremlinQuery<T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.ValueMap() => ValueMap<T3>(ImmutableArray<string>.Empty);

        IGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Values<TTarget>(params Expression<Func<T3, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IVertexPropertyGremlinQuery<T1, T2, T3> IVertexPropertyGremlinQueryBase<T1, T2, T3>.Where(Expression<Func<VertexProperty<T2, T3>, bool>> predicate) => Where(predicate);

        IElementGremlinQuery<T1> IVertexPropertyGremlinQueryBase<T1, T2>.Lower() => this;

        IElementGremlinQuery<object> IVertexPropertyGremlinQueryBase.Lower() => CloneAs<IElementGremlinQuery<object>>();

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>(ImmutableArray<string>.Empty);

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys.ToImmutableArray());

        IMapGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params string[] keys) => ValueMap<IDictionary<string, object>>(keys.ToImmutableArray());

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>() => ValuesForKeys<TValue>(Array.Empty<Key>());

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params string[] keys) => ValuesForKeys<TValue>(keys.Select(static x => (Key)x));

        IGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params string[] keys) => ValuesForKeys<object>(keys.Select(static x => (Key)x));

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params string[] keys) => Properties<Property<object>, object, object>(Projection.Property, keys);

        IVertexPropertyGremlinQuery<VertexProperty<T2, TNewMeta>, T2, TNewMeta> IVertexPropertyGremlinQueryBase<T1, T2>.Meta<TNewMeta>() => CloneAs<IVertexPropertyGremlinQuery<VertexProperty<T2, TNewMeta>, T2, TNewMeta>>();

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<T1, T2>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, object, object>(Projection.Property, keys);

        IGremlinQuery<T2> IVertexPropertyGremlinQueryBase<T1, T2>.Value() => Value<T2>();

        IInEdgeGremlinQuery<T1, T3> IEdgeGremlinQuery<T1, T2, T3>.AsInEdge() => this;

        IOutEdgeGremlinQuery<T1, T2> IEdgeGremlinQuery<T1, T2, T3>.AsOutEdge() => this;

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.LimitLocal(long count) => LimitLocal(count);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.RangeLocal(long low, long high) => RangeLocal(low, high);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.SkipLocal(long count) => Skip(count, Scope.Local);

        IArrayGremlinQuery<T1, T2, T4> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<T1, T2, T4>>.TailLocal(long count) => TailLocal(count);
    }
}
