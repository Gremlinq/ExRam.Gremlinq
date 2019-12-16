using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexGremlinQuery : IElementGremlinQuery
    {
        IVertexGremlinQuery<object> Both();
        IVertexGremlinQuery<object> Both<TEdge>();

        IEdgeGremlinQuery<object> BothE();
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();

        IVertexGremlinQuery<object> In();
        IVertexGremlinQuery<object> In<TEdge>();

        IEdgeGremlinQuery<object> InE();
        IEdgeGremlinQuery<TEdge> InE<TEdge>();

        IVertexGremlinQuery<object> Out();
        IVertexGremlinQuery<object> Out<TEdge>();

        IEdgeGremlinQuery<object> OutE();
        IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }

    public partial interface IVertexGremlinQuery<TVertex> : IElementGremlinQuery<TVertex>, IVertexGremlinQuery
    {
        new IEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();
        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties();
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>();

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params string[] keys);
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params string[] keys);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IValueGremlinQuery<object> Values(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);
    }
}
