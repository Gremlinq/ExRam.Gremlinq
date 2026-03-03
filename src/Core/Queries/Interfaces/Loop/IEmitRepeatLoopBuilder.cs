#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>A loop builder with emit and repeat steps set.</summary>
    public interface IEmitRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Defines a condition for when the loop should exit.
        /// Corresponds to the Gremlin <c>until()</c> modulator.
        /// </summary>
        /// <param name="condition">The traversal that determines when the loop exits.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IEmitRepeatUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);

        /// <summary>
        /// Specifies how many loops should occur before exiting.
        /// Corresponds to the Gremlin <c>times()</c> modulator.
        /// </summary>
        /// <param name="loopCount">The number of loops to execute.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IEmitRepeatUntilLoopBuilder<TQuery> Times(int loopCount);
    }
}
