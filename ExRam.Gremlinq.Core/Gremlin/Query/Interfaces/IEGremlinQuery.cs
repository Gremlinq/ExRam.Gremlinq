using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IEGremlinQuery : IElementGremlinQuery
    {
        IVGremlinQuery<IVertex> BothV();
        
        IVGremlinQuery<IVertex> InV();

        IVGremlinQuery<IVertex> OtherV();
        IVGremlinQuery<IVertex> OutV();
    }

    public partial interface IEGremlinQuery<TEdge> : IElementGremlinQuery<TEdge>, IEGremlinQuery
    {
        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        IEPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        IEPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);
        IEPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);
        IEPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, Property<TTarget>[]>>[] projections);

        IInEGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
        IInEGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>[]>>[] projections);
    }

    public partial interface IEGremlinQuery<TEdge, TAdjacentVertex> : IEGremlinQuery<TEdge>
    {
        IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IVGremlinQuery<TAdjacentVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal);
        new IEGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IVGremlinQuery<TAdjacentVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal);
        new IEGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);
    }

    public partial interface IEGremlinQuery<TEdge, TOutVertex, TInVertex> : IEGremlinQuery<TEdge, TOutVertex>, IOutEGremlinQuery<TEdge, TOutVertex>, IInEGremlinQuery<TEdge, TInVertex>
    {
        new IVGremlinQuery<TInVertex> InV();
        new IVGremlinQuery<TOutVertex> OutV();
    }
}
