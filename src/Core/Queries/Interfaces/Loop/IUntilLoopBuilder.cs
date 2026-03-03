#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>A loop builder with an until condition set.</summary>
    public interface IUntilLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Defines the traversal to loop over.
        /// Corresponds to the Gremlin <c>repeat()</c> step used after <c>until()</c>.
        /// </summary>
        /// <param name="loop">The traversal to repeat.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IUntilRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);

        /// <summary>
        /// Emit all objects from the loop.
        /// Corresponds to the Gremlin <c>emit()</c> modulator used after <c>until()</c>.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IUntilEmitLoopBuilder<TQuery> Emit();
    }
}
