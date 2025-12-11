namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a query for elements that can be either edges or vertices.
    /// </summary>
    public interface IEdgeOrVertexGremlinQueryBase :
        IElementGremlinQueryBase
    {
        /// <summary>
        /// Casts the query to a different result type.
        /// </summary>
        /// <typeparam name="TResult">The target result type.</typeparam>
        /// <returns>An edge-or-vertex query with the specified result type.</returns>
        new IEdgeOrVertexGremlinQuery<TResult> Cast<TResult>();

        /// <summary>
        /// Downcasts the query to a less specific element query type.
        /// </summary>
        /// <returns>A general element query.</returns>
        new IElementGremlinQuery<object> Lower();
    }

    /// <summary>
    /// Represents a recursive edge-or-vertex query.
    /// </summary>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IEdgeOrVertexGremlinQueryBaseRec<TSelf> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBaseRec<TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TSelf>;

    /// <summary>
    /// Represents a strongly-typed query for elements that can be either edges or vertices.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public interface IEdgeOrVertexGremlinQueryBase<TElement> :
        IEdgeOrVertexGremlinQueryBase,
        IElementGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Downcasts the query to a less specific element query type.
        /// </summary>
        /// <returns>A general element query for the element type.</returns>
        new IElementGremlinQuery<TElement> Lower();
    }

    /// <summary>
    /// Represents a recursive strongly-typed edge-or-vertex query.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    /// <typeparam name="TSelf">The concrete query type for method chaining.</typeparam>
    public interface IEdgeOrVertexGremlinQueryBaseRec<TElement, TSelf> :
        IEdgeOrVertexGremlinQueryBaseRec<TSelf>,
        IEdgeOrVertexGremlinQueryBase<TElement>,
        IElementGremlinQueryBaseRec<TElement, TSelf>
        where TSelf : IElementGremlinQueryBaseRec<TElement, TSelf>;

    /// <summary>
    /// Represents a query for strongly-typed elements that can be either edges or vertices.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    public interface IEdgeOrVertexGremlinQuery<TElement> :
        IEdgeOrVertexGremlinQueryBaseRec<TElement, IEdgeOrVertexGremlinQuery<TElement>>;
}
