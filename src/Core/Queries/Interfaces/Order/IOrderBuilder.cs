using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Builds ordering specifications for query results.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IOrderBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies an ascending order by the results of a traversal.
        /// </summary>
        /// <param name="traversal">A traversal that produces values to order by.</param>
        /// <returns>An order builder with the ordering specification applied.</returns>
        IOrderBuilderWithBy<TSourceQuery> By(Func<TSourceQuery, IGremlinQueryBase> traversal);
        
        /// <summary>
        /// Specifies a descending order by the results of a traversal.
        /// </summary>
        /// <param name="traversal">A traversal that produces values to order by.</param>
        /// <returns>An order builder with the ordering specification applied.</returns>
        IOrderBuilderWithBy<TSourceQuery> ByDescending(Func<TSourceQuery, IGremlinQueryBase> traversal);
    }

    /// <summary>
    /// Builds ordering specifications for strongly-typed query results.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IOrderBuilder<TElement, out TSourceQuery> :
        IOrderBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Specifies an ascending order by a projected property.
        /// </summary>
        /// <param name="projection">Expression selecting the property to order by.</param>
        /// <returns>An order builder with the ordering specification applied.</returns>
        IOrderBuilderWithBy<TElement, TSourceQuery> By(Expression<Func<TElement, object?>> projection);
        
        /// <summary>
        /// Specifies a descending order by a projected property.
        /// </summary>
        /// <param name="projection">Expression selecting the property to order by.</param>
        /// <returns>An order builder with the ordering specification applied.</returns>
        IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(Expression<Func<TElement, object?>> projection);

        /// <summary>
        /// Specifies an ascending order by the results of a traversal.
        /// </summary>
        /// <param name="traversal">A traversal that produces values to order by.</param>
        /// <returns>An order builder with the ordering specification applied.</returns>
        new IOrderBuilderWithBy<TElement, TSourceQuery> By(Func<TSourceQuery, IGremlinQueryBase> traversal);
        
        /// <summary>
        /// Specifies a descending order by the results of a traversal.
        /// </summary>
        /// <param name="traversal">A traversal that produces values to order by.</param>
        /// <returns>An order builder with the ordering specification applied.</returns>
        new IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(Func<TSourceQuery, IGremlinQueryBase> traversal);
    }

    /// <summary>
    /// Represents an order builder with at least one ordering specification that can be finalized.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IOrderBuilderWithBy<out TSourceQuery> : IOrderBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Builds and returns the ordered query.
        /// </summary>
        /// <returns>The source query with ordering applied.</returns>
        TSourceQuery Build();
    }

    /// <summary>
    /// Represents a strongly-typed order builder with at least one ordering specification that can be finalized.
    /// </summary>
    /// <typeparam name="TElement">The element type.</typeparam>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IOrderBuilderWithBy<TElement, out TSourceQuery> :
        IOrderBuilderWithBy<TSourceQuery>,
        IOrderBuilder<TElement, TSourceQuery>
        where TSourceQuery : IGremlinQueryBase<TElement>;
}
