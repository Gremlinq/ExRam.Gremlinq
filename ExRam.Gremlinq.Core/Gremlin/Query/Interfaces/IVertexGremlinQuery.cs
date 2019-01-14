using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVertexGremlinQuery : IElementGremlinQuery
    {
        IVertexGremlinQuery<IVertex> Both<TEdge>();
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();

        IVertexGremlinQuery<IVertex> In<TEdge>();

        IVertexGremlinQuery<IVertex> Out<TEdge>();
    }

    public partial interface IVertexGremlinQuery<TVertex> : IElementGremlinQuery<TVertex>, IVertexGremlinQuery
    {
        new IEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();

        IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IVertexPropertyGremlinQuery<TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);

        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
        IVertexPropertyGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IVertexPropertyGremlinQuery<TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);
    }
}
