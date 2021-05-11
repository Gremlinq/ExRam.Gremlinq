namespace ExRam.Gremlinq.Core
{
    public sealed class OptionTraversalStep : Step
    {
        public OptionTraversalStep(object? guard, Traversal optionTraversal, QuerySemantics? semantics = default) : base(semantics)
        {
            Guard = guard;
            OptionTraversal = optionTraversal;
        }

        public object? Guard { get; }

        public Traversal OptionTraversal { get; }
    }
}
