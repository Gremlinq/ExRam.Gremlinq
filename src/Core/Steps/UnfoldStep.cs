namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>unfold()</c> step that unrolls list/array results into individual elements.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#unfold-step">Reference Documentation - Unfold Step</seealso>
    public sealed class UnfoldStep : Step
    {
        public static readonly UnfoldStep Instance = new();
    }
}
