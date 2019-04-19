using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource
    {
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TItem> UpdateE<TItem>(TItem edge);
        IVertexGremlinQuery<TItem> UpdateV<TItem>(TItem vertex);
        IVertexGremlinQuery<IVertex> V(params object[] ids);
        IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        IEdgeGremlinQuery<IEdge> E(params object[] ids);
        IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
        IVertexGremlinQuery<TVertex> ReplaceV<TVertex>(TVertex vertex);
        IEdgeGremlinQuery<TItem> ReplaceE<TItem>(TItem edge);
    }
}
