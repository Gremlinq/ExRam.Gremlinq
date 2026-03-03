#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>The terminal state of a loop builder that can produce a final query.</summary>
    public interface IFinalLoopBuilder<out TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Builds and returns the final loop query.
        /// </summary>
        TQuery Build();
    }
}
