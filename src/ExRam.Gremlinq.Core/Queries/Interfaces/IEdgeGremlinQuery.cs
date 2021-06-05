using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IEdgeGremlinQueryBase : IEdgeOrVertexGremlinQueryBase
    {
        IVertexGremlinQuery<object> BothV();
        IVertexGremlinQuery<TVertex> BothV<TVertex>();

        new IEdgeGremlinQuery<TResult> Cast<TResult>();

        IVertexGremlinQuery<object> InV();
        IVertexGremlinQuery<TVertex> InV<TVertex>();

        new IEdgeOrVertexGremlinQuery<object> Lower();

        IEdgeGremlinQuery<TTarget> OfType<TTarget>();

        IVertexGremlinQuery<object> OtherV();
        IVertexGremlinQuery<TVertex> OtherV<TVertex>();

        IVertexGremlinQuery<object> OutV();
        IVertexGremlinQuery<TVertex> OutV<TVertex>();

        IPropertyGremlinQuery<Property<object>> Properties();
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>();
    }

    public interface IEdgeGremlinQueryBase<TEdge> :
        IEdgeGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Update(TEdge element);

        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        new IEdgeOrVertexGremlinQuery<TEdge> Lower();

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, TValue>>[] projections);

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        IPropertyGremlinQuery<Property<object>> Properties(params Expression<Func<TEdge, Property<object>>>[] projections);

        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, TTarget>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TEdge, Property<TTarget>>>[] projections);

        IValueGremlinQuery<object> Values(params Expression<Func<TEdge, Property<object>>>[] projections);
    }

    public interface IEdgeGremlinQueryBaseRec<TSelf> : IEdgeGremlinQueryBase, IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IEdgeGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IEdgeGremlinQueryBaseRec<TEdge, TSelf> :
        IEdgeGremlinQueryBaseRec<TSelf>,
        IEdgeGremlinQueryBase<TEdge>,
        IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IEdgeGremlinQueryBaseRec<TEdge, TSelf>
    {

    }

    public interface IEdgeGremlinQuery<TEdge> :
        IEdgeGremlinQueryBaseRec<TEdge, IEdgeGremlinQuery<TEdge>>
    {
       
    }
}
