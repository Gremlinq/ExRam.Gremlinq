namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>drop()</c> step that removes elements from the graph.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#drop-step">Reference Documentation - Drop Step</seealso>
    public sealed class DropStep : Step
    {
        public static readonly DropStep Instance = new();

        public DropStep() : base(SideEffectSemanticsChange.Write)
        {
        }
    }
}
