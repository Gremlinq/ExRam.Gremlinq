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

        IOrderedVGremlinQuery<TElement>,
        IOrderedEGremlinQuery<TElement>,
        IOrderedEGremlinQuery<TElement, TOutVertex>,
        IOrderedInEGremlinQuery<TElement, TInVertex>,
        IOrderedOutEGremlinQuery<TElement, TOutVertex>,
        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>,

        IOrderedVPropertiesGremlinQuery<TElement>,
        IOrderedVPropertiesGremlinQuery<TElement, TMeta>,
        IOrderedEPropertiesGremlinQuery<TElement>
    {
        IEGremlinQuery<TEdge> IGremlinQuerySource.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.AddE<TEdge>(TEdge edge) => AddE(edge);

        IEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.AddE<TEdge>() => AddE(new TEdge());

        IVGremlinQuery<TVertex> IGremlinQuerySource.AddV<TVertex>(TVertex vertex) => AddV(vertex);

        TTargetQuery IGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedVGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedVGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedEGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedInEGremlinQuery<TElement, TInVertex>.Aggregate<TTargetQuery>(Func<IOrderedInEGremlinQuery<TElement, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedOutEGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOrderedOutEGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.Aggregate<TTargetQuery>(Func<IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, StepLabel<VertexProperty<TElement>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, StepLabel<VertexProperty<TElement>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Aggregate<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<VertexProperty<TElement, TMeta>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<VertexProperty<TElement, TMeta>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IGremlinQuery<Property<TElement>>.Aggregate<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, StepLabel<Property<TElement>[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedVPropertiesGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedVPropertiesGremlinQuery<TElement, TMeta>.Aggregate<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TElement, TMeta>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEPropertiesGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedEPropertiesGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedEGremlinQuery<TElement, TOutVertex>.Aggregate<TTargetQuery>(Func<IOrderedEGremlinQuery<TElement, TOutVertex>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        TTargetQuery IOrderedElementGremlinQuery<TElement>.Aggregate<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<TElement[]>, TTargetQuery> continuation) => Aggregate(continuation);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.And(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.And(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery IGremlinQuery.And(params Func<IGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVGremlinQuery IVGremlinQuery.And(params Func<IVGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery IElementGremlinQuery.And(params Func<IElementGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.And(params Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.And(params Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.And(params Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.And(params Func<IEGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEGremlinQuery IEGremlinQuery.And(params Func<IEGremlinQuery, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.And(params Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.And(params Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.And(params Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.And(params Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.And(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.And(params Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.And(params Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.And(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.And(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.And(params Func<IGremlinQuery<Property<TElement>>, IGremlinQuery>[] andTraversals) => And(andTraversals);

        TTargetQuery IVGremlinQuery<TElement>.As<TTargetQuery>(Func<IVGremlinQuery<TElement>, StepLabel<IVGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement>.As<TTargetQuery>(Func<IEGremlinQuery<TElement>, StepLabel<IEGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, StepLabel<IOutEGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, StepLabel<IEGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedGremlinQuery<TElement>, StepLabel<IOrderedGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<TElement>.As<TTargetQuery>(Func<IGremlinQuery<TElement>, StepLabel<IGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedVGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedVGremlinQuery<TElement>, StepLabel<IOrderedVGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IElementGremlinQuery<TElement>, StepLabel<IElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.As<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, StepLabel<IVPropertiesGremlinQuery<TElement, TMeta>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.As<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, StepLabel<IVPropertiesGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.As<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, StepLabel<IEPropertiesGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedEGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedEGremlinQuery<TElement>, StepLabel<IOrderedEGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedInEGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IOrderedInEGremlinQuery<TElement, TInVertex>, StepLabel<IOrderedInEGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.As<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, StepLabel<IInEGremlinQuery<TElement, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedOutEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOrderedOutEGremlinQuery<TElement, TOutVertex>, StepLabel<IOrderedOutEGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.As<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, StepLabel<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.As<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, StepLabel<IElementGremlinQuery<VertexProperty<TElement>>, VertexProperty<TElement>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.As<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, StepLabel<IGremlinQuery<VertexProperty<TElement>>, VertexProperty<TElement>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.As<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, VertexProperty<TElement, TMeta>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.As<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, StepLabel<IGremlinQuery<VertexProperty<TElement, TMeta>>, VertexProperty<TElement, TMeta>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IGremlinQuery<Property<TElement>>.As<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, StepLabel<IGremlinQuery<Property<TElement>>, Property<TElement>>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedVPropertiesGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TElement>, StepLabel<IOrderedVPropertiesGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedVPropertiesGremlinQuery<TElement, TMeta>.As<TTargetQuery>(Func<IOrderedVPropertiesGremlinQuery<TElement, TMeta>, StepLabel<IOrderedVPropertiesGremlinQuery<TElement, TMeta>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedEPropertiesGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedEPropertiesGremlinQuery<TElement>, StepLabel<IOrderedEPropertiesGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery IGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IVGremlinQuery IVGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery IElementGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.As(StepLabel stepLabel) => As(stepLabel);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IEGremlinQuery IEGremlinQuery.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.As(StepLabel stepLabel) => As(stepLabel);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.As(StepLabel stepLabel) => As(stepLabel);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.As(StepLabel stepLabel) => As(stepLabel);

        TTargetQuery IOrderedEGremlinQuery<TElement, TOutVertex>.As<TTargetQuery>(Func<IOrderedEGremlinQuery<TElement, TOutVertex>, StepLabel<IOrderedEGremlinQuery<TElement, TOutVertex>, TElement>, TTargetQuery> continuation) => As(continuation);

        TTargetQuery IOrderedElementGremlinQuery<TElement>.As<TTargetQuery>(Func<IOrderedElementGremlinQuery<TElement>, StepLabel<IOrderedElementGremlinQuery<TElement>, TElement>, TTargetQuery> continuation) => As(continuation);

        IGremlinQueryAdmin IGremlinQuery.AsAdmin() => this;

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Barrier() => Barrier();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Barrier() => Barrier();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Barrier() => Barrier();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Barrier() => Barrier();

        IGremlinQuery IGremlinQuery.Barrier() => Barrier();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Barrier() => Barrier();

        IElementGremlinQuery IElementGremlinQuery.Barrier() => Barrier();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Barrier() => Barrier();

        IVGremlinQuery IVGremlinQuery.Barrier() => Barrier();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Barrier() => Barrier();

        IEGremlinQuery IEGremlinQuery.Barrier() => Barrier();

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Barrier() => Barrier();

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Barrier() => Barrier();

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Barrier() => Barrier();

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Barrier() => Barrier();

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Barrier() => Barrier();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Barrier() => Barrier();

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Barrier() => Barrier();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Barrier() => Barrier();

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Barrier() => Barrier();

        IVGremlinQuery<IVertex> IVGremlinQuery.Both<TEdge>() => AddStep<IVertex>(new BothStep(_model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));

        IEGremlinQuery<TEdge> IVGremlinQuery.BothE<TEdge>() => AddStep<TEdge>(new BothEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IVGremlinQuery<IVertex> IEGremlinQuery.BothV() => AddStep<IVertex>(BothVStep.Instance);

        IGremlinQuery<TResult> IGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IVGremlinQuery<TResult> IVGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IEGremlinQuery<TResult, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IEGremlinQuery<TResult> IEGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IEGremlinQuery<TResult, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IOutEGremlinQuery<TResult, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IInEGremlinQuery<TResult, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedEGremlinQuery<TResult, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IEGremlinQuery<TResult> IEGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery.Cast<TResult>() => Cast<TResult>();

        IOrderedOutEGremlinQuery<TResult, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedInEGremlinQuery<TResult, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedEGremlinQuery<TResult> IOrderedEGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IEPropertiesGremlinQuery<TResult> IEPropertiesGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IVPropertiesGremlinQuery<TResult, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Cast<TResult>() => Cast<TResult>();

        IVPropertiesGremlinQuery<TResult> IVPropertiesGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedVGremlinQuery<TResult> IOrderedVGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IVGremlinQuery<TResult> IVGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedGremlinQuery<TResult> IOrderedGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery<VertexProperty<TElement>>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<VertexProperty<TElement>>.Cast<TResult>() => Cast<TResult>();

        IElementGremlinQuery<TResult> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<VertexProperty<TElement, TMeta>>.Cast<TResult>() => Cast<TResult>();

        IGremlinQuery<TResult> IGremlinQuery<Property<TElement>>.Cast<TResult>() => Cast<TResult>();

        IOrderedVPropertiesGremlinQuery<TResult> IOrderedVPropertiesGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedVPropertiesGremlinQuery<TResult, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.Cast<TResult>() => Cast<TResult>();

        IOrderedEPropertiesGremlinQuery<TResult> IOrderedEPropertiesGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        IOrderedEGremlinQuery<TResult, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.Cast<TResult>() => Cast<TResult>();

        IOrderedElementGremlinQuery<TResult> IOrderedElementGremlinQuery<TElement>.Cast<TResult>() => Cast<TResult>();

        TTargetQuery IGremlinQueryAdmin.ChangeQueryType<TTargetQuery>() => ChangeQueryType<TTargetQuery>();

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<Property<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<Property<TElement>>, TTargetQuery> trueChoice, Func<IGremlinQuery<Property<TElement>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<Property<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<Property<TElement>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> trueChoice, Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice, Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Choose<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice, Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Choose<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice, Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Choose<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversalPredicate, Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice, Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Choose<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IEGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEGremlinQuery.Choose<TTargetQuery>(Func<IEGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery, TTargetQuery> trueChoice, Func<IEGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IEGremlinQuery.Choose<TTargetQuery>(Func<IEGremlinQuery, IGremlinQuery> traversalPredicate, Func<IEGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IVGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IVGremlinQuery.Choose<TTargetQuery>(Func<IVGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery, TTargetQuery> trueChoice, Func<IVGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IVGremlinQuery.Choose<TTargetQuery>(Func<IVGremlinQuery, IGremlinQuery> traversalPredicate, Func<IVGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IElementGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice, Func<IElementGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IElementGremlinQuery.Choose<TTargetQuery>(Func<IElementGremlinQuery, IGremlinQuery> traversalPredicate, Func<IElementGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice, Func<IGremlinQuery<TElement>, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery<TElement>.Choose<TTargetQuery>(Func<IGremlinQuery<TElement>, IGremlinQuery> traversalPredicate, Func<IGremlinQuery<TElement>, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice, Func<IGremlinQuery, TTargetQuery> falseChoice) => Choose(traversalPredicate, trueChoice, falseChoice);

        TTargetQuery IGremlinQuery.Choose<TTargetQuery>(Func<IGremlinQuery, IGremlinQuery> traversalPredicate, Func<IGremlinQuery, TTargetQuery> trueChoice) => Choose(traversalPredicate, trueChoice);

        TTargetQuery IEGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IEGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEGremlinQuery.Coalesce<TTargetQuery>(params Func<IEGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery.Coalesce<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Coalesce<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Coalesce<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Coalesce<TTargetQuery>(params Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Coalesce<TTargetQuery>(params Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Coalesce<TTargetQuery>(params Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVGremlinQuery.Coalesce<TTargetQuery>(params Func<IVGremlinQuery, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IVGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IVGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<TElement>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        TTargetQuery IGremlinQuery<Property<TElement>>.Coalesce<TTargetQuery>(params Func<IGremlinQuery<Property<TElement>>, TTargetQuery>[] traversals) => Coalesce(traversals);

        IGremlinQuery<long> IGremlinQuery.Count() => AddStep<long, Unit, Unit, Unit>(CountStep.Instance);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Dedup() => Dedup();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Dedup() => Dedup();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Dedup() => Dedup();

        IGremlinQuery IGremlinQuery.Dedup() => Dedup();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Dedup() => Dedup();

        IVGremlinQuery IVGremlinQuery.Dedup() => Dedup();

        IElementGremlinQuery IElementGremlinQuery.Dedup() => Dedup();

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Dedup() => Dedup();

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Dedup() => Dedup();

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Dedup() => Dedup();

        IEGremlinQuery IEGremlinQuery.Dedup() => Dedup();

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Dedup() => Dedup();

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Dedup() => Dedup();

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Dedup() => Dedup();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Dedup() => Dedup();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Dedup() => Dedup();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Dedup() => Dedup();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Dedup() => Dedup();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Dedup() => Dedup();

        IGremlinQuery<Unit> IGremlinQuery.Drop() => Drop();

        IEGremlinQuery<TEdge> IGremlinQuerySource.E<TEdge>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit>(new EStep(ids)).OfType<TEdge>(_model.EdgesModel, true);

        IEGremlinQuery<IEdge> IGremlinQuerySource.E(params object[] ids) => AddStep<IEdge, Unit, Unit, Unit>(new EStep(ids));

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Emit() => Emit();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Emit() => Emit();

        IGremlinQuery IGremlinQuery.Emit() => Emit();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Emit() => Emit();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Emit() => Emit();

        IVGremlinQuery IVGremlinQuery.Emit() => Emit();

        IElementGremlinQuery IElementGremlinQuery.Emit() => Emit();

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Emit() => Emit();

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Emit() => Emit();

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Emit() => Emit();

        IEGremlinQuery IEGremlinQuery.Emit() => Emit();

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Emit() => Emit();

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Emit() => Emit();

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Emit() => Emit();

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

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IEGremlinQuery IEGremlinQuery.Filter(string lambda) => Filter(lambda);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Filter(string lambda) => Filter(lambda);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Filter(string lambda) => Filter(lambda);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Filter(string lambda) => Filter(lambda);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Filter(string lambda) => Filter(lambda);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IVGremlinQuery IVGremlinQuery.Filter(string lambda) => Filter(lambda);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Filter(string lambda) => Filter(lambda);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Filter(string lambda) => Filter(lambda);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Filter(string lambda) => Filter(lambda);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Filter(string lambda) => Filter(lambda);

        IGremlinQuery<TElement[]> IGremlinQuery<TElement>.Fold() => Fold<TElement[]>();

        IGremlinQuery<VertexProperty<TElement>[]> IGremlinQuery<VertexProperty<TElement>>.Fold() => Fold<VertexProperty<TElement>[]>();

        IGremlinQuery<VertexProperty<TElement, TMeta>[]> IGremlinQuery<VertexProperty<TElement, TMeta>>.Fold() => Fold<VertexProperty<TElement, TMeta>[]>();

        IGremlinQuery<Property<TElement>[]> IGremlinQuery<Property<TElement>>.Fold() => Fold<Property<TElement>[]>();

        IOutEGremlinQuery<TElement, TNewOutVertex> IEGremlinQuery<TElement>.From<TNewOutVertex>(StepLabel<TNewOutVertex> stepLabel) => AddStep<TElement, TNewOutVertex, Unit, Unit>(new FromLabelStep(stepLabel));

        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => AddStep<TElement, TTargetVertex, TOutVertex, Unit>(new FromLabelStep(stepLabel));

        IEGremlinQuery<TElement, TTargetVertex, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.From<TTargetVertex>(Func<IVGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal) => AddStep<TElement, TTargetVertex, TOutVertex, Unit>(new FromTraversalStep(fromVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit>())));

        IOutEGremlinQuery<TElement, TNewOutVertex> IEGremlinQuery<TElement>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, Unit>(fromVertexTraversal);

        IEGremlinQuery<TElement, TNewOutVertex, TInVertex> IInEGremlinQuery<TElement, TInVertex>.From<TNewOutVertex>(Func<IGremlinQuery, IGremlinQuery<TNewOutVertex>> fromVertexTraversal) => From<TElement, TNewOutVertex, TInVertex>(fromVertexTraversal);

        IAsyncEnumerator<TElement> IAsyncEnumerable<TElement>.GetEnumerator() => GetEnumerator<TElement>();

        IAsyncEnumerator<VertexProperty<TElement>> IAsyncEnumerable<VertexProperty<TElement>>.GetEnumerator() => GetEnumerator<VertexProperty<TElement>>();

        IAsyncEnumerator<VertexProperty<TElement, TMeta>> IAsyncEnumerable<VertexProperty<TElement, TMeta>>.GetEnumerator() => GetEnumerator<VertexProperty<TElement, TMeta>>();

        IAsyncEnumerator<Property<TElement>> IAsyncEnumerable<Property<TElement>>.GetEnumerator() => GetEnumerator<Property<TElement>>();

        IGremlinQuery<object> IElementGremlinQuery.Id() => Id();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Identity() => Identity();

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Identity() => Identity();

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Identity() => Identity();

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Identity() => Identity();

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Identity() => Identity();

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Identity() => Identity();

        IGremlinQuery IGremlinQuery.Identity() => Identity();

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Identity() => Identity();

        IVGremlinQuery IVGremlinQuery.Identity() => Identity();

        IElementGremlinQuery IElementGremlinQuery.Identity() => Identity();

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Identity() => Identity();

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Identity() => Identity();

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Identity() => Identity();

        IEGremlinQuery IEGremlinQuery.Identity() => Identity();

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Identity() => Identity();

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Identity() => Identity();

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Identity() => Identity();

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Identity() => Identity();

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Identity() => Identity();

        IVGremlinQuery<IVertex> IVGremlinQuery.In<TEdge>() => AddStep<IVertex>(new InStep(_model.VerticesModel.GetValidFilterLabels(typeof(TEdge))));

        IInEGremlinQuery<TEdge, TElement> IVGremlinQuery<TElement>.InE<TEdge>() => AddStep<TEdge, Unit, TElement, Unit>(new InEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TEdge))));

        IGremlinQuery<TNewElement> IGremlinQuerySource.Inject<TNewElement>(params TNewElement[] elements) => Inject(elements);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Inject(params TElement[] elements) => Inject(elements);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Inject(params VertexProperty<TElement>[] elements) => Inject(elements);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Inject(params VertexProperty<TElement, TMeta>[] elements) => Inject(elements);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Inject(params Property<TElement>[] elements) => Inject(elements);

        IGremlinQuery IGremlinQueryAdmin.InsertStep(int index, Step step) => new GremlinQueryImpl<TElement, TOutVertex, TInVertex, TMeta>(_model, _queryExecutor, _steps.Insert(index, step), _stepLabelMappings, _logger);

        IVGremlinQuery<IVertex> IEGremlinQuery.InV() => InV<IVertex>();

        IVGremlinQuery<TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.InV() => InV<TInVertex>();

        IVGremlinQuery<TInVertex> IInEGremlinQuery<TElement, TInVertex>.InV() => InV<TInVertex>();

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IGremlinQuery IGremlinQuery.Limit(long count) => Limit(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IVGremlinQuery IVGremlinQuery.Limit(long count) => Limit(count);

        IElementGremlinQuery IElementGremlinQuery.Limit(long count) => Limit(count);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Limit(long count) => Limit(count);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Limit(long count) => Limit(count);

        IEGremlinQuery IEGremlinQuery.Limit(long count) => Limit(count);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Limit(long count) => Limit(count);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Limit(long count) => Limit(count);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Limit(long count) => Limit(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Limit(long count) => Limit(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Limit(long count) => Limit(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Limit(long count) => Limit(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Limit(long count) => Limit(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Limit(long count) => Limit(count);

        TTargetQuery IEGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<TElement>.Local<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery.Local<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Local<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVGremlinQuery.Local<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery.Local<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Local<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Local<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Local<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEGremlinQuery.Local<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Local<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Local<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Local<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Local<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Local<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Local<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Local<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.Local<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> localTraversal) => Local(localTraversal);

        TTargetQuery IGremlinQuery.Map<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Map<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Map<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Map<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<TElement>.Map<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery<TElement>.Map<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVGremlinQuery.Map<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery.Map<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Map<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Map<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Map<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IEGremlinQuery.Map<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Map<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Map<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Map<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Map<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> mapping) => Map(mapping);

        TTargetQuery IGremlinQuery<Property<TElement>>.Map<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> mapping) => Map(mapping);

        IVPropertiesGremlinQuery<TElement, TNewMeta> IVPropertiesGremlinQuery<TElement>.Meta<TNewMeta>() => Cast<TElement, Unit, Unit, TNewMeta>();

        IGraphModel IGremlinQueryAdmin.Model => _model;

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Not(Func<IGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Not(Func<IVGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Not(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Not(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Not(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IGremlinQuery IGremlinQuery.Not(Func<IGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Not(Func<IElementGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery IElementGremlinQuery.Not(Func<IElementGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVGremlinQuery IVGremlinQuery.Not(Func<IVGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Not(Func<IEGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEGremlinQuery IEGremlinQuery.Not(Func<IEGremlinQuery, IGremlinQuery> notTraversal) => Not(notTraversal);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Not(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Not(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Not(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Not(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Not(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Not(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Not(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Not(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Not(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> notTraversal) => Not(notTraversal);

        IVGremlinQuery<TTarget> IVGremlinQuery.OfType<TTarget>() => OfType<TTarget>(_model.VerticesModel);

        IEGremlinQuery<TTarget, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEGremlinQuery<TTarget> IEGremlinQuery.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEGremlinQuery<TTarget, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IOutEGremlinQuery<TTarget, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IInEGremlinQuery<TTarget, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IEGremlinQuery<TTarget> IEGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(_model.EdgesModel);

        IVGremlinQuery<TTarget> IVGremlinQuery<TElement>.OfType<TTarget>() => OfType<TTarget>(_model.VerticesModel);

        TTargetQuery IGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Optional<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Optional<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.Optional<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IGremlinQuery.Optional<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery.Optional<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVGremlinQuery.Optional<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEGremlinQuery.Optional<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Optional<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Optional<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Optional<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Optional<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Optional<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Optional<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Optional<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Optional<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> optionalTraversal) => Optional(optionalTraversal);

        IGremlinQuery IGremlinQuery.Or(params Func<IGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Or(params Func<IElementGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery IElementGremlinQuery.Or(params Func<IElementGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVGremlinQuery IVGremlinQuery.Or(params Func<IVGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Or(params Func<IEGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEGremlinQuery IEGremlinQuery.Or(params Func<IEGremlinQuery, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Or(params Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Or(params Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Or(params Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Or(params Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Or(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Or(params Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Or(params Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Or(params Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Or(params Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Or(params Func<IGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Or(params Func<IVGremlinQuery<TElement>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Or(params Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Or(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Or(params Func<IGremlinQuery<Property<TElement>>, IGremlinQuery>[] orTraversals) => Or(orTraversals);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderBy(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderBy(Expression<Func<Property<TElement>, object>> projection) => Cast<Property<TElement>>().OrderBy(projection, Order.Increasing);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.OrderBy(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderBy(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Increasing);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderBy(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderBy(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversal) => Cast<Property<TElement>>().OrderBy(traversal, Order.Increasing);

        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderBy(string lambda) => Cast<VertexProperty<TElement>>().OrderBy(lambda);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(string lambda) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(lambda);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderBy(string lambda) => Cast<Property<TElement>>().OrderBy(lambda);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderBy(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OrderBy(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.OrderBy(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.OrderBy(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderBy(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Increasing);

        IOrderedEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.OrderBy(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderBy(string lambda) => Cast<VertexProperty<TElement>>().OrderBy(lambda);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.OrderBy(string lambda) => OrderBy(lambda);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderBy(string lambda) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(lambda);

        IOrderedEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.OrderBy(string lambda) => OrderBy(lambda);


        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Decreasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderByDescending(Expression<Func<Property<TElement>, object>> projection) => Cast<Property<TElement>>().OrderBy(projection, Order.Decreasing);


        IOrderedGremlinQuery<TElement> IGremlinQuery<TElement>.OrderByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVGremlinQuery<TElement> IVGremlinQuery<TElement>.OrderByDescending(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IEGremlinQuery<TElement>.OrderByDescending(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.OrderByDescending(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OrderByDescending(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Decreasing);

        IOrderedGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Decreasing);

        IOrderedGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.OrderByDescending(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> traversal) => Cast<Property<TElement>>().OrderBy(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Expression<Func<VertexProperty<TElement>, object>> projection) => Cast<VertexProperty<TElement>>().OrderBy(projection, Order.Decreasing);

        IOrderedEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.OrderByDescending(Expression<Func<TElement, object>> projection) => OrderBy(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Expression<Func<VertexProperty<TElement, TMeta>, object>> projection) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.OrderByDescending(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.OrderByDescending(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.OrderByDescending(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement>>().OrderBy(traversal, Order.Decreasing);

        IOrderedEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.OrderByDescending(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.OrderByDescending(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.OrderByDescending(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> traversal) => Cast<VertexProperty<TElement, TMeta>>().OrderBy(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.OrderByDescending(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => OrderBy(traversal, Order.Decreasing);

        IVGremlinQuery<IVertex> IEGremlinQuery.OtherV() => AddStep<IVertex>(OtherVStep.Instance);

        IVGremlinQuery<IVertex> IVGremlinQuery.Out<TNewEdge>() => AddStep<IVertex, Unit, Unit, Unit>(new OutStep(_model.EdgesModel.GetValidFilterLabels(typeof(TNewEdge))));

        IOutEGremlinQuery<TNewEdge, TElement> IVGremlinQuery<TElement>.OutE<TNewEdge>() => AddStep<TNewEdge, TElement, Unit, Unit>(new OutEStep(_model.EdgesModel.GetValidFilterLabels(typeof(TNewEdge))));

        IVGremlinQuery<IVertex> IEGremlinQuery.OutV() => AddStep<IVertex, Unit, Unit, Unit>(OutVStep.Instance);

        IVGremlinQuery<TOutVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit>(OutVStep.Instance);

        IVGremlinQuery<TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.OutV() => AddStep<TOutVertex, Unit, Unit, Unit>(OutVStep.Instance);

        IGremlinQuery<string> IGremlinQuery.Profile() => AddStep<string>(ProfileStep.Instance);

        IVPropertiesGremlinQuery<TTarget> IVGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Properties<TElement, TTarget, TTarget, Unit>(projections);

        IVPropertiesGremlinQuery<TTarget> IVGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Properties<TElement, TTarget[], TTarget, Unit>(projections);

        IVPropertiesGremlinQuery<TTarget> IVGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>>>[] projections) => Properties<TElement, VertexProperty<TTarget>, TTarget, Unit>(projections);

        IVPropertiesGremlinQuery<TTarget, TNewMeta> IVGremlinQuery<TElement>.Properties<TTarget, TNewMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TNewMeta>>>[] projections) => Properties<TElement, VertexProperty<TTarget, TNewMeta>, TTarget, TNewMeta>(projections);

        IVPropertiesGremlinQuery<TTarget> IVGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => Properties<TElement, VertexProperty<TTarget>[], TTarget, Unit>(projections);

        IVPropertiesGremlinQuery<TTarget, TNewMeta> IVGremlinQuery<TElement>.Properties<TTarget, TNewMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TNewMeta>[]>>[] projections) => Properties<TElement, VertexProperty<TTarget, TNewMeta>[], TTarget, TNewMeta>(projections);

        IGremlinQuery<Property<TTarget>> IVPropertiesGremlinQuery<TElement, TMeta>.Properties<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => Properties<TMeta, TTarget, Property<TTarget>, Unit>(projections);

        IEPropertiesGremlinQuery<TTarget> IEGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Properties<TElement, TTarget, TTarget, Unit>(projections);

        IEPropertiesGremlinQuery<TTarget> IEGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, Property<TTarget>>>[] projections) => Properties<TElement, Property<TTarget>, TTarget, Unit>(projections);

        IEPropertiesGremlinQuery<TTarget> IEGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Properties<TElement, TTarget[], TTarget, Unit>(projections);

        IEPropertiesGremlinQuery<TTarget> IEGremlinQuery<TElement>.Properties<TTarget>(params Expression<Func<TElement, Property<TTarget>[]>>[] projections) => Properties<TElement, Property<TTarget>[], TTarget, Unit>(projections);

        IGremlinQuery<Property<object>> IVPropertiesGremlinQuery<TElement>.Properties(params string[] keys) => Properties(keys);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.Edge, value);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, [AllowNull] TValue value) => Property(projection, GraphElementType.Edge, value);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Property<TValue>(Expression<Func<TMeta, TValue>> projection, TValue value) => Property(projection, GraphElementType.VertexProperty, value);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Property<TValue>(Expression<Func<TElement, TValue[]>> projection, TValue value) => Property(projection, GraphElementType.Edge, value);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Property(string key, [AllowNull] object value)
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

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IGremlinQuery IGremlinQuery.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IVGremlinQuery IVGremlinQuery.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery IElementGremlinQuery.Range(long low, long high) => Range(low, high);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Range(long low, long high) => Range(low, high);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Range(long low, long high) => Range(low, high);

        IEGremlinQuery IEGremlinQuery.Range(long low, long high) => Range(low, high);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Range(long low, long high) => Range(low, high);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Range(long low, long high) => Range(low, high);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Range(long low, long high) => Range(low, high);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Range(long low, long high) => Range(low, high);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Range(long low, long high) => Range(low, high);

        TTargetQuery IEGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery.Repeat<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery.Repeat<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVGremlinQuery.Repeat<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEGremlinQuery.Repeat<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Repeat<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Repeat<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Repeat<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Repeat<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Repeat<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Repeat<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Repeat<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Repeat<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Repeat<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IGremlinQuery<Property<TElement>>.Repeat<TTargetQuery>(Func<IGremlinQuery<Property<TElement>>, TTargetQuery> repeatTraversal) => Repeat(repeatTraversal);

        TTargetQuery IEGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IEGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IEGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery.RepeatUntil<TTargetQuery>(Func<IGremlinQuery, TTargetQuery> repeatTraversal, Func<IGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IVGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IVGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVGremlinQuery.RepeatUntil<TTargetQuery>(Func<IVGremlinQuery, TTargetQuery> repeatTraversal, Func<IVGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEGremlinQuery.RepeatUntil<TTargetQuery>(Func<IEGremlinQuery, TTargetQuery> repeatTraversal, Func<IEGremlinQuery, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.RepeatUntil<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal, Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.RepeatUntil<TTargetQuery>(Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery> repeatTraversal, Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.RepeatUntil<TTargetQuery>(Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery> repeatTraversal, Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.RepeatUntil<TTargetQuery>(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery> repeatTraversal, Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.RepeatUntil<TTargetQuery>(Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery> repeatTraversal, Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.RepeatUntil<TTargetQuery>(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal, Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.RepeatUntil<TTargetQuery>(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery> repeatTraversal, Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.RepeatUntil<TTargetQuery>(Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery> repeatTraversal, Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> untilTraversal) => RepeatUntil(repeatTraversal, untilTraversal);

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

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.SideEffect(Func<IVGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.SideEffect(Func<IEGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.SideEffect(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.SideEffect(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.SideEffect(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery IGremlinQuery.SideEffect(Func<IGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.SideEffect(Func<IElementGremlinQuery<TElement>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IVGremlinQuery IVGremlinQuery.SideEffect(Func<IVGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery IElementGremlinQuery.SideEffect(Func<IElementGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEGremlinQuery IEGremlinQuery.SideEffect(Func<IEGremlinQuery, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.SideEffect(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.SideEffect(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.SideEffect(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.SideEffect(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.SideEffect(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.SideEffect(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.SideEffect(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.SideEffect(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.SideEffect(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> sideEffectTraversal) => SideEffect(sideEffectTraversal);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Skip(long count) => Skip(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Skip(long count) => Skip(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Skip(long count) => Skip(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IVGremlinQuery IVGremlinQuery.Skip(long count) => Skip(count);

        IElementGremlinQuery IElementGremlinQuery.Skip(long count) => Skip(count);

        IGremlinQuery IGremlinQuery.Skip(long count) => Skip(count);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Skip(long count) => Skip(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Skip(long count) => Skip(count);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Skip(long count) => Skip(count);

        IEGremlinQuery IEGremlinQuery.Skip(long count) => Skip(count);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Skip(long count) => Skip(count);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Skip(long count) => Skip(count);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Skip(long count) => Skip(count);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Skip(long count) => Skip(count);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Skip(long count) => Skip(count);

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

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Tail(long count) => Tail(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Tail(long count) => Tail(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Tail(long count) => Tail(count);

        IEGremlinQuery IEGremlinQuery.Tail(long count) => Tail(count);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Tail(long count) => Tail(count);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Tail(long count) => Tail(count);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Tail(long count) => Tail(count);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Tail(long count) => Tail(count);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IVGremlinQuery IVGremlinQuery.Tail(long count) => Tail(count);

        IElementGremlinQuery IElementGremlinQuery.Tail(long count) => Tail(count);

        IGremlinQuery IGremlinQuery.Tail(long count) => Tail(count);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Tail(long count) => Tail(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Tail(long count) => Tail(count);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Tail(long count) => Tail(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Tail(long count) => Tail(count);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenBy(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenBy(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.ThenBy(string lambda) => By(lambda);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenBy(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenBy(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenBy(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement> IOrderedVPropertiesGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement> IOrderedVPropertiesGremlinQuery<TElement>.ThenBy(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement> IOrderedVPropertiesGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.ThenBy(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.ThenBy(string lambda) => By(lambda);

        IOrderedEPropertiesGremlinQuery<TElement> IOrderedEPropertiesGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedEPropertiesGremlinQuery<TElement> IOrderedEPropertiesGremlinQuery<TElement>.ThenBy(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedEPropertiesGremlinQuery<TElement> IOrderedEPropertiesGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenBy(Expression<Func<TElement, object>> projection) => By(projection, Order.Increasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenBy(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Increasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenBy(string lambda) => By(lambda);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.ThenByDescending(Func<IGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.ThenByDescending(Func<IVGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.ThenByDescending(Func<IEGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.ThenByDescending(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.ThenByDescending(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.ThenByDescending(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement> IOrderedVPropertiesGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement> IOrderedVPropertiesGremlinQuery<TElement>.ThenByDescending(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedVPropertiesGremlinQuery<TElement, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.ThenByDescending(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedEPropertiesGremlinQuery<TElement> IOrderedEPropertiesGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedEPropertiesGremlinQuery<TElement> IOrderedEPropertiesGremlinQuery<TElement>.ThenByDescending(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenByDescending(Expression<Func<TElement, object>> projection) => By(projection, Order.Decreasing);

        IOrderedElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.ThenByDescending(Func<IElementGremlinQuery<TElement>, IGremlinQuery> traversal) => By(traversal, Order.Decreasing);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Times(int count) => Times(count);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Times(int count) => Times(count);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Times(int count) => Times(count);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Times(int count) => Times(count);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Times(int count) => Times(count);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Times(int count) => Times(count);

        IEGremlinQuery IEGremlinQuery.Times(int count) => Times(count);

        IElementGremlinQuery IElementGremlinQuery.Times(int count) => Times(count);

        IGremlinQuery IGremlinQuery.Times(int count) => Times(count);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Times(int count) => Times(count);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Times(int count) => Times(count);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Times(int count) => Times(count);

        IVGremlinQuery IVGremlinQuery.Times(int count) => Times(count);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Times(int count) => Times(count);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Times(int count) => Times(count);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Times(int count) => Times(count);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Times(int count) => Times(count);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Times(int count) => Times(count);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Times(int count) => Times(count);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Times(int count) => Times(count);

        IInEGremlinQuery<TElement, TNewInVertex> IEGremlinQuery<TElement>.To<TNewInVertex>(StepLabel<TNewInVertex> stepLabel) => To<TElement, Unit, TNewInVertex>(stepLabel);

        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel) => To<TElement, TOutVertex, TTargetVertex>(stepLabel);

        IEGremlinQuery<TElement, TOutVertex, TTargetVertex> IEGremlinQuery<TElement, TOutVertex>.To<TTargetVertex>(Func<IVGremlinQuery<TOutVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal) => AddStep<TElement, TOutVertex, TTargetVertex, Unit>(new ToTraversalStep(toVertexTraversal(Anonymize<TOutVertex, Unit, Unit, Unit>())));

        IInEGremlinQuery<TElement, TNewInVertex> IEGremlinQuery<TElement>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, Unit, TNewInVertex>(toVertexTraversal);

        IEGremlinQuery<TElement, TOutVertex, TNewInVertex> IOutEGremlinQuery<TElement, TOutVertex>.To<TNewInVertex>(Func<IGremlinQuery, IGremlinQuery<TNewInVertex>> toVertexTraversal) => To<TElement, TOutVertex, TNewInVertex>(toVertexTraversal);

        IGremlinQuery<TItem> IGremlinQuery<TElement>.Unfold<TItem>() => Unfold<TItem>();

        IGremlinQuery<TItem> IGremlinQuery<VertexProperty<TElement>>.Unfold<TItem>() => Unfold<TItem>();

        IGremlinQuery<TItem> IGremlinQuery<VertexProperty<TElement, TMeta>>.Unfold<TItem>() => Unfold<TItem>();

        IGremlinQuery<TItem> IGremlinQuery<Property<TElement>>.Unfold<TItem>() => Unfold<TItem>();

        TTargetQuery IGremlinQuery.Union<TTargetQuery>(params Func<IGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery.Union<TTargetQuery>(params Func<IElementGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVGremlinQuery.Union<TTargetQuery>(params Func<IVGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEGremlinQuery.Union<TTargetQuery>(params Func<IEGremlinQuery, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEPropertiesGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IEPropertiesGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<Property<TElement>>.Union<TTargetQuery>(params Func<IGremlinQuery<Property<TElement>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVPropertiesGremlinQuery<TElement, TMeta>.Union<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TElement, TMeta>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Union<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement, TMeta>>.Union<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVPropertiesGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVPropertiesGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery<VertexProperty<TElement>>.Union<TTargetQuery>(params Func<IElementGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<VertexProperty<TElement>>.Union<TTargetQuery>(params Func<IGremlinQuery<VertexProperty<TElement>>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex, TInVertex>.Union<TTargetQuery>(params Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IOutEGremlinQuery<TElement, TOutVertex>.Union<TTargetQuery>(params Func<IOutEGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IInEGremlinQuery<TElement, TInVertex>.Union<TTargetQuery>(params Func<IInEGremlinQuery<TElement, TInVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEGremlinQuery<TElement, TOutVertex>.Union<TTargetQuery>(params Func<IEGremlinQuery<TElement, TOutVertex>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IEGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IEGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IElementGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IElementGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);

        TTargetQuery IVGremlinQuery<TElement>.Union<TTargetQuery>(params Func<IVGremlinQuery<TElement>, TTargetQuery>[] unionTraversals) => Union(unionTraversals);


        IVGremlinQuery<TVertex> IGremlinQuerySource.V<TVertex>(params object[] ids) => AddStep<Unit, Unit, Unit, Unit>(new VStep(ids)).OfType<TVertex>(_model.VerticesModel, true);

        IVGremlinQuery<IVertex> IGremlinQuerySource.V(params object[] ids) => AddStep<IVertex, Unit, Unit, Unit>(new VStep(ids));

        IGremlinQuery<IDictionary<string, object>> IVPropertiesGremlinQuery<TElement>.ValueMap() => ValueMap<IDictionary<string, object>>();

        IGremlinQuery<TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.ValueMap() => ValueMap<TMeta>();

        IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Values<TElement, TTarget, TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IEGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget>>[] projections) => Values<TElement, TTarget, TTarget>(GraphElementType.Edge, projections);

        IGremlinQuery<TValue> IVGremlinQuery<TElement>.Values<TValue, TNewMeta>(params Expression<Func<TElement, VertexProperty<TValue, TNewMeta>>>[] projections) => Values<TElement, VertexProperty<TValue, TNewMeta>, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IVGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, VertexProperty<TValue>>>[] projections) => Values<TElement, VertexProperty<TValue>, TValue>(GraphElementType.Vertex, projections);

        IGremlinQuery<TValue> IEGremlinQuery<TElement>.Values<TValue>(params Expression<Func<TElement, Property<TValue>>>[] projections) => Values<TElement, Property<TValue>, TValue>(GraphElementType.Edge, projections);

        IGremlinQuery<TTarget> IVPropertiesGremlinQuery<TElement, TMeta>.Values<TTarget>(params Expression<Func<TMeta, TTarget>>[] projections) => Values<TMeta, TTarget, TTarget>(GraphElementType.VertexProperty, projections);

        IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Values<TElement, TTarget[], TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IEGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, TTarget[]>>[] projections) => Values<TElement, TTarget[], TTarget>(GraphElementType.Edge, projections);

        IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, VertexProperty<TTarget>[]>>[] projections) => Values<TElement, VertexProperty<TTarget>[], TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IVGremlinQuery<TElement>.Values<TTarget, TTargetMeta>(params Expression<Func<TElement, VertexProperty<TTarget, TTargetMeta>[]>>[] projections) => Values<TElement, VertexProperty<TTarget, TTargetMeta>[], TTarget>(GraphElementType.Vertex, projections);

        IGremlinQuery<TTarget> IEGremlinQuery<TElement>.Values<TTarget>(params Expression<Func<TElement, Property<TTarget>[]>>[] projections) => Values<TElement, Property<TTarget>[], TTarget>(GraphElementType.Edge, projections);

        IGremlinQuery<object> IVPropertiesGremlinQuery<TElement>.Values(params string[] keys) => AddStep<object, Unit, Unit, Unit>(new ValuesStep(keys));


        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.None, predicate);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Where(Expression<Func<VertexProperty<TElement>, bool>> predicate) => Cast<VertexProperty<TElement>>().Where(GraphElementType.VertexProperty, predicate);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Where(Expression<Func<VertexProperty<TElement, TMeta>, bool>> predicate) => Cast<VertexProperty<TElement, TMeta>>().Where(GraphElementType.VertexProperty, predicate);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Where(Expression<Func<Property<TElement>, bool>> predicate) => Cast<Property<TElement>>().Where(GraphElementType.None, predicate);

        IGremlinQuery<TElement> IGremlinQuery<TElement>.Where(Func<IGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where(Func<IEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where(Func<IEGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where(Func<IVGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where(Func<IOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where(Func<IInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<VertexProperty<TElement>> IGremlinQuery<VertexProperty<TElement>>.Where(Func<IGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<VertexProperty<TElement, TMeta>> IGremlinQuery<VertexProperty<TElement, TMeta>>.Where(Func<IGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<Property<TElement>> IGremlinQuery<Property<TElement>>.Where(Func<IGremlinQuery<Property<TElement>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IGremlinQuery<TElement> IOrderedGremlinQuery<TElement>.Where(Func<IOrderedGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVGremlinQuery<TElement> IOrderedVGremlinQuery<TElement>.Where(Func<IOrderedVGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<TElement> IElementGremlinQuery<TElement>.Where(Func<IElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVPropertiesGremlinQuery<TElement> IVPropertiesGremlinQuery<TElement>.Where(Func<IVPropertiesGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<VertexProperty<TElement>> IElementGremlinQuery<VertexProperty<TElement>>.Where(Func<IElementGremlinQuery<VertexProperty<TElement>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVPropertiesGremlinQuery<TElement, TMeta> IVPropertiesGremlinQuery<TElement, TMeta>.Where(Func<IVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<VertexProperty<TElement, TMeta>> IElementGremlinQuery<VertexProperty<TElement, TMeta>>.Where(Func<IElementGremlinQuery<VertexProperty<TElement, TMeta>>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement> IOrderedEGremlinQuery<TElement>.Where(Func<IOrderedEGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IInEGremlinQuery<TElement, TInVertex> IOrderedInEGremlinQuery<TElement, TInVertex>.Where(Func<IOrderedInEGremlinQuery<TElement, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOrderedOutEGremlinQuery<TElement, TOutVertex>.Where(Func<IOrderedOutEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Func<IOrderedEGremlinQuery<TElement, TOutVertex, TInVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEPropertiesGremlinQuery<TElement> IEPropertiesGremlinQuery<TElement>.Where(Func<IEPropertiesGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVPropertiesGremlinQuery<TElement> IOrderedVPropertiesGremlinQuery<TElement>.Where(Func<IOrderedVPropertiesGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVPropertiesGremlinQuery<TElement, TMeta> IOrderedVPropertiesGremlinQuery<TElement, TMeta>.Where(Func<IOrderedVPropertiesGremlinQuery<TElement, TMeta>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEPropertiesGremlinQuery<TElement> IOrderedEPropertiesGremlinQuery<TElement>.Where(Func<IOrderedEPropertiesGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Vertex, predicate);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where(Expression<Func<TElement, bool>> predicate) => Where(GraphElementType.Edge, predicate);

        IEGremlinQuery<TElement> IEGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Vertex, projection, propertyTraversal);

        IVGremlinQuery<TElement> IVGremlinQuery<TElement>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IEGremlinQuery<TElement, TOutVertex> IOrderedEGremlinQuery<TElement, TOutVertex>.Where(Func<IOrderedEGremlinQuery<TElement, TOutVertex>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IElementGremlinQuery<TElement> IOrderedElementGremlinQuery<TElement>.Where(Func<IOrderedElementGremlinQuery<TElement>, IGremlinQuery> filterTraversal) => Where(filterTraversal);

        IEGremlinQuery<TElement, TOutVertex, TInVertex> IEGremlinQuery<TElement, TOutVertex, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IEGremlinQuery<TElement, TOutVertex> IEGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IOutEGremlinQuery<TElement, TOutVertex> IOutEGremlinQuery<TElement, TOutVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);

        IInEGremlinQuery<TElement, TInVertex> IInEGremlinQuery<TElement, TInVertex>.Where<TProjection>(Expression<Func<TElement, TProjection>> projection, Func<IGremlinQuery<TProjection>, IGremlinQuery> propertyTraversal) => Where(GraphElementType.Edge, projection, propertyTraversal);
    }
}
