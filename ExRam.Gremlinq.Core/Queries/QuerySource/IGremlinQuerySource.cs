using System;
using System.Collections.Immutable;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    public interface IGremlinQuerySource
    {
        IVertexGremlinQuery<TVertex> AddV<TVertex>(TVertex vertex);
        IVertexGremlinQuery<TVertex> UpdateV<TVertex>(TVertex vertex);
        IVertexGremlinQuery<TVertex> UpdateV<TVertex>(TVertex vertex, Func<string, bool> excludePropertyFilter);
        IVertexGremlinQuery<TVertex> UpdateV<TVertex>(TVertex vertex, ImmutableList<string> excludeFromUpdate);
        IEdgeGremlinQuery<TEdge> AddE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TEdge> UpdateE<TEdge>(TEdge edge);
        IEdgeGremlinQuery<TEdge> UpdateE<TEdge>(TEdge edge, Func<string, bool> excludePropertyFilter);
        IEdgeGremlinQuery<TEdge> UpdateE<TEdge>(TEdge edge, ImmutableList<string> excludeFromUpdate);
        IVertexGremlinQuery<IVertex> V(params object[] ids);
        IVertexGremlinQuery<TVertex> V<TVertex>(params object[] ids);
        IEdgeGremlinQuery<IEdge> E(params object[] ids);
        IEdgeGremlinQuery<TEdge> E<TEdge>(params object[] ids);
        IGremlinQuery<TElement> Inject<TElement>(params TElement[] elements);
    }
}
