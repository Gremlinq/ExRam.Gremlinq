#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    public interface IUntilRepeatLoopBuilder<out TQuery> : IFinalLoopBuilder<TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Emit all objects from the loop.
        /// Corresponds to the Gremlin <c>emit()</c> modulator.
        /// </summary>
        /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#repeat-step">Reference Documentation - Repeat Step</seealso>
        IUntilRepeatEmitLoopBuilder<TQuery> Emit();
    }
}
