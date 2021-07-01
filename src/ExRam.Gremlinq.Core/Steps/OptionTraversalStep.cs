namespace ExRam.Gremlinq.Core.Steps
{
    public sealed class OptionTraversalStep : Step
    {
        public OptionTraversalStep(object? guard, Traversal optionTraversal) : base(optionTraversal.GetTraversalSemanticsChange())
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        public object? Guard { get; }

        public Traversal OptionTraversal { get; }
    }
}
