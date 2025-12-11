namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Builds conditional (choose/switch) logic for graph traversals.
    /// </summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IChooseBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the condition to evaluate for the choose logic.
        /// </summary>
        /// <typeparam name="TElement">The type of value produced by the condition.</typeparam>
        /// <param name="chooseTraversal">A traversal that produces the value to switch on.</param>
        /// <returns>A choose builder with the condition specified.</returns>
        IChooseBuilderWithCondition<TSourceQuery, TElement> On<TElement>(Func<TSourceQuery, IGremlinQueryBase<TElement>> chooseTraversal);
    }
}
