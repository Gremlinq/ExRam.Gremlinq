#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a loop builder with a repeat specification that can be finalized or extended.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    public interface IRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Specifies that results should be emitted during loop execution.
        /// </summary>
        /// <returns>A repeat-emit loop builder.</returns>
        IRepeatEmitLoopBuilder<TQuery> Emit();
        
        /// <summary>
        /// Specifies the condition for terminating the loop.
        /// </summary>
        /// <param name="condition">A traversal that defines the termination condition.</param>
        /// <returns>A repeat-until loop builder.</returns>
        IRepeatUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
        
        /// <summary>
        /// Specifies the maximum number of loop iterations.
        /// </summary>
        /// <param name="loopCount">The number of times to repeat the loop.</param>
        /// <returns>A repeat-until loop builder.</returns>
        IRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
    }
}
