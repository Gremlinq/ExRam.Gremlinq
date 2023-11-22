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
    internal partial class GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery> :
        IGremlinQueryAdmin,

        IGremlinQuerySource,
        IGremlinQuery<TElement>,

        IArrayGremlinQuery<TElement, TScalar, TFoldedQuery>,

        IElementGremlinQuery<TElement>,

        IMapGremlinQuery<TElement>,

        IEdgeOrVertexGremlinQuery<TElement>,
        IVertexGremlinQuery<TElement>,
        IEdgeGremlinQuery<TElement>,
        IInOrOutEdgeGremlinQuery<TElement, TOutVertex>,
        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>,

        IInEdgeGremlinQuery<TElement, TInVertex>,
        IOutEdgeGremlinQuery<TElement, TOutVertex>,

        IVertexPropertyGremlinQuery<TElement, TScalar>,
        IVertexPropertyGremlinQuery<TElement, TScalar, TMeta>,
        IPropertyGremlinQuery<TElement>
    {
        TFoldedQuery IArrayGremlinQueryBase<TElement, TScalar, TFoldedQuery>.Unfold() => Unfold<TFoldedQuery>();

        IGremlinQuery<TElement> IArrayGremlinQueryBase<TElement, TScalar>.Lower() => this;

        IGremlinQuery<object> IArrayGremlinQueryBase.Unfold() => Unfold<IGremlinQuery<object>>();

        IGremlinQuery<TScalar[]> IArrayGremlinQueryBase<TScalar>.Lower() => Cast<TScalar[]>();

        IGremlinQuery<object[]> IArrayGremlinQueryBase.Lower() => Cast<object[]>();

        IGremlinQuery<TScalar> IArrayGremlinQueryBase<TScalar>.Unfold() => Unfold<IGremlinQuery<TScalar>>();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Lower() => this;

        IEdgeGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEdgeGremlinQueryBase<TElement, TInVertex>.From<TNewOutVertex>(Func<IVertexGremlinQuery<TInVertex>, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal) => Cast<TInVertex>().From<TElement, TNewOutVertex, TInVertex>(fromVertexTraversal);

        IVertexGremlinQuery<TInVertex> IInEdgeGremlinQueryBase<TElement, TInVertex>.InV() => InV<TInVertex>();

        IEdgeGremlinQuery<TElement> IInEdgeGremlinQueryBase<TElement, TInVertex>.Lower() => this;

        IEdgeGremlinQuery<TElement> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.Lower() => this;

        IVertexGremlinQuery<TOutVertex> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.OutV() => OutV<TOutVertex>();

        IEdgeGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEdgeGremlinQueryBase<TElement, TOutVertex>.To<TNewInVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal) => Cast<TOutVertex>().To<TElement, TOutVertex, TNewInVertex>(toVertexTraversal);

        IEdgeGremlinQuery<object> IInEdgeGremlinQueryBase.Lower() => Cast<object>();

        IEdgeGremlinQuery<object> IOutEdgeGremlinQueryBase.Lower() => Cast<object>();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQueryBase<TElement, TOutVertex, TInVertex>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IEdgeGremlinQueryBase.Lower() => Cast<object>();

        IVertexGremlinQuery<object> IEdgeGremlinQueryBase.BothV() => BothV<object>();

        IVertexGremlinQuery<TVertex> IEdgeGremlinQueryBase.BothV<TVertex>() => ((IEdgeGremlinQueryBase)this)
            .BothV()
            .OfType<TVertex>();

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQueryBase<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => From<TElement, TNewOutVertex, TInVertex>(stepLabel);

        IEdgeOrVertexGremlinQuery<TElement> IEdgeGremlinQueryBase<TElement>.Lower() => this;

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQueryBase<TElement>.From<TNewOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, object>(fromVertexTraversal);

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

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase<TElement>.Properties(params Expression<Func<TElement, Property<object>>>[] projections) => Properties<Property<object>, object, object>(Projection.Property, projections);

        IPropertyGremlinQuery<Property<object>> IEdgeGremlinQueryBase.Properties() => Properties<Property<object>, object, object>(Projection.Property, Array.Empty<string>());

        IPropertyGremlinQuery<Property<TValue>> IEdgeGremlinQueryBase.Properties<TValue>() => Properties<Property<TValue>, object, object>(Projection.Property, Array.Empty<string>());

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQueryBase<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, object, TNewInVertex>(stepLabel);

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQueryBase<TElement>.To<TNewInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TNewInVertex>> toVertexTraversal) => To<TElement, object, TNewInVertex>(toVertexTraversal);

        IGremlinQuery<TValue> IEdgeGremlinQueryBase<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<object> IEdgeGremlinQueryBase<TElement>.Values(params Expression<Func<TElement, Property<object>>>[] projections) => ValuesForProjections<object>(projections);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQueryBase<TElement>.Update(TElement element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IEdgeGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IElementGremlinQuery<TElement> IEdgeOrVertexGremlinQueryBase<TElement>.Lower() => this;

        IElementGremlinQuery<object> IEdgeOrVertexGremlinQueryBase.Lower() => Cast<object>();

        IGremlinQuery<object> IElementGremlinQueryBase.Id() => Id();

        IGremlinQuery<string> IElementGremlinQueryBase.Label() => Label();

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>(ImmutableArray<string>.Empty);

        IMapGremlinQuery<IDictionary<string, object>> IElementGremlinQueryBase.ValueMap() => ValueMap<IDictionary<string, object>>(ImmutableArray<string>.Empty);

        IGremlinQuery<TValue> IElementGremlinQueryBase.Values<TValue>() => ValuesForKeys<TValue>(Array.Empty<Key>());

        IGremlinQuery<object> IElementGremlinQueryBase.Values() => ValuesForKeys<object>(Array.Empty<Key>());

        IElementGremlinQuery<TElement> IElementGremlinQueryBase<TElement>.Update(TElement element) => AddOrUpdate(element, false);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IElementGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IMapGremlinQuery<IDictionary<string, TTarget>> IElementGremlinQueryBase<TElement>.ValueMap<TTarget>(params Expression<Func<TElement, TTarget>>[] keys) => ValueMap<IDictionary<string, TTarget>>(keys);

        IMapGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceValueTuple() => CloneAs<IMapGremlinQuery<TElement>>(maybeNewTraversal: Steps.WithProjection(Projection.Value));

        IArrayGremlinQuery<TElement[], TElement, IGremlinQuery<TElement>> IGremlinQueryBase<TElement>.ForceArray() => CloneAs<IArrayGremlinQuery<TElement[], TElement, IGremlinQuery<TElement>>>(maybeNewTraversal: Steps.WithProjection(Projection.Value.Fold()));

        IGremlinQuery<TValue> IGremlinQueryBase.Constant<TValue>(TValue constant) => Constant(constant);

        string IGremlinQueryBase.Debug() => Debug();

        IGremlinQuery<long> IGremlinQueryBase.Count() => CountGlobal();

        IGremlinQuery<long> IGremlinQueryBase.CountLocal() => CountLocal();

        IGremlinQuery<string> IGremlinQueryBase.Explain() => Explain();

        IMapGremlinQuery<IDictionary<TElement, TElement[]>> IGremlinQueryBase<TElement>.Group() => Group(static _ => _.ByKey(static __ => __));

        TaskAwaiter IGremlinQueryBase.GetAwaiter() => ((Task)((IGremlinQuery<TElement>)this).ToAsyncEnumerable().LastOrDefaultAsync().AsTask()).GetAwaiter();

        IGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceBase() => CloneAs<IGremlinQuery<TElement>>();

        IElementGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceElement() => this
            .Continue()
            .Build(static builder => builder
                .WithNewProjection(static _ => _.Highest(Projection.Element))
                .Build<IElementGremlinQuery<TElement>>());

        IVertexGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceVertex() => CloneAs<IVertexGremlinQuery<TElement>>(maybeNewTraversal: Steps.WithProjection(Projection.Vertex));

        IVertexPropertyGremlinQuery<TElement, TNewValue> IGremlinQueryBase<TElement>.ForceVertexProperty<TNewValue>() => CloneAs<IVertexPropertyGremlinQuery<TElement, TNewValue>>(maybeNewTraversal: Steps.WithProjection(Projection.Element));

        IVertexPropertyGremlinQuery<TElement, TNewValue, TNewMeta> IGremlinQueryBase<TElement>.ForceVertexProperty<TNewValue, TNewMeta>() => CloneAs<IVertexPropertyGremlinQuery<TElement, TNewValue, TNewMeta>>(maybeNewTraversal: Steps.WithProjection(Projection.Element));

        IPropertyGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceProperty() => CloneAs<IPropertyGremlinQuery<TElement>>(maybeNewTraversal: Steps.WithProjection(Projection.Value));

        IEdgeGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceEdge() => CloneAs<IEdgeGremlinQuery<TElement>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IInEdgeGremlinQuery<TElement, TNewInVertex> IGremlinQueryBase<TElement>.ForceInEdge<TNewInVertex>() => CloneAs<IInEdgeGremlinQuery<TElement, TNewInVertex>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IGremlinQueryBase<TElement>.ForceOutEdge<TNewOutVertex>() => CloneAs<IOutEdgeGremlinQuery<TElement, TNewOutVertex>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IEdgeGremlinQuery<TElement, TNewOutVertex, TNewInVertex> IGremlinQueryBase<TElement>.ForceEdge<TNewOutVertex, TNewInVertex>() => CloneAs<IEdgeGremlinQuery<TElement, TNewOutVertex, TNewInVertex>>(maybeNewTraversal: Steps.WithProjection(Projection.Edge));

        IGremlinQuery<TElement> IGremlinQueryBase<TElement>.ForceValue() => CloneAs<IGremlinQuery<TElement>>(maybeNewTraversal: Steps.WithProjection(Projection.Value));

        GremlinQueryAwaiter<TElement> IGremlinQueryBase<TElement>.GetAwaiter() => new((this).ToArrayAsync().AsTask().GetAwaiter());

        IAsyncEnumerable<TElement> IGremlinQueryBase<TElement>.ToAsyncEnumerable() => Environment.Executor
            .Execute<TElement>(GremlinQueryExecutionContext.Create(this));

        IGremlinQuery<Path> IGremlinQueryBase.Path() => Path();

        IGremlinQuery<string> IGremlinQueryBase.Profile() => Profile();

        TQuery IGremlinQueryBase.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label) => Select(label).CloneAs<TQuery>();

        IArrayGremlinQuery<TNewElement, TNewScalar, TQuery> IGremlinQueryBase.Cap<TNewElement, TNewScalar, TQuery>(StepLabel<IArrayGremlinQuery<TNewElement, TNewScalar, TQuery>, TNewElement> label) => Cap(label);

        IGremlinQuery<TLabelledElement> IGremlinQueryBase.Select<TLabelledElement>(StepLabel<TLabelledElement> label) => Select(label);

        IGremlinQuery<TElement> IGremlinQueryBase<TElement>.Lower() => this;

        IGremlinQuery<object> IGremlinQueryBase.Lower() => Cast<object>();

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

        IGremlinQuerySource IGremlinQueryAdmin.GetSource() => CloneAs<GremlinQuery<object, object, object, object, object, object>>(maybeNewTraversal: Traversal.Empty);

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

        IGremlinQuerySource IGremlinQuerySource.ConfigureEnvironment(Func<IGremlinQueryEnvironment, IGremlinQueryEnvironment> transformation) => new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(transformation(Environment), Steps, LabelProjections, Metadata);

        IGremlinQuerySource IGremlinQuerySource.ConfigureMetadata(Func<IImmutableDictionary<object, object?>, IImmutableDictionary<object, object?>> metadataTransformation) => new GremlinQuery<TElement, TOutVertex, TInVertex, TScalar, TMeta, TFoldedQuery>(Environment, Steps, LabelProjections, metadataTransformation(Metadata));

        IGremlinQuerySource IGremlinQuerySource.WithSideEffect<TSideEffect>(StepLabel<TSideEffect> label, TSideEffect value) => WithSideEffect(label, value);

        TQuery IGremlinQuerySource.WithSideEffect<TSideEffect, TQuery>(TSideEffect value, Func<IGremlinQuerySource, StepLabel<TSideEffect>, TQuery> continuation) => WithSideEffect(value, continuation);

        IEdgeGremlinQuery<TElement> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.Lower() => this;

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => From<TElement, TTargetVertex, TOutVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.From<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQueryBase<TTargetVertex>> fromVertexTraversal) => Cast<TOutVertex>().From<TElement, TTargetVertex, TOutVertex>(fromVertexTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IInOrOutEdgeGremlinQueryBase<TElement, TOutVertex>.To<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IVertexGremlinQueryBase<TTargetVertex>> toVertexTraversal) => Cast<TOutVertex>().To<TElement, TOutVertex, TTargetVertex>(toVertexTraversal);

        IEdgeGremlinQuery<object> IInOrOutEdgeGremlinQueryBase.Lower() => Cast<object>();

        IGremlinQuery<string> IPropertyGremlinQueryBase<TElement>.Key() => Key();

        IGremlinQuery<TValue> IPropertyGremlinQueryBase<TElement>.Value<TValue>() => Value<TValue>();

        IGremlinQuery<object> IPropertyGremlinQueryBase<TElement>.Value() => Value<object>();

        IGremlinQuery<TScalar> IArrayGremlinQueryBase<TScalar>.SumLocal() => SumLocal<TScalar>();

        IGremlinQuery<object> IArrayGremlinQueryBase.SumLocal() => SumLocal<object>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TScalar, TFoldedQuery>.SumLocal() => SumLocal<object>().CloneAs<TFoldedQuery>();

        IGremlinQuery<TScalar> IArrayGremlinQueryBase<TScalar>.MinLocal() => MinLocal<TScalar>();

        IGremlinQuery<object> IArrayGremlinQueryBase.MinLocal() => MinLocal<object>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TScalar, TFoldedQuery>.MinLocal() => MinLocal<object>().CloneAs<TFoldedQuery>();

        IGremlinQuery<TScalar> IArrayGremlinQueryBase<TScalar>.MaxLocal() => MaxLocal<TScalar>();

        IGremlinQuery<object> IArrayGremlinQueryBase.MaxLocal() => MaxLocal<object>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TScalar, TFoldedQuery>.MaxLocal() => MaxLocal<object>().CloneAs<TFoldedQuery>();

        IGremlinQuery<TScalar> IArrayGremlinQueryBase<TScalar>.MeanLocal() => MeanLocal<TScalar>();

        IGremlinQuery<object> IArrayGremlinQueryBase.MeanLocal() => MeanLocal<object>();

        TFoldedQuery IArrayGremlinQueryBase<TElement, TScalar, TFoldedQuery>.MeanLocal() => MeanLocal<object>().CloneAs<TFoldedQuery>();

        IGremlinQuery<TTargetValue> IMapGremlinQueryBase<TElement>.Select<TTargetValue>(Expression<Func<TElement, TTargetValue>> projection) => Select<IGremlinQuery<TTargetValue>>(projection);

        IEdgeOrVertexGremlinQuery<TElement> IVertexGremlinQueryBase<TElement>.Lower() => this;

        IEdgeOrVertexGremlinQuery<object> IVertexGremlinQueryBase.Lower() => Cast<object>();

        IInOrOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IInOrOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both() => Both();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Both<TEdge>() => Both<TEdge>();

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.BothE() => BothE();

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.BothE<TEdge>() => BothE<TEdge>();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In() => In();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.In<TEdge>() => In<TEdge>();

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.InE() => InE();

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.InE<TEdge>() => InE<TEdge>();

        IInEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.InE<TEdge>() => InE<TEdge>();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out() => Out();

        IVertexGremlinQuery<object> IVertexGremlinQueryBase.Out<TEdge>() => Out<TEdge>();

        IEdgeGremlinQuery<TEdge> IVertexGremlinQueryBase.OutE<TEdge>() => OutE<TEdge>();

        IEdgeGremlinQuery<object> IVertexGremlinQueryBase.OutE() => OutE();

        IOutEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQueryBase<TElement>.OutE<TEdge>() => OutE<TEdge>();

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<TElement>.Properties() => VertexProperties<object>(Array.Empty<Expression>());

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, TValue[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => VertexProperties<TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>(params Expression<Func<TElement, VertexProperty<TValue>[]>>[] projections) => VertexProperties<TValue>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TNewMeta>, TValue, TNewMeta> IVertexGremlinQueryBase<TElement>.Properties<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>[]>>[] projections) => VertexProperties<TValue, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> IVertexGremlinQueryBase<TElement>.Properties<TValue>() => VertexProperties<TValue>(Array.Empty<Expression>());

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> IVertexGremlinQueryBase<TElement>.Properties(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => VertexProperties<object>(projections);

        IGremlinQuery<TValue> IVertexGremlinQueryBase<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TValue> IVertexGremlinQueryBase<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => ValuesForProjections<TValue>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<object> IVertexGremlinQueryBase<TElement>.Values(params Expression<Func<TElement, VertexProperty<object>>>[] projections) => ValuesForProjections<object>(projections);

        IVertexGremlinQuery<TElement> IVertexGremlinQueryBase<TElement>.Update(TElement element) => AddOrUpdate(element, false);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TProjectedValue>(Expression<Func<TElement, TProjectedValue[]>> projection, TProjectedValue value) => Property(projection, value != null ? new[] { value } : null);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IGremlinQuery<TTarget> IVertexGremlinQueryBase<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => ValuesForProjections<TTarget>(projections);

        IElementGremlinQuery<TElement> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Lower() => this;

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Properties<TValue>(params Expression<Func<TMeta, TValue>>[] projections) => Properties<Property<TValue>, TValue, object>(Projection.Property, projections);

        IVertexPropertyGremlinQuery<TElement, TScalar, TMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, value);

        IGremlinQuery<TScalar> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Value() => Value<TScalar>();

        IGremlinQuery<TMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.ValueMap() => ValueMap<TMeta>(ImmutableArray<string>.Empty);

        IGremlinQuery<TTarget> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => ValuesForProjections<TTarget>(projections);

        IVertexPropertyGremlinQuery<TElement, TScalar, TMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar, TMeta>.Where(Expression<Func<VertexProperty<TScalar, TMeta>, bool>> predicate) => Where(predicate);

        IElementGremlinQuery<TElement> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Lower() => this;

        IElementGremlinQuery<object> IVertexPropertyGremlinQueryBase.Lower() => Cast<object>();

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>() => ValueMap<IDictionary<string, TTarget>>(ImmutableArray<string>.Empty);

        IMapGremlinQuery<IDictionary<string, TTarget>> IVertexPropertyGremlinQueryBase.ValueMap<TTarget>(params string[] keys) => ValueMap<IDictionary<string, TTarget>>(keys.ToImmutableArray());

        IMapGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQueryBase.ValueMap(params string[] keys) => ValueMap<IDictionary<string, object>>(keys.ToImmutableArray());

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>() => ValuesForKeys<TValue>(Array.Empty<Key>());

        IGremlinQuery<TValue> IVertexPropertyGremlinQueryBase.Values<TValue>(params string[] keys) => ValuesForKeys<TValue>(keys.Select(static x => (Key)x));

        IGremlinQuery<object> IVertexPropertyGremlinQueryBase.Values(params string[] keys) => ValuesForKeys<object>(keys.Select(static x => (Key)x));

        IPropertyGremlinQuery<Property<object>> IVertexPropertyGremlinQueryBase.Properties(params string[] keys) => Properties<Property<object>, object, object>(Projection.Property, keys);

        IVertexPropertyGremlinQuery<VertexProperty<TScalar, TNewMeta>, TScalar, TNewMeta> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Meta<TNewMeta>() => CloneAs<GremlinQuery<VertexProperty<TScalar, TNewMeta>, object, object, TScalar, TNewMeta, object>>();

        IPropertyGremlinQuery<Property<TValue>> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Properties<TValue>(params string[] keys) => Properties<Property<TValue>, object, object>(Projection.Property, keys);

        IGremlinQuery<TScalar> IVertexPropertyGremlinQueryBase<TElement, TScalar>.Value() => Value<TScalar>();

        IInEdgeGremlinQuery<TElement, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.AsInEdge() => this;

        IOutEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.AsOutEdge() => this;

        IArrayGremlinQuery<TElement, TScalar, TFoldedQuery> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TScalar, TFoldedQuery>>.LimitLocal(long count) => LimitLocal(count);

        IArrayGremlinQuery<TElement, TScalar, TFoldedQuery> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TScalar, TFoldedQuery>>.RangeLocal(long low, long high) => RangeLocal(low, high);

        IArrayGremlinQuery<TElement, TScalar, TFoldedQuery> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TScalar, TFoldedQuery>>.SkipLocal(long count) => Skip(count, Scope.Local);

        IArrayGremlinQuery<TElement, TScalar, TFoldedQuery> IArrayGremlinQueryBaseRec<IArrayGremlinQuery<TElement, TScalar, TFoldedQuery>>.TailLocal(long count) => TailLocal(count);
    }
}
