namespace ExRam.Gremlinq.Core
{
    /// <summary>Builder interface for constructing a <c>choose()</c> step with multiple case branches.</summary>
    /// <typeparam name="TSourceQuery">The source query type.</typeparam>
    public interface IChooseBuilder<out TSourceQuery>
        where TSourceQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the traversal whose result determines the branch to take.
        /// </summary>
        /// <typeparam name="TElement">The type of the element produced by the choice traversal.</typeparam>
        /// <param name="chooseTraversal">The traversal used to determine the value for the branch.</param>
        IChooseBuilderWithCondition<TSourceQuery, TElement> On<TElement>(Func<TSourceQuery, IGremlinQueryBase<TElement>> chooseTraversal);
    }
}
