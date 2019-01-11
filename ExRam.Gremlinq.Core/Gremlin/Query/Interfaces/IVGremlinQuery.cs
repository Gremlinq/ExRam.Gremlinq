using System;
using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public partial interface IVGremlinQuery : IElementGremlinQuery
    {
        IVGremlinQuery<IVertex> Both<TEdge>();
        IEGremlinQuery<TEdge> BothE<TEdge>();

        IVGremlinQuery<IVertex> In<TEdge>();

        IVGremlinQuery<IVertex> Out<TEdge>();
    }

    public partial interface IVGremlinQuery<TVertex> : IElementGremlinQuery<TVertex>, IVGremlinQuery
    {
        new IEGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        IEGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        IInEGremlinQuery<TEdge, TVertex> InE<TEdge>();

        IOutEGremlinQuery<TEdge, TVertex> OutE<TEdge>();

        IVPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget>>[] projections);
        IVPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IVPropertiesGremlinQuery<TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);

        IVPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, TTarget[]>>[] projections);
        IVPropertiesGremlinQuery<TTarget> Properties<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IVPropertiesGremlinQuery<TTarget, TMeta> Properties<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);

        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget>(params Expression<Func<TVertex, VertexProperty<TTarget>[]>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>>>[] projections);
        IGremlinQuery<TTarget> Values<TTarget, TMeta>(params Expression<Func<TVertex, VertexProperty<TTarget, TMeta>[]>>[] projections);
    }
}
