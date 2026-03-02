#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    public interface IUntilEmitLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Defines the traversal to loop over.
        /// Corresponds to the Gremlin <c>repeat()</c> step.
        /// </summary>
        /// <param name="loop">The traversal to repeat.</param>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IUntilEmitRepeatLoopBuilder<TQuery> Repeat(Func<TQuery, TQuery> loop);
    }
}
