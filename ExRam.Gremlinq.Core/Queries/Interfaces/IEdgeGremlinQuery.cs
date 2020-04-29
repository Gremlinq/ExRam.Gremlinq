using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IEdgeGremlinQueryBase : IEdgeOrVertexGremlinQueryBase
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

    public partial interface IEdgeGremlinQueryBase<TEdge> :
        IEdgeGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TEdge>
    {
        new IEdgeGremlinQuery<TEdge> Update(TEdge element);

        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQuery<TOutVertex>> fromVertexTraversal);
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        new IEdgeOrVertexGremlinQuery<TEdge> Lower();

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, TValue>>[] projections);

        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        IPropertyGremlinQuery<Property<object>> Properties(params Expression<Func<TEdge, Property<object>>>[] projections);

        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQuery<TInVertex>> toVertexTraversal);
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


    public interface IInOrOutEdgeGremlinQueryBase : IEdgeGremlinQueryBase
    {

    }

    public partial interface IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBase<TEdge>
    {
        IBothEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IVertexGremlinQuery<TTargetVertex>> fromVertexTraversal);
        new IBothEdgeGremlinQuery<TEdge, TTargetVertex, TAdjacentVertex> From<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);

        IBothEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(Func<IVertexGremlinQuery<TAdjacentVertex>, IVertexGremlinQuery<TTargetVertex>> toVertexTraversal);
        new IBothEdgeGremlinQuery<TEdge, TAdjacentVertex, TTargetVertex> To<TTargetVertex>(StepLabel<TTargetVertex> stepLabel);
    }

    public interface IInOrOutEdgeGremlinQueryBaseRec<TSelf> :
        IInOrOutEdgeGremlinQueryBase,
        IEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf> :
        IInOrOutEdgeGremlinQueryBaseRec<TSelf>,
        IInOrOutEdgeGremlinQueryBase<TEdge, TAdjacentVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, TSelf>
    {

    }

    public interface IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<TEdge, TAdjacentVertex, IInOrOutEdgeGremlinQuery<TEdge, TAdjacentVertex>>
    {
    }



    public interface IBothEdgeGremlinQueryBase :
        IInOrOutEdgeGremlinQueryBase,
        IOutEdgeGremlinQueryBase,
        IInEdgeGremlinQueryBase
    {

    }

    public partial interface IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> :
        IBothEdgeGremlinQueryBase,
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>
    {
        
    }

    public interface IBothEdgeGremlinQueryRec<TSelf> :
        IBothEdgeGremlinQueryBase,
        IInOrOutEdgeGremlinQueryBaseRec<TSelf>,
        IOutEdgeGremlinQueryBaseRec<TSelf>,
        IInEdgeGremlinQueryBaseRec<TSelf>
        where TSelf : IBothEdgeGremlinQueryRec<TSelf>
    {

    }

    public interface IBothEdgeGremlinQueryRec<TEdge, TOutVertex, TInVertex, TSelf> :
        IBothEdgeGremlinQueryRec<TSelf>,
        IBothEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IBothEdgeGremlinQueryRec<TEdge, TOutVertex, TInVertex, TSelf>
    {

    }

    public interface IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> :
        IBothEdgeGremlinQueryRec<TEdge, TOutVertex, TInVertex, IBothEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>
    {

    }
}
