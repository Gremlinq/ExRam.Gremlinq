namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>barrier()</c> step that turns the lazy traversal pipeline into a bulk-synchronous pipeline.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#barrier-step">Reference Documentation - Barrier Step</seealso>
    public sealed class BarrierStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="BarrierStep"/>.</summary>
        public static readonly BarrierStep Instance = new();
    }
}
