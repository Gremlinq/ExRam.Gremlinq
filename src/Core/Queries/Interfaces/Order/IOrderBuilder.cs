using System.Linq.Expressions;

namespace ExRam.Gremlinq.Core
{
    public interface IOrderBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Modulates the order step with an ascending ordering by the given traversal.
        /// Corresponds to the Gremlin <c>by()</c> modulator on an <c>order()</c> step.
        /// </summary>
        /// <param name="traversal">The traversal whose result is used for ordering.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
        IOrderBuilderWithBy<TSourceQuery> By(Func<TSourceQuery, IGremlinQueryBase> traversal);

        /// <summary>
        /// Modulates the order step with a descending ordering by the given traversal.
        /// Corresponds to the Gremlin <c>by(..., desc)</c> modulator on an <c>order()</c> step.
        /// </summary>
        /// <param name="traversal">The traversal whose result is used for ordering.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
        IOrderBuilderWithBy<TSourceQuery> ByDescending(Func<TSourceQuery, IGremlinQueryBase> traversal);
    }

    public interface IOrderBuilder<TElement, out TSourceQuery> :
        IOrderBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQueryBase<TElement>
    {
        /// <summary>
        /// Modulates the order step with an ascending ordering by a property projection.
        /// Corresponds to the Gremlin <c>by()</c> modulator on an <c>order()</c> step.
        /// </summary>
        /// <param name="projection">An expression selecting the property to order by.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
        IOrderBuilderWithBy<TElement, TSourceQuery> By(Expression<Func<TElement, object?>> projection);

        /// <summary>
        /// Modulates the order step with a descending ordering by a property projection.
        /// Corresponds to the Gremlin <c>by(..., desc)</c> modulator on an <c>order()</c> step.
        /// </summary>
        /// <param name="projection">An expression selecting the property to order by.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#order-step">Reference Documentation - Order Step</seealso>
        IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(Expression<Func<TElement, object?>> projection);

        /// <inheritdoc cref="IOrderBuilder{TSourceQuery}.By" />
        new IOrderBuilderWithBy<TElement, TSourceQuery> By(Func<TSourceQuery, IGremlinQueryBase> traversal);

        /// <inheritdoc cref="IOrderBuilder{TSourceQuery}.ByDescending" />
        new IOrderBuilderWithBy<TElement, TSourceQuery> ByDescending(Func<TSourceQuery, IGremlinQueryBase> traversal);
    }

    public interface IOrderBuilderWithBy<out TSourceQuery> : IOrderBuilder<TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Builds and returns the ordered query.
        /// </summary>
        TSourceQuery Build();
    }

    public interface IOrderBuilderWithBy<TElement, out TSourceQuery> :
        IOrderBuilderWithBy<TSourceQuery>,
        IOrderBuilder<TElement, TSourceQuery>
        where TSourceQuery : IGremlinQueryBase<TElement>;
}
