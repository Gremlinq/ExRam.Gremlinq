using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexGremlinQueryBase :
        IEdgeOrVertexGremlinQueryBase
    {
        IVertexGremlinQuery<object> Both();
        IVertexGremlinQuery<object> Both<TEdge>();

        IEdgeGremlinQuery<object> BothE();
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();

        new IVertexGremlinQuery<TResult> Cast<TResult>();
        
        IVertexGremlinQuery<object> In();
        IVertexGremlinQuery<object> In<TEdge>();

        IEdgeGremlinQuery<object> InE();
        IEdgeGremlinQuery<TEdge> InE<TEdge>();

        new IEdgeOrVertexGremlinQuery<object> Lower();

        IVertexGremlinQuery<TTarget> OfType<TTarget>();

        IVertexGremlinQuery<object> Out();
        IVertexGremlinQuery<object> Out<TEdge>();

        IEdgeGremlinQuery<object> OutE();
        IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }

    public partial interface IVertexGremlinQueryBase<TVertex> :
        IVertexGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TVertex>
    {
        new IVertexGremlinQuery<TVertex> Update(TVertex element);

        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        new IInOrOutEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();

        new IEdgeOrVertexGremlinQuery<TVertex> Lower();

        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties();
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>();

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections)
            where TMeta : class;

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections)
            where TMeta : class;

        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        new IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IValueGremlinQuery<object> Values(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections) where TMeta : class;
        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections) where TMeta : class;
    }

    public interface IVertexGremlinQueryBaseRec<TSelf> :
        IVertexGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBaseRec<TSelf>
        where TSelf : IVertexGremlinQueryBaseRec<TSelf>
    {

    }

    public interface IVertexGremlinQueryBaseRec<TVertex, TSelf> :
        IVertexGremlinQueryBaseRec<TSelf>,
        IVertexGremlinQueryBase<TVertex>,
        IEdgeOrVertexGremlinQueryBaseRec<TVertex, TSelf>
        where TSelf : IVertexGremlinQueryBaseRec<TVertex, TSelf>
    {
        TSelf Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }

    public interface IVertexGremlinQuery<TVertex> :
        IVertexGremlinQueryBaseRec<TVertex, IVertexGremlinQuery<TVertex>>
    {

    }
}
