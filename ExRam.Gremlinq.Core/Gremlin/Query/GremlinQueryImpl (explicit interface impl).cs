// ReSharper disable ArrangeThisQualifier
// ReSharper disable CoVariantArrayConversion
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;
using LanguageExt;
using NullGuard;

namespace ExRam.Gremlinq.Core
{
    partial class GremlinQueryImpl<TElement, TOutVertex, TInVertex, TMeta> :
        IGremlinQueryAdmin,

        IOrderedGremlinQuery<TElement>,
        IOrderedElementGremlinQuery<TElement>,

        IOrderedVertexGremlinQuery<TElement>,
        IOrderedEdgeGremlinQuery<TElement>,
        IOrderedEdgeGremlinQuery<TElement, TOutVertex>,
        IOrderedInEdgeGremlinQuery<TElement, TInVertex>,
        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>,
        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>,

        IOrderedVertexPropertyGremlinQuery<TElement>,
        IOrderedVertexPropertyGremlinQuery<TElement, TMeta>,
        IOrderedEdgePropertyGremlinQuery<TElement>
    {
        IEdgeGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.AddE<TEdge>() => AddE(new TEdge());

        IVertexGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        TTargetQuery IGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVertexGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedVertexGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, StepLabel<VertexProperty<TElement>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, StepLabel<VertexProperty<TElement>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<VertexProperty<TElement, TMeta>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<VertexProperty<TElement, TMeta>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<Property<TElement>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, StepLabel<Property<TElement>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.Aggregate<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement, TMeta>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEdgePropertyGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedEdgePropertyGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.And(params Func<IVertexGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery IGremlinQuery.And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVertexGremlinQuery IVertexGremlinQuery.And(params Func<IVertexGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery IElementGremlinQuery.And(params Func<IElementGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.And(params Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.And(params Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.And(params Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.And(params Func<IEdgeGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEdgeGremlinQuery IEdgeGremlinQuery.And(params Func<IEdgeGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.And(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.And(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.And(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.And(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.And(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.And(params Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.And(params Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.And(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.And(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.And(params Func<IGremlinQuery<Property<TElement>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        TTargetQuery IVertexGremlinQuery<TElement>.As<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, StepLabel<IVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, StepLabel<IEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<IOrderedGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<TElement>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedVertexGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedVertexGremlinQuery<TElement>, StepLabel<IOrderedVertexGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, StepLabel<IVertexPropertyGremlinQuery<TElement, TMeta>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.As<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, StepLabel<IVertexPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.As<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, StepLabel<IEdgePropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement>, StepLabel<IOrderedEdgeGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedInEdgeGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, StepLabel<IInEdgeGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.As<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, StepLabel<IElementGremlinQuery<VertexProperty<TElement>>, VertexProperty<TElement>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.As<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, StepLabel<IGremlinQuery<VertexProperty<TElement>>, VertexProperty<TElement>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.As<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, VertexProperty<TElement, TMeta>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.As<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<IGremlinQuery<VertexProperty<TElement, TMeta>>, VertexProperty<TElement, TMeta>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<Property<TElement>>.As<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, StepLabel<IGremlinQuery<Property<TElement>>, Property<TElement>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement>, StepLabel<IOrderedVertexPropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.As<TTargetQuery>(Func<IOrderedVertexPropertyGremlinQuery<TElement, TMeta>, StepLabel<IOrderedVertexPropertyGremlinQuery<TElement, TMeta>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedEdgePropertyGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedEdgePropertyGremlinQuery<TElement>, StepLabel<IOrderedEdgePropertyGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery IGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IVertexGremlinQuery IVertexGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery IElementGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.As(StepLabel stepLabel) => As(stepLabel);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEdgeGremlinQuery IEdgeGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.As(StepLabel stepLabel) => As(stepLabel);

        TTargetQuery IOrderedEdgeGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, StepLabel<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<IOrderedElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IGremlinQueryAdmin IGremlinQuery.AsAdmin() => this;

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier() => Barrier();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Barrier() => Barrier();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Barrier() => Barrier();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Barrier() => Barrier();

        IGremlinQuery IGremlinQuery.Barrier() => Barrier();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Barrier() => Barrier();

        IElementGremlinQuery IElementGremlinQuery.Barrier() => Barrier();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Barrier() => Barrier();

        IVertexGremlinQuery IVertexGremlinQuery.Barrier() => Barrier();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Barrier() => Barrier();

        IEdgeGremlinQuery IEdgeGremlinQuery.Barrier() => Barrier();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Barrier() => Barrier();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Barrier() => Barrier();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Barrier() => Barrier();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Barrier() => Barrier();

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Barrier() => Barrier();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Barrier() => Barrier();

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Barrier() => Barrier();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Barrier() => Barrier();

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Barrier() => Barrier();

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Both<TEdge>() => AddStep<IVertex>(new BothStep(_model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));

        IEdgeGremlinQuery<TEdge> IVertexGremlinQuery.BothE<TEdge>() => AddStep<TEdge>(new BothEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.BothV() => AddStep<IVertex>(BothVStep.Instance);

        IGremlinQuery<TResult> IGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IVertexGremlinQuery<TResult> IVertexGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IEdgeGremlinQuery<TResult, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IEdgeGremlinQuery<TResult> IEdgeGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IEdgeGremlinQuery<TResult, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IOutEdgeGremlinQuery<TResult, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IInEdgeGremlinQuery<TResult, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedEdgeGremlinQuery<TResult, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IEdgeGremlinQuery<TResult> IEdgeGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IOrderedOutEdgeGremlinQuery<TResult, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedInEdgeGremlinQuery<TResult, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedEdgeGremlinQuery<TResult> IOrderedEdgeGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IEdgePropertyGremlinQuery<TResult> IEdgePropertyGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IVertexPropertyGremlinQuery<TResult, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Cast<TResult>() => Cast<TResult>();

        IVertexPropertyGremlinQuery<TResult> IVertexPropertyGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedVertexGremlinQuery<TResult> IOrderedVertexGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IVertexGremlinQuery<TResult> IVertexGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedGremlinQuery<TResult> IOrderedGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery<VertexProperty<TElement>>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<VertexProperty<TElement>>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<VertexProperty<TElement, TMeta>>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<Property<TElement>>.Cast<TResult>() => Cast<TResult>();

        IOrderedVertexPropertyGremlinQuery<TResult> IOrderedVertexPropertyGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedVertexPropertyGremlinQuery<TResult, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.Cast<TResult>() => Cast<TResult>();

        IOrderedEdgePropertyGremlinQuery<TResult> IOrderedEdgePropertyGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedEdgeGremlinQuery<TResult, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedElementGremlinQuery<TResult> IOrderedElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<Property<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<Property<TElement>>, TTargetQuery> trueChoice, Func<IGremlinQuery<Property<TElement>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<Property<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<Property<TElement>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice, Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEdgeGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEdgeGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEdgeGremlinQuery.Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice, Func<IEdgeGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEdgeGremlinQuery.Choose<TTargetQuery>(Func<IEdgeGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEdgeGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVertexGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IVertexGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVertexGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVertexGremlinQuery.Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice, Func<IVertexGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVertexGremlinQuery.Choose<TTargetQuery>(Func<IVertexGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVertexGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice, Func<IElementGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEdgeGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEdgeGremlinQuery.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery.Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Coalesce<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Coalesce<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVertexGremlinQuery.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVertexGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<Property<TElement>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<Property<TElement>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery<long> IGremlinQuery.Count() => AddStep<long, Unit, Unit, Unit>(CountStep.Instance);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup() => Dedup();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Dedup() => Dedup();

        IGremlinQuery IGremlinQuery.Dedup() => Dedup();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Dedup() => Dedup();

        IVertexGremlinQuery IVertexGremlinQuery.Dedup() => Dedup();

        IElementGremlinQuery IElementGremlinQuery.Dedup() => Dedup();

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Dedup() => Dedup();

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Dedup() => Dedup();

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Dedup() => Dedup();

        IEdgeGremlinQuery IEdgeGremlinQuery.Dedup() => Dedup();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Dedup() => Dedup();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Dedup() => Dedup();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Dedup() => Dedup();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Dedup() => Dedup();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Dedup() => Dedup();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Dedup() => Dedup();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Dedup() => Dedup();

        IGremlinQuery<Unit> IGremlinQuery.Drop() => Drop();

        IEdgeGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit>(new EStep(ids)).OfType<TEdge>(_model.EdgesModel, true);

        IEdgeGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids) => AddStep<IEdge, Unit, Unit, Unit>(new EStep(ids));

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Emit() => Emit();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit() => Emit();

        IGremlinQuery IGremlinQuery.Emit() => Emit();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Emit() => Emit();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Emit() => Emit();

        IVertexGremlinQuery IVertexGremlinQuery.Emit() => Emit();

        IElementGremlinQuery IElementGremlinQuery.Emit() => Emit();

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Emit() => Emit();

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Emit() => Emit();

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Emit() => Emit();

        IEdgeGremlinQuery IEdgeGremlinQuery.Emit() => Emit();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Emit() => Emit();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Emit() => Emit();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Emit() => Emit();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Emit() => Emit();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Emit() => Emit();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Emit() => Emit();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Emit() => Emit();

        IGremlinQuery<string> IGremlinQuery.Explain() => AddStep<string, Unit, Unit, Unit>(ExplainStep.Instance);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Filter(string lambda) => Filter(lambda);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Filter(string lambda) => Filter(lambda);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Filter(string lambda) => Filter(lambda);

        IGremlinQuery IGremlinQuery.Filter(string lambda) => Filter(lambda);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IElementGremlinQuery IElementGremlinQuery.Filter(string lambda) => Filter(lambda);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IEdgeGremlinQuery IEdgeGremlinQuery.Filter(string lambda) => Filter(lambda);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Filter(string lambda) => Filter(lambda);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Filter(string lambda) => Filter(lambda);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Filter(string lambda) => Filter(lambda);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Filter(string lambda) => Filter(lambda);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IVertexGremlinQuery IVertexGremlinQuery.Filter(string lambda) => Filter(lambda);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Filter(string lambda) => Filter(lambda);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Filter(string lambda) => Filter(lambda);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Filter(string lambda) => Filter(lambda);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IGremlinQuery<TElement[]> IGremlinQuery<TElement>.Fold() => Fold<TElement[]>();

        IGremlinQuery<VertexProperty<TElement>[]> IGremlinQuery<VertexProperty<TElement>>.Fold() => Fold<VertexProperty<TElement>[]>();

        IGremlinQuery<VertexProperty<TElement, TMeta>[]> IGremlinQuery<VertexProperty<TElement, TMeta>>.Fold() => Fold<VertexProperty<TElement, TMeta>[]>();

        IGremlinQuery<Property<TElement>[]> IGremlinQuery<Property<TElement>>.Fold() => Fold<Property<TElement>[]>();

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, Unit, Unit>(new FromLabelStep(stepLabel));

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, Unit>(new FromLabelStep(stepLabel));

        IEdgeGremlinQuery<TElement, TTargetVertex, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit>())));

        IOutEdgeGremlinQuery<TElement, TNewOutVertex> IEdgeGremlinQuery<TElement>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, Unit>(fromVertexTraversal);

        IEdgeGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, TInVertex>(fromVertexTraversal);

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetEnumerator() => GetEnumerator<TElement>();

        IAsyncEnumerator<VertexProperty<TElement>> IAsyncEnumerable<VertexProperty<TElement>>.GetEnumerator() => GetEnumerator<VertexProperty<TElement>>();

        IAsyncEnumerator<VertexProperty<TElement, TMeta>> IAsyncEnumerable<VertexProperty<TElement, TMeta>>.GetEnumerator() => GetEnumerator<VertexProperty<TElement, TMeta>>();

        IAsyncEnumerator<Property<TElement>> IAsyncEnumerable<Property<TElement>>.GetEnumerator() => GetEnumerator<Property<TElement>>();

        IGremlinQuery<object> IElementGremlinQuery.Id() => Id();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity() => Identity();

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Identity() => Identity();

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Identity() => Identity();

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Identity() => Identity();

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Identity() => Identity();

        IGremlinQuery IGremlinQuery.Identity() => Identity();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Identity() => Identity();

        IVertexGremlinQuery IVertexGremlinQuery.Identity() => Identity();

        IElementGremlinQuery IElementGremlinQuery.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Identity() => Identity();

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Identity() => Identity();

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Identity() => Identity();

        IEdgeGremlinQuery IEdgeGremlinQuery.Identity() => Identity();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Identity() => Identity();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Identity() => Identity();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Identity() => Identity();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Identity() => Identity();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Identity() => Identity();

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.In<TEdge>() => AddStep<IVertex>(new InStep(_model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));

        IInEdgeGremlinQuery<TEdge, TElement> IVertexGremlinQuery<TElement>.InE<TEdge>() => AddStep<TEdge, Unit, TElement, Unit>(new InEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IGremlinQuery<TNewElement> IGremlinQuerySource.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Inject(params VertexProperty<TElement>[] elements) => Inject(elements);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Inject(params VertexProperty<TElement, TMeta>[] elements) => Inject(elements);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Inject(params Property<TElement>[] elements) => Inject(elements);

        IGremlinQuery IGremlinQueryAdmin.InsertStep(int index, Step step) => new GremlinQueryImpl<TElement, TOutVertex, TInVertex, TMeta>(_model, _queryExecutor, _steps.Insert(index, step), _stepLabelMappings, _logger);

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.InV() => InV<IVertex>();

        IVertexGremlinQuery<TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => InV<TInVertex>();

        IVertexGremlinQuery<TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.InV() => InV<TInVertex>();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IGremlinQuery IGremlinQuery.Limit(long count) => Limit(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IVertexGremlinQuery IVertexGremlinQuery.Limit(long count) => Limit(count);

        IElementGremlinQuery IElementGremlinQuery.Limit(long count) => Limit(count);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Limit(long count) => Limit(count);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEdgeGremlinQuery IEdgeGremlinQuery.Limit(long count) => Limit(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Limit(long count) => Limit(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Limit(long count) => Limit(count);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Limit(long count) => Limit(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Limit(long count) => Limit(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Limit(long count) => Limit(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Limit(long count) => Limit(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Limit(long count) => Limit(count);

        TTargetQuery IEdgeGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<TElement>.Local<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery.Local<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Local<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexGremlinQuery.Local<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery.Local<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery.Local<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Local<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Local<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Local<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Local<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Local<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.Local<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery.Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Map<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<TElement>.Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVertexGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEdgeGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery<TElement>.Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVertexGremlinQuery.Map<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery.Map<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEdgeGremlinQuery.Map<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Map<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Map<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Map<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Map<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<Property<TElement>>.Map<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> mapping) => Map(mapping);

        IVertexPropertyGremlinQuery<TElement, TNewMeta> IVertexPropertyGremlinQuery<TElement>.Meta<TNewMeta>() => Cast<TElement, Unit, Unit, TNewMeta>();

        IGraphModel IGremlinQueryAdmin.Model => _model;

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Not(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Not(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Not(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Not(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery IGremlinQuery.Not(Func<IGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Not(Func<IElementGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery IElementGremlinQuery.Not(Func<IElementGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVertexGremlinQuery IVertexGremlinQuery.Not(Func<IVertexGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Not(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEdgeGremlinQuery IEdgeGremlinQuery.Not(Func<IEdgeGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Not(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Not(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Not(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Not(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Not(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Not(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Not(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Not(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Not(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVertexGremlinQuery<TTarget> IVertexGremlinQuery.OfType<TTarget>() => OfType<TTarget>(_model.VerticesModel);

        IEdgeGremlinQuery<TTarget, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEdgeGremlinQuery<TTarget> IEdgeGremlinQuery.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEdgeGremlinQuery<TTarget, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IOutEdgeGremlinQuery<TTarget, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IInEdgeGremlinQuery<TTarget, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEdgeGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IVertexGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(_model.VerticesModel);

        TTargetQuery IGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Optional<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Optional<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.Optional<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery.Optional<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery.Optional<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVertexGremlinQuery.Optional<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEdgeGremlinQuery.Optional<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Optional<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Optional<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Optional<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Optional<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Optional<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Optional<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        IGremlinQuery IGremlinQuery.Or(params Func<IGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Or(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery IElementGremlinQuery.Or(params Func<IElementGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery IVertexGremlinQuery.Or(params Func<IVertexGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Or(params Func<IEdgeGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery IEdgeGremlinQuery.Or(params Func<IEdgeGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Or(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Or(params Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Or(params Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Or(params Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Or(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Or(params Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Or(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Or(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Or(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Or(params Func<IVertexGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Or(params Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Or(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Or(params Func<IGremlinQuery<Property<TElement>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderBy(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderBy(Expression<Func<Property<TElement>, object>> projection) => Cast<Property<TElement>>().OrderBy(projection, Order.Increasing);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderBy(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderBy(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderBy(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderBy(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderBy(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderBy(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversal) => Cast<Property<TElement>>().OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderBy(string lambda) => Cast<VertexProperty<TElement>>().OrderBy(lambda);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(string lambda) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(lambda);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderBy(string lambda) => Cast<Property<TElement>>().OrderBy(lambda);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderBy(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.OrderBy(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.OrderBy(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderBy(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Increasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.OrderBy(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderBy(string lambda) => Cast<VertexProperty<TElement>>().OrderBy(lambda);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(string lambda) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(lambda);

        IOrderedEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);


        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Decreasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderByDescending(Expression<Func<Property<TElement>, object>> projection) => Cast<Property<TElement>>().OrderBy(projection, Order.Decreasing);


        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.OrderByDescending(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.OrderByDescending(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.OrderByDescending(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Decreasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderByDescending(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversal) => Cast<Property<TElement>>().OrderBy(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderByDescending(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.OrderByDescending(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.OrderByDescending(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.OrderByDescending(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.OtherV() => AddStep<IVertex>(OtherVStep.Instance);

        IVertexGremlinQuery<IVertex> IVertexGremlinQuery.Out<TNewEdge>() => AddStep<IVertex, Unit, Unit, Unit>(new OutStep(_model.EdgesModel.GetValidFilterLabels(typeof(TNewEdge))));

        IOutEdgeGremlinQuery<TNewEdge, TElement> IVertexGremlinQuery<TElement>.OutE<TNewEdge>() => AddStep<TNewEdge, TElement, Unit, Unit>(new OutEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TNewEdge))));

        IVertexGremlinQuery<IVertex> IEdgeGremlinQuery.OutV() => AddStep<IVertex, Unit, Unit, Unit>(OutVStep.Instance);

        IVertexGremlinQuery<TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit>(OutVStep.Instance);

        IVertexGremlinQuery<TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit>(OutVStep.Instance);

        IGremlinQuery<string> IGremlinQuery.Profile() => AddStep<string>(ProfileStep.Instance);

        IVertexPropertyGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Properties<TElement, TTarget, TTarget, Unit>(projections);

        IVertexPropertyGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Properties<TElement, TTarget[], TTarget, Unit>(projections);

        IVertexPropertyGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>>>[] projections) => Properties<TElement, VertexProperty<TTarget>, TTarget, Unit>(projections);

        IVertexPropertyGremlinQuery<TTarget, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TTarget, TNewMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TNewMeta>>>[] projections) => Properties<TElement, VertexProperty<TTarget, TNewMeta>, TTarget, TNewMeta>(projections);

        IVertexPropertyGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => Properties<TElement, VertexProperty<TTarget>[], TTarget, Unit>(projections);

        IVertexPropertyGremlinQuery<TTarget, TNewMeta> IVertexGremlinQuery<TElement>.Properties<TTarget, TNewMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TNewMeta>[]>>[] projections) => Properties<TElement, VertexProperty<TTarget, TNewMeta>[], TTarget, TNewMeta>(projections);

        IGremlinQuery<Property<TTarget>> IVertexPropertyGremlinQuery<TElement, TMeta>.Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => Properties<TMeta, TTarget, Property<TTarget>, Unit>(projections);

        IEdgePropertyGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Properties<TElement, TTarget, TTarget, Unit>(projections);

        IEdgePropertyGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, Property<TTarget>>>[] projections) => Properties<TElement, Property<TTarget>, TTarget, Unit>(projections);

        IEdgePropertyGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Properties<TElement, TTarget[], TTarget, Unit>(projections);

        IEdgePropertyGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, Property<TTarget>[]>>[] projections) => Properties<TElement, Property<TTarget>[], TTarget, Unit>(projections);

        IGremlinQuery<Property<object>> IVertexPropertyGremlinQuery<TElement>.Properties(params string[] keys) => Properties(keys);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.Edge, value);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.Edge, value);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Property(string key, [AllowNull] object value)
        {
            if (value == null)
            {
                return SideEffect(_ => _
                    .Properties(key)
                    .Drop());
            }

            return AddStep(new MetaPropertyStep(key, value));
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IGremlinQuery IGremlinQuery.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IVertexGremlinQuery IVertexGremlinQuery.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery IElementGremlinQuery.Range(long low, long high) => Range(low, high);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Range(long low, long high) => Range(low, high);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery IEdgeGremlinQuery.Range(long low, long high) => Range(low, high);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Range(long low, long high) => Range(low, high);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Range(long low, long high) => Range(low, high);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Range(long low, long high) => Range(low, high);

        TTargetQuery IEdgeGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery.Repeat<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery.Repeat<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVertexGremlinQuery.Repeat<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEdgeGremlinQuery.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Repeat<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Repeat<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Repeat<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Repeat<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Repeat<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Repeat<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.Repeat<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery.RepeatUntil<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVertexGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IVertexGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IVertexGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVertexGremlinQuery.RepeatUntil<TTargetQuery>(Func<IVertexGremlinQuery, TTargetQuery> repeatTraversal, Func<IVertexGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEdgeGremlinQuery.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.RepeatUntil<TTargetQuery>(Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery> repeatTraversal, Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.RepeatUntil<TTargetQuery>(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal, Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.RepeatUntil<TTargetQuery>(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal, Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.RepeatUntil<TTargetQuery>(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery> repeatTraversal, Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TQuery IGremlinQuery.Select<TQuery, TStepElement>(StepLabel<TQuery, TStepElement> label)
        {
            return this
                .Select<TStepElement, Unit, Unit>(label)
                .ChangeQueryType<TQuery>();
        }

        IGremlinQuery<TStep> IGremlinQuery.Select<TStep>(StepLabel<TStep> label) => Select<TStep, Unit, Unit>(label);

        IGremlinQuery<(T1, T2)> IGremlinQuery.Select<T1, T2>(StepLabel<T1> label1, StepLabel<T2> label2)
        {
            return this
                .AddStep<(T1, T2), Unit, Unit, Unit>(new SelectStep(label1, label2))
                .AddStepLabelBinding(label1, x => x.Item1)
                .AddStepLabelBinding(label2, x => x.Item2);
        }

        IGremlinQuery<(T1, T2, T3)> IGremlinQuery.Select<T1, T2, T3>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3)
        {
            return this
                .AddStep<(T1, T2, T3), Unit, Unit, Unit>(new SelectStep(label1, label2, label3))
                .AddStepLabelBinding(label1, x => x.Item1)
                .AddStepLabelBinding(label2, x => x.Item2)
                .AddStepLabelBinding(label3, x => x.Item3);
        }

        IGremlinQuery<(T1, T2, T3, T4)> IGremlinQuery.Select<T1, T2, T3, T4>(StepLabel<T1> label1, StepLabel<T2> label2, StepLabel<T3> label3, StepLabel<T4> label4)
        {
            return this
                .AddStep<(T1, T2, T3, T4), Unit, Unit, Unit>(new SelectStep(label1, label2, label3, label4))
                .AddStepLabelBinding(label1, x => x.Item1)
                .AddStepLabelBinding(label2, x => x.Item2)
                .AddStepLabelBinding(label3, x => x.Item3)
                .AddStepLabelBinding(label4, x => x.Item4);
        }

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SideEffect(Func<IGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.SideEffect(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.SideEffect(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.SideEffect(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.SideEffect(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery IGremlinQuery.SideEffect(Func<IGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.SideEffect(Func<IElementGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVertexGremlinQuery IVertexGremlinQuery.SideEffect(Func<IVertexGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery IElementGremlinQuery.SideEffect(Func<IElementGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery IEdgeGremlinQuery.SideEffect(Func<IEdgeGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.SideEffect(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.SideEffect(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.SideEffect(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.SideEffect(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.SideEffect(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.SideEffect(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.SideEffect(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.SideEffect(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.SideEffect(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Skip(long count) => Skip(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Skip(long count) => Skip(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Skip(long count) => Skip(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IVertexGremlinQuery IVertexGremlinQuery.Skip(long count) => Skip(count);

        IElementGremlinQuery IElementGremlinQuery.Skip(long count) => Skip(count);

        IGremlinQuery IGremlinQuery.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Skip(long count) => Skip(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Skip(long count) => Skip(count);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery IEdgeGremlinQuery.Skip(long count) => Skip(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Skip(long count) => Skip(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Skip(long count) => Skip(count);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Skip(long count) => Skip(count);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Skip(long count) => Skip(count);

        IImmutableDictionary<StepLabel, string> IGremlinQueryAdmin.StepLabelMappings => _stepLabelMappings;
        IImmutableList<Step> IGremlinQueryAdmin.Steps => _steps;

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SumGlobal() => SumGlobal();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.SumGlobal() => SumGlobal();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.SumGlobal() => SumGlobal();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.SumGlobal() => SumGlobal();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.SumLocal() => SumLocal();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.SumLocal() => SumLocal();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.SumLocal() => SumLocal();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.SumLocal() => SumLocal();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Tail(long count) => Tail(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Tail(long count) => Tail(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Tail(long count) => Tail(count);

        IEdgeGremlinQuery IEdgeGremlinQuery.Tail(long count) => Tail(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Tail(long count) => Tail(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Tail(long count) => Tail(count);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Tail(long count) => Tail(count);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Tail(long count) => Tail(count);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IVertexGremlinQuery IVertexGremlinQuery.Tail(long count) => Tail(count);

        IElementGremlinQuery IElementGremlinQuery.Tail(long count) => Tail(count);

        IGremlinQuery IGremlinQuery.Tail(long count) => Tail(count);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Tail(long count) => Tail(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Tail(long count) => Tail(count);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Tail(long count) => Tail(count);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenBy(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenBy(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenBy(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IOrderedVertexPropertyGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IOrderedVertexPropertyGremlinQuery<TElement>.ThenBy(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IOrderedVertexPropertyGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.ThenBy(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.ThenBy(string lambda) => By(lambda);

        IOrderedEdgePropertyGremlinQuery<TElement> IOrderedEdgePropertyGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IOrderedEdgePropertyGremlinQuery<TElement>.ThenBy(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IOrderedEdgePropertyGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenBy(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.ThenByDescending(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.ThenByDescending(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.ThenByDescending(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IOrderedVertexPropertyGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement> IOrderedVertexPropertyGremlinQuery<TElement>.ThenByDescending(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVertexPropertyGremlinQuery<TElement, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.ThenByDescending(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IOrderedEdgePropertyGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEdgePropertyGremlinQuery<TElement> IOrderedEdgePropertyGremlinQuery<TElement>.ThenByDescending(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenByDescending(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count) => Times(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Times(int count) => Times(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Times(int count) => Times(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Times(int count) => Times(count);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Times(int count) => Times(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Times(int count) => Times(count);

        IEdgeGremlinQuery IEdgeGremlinQuery.Times(int count) => Times(count);

        IElementGremlinQuery IElementGremlinQuery.Times(int count) => Times(count);

        IGremlinQuery IGremlinQuery.Times(int count) => Times(count);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Times(int count) => Times(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Times(int count) => Times(count);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Times(int count) => Times(count);

        IVertexGremlinQuery IVertexGremlinQuery.Times(int count) => Times(count);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Times(int count) => Times(count);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Times(int count) => Times(count);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Times(int count) => Times(count);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Times(int count) => Times(count);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Times(int count) => Times(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Times(int count) => Times(count);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Times(int count) => Times(count);

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, Unit, TNewInVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IEdgeGremlinQuery<TElement, TOutVertex, TTargetVertex> IEdgeGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVertexGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit>())));

        IInEdgeGremlinQuery<TElement, TNewInVertex> IEdgeGremlinQuery<TElement>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, Unit, TNewInVertex>(toVertexTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, TOutVertex, TNewInVertex>(toVertexTraversal);

        IGremlinQuery<TItem> IGremlinQuery<TElement>.Unfold<TItem>() => Unfold<TItem>();

        IGremlinQuery<TItem> IGremlinQuery<VertexProperty<TElement>>.Unfold<TItem>() => Unfold<TItem>();

        IGremlinQuery<TItem> IGremlinQuery<VertexProperty<TElement, TMeta>>.Unfold<TItem>() => Unfold<TItem>();

        IGremlinQuery<TItem> IGremlinQuery<Property<TElement>>.Unfold<TItem>() => Unfold<TItem>();

        TTargetQuery IGremlinQuery.Union<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery.Union<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVertexGremlinQuery.Union<TTargetQuery>(params Func<IVertexGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEdgeGremlinQuery.Union<TTargetQuery>(params Func<IEdgeGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEdgePropertyGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IEdgePropertyGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<Property<TElement>>.Union<TTargetQuery>(params Func<IGremlinQuery<Property<TElement>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVertexPropertyGremlinQuery<TElement, TMeta>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement, TMeta>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Union<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Union<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVertexPropertyGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVertexPropertyGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Union<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Union<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IOutEdgeGremlinQuery<TElement, TOutVertex>.Union<TTargetQuery>(params Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IInEdgeGremlinQuery<TElement, TInVertex>.Union<TTargetQuery>(params Func<IInEdgeGremlinQuery<TElement, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEdgeGremlinQuery<TElement, TOutVertex>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEdgeGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IEdgeGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVertexGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVertexGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);


        IVertexGremlinQuery<TVertex> IGremlinQuerySource.V<TVertex>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit>(new VStep(ids)).OfType<TVertex>(_model.VerticesModel, true);

        IVertexGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids) => AddStep<IVertex, Unit, Unit, Unit>(new VStep(ids));

        IGremlinQuery<IDictionary<string, object>> IVertexPropertyGremlinQuery<TElement>.ValueMap() => ValueMap<IDictionary<string, object>>();

        IGremlinQuery<TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.ValueMap() => ValueMap<TMeta>();

        IGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Values<TElement, TTarget, TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Values<TElement, TTarget, TTarget>(GraphElementType.Edge, projections);

        IGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Values<TElement, VertexProperty<TValue, TNewMeta>, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IVertexGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => Values<TElement, VertexProperty<TValue>, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IEdgeGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Values<TElement, Property<TValue>, TValue>(GraphElementType.Edge, projections);

        IGremlinQuery<TTarget> IVertexPropertyGremlinQuery<TElement, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => Values<TMeta, TTarget, TTarget>(GraphElementType.VertexProperty, projections);

        IGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Values<TElement, TTarget[], TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Values<TElement, TTarget[], TTarget>(GraphElementType.Edge, projections);

        IGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => Values<TElement, VertexProperty<TTarget>[], TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IVertexGremlinQuery<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => Values<TElement, VertexProperty<TTarget, TTargetMeta>[], TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IEdgeGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, Property<TTarget>[]>>[] projections) => Values<TElement, Property<TTarget>[], TTarget>(GraphElementType.Edge, projections);

        IGremlinQuery<object> IVertexPropertyGremlinQuery<TElement>.Values(params string[] keys) => AddStep<object, Unit, Unit, Unit>(new ValuesStep(keys));


        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.None, predicate);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Where(Expression<Func<VertexProperty<TElement>, bool>> predicate) => Cast<VertexProperty<TElement>>().Where(GraphElementType.VertexProperty, predicate);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Where(Expression<Func<VertexProperty<TElement, TMeta>, bool>> predicate) => Cast<VertexProperty<TElement, TMeta>>().Where(GraphElementType.VertexProperty, predicate);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Where(Expression<Func<Property<TElement>, bool>> predicate) => Cast<Property<TElement>>().Where(GraphElementType.None, predicate);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(Func<IEdgeGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(Func<IVertexGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(Func<IInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Where(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Where(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Where(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.Where(Func<IOrderedGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexGremlinQuery<TElement> IOrderedVertexGremlinQuery<TElement>.Where(Func<IOrderedVertexGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexPropertyGremlinQuery<TElement> IVertexPropertyGremlinQuery<TElement>.Where(Func<IVertexPropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Where(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexPropertyGremlinQuery<TElement, TMeta> IVertexPropertyGremlinQuery<TElement, TMeta>.Where(Func<IVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Where(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TElement> IOrderedEdgeGremlinQuery<TElement>.Where(Func<IOrderedEdgeGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IOrderedInEdgeGremlinQuery<TElement, TInVertex>.Where(Func<IOrderedInEdgeGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOrderedOutEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgePropertyGremlinQuery<TElement> IEdgePropertyGremlinQuery<TElement>.Where(Func<IEdgePropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexPropertyGremlinQuery<TElement> IOrderedVertexPropertyGremlinQuery<TElement>.Where(Func<IOrderedVertexPropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexPropertyGremlinQuery<TElement, TMeta> IOrderedVertexPropertyGremlinQuery<TElement, TMeta>.Where(Func<IOrderedVertexPropertyGremlinQuery<TElement, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgePropertyGremlinQuery<TElement> IOrderedEdgePropertyGremlinQuery<TElement>.Where(Func<IOrderedEdgePropertyGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Vertex, predicate);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEdgeGremlinQuery<TElement> IEdgeGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Vertex, projection, propertyTraversal);

        IVertexGremlinQuery<TElement> IVertexGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex> IOrderedEdgeGremlinQuery<TElement, TOutVertex>.Where(Func<IOrderedEdgeGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.Where(Func<IOrderedElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex, TInVertex> IEdgeGremlinQuery<TElement, TOutVertex, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IEdgeGremlinQuery<TElement, TOutVertex> IEdgeGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IOutEdgeGremlinQuery<TElement, TOutVertex> IOutEdgeGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IInEdgeGremlinQuery<TElement, TInVertex> IInEdgeGremlinQuery<TElement, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);
    }
}
