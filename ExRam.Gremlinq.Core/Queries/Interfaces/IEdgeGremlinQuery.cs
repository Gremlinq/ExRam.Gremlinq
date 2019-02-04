using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IEdgeGremlinQuery : IElementGremlinQuery
    {
        IVertexGremlinQuery<IVertex> BothV();
        IVertexGremlinQuery<TVertex> BothV<TVertex>();

        IVertexGremlinQuery<IVertex> InV();
        IVertexGremlinQuery<TVertex> InV<TVertex>();

        IVertexGremlinQuery<IVertex> OtherV();
        IVertexGremlinQuery<TVertex> OtherV<TVertex>();

        IVertexGremlinQuery<IVertex> OutV();
        IVertexGremlinQuery<TVertex> OutV<TVertex>();
    }

    public partial interface IEdgeGremlinQuery<TEdge> : IElementGremlinQuery<TEdge>, IEdgeGremlinQuery
    {
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IGremlinQuery, IGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        IPropertyGremlinQuery<Property<object>> Properties();
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>();

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params string[] keys);
        IPropertyGremlinQuery<Property<object>> Properties(params string[] keys);

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, TValue>>[] projections);

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        IPropertyGremlinQuery<Property<object>> Properties(params Expression<Func<TEdge, Property<object>>>[] projections);

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, TValue[]>>[] projections);
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, Property<TValue>[]>>[] projections);

        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IGremlinQuery, IGremlinQuery<TInVertex>> toVertexTraversal);
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>[]>>[] projections);

        IValueGremlinQuery<object> Values(params Expression<Func<TEdge, Property<object>>>[] projections);
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
