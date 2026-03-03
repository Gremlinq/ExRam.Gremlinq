namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>path()</c> step that maps traversers to their path information.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#path-step">Reference Documentation - Path Step</seealso>
    public sealed class PathStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="PathStep"/>.</summary>
        public static readonly PathStep Instance = new();
    }
}
