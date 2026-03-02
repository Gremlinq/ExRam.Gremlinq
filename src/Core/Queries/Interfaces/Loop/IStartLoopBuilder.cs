#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    public interface IStartLoopBuilder<TQuery> where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Defines the traversal to loop over.
        /// Corresponds to the Gremlin <c>repeat()</c> step.
        /// </summary>
        /// <param name="loop">The traversal to repeat.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);

        /// <summary>
        /// Emit all objects from the loop.
        /// Corresponds to the Gremlin <c>emit()</c> modulator.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IEmitLoopBuilder<TQuery> Emit();

        /// <summary>
        /// Defines a condition for when the loop should exit.
        /// Corresponds to the Gremlin <c>until()</c> modulator.
        /// </summary>
        /// <param name="condition">The traversal that determines when the loop exits.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IUntilLoopBuilder<TQuery> Until(Func<TQuery, IGremlinQueryBase> condition);
    }
}
