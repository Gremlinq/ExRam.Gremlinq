using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexGremlinQuery : IElementGremlinQuery
    {
        IVertexGremlinQuery<IVertex> Both();
        IVertexGremlinQuery<IVertex> Both<TEdge>();

        IEdgeGremlinQuery<IEdge> BothE();
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();

        IVertexGremlinQuery<IVertex> In();
        IVertexGremlinQuery<IVertex> In<TEdge>();

        IEdgeGremlinQuery<IEdge> InE();
        IEdgeGremlinQuery<TEdge> InE<TEdge>();

        IVertexGremlinQuery<IVertex> Out();
        IVertexGremlinQuery<IVertex> Out<TEdge>();

        IEdgeGremlinQuery<IEdge> OutE();
        IEdgeGremlinQuery<TEdge> OutE<TEdge>();
    }

    public partial interface IVertexGremlinQuery<TVertex> : IElementGremlinQuery<TVertex>, IVertexGremlinQuery
    {
        new IEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();
        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);

        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IValueGremlinQuery<object> Values(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);
    }
}
