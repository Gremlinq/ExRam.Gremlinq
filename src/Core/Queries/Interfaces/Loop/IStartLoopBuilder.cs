#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents the initial state for building loop (repeat/until/emit) traversals.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    public interface IStartLoopBuilder<TQuery> where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies the traversal to repeat in the loop.
        /// </summary>
        /// <param name="loop">A function that defines the loop body traversal.</param>
        /// <returns>A repeat loop builder.</returns>
        IRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);
        
        /// <summary>
        /// Specifies that results should be emitted during the loop.
        /// </summary>
        /// <returns>An emit loop builder.</returns>
        IEmitLoopBuilder<TQuery> Emit();
        
        /// <summary>
        /// Specifies the condition for terminating the loop.
        /// </summary>
        /// <param name="condition">A traversal that defines the termination condition.</param>
        /// <returns>An until loop builder.</returns>
        IUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
    }
}
