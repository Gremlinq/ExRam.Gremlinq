namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>fold()</c> step that collects all traversers into a single list.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#fold-step">Reference Documentation - Fold Step</seealso>
    public sealed class FoldStep : Step
    {
        public static readonly FoldStep Instance = new();
    }
}
