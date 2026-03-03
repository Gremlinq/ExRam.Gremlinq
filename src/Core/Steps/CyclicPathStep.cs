namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>cyclicPath()</c> step that filters on cyclic paths.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#cyclicpath-step">Reference Documentation - CyclicPath Step</seealso>
    public sealed class CyclicPathStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="CyclicPathStep"/>.</summary>
        public static readonly CyclicPathStep Instance = new ();
    }
}
