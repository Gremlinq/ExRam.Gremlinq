namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>simplePath()</c> step that filters on non-cyclic paths.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#simplepath-step">Reference Documentation - SimplePath Step</seealso>
    public sealed class SimplePathStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="SimplePathStep"/>.</summary>
        public static readonly SimplePathStep Instance = new ();
    }
}
