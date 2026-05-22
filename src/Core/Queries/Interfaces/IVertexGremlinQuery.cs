using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    /// <summary>Provides base operations for queries over vertices, including traversal to adjacent vertices and edges.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Steps</seealso>
    public interface IVertexGremlinQueryBase :
        IEdgeOrVertexGremlinQueryBase
    {
        /// <summary>
        /// Map the vertex to its adjacent vertices via both incoming and outgoing edges.
        /// Corresponds to the Gremlin <c>both()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> Both();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="Both()" />
        IVertexGremlinQuery<object> Both<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <summary>
        /// Map the vertex to its incident edges in both directions.
        /// Corresponds to the Gremlin <c>bothE()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IEdgeGremlinQuery<object> BothE();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<TEdge> BothE<TEdge>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="BothE()" />
        IEdgeGremlinQuery<object> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <inheritdoc cref="IGremlinQueryBase.Cast{TResult}" />
        new IVertexGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Map the vertex to its adjacent vertices connected by incoming edges.
        /// Corresponds to the Gremlin <c>in()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> In();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="In()" />
        IVertexGremlinQuery<object> In<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <summary>
        /// Map the vertex to its incoming incident edges.
        /// Corresponds to the Gremlin <c>inE()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IEdgeGremlinQuery<object> InE();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<TEdge> InE<TEdge>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="InE()" />
        IEdgeGremlinQuery<object> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeOrVertexGremlinQuery<object> Lower();

        /// <summary>
        /// Filter vertices by their type (label). The type parameters determine which vertex types are included.
        /// Corresponds to the Gremlin <c>hasLabel()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
        IVertexGremlinQuery<TTarget> OfType<TTarget>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13, TTarget14>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13, TTarget14, TTarget15>();
        /// <inheritdoc cref="OfType{TTarget}()" />
        IVertexGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13, TTarget14, TTarget15, TTarget16>();

        /// <summary>
        /// Map the vertex to its adjacent vertices connected by outgoing edges.
        /// Corresponds to the Gremlin <c>out()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> Out();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="Out()" />
        IVertexGremlinQuery<object> Out<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <summary>
        /// Map the vertex to its outgoing incident edges.
        /// Corresponds to the Gremlin <c>outE()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IEdgeGremlinQuery<object> OutE();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<TEdge> OutE<TEdge>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="OutE()" />
        IEdgeGremlinQuery<object> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();
    }

    /// <summary>Provides typed base operations for vertex queries carrying vertices of type <typeparamref name="TVertex"/>.</summary>
    /// <typeparam name="TVertex">The vertex type.</typeparam>
    public interface IVertexGremlinQueryBase<TVertex> :
        IVertexGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TVertex>
    {
        /// <inheritdoc cref="IElementGremlinQueryBase{TElement}.Update" />
        new IVertexGremlinQuery<TVertex> Update(TVertex element);

        /// <inheritdoc cref="IStartGremlinQuery.AddE{TEdge}(TEdge)" />
        new IAddEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>(TEdge edge);
        /// <inheritdoc cref="IStartGremlinQuery.AddE{TEdge}()" />
        new IAddEdgeGremlinQuery<TEdge, TVertex> AddE<TEdge>() where TEdge : new();

        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<TEdge, TVertex> BothE<TEdge>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.BothE()" />
        new IEdgeGremlinQuery<object, TVertex> BothE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<TEdge, TVertex> InE<TEdge>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.InE()" />
        new IInEdgeGremlinQuery<object, TVertex> InE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeOrVertexGremlinQuery<TVertex> Lower();

        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<TEdge, TVertex> OutE<TEdge>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15>();
        /// <inheritdoc cref="IVertexGremlinQueryBase.OutE()" />
        new IOutEdgeGremlinQuery<object, TVertex> OutE<TEdge1, TEdge2, TEdge3, TEdge4, TEdge5, TEdge6, TEdge7, TEdge8, TEdge9, TEdge10, TEdge11, TEdge12, TEdge13, TEdge14, TEdge15, TEdge16>();

        /// <summary>
        /// Map the vertex to its vertex properties.
        /// Corresponds to the Gremlin <c>properties()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-properties">Reference Documentation - Vertex Properties</seealso>
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties();
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>();

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue>>> projections);

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>>>> projections);

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<object>, object> Properties(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<object>>>> projections);

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue[]>>> projections);

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue>, TValue> Properties<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>[]>>> projections);

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>> projections);

        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);
        /// <inheritdoc cref="Properties()" />
        IVertexPropertyGremlinQuery<VertexProperty<TValue, TMeta>, TValue, TMeta> Properties<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, TValue>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, TValue[]>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, TValue[]>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<object> Values(params Expression<Func<TVertex, VertexProperty<object>>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<object> Values(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<object>>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TVertex, VertexProperty<TValue>[]>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue>[]>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue, TMeta>(params Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue, TMeta>(params ReadOnlySpan<Expression<Func<TVertex, VertexProperty<TValue, TMeta>[]>>> projections);
    }

    /// <summary>A query over vertices of type <typeparamref name="TVertex"/>.</summary>

    /// <typeparam name="TVertex">The vertex type.</typeparam>

    public interface IVertexGremlinQuery<TVertex> :
        IVertexGremlinQueryBase<TVertex>,
        IEdgeOrVertexGremlinQueryBaseRec<TVertex, IVertexGremlinQuery<TVertex>>
    {
        /// <summary>
        /// Set a single value on a multi-valued (array) vertex property.
        /// Corresponds to the Gremlin <c>property()</c> step.
        /// </summary>
        /// <typeparam name="TProjectedValue">The type of the property value.</typeparam>
        /// <param name="projection">An expression selecting the multi-valued property.</param>
        /// <param name="value">The value to set.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addproperty-step">Reference Documentation - AddProperty Step</seealso>
        IVertexGremlinQuery<TVertex> Property<TProjectedValue>(Expression<Func<TVertex, TProjectedValue[]>> projection, TProjectedValue value);
    }
}
