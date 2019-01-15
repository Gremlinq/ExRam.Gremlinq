using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IEdgeGremlinQuery : IElementGremlinQuery
    {
        IVertexGremlinQuery<IVertex> BothV();

        IVertexGremlinQuery<IVertex> InV();

        IVertexGremlinQuery<IVertex> OtherV();

        IVertexGremlinQuery<IVertex> OutV();
    }

    public partial interface IEdgeGremlinQuery<TEdge> : IElementGremlinQuery<TEdge>, IEdgeGremlinQuery
    {
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        IEdgePropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);
        IEdgePropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);
        IEdgePropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, TTarget[]>>[] projections);
        IEdgePropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TEdge, Property<TTarget>[]>>[] projections);

        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>[]>>[] projections);
    }

    public partial interface IEdgeGremlinQuery<TEdge, TAdjacentVertex> : IEdgeGremlinQuery<TEdge>
    {
        IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IGremlinQuery<TTargetVertex>> fromVertexTraversal);
        new IEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IGremlinQuery<TTargetVertex>> toVertexTraversal);
        new IEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);
    }

    public partial interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> : IEdgeGremlinQuery<TEdge, TOutVertex>, IOutEdgeGremlinQuery<TEdge, TOutVertex>, IInEdgeGremlinQuery<TEdge, TInVertex>
    {
        new IVertexGremlinQuery<TInVertex> InV();
        new IVertexGremlinQuery<TOutVertex> OutV();
    }
}
