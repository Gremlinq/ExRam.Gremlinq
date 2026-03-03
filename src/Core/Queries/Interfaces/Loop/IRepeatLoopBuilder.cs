#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>A loop builder with a repeat traversal set.</summary>
    public interface IRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Emit all objects from the loop.
        /// Corresponds to the Gremlin <c>emit()</c> modulator used after <c>repeat()</c>.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IRepeatEmitLoopBuilder<TQuery> Emit();

        /// <summary>
        /// Defines a condition for when the loop should exit.
        /// Corresponds to the Gremlin <c>until()</c> modulator used after <c>repeat()</c>.
        /// </summary>
        /// <param name="condition">The traversal that determines when the loop exits.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IRepeatUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);

        /// <summary>
        /// Specifies how many loops should occur before exiting.
        /// Corresponds to the Gremlin <c>times()</c> modulator.
        /// </summary>
        /// <param name="loopCount">The number of loops to execute.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
    }
}
