using System.Linq.Expressions;
using ExRam.Gremlinq.Core.GraphElements;

namespace ExRam.Gremlinq.Core
{
    /// <summary>Provides base operations for queries over edges, including traversal to incident vertices and properties.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Steps</seealso>
    public interface IEdgeGremlinQueryBase : IEdgeOrVertexGremlinQueryBase
    {
        /// <summary>
        /// Map the edge to both its incoming and outgoing vertex.
        /// Corresponds to the Gremlin <c>bothV()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> BothV();
        /// <inheritdoc cref="BothV()" />
        IVertexGremlinQuery<TVertex> BothV<TVertex>();

        /// <inheritdoc cref="IGremlinQueryBase.Cast{TResult}" />
        new IEdgeGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Map the edge to its incoming/head incident vertex.
        /// Corresponds to the Gremlin <c>inV()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> InV();
        /// <inheritdoc cref="InV()" />
        IVertexGremlinQuery<TVertex> InV<TVertex>();

        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeOrVertexGremlinQuery<object> Lower();

        /// <summary>
        /// Filter edges to those of the specified type.
        /// Corresponds to the Gremlin <c>hasLabel()</c> step.
        /// </summary>
        /// <typeparam name="TTarget">The edge type to filter by.</typeparam>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#has-step">Reference Documentation - Has Step</seealso>
        IEdgeGremlinQuery<TTarget> OfType<TTarget>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13, TTarget14>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13, TTarget14, TTarget15>();
        /// <inheritdoc cref="OfType{TTarget}" />
        IEdgeGremlinQuery<object> OfType<TTarget1, TTarget2, TTarget3, TTarget4, TTarget5, TTarget6, TTarget7, TTarget8, TTarget9, TTarget10, TTarget11, TTarget12, TTarget13, TTarget14, TTarget15, TTarget16>();

        /// <summary>
        /// Map the edge to the incident vertex that was not traversed from.
        /// Corresponds to the Gremlin <c>otherV()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> OtherV();
        /// <inheritdoc cref="OtherV()" />
        IVertexGremlinQuery<TVertex> OtherV<TVertex>();

        /// <summary>
        /// Map the edge to its outgoing/tail incident vertex.
        /// Corresponds to the Gremlin <c>outV()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#vertex-steps">Reference Documentation - Vertex Step</seealso>
        IVertexGremlinQuery<object> OutV();
        /// <inheritdoc cref="OutV()" />
        IVertexGremlinQuery<TVertex> OutV<TVertex>();

        /// <summary>
        /// Map edges to their properties.
        /// Corresponds to the Gremlin <c>properties()</c> step.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#properties-step">Reference Documentation - Properties Step</seealso>
        IPropertyGremlinQuery<Property<object>> Properties();
        /// <inheritdoc cref="Properties()" />
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>();
    }

    /// <summary>Provides typed base operations for edge queries carrying edges of type <typeparamref name="TEdge"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    public interface IEdgeGremlinQueryBase<TEdge> :
        IEdgeGremlinQueryBase,
        IEdgeOrVertexGremlinQueryBase<TEdge>
    {
        /// <inheritdoc cref="IElementGremlinQueryBase{TElement}.Update" />
        new IEdgeGremlinQuery<TEdge> Update(TEdge element);

        /// <summary>
        /// Specifies the outgoing vertex of the edge via a traversal.
        /// Corresponds to the Gremlin <c>from()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="fromVertexTraversal">The traversal that selects the outgoing vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TOutVertex>> fromVertexTraversal);
        /// <summary>
        /// Specifies the outgoing vertex of the edge via a step label.
        /// Corresponds to the Gremlin <c>from()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TOutVertex">The type of the outgoing vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the outgoing vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IOutEdgeGremlinQuery<TEdge, TOutVertex> From<TOutVertex>(StepLabel<TOutVertex> stepLabel);

        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeOrVertexGremlinQuery<TEdge> Lower();

        /// <inheritdoc cref="IEdgeGremlinQueryBase.Properties()" />
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, TValue>>[] projections);
        /// <inheritdoc cref="IEdgeGremlinQueryBase.Properties()" />
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params ReadOnlySpan<Expression<Func<TEdge, TValue>>> projections);

        /// <inheritdoc cref="IEdgeGremlinQueryBase.Properties()" />
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        /// <inheritdoc cref="IEdgeGremlinQueryBase.Properties()" />
        IPropertyGremlinQuery<Property<TValue>> Properties<TValue>(params ReadOnlySpan<Expression<Func<TEdge, Property<TValue>>>> projections);

        /// <inheritdoc cref="IEdgeGremlinQueryBase.Properties()" />
        IPropertyGremlinQuery<Property<object>> Properties(params Expression<Func<TEdge, Property<object>>>[] projections);
        /// <inheritdoc cref="IEdgeGremlinQueryBase.Properties()" />
        IPropertyGremlinQuery<Property<object>> Properties(params ReadOnlySpan<Expression<Func<TEdge, Property<object>>>> projections);

        /// <summary>
        /// Specifies the incoming vertex of the edge via a traversal.
        /// Corresponds to the Gremlin <c>to()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <param name="toVertexTraversal">The traversal that selects the incoming vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(Func<IVertexGremlinQueryBase, IVertexGremlinQueryBase<TInVertex>> toVertexTraversal);
        /// <summary>
        /// Specifies the incoming vertex of the edge via a step label.
        /// Corresponds to the Gremlin <c>to()</c> modulator on an <c>addE()</c> step.
        /// </summary>
        /// <typeparam name="TInVertex">The type of the incoming vertex.</typeparam>
        /// <param name="stepLabel">The step label referencing the incoming vertex.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#addedge-step">Reference Documentation - AddEdge Step</seealso>
        IInEdgeGremlinQuery<TEdge, TInVertex> To<TInVertex>(StepLabel<TInVertex> stepLabel);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TEdge, TValue>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        new IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TEdge, TValue>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params Expression<Func<TEdge, Property<TValue>>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<TValue> Values<TValue>(params ReadOnlySpan<Expression<Func<TEdge, Property<TValue>>>> projections);

        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<object> Values(params Expression<Func<TEdge, Property<object>>>[] projections);
        /// <inheritdoc cref="IElementGremlinQueryBase.Values()" />
        IGremlinQuery<object> Values(params ReadOnlySpan<Expression<Func<TEdge, Property<object>>>> projections);
    }

    /// <summary>Provides recursive (CRTP) base operations for edge queries.</summary>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IEdgeGremlinQueryBaseRec<TSelf> : IEdgeGremlinQueryBase, IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IEdgeGremlinQueryBaseRec<TSelf>;

    /// <summary>Provides recursive (CRTP) typed base operations for edge queries.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for fluent chaining.</typeparam>
    public interface IEdgeGremlinQueryBaseRec<TEdge, TSelf> :
        IEdgeGremlinQueryBaseRec<TSelf>,
        IEdgeGremlinQueryBase<TEdge>,
        IEdgeOrVertexGremlinQueryBaseRec<TEdge, TSelf>
        where TSelf : IEdgeGremlinQueryBaseRec<TEdge, TSelf>;

    /// <summary>A query over edges of type <typeparamref name="TEdge"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    public interface IEdgeGremlinQuery<TEdge> :
        IEdgeGremlinQueryBaseRec<TEdge, IEdgeGremlinQuery<TEdge>>;

    /// <summary>Provides base operations for edge queries with a known adjacent vertex type.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The adjacent vertex type.</typeparam>
    public interface IEdgeGremlinQueryBase<TEdge, TAdjacentVertex> :
        IEdgeGremlinQueryBase<TEdge>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();
    }

    /// <summary>A query over edges of type <typeparamref name="TEdge"/> with a known adjacent vertex of type <typeparamref name="TAdjacentVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TAdjacentVertex">The adjacent vertex type.</typeparam>
    public interface IEdgeGremlinQuery<TEdge, TAdjacentVertex> :
        IEdgeGremlinQueryBase<TEdge, TAdjacentVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, IEdgeGremlinQuery<TEdge, TAdjacentVertex>>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Interpret this edge query as an in-edge query where <typeparamref name="TAdjacentVertex"/> is the incoming vertex.
        /// </summary>
        IInEdgeGremlinQuery<TEdge, TAdjacentVertex> AsInEdge();
        /// <summary>
        /// Interpret this edge query as an out-edge query where <typeparamref name="TAdjacentVertex"/> is the outgoing vertex.
        /// </summary>
        IOutEdgeGremlinQuery<TEdge, TAdjacentVertex> AsOutEdge();
    }

    /// <summary>Provides base operations for edge queries with both outgoing and incoming vertex types known.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (tail) vertex type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (head) vertex type.</typeparam>
    public interface IEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex> :
        IOutEdgeGremlinQueryBase<TEdge, TOutVertex>,
        IInEdgeGremlinQueryBase<TEdge, TInVertex>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();
    }

    /// <summary>A query over edges of type <typeparamref name="TEdge"/> from <typeparamref name="TOutVertex"/> to <typeparamref name="TInVertex"/>.</summary>
    /// <typeparam name="TEdge">The edge type.</typeparam>
    /// <typeparam name="TOutVertex">The outgoing (tail) vertex type.</typeparam>
    /// <typeparam name="TInVertex">The incoming (head) vertex type.</typeparam>
    public interface IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex> :
        IInOrOutEdgeGremlinQueryBaseRec<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IOutEdgeGremlinQueryBaseRec<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IInEdgeGremlinQueryBaseRec<IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>,
        IEdgeGremlinQueryBase<TEdge, TOutVertex, TInVertex>,
        IEdgeGremlinQueryBaseRec<TEdge, IEdgeGremlinQuery<TEdge, TOutVertex, TInVertex>>
    {
        /// <inheritdoc cref="IEdgeOrVertexGremlinQueryBase.Lower" />
        new IEdgeGremlinQuery<TEdge> Lower();

        /// <summary>
        /// Interpret this edge query as an in-edge query where <typeparamref name="TInVertex"/> is the incoming vertex.
        /// </summary>
        IInEdgeGremlinQuery<TEdge, TInVertex> AsInEdge();
        /// <summary>
        /// Interpret this edge query as an out-edge query where <typeparamref name="TOutVertex"/> is the outgoing vertex.
        /// </summary>
        IOutEdgeGremlinQuery<TEdge, TOutVertex> AsOutEdge();
    }
}
