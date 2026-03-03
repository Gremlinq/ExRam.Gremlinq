namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>explain()</c> step that returns the traversal execution plan.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#explain-step">Reference Documentation - Explain Step</seealso>
    public sealed class ExplainStep : Step
    {
        public static readonly ExplainStep Instance = new();
    }
}
