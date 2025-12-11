#pragma warning disable IDE0003
// ReSharper disable ArrangeThisQualifier

namespace ExRam.Gremlinq.Core
{
    /// <summary>
    /// Represents a loop builder that can be finalized to produce the query.
    /// </summary>
    /// <typeparam name="TQuery">The query type.</typeparam>
    public interface IFinalLoopBuilder<out TQuery>
        where TQuery : IGremlinQueryBase
    {
        /// <summary>
        /// Builds and returns the query with the loop configuration applied.
        /// </summary>
        /// <returns>The query with the loop.</returns>
        TQuery Build();
    }
}
