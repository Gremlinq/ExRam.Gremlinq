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

        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IVertexPropertyGremlinQuery<TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);

        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IVertexPropertyGremlinQuery<TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);

        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);
        IValueGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);
    }
}
