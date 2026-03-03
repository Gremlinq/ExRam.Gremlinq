namespace ExRam.Gremlinq.Core.Steps
{
    /// <summary>Represents the Gremlin <c>drop()</c> step that removes elements from the graph.</summary>
    /// <seealso href="https://tinkerpop.apache.org/docs/current/reference/#drop-step">Reference Documentation - Drop Step</seealso>
    public sealed class DropStep : Step
    {
        /// <summary>Gets the singleton instance of <see cref="DropStep"/>.</summary>
        public static readonly DropStep Instance = new();

        /// <summary>Initializes a new instance of <see cref="DropStep"/>.</summary>
        public DropStep() : base(SideEffectSemanticsChange.Write)
        {
        }
    }
}
