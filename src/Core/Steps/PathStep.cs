namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>path()</c> step that maps traversers to their path information.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#path-step">Reference Documentation - Path Step</seealso>
    public sealed class PathStep : Step
    {
        public static readonly PathStep Instance = new();
    }
}
